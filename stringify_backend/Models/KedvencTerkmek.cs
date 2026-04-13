using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stringify_backend.Models
{
    [Table("kedvenc_termek")]
    public class KedvencTermek
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("FelhasznaloId")]
        public int FelhasznaloId { get; set; }

        [Required]
        [Column("TermekId")]
        public int TermekId { get; set; }

        [Column("Letrehozva")]
        public DateTime Letrehozva { get; set; } = DateTime.Now;

        public virtual User? Felhasznalo { get; set; }
        public virtual Termek? Termek { get; set; }
    }
}