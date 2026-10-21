namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

public interface IProductsGateway
{
    Task<IReadOnlyList<AvailableProductDto>> GetAvailableProductsAsync(string? search = null);
}
