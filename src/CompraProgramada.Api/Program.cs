using CompraProgramada.Api.Middlewares;
using CompraProgramada.Infra.Data.Context;
using CompraProgramada.Infra.Data.Seeds;
using CompraProgramada.Infra.IoC;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

await DatabaseSeed.SeedAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseRouting();

app.UseMiddleware<ValidationExceptionMiddleware>();

app.UseCors();

app.MapControllers();

app.Map("/", () => Results.Redirect("/swagger"));

app.Run();
