using System.Text;
using API.Helpers;
using API.Helpers.Errors;
using API.Services;
using AspNetCoreRateLimit;
using Core.Entities;
using Core.Interfaces;
using Infraestructure.Repositories;
using Infraestructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        //metodo que maneja los CORS
        public static void ConfigureCors(this IServiceCollection services) => 
        services.AddCors(option =>{
            option.AddPolicy("CorsPolicy", builder => 
                builder.AllowAnyOrigin() //WithOrigins("miDominio")
                .AllowAnyMethod()   //WithMethods("GET","POST")
                .AllowAnyHeader()  //WithHeaders("accept","content-type")
            );
        });

        //metodo que maneja la inyeccion de dependencias 
        public static void AddAplicacionServices(this IServiceCollection services){
            // services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            // services.AddScoped<IProductoRepository,ProductoRepository>();
            // services.AddScoped<IMarcaRepository,MarcaRepository>();
            // services.AddScoped<ICategoriaRepository,CategoriaRepository>();
            services.AddScoped<IPasswordHasher<Usuario>,PasswordHasher<Usuario>>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
        }

        //metodo que configura el numero de Peticiones a nuestro API
        public static void ConfigureRateLimitiong(this IServiceCollection services){
            services.AddMemoryCache();
            services.AddSingleton<IRateLimitConfiguration,RateLimitConfiguration>();
            services.AddInMemoryRateLimiting();

            services.Configure<IpRateLimitOptions>(options=>{
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode=429;
                options.RealIpHeader ="X-Real-IP";
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Period = "10s",
                        Limit =  2
                    }
                };
                
            });
        }

        //metodo que configura las versiones de mi API
        public static void ConfigureApiVersioning(this IServiceCollection services){
            services.AddApiVersioning(options=>{
                options.DefaultApiVersion = new ApiVersion(1,0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                //options.ApiVersionReader = new QueryStringApiVersionReader("ver"); //versionado por QueryParam
                //options.ApiVersionReader = new HeaderApiVersionReader("X-Version");//versionado por Header
                
                //convino los dos metodos de versioando de APiS
                options.ApiVersionReader =  ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("ver"),
                    new HeaderApiVersionReader("X-Version")
                );
                options.ReportApiVersions = true;
            });
        }

        //metodod que configura el token
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration){

            services.Configure<JWT>(configuration.GetSection("JWT"));

            services.AddAuthentication(options =>{
                 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o=>{
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JWT:Issuer"],
                        ValidAudience = configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                        
                    };
                });
        }

        //metodo para maenejo de errores en el modelState
        public static void AddValidationErrors(this IServiceCollection services){
            services.Configure<ApiBehaviorOptions>(options =>{
                options.InvalidModelStateResponseFactory = actionContext =>{

                    var errors = actionContext.ModelState.Where(u=>u.Value.Errors.Count >0 )
                                .SelectMany(u=>u.Value.Errors)
                                .Select(u=>u.ErrorMessage).ToArray();

                    var errorResponse  = new ApiValidation(){
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });
        }
    }
}