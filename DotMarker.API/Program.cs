using DotMarker.API.Middlewares;
using DotMarker.Application.Interfaces;
using DotMarker.Application.Mapping;
using DotMarker.Application.Services;
using DotMarker.Infrastructure.Caching;
using DotMarker.Infrastructure.Data;
using DotMarker.Repositories.DI;
using DotMarker.Repositories.Interfaces;
using DotMarker.Repositories.UOW;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
var configuration = builder.Configuration;

// Configure services
ConfigureServices(builder.Services, configuration);

// Configure logging
ConfigureLogging(builder.Logging);

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline
ConfigureMiddleware(app, app.Environment);

app.Run();
return;

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Register DbContext with In-Memory Database
    services.AddDbContext<DotMarkerDatabaseContext>(options =>
        options.UseInMemoryDatabase("CMSDatabase"));

    // Register IMemoryCache
    services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = configuration.GetConnectionString("Redis");
        options.InstanceName = "DotMarker_";
    });

    // Register Mapster IMapper
    DotMarkerMapper.RegisterMappings();
    services.AddScoped<IMapper, Mapper>();

    services.AddRepositories();

    // Dependency Injection
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IContentService, ContentService>();
    services.AddScoped<ICacheManager, CacheManager>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();

    // Add controllers
    services.AddControllers();

    // Configure Swagger
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "CMS API",
            Version = "v1",
            Description = "Content Management System API for managing users and contents.",
            Contact = new OpenApiContact
            {
                Name = "Erdem Camlioglu",
                Email = "secamlioglu@gmail.com"
            }
        });

        // Include XML comments if the file exists
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }
    });
}

void ConfigureLogging(ILoggingBuilder logging)
{
    logging.ClearProviders();
    logging.AddConsole();
}

void ConfigureMiddleware(WebApplication app, IWebHostEnvironment env)
{
    // Enable Swagger middleware
    if (env.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "CMS API v1");
            c.RoutePrefix = string.Empty;
        });
    }

    app.UseMiddleware<LoggingMiddleware>();
    app.UseAuthorization();
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.MapGet("/", () => "Welcome to the CMS API!");

    app.MapControllers();
}