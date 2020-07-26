using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraniteHouse.Data.Migrations
{
    public partial class BigUpdateFinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Customer_CustomerPhoneNumber",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsSelectedForAppointment",
                table: "ProductsSelectedForAppointment");

            migrationBuilder.DropIndex(
                name: "IX_ProductsSelectedForAppointment_AppointmentId",
                table: "ProductsSelectedForAppointment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductsSelectedForAppointment");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalQuantity",
                table: "ProductsSelectedForAppointment",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAppointment",
                table: "Appointments",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentCreatedDate",
                table: "Appointments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Appointments",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsSelectedForAppointment",
                table: "ProductsSelectedForAppointment",
                columns: new[] { "AppointmentId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "CusPhone");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Customers_CustomerPhoneNumber",
                table: "Appointments",
                column: "CustomerPhoneNumber",
                principalTable: "Customers",
                principalColumn: "CusPhone",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Customers_CustomerPhoneNumber",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsSelectedForAppointment",
                table: "ProductsSelectedForAppointment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "AppointmentCreatedDate",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Appointments");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalQuantity",
                table: "ProductsSelectedForAppointment",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductsSelectedForAppointment",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Products",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAppointment",
                table: "Appointments",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsSelectedForAppointment",
                table: "ProductsSelectedForAppointment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "CusPhone");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsSelectedForAppointment_AppointmentId",
                table: "ProductsSelectedForAppointment",
                column: "AppointmentId");

        }
    }
}
