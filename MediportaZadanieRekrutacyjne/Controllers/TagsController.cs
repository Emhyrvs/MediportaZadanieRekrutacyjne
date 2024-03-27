using MediportaZadanieRekrutacyjne.Models;
using MediPortaZadanieTestowe.Models;
using MediPortaZadanieTestowe.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MediPortaZadanieTestowe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {

        private readonly IStackExchangeService _stackExchangeService;
        private readonly DbContext _dbContext;


        public TagsController(IStackExchangeService stackExchangeService,DbContext dbContext)
        {
            _stackExchangeService = stackExchangeService;
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<List<TagItemDto>>> GetTagsFromApi()
        {

            var Tags = await _stackExchangeService.GetAllTagsAsync();



            return Tags;

        }

        [HttpGet("from-database")] // Dodajmy ścieżkę do odróżnienia od poprzedniej metody
        public async Task<ActionResult<List<TagItem>>> GetTagsFromDatabase()
        {
            // Pobierz listę tagów z bazy danych
            var tagsFromDatabase = await _dbContext.Set<TagItem>().ToListAsync();

            // Jeśli nie ma tagów w bazie danych, zwróć 404 Not Found
            if (tagsFromDatabase == null || tagsFromDatabase.Count == 0)
            {
                return NotFound();
            }

            // Jeśli tagi zostały znalezione, zwróć je jako wynik żądania
            return Ok(tagsFromDatabase);
        }





    }
}
