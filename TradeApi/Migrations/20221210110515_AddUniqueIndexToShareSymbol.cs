using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToShareSymbol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Shares_Symbol",
                table: "Shares",
                column: "Symbol",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Shares_Symbol",
                table: "Shares");
        }
    }
}
