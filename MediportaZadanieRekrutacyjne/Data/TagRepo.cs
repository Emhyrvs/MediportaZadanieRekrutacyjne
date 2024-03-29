using AutoMapper;
using MediportaZadanieRekrutacyjne.Models;
using MediPortaZadanieTestowe.Models;
using MediPortaZadanieTestowe.Services;
using Microsoft.EntityFrameworkCore;
using YamlDotNet.Core.Tokens;

namespace MediportaZadanieRekrutacyjne.Data
{
    public class TagRepo:ITagRepo
    {

        private readonly DataDbContext _context;
        private readonly IStackExchangeService _stackExchangeService;
        private readonly IMapper _mapper;

        public TagRepo(DataDbContext context, IStackExchangeService stackExchangeService,IMapper mapper)
        {
            _context = context;
            _stackExchangeService = stackExchangeService;   
            _mapper = mapper;
        }


        public async Task CreateTag(TagItem tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            await _context.Tags.AddAsync(tag);


            await _context.SaveChangesAsync();



        }

        public async Task<List<TagItem>> GetTagsAsync()
        {
            return  await _context.Tags.ToListAsync();
        }

        public  async Task GetTagsFromService()
        {

            if (_context.Tags.Any())
            {
                _context.Tags.RemoveRange(_context.Tags); // Usuwa wszystkie rekordy z tabeli Tags
                await _context.SaveChangesAsync(); // Zapisuje zmiany w bazie danych
            }

            int i = 0;
            while (_context.Tags.Count()<1000)
            {
                i++;
                List<TagItem> tagsDto = await _stackExchangeService.GetAllTagsAsync(i, 100);



                _context.Tags.AddRange(tagsDto);
                await _context.SaveChangesAsync();


            }
             

        }
        public async Task<List<TagItem>> GetTagsAsync(int pageNumber, int pageSize, string sortBy, string sortOrder)
        {
            // Logika pobierania tagów z bazy danych wraz ze stronicowaniem i sortowaniem
            // Użyj LINQ lub metod zapytań Entity Framework, aby wykonać operacje na bazie danych

            // Przykładowe zapytanie LINQ
            var query = _context.Tags.AsQueryable();

            switch (sortBy)
            {
                case "name":
                    query = sortOrder == "asc" ? query.OrderBy(t => t.Name) : query.OrderByDescending(t => t.Name);
                    break;
                case "share":
                    query = sortOrder == "asc" ? query.OrderBy(t => t.Share) : query.OrderByDescending(t => t.Share);
                    break;
                // Dodaj więcej przypadków sortowania według potrzeb
                default:
                    query = query.OrderBy(t => t.Name); // Domyślne sortowanie
                    break;
            }

            var tags = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return tags;
        }
    


    public async Task UpdateCount()
            {


            List<TagItem> tags = await _context.Tags.ToListAsync();
            if (tags.Any(a => a.Share <= 0))
            {
                tags = CountPrecentInPopulation(tags);


                _context.UpdateRange(tags);
                await _context.SaveChangesAsync();
            }


            
        }

        private List<TagItem> GetTagsFormTagsDto(List<TagItemDto> tagItemDtos)

        {
            List<TagItem> tags = tagItemDtos.Select(tagDto => _mapper.Map<TagItem>(tagDto)).ToList();

            //List<TagItem> tags2 = tags.Select(tag => tag.PoleCount = tag.Count *)

            return tags;
        }


        private List<TagItem> CountPrecentInPopulation(List<TagItem> tags)
        {
            // Obliczanie sumy wszystkich wartości Count w tagach
            int total = tags.Sum(tag => tag.Count);

            // Aktualizacja tagów, aby zawierały procentowy udział
            List<TagItem> tags2 = tags.Select(tag =>
            {
                tag.Share = Math.Round((decimal)tag.Count * 100.0m / (decimal)total, 2);
                return tag;
            }).ToList();


            return tags2;
        }
    }

}
