using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class SafeInitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Создаем applications только если не существует
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN 
                    IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_schema = 'public' AND table_name = 'applications') THEN
                        CREATE TABLE applications (
                            application_id uuid NOT NULL,
                            user_id uuid NOT NULL,
                            description character varying(1000) NOT NULL,
                            cost numeric(18,2) NOT NULL,
                            status character varying(50) NOT NULL DEFAULT 'New',
                            created_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
                            updated_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
                            deleted_at timestamp with time zone,
                            CONSTRAINT ""PK_applications"" PRIMARY KEY (application_id)
                        );
                    END IF;
                END $$;
            ");

            // Таблицы users и deals уже существуют, поэтому их не создаем
            // Индексы тоже уже существуют, поэтому их не создаем
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS applications;");
        }
    }
}