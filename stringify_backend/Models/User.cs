using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stringify_backend.Models
{
    [Table("felhasznalo")]
    public class User
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("Nev")]
        [MaxLength(60)]
        public string Nev { get; set; } = string.Empty;

        [Required]
        [Column("Email")]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Column("Telefonszam")]
        [MaxLength(30)]
        public string Telefonszam { get; set; } = string.Empty;

        [Required]
        [Column("Jelszo")]
        [MaxLength(64)]
        public string Jelszo { get; set; } = string.Empty;

        [Required]
        [Column("SALT")]
        [MaxLength(64)]
        public string Salt { get; set; } = string.Empty;

        [Column("Jogosultsag")]
        public int Jogosultsag { get; set; } = 1;

        [Column("Aktiv")]
        public int Aktiv { get; set; } = 1;

        public virtual Jogok? JogosultsagNavigation { get; set; }
    }
}