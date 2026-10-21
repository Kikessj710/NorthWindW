namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(order => order.CustomerId)
            .IsRequired()
            .HasMaxLength(5)
            .IsFixedLength();

        builder.Property(order => order.ShipAddress)
            .IsRequired()
            .HasMaxLength(60);

        builder.Property(order => order.ShipCity)
            .HasMaxLength(15);

        builder.Property(order => order.ShipCountry)
            .HasMaxLength(15);

        builder.Property(order => order.ShipPostalCode)
            .HasMaxLength(10);

        builder.Property(order => order.Subtotal).HasPrecision(18, 2);
        builder.Property(order => order.VatAmount).HasPrecision(18, 2);
        builder.Property(order => order.TotalAmount).HasPrecision(18, 2);

        builder.HasOne<Repositories.Entities.Customer>()
            .WithMany()
            .HasForeignKey(order => order.CustomerId);
    }
}
