using System.Reflection;
using API.Extensions;
using API.Helpers.Errors;
using AspNetCoreRateLimit;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//configuro seriLog
var logger = new LoggerConfiguration()
                                    .ReadFrom.Configuration(builder.Configuration)
                                    .Enrich.FromLogContext()
                                    .CreateLogger();

//builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

//agrego servicio de Automapper
builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

//agrego servicio de Limites de peticiones
builder.Services.ConfigureRateLimitiong();

// Add services to the container.
builder.Services.ConfigureCors(); //invoco la configuracion de los cors desde la clase ApplicationServiceExtensions
builder.Services.AddAplicacionServices();//invoco configuracion inyeccion de dependencias desde la clase ApplicationServiceExtensions
builder.Services.ConfigureApiVersioning(); //invoco la configuracion de versionado desde la clase ApplicationServiceExtensions
builder.Services.AddJwt(builder.Configuration);//invoco la configuracion del Token desde la clase ApplicationServiceExtensions

//configuro Formato XML
builder.Services.AddControllers(optios=>{
    optios.RespectBrowserAcceptHeader = true; 
    optios.ReturnHttpNotAcceptable = true; //configuracion para que envie mensaje de error en caso de que envien un formato no valido
}).AddXmlSerializerFormatters(); 

builder.Services.AddValidationErrors();//invoco la configuracion de la validacion de errores del modelState  desde la clase ApplicationServiceExtensions

//agrego version del servidor  mysql
var serverVersion = new MySqlServerVersion(new Version(8, 0, 32));

//agrego del dbContext
builder.Services.AddDbContext<TiendaContext>(options=>{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),serverVersion);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//invoco el middleware para el manejo de las exepciones
app.UseMiddleware<ExceptionMiddleware>();

//invoco el middleware para generar paginas personalizadas cuando ocurra un error en el servidor
app.UseStatusCodePagesWithReExecute("/errors/{0}");

//uso el middleware para validar el numero de peticiones que voy aceptar
app.UseIpRateLimiting(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Configuro migraciones de Manera automatica
using(var scope = app.Services.CreateScope()){
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try{
        var context = services.GetRequiredService<TiendaContext>();
        await context.Database.MigrateAsync();
        await TiendaContextSeed.SeedAsync(context,loggerFactory);
        await TiendaContextSeed.SeedRolesAsync(context,loggerFactory);
    }
    catch(Exception ex){
        var _logger = loggerFactory.CreateLogger<Program>();
        _logger.LogError(ex,"Ocurrio un error durante la migracion");
    }
}

app.UseCors("CorsPolicy");  //uso las politicas de CORS

app.UseHttpsRedirection();

app.UseAuthentication(); //middleware de autenticacion para JWT
app.UseAuthorization();

app.MapControllers();

app.Run();
