using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaWeb.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Obras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", nullable: false),
                    Autor = table.Column<string>(type: "TEXT", nullable: false),
                    FotoUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    Modalidade = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecoCompra = table.Column<decimal>(type: "TEXT", nullable: false),
                    PrecoAluguel = table.Column<decimal>(type: "TEXT", nullable: false),
                    LojaOuPlataforma = table.Column<string>(type: "TEXT", nullable: false),
                    ContadorPesquisas = table.Column<int>(type: "INTEGER", nullable: false),
                    ContadorVendas = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obras", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Obras");
        }
    }
}
