using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZumZumFood.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Codes",
                columns: table => new
                {
                    CodeId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CodeDes = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteFlag = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codes", x => x.CodeId);
                });

            migrationBuilder.CreateTable(
                name: "CodeValues",
                columns: table => new
                {
                    CodeId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CodeValue = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CodeValueDes = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    CodeValueDes1 = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    CodeValueDes2 = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    CodeValueDes3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteFlag = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeValues", x => x.CodeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeValues_CodeValue",
                table: "CodeValues",
                column: "CodeValue",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Codes");

            migrationBuilder.DropTable(
                name: "CodeValues");
        }
    }
}
