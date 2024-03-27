
using MediportaZadanieRekrutacyjne.Models;
using MediPortaZadanieTestowe.Models;

namespace MediPortaZadanieTestowe.Services
{
    public interface IStackExchangeService
    {
       
        Task<List<TagItemDto>> GetAllTagsAsync();
    }
}
