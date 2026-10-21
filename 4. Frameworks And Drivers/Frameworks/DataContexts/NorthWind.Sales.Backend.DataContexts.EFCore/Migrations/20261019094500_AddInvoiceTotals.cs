using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NorthWind.Sales.Backend.DataContexts.EFCore.Migrations;

[Migration("20261019094500_AddInvoiceTotals")]
public partial class AddInvoiceTotals : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<decimal>("Subtotal", "Orders", type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m);
        migrationBuilder.AddColumn<decimal>("VatAmount", "Orders", type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m);
        migrationBuilder.AddColumn<decimal>("TotalAmount", "Orders", type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn("Subtotal", "Orders");
        migrationBuilder.DropColumn("VatAmount", "Orders");
        migrationBuilder.DropColumn("TotalAmount", "Orders");
    }
}
