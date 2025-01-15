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
            migrationBuilder.RenameColumn(
                name: "workTation",
                table: "Logs",
                newName: "WorkTation");

            migrationBuilder.RenameColumn(
                name: "userName",
                table: "Logs",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "timeLogout",
                table: "Logs",
                newName: "TimeLogout");

            migrationBuilder.RenameColumn(
                name: "timeLogin",
                table: "Logs",
                newName: "TimeLogin");

            migrationBuilder.RenameColumn(
                name: "timeActionRequest",
                table: "Logs",
                newName: "TimeActionRequest");

            migrationBuilder.RenameColumn(
                name: "response",
                table: "Logs",
                newName: "Response");

            migrationBuilder.RenameColumn(
                name: "request",
                table: "Logs",
                newName: "Request");

            migrationBuilder.RenameColumn(
                name: "ipAdress",
                table: "Logs",
                newName: "IpAdress");

            migrationBuilder.AddColumn<string>(
                name: "KeyApi",
                table: "Logs",
                type: "nvarchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Logs",
                type: "nvarchar(255)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Codes",
                columns: table => new
                {
                    CodeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    CodeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodeValue = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_CodeValues", x => new { x.CodeId, x.CodeValue });
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    NameEN = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    NameZH = table.Column<string>(type: "nvarchar(100)", nullable: true),
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
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Codes");

            migrationBuilder.DropTable(
                name: "CodeValues");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropColumn(
                name: "KeyApi",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Logs");

            migrationBuilder.RenameColumn(
                name: "WorkTation",
                table: "Logs",
                newName: "workTation");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Logs",
                newName: "userName");

            migrationBuilder.RenameColumn(
                name: "TimeLogout",
                table: "Logs",
                newName: "timeLogout");

            migrationBuilder.RenameColumn(
                name: "TimeLogin",
                table: "Logs",
                newName: "timeLogin");

            migrationBuilder.RenameColumn(
                name: "TimeActionRequest",
                table: "Logs",
                newName: "timeActionRequest");

            migrationBuilder.RenameColumn(
                name: "Response",
                table: "Logs",
                newName: "response");

            migrationBuilder.RenameColumn(
                name: "Request",
                table: "Logs",
                newName: "request");

            migrationBuilder.RenameColumn(
                name: "IpAdress",
                table: "Logs",
                newName: "ipAdress");
        }
    }
}
