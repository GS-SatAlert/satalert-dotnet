using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SatAlert.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SAT_Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    Telefone = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    RegiaoInteresse = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Ativo = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAT_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SAT_Notificacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Mensagem = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: false),
                    DataEnvio = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Lida = table.Column<bool>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAT_Notificacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SAT_Notificacoes_SAT_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "SAT_Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SAT_Notificacoes_UsuarioId",
                table: "SAT_Notificacoes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_SAT_Usuarios_Email",
                table: "SAT_Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SAT_Notificacoes");

            migrationBuilder.DropTable(
                name: "SAT_Usuarios");
        }
    }
}
