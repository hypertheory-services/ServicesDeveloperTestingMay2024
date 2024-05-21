using FluentValidation;
using Microsoft.FeatureManagement;
using ReferenceApi.Employees;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IGenerateSlugsForNewEmployees, EmployeeSlugGeneratorWithUniqueIds>();
// Add services to the container.
builder.Services.AddFeatureManagement(); // In Configration, look for "FeatureManagement"
builder.Services.AddValidatorsFromAssemblyContaining<EmployeeCreateRequestValidator>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program { }