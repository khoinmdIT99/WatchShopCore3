using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraniteHouse.Data.Migrations
{
    public partial class bigUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalQuantity",
                table: "ProductsSelectedForAppointment",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "productName",
                table: "ProductsSelectedForAppointment",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "ProductsSelectedForAppointment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Products",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhoneNumber",
                table: "Appointments",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerAddress",
                table: "Appointments",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAppointment",
                table: "Appointments",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "isCompleted",
                table: "Appointments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CusPhone = table.Column<string>(nullable: false),
                    CusName = table.Column<string>(nullable: true),
                    CusEmail = table.Column<string>(nullable: true),
                    CusAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CusPhone);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CustomerPhoneNumber",
                table: "Appointments",
                column: "CustomerPhoneNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Customer_CustomerPhoneNumber",
                table: "Appointments",
                column: "CustomerPhoneNumber",
                principalTable: "Customer",
                principalColumn: "CusPhone",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Customer_CustomerPhoneNumber",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_CustomerPhoneNumber",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "TotalQuantity",
                table: "ProductsSelectedForAppointment");

            migrationBuilder.DropColumn(
                name: "productName",
                table: "ProductsSelectedForAppointment");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "ProductsSelectedForAppointment");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CustomerAddress",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "TotalAppointment",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "isCompleted",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhoneNumber",
                table: "Appointments",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
