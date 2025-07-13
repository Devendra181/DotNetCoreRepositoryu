using eCommerce.Infrastructure;
using eCommerce.Core;
using eCommerce.API.Middlewares;
using System.Text.Json.Serialization;
using eCommerce.Core.Mappers;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//Add Infrastructure services
builder.Services.AddInfrastructure();
builder.Services.AddCore();

// Add controllers to the service collection
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Fix: Use the correct overload of AddAutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(ApplicationUserMappingProfile).Assembly));

//FluentValidations
builder.Services.AddFluentValidationAutoValidation();

//swagger
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

//Build the web application
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandlingMiddleware();

//Routing
app.UseRouting();

//Auth
app.UseAuthentication();
app.UseAuthorization();

//Controller routes
app.MapControllers();

app.Run();
