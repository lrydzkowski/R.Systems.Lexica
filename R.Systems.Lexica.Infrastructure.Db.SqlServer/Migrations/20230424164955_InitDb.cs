using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "set",
                columns: table => new
                {
                    set_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    created_at_utc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_set", x => x.set_id);
                });

            migrationBuilder.CreateTable(
                name: "translation",
                columns: table => new
                {
                    translation_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    translation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_translation", x => x.translation_id);
                });

            migrationBuilder.CreateTable(
                name: "word_type",
                columns: table => new
                {
                    word_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_word_type", x => x.word_type_id);
                });

            migrationBuilder.CreateTable(
                name: "word",
                columns: table => new
                {
                    word_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    word = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    word_type_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_word", x => x.word_id);
                    table.ForeignKey(
                        name: "FK_word_word_type_word_type_id",
                        column: x => x.word_type_id,
                        principalTable: "word_type",
                        principalColumn: "word_type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "set_word",
                columns: table => new
                {
                    set_id = table.Column<long>(type: "bigint", nullable: false),
                    word_id = table.Column<long>(type: "bigint", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_set_word", x => new { x.set_id, x.word_id });
                    table.ForeignKey(
                        name: "FK_set_word_set_set_id",
                        column: x => x.set_id,
                        principalTable: "set",
                        principalColumn: "set_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_set_word_word_word_id",
                        column: x => x.word_id,
                        principalTable: "word",
                        principalColumn: "word_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "word_translation",
                columns: table => new
                {
                    translation_id = table.Column<long>(type: "bigint", nullable: false),
                    word_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_word_translation", x => new { x.translation_id, x.word_id });
                    table.ForeignKey(
                        name: "FK_word_translation_translation_translation_id",
                        column: x => x.translation_id,
                        principalTable: "translation",
                        principalColumn: "translation_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_word_translation_word_word_id",
                        column: x => x.word_id,
                        principalTable: "word",
                        principalColumn: "word_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "word_type",
                columns: new[] { "word_type_id", "name" },
                values: new object[,]
                {
                    { 1, "None" },
                    { 2, "Noun" },
                    { 3, "Verb" },
                    { 4, "Adjective" },
                    { 5, "Adverb" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_set_name",
                table: "set",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_set_word_word_id",
                table: "set_word",
                column: "word_id");

            migrationBuilder.CreateIndex(
                name: "IX_word_word_type_id",
                table: "word",
                column: "word_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_word_translation_word_id",
                table: "word_translation",
                column: "word_id");

            migrationBuilder.CreateIndex(
                name: "IX_word_type_name",
                table: "word_type",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "set_word");

            migrationBuilder.DropTable(
                name: "word_translation");

            migrationBuilder.DropTable(
                name: "set");

            migrationBuilder.DropTable(
                name: "translation");

            migrationBuilder.DropTable(
                name: "word");

            migrationBuilder.DropTable(
                name: "word_type");
        }
    }
}
