namespace NorthWind.Sales.Backend.Repositories.Repositories;

internal class CommandsRepository(INorthWindSalesCommandsDataContext context)
    : ICommandsRepository
{
    public async Task CreateOrder(OrderAggregate order)
    {
        await context.AddOrderAsync(order);
        await context.AddOrderDetailsAsync(
            order.OrderDetails
                .Select(detail => new Entities.OrderDetail
                {
                    Order = order,
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                })
                .ToArray());
    }

    public async Task DecreaseProductStock(IEnumerable<ProductStockChange> changes) =>
        await context.DecreaseProductStockAsync(changes);

    public async Task SaveChanges() => await context.SaveChangesAsync();
}
