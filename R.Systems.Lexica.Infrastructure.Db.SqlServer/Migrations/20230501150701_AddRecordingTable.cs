using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddRecordingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "recording",
                columns: table => new
                {
                    recording_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    word = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    word_type_id = table.Column<int>(type: "int", nullable: false),
                    file_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recording", x => x.recording_id);
                    table.ForeignKey(
                        name: "FK_recording_word_type_word_type_id",
                        column: x => x.word_type_id,
                        principalTable: "word_type",
                        principalColumn: "word_type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_recording_word_type_id",
                table: "recording",
                column: "word_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "recording");
        }
    }
}
