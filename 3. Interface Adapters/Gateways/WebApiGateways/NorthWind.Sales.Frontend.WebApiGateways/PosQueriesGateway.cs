namespace NorthWind.Sales.Frontend.WebApiGateways;

internal sealed class PosQueriesGateway(HttpClient client) : IPosQueriesGateway
{
    public async Task<IReadOnlyList<CustomerSearchDto>> SearchCustomersAsync(string? id, string? name, int page) =>
        await client.GetFromJsonAsync<List<CustomerSearchDto>>($"{Endpoints.SearchCustomers}?id={Uri.EscapeDataString(id ?? string.Empty)}&name={Uri.EscapeDataString(name ?? string.Empty)}&page={page}") ?? [];

    public async Task<IReadOnlyList<OrderSearchDto>> SearchOrdersAsync(string? orderId, string? customerId, int page) =>
        await client.GetFromJsonAsync<List<OrderSearchDto>>($"{Endpoints.SearchOrders}?orderId={Uri.EscapeDataString(orderId ?? string.Empty)}&customerId={Uri.EscapeDataString(customerId ?? string.Empty)}&page={page}") ?? [];

    public Task<OrderInvoiceDto?> GetOrderInvoiceAsync(int orderId) =>
        client.GetFromJsonAsync<OrderInvoiceDto>($"{Endpoints.GetOrderInvoice}/{orderId}");
}
