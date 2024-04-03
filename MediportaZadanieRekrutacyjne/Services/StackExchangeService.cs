using AutoMapper;
using MediportaZadanieRekrutacyjne.Models;
using MediPortaZadanieTestowe.Models;
using MediPortaZadanieTestowe.Services;
using Newtonsoft.Json;
using System.IO.Compression;



public class StackExchangeService:IStackExchangeService
{
    private readonly HttpClient _client;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;   

    public StackExchangeService(IMapper mapper, Serilog.ILogger logger)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api.stackexchange.com");
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<TagItem>> GetAllTagsAsync(int page, int pageSize)
    {
        

        try
        {
            string apiUrl = $"https://api.stackexchange.com/2.3/tags?page={page}&pagesize={pageSize}&order=desc&sort=popular&site=stackoverflow";

           
            HttpResponseMessage response = await _client.GetAsync(apiUrl);

          
            if (response.IsSuccessStatusCode)
            {
               
                if (response.Content.Headers.ContentEncoding.Contains("gzip"))
                {
                    using (var gzipStream = new GZipStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress))
                    using (var reader = new StreamReader(gzipStream))
                    {
                        
                        string responseBody = await reader.ReadToEndAsync();

                      
                        TagsDtoRead tags = JsonConvert.DeserializeObject<TagsDtoRead>(responseBody);

                       

                        return GetTagsFormTagsDto(tags.Items);

                    }
                }
                else 
                {
                    
                    string responseBody = await response.Content.ReadAsStringAsync();

                   



                }
            }
            else
            {
                _logger.Error("Błąd podczas pobierania tagów: " + response.StatusCode);
            }
        }
        catch (Exception ex)
        {
           _logger.Error("Błąd: " + ex.Message);
        }

        return null;
    }

    private List<TagItem> GetTagsFormTagsDto(List<TagItemDto> tagItemDtos)

    {
        List<TagItem> tags = tagItemDtos.Select(tagDto => _mapper.Map<TagItem>(tagDto)).ToList();

        //List<TagItem> tags2 = tags.Select(tag => tag.PoleCount = tag.Count *)

        return tags;
    }


    
}

