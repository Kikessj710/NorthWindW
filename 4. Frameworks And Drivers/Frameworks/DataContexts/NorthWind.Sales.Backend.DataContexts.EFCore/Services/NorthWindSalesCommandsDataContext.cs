namespace NorthWind.Sales.Backend.DataContexts.EFCore.Services;

internal class NorthWindSalesCommandsDataContext(IOptions<DBOptions> dbOptions)
    : NorthWindSalesContext(dbOptions), INorthWindSalesCommandsDataContext
{
    public async Task AddOrderAsync(Order order) => await AddAsync(order);

    public async Task AddOrderDetailsAsync(
        IEnumerable<Repositories.Entities.OrderDetail> orderDetails) =>
        await AddRangeAsync(orderDetails);

    public async Task DecreaseProductStockAsync(IEnumerable<ProductStockChange> changes)
    {
        foreach (var change in changes)
        {
            var product = await Products.FirstOrDefaultAsync(item => item.Id == change.ProductId);
            if (product is null)
                throw new InvalidOperationException($"Product {change.ProductId} was not found.");
            if (product.UnitsInStock < change.Quantity)
                throw new InvalidOperationException($"Insufficient stock for product {change.ProductId}.");
            product.UnitsInStock -= change.Quantity;
        }
    }

    public async Task SaveChangesAsync() => await GuardDBContext.AgainstSaveChangesErrorAsync(this);
}
