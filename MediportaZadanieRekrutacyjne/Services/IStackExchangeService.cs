
using MediportaZadanieRekrutacyjne.Models;
using MediPortaZadanieTestowe.Models;

namespace MediPortaZadanieTestowe.Services
{
    public interface IStackExchangeService
    {
       
        Task<List<TagItem>> GetAllTagsAsync(int page, int Pagesize);
    }
}
