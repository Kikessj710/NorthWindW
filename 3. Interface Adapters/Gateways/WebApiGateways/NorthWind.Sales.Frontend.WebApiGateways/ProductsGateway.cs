namespace NorthWind.Sales.Frontend.WebApiGateways;

internal sealed class ProductsGateway(HttpClient client) : IProductsGateway
{
    public async Task<IReadOnlyList<AvailableProductDto>> GetAvailableProductsAsync(string? search = null)
    {
        var endpoint = "/Products/available";
        if (!string.IsNullOrWhiteSpace(search))
            endpoint += $"?search={Uri.EscapeDataString(search)}";
        return await client.GetFromJsonAsync<List<AvailableProductDto>>(endpoint) ?? [];
    }
}
