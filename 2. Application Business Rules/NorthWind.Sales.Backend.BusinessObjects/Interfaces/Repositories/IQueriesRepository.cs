namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;

public interface IQueriesRepository
{
    Task<decimal?> GetCustomerCurrentBalance(string customerId);
    Task<IEnumerable<ProductUnitsInStock>> GetProductsUnitsInStock(IEnumerable<int> productIds);
    Task<IEnumerable<AvailableProduct>> GetAvailableProducts(string? search = null);
    Task<IEnumerable<CustomerSearchDto>> SearchCustomers(string? id, string? name, int page, int pageSize);
    Task<IEnumerable<OrderSearchDto>> SearchOrders(string? orderId, string? customerId, int page, int pageSize);
    Task<OrderInvoiceDto?> GetOrderInvoice(int orderId);
}
