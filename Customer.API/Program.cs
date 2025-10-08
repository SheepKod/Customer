using Customer.API.Middlewares;
using Customer.Application;
using Customer.Application.Abstractions;
using Customer.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(dbContextBuilder=>
    dbContextBuilder.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddControllers();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<CustomerService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.MapControllers();
app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();


app.Run();

