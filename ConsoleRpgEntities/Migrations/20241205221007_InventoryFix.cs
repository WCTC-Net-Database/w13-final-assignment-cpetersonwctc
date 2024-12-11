using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleRpgEntities.Migrations
{
    public partial class InventoryFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Players_PlayerId",
                table: "Inventory");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Items_Inventory_InventoryId",
            //    table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inventory",
                table: "Inventory");

            migrationBuilder.RenameTable(
                name: "Inventory",
                newName: "Inventories");

            migrationBuilder.RenameIndex(
                name: "IX_Inventory_PlayerId",
                table: "Inventories",
                newName: "IX_Inventories_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inventories",
                table: "Inventories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Players_PlayerId",
                table: "Inventories",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Items_Inventories_InventoryId",
            //    table: "Items",
            //    column: "InventoryId",
            //    principalTable: "Inventories",
            //    principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Players_PlayerId",
                table: "Inventories");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Items_Inventories_InventoryId",
            //    table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inventories",
                table: "Inventories");

            migrationBuilder.RenameTable(
                name: "Inventories",
                newName: "Inventory");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_PlayerId",
                table: "Inventory",
                newName: "IX_Inventory_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inventory",
                table: "Inventory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Players_PlayerId",
                table: "Inventory",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Items_Inventory_InventoryId",
            //    table: "Items",
            //    column: "InventoryId",
            //    principalTable: "Inventory",
            //    principalColumn: "Id");
        }
    }
}
