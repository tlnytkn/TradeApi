using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAmountColumnShareTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Shares");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Shares",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
