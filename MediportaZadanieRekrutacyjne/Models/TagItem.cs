namespace MediportaZadanieRekrutacyjne.Models
{
    public class TagItem
    {

        public int Id { get; set; } 
        public bool HasSynonyms { get; set; }
        public bool IsModeratorOnly { get; set; }
        public bool IsRequired { get; set; }
        public int Count { get; set; }
        public decimal Share { get; set;  }
        public string Name { get; set; }
    }
}
