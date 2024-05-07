using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetReplacement.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeReplaceToAssetRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeReplace",
                table: "AssetRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeReplace",
                table: "AssetRequests");
        }
    }
}
