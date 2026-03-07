using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompraProgramada.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCreateTableT_COTACAO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_COTACAO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TICKER = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PRECO_FECHAMENTO = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PRECO_ABERTURA = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PRECO_MAXIMO = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PRECO_MINIMO = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DATA_PREGAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CODIGO_BDI = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TIPO_MERCADO = table.Column<int>(type: "int", nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_COTACAO", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_COTACAO_TICKER_DATA",
                table: "T_COTACAO",
                columns: new[] { "TICKER", "DATA_PREGAO" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_COTACAO");
        }
    }
}
