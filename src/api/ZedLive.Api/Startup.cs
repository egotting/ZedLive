using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ZedLive.Domain.ValueObjects.StructType;
using ZedLive.IoC;

namespace ZedLive.Api;

internal sealed class Startup
{
    private readonly IConfiguration _configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddUserSecrets<Startup>()
        .Build();
    // private readonly ILogger<Startup> _logger;
    // private const string connectionString = "DbConnection";

    // _logger.LogInformation("connection string: " + _configuration.GetConnectionString(connectionString));

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        #region Options
        services.AddOptions<StreamOptions>()
            .BindConfiguration("Stream");
        services.AddOptions<JwtOptions>()
            .BindConfiguration("Jwt");
        services.AddOptions<ConnectionStringsOptions>()
            .BindConfiguration("DbConnection");
        #endregion

        #region Dependencies
        services.DependencyInjection(_configuration);
        #endregion Dependencies

        #region Configuration Authentication & Authorization
        services.AddOptions<JwtOptions>()
            .BindConfiguration("Jwt");

        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                var jwtSettings = _configuration.GetSection("Jwt").Get<JwtOptions>();
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

    public void Configure(IApplicationBuilder app)
    {
        app.UseCors(builder => { builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}