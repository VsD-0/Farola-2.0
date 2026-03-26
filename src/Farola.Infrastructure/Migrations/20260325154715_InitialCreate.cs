using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Farola.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор роли")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, comment: "Наименование роли")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                },
                comment: "Справочник ролей пользователей");

            migrationBuilder.CreateTable(
                name: "specializations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор специализации")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Наименование специализации"),
                    photo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Фото специализации")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specializations", x => x.id);
                },
                comment: "Справочник специализаций");

            migrationBuilder.CreateTable(
                name: "statement_statuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор статуса заявления")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false, comment: "Наименование статуса")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statement_statuses", x => x.id);
                },
                comment: "Справочник статусов заявлений");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор пользователя")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<int>(type: "integer", nullable: false, comment: "Номер роли"),
                    surname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Фамилия пользователя"),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Имя пользователя"),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, comment: "Номер телефона"),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Электронная почта"),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Пароль"),
                    area = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Место работы"),
                    information = table.Column<string>(type: "text", nullable: true, comment: "Подробная информация"),
                    specialization_id = table.Column<int>(type: "integer", nullable: true, comment: "Номер специализации"),
                    photo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true, comment: "Имя фото"),
                    date_registration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()", comment: "Дата регистрации"),
                    profession = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Профессия"),
                    patronymic = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true, comment: "Отчество"),
                    is_closed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Статус профиля специалиста (открыт/закрыт)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_role",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_specialization",
                        column: x => x.specialization_id,
                        principalTable: "specializations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Таблица пользователей системы");

            migrationBuilder.CreateTable(
                name: "favorites",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    professional_id = table.Column<int>(type: "integer", nullable: false, comment: "Номер специалиста"),
                    client_id = table.Column<int>(type: "integer", nullable: false, comment: "Номер клиента")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorites", x => x.id);
                    table.ForeignKey(
                        name: "fk_favorite_client",
                        column: x => x.client_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_favorite_professional",
                        column: x => x.professional_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Таблица избранных специалистов");

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор токена")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор пользователя"),
                    token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Токен"),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Дата и время создания"),
                    expiresat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата и время истечения срока действия")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_token_user",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Токены обновления");

            migrationBuilder.CreateTable(
                name: "statements",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор заявления")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    professional_id = table.Column<int>(type: "integer", nullable: false, comment: "Номер специалиста"),
                    client_id = table.Column<int>(type: "integer", nullable: false, comment: "Номер клиента"),
                    status_id = table.Column<int>(type: "integer", nullable: false, comment: "Номер статуса заявления"),
                    date_added = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()", comment: "Дата создания"),
                    date_expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата закрытия заявки"),
                    grade = table.Column<float>(type: "real", nullable: true, comment: "Оценка специалиста на заказ"),
                    comment = table.Column<string>(type: "text", nullable: true, comment: "Комментарий специалиста")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statements", x => x.id);
                    table.ForeignKey(
                        name: "fk_statement_client",
                        column: x => x.client_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_statement_professional",
                        column: x => x.professional_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_statement_status",
                        column: x => x.status_id,
                        principalTable: "statement_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Таблица заявлений");

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, comment: "Идентификатор отзыва")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    statement_id = table.Column<int>(type: "integer", nullable: false, comment: "Номер заявления"),
                    grade = table.Column<float>(type: "real", nullable: false, comment: "Оценка работы"),
                    text = table.Column<string>(type: "text", nullable: true, comment: "Текст отзыва"),
                    date_added = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()", comment: "Дата добавления")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.id);
                    table.ForeignKey(
                        name: "fk_review_statement",
                        column: x => x.statement_id,
                        principalTable: "statements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Таблица отзывов клиентов");

            migrationBuilder.CreateIndex(
                name: "IX_favorites_client_id",
                table: "favorites",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_favorites_professional_client",
                table: "favorites",
                columns: new[] { "professional_id", "client_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_token",
                table: "refresh_tokens",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_userid",
                table: "refresh_tokens",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_statement_id",
                table: "reviews",
                column: "statement_id");

            migrationBuilder.CreateIndex(
                name: "ix_statements_client_id",
                table: "statements",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_statements_professional_id",
                table: "statements",
                column: "professional_id");

            migrationBuilder.CreateIndex(
                name: "ix_statements_status_id",
                table: "statements",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_phone",
                table: "users",
                column: "phone_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_specialization_id",
                table: "users",
                column: "specialization_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "favorites");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "statements");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "statement_statuses");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "specializations");
        }
    }
}
