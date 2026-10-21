using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace NorthWind.Sales.Frontend.Views.Pages;

public partial class Invoices
{
    [Inject] public IPosQueriesGateway Gateway { get; set; } = null!;
    [Inject] public IJSRuntime Js { get; set; } = null!;
    public string OrderIdFilter { get; set; } = string.Empty;
    public string CustomerIdFilter { get; set; } = string.Empty;
    public List<OrderSearchDto> Results { get; set; } = [];
    public OrderInvoiceDto? Invoice { get; set; }
    public string Message { get; set; } = string.Empty;
    public int Page { get; set; } = 1;

    protected override async Task OnInitializedAsync() => await SearchAsync();

    public async Task SearchAsync()
    {
        Page = 1;
        Invoice = null;
        Results = (await Gateway.SearchOrdersAsync(OrderIdFilter, CustomerIdFilter, Page)).ToList();
        Message = Results.Count == 0 ? "No se encontraron facturas." : string.Empty;
    }

    public async Task SelectAsync(int orderId)
    {
        Invoice = await Gateway.GetOrderInvoiceAsync(orderId);
        Message = Invoice is null ? "La factura no existe." : string.Empty;
    }

    public async Task PreviousPageAsync() { if (Page > 1) { Page--; Results = (await Gateway.SearchOrdersAsync(OrderIdFilter, CustomerIdFilter, Page)).ToList(); } }
    public async Task NextPageAsync() { if (Results.Count == 8) { Page++; Results = (await Gateway.SearchOrdersAsync(OrderIdFilter, CustomerIdFilter, Page)).ToList(); } }
    public Task PrintAsync() => Js.InvokeVoidAsync("window.print").AsTask();
}
