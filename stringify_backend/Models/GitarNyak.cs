namespace stringify_backend.Models;

public class GitarNyak
{
    public int Id { get; set; }
    public string Nev { get; set; } = null!;
    public string KepUrl { get; set; } = null!;
    public int Ar { get; set; }
    public int ZIndex { get; set; }
}