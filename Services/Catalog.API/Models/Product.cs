namespace eCommerceApp.Catalog.API.Models;

public class Product
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public List<Guid> CategoryIds { get; set; } = new();
    public string Description { get; set; } = default!;
    public string ImageFile { get; set; } = default!;
    public decimal Price { get; set; }
}