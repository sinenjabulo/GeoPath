using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GeoPath.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCoreDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "definitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_definitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    Name = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "theorems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    Statement = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Explanation = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_theorems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "topics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Role = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastLoginAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_roles_Role",
                        column: x => x.Role,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "examples",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    Prompt = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: false),
                    Difficulty = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    GuidedFlowJson = table.Column<string>(type: "jsonb", nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_examples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_examples_topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "classes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    JoinCodeHash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_classes_users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "attempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LearnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExampleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    StartedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CompletedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_attempts_examples_ExampleId",
                        column: x => x.ExampleId,
                        principalTable: "examples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_attempts_users_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "example_definitions",
                columns: table => new
                {
                    ExampleId = table.Column<Guid>(type: "uuid", nullable: false),
                    DefinitionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_example_definitions", x => new { x.ExampleId, x.DefinitionId });
                    table.ForeignKey(
                        name: "FK_example_definitions_definitions_DefinitionId",
                        column: x => x.DefinitionId,
                        principalTable: "definitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_example_definitions_examples_ExampleId",
                        column: x => x.ExampleId,
                        principalTable: "examples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "example_theorems",
                columns: table => new
                {
                    ExampleId = table.Column<Guid>(type: "uuid", nullable: false),
                    TheoremId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_example_theorems", x => new { x.ExampleId, x.TheoremId });
                    table.ForeignKey(
                        name: "FK_example_theorems_examples_ExampleId",
                        column: x => x.ExampleId,
                        principalTable: "examples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_example_theorems_theorems_TheoremId",
                        column: x => x.TheoremId,
                        principalTable: "theorems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassroomId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExampleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByTeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    DueAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_assignments_classes_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_assignments_examples_ExampleId",
                        column: x => x.ExampleId,
                        principalTable: "examples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_assignments_users_CreatedByTeacherId",
                        column: x => x.CreatedByTeacherId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "class_enrollments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassroomId = table.Column<Guid>(type: "uuid", nullable: false),
                    LearnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    EnrolledAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_enrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_class_enrollments_classes_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_class_enrollments_users_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "attempt_stage_results",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    Stage = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Outcome = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    ResponseJson = table.Column<string>(type: "jsonb", nullable: true),
                    RecordedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attempt_stage_results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_attempt_stage_results_attempts_AttemptId",
                        column: x => x.AttemptId,
                        principalTable: "attempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "definitions",
                columns: new[] { "Id", "Description", "IsPublished", "Name" },
                values: new object[] { new Guid("ef08a50d-dc56-47ab-9c99-2104ce69c004"), "A chord is a straight line segment joining two points on a circle.", true, "Chord" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "Admin", "Admin" },
                    { "Author", "Author" },
                    { "Learner", "Learner" },
                    { "Teacher", "Teacher" },
                    { "Visitor", "Visitor" }
                });

            migrationBuilder.InsertData(
                table: "theorems",
                columns: new[] { "Id", "Explanation", "IsPublished", "Name", "Statement" },
                values: new object[] { new Guid("ef08a50d-dc56-47ab-9c99-2104ce69c003"), "The two angles stand on the same chord and lie in the same segment of the circle.", true, "Angles in the same segment", "Angles subtended by the same chord at the circumference, on the same segment, are equal." });

            migrationBuilder.InsertData(
                table: "topics",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("ef08a50d-dc56-47ab-9c99-2104ce69c001"), "Angles, chords, and circle theorems.", "Circle geometry" });

            migrationBuilder.InsertData(
                table: "examples",
                columns: new[] { "Id", "CreatedAtUtc", "Difficulty", "GuidedFlowJson", "IsPublished", "Prompt", "Title", "TopicId" },
                values: new object[] { new Guid("ef08a50d-dc56-47ab-9c99-2104ce69c002"), new DateTimeOffset(new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Medium", null, true, "Identify the angles subtended by chord AB at points C and D on the same segment.", "Angles in the same segment", new Guid("ef08a50d-dc56-47ab-9c99-2104ce69c001") });

            migrationBuilder.InsertData(
                table: "example_definitions",
                columns: new[] { "DefinitionId", "ExampleId" },
                values: new object[] { new Guid("ef08a50d-dc56-47ab-9c99-2104ce69c004"), new Guid("ef08a50d-dc56-47ab-9c99-2104ce69c002") });

            migrationBuilder.InsertData(
                table: "example_theorems",
                columns: new[] { "ExampleId", "TheoremId" },
                values: new object[] { new Guid("ef08a50d-dc56-47ab-9c99-2104ce69c002"), new Guid("ef08a50d-dc56-47ab-9c99-2104ce69c003") });

            migrationBuilder.CreateIndex(
                name: "IX_assignments_ClassroomId_DueAtUtc",
                table: "assignments",
                columns: new[] { "ClassroomId", "DueAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_assignments_CreatedByTeacherId",
                table: "assignments",
                column: "CreatedByTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_assignments_ExampleId",
                table: "assignments",
                column: "ExampleId");

            migrationBuilder.CreateIndex(
                name: "IX_attempt_stage_results_AttemptId_Stage",
                table: "attempt_stage_results",
                columns: new[] { "AttemptId", "Stage" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_attempts_ExampleId",
                table: "attempts",
                column: "ExampleId");

            migrationBuilder.CreateIndex(
                name: "IX_attempts_LearnerId_StartedAtUtc",
                table: "attempts",
                columns: new[] { "LearnerId", "StartedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_class_enrollments_ClassroomId_LearnerId",
                table: "class_enrollments",
                columns: new[] { "ClassroomId", "LearnerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_class_enrollments_LearnerId",
                table: "class_enrollments",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_classes_JoinCodeHash",
                table: "classes",
                column: "JoinCodeHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_classes_TeacherId",
                table: "classes",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_definitions_IsPublished_Name",
                table: "definitions",
                columns: new[] { "IsPublished", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_example_definitions_DefinitionId",
                table: "example_definitions",
                column: "DefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_example_theorems_TheoremId",
                table: "example_theorems",
                column: "TheoremId");

            migrationBuilder.CreateIndex(
                name: "IX_examples_IsPublished_Title",
                table: "examples",
                columns: new[] { "IsPublished", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_examples_TopicId",
                table: "examples",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_roles_Name",
                table: "roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_theorems_IsPublished_Name",
                table: "theorems",
                columns: new[] { "IsPublished", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_topics_Name",
                table: "topics",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Role",
                table: "users",
                column: "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assignments");

            migrationBuilder.DropTable(
                name: "attempt_stage_results");

            migrationBuilder.DropTable(
                name: "class_enrollments");

            migrationBuilder.DropTable(
                name: "example_definitions");

            migrationBuilder.DropTable(
                name: "example_theorems");

            migrationBuilder.DropTable(
                name: "attempts");

            migrationBuilder.DropTable(
                name: "classes");

            migrationBuilder.DropTable(
                name: "definitions");

            migrationBuilder.DropTable(
                name: "theorems");

            migrationBuilder.DropTable(
                name: "examples");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "topics");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
