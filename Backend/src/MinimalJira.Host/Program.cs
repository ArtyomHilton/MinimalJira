using FluentValidation;
using MinimalJira.Application.Extensions;
using MinimalJira.Host.Middleware;
using MinimalJira.Host.Validators;
using MinimalJira.Infrastructure.Extensions;
using MinimalJira.Persistence.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddUseCases();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddDistributedCache(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<AddProjectRequestValidator>();
builder.Services.AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler(e => { });

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = String.Empty;
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
