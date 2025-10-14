using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfResearch.Financial.API.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletForeingKeyToPropagatedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_wallets_user_id",
                table: "wallets",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_wallets_prp_users_user_id",
                table: "wallets",
                column: "user_id",
                principalTable: "prp_users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_wallets_prp_users_user_id",
                table: "wallets");

            migrationBuilder.DropIndex(
                name: "IX_wallets_user_id",
                table: "wallets");
        }
    }
}
