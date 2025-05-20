using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RestApiExample.Data;
using RestApiExample.Services;
using RestApiExample.Repositories;
using AutoMapper;
using RestApiExample.Mapping;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure Services
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactDevPolicy", policy =>
        policy
          .WithOrigins("http://localhost:3000")
          .AllowAnyHeader()
          .AllowAnyMethod()
    );
});

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("H2InMemory"));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// AutoMapper profiles
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = " Agricura Product API", Version = "v1" });
});

// 2. Build app
var app = builder.Build();

// 3. Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors("ReactDevPolicy");

app.UseAuthorization();

app.MapControllers();

// 4. Run
app.Run();
