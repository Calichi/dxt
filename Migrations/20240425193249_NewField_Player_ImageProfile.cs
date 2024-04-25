using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dxt.Migrations
{
    /// <inheritdoc />
    public partial class NewField_Player_ImageProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageProfile",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageProfile",
                table: "Players");
        }
    }
}
