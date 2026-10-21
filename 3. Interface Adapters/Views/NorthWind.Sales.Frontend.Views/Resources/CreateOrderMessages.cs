using System.Globalization;
using System.Resources;

namespace NorthWind.Sales.Frontend.Views.Resources;

public static class CreateOrderMessages
{
    private static readonly ResourceManager ResourceManager = new(
        "NorthWind.Sales.Frontend.Views.Resources.CreateOrderMessages",
        typeof(CreateOrderMessages).Assembly);

    public static string CustomerIdLabel => GetString(nameof(CustomerIdLabel));
    public static string ShipAddressLabel => GetString(nameof(ShipAddressLabel));
    public static string ShipCityLabel => GetString(nameof(ShipCityLabel));
    public static string ShipCountryLabel => GetString(nameof(ShipCountryLabel));
    public static string ShipPostalCodeLabel => GetString(nameof(ShipPostalCodeLabel));
    public static string ProductIdLabel => GetString(nameof(ProductIdLabel));
    public static string UnitPriceLabel => GetString(nameof(UnitPriceLabel));
    public static string QuantityLabel => GetString(nameof(QuantityLabel));
    public static string AddProductButtonText => GetString(nameof(AddProductButtonText));
    public static string SendButtonText => GetString(nameof(SendButtonText));
    public static string CreatedOrderTemplate => GetString(nameof(CreatedOrderTemplate));
    public static string CreateOrderMenuItemTitle => GetString(nameof(CreateOrderMenuItemTitle));
    public static string CreateOrderPageTitle => GetString(nameof(CreateOrderPageTitle));

    private static string GetString(string name) =>
        ResourceManager.GetString(name, CultureInfo.CurrentUICulture)!;
}
