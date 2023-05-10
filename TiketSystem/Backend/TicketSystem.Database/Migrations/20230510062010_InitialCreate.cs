using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "task_seq");

            migrationBuilder.CreateSequence(
                name: "task_status_seq");

            migrationBuilder.CreateSequence(
                name: "user_seq");

            migrationBuilder.CreateTable(
                name: "task_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('task_status_seq'::regclass)"),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("task_status_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_role_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('user_seq'::regclass)"),
                    user_name = table.Column<string>(type: "text", nullable: true),
                    user_password = table.Column<string>(type: "text", nullable: true),
                    user_patronymic = table.Column<string>(type: "text", nullable: true),
                    user_surname = table.Column<string>(type: "text", nullable: true),
                    user_first_name = table.Column<string>(type: "text", nullable: true),
                    user_role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pk", x => x.user_id);
                    table.ForeignKey(
                        name: "user_fk",
                        column: x => x.user_role_id,
                        principalTable: "user_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('task_seq'::regclass)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    task_status_id = table.Column<int>(type: "integer", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    cabinet = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("task_pk", x => x.id);
                    table.ForeignKey(
                        name: "task_task_status_fk",
                        column: x => x.task_status_id,
                        principalTable: "task_status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "task_user_fk",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_task_task_status_id",
                table: "task",
                column: "task_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_user_id",
                table: "task",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_user_role_id",
                table: "user",
                column: "user_role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "task");

            migrationBuilder.DropTable(
                name: "task_status");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "user_role");

            migrationBuilder.DropSequence(
                name: "task_seq");

            migrationBuilder.DropSequence(
                name: "task_status_seq");

            migrationBuilder.DropSequence(
                name: "user_seq");
        }
    }
}
