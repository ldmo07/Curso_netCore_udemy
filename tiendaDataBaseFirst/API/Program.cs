using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Agrego el contexto de bd
builder.Services.AddDbContext<TiendaContext>(options=>{
    var versionServidor = ServerVersion.Parse("8.0.32-mysql");
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), versionServidor);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//habilito las migraciones databaseFirst
using(var scope = app.Services.CreateScope()){

    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<TiendaContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        
       var logger = loggerFactory.CreateLogger<Program>();
       logger.LogError(ex,"Ocurrio un error durante la migracion");
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
