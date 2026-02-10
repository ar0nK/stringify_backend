using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stringify_backend.Models
{
    [Table("jogok")]
    public class Jogok
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("Nev")]
        [MaxLength(32)]
        public string Nev { get; set; } = string.Empty;

        [Required]
        [Column("Szint")]
        public int Szint { get; set; }

        [Column("Leiras")]
        [MaxLength(128)]
        public string? Leiras { get; set; }

        // Navigation property
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}