using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NzWalksDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString")));

// You can change which concrete implementation is used here, allowing you to change data sources
builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();

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