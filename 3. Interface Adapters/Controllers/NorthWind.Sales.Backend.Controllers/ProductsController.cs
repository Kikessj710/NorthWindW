namespace Microsoft.AspNetCore.Builder;

public static class ProductsController
{
    public static WebApplication UseProductsController(this WebApplication app)
    {
        app.MapGet("/Products/available", GetAvailableProducts)
            .RequireAuthorization();
        return app;
    }

    private static Task<IEnumerable<AvailableProduct>> GetAvailableProducts(
        IQueriesRepository repository, string? search = null) =>
        repository.GetAvailableProducts(search);
}
