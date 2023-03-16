using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDealer.Migrations
{
    public partial class RenameColumnTraveledDistance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TravelledDistance",
                table: "Cars",
                newName: "TraveledDistance");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TraveledDistance",
                table: "Cars",
                newName: "TravelledDistance");
        }
    }
}
