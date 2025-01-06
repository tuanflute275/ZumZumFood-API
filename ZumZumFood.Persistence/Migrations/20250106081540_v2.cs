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
            migrationBuilder.AlterColumn<int>(
                name: "Percent",
                table: "Coupons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "Coupons",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Coupons",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleteBy",
                table: "Coupons",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "Coupons",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeleteFlag",
                table: "Coupons",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateBy",
                table: "Coupons",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Coupons",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "DeleteBy",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "DeleteFlag",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Coupons");

            migrationBuilder.AlterColumn<int>(
                name: "Percent",
                table: "Coupons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
