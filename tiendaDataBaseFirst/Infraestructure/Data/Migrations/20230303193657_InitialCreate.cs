using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AlterDatabase()
            //     .Annotation("MySql:CharSet", "utf8mb4");

            // migrationBuilder.CreateTable(
            //     name: "Categoria",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //         Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
            //             .Annotation("MySql:CharSet", "utf8mb4")
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PRIMARY", x => x.Id);
            //     })
            //     .Annotation("MySql:CharSet", "utf8mb4")
            //     .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            // migrationBuilder.CreateTable(
            //     name: "Marca",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //         Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
            //             .Annotation("MySql:CharSet", "utf8mb4")
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PRIMARY", x => x.Id);
            //     })
            //     .Annotation("MySql:CharSet", "utf8mb4")
            //     .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            // migrationBuilder.CreateTable(
            //     name: "Producto",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //         Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
            //             .Annotation("MySql:CharSet", "utf8mb4"),
            //         Precio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
            //         FechaCreacion = table.Column<DateTime>(type: "datetime(6)", maxLength: 6, nullable: false),
            //         CategoriaId = table.Column<int>(type: "int", nullable: false),
            //         MarcaId = table.Column<int>(type: "int", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PRIMARY", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Producto_Categoria_CategoriaId",
            //             column: x => x.CategoriaId,
            //             principalTable: "Categoria",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_Producto_Marca_MarcaId",
            //             column: x => x.MarcaId,
            //             principalTable: "Marca",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     })
            //     .Annotation("MySql:CharSet", "utf8mb4")
            //     .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Producto_CategoriaId",
            //     table: "Producto",
            //     column: "CategoriaId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Producto_MarcaId",
            //     table: "Producto",
            //     column: "MarcaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropTable(
            //     name: "Producto");

            // migrationBuilder.DropTable(
            //     name: "Categoria");

            // migrationBuilder.DropTable(
            //     name: "Marca");
        }
    }
}
