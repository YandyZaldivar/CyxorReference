using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alimatic.Server.Alimatic.Pt.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Charges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Name = table.Column<string>(maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Completions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Completions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Criteria",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Name = table.Column<string>(maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criteria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Data = table.Column<byte[]>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Repetitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Value = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repetitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    ChargeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workers_Charges_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "Charges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubCriteria",
                columns: table => new
                {
                    CriterionId = table.Column<int>(nullable: false),
                    SubCriterionId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCriteria", x => new { x.CriterionId, x.SubCriterionId });
                    table.ForeignKey(
                        name: "FK_SubCriteria_Criteria_CriterionId",
                        column: x => x.CriterionId,
                        principalTable: "Criteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Description = table.Column<string>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    EndingDate = table.Column<DateTime>(nullable: true),
                    ListedDate = table.Column<DateTime>(nullable: false),
                    RepetitionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_Repetitions_RepetitionId",
                        column: x => x.RepetitionId,
                        principalTable: "Repetitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityCriterion",
                columns: table => new
                {
                    ActivityId = table.Column<int>(nullable: false),
                    CriterionId = table.Column<int>(nullable: false),
                    SubCriterionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityCriterion", x => new { x.ActivityId, x.CriterionId, x.SubCriterionId });
                    table.ForeignKey(
                        name: "FK_ActivityCriterion_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityCriterion_Criteria_CriterionId",
                        column: x => x.CriterionId,
                        principalTable: "Criteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityCriterion_SubCriteria_CriterionId_SubCriterionId",
                        columns: x => new { x.CriterionId, x.SubCriterionId },
                        principalTable: "SubCriteria",
                        principalColumns: new[] { "CriterionId", "SubCriterionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityDocument",
                columns: table => new
                {
                    ActivityId = table.Column<int>(nullable: false),
                    DocumentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDocument", x => new { x.ActivityId, x.DocumentId });
                    table.ForeignKey(
                        name: "FK_ActivityDocument_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityDocument_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerActivities",
                columns: table => new
                {
                    ActivityId = table.Column<int>(nullable: false),
                    WorkerId = table.Column<int>(nullable: false),
                    Observations = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerActivities", x => new { x.ActivityId, x.WorkerId });
                    table.ForeignKey(
                        name: "FK_WorkerActivities_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerActivities_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerActivityCompletions",
                columns: table => new
                {
                    WorkerActivityWorkerId = table.Column<int>(nullable: false),
                    WorkerActivityActivityId = table.Column<int>(nullable: false),
                    CompletionId = table.Column<int>(nullable: false),
                    Observations = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerActivityCompletions", x => new { x.WorkerActivityWorkerId, x.WorkerActivityActivityId, x.CompletionId });
                    table.ForeignKey(
                        name: "FK_WorkerActivityCompletions_Completions_CompletionId",
                        column: x => x.CompletionId,
                        principalTable: "Completions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerActivityCompletions_WorkerActivities_WorkerActivityActivityId_WorkerActivityWorkerId",
                        columns: x => new { x.WorkerActivityActivityId, x.WorkerActivityWorkerId },
                        principalTable: "WorkerActivities",
                        principalColumns: new[] { "ActivityId", "WorkerId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_RepetitionId",
                table: "Activities",
                column: "RepetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityCriterion_CriterionId_SubCriterionId",
                table: "ActivityCriterion",
                columns: new[] { "CriterionId", "SubCriterionId" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDocument_DocumentId",
                table: "ActivityDocument",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Charges_Name",
                table: "Charges",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workers_ChargeId",
                table: "Workers",
                column: "ChargeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkerActivities_WorkerId",
                table: "WorkerActivities",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerActivityCompletions_CompletionId",
                table: "WorkerActivityCompletions",
                column: "CompletionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerActivityCompletions_WorkerActivityActivityId_WorkerActivityWorkerId",
                table: "WorkerActivityCompletions",
                columns: new[] { "WorkerActivityActivityId", "WorkerActivityWorkerId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityCriterion");

            migrationBuilder.DropTable(
                name: "ActivityDocument");

            migrationBuilder.DropTable(
                name: "WorkerActivityCompletions");

            migrationBuilder.DropTable(
                name: "SubCriteria");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Completions");

            migrationBuilder.DropTable(
                name: "WorkerActivities");

            migrationBuilder.DropTable(
                name: "Criteria");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "Repetitions");

            migrationBuilder.DropTable(
                name: "Charges");
        }
    }
}
