using MediportaZadanieRekrutacyjne.Models;
using MediPortaZadanieTestowe.Models;
using MediPortaZadanieTestowe.Services;
using Newtonsoft.Json;
using System.IO.Compression;

public class StackExchangeService:IStackExchangeService
{
    private readonly HttpClient _client;

    public StackExchangeService()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api.stackexchange.com");
    }

    public async Task<List<TagItemDto>> GetAllTagsAsync()
    {
        

        try
        {
            // Wykonanie zapytania GET do API Stack Exchange
            HttpResponseMessage response = await _client.GetAsync("https://api.stackexchange.com/2.3/tags?pagesize=10&order=desc&sort=popular&site=stackoverflow");

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

                        return tags.Items;

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
}

