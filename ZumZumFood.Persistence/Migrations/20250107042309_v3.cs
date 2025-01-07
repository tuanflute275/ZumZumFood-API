using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZumZumFood.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "Combos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Combos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleteBy",
                table: "Combos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "Combos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeleteFlag",
                table: "Combos",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateBy",
                table: "Combos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Combos",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "Combos");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Combos");

            migrationBuilder.DropColumn(
                name: "DeleteBy",
                table: "Combos");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "Combos");

            migrationBuilder.DropColumn(
                name: "DeleteFlag",
                table: "Combos");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "Combos");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Combos");
        }
    }
}
