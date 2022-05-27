using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RentingService;
using RentingService.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RentingServiceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RentingServiceContext") ?? throw new InvalidOperationException("Connection string 'RentingServiceContext' not found.")));

builder.Services.AddHostedService<ClientDetailsReceiver>();
builder.Services.AddHostedService<ProductDetailsReceiver>();

// Add services to the container.

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
