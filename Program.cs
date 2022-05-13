using AspNetCoreRateLimit;
using BMSAPI.ApiConfig;
using BMSAPI.Database;
using BMSAPI.Exceptions;
using BMSAPI.Repositories;
using BMSAPI.Services;
using BMSAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options => { options.EnableSensitiveDataLogging(); });
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JWTService>();
builder.Services.AddScoped<ChildService>();
builder.Services.AddScoped<DiaperService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ChildRepository>();
builder.Services.AddScoped<DiaperRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimit();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureCors();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddAutoMapper(typeof(ObjectMapper));

var logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .Enrich
    .FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services
    .AddControllers(config => {
        config
            .CacheProfiles
            .Add("duration120sec", new CacheProfile {Duration = 120});
    })
    .AddNewtonsoftJson(options => {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
// app.ConfigureExceptionHandler();
app.UseCors("CorsPolicyAllowAll");
app.UseResponseCaching();
app.UseHttpCacheHeaders();
app.UseIpRateLimiting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

try {
    logger.Information("\n");
    logger.Information("BMSAPI starting");
    app.Run();
}
catch (Exception e) {
    logger.Fatal(e, "BMSAPI failed to start");
}
finally {
    logger.Information("disposing logger");
    logger.Dispose();
}