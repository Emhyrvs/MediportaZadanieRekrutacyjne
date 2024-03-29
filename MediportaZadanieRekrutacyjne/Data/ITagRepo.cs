using MediportaZadanieRekrutacyjne.Models;

namespace MediportaZadanieRekrutacyjne.Data
{
    public interface ITagRepo
    {
        Task CreateTag(TagItem tag);
        
        Task<List<TagItem>> GetTagsAsync(int pageNumber, int pageSize, string sortBy, string sortOrder);
        Task GetTagsFromService();
        Task UpdateCount();
    }
}
