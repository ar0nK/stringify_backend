using System;

namespace stringify_backend.Models;

public partial class Termek
{
    public int Id { get; set; }
    public string Nev { get; set; } = null!;
    public string Leiras { get; set; } = null!;
    public string? RovidLeiras { get; set; }
    public int Ar { get; set; }
    public bool Elerheto { get; set; }
    public int? GitarTipusId { get; set; }
    public DateTime? Letrehozva { get; set; }

    public virtual TermekKepek? TermekKepek { get; set; }
}
