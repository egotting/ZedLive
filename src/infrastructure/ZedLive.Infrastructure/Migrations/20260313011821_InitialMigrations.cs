using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ZedLive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "tb_status_user",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<byte>(type: "smallint", nullable: false),
                    value = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_status_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_usuarios",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "varchar", maxLength: 266, nullable: false),
                    password = table.Column<string>(type: "varchar", nullable: false),
                    salt = table.Column<byte[]>(type: "bytea", nullable: false),
                    status_user_id = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)1),
                    stream_key = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LoggedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_usuarios_tb_status_user_status_user_id",
                        column: x => x.status_user_id,
                        principalSchema: "dbo",
                        principalTable: "tb_status_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "tb_status_user",
                columns: new[] { "id", "value" },
                values: new object[,]
                {
                    { (byte)1, "Online" },
                    { (byte)2, "Offline" },
                    { (byte)3, "Inactive" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_usuarios_email",
                schema: "dbo",
                table: "tb_usuarios",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_usuarios_Login",
                schema: "dbo",
                table: "tb_usuarios",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_usuarios_status_user_id",
                schema: "dbo",
                table: "tb_usuarios",
                column: "status_user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_usuarios_stream_key",
                schema: "dbo",
                table: "tb_usuarios",
                column: "stream_key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_usuarios",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tb_status_user",
                schema: "dbo");
        }
    }
}
