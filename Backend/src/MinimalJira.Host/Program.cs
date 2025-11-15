using MinimalJira.Application.Extensions;
using MinimalJira.Infrastructure.Extensions;
using MinimalJira.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddUseCases();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddDistributedCache(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = String.Empty;
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
