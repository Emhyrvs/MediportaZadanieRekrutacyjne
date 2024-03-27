namespace MediPortaZadanieTestowe.Models
{
    public class TagsDtoRead
    {
        public List<TagItemDto> Items { get; set; }
        public bool HasMore { get; set; }
        public int QuotaMax { get; set; }
        public int QuotaRemaining { get; set; }
    }
}
