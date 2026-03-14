using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Npgsql.Internal.Postgres;
using Scalar.AspNetCore;
using ZedLive.Domain.ValueObjects;
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

        #region Scalar

        services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });

        #endregion

        #region ModelState

        services.Configure<ApiBehaviorOptions>(opt =>
        {
            opt.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new
                    {
                        Field = x.Key,
                        Error = x.Value?.Errors.First().ErrorMessage
                    });
                return new BadRequestObjectResult(new
                {
                    Message = "Validation erros",
                    Error = errors
                });
            };
        });

        #endregion

        #region Options

        services.Configure<StreamOptions>(_configuration.GetSection("Stream"));
        services.Configure<JwtOptions>(_configuration.GetSection("Jwt"));
        services.Configure<ConnectionStringsOptions>(_configuration.GetSection("DbConnection"));

        #endregion

        #region Dependencies

        services.DependencyInjection(_configuration);

        #endregion Dependencies

        #region Configuration Authentication & Authorization

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtOptions>>((opt, jwtOptions) =>
            {
                var jwt = jwtOptions.Value;

                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;

                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwt.SecretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwt.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwt.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1),
                    NameClaimType = ClaimTypes.NameIdentifier
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

        // app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(ep =>
        {
            ep.MapControllers();
            ep.MapOpenApi();
            ep.MapScalarApiReference(opt =>
            {
                opt.WithTitle("ZedLive")
                    .WithTheme(ScalarTheme.Purple);
            });
        });
    }

    internal sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
    {
        private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;

        public BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider)
        {
            _authenticationSchemeProvider = authenticationSchemeProvider;
        }

        public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context,
            CancellationToken cancellationToken)
        {
            var authenticationSchemes = await _authenticationSchemeProvider.GetAllSchemesAsync();
            if (!authenticationSchemes.Any(a => a.Name == "Bearer"))
                return;

            var bearerScheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            };

            document.Components ??= new OpenApiComponents();

            document.AddComponent("Bearer", bearerScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            };

            foreach (var path in document.Paths.Values)
            {
                foreach (var operation in path.Operations.Values)
                {
                    operation.Security ??= new List<OpenApiSecurityRequirement>();
                    operation.Security.Add(securityRequirement);
                }
            }
        }
    }
}