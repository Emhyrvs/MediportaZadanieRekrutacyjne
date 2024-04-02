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
        private readonly ILogger _logger;   


        public TagsController(IStackExchangeService stackExchangeService, ITagRepo tagRepo, ILogger logger)
        {
            _stackExchangeService = stackExchangeService;
           
            _tagRepo = tagRepo;
            _logger = logger;   
            
        }
      

        [HttpGet] 
        public async Task<ActionResult<List<TagItem>>> GetTagsFromDatabase(
           [FromQuery] int pageNumber = 1,
           [FromQuery] int pageSize = 10,
           [FromQuery] string sortBy = "name",
           [FromQuery] string sortOrder = "asc")
        {

            await _tagRepo.UpdateCount();
            
            var tagsFromDatabase = await _tagRepo.GetTagsAsync(pageNumber, pageSize, sortBy, sortOrder);

           
            if (tagsFromDatabase == null || tagsFromDatabase.Count == 0)
            {

                _logger.LogWarning("Bład nie znaleziono w bazie danych");
                return NotFound();
            }

            
            return Ok(tagsFromDatabase);
        }

        [HttpGet("DownloadTags")] 
        public async Task<ActionResult> DoTags()
        {
            await _tagRepo.GetTagsFromService();



          
            return Ok();
        }





    }
}
