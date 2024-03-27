namespace MediPortaZadanieTestowe.Models
{
    public class TagItemDto
    {
        public bool HasSynonyms { get; set; }
        public bool IsModeratorOnly { get; set; }
        public bool IsRequired { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
    }
}
