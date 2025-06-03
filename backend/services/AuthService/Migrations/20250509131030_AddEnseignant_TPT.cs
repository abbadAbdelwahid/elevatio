using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class AddEnseignant_TPT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Enseignants",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Grade = table.Column<string>(type: "text", nullable: true),
                    Specialite = table.Column<string>(type: "text", nullable: true),
                    DateEmbauche = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enseignants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enseignants_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enseignants");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
