namespace Microsoft.AspNetCore.Builder;

public static class PosQueriesController
{
    public static WebApplication UsePosQueriesController(this WebApplication app)
    {
        app.MapGet(Endpoints.SearchCustomers, async (IQueriesRepository repository, string? id, string? name, int page = 1) =>
            Results.Ok(await repository.SearchCustomers(id, name, page, 8))).RequireAuthorization();
        app.MapGet(Endpoints.SearchOrders, async (IQueriesRepository repository, string? orderId, string? customerId, int page = 1) =>
            Results.Ok(await repository.SearchOrders(orderId, customerId, page, 8))).RequireAuthorization();
        app.MapGet($"{Endpoints.GetOrderInvoice}/{{orderId:int}}", async (IQueriesRepository repository, int orderId) =>
        {
            var invoice = await repository.GetOrderInvoice(orderId);
            return invoice is null ? Results.NotFound() : Results.Ok(invoice);
        }).RequireAuthorization();
        return app;
    }
}
