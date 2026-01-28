namespace stringify_backend.Models
{
    public class EgyediGitar
    {
        public int Id { get; set; }
        public int? FelhasznaloId { get; set; }
        public int BodyShapeId { get; set; }
        public int BodyWoodId { get; set; }
        public int NeckWoodId { get; set; }
        public int? NeckPickupId { get; set; }
        public int? MiddlePickupId { get; set; }
        public int? BridgePickupId { get; set; }
        public int? FinishId { get; set; }
        public int? PickguardId { get; set; }
        public DateTime Letrehozva { get; set; }
    }
}
