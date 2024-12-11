using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleRpgEntities.Migrations
{
    public partial class ManaFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Items_Inventories_InventoryId",
            //    table: "Items");

            //migrationBuilder.DropIndex(
            //    name: "IX_Items_InventoryId",
            //    table: "Items");

            //migrationBuilder.DropColumn(
            //    name: "InventoryId",
            //    table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "Mana",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Distance",
                table: "Abilities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefenseBonus",
                table: "Abilities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MagicAbility_Damage",
                table: "Abilities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManaCost",
                table: "Abilities",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mana",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "DefenseBonus",
                table: "Abilities");

            migrationBuilder.DropColumn(
                name: "MagicAbility_Damage",
                table: "Abilities");

            migrationBuilder.DropColumn(
                name: "ManaCost",
                table: "Abilities");

            //migrationBuilder.AddColumn<int>(
            //    name: "InventoryId",
            //    table: "Items",
            //    type: "int",
            //    nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Distance",
                table: "Abilities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Items_InventoryId",
            //    table: "Items",
            //    column: "InventoryId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Items_Inventories_InventoryId",
            //    table: "Items",
            //    column: "InventoryId",
            //    principalTable: "Inventories",
            //    principalColumn: "Id");
        }
    }
}
