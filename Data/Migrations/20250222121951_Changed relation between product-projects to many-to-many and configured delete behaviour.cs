using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Changedrelationbetweenproductprojectstomanytomanyandconfigureddeletebehaviour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Products_ProductId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProductId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Projects");

            migrationBuilder.CreateTable(
                name: "ProjectProduct",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectProduct", x => new { x.ProductId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_ProjectProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectProduct_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProduct_ProjectId",
                table: "ProjectProduct",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectProduct");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProductId",
                table: "Projects",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Products_ProductId",
                table: "Projects",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
