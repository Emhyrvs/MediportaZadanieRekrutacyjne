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
                _context.Tags.RemoveRange(_context.Tags); 
                await _context.SaveChangesAsync(); 
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
            
            var query = _context.Tags.AsQueryable();

            switch (sortBy)
            {
                case "name":
                    query = sortOrder == "asc" ? query.OrderBy(t => t.Name) : query.OrderByDescending(t => t.Name);
                    break;
                case "share":
                    query = sortOrder == "asc" ? query.OrderBy(t => t.Share) : query.OrderByDescending(t => t.Share);
                    break;
               
                default:
                    query = query.OrderBy(t => t.Name);
                    break;
            }

            var tags = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return tags;
        }
    


    public async Task UpdateCount()
            {


            List<TagItem> tags = await _context.Tags.ToListAsync();
            if (tags.Any(a => a.Share <= 0) && tags.All(a=>a.Count<=0))
            {
                tags = CountShareInPopulation(tags);


                _context.UpdateRange(tags);
                await _context.SaveChangesAsync();
            }
            else if(tags.Any(a => a.Count <= 0))
            {
                throw new ArgumentException();
            }


            
        }

        private List<TagItem> GetTagsFormTagsDto(List<TagItemDto> tagItemDtos)

        {
            List<TagItem> tags = tagItemDtos.Select(tagDto => _mapper.Map<TagItem>(tagDto)).ToList();

            //List<TagItem> tags2 = tags.Select(tag => tag.PoleCount = tag.Count *)

            return tags;
        }


        private List<TagItem> CountShareInPopulation(List<TagItem> tags)
        {
            
            int total = tags.Sum(tag => tag.Count);

          
            List<TagItem> tags2 = tags.Select(tag =>
            {
                tag.Share = Math.Round((decimal)tag.Count * 100.0m / (decimal)total, 2);
                return tag;
            }).ToList();


            return tags2;
        }
    }

}
