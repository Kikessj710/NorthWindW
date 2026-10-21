namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

public interface IPosQueriesGateway
{
    Task<IReadOnlyList<CustomerSearchDto>> SearchCustomersAsync(string? id, string? name, int page);
    Task<IReadOnlyList<OrderSearchDto>> SearchOrdersAsync(string? orderId, string? customerId, int page);
    Task<OrderInvoiceDto?> GetOrderInvoiceAsync(int orderId);
}
