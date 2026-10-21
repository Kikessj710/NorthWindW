namespace NorthWind.Sales.Entities.ValueObjects;

public class Endpoints
{
    public const string CreateOrder = $"/{nameof(CreateOrder)}";
    public const string SearchCustomers = "/Customers/search";
    public const string SearchOrders = "/Orders/search";
    public const string GetOrderInvoice = "/Orders";
}
