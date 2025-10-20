using Khaoticen.CookBook.Api.Api.Middleware;
using Khaoticen.CookBook.Api.Core.Services.Entity;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Khaoticen.CookBook.Api.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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

#region DEBUG // todo: remove
Console.WriteLine("##########");

app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("########## Routes ##########");
    var endpointSources = app.Services.GetRequiredService<IEnumerable<EndpointDataSource>>();

    foreach (var endpoint in endpointSources.SelectMany(es => es.Endpoints).OfType<RouteEndpoint>())
    {
        var methods = endpoint.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault()?.HttpMethods;
        Console.WriteLine($"Route: {endpoint.RoutePattern.RawText}, Methods: {string.Join(", ", methods ?? new List<string>())}");
    }
});

#endregion