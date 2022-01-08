using Microsoft.EntityFrameworkCore.Migrations;

namespace Demo.Infrastructure.Persistence.Migrations
{
    public partial class Added_InvoiceLine_LineNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LineNumber",
                schema: "demo",
                table: "InvoiceLine",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LineNumber",
                schema: "demo",
                table: "InvoiceLine");
        }
    }
}
