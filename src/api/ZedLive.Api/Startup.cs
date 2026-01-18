using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ZedLive.Api.Configuration.Jwt;
using ZedLive.Application.Services.Stream;
using ZedLive.Domain.Contracts.Users.Configuration;
using ILogger = Serilog.ILogger;
using Stream = ZedLive.Api.Configuration.Jwt.Stream;

namespace ZedLive.Api;

internal class Startup
{
    // todo: Configurar startup
    private readonly IConfiguration _configuration;
    // private static ILogger _logger;

    public Startup()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Startup>()
            .Build();
        // _logger.Information("connection string: " + _configuration.GetConnectionString(connectionString));
    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

    #region Db Configuration

        

    #endregion Db Configuration
        
    #region Repositories

    #endregion

    #region Services

        services.AddOptions<Stream>()
            .BindConfiguration("Stream");
        services.AddScoped<IStreamKey, StreamKey>();

    #endregion


    #region Configuration Authentication & Authorization

        services.AddOptions<Jwt>()
            .BindConfiguration("Jwt");

        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                var jwtSettings = _configuration.GetSection("Jwt").Get<Jwt>();
                opt.RequireHttpsMetadata = true;
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
                opt.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetService<ILogger<Startup>>();
                        logger?.LogWarning("JWT Authentication failed: {exception}", context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetService<ILogger<Startup>>();
                        logger?.LogWarning("JWT Token validated for user: {User}", context.Principal?.Identity?.Name);
                        return Task.CompletedTask;
                    }
                };
            });
        services.AddAuthorization();

    #endregion Configuration Authentication & Authorization
    }

    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
    {
        app.UseCors(builder => { builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}