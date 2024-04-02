using MediportaZadanieRekrutacyjne.Data;
using MediPortaZadanieTestowe.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlatformService.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IStackExchangeService, StackExchangeService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<DataDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ITagRepo, TagRepo>();
Log.Logger = new LoggerConfiguration()
          .WriteTo.Console()
          .CreateLogger();

// Rejestrujemy Serilog ILogger jako us?ug? w kontenerze DI
builder.Services.AddSingleton(Log.Logger);
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
await PrepDbcs.PrepPopulation(app);
app.Run();
