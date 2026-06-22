using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddLastDailyBonus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Coins",
                table: "Pets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Experience",
                table: "Pets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAlive",
                table: "Pets",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastDailyBonus",
                table: "Pets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Pets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ShopItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Effect = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<int>(type: "INTEGER", nullable: false),
                    HungerEffect = table.Column<int>(type: "INTEGER", nullable: false),
                    HappinessEffect = table.Column<int>(type: "INTEGER", nullable: false),
                    HealthEffect = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItems", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Coins", "Experience", "IsAlive", "LastDailyBonus", "Level" },
                values: new object[] { 100, 0, true, null, 1 });

            migrationBuilder.UpdateData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Coins", "Experience", "IsAlive", "LastDailyBonus", "Level" },
                values: new object[] { 100, 0, true, null, 1 });

            migrationBuilder.UpdateData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Coins", "Experience", "IsAlive", "LastDailyBonus", "Level" },
                values: new object[] { 100, 0, true, null, 1 });

            migrationBuilder.InsertData(
                table: "ShopItems",
                columns: new[] { "Id", "Effect", "HappinessEffect", "HealthEffect", "HungerEffect", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Їжа", 5, 0, -30, "Риба", 10 },
                    { 2, "Їжа", 10, 0, -50, "М'ясо", 20 },
                    { 3, "Їжа", 20, -5, -10, "Цукерка", 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopItems");

            migrationBuilder.DropColumn(
                name: "Coins",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Experience",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "IsAlive",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "LastDailyBonus",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Pets");
        }
    }
}
