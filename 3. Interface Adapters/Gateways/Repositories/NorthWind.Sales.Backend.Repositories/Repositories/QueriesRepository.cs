namespace NorthWind.Sales.Backend.Repositories.Repositories;

internal class QueriesRepository(INorthWindSalesQueriesDataContext context) : IQueriesRepository
{
    public async Task<decimal?> GetCustomerCurrentBalance(string customerId)
    {
        var queryable = context.Customers.Where(c => c.Id == customerId).Select(c => new { c.CurrentBalance });
        var result = await context.FirstOrDefaultAync(queryable);
        return result?.CurrentBalance;
    }

    public async Task<IEnumerable<ProductUnitsInStock>> GetProductsUnitsInStock(IEnumerable<int> productIds)
    {
        var queryable = context.Products.Where(p => productIds.Contains(p.Id)).Select(p => new ProductUnitsInStock(p.Id, p.UnitsInStock));
        return await context.ToListAsync(queryable);
    }

    public async Task<IEnumerable<AvailableProduct>> GetAvailableProducts(string? search = null)
    {
        var queryable = context.Products.Where(product => product.UnitsInStock > 0);
        if (!string.IsNullOrWhiteSpace(search))
            queryable = queryable.Where(product => product.Name.Contains(search));
        var projected = queryable.OrderBy(product => product.Name)
            .Select(product => new AvailableProduct(product.Id, product.Name, product.UnitPrice, product.UnitsInStock));
        return await context.ToListAsync(projected);
    }

    public async Task<IEnumerable<CustomerSearchDto>> SearchCustomers(string? id, string? name, int page, int pageSize)
    {
        var query = context.Customers.AsQueryable();
        if (!string.IsNullOrWhiteSpace(id)) query = query.Where(customer => customer.Id.Contains(id));
        if (!string.IsNullOrWhiteSpace(name)) query = query.Where(customer => customer.Name.Contains(name));
        return await context.ToListAsync(query.OrderBy(customer => customer.Id)
            .Skip(Math.Max(page - 1, 0) * pageSize).Take(pageSize)
            .Select(customer => new CustomerSearchDto { Id = customer.Id, Name = customer.Name }));
    }

    public async Task<IEnumerable<OrderSearchDto>> SearchOrders(string? orderId, string? customerId, int page, int pageSize)
    {
        var query = context.Orders.AsQueryable();
        if (int.TryParse(orderId, out var parsedId)) query = query.Where(order => order.Id == parsedId);
        if (!string.IsNullOrWhiteSpace(customerId)) query = query.Where(order => order.CustomerId.Contains(customerId));
        return await context.ToListAsync(query.OrderByDescending(order => order.OrderDate)
            .Skip(Math.Max(page - 1, 0) * pageSize).Take(pageSize)
            .Select(order => new OrderSearchDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount
            }));
    }

    public async Task<OrderInvoiceDto?> GetOrderInvoice(int orderId)
    {
        var order = await context.FirstOrDefaultAync(context.Orders.Where(item => item.Id == orderId));
        if (order is null) return null;
        var details = await context.ToListAsync(context.OrderDetails.Where(detail => detail.OrderId == orderId));
        var productIds = details.Select(detail => detail.ProductId).ToArray();
        var products = await context.ToListAsync(context.Products.Where(product => productIds.Contains(product.Id)));
        var productNames = products.ToDictionary(product => product.Id, product => product.Name);
        return new OrderInvoiceDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            ShipAddress = order.ShipAddress,
            ShipCity = order.ShipCity,
            ShipCountry = order.ShipCountry,
            ShipPostalCode = order.ShipPostalCode,
            OrderDate = order.OrderDate,
            Subtotal = order.Subtotal,
            VatAmount = order.VatAmount,
            TotalAmount = order.TotalAmount,
            Details = details.Select(detail => new OrderDetailDto
            {
                ProductId = detail.ProductId,
                ProductName = productNames.GetValueOrDefault(detail.ProductId, $"Product {detail.ProductId}"),
                UnitPrice = detail.UnitPrice,
                Quantity = detail.Quantity
            }).ToList()
        };
    }
}
