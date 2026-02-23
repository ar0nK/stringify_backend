namespace stringify_backend.Models
{
    public class EgyediGitar
    {
        public int Id { get; set; }
        public int? FelhasznaloId { get; set; }
        public int TestformaId { get; set; }
        public int NeckId { get; set; }
        public int? FinishId { get; set; }
        public int? PickguardId { get; set; }
        public DateTime Letrehozva { get; set; }
    }
}
