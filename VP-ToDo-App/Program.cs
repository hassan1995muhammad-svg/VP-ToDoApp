using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using VP_ToDo_App.Application.Mappings;
using VP_ToDo_App.Infrastructure.Data;
using VP_ToDo_App.Infrastructure.Repositories;
using VP_ToDo_App.Middleware;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/todo-app-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting VP-ToDo-App");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog();

    // Add services to the container
    builder.Services.AddControllers();

    // Add DbContext with InMemory Database
    builder.Services.AddDbContext<TodoDbContext>(options =>
        options.UseInMemoryDatabase("VPToDoDb"));

    // Add Repository Pattern
    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();

    // Add MediatR
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

    // Add AutoMapper
    builder.Services.AddAutoMapper(typeof(MappingProfile));

    // Add FluentValidation
    builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

    // Add CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "VP ToDo API",
            Version = "v1",
            Description = "A professional ToDo API built with Clean Architecture, CQRS, and MediatR. Using InMemory Database for simplified deployment."
        });
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "VP ToDo API V1");
        });
    }

    app.UseCors("AllowAll");

    app.UseAuthorization();

    app.MapControllers();

    // Ensure database is created (InMemory database is created automatically)
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
        dbContext.Database.EnsureCreated();
        Log.Information("InMemory database initialized");
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
