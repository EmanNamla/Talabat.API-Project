using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repository.Data.Migrations
{
    public partial class RenametheshipToAddressCloulm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippingAddress_Street",
                table: "Orders",
                newName: "shipToAddress_Street");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_LastName",
                table: "Orders",
                newName: "shipToAddress_LastName");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_FirstName",
                table: "Orders",
                newName: "shipToAddress_FirstName");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_Country",
                table: "Orders",
                newName: "shipToAddress_Country");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_City",
                table: "Orders",
                newName: "shipToAddress_City");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "shipToAddress_Street",
                table: "Orders",
                newName: "ShippingAddress_Street");

            migrationBuilder.RenameColumn(
                name: "shipToAddress_LastName",
                table: "Orders",
                newName: "ShippingAddress_LastName");

            migrationBuilder.RenameColumn(
                name: "shipToAddress_FirstName",
                table: "Orders",
                newName: "ShippingAddress_FirstName");

            migrationBuilder.RenameColumn(
                name: "shipToAddress_Country",
                table: "Orders",
                newName: "ShippingAddress_Country");

            migrationBuilder.RenameColumn(
                name: "shipToAddress_City",
                table: "Orders",
                newName: "ShippingAddress_City");
        }
    }
}
