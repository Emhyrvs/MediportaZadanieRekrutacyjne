using MediportaZadanieRekrutacyjne.Data;
using MediPortaZadanieTestowe.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlatformService.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder
                .AllowAnyOrigin()   // Umożliwia żądania z dowolnej domeny
                .AllowAnyMethod()   // Umożliwia użycie dowolnej metody HTTP (GET, POST, itd.)
                .AllowAnyHeader();  // Umożliwia użycie dowolnych nagłówków w żądaniach
        });
});
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



app.UseHttpsRedirection();


app.UseCors("AllowAllOrigins");
app.MapControllers();
await PrepDbcs.PrepPopulation(app,true);
app.Run();
