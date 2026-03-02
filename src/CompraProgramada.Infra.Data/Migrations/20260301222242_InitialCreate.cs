using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompraProgramada.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_ACAO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CODIGO = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NOME_EMPRESA = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PRECO = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ATIVO = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ACAO", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_CESTA_RECOMENDACAO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ATIVA = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DATA_VIGENCIA = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CESTA_RECOMENDACAO", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_CLIENTE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NOME = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CPF = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EMAIL = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VALOR_APORTE_MENSAL = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ATIVO = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DATA_ADESAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DATA_SAIDA = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CLIENTE", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_CONTA_MASTER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DESCRICAO = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CONTA_MASTER", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_ORDEM_COMPRA",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DATA_EXECUCAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    VALOR_TOTAL = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    STATUS = table.Column<int>(type: "int", nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ORDEM_COMPRA", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_CESTA_RECOMENDACAO_ITEM",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CESTA_RECOMENDACAO_ID = table.Column<int>(type: "int", nullable: false),
                    ACAO_ID = table.Column<int>(type: "int", nullable: false),
                    PERCENTUAL = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CESTA_RECOMENDACAO_ITEM", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_CESTA_RECOMENDACAO_ITEM_T_ACAO_ACAO_ID",
                        column: x => x.ACAO_ID,
                        principalTable: "T_ACAO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_CESTA_RECOMENDACAO_ITEM_T_CESTA_RECOMENDACAO_CESTA_RECOMEN~",
                        column: x => x.CESTA_RECOMENDACAO_ID,
                        principalTable: "T_CESTA_RECOMENDACAO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_CONTA_GRAFICA",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CLIENTE_ID = table.Column<int>(type: "int", nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CONTA_GRAFICA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_CONTA_GRAFICA_T_CLIENTE_CLIENTE_ID",
                        column: x => x.CLIENTE_ID,
                        principalTable: "T_CLIENTE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_CUSTODIA_MASTER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CONTA_MASTER_ID = table.Column<int>(type: "int", nullable: false),
                    ACAO_ID = table.Column<int>(type: "int", nullable: false),
                    QUANTIDADE = table.Column<int>(type: "int", nullable: false),
                    PRECO_MEDIO = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CUSTODIA_MASTER", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_CUSTODIA_MASTER_T_ACAO_ACAO_ID",
                        column: x => x.ACAO_ID,
                        principalTable: "T_ACAO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_CUSTODIA_MASTER_T_CONTA_MASTER_CONTA_MASTER_ID",
                        column: x => x.CONTA_MASTER_ID,
                        principalTable: "T_CONTA_MASTER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_ORDEM_COMPRA_ITEM",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ORDEM_COMPRA_ID = table.Column<int>(type: "int", nullable: false),
                    ACAO_ID = table.Column<int>(type: "int", nullable: false),
                    QUANTIDADE = table.Column<int>(type: "int", nullable: false),
                    PRECO_UNITARIO = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    VALOR_TOTAL = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TIPO_MERCADO = table.Column<int>(type: "int", nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ORDEM_COMPRA_ITEM", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_ORDEM_COMPRA_ITEM_T_ACAO_ACAO_ID",
                        column: x => x.ACAO_ID,
                        principalTable: "T_ACAO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_ORDEM_COMPRA_ITEM_T_ORDEM_COMPRA_ORDEM_COMPRA_ID",
                        column: x => x.ORDEM_COMPRA_ID,
                        principalTable: "T_ORDEM_COMPRA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_CUSTODIA_FILHOTE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CONTA_GRAFICA_ID = table.Column<int>(type: "int", nullable: false),
                    ACAO_ID = table.Column<int>(type: "int", nullable: false),
                    QUANTIDADE = table.Column<int>(type: "int", nullable: false),
                    PRECO_MEDIO = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CUSTODIA_FILHOTE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_CUSTODIA_FILHOTE_T_ACAO_ACAO_ID",
                        column: x => x.ACAO_ID,
                        principalTable: "T_ACAO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_CUSTODIA_FILHOTE_T_CONTA_GRAFICA_CONTA_GRAFICA_ID",
                        column: x => x.CONTA_GRAFICA_ID,
                        principalTable: "T_CONTA_GRAFICA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_DISTRIBUICAO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ORDEM_COMPRA_ID = table.Column<int>(type: "int", nullable: false),
                    CONTA_GRAFICA_ID = table.Column<int>(type: "int", nullable: false),
                    ACAO_ID = table.Column<int>(type: "int", nullable: false),
                    QUANTIDADE = table.Column<int>(type: "int", nullable: false),
                    PRECO_UNITARIO = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    VALOR_IR_DEDO_DURO = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DATA_DISTRIBUICAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_DISTRIBUICAO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_DISTRIBUICAO_T_ACAO_ACAO_ID",
                        column: x => x.ACAO_ID,
                        principalTable: "T_ACAO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_DISTRIBUICAO_T_CONTA_GRAFICA_CONTA_GRAFICA_ID",
                        column: x => x.CONTA_GRAFICA_ID,
                        principalTable: "T_CONTA_GRAFICA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_DISTRIBUICAO_T_ORDEM_COMPRA_ORDEM_COMPRA_ID",
                        column: x => x.ORDEM_COMPRA_ID,
                        principalTable: "T_ORDEM_COMPRA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_T_CESTA_RECOMENDACAO_ITEM_ACAO_ID",
                table: "T_CESTA_RECOMENDACAO_ITEM",
                column: "ACAO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_CESTA_RECOMENDACAO_ITEM_CESTA_RECOMENDACAO_ID",
                table: "T_CESTA_RECOMENDACAO_ITEM",
                column: "CESTA_RECOMENDACAO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_CLIENTE_CPF",
                table: "T_CLIENTE",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_CONTA_GRAFICA_CLIENTE_ID",
                table: "T_CONTA_GRAFICA",
                column: "CLIENTE_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_CUSTODIA_FILHOTE_ACAO_ID",
                table: "T_CUSTODIA_FILHOTE",
                column: "ACAO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_CUSTODIA_FILHOTE_CONTA_GRAFICA_ID_ACAO_ID",
                table: "T_CUSTODIA_FILHOTE",
                columns: new[] { "CONTA_GRAFICA_ID", "ACAO_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_CUSTODIA_MASTER_ACAO_ID",
                table: "T_CUSTODIA_MASTER",
                column: "ACAO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_CUSTODIA_MASTER_CONTA_MASTER_ID_ACAO_ID",
                table: "T_CUSTODIA_MASTER",
                columns: new[] { "CONTA_MASTER_ID", "ACAO_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_DISTRIBUICAO_ACAO_ID",
                table: "T_DISTRIBUICAO",
                column: "ACAO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_DISTRIBUICAO_CONTA_GRAFICA_ID",
                table: "T_DISTRIBUICAO",
                column: "CONTA_GRAFICA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_DISTRIBUICAO_ORDEM_COMPRA_ID",
                table: "T_DISTRIBUICAO",
                column: "ORDEM_COMPRA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_ORDEM_COMPRA_ITEM_ACAO_ID",
                table: "T_ORDEM_COMPRA_ITEM",
                column: "ACAO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_ORDEM_COMPRA_ITEM_ORDEM_COMPRA_ID",
                table: "T_ORDEM_COMPRA_ITEM",
                column: "ORDEM_COMPRA_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_CESTA_RECOMENDACAO_ITEM");

            migrationBuilder.DropTable(
                name: "T_CUSTODIA_FILHOTE");

            migrationBuilder.DropTable(
                name: "T_CUSTODIA_MASTER");

            migrationBuilder.DropTable(
                name: "T_DISTRIBUICAO");

            migrationBuilder.DropTable(
                name: "T_ORDEM_COMPRA_ITEM");

            migrationBuilder.DropTable(
                name: "T_CESTA_RECOMENDACAO");

            migrationBuilder.DropTable(
                name: "T_CONTA_MASTER");

            migrationBuilder.DropTable(
                name: "T_CONTA_GRAFICA");

            migrationBuilder.DropTable(
                name: "T_ACAO");

            migrationBuilder.DropTable(
                name: "T_ORDEM_COMPRA");

            migrationBuilder.DropTable(
                name: "T_CLIENTE");
        }
    }
}
