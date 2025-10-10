using Amazon.S3;
using Customer.API.ActionFilters;
using Customer.API.Middlewares;
using Customer.Application;
using Customer.Application.Abstractions;
using Customer.Application.Services;
using Customer.Application.Validators;
using Customer.Infrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddValidatorsFromAssemblyContaining<AddCustomerDTOValidator>();
builder.Services.AddDbContext<ApplicationDbContext>(dbContextBuilder=>
    dbContextBuilder.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddControllers(opts =>
{
    opts.Filters.Add<ValidationActionFilter>();
});
builder.Services.AddScoped<LocalizationService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// ამას ინტერფეისი ხომ არ გავუკეთო?
builder.Services.AddScoped<CustomerService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}



app.UseMiddleware<LocalizationMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();


app.Run();

