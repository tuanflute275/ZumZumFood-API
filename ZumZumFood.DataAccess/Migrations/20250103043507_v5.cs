using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZumZumFood.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banner_Restaurants_RestaurantId",
                table: "Banner");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Banner",
                table: "Banner");

            migrationBuilder.RenameTable(
                name: "Banner",
                newName: "Banners");

            migrationBuilder.RenameIndex(
                name: "IX_Banner_RestaurantId",
                table: "Banners",
                newName: "IX_Banners_RestaurantId");

            migrationBuilder.AlterColumn<bool>(
                name: "UserAccessibleFlag",
                table: "Parameters",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "AdminAccessibleFlag",
                table: "Parameters",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Banners",
                table: "Banners",
                column: "BannerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Banners_Restaurants_RestaurantId",
                table: "Banners",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "RestaurantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banners_Restaurants_RestaurantId",
                table: "Banners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Banners",
                table: "Banners");

            migrationBuilder.RenameTable(
                name: "Banners",
                newName: "Banner");

            migrationBuilder.RenameIndex(
                name: "IX_Banners_RestaurantId",
                table: "Banner",
                newName: "IX_Banner_RestaurantId");

            migrationBuilder.AlterColumn<bool>(
                name: "UserAccessibleFlag",
                table: "Parameters",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AdminAccessibleFlag",
                table: "Parameters",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Banner",
                table: "Banner",
                column: "BannerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Banner_Restaurants_RestaurantId",
                table: "Banner",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "RestaurantId");
        }
    }
}
