using System;
using System.Collections.Generic;
using Demo.Domain.ApplicationSettings;
using Demo.Domain.Auditlog;
using Demo.Domain.FeatureFlagSettings;
using Demo.Domain.UserPreferences;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.Infrastructure.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "demo");

            migrationBuilder.CreateSequence<int>(
                name: "CustomerCode",
                schema: "demo",
                minValue: 1L,
                maxValue: 9999999L);

            migrationBuilder.CreateSequence<int>(
                name: "InvoiceNumber",
                schema: "demo",
                startValue: 100000L,
                minValue: 100000L,
                maxValue: 999999L,
                cyclic: true);

            migrationBuilder.CreateTable(
                name: "ApplicationSettings",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Settings = table.Column<ApplicationSettingsSettings>(type: "jsonb", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", maxLength: 64, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", maxLength: 64, nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auditlog",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", maxLength: 64, nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AuditlogItems = table.Column<List<AuditlogItem>>(type: "jsonb", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditlog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeatureFlagSettings",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Settings = table.Column<FeatureFlagSettingsSettings>(type: "jsonb", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureFlagSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    StreetName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HouseNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CountryCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxEvent",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Event = table.Column<string>(type: "jsonb", nullable: true),
                    LockedUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LockToken = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxEvent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessage",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Message = table.Column<string>(type: "jsonb", nullable: true),
                    LockedUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LockToken = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsSent = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGroup",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Fullname = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    GivenName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FamilyName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: true),
                    UserType = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<int>(type: "integer", maxLength: 10, nullable: false, defaultValueSql: "nextval('demo.\"CustomerCode\"')"),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    InvoiceEmailAddress = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", maxLength: 64, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", maxLength: 64, nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Location_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "demo",
                        principalTable: "Location",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PermissionGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permission_PermissionGroup_PermissionGroupId",
                        column: x => x.PermissionGroupId,
                        principalSchema: "demo",
                        principalTable: "PermissionGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Preferences = table.Column<UserPreferencesPreferences>(type: "jsonb", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPreferences_User_Id",
                        column: x => x.Id,
                        principalSchema: "demo",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "demo",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "demo",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "demo",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValueSql: "CONCAT(date_part('year', current_date), nextval('demo.\"InvoiceNumber\"'))"),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "date", nullable: false),
                    PaymentTerm = table.Column<int>(type: "integer", nullable: false),
                    OrderReference = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PdfIsSynced = table.Column<bool>(type: "boolean", nullable: false),
                    PdfChecksum = table.Column<string>(type: "varchar", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", maxLength: 64, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", maxLength: 64, nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoice_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "demo",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                schema: "demo",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "demo",
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "demo",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceLine",
                schema: "demo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LineNumber = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SellingPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceLine_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "demo",
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "demo",
                table: "PermissionGroup",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("4b4e2d70-02dc-43ac-a8bc-c75c25e1e71d"), "Invoices" },
                    { new Guid("6fd39917-5f96-472d-ac69-d2a8c56880b7"), "FeatureFlagSettings" },
                    { new Guid("7af7d630-e85d-4183-966f-a2cf4a3d67f0"), "Roles" },
                    { new Guid("9b621e5b-e277-4c88-88d2-18a7befb45aa"), "Users" },
                    { new Guid("bce43d29-e527-4364-890a-0a49224abf74"), "Customers" },
                    { new Guid("d3fda6f7-4a23-4a2c-bfcf-abd0aec25774"), "ApplicationSettings" }
                });

            migrationBuilder.InsertData(
                schema: "demo",
                table: "Permission",
                columns: new[] { "Id", "Name", "PermissionGroupId" },
                values: new object[,]
                {
                    { new Guid("286864f7-1c3c-4ca5-9ae0-5efe8b56bf5e"), "Users:Read", new Guid("9b621e5b-e277-4c88-88d2-18a7befb45aa") },
                    { new Guid("29ece69e-c315-4902-b959-82790a38dc8a"), "ApplicationSettings:Write", new Guid("d3fda6f7-4a23-4a2c-bfcf-abd0aec25774") },
                    { new Guid("537d9994-517f-490b-8c7e-5da886e80d44"), "FeatureFlagSettings:Read", new Guid("6fd39917-5f96-472d-ac69-d2a8c56880b7") },
                    { new Guid("692a27f6-c217-4bfe-a210-8ab19d809199"), "Roles:Write", new Guid("7af7d630-e85d-4183-966f-a2cf4a3d67f0") },
                    { new Guid("7baf877f-dda2-4940-b7e5-38274fe7f28b"), "Roles:Read", new Guid("7af7d630-e85d-4183-966f-a2cf4a3d67f0") },
                    { new Guid("7f77e408-04ce-496c-b347-bac63b0bc870"), "FeatureFlagSettings:Write", new Guid("6fd39917-5f96-472d-ac69-d2a8c56880b7") },
                    { new Guid("931d572a-f85b-4dbb-a32d-8fee11e0e28d"), "Invoices:Read", new Guid("4b4e2d70-02dc-43ac-a8bc-c75c25e1e71d") },
                    { new Guid("b274daec-0a76-4bcc-b268-09768517d265"), "Users:Write", new Guid("9b621e5b-e277-4c88-88d2-18a7befb45aa") },
                    { new Guid("bdd6b139-be77-4302-b80e-c1bce405ada5"), "Customers:Read", new Guid("bce43d29-e527-4364-890a-0a49224abf74") },
                    { new Guid("c97b9d9d-6611-4a26-a1b4-43708402a49a"), "Customers:Write", new Guid("bce43d29-e527-4364-890a-0a49224abf74") },
                    { new Guid("d5d6786c-ca5d-476e-b7a9-ccf67422b98d"), "Invoices:Write", new Guid("4b4e2d70-02dc-43ac-a8bc-c75c25e1e71d") },
                    { new Guid("db7b21d3-41ee-44d2-8218-78ef78f262d3"), "ApplicationSettings:Read", new Guid("d3fda6f7-4a23-4a2c-bfcf-abd0aec25774") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditlog_EntityId",
                schema: "demo",
                table: "Auditlog",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Auditlog_EntityName",
                schema: "demo",
                table: "Auditlog",
                column: "EntityName");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_AddressId",
                schema: "demo",
                table: "Customer",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Code",
                schema: "demo",
                table: "Customer",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Deleted",
                schema: "demo",
                table: "Customer",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Name",
                schema: "demo",
                table: "Customer",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_CustomerId",
                schema: "demo",
                table: "Invoice",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_Deleted",
                schema: "demo",
                table: "Invoice",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_InvoiceDate",
                schema: "demo",
                table: "Invoice",
                column: "InvoiceDate");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_InvoiceNumber",
                schema: "demo",
                table: "Invoice",
                column: "InvoiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_Status",
                schema: "demo",
                table: "Invoice",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLine_InvoiceId",
                schema: "demo",
                table: "InvoiceLine",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxEvent_IsPublished",
                schema: "demo",
                table: "OutboxEvent",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxEvent_LockedUntil",
                schema: "demo",
                table: "OutboxEvent",
                column: "LockedUntil");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_IsSent",
                schema: "demo",
                table: "OutboxMessage",
                column: "IsSent");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_LockedUntil",
                schema: "demo",
                table: "OutboxMessage",
                column: "LockedUntil");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Name",
                schema: "demo",
                table: "Permission",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permission_PermissionGroupId",
                schema: "demo",
                table: "Permission",
                column: "PermissionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroup_Name",
                schema: "demo",
                table: "PermissionGroup",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                schema: "demo",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                schema: "demo",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "demo",
                table: "User",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_User_Fullname",
                schema: "demo",
                table: "User",
                column: "Fullname");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                schema: "demo",
                table: "UserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationSettings",
                schema: "demo");

            migrationBuilder.DropTable(
                name: "Auditlog",
                schema: "demo");

            migrationBuilder.DropTable(
                name: "FeatureFlagSettings",
                schema: "demo");

            migrationBuilder.DropTable(
                name: "InvoiceLine",
                schema: "demo");

            migrationBuilder.DropTable(
                name: "OutboxEvent",
                schema: "demo");

            migrationBuilder.DropTable(
                name: "OutboxMessage",
                schema: "demo");

            migrationBuilder.DropTable(
                name: "RolePermission",
                schema: "demo");

            migrationBuilder.DropTable(
                name: "UserPreferences",
                schema: "demo");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "demo");

            migrationBuilder.DropTable(
                name: "Invoice",
                schema: "demo");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "demo");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "demo");

            migrationBuilder.DropTable(
                name: "User",
                schema: "demo");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "demo");

            migrationBuilder.DropTable(
                name: "PermissionGroup",
                schema: "demo");

            migrationBuilder.DropTable(
                name: "Location",
                schema: "demo");

            migrationBuilder.DropSequence(
                name: "CustomerCode",
                schema: "demo");

            migrationBuilder.DropSequence(
                name: "InvoiceNumber",
                schema: "demo");
        }
    }
}
