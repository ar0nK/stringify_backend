using System;

namespace stringify_backend.Dtos;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string? ShortDescription { get; set; }
    public int Price { get; set; }
    public bool IsAvailable { get; set; }
    public int? GuitarTypeId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<string> Images { get; set; } = new();
}
