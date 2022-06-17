using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiddleTemplate.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teas", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("40b76c62-2569-4fa1-80f9-18cdbb1125a9"), new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8602), "Chaga Chai Mushroom Tea", 20m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("5eec791a-b0b3-4c30-8c3e-b9629f690a12"), new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8596), "Jasmine Pearls", 41m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("5fac5dc1-3178-4d72-b32a-3c193bb01a8c"), new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8598), "Dragonwell", 30m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("66ee4c28-b49e-40c3-8cf2-8ac08e2e7a16"), new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8582), "Big Sur Tea", 25m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("6a84d852-42b2-4569-8d07-3aca01683a59"), new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8595), "Big Sur Tea", 25m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("7dab1670-640c-40a3-84d6-568cb487d3b4"), new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8599), "White Peach Tea", 29m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("868fddce-fcc3-4c14-a2dc-faa3bc719dfd"), new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8600), "Vanilla Berry Tea", 21m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("b377fbb8-bae6-4b24-8d8a-3f707dc889ba"), new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(7905), "Earl Gray", 20m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("b3d29f03-9eb8-4277-9d2a-f81432325273"), new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8581), "English Breakfast Tea", 18m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("b48e8bec-fbcd-4775-8add-d11c99e5c6ef"), new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8603), "Naked Pu-erh Tea", 27m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("e04e349f-448d-40d8-aa86-cba7e68984af"), new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8577), "Rose Tea", 20m, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Teas");
        }
    }
}
