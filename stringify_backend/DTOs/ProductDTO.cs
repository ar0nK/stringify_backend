using System;

namespace stringify_backend.Dtos;

public class ProductDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? ShortDescription { get; set; }
    public string LongDescription { get; set; } = "";
    public string PreviewDescription { get; set; } = "";
    public int Price { get; set; }
    public bool IsAvailable { get; set; }
    public int? GuitarTypeId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<string> Images { get; set; } = new();
}
