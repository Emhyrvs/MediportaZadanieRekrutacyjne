using MediportaZadanieRekrutacyjne.Data;
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
        private readonly ITagRepo _tagRepo;


        public TagsController(IStackExchangeService stackExchangeService, ITagRepo tagRepo)
        {
            _stackExchangeService = stackExchangeService;
           
            _tagRepo = tagRepo;
            
        }
      

        [HttpGet] // Dodajmy ścieżkę do odróżnienia od poprzedniej metody
        public async Task<ActionResult<List<TagItem>>> GetTagsFromDatabase(
           [FromQuery] int pageNumber = 1,
           [FromQuery] int pageSize = 10,
           [FromQuery] string sortBy = "name",
           [FromQuery] string sortOrder = "asc")
        {

            await _tagRepo.UpdateCount();
            // Pobierz listę tagów z bazy danych z uwzględnieniem stronicowania i sortowania
            var tagsFromDatabase = await _tagRepo.GetTagsAsync(pageNumber, pageSize, sortBy, sortOrder);

            // Jeśli nie ma tagów w bazie danych, zwróć 404 Not Found
            if (tagsFromDatabase == null || tagsFromDatabase.Count == 0)
            {
                return NotFound();
            }

            // Jeśli tagi zostały znalezione, zwróć je jako wynik żądania
            return Ok(tagsFromDatabase);
        }

        [HttpGet("DownloadTags")] // Dodajmy ścieżkę do odróżnienia od poprzedniej metody
        public async Task<ActionResult> DoTags()
        {
            // Pobierz listę tagów z bazy danych
            await _tagRepo.GetTagsFromService();



            // Jeśli tagi zostały znalezione, zwróć je jako wynik żądania
            return Ok();
        }





    }
}
