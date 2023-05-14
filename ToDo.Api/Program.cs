using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using ToDo.Api;
using ToDo.Api.Extensions;
using ToDo.Core;
using ToDo.Data;

var builder = WebApplication.CreateBuilder(args);



// Configure the database
//var connectionString = builder.Configuration.GetConnectionString("Todos");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));




var serviceCollection = builder.Services;
serviceCollection.AddLogging();
var serviceProvider = serviceCollection.BuildServiceProvider();
var _logger = serviceProvider.GetService<ILogger<Program>>();


builder.Services.ConfigureEntityFramework(builder.Configuration, _logger);
builder.Services.ConfigureIdentity(_logger);
builder.Services.ConfigureJwtAuthentication(builder.Configuration, _logger);
builder.Services.ConfigureRepository(_logger);
builder.Services.ConfigureStorage(builder.Configuration, _logger);
builder.Services.ConfigureAutoMapper(_logger);
builder.Services.ConfigureSwagger(_logger);
builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.InitializeAsync(services).Wait();

}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


