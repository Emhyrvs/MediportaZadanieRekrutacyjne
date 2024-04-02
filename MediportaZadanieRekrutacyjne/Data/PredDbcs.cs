using System;
using System.Collections.Generic; // Dodaj brakującą przestrzeń nazw
using System.Linq;
using System.Threading.Tasks; // Dodaj brakującą przestrzeń nazw
using AutoMapper;
using MediportaZadanieRekrutacyjne.Data;
using MediportaZadanieRekrutacyjne.Models;
using MediPortaZadanieTestowe.Models;
using MediPortaZadanieTestowe.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace PlatformService.Data
{
    public static class PrepDbcs
    {
        public static async Task PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var mapper = serviceScope.ServiceProvider.GetRequiredService<IMapper>();
                var logger = serviceScope.ServiceProvider.GetRequiredService<Serilog.ILogger>();

                await SeedData(serviceScope.ServiceProvider.GetRequiredService<DataDbContext>(), serviceScope.ServiceProvider.GetRequiredService<ITagRepo>(),mapper,logger);
            }
        }

        private static async Task SeedData(DataDbContext context,ITagRepo tagRepo,IMapper mapper, Serilog.ILogger logger)
        {

            if (context.Tags.Count() < 1000)
            {
                logger.Information("Dodano przykładowe dane do bazy danych.");
                await tagRepo.GetTagsFromService();
            }
            else
            {
                logger.Warning("Mamy już dane w bazie danych.");

            }
        }
    }
}