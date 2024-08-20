using System;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Viewer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankDocumentFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDocumentFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneratedRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EnglishString = table.Column<string>(type: "text", nullable: false),
                    RussianString = table.Column<string>(type: "text", nullable: false),
                    IntegerNumber = table.Column<int>(type: "integer", nullable: false),
                    FloatNumber = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneratedRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatisticsDto",
                columns: table => new
                {
                    Sum = table.Column<BigInteger>(type: "numeric", nullable: false),
                    Median = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "OperationClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    BankDocumentFileId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationClass_BankDocumentFiles_BankDocumentFileId",
                        column: x => x.BankDocumentFileId,
                        principalTable: "BankDocumentFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountingCode = table.Column<int>(type: "integer", nullable: false),
                    ActiveSaldoIn = table.Column<decimal>(type: "numeric", nullable: false),
                    PassiveSaldoIn = table.Column<decimal>(type: "numeric", nullable: false),
                    Debit = table.Column<decimal>(type: "numeric", nullable: false),
                    Credit = table.Column<decimal>(type: "numeric", nullable: false),
                    ActiveSaldoOut = table.Column<decimal>(type: "numeric", nullable: false),
                    PassiveSaldoOut = table.Column<decimal>(type: "numeric", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    BankDocumentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_BankDocumentFiles_BankDocumentId",
                        column: x => x.BankDocumentId,
                        principalTable: "BankDocumentFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operations_OperationClass_ClassId",
                        column: x => x.ClassId,
                        principalTable: "OperationClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperationClass_BankDocumentFileId",
                table: "OperationClass",
                column: "BankDocumentFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_BankDocumentId",
                table: "Operations",
                column: "BankDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_ClassId",
                table: "Operations",
                column: "ClassId");
            
            ///////////////////
            
            migrationBuilder.Sql(@"

            CREATE OR REPLACE FUNCTION CalculateSumAndMedian()
                RETURNS TABLE(IntegerSum BIGINT, FloatMedian DOUBLE PRECISION) AS $$
            DECLARE
                int_sum BIGINT;
                float_median_val DOUBLE PRECISION;
                record_count INT;
            BEGIN
                -- Calculate the sum of IntegerNumber
                SELECT SUM(""IntegerNumber"") INTO int_sum
                FROM ""GeneratedRecords"";

                -- Get the count of FloatNumber records
                SELECT COUNT(""FloatNumber"") INTO record_count
                FROM ""GeneratedRecords"";

                -- Calculate the median of FloatNumber
                IF record_count = 0 THEN
                    float_median_val := NULL;
                ELSIF record_count % 2 = 1 THEN
                    -- Odd count: take the middle value
                    SELECT ""FloatNumber""
                    INTO float_median_val
                    FROM ""GeneratedRecords""
                    ORDER BY ""FloatNumber""
                    LIMIT 1 OFFSET (record_count / 2);
                ELSE
                    -- Even count: take the average of the two middle values
                    SELECT AVG(m.""FloatNumber"")
                    INTO float_median_val
                    FROM (
                             SELECT ""FloatNumber""
                             FROM ""GeneratedRecords""
                             ORDER BY ""FloatNumber""
                             LIMIT 2 OFFSET (record_count / 2) - 1
                         ) AS m;
                END IF;

                -- Return the calculated values
                RETURN QUERY SELECT int_sum, float_median_val;
            END;
            $$ LANGUAGE plpgsql;
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneratedRecords");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "StatisticsDto");

            migrationBuilder.DropTable(
                name: "OperationClass");

            migrationBuilder.DropTable(
                name: "BankDocumentFiles");
            
            ///////////////////////
            
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS CalculateSumAndMedian();");
        }
    }
}
