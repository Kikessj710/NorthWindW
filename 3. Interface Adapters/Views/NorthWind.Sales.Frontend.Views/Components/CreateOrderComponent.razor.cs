using Microsoft.AspNetCore.Components;

namespace NorthWind.Sales.Frontend.Views.Components;

public partial class CreateOrderComponent
{
    [Inject] public IPosQueriesGateway PosQueries { get; set; } = null!;
    [Inject] public IProductsGateway ProductsGateway { get; set; } = null!;
    [Parameter]
    public CreateOrderViewModel Order { get; set; } = null!;

    protected override async Task OnInitializedAsync() =>
        await Order.LoadAvailableProductsAsync();

    public int SelectedProductId { get; set; }
    public bool CustomerModalVisible { get; set; }
    public bool ProductModalVisible { get; set; }
    public string CustomerIdSearch { get; set; } = string.Empty;
    public string CustomerNameSearch { get; set; } = string.Empty;
    public string ProductSearch { get; set; } = string.Empty;
    public IReadOnlyList<CustomerSearchDto> CustomerResults { get; set; } = [];
    public IReadOnlyList<AvailableProductDto> ProductResults { get; set; } = [];

    public async Task OpenCustomerModalAsync()
    {
        CustomerModalVisible = true;
        CustomerResults = await PosQueries.SearchCustomersAsync(CustomerIdSearch, CustomerNameSearch, 1);
    }

    public async Task SearchCustomersAsync() =>
        CustomerResults = await PosQueries.SearchCustomersAsync(CustomerIdSearch, CustomerNameSearch, 1);

    public void SelectCustomer(CustomerSearchDto customer)
    {
        Order.CustomerId = customer.Id;
        CustomerModalVisible = false;
    }

    public async Task OpenProductModalAsync()
    {
        ProductModalVisible = true;
        ProductResults = await ProductsGateway.GetAvailableProductsAsync(ProductSearch);
    }

    public async Task SearchProductsAsync() =>
        ProductResults = await ProductsGateway.GetAvailableProductsAsync(ProductSearch);

    public void SelectProduct(AvailableProductDto product)
    {
        Order.AddProduct(product);
        ProductModalVisible = false;
    }

    public void ReplaceProduct(CreateOrderDetailViewModel detail, int productId)
    {
        var product = Order.AvailableProducts.FirstOrDefault(item => item.Id == productId);
        if (product is null || Order.OrderDetails.Any(item => item != detail && item.ProductId == productId))
        {
            Order.SetInformationMessage("El producto ya existe en la orden o no está disponible.");
            return;
        }
        detail.ProductId = product.Id;
        detail.UnitPrice = product.UnitPrice;
    }

    public void AddSelectedProduct()
    {
        var product = Order.AvailableProducts.FirstOrDefault(item => item.Id == SelectedProductId);
        if (product is not null)
            Order.AddProduct(product);
    }
}
