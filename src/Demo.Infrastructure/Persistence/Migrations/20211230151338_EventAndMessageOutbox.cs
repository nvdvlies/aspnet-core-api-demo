using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Demo.Infrastructure.Persistence.Migrations
{
    public partial class EventAndMessageOutbox : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventOutbox",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Event = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LockedUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LockToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventOutbox", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageOutbox",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LockedUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LockToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSent = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageOutbox", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventOutbox_IsPublished",
                schema: "demo",
                table: "EventOutbox",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_EventOutbox_LockedUntil",
                schema: "demo",
                table: "EventOutbox",
                column: "LockedUntil");

            migrationBuilder.CreateIndex(
                name: "IX_MessageOutbox_IsSent",
                schema: "demo",
                table: "MessageOutbox",
                column: "IsSent");

            migrationBuilder.CreateIndex(
                name: "IX_MessageOutbox_LockedUntil",
                schema: "demo",
                table: "MessageOutbox",
                column: "LockedUntil");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventOutbox",
                schema: "demo");

            migrationBuilder.DropTable(
                name: "MessageOutbox",
                schema: "demo");
        }
    }
}
