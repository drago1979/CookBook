using Khaoticen.CookBook.Api.Core.Services;
using Khaoticen.CookBook.Api.Core.Services.Entity;
using Khaoticen.CookBook.Api.Infrastructure;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Khaoticen.CookBook.Api.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

#region DB
// builder.Services.AddDbContext<AppDbContext>(options =>
    // options.UseSqlite(builder.Configuration.GetConnectionString("DbConnectionString")));

    builder.Services.AddDbContext<AppDbContext>((sp, options) =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("DbConnectionString"));

        // Interceptors
        options.AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
        options.AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>());
    });

// todo: prebaci u skriptu - debugging
// Console.WriteLine("############# ");
// Console.WriteLine(builder.Configuration.GetConnectionString("DbConnectionString"));
#endregion

#region Autowiring // todo: note - order important: before services; 
builder.Services.Scan(scan => scan
    .FromAssemblyOf<RecipeService>()
    // Factories first
    .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Factory")))
    .AsSelfWithInterfaces()
    .WithScopedLifetime()
    // Then services
    .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Service")))
    .AsSelfWithInterfaces()
    .WithScopedLifetime()
);
#endregion

#region Services
builder.Services.AddAutoMapper(typeof(Program));

// todo: remove
// builder.Services.AddScoped<IRecipeService, RecipeService>();

builder.Services.AddScoped<AuditInterceptor>();
builder.Services.AddSingleton<SoftDeleteInterceptor>();
#endregion

var app = builder.Build();

// HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();