
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class FeedbackMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                        name: "feedbacks",
                        columns: table => new
                        {
                            feedback_id = table.Column<Guid>(type: "uuid", nullable: false),
                            deal_id = table.Column<Guid>(type: "uuid", nullable: false),
                            sender_id = table.Column<Guid>(type: "uuid", nullable: false),
                            recipient_id = table.Column<Guid>(type: "uuid", nullable: false),
                            text = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                            stars = table.Column<int>(type: "integer", nullable: false),
                            created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                            updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                            deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_feedbacks", x => x.feedback_id);
                            table.ForeignKey(
                                name: "FK_feedbacks_users_recipient_id",
                                column: x => x.recipient_id,
                                principalTable: "users",
                                principalColumn: "user_id",
                                onDelete: ReferentialAction.Restrict);
                            table.ForeignKey(
                                name: "FK_feedbacks_users_sender_id",
                                column: x => x.sender_id,
                                principalTable: "users",
                                principalColumn: "user_id",
                                onDelete: ReferentialAction.Restrict);
                            table.ForeignKey(
                                name: "FK_feedbacks_deal_id",
                                column: x => x.deal_id,
                                principalTable: "deals",
                                principalColumn: "deal_id",
                                onDelete: ReferentialAction.Restrict
                            );
                        });

            migrationBuilder.CreateIndex(
                name: "IX_feedbacks_recipient_id",
                table: "feedbacks",
                column: "recipient_id");

            migrationBuilder.CreateIndex(
                name: "IX_feedbacks_sender_id",
                table: "feedbacks",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_deals_deal_id",
                table: "deals",
                column: "deal_id");
        }
    }
}