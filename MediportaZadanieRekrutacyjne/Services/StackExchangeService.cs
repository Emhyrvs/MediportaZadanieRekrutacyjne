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

    public StackExchangeService(IMapper mapper)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api.stackexchange.com");
        _mapper = mapper;
    }

    public async Task<List<TagItem>> GetAllTagsAsync(int page, int pageSize)
    {
        

        try
        {
            string apiUrl = $"https://api.stackexchange.com/2.3/tags?page={page}&pagesize={pageSize}&order=desc&sort=popular&site=stackoverflow";

            // Wykonanie zapytania GET do API Stack Exchange
            HttpResponseMessage response = await _client.GetAsync(apiUrl);

            // Sprawdzenie, czy odpowiedź jest poprawna
            if (response.IsSuccessStatusCode)
            {
                // Sprawdzenie, czy odpowiedź jest zakodowana w gzip
                if (response.Content.Headers.ContentEncoding.Contains("gzip"))
                {
                    using (var gzipStream = new GZipStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress))
                    using (var reader = new StreamReader(gzipStream))
                    {
                        // Odczytanie zawartości odpowiedzi
                        string responseBody = await reader.ReadToEndAsync();

                        // Deserializacja odpowiedzi JSON do obiektu TagResponse
                        TagsDtoRead tags = JsonConvert.DeserializeObject<TagsDtoRead>(responseBody);

                        // Pobranie nazw tagów z obiektu TagResponse

                        return GetTagsFormTagsDto(tags.Items);

                    }
                }
                else // Jeśli odpowiedź nie jest zakodowana w gzip
                {
                    // Odczytanie zawartości odpowiedzi
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserializacja odpowiedzi JSON do obiektu TagResponse
                   // var tagResponse = JsonConvert.DeserializeObject<TagsDtoRead>(responseBody);



                }
            }
            else
            {
                Console.WriteLine("Błąd podczas pobierania tagów: " + response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd: " + ex.Message);
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

