using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etiquetas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BloquearExclusaoProdutoComProducoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producoes_Produtos_ProdutoId",
                table: "Producoes");

            migrationBuilder.AddForeignKey(
                name: "FK_Producoes_Produtos_ProdutoId",
                table: "Producoes",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producoes_Produtos_ProdutoId",
                table: "Producoes");

            migrationBuilder.AddForeignKey(
                name: "FK_Producoes_Produtos_ProdutoId",
                table: "Producoes",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
