using System;
using System.Collections.Generic;

namespace stringify_backend.Models
{
    public partial class Rendeles
    {
        public int Id { get; set; }
        public int FelhasznaloId { get; set; }
        public int Osszeg { get; set; }
        public string Status { get; set; } = null!;
        public DateTime? Datum { get; set; }

        public virtual ICollection<RendelesTetel> Tetelek { get; set; } = new List<RendelesTetel>();
    }
}
