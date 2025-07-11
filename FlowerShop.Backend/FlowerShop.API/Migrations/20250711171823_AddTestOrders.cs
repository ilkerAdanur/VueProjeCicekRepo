using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlowerShop.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTestOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "City", "CreatedAt", "Email", "FirstName", "LastName", "Phone", "PostalCode" },
                values: new object[] { 1, "", "", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "test@example.com", "Test", "Customer", "555-0123", "" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AEOEmypQRSZKzj5zc2bT4AQ9opGggat+5A6dcT+Kgtw=");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "/+q6yUKvv+TaQyV1KgtAcpVA12kno4wKC2ushly3UCg=");

            // Update demo user email
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Email",
                value: "demo@demo");

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerId", "DeliveryAddress", "DeliveryCity", "DeliveryDate", "DeliveryPostalCode", "Notes", "OrderDate", "OrderNumber", "Status", "TotalAmount", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "123 Test Street", "Istanbul", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "34000", "Test sipariş", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ORD-20250711-0001", 0, 150.00m, null },
                    { 2, 1, "456 Demo Avenue", "Ankara", new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), "06000", "Demo sipariş", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ORD-20250711-0002", 1, 200.00m, null }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "FlowerId", "OrderId", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, 2, 75.00m },
                    { 2, 2, 2, 1, 200.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg=");
        }
    }
}
