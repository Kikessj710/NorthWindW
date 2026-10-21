namespace NorthWind.Sales.Backend.BusinessObjects.ValueObjects;

public class ProductUnitsInStock(int productId, short unitsInStock)
{
    public int ProductId => productId;
    public short UnitsInStock => unitsInStock;
}

public class ProductStockChange(int productId, short quantity)
{
    public int ProductId => productId;
    public short Quantity => quantity;
}
