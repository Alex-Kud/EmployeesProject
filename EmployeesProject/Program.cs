using EmployeesProject.Data;
using EmployeesProject.Migrations;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<Database>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Employee",
        Description = "Тестовое задание Smartway",
        Contact = new OpenApiContact()
        {
            Name = "Александр Кудашов",
            Email = "sasha.kudaschov2014@yandex.ru"
        },
        License = new OpenApiLicense
        {
            Name = "Александр Кудашов",
            Url = new Uri("https://vk.com/alex_kudashov")
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
