using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmallFirmApp.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddOns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isActive = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dds = table.Column<bool>(type: "bit", nullable: false),
                    Ein = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recipient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnMonth = table.Column<bool>(type: "bit", nullable: false),
                    Tax = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Consumative",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isActive = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumative", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExtraService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isActive = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraService", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isActive = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForDelivery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ForDeliver = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForDelivery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForDelivery_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDDS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    DayOfInvoice = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDDS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceDDS_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceSimple",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    DayOfInvoice = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceSimple", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceSimple_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Visite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Staff = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddOn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OnMonth = table.Column<bool>(type: "bit", nullable: false),
                    Tax = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visite_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Visite_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisiteID = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConsumativeId = table.Column<int>(type: "int", nullable: true),
                    ConsCount = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    isVisited = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deliveries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deliveries_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deliveries_Consumative_ConsumativeId",
                        column: x => x.ConsumativeId,
                        principalTable: "Consumative",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProcessedService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisiteID = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtraServiceID = table.Column<int>(type: "int", nullable: true),
                    ExtraCount = table.Column<int>(type: "int", nullable: true),
                    IsVisited = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessedService_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessedService_ExtraService_ExtraServiceID",
                        column: x => x.ExtraServiceID,
                        principalTable: "ExtraService",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AddOns",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "---" },
                    { 2, "AddOn1" }
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "Address", "City", "Dds", "Ein", "Email", "Mol", "Name", "OnMonth", "Recipient", "Tax", "isActive" },
                values: new object[,]
                {
                    { 1, "", "", false, "", "", "", "---", false, "", 0.0, 1 },
                    { 2, "", "", false, "", "", "", "Client1", false, "", 0.0, 1 }
                });

            migrationBuilder.InsertData(
                table: "Consumative",
                columns: new[] { "Id", "Name", "Price", "date", "isActive" },
                values: new object[,]
                {
                    { 1, "Consumative1", 6.7999999999999998, new DateTime(2025, 6, 14, 20, 44, 14, 406, DateTimeKind.Local).AddTicks(1701), 1 },
                    { 2, "Consumative2", 6.7999999999999998, new DateTime(2025, 6, 14, 20, 44, 14, 406, DateTimeKind.Local).AddTicks(1785), 1 }
                });

            migrationBuilder.InsertData(
                table: "ExtraService",
                columns: new[] { "Id", "ExtraPrice", "Name", "date", "isActive" },
                values: new object[,]
                {
                    { 1, 20.0, "ExtraService1", new DateTime(2025, 6, 14, 20, 44, 14, 406, DateTimeKind.Local).AddTicks(1901), 1 },
                    { 2, 20.0, "ExtraService2", new DateTime(2025, 6, 14, 20, 44, 14, 406, DateTimeKind.Local).AddTicks(1913), 1 }
                });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "Id", "Name", "date", "isActive" },
                values: new object[,]
                {
                    { 1, "---", new DateTime(2025, 6, 14, 20, 44, 14, 406, DateTimeKind.Local).AddTicks(1826), 1 },
                    { 2, "Staff1", new DateTime(2025, 6, 14, 20, 44, 14, 406, DateTimeKind.Local).AddTicks(1830), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_ClientId",
                table: "Deliveries",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_ConsumativeId",
                table: "Deliveries",
                column: "ConsumativeId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_UserId",
                table: "Deliveries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ForDelivery_ClientId",
                table: "ForDelivery",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDDS_ClientId",
                table: "InvoiceDDS",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSimple_ClientId",
                table: "InvoiceSimple",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedService_ClientId",
                table: "ProcessedService",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedService_ExtraServiceID",
                table: "ProcessedService",
                column: "ExtraServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_Visite_ClientId",
                table: "Visite",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Visite_UserId",
                table: "Visite",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddOns");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "ForDelivery");

            migrationBuilder.DropTable(
                name: "InvoiceDDS");

            migrationBuilder.DropTable(
                name: "InvoiceSimple");

            migrationBuilder.DropTable(
                name: "ProcessedService");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "Visite");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Consumative");

            migrationBuilder.DropTable(
                name: "ExtraService");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Client");
        }
    }
}
