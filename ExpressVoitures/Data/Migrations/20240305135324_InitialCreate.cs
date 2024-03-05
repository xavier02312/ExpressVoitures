using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressVoitures.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Annonces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdVoiture = table.Column<int>(type: "int", nullable: false),
                    TitreAnnonce = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAnnonce = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Annonces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Marges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Voitures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeVin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Annee = table.Column<int>(type: "int", nullable: false),
                    Marque = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modele = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Finition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAchat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrixAchat = table.Column<float>(type: "real", nullable: false),
                    DateDisponibiliteDeVente = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrixDeVente = table.Column<float>(type: "real", nullable: true),
                    DateDeVente = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voitures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoitureImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAnnonce = table.Column<int>(type: "int", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LienPhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnnonceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoitureImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoitureImages_Annonces_AnnonceId",
                        column: x => x.AnnonceId,
                        principalTable: "Annonces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reparations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdVoiture = table.Column<int>(type: "int", nullable: false),
                    TypeDeReparation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoutsReparation = table.Column<float>(type: "real", nullable: false),
                    VoitureId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reparations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reparations_Voitures_VoitureId",
                        column: x => x.VoitureId,
                        principalTable: "Voitures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reparations_VoitureId",
                table: "Reparations",
                column: "VoitureId");

            migrationBuilder.CreateIndex(
                name: "IX_VoitureImages_AnnonceId",
                table: "VoitureImages",
                column: "AnnonceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Marges");

            migrationBuilder.DropTable(
                name: "Reparations");

            migrationBuilder.DropTable(
                name: "VoitureImages");

            migrationBuilder.DropTable(
                name: "Voitures");

            migrationBuilder.DropTable(
                name: "Annonces");
        }
    }
}
