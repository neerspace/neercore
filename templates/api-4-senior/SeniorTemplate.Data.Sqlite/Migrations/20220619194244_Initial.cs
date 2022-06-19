using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeniorTemplate.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Registered = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "AppRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<int>(type: "smallint", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRoleClaims_AppRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppRefreshTokens",
                columns: table => new
                {
                    Token = table.Column<string>(type: "varchar(128)", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserAgent = table.Column<string>(type: "TEXT", unicode: false, maxLength: 512, nullable: false),
                    IpAddress = table.Column<string>(type: "TEXT", unicode: false, maxLength: 46, nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRefreshTokens", x => x.Token);
                    table.ForeignKey(
                        name: "FK_AppRefreshTokens_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserClaims_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AppUserLogins_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoleId = table.Column<int>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AppUserRoles_AppRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserRoles_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AppUserTokens_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 1, "64c2b083-c54b-4d9f-ba16-1294ec1ceafa", "user", "USER" });

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 2, "fdb2e068-85b3-4108-9830-3eb52c04f702", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Description", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Registered", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1, 0, "777fd44b-5dde-4356-928e-654ff5840563", null, "aspadmin@asp.net", true, false, null, "ASPADMIN@ASP.NET", "ASPADMIN", "AQAAAAEAACcQAAAAEMhMB5TD25L/FkonrYU9upx8wfsDCWCAD97/kNIhgkPcXvOuzoCJxe5L+fbPffZpmQ==", null, false, new DateTime(2022, 6, 19, 19, 42, 43, 802, DateTimeKind.Utc).AddTicks(6995), "5060a0f8-d33e-4f98-a6ab-16e61ea03c1e", false, "aspadmin" });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("023f9bec-d736-4c54-94e8-69257af821e2"), new DateTime(2022, 6, 19, 19, 42, 43, 814, DateTimeKind.Utc).AddTicks(5666), "Naked Pu-erh Tea", 27m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("18b4a333-c4d9-4604-b816-71dce6cd6c67"), new DateTime(2022, 6, 19, 19, 42, 43, 814, DateTimeKind.Utc).AddTicks(5665), "Chaga Chai Mushroom Tea", 20m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("55672e16-310a-49c4-a87e-5c429cc783e2"), new DateTime(2022, 6, 19, 19, 42, 43, 814, DateTimeKind.Utc).AddTicks(5649), "Rose Tea", 20m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("60e8efd0-8f74-49a2-895b-35913a9f05cb"), new DateTime(2022, 6, 19, 19, 42, 43, 814, DateTimeKind.Utc).AddTicks(5654), "Big Sur Tea", 25m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("6784be54-7648-469a-b158-f1e1affff931"), new DateTime(2022, 6, 19, 19, 42, 43, 814, DateTimeKind.Utc).AddTicks(5663), "White Peach Tea", 29m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("8b015cb5-ffaa-4efe-a9b3-160c3e7bf1da"), new DateTime(2022, 6, 19, 19, 42, 43, 814, DateTimeKind.Utc).AddTicks(5664), "Vanilla Berry Tea", 21m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("ab874d21-0824-4b43-a65d-7fd31a05f5af"), new DateTime(2022, 6, 19, 19, 42, 43, 814, DateTimeKind.Utc).AddTicks(5653), "Big Sur Tea", 25m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("c35abbae-4191-4541-8304-3dd071a7cd2c"), new DateTime(2022, 6, 19, 19, 42, 43, 814, DateTimeKind.Utc).AddTicks(5662), "Dragonwell", 30m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("c6b2e090-be68-4f3c-96c0-c62670b44934"), new DateTime(2022, 6, 19, 19, 42, 43, 814, DateTimeKind.Utc).AddTicks(4869), "Earl Gray", 20m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("dcfdfe8b-d61e-49e9-aa0a-bdf113a4f841"), new DateTime(2022, 6, 19, 19, 42, 43, 814, DateTimeKind.Utc).AddTicks(5652), "English Breakfast Tea", 18m, null });

            migrationBuilder.InsertData(
                table: "Teas",
                columns: new[] { "Id", "Created", "Name", "Price", "Updated" },
                values: new object[] { new Guid("e3e61b63-f384-4a35-a163-7bde25af7c38"), new DateTime(2022, 6, 19, 19, 42, 43, 814, DateTimeKind.Utc).AddTicks(5661), "Jasmine Pearls", 41m, null });

            migrationBuilder.InsertData(
                table: "AppRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[] { 1, "permission", "mt", 2 });

            migrationBuilder.InsertData(
                table: "AppUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 1, "permission", "*", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_AppRefreshTokens_UserId",
                table: "AppRefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleClaims_RoleId",
                table: "AppRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AppRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserClaims_UserId",
                table: "AppUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserLogins_UserId",
                table: "AppUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoles_RoleId",
                table: "AppUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AppUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AppUsers",
                column: "NormalizedUserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppRefreshTokens");

            migrationBuilder.DropTable(
                name: "AppRoleClaims");

            migrationBuilder.DropTable(
                name: "AppUserClaims");

            migrationBuilder.DropTable(
                name: "AppUserLogins");

            migrationBuilder.DropTable(
                name: "AppUserRoles");

            migrationBuilder.DropTable(
                name: "AppUserTokens");

            migrationBuilder.DropTable(
                name: "Teas");

            migrationBuilder.DropTable(
                name: "AppRoles");

            migrationBuilder.DropTable(
                name: "AppUsers");
        }
    }
}
