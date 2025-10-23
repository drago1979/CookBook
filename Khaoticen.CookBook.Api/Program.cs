using System.Text.Json.Serialization;
using Khaoticen.CookBook.Api.Api.Middleware;
using Khaoticen.CookBook.Api.Core.Services.Entity;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Khaoticen.CookBook.Api.Infrastructure.Interceptors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Init version
// builder.Services.AddControllers();

// Changed: Suppress circular refs.in responses // todo!!: confirm circ.refs in responeses sit.
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);



builder.Services.AddOpenApi();

#region DB

    builder.Services.AddDbContext<AppDbContext>((sp, options) =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("DbConnectionString"));

        // Interceptors
        options.AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
    });

#endregion

#region Autowiring
builder.Services.Scan(scan => scan
    .FromAssemblyOf<RecipeService>()
    
    // Factories
    .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Factory")))
    .AsSelfWithInterfaces()
    .WithScopedLifetime()
    
    // Services
    .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Service")))
    .AsSelfWithInterfaces()
    .WithScopedLifetime()
    
    // Repositories
    .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Repository")))
    .AsSelfWithInterfaces()
    .WithScopedLifetime()
);
#endregion

#region Services
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<AuditInterceptor>();
#endregion

#region Validation-error-customization
#region Customize validation error exceptions

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );
        
        var remappedErrors = errors.ToDictionary(
            kvp => kvp.Key switch
            {
                "$" => "json",
                "requestDto" => "requestDto",
                _ => kvp.Key
            },
            kvp => kvp.Value
        );
        
        var result = new
        {
            status = 422,
            message = "Validation failed",
            errors = remappedErrors
        };

        return new ObjectResult(result)
        {
            StatusCode = 422
        };
    };
});

// builder.Services.Configure<JsonOptions>(options =>
// {
//     options.JsonSerializerOptions.MaxDepth = 1;
//     options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
// });


#endregion


#endregion

var app = builder.Build();

// HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();

