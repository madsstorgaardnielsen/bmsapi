using System.Text;
using AspNetCoreRateLimit;
using BMSAPI.Database;
using BMSAPI.Database.Models;
using BMSAPI.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace BMSAPI.ApiConfig;

public static class ApiConfiguration {
    public static void ConfigureRateLimit(this IServiceCollection services) {
        var rules = new List<RateLimitRule> {
            new() {
                Endpoint = "*",
                Limit = 10,
                Period = "1s"
            }
        };
        services.Configure<IpRateLimitOptions>(options => { options.GeneralRules = rules; });
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    }

    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration) {
        var jwtSettings = configuration
            .GetSection("JwtToken");

        //if in a real world app, the key shouldnt be set in appsettings
        services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.GetSection("ValidIssuer").Value,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("IssuerSigningKey").Value)),
                ValidateAudience = false,
                RequireExpirationTime = true,
            };
        });
    }

    public static void ConfigureIdentity(this IServiceCollection services) {
        var builder = services
            .AddIdentityCore<User>(options => { options.User.RequireUniqueEmail = true; });

        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
        builder.AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();
    }

    public static void ConfigureHttpCacheHeaders(this IServiceCollection services) {
        services.AddResponseCaching();
        services.AddHttpCacheHeaders(
            expirationOptions => {
                expirationOptions.MaxAge = 120;
                expirationOptions.CacheLocation = CacheLocation.Private;
            },
            (validationOptions) => { validationOptions.MustRevalidate = true; }
        );
    }

    public static void ConfigureExceptionHandler(this IApplicationBuilder app) {
        app.UseExceptionHandler(error => {
            error.Run(async context => {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null) {
                    Log.Error($"Error in {contextFeature.Error}");
                    await context.Response.WriteAsync(new ErrorDTO {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal server error"
                    }.ToString());
                }
            });
        });
    }


    public static void ConfigureCors(this IServiceCollection services) {
        services.AddCors(options => {
            options.AddPolicy("CorsPolicyAllowAll",
                policy => {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
    }

    public static void ConfigureApiVersioning(this IServiceCollection services) {
        services.AddApiVersioning(options => {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ApiVersionReader = new HeaderApiVersionReader("api-version");
        });
    }
}