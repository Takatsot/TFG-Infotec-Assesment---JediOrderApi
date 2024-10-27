using Core.Models.Domain;
using Infrastructure.Data;
using JediOrderApi.Middleware;
using Microsoft.EntityFrameworkCore;
using TFGInfotecApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.GetServices();
builder.Services.AutoMapper();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();
//app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:", "https://localhost"));
app.MapControllers();
app.MapGroup("api").MapIdentityApi<AppUser>(); // api/login
try
{
    using var scope = app.Services.CreateScope();
    var services    = scope.ServiceProvider;
    var context     = services.GetRequiredService<JediOrderDbContext>();
    await context.Database.MigrateAsync();
    await JediOrderDbContextSeed.SeedDataAsync(context);
}
catch(Exception ex)
{
    Console.WriteLine(ex.ToString());
    throw;
}

app.Run();
