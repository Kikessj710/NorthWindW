namespace NorthWind.Sales.Backend.BusinessObjects.Aggregates;

public class OrderAggregate : Order
{
    private readonly List<OrderDetail> OrderDetailsField = [];

    public IReadOnlyCollection<OrderDetail> OrderDetails => OrderDetailsField;

    public void AddDetail(int productId, decimal unitPrice, short quantity)
    {
        var existingOrderDetail = OrderDetailsField
            .FirstOrDefault(orderDetail => orderDetail.ProductId == productId);

        if (existingOrderDetail is not null)
        {
            quantity += existingOrderDetail.Quantity;
            OrderDetailsField.Remove(existingOrderDetail);
        }

        OrderDetailsField.Add(new OrderDetail(productId, unitPrice, quantity));
        RecalculateTotals();
    }

    private void RecalculateTotals()
    {
        Subtotal = decimal.Round(OrderDetailsField.Sum(detail => detail.UnitPrice * detail.Quantity), 2);
        VatAmount = decimal.Round(Subtotal * 0.15m, 2);
        TotalAmount = Subtotal + VatAmount;
    }

    public static OrderAggregate From(CreateOrderDto orderDto)
    {
        var order = new OrderAggregate
        {
            CustomerId = orderDto.CustomerId,
            ShipAddress = orderDto.ShipAddress,
            ShipCity = orderDto.ShipCity,
            ShipCountry = orderDto.ShipCountry,
            ShipPostalCode = orderDto.ShipPostalCode
        };

        foreach (var item in orderDto.OrderDetails)
        {
            order.AddDetail(item.ProductId, item.UnitPrice, item.Quantity);
        }

        return order;
    }
}
