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

namespace PlatformService.Data
{
    public static class PrepDbcs
    {
        public static async Task PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var mapper = serviceScope.ServiceProvider.GetRequiredService<IMapper>();

                await SeedData(serviceScope.ServiceProvider.GetRequiredService<DataDbContext>(), isProd, serviceScope.ServiceProvider.GetRequiredService<IStackExchangeService>(),mapper);
            }
        }

        private static async Task SeedData(DataDbContext context, bool isProd, IStackExchangeService stackExchangeService,IMapper mapper)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    await context.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }

            if (!context.Tags.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                do
                {
                    List<TagItemDto> tagsDto = await stackExchangeService.GetAllTagsAsync();
                    List<TagItem> tags = tagsDto.Select(tagDto => mapper.Map<TagItem>(tagDto)).ToList();

                   
                    context.Tags.AddRange(tags);
                    await context.SaveChangesAsync();
                } while (context.Tags.Count() < 1000);
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}