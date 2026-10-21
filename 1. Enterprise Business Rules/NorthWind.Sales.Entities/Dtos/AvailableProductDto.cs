namespace NorthWind.Sales.Entities.Dtos;

public sealed class AvailableProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public short UnitsInStock { get; set; }
}
