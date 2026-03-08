using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZedLive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSaltUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StreamKey",
                schema: "dbo",
                table: "tb_usuarios",
                newName: "stream_key");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                schema: "dbo",
                table: "tb_usuarios",
                type: "character varying(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "stream_key",
                schema: "dbo",
                table: "tb_usuarios",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "salt",
                schema: "dbo",
                table: "tb_usuarios",
                type: "varchar",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

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
            migrationBuilder.DropIndex(
                name: "IX_tb_usuarios_stream_key",
                schema: "dbo",
                table: "tb_usuarios");

            migrationBuilder.DropColumn(
                name: "salt",
                schema: "dbo",
                table: "tb_usuarios");

            migrationBuilder.RenameColumn(
                name: "stream_key",
                schema: "dbo",
                table: "tb_usuarios",
                newName: "StreamKey");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                schema: "dbo",
                table: "tb_usuarios",
                type: "character varying(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StreamKey",
                schema: "dbo",
                table: "tb_usuarios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250);
        }
    }
}
