using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MakiseSharpServer.API.Migrations
{
    public partial class NotificationAggregateRefactorToStatusProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Webhook_WebhookDiscordId = table.Column<ulong>(nullable: false),
                    Webhook_WebhookDiscordToken = table.Column<string>(nullable: false),
                    Webhook_ChannelId = table.Column<ulong>(nullable: false),
                    Webhook_GuildId = table.Column<ulong>(nullable: false),
                    StatusProvider = table.Column<string>(nullable: false),
                    Slug = table.Column<string>(nullable: false),
                    AuthorId = table.Column<Guid>(nullable: false),
                    LastBuildNumber = table.Column<uint>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Webhook_WebhookDiscordId",
                table: "Notifications",
                column: "Webhook_WebhookDiscordId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");
        }
    }
}
