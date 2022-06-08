using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    public partial class newmigratio43 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sku = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Roles = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Street = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    State = table.Column<string>(type: "TEXT", nullable: false),
                    ZipCode = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerAddresses_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<long>(type: "INTEGER", nullable: false),
                    Date = table.Column<string>(type: "TEXT", nullable: false),
                    OrderTotal = table.Column<decimal>(type: "TEXT", nullable: false),
                    OrderDetailsId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_OrderDetails_OrderDetailsId",
                        column: x => x.OrderDetailsId,
                        principalTable: "OrderDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "Name" },
                values: new object[] { 1, "d@j.com1", "Customer1" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "Name" },
                values: new object[] { 2, "d@j.com2", "Customer2" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "Name" },
                values: new object[] { 3, "d@j.com3", "Customer3" });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "Id", "OrderId", "ProductId", "Quantity" },
                values: new object[] { 1, 1, 5, 12 });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "Id", "OrderId", "ProductId", "Quantity" },
                values: new object[] { 2, 2, 6, 5 });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "Id", "OrderId", "ProductId", "Quantity" },
                values: new object[] { 3, 3, 2, 22 });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerId", "Date", "OrderDetailsId", "OrderTotal" },
                values: new object[] { 1L, 1L, "1999-04-03", null, 12.32m });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerId", "Date", "OrderDetailsId", "OrderTotal" },
                values: new object[] { 2L, 2L, "1999-04-03", null, 12.32m });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerId", "Date", "OrderDetailsId", "OrderTotal" },
                values: new object[] { 3L, 3L, "1999-04-03", null, 12.32m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Manufacturer", "Name", "Price", "Sku", "Type" },
                values: new object[] { 1L, "descript1", "homedepot1", "name1", 42.15m, "23451", "Hello1" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Manufacturer", "Name", "Price", "Sku", "Type" },
                values: new object[] { 2L, "descript2", "homedepot2", "name2", 42.25m, "23452", "Hello2" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Manufacturer", "Name", "Price", "Sku", "Type" },
                values: new object[] { 3L, "descript3", "homedepot3", "name3", 42.35m, "23453", "Hello3" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Manufacturer", "Name", "Price", "Sku", "Type" },
                values: new object[] { 4L, "descript4", "homedepot4", "name4", 42.45m, "23454", "Hello4" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Manufacturer", "Name", "Price", "Sku", "Type" },
                values: new object[] { 5L, "descript5", "homedepot5", "name5", 42.55m, "23455", "Hello5" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Manufacturer", "Name", "Price", "Sku", "Type" },
                values: new object[] { 6L, "descript6", "homedepot6", "name6", 42.65m, "23456", "Hello6" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Manufacturer", "Name", "Price", "Sku", "Type" },
                values: new object[] { 7L, "descript7", "homedepot7", "name7", 42.75m, "23457", "Hello7" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Manufacturer", "Name", "Price", "Sku", "Type" },
                values: new object[] { 8L, "descript8", "homedepot8", "name8", 42.85m, "23458", "Hello8" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Manufacturer", "Name", "Price", "Sku", "Type" },
                values: new object[] { 9L, "descript9", "homedepot9", "name9", 42.95m, "29459", "Hello9" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "Roles", "Title" },
                values: new object[] { 1, "d@j.com", "David", "123pw", "[EMPLOYEE, ADMIN]", "Janitor" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "Roles", "Title" },
                values: new object[] { 2, "A@j.com", "Amir", "123pw", "[EMPLOYEE]", "Cleaner" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "Roles", "Title" },
                values: new object[] { 3, "h@j.com", "Hayes", "123pw", "[EMPLOYEE, ADMIN]", "Boss" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "Roles", "Title" },
                values: new object[] { 4, "c@j.com", "Cody", "123pw", "[ADMIN]", "HR" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "Roles", "Title" },
                values: new object[] { 5, "j@j.com", "Joe", "123pw", "[ADMIN]", "Janitor" });

            migrationBuilder.InsertData(
                table: "CustomerAddresses",
                columns: new[] { "Id", "City", "CustomerId", "State", "Street", "ZipCode" },
                values: new object[] { 1, "City1", 1, "CA", "Street1", "22341" });

            migrationBuilder.InsertData(
                table: "CustomerAddresses",
                columns: new[] { "Id", "City", "CustomerId", "State", "Street", "ZipCode" },
                values: new object[] { 2, "City2", 2, "CA", "Street2", "22342" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddresses_CustomerId",
                table: "CustomerAddresses",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderDetailsId",
                table: "Orders",
                column: "OrderDetailsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerAddresses");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "OrderDetails");
        }
    }
}
