using System.ComponentModel.DataAnnotations.Schema;

namespace stringify_backend.Models
{
    public partial class RendelesTetel
    {
        public int Id { get; set; }
        public int RendelesId { get; set; }
        public int? TermekId { get; set; }
        public int? EgyediGitarId { get; set; }

        [Column("TermekDarab")]
        public int Darabszam { get; set; } = 1;

        public virtual Rendeles Rendeles { get; set; } = null!;
        public virtual Termek? Termek { get; set; }
        public virtual EgyediGitar? EgyediGitar { get; set; }
    }
}