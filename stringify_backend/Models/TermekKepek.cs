namespace stringify_backend.Models;

public partial class TermekKepek
{
    public int Id { get; set; }
    public int TermekId { get; set; }

    public string Kep1 { get; set; } = null!;
    public string Kep2 { get; set; } = null!;
    public string Kep3 { get; set; } = null!;
    public string Kep4 { get; set; } = null!;
    public string Kep5 { get; set; } = null!;

    public virtual Termek Termek { get; set; } = null!;
}
