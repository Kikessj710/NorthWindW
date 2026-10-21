namespace NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;

public class CreateOrderViewModel(ICreateOrderGateway gateway, IProductsGateway productsGateway, IModelValidatorHub<CreateOrderViewModel> validator)
{
    public IModelValidatorHub<CreateOrderViewModel> Validator => validator;
    public ModelValidator<CreateOrderViewModel>? ModelValidatorComponentReference { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public string ShipAddress { get; set; } = string.Empty;
    public string ShipCity { get; set; } = string.Empty;
    public string ShipCountry { get; set; } = string.Empty;
    public string ShipPostalCode { get; set; } = string.Empty;
    public List<CreateOrderDetailViewModel> OrderDetails { get; set; } = [];
    public string InformationMessage { get; private set; } = string.Empty;
    public void SetInformationMessage(string message) => InformationMessage = message;
    public IReadOnlyList<AvailableProductDto> AvailableProducts { get; private set; } = [];
    public decimal Subtotal => OrderDetails.Sum(item => item.UnitPrice * item.Quantity);
    public decimal VatAmount => decimal.Round(Subtotal * 0.15m, 2);
    public decimal TotalAmount => Subtotal + VatAmount;

    public async Task LoadAvailableProductsAsync(string? search = null) =>
        AvailableProducts = await productsGateway.GetAvailableProductsAsync(search);

    public void AddProduct(AvailableProductDto product)
    {
        if (OrderDetails.Any(item => item.ProductId == product.Id))
        {
            InformationMessage = "El producto ya está agregado a la orden.";
            return;
        }
        OrderDetails.Add(new CreateOrderDetailViewModel
        {
            ProductId = product.Id,
            UnitPrice = product.UnitPrice,
            Quantity = 1
        });
        InformationMessage = string.Empty;
    }

    public void RemoveProduct(CreateOrderDetailViewModel item) => OrderDetails.Remove(item);

    public void AddNewOrderDetailItem() => OrderDetails.Add(new CreateOrderDetailViewModel());

    public async Task Send()
    {
        InformationMessage = string.Empty;
        try
        {
            var orderId = await gateway.CreateOrderAsync((CreateOrderDto)this);
            InformationMessage = string.Format(CreateOrderMessages.CreatedOrderTemplate, orderId);
        }
        catch (HttpRequestException exception)
        {
            if (exception.Data["Errors"] is IEnumerable<ValidationError> errors)
                ModelValidatorComponentReference?.AddErrors(errors);
            else
                throw;
        }
    }

    public static explicit operator CreateOrderDto(CreateOrderViewModel model) =>
        new(model.CustomerId, model.ShipAddress, model.ShipCity, model.ShipCountry, model.ShipPostalCode,
            model.OrderDetails.Select(detail => new CreateOrderDetailDto(detail.ProductId, detail.UnitPrice, detail.Quantity)));
}
