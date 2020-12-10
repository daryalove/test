using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RopeDetection.Entities.Migrations
{
    public partial class Original_Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModelObjectTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelObjectTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserFIO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAllowed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModelObjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DownloadedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Characteristic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelObjects_ModelObjectTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ModelObjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileContent = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ParentCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileIndex = table.Column<int>(type: "int", nullable: false),
                    ParentType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileDatas_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LearningStatus = table.Column<bool>(type: "bit", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Models_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FinishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnalysisResult = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetectionType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysisHistories_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelAndObjects",
                columns: table => new
                {
                    ModelObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelAndObjects", x => new { x.ModelObjectId, x.ModelId });
                    table.ForeignKey(
                        name: "FK_ModelAndObjects_ModelObjects_ModelObjectId",
                        column: x => x.ModelObjectId,
                        principalTable: "ModelObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelAndObjects_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainedModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ZipPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LearningStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainedModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainedModels_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PredictedValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DownloadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Characteristic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxScore = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysisResults_AnalysisHistories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "AnalysisHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalyzedObjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrainedModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DownloadedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Characteristic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyzedObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalyzedObjects_TrainedModels_TrainedModelId",
                        column: x => x.TrainedModelId,
                        principalTable: "TrainedModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnalyzedObjects_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisHistories_ModelId",
                table: "AnalysisHistories",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisResults_HistoryId",
                table: "AnalysisResults",
                column: "HistoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnalyzedObjects_TrainedModelId",
                table: "AnalyzedObjects",
                column: "TrainedModelId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyzedObjects_UserId",
                table: "AnalyzedObjects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FileDatas_UserId",
                table: "FileDatas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelAndObjects_ModelId",
                table: "ModelAndObjects",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelObjects_TypeId",
                table: "ModelObjects",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_UserId",
                table: "Models",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainedModels_ModelId",
                table: "TrainedModels",
                column: "ModelId",
                unique: true,
                filter: "[ModelId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalysisResults");

            migrationBuilder.DropTable(
                name: "AnalyzedObjects");

            migrationBuilder.DropTable(
                name: "FileDatas");

            migrationBuilder.DropTable(
                name: "ModelAndObjects");

            migrationBuilder.DropTable(
                name: "AnalysisHistories");

            migrationBuilder.DropTable(
                name: "TrainedModels");

            migrationBuilder.DropTable(
                name: "ModelObjects");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "ModelObjectTypes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
