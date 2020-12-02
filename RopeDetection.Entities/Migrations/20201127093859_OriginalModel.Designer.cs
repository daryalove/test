﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RopeDetection.Entities.DataContext;

namespace RopeDetection.Entities.Migrations
{
    [DbContext(typeof(ModelContext))]
    [Migration("20201127093859_OriginalModel")]
    partial class OriginalModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("RopeDetection.Entities.Models.AnalysisHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AnalysisResult")
                        .HasColumnType("int");

                    b.Property<int>("DetectionType")
                        .HasColumnType("int");

                    b.Property<DateTime>("FinishedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ModelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ModelId");

                    b.ToTable("AnalysisHistories");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.AnalysisResult", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Characteristic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DownloadDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("FileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HistoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Label")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaxScore")
                        .HasColumnType("int");

                    b.Property<string>("PredictedValue")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HistoryId")
                        .IsUnique();

                    b.ToTable("AnalysisResults");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.AnalyzedObject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Characteristic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DownloadedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Owner")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TrainedModelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TrainedModelId");

                    b.HasIndex("UserId");

                    b.ToTable("AnalyzedObjects");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.FileData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("FileContent")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("FileIndex")
                        .HasColumnType("int");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ParentCode")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ParentType")
                        .HasColumnType("int");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FileIndex")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("FileDatas");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.Model", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ChangedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("LearningStatus")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.ModelAndObject", b =>
                {
                    b.Property<Guid>("ModelObjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ModelId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ModelObjectId", "ModelId");

                    b.HasIndex("ModelId");

                    b.ToTable("ModelAndObjects");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.ModelObject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Characteristic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DownloadedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("ModelObjects");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.ModelObjectType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Label")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TextContent")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ModelObjectTypes");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.TrainedModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ChangedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LearningStatus")
                        .HasColumnType("int");

                    b.Property<Guid?>("ModelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("ZipPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ModelId")
                        .IsUnique()
                        .HasFilter("[ModelId] IS NOT NULL");

                    b.ToTable("TrainedModels");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAllowed")
                        .HasColumnType("bit");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("UserFIO")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.AnalysisHistory", b =>
                {
                    b.HasOne("RopeDetection.Entities.Models.Model", "Model")
                        .WithMany("AnalysisHistories")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Model");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.AnalysisResult", b =>
                {
                    b.HasOne("RopeDetection.Entities.Models.AnalysisHistory", "History")
                        .WithOne("Result")
                        .HasForeignKey("RopeDetection.Entities.Models.AnalysisResult", "HistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("History");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.AnalyzedObject", b =>
                {
                    b.HasOne("RopeDetection.Entities.Models.TrainedModel", "trainedModel")
                        .WithMany("AnalyzedObjects")
                        .HasForeignKey("TrainedModelId");

                    b.HasOne("RopeDetection.Entities.Models.User", "User")
                        .WithMany("AnalyzedObjects")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("trainedModel");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.FileData", b =>
                {
                    b.HasOne("RopeDetection.Entities.Models.User", "User")
                        .WithMany("FileDatas")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.Model", b =>
                {
                    b.HasOne("RopeDetection.Entities.Models.User", "User")
                        .WithMany("Models")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.ModelAndObject", b =>
                {
                    b.HasOne("RopeDetection.Entities.Models.Model", "Model")
                        .WithMany("ModelAndObjects")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RopeDetection.Entities.Models.ModelObject", "ModelObject")
                        .WithMany("ModelAndObjects")
                        .HasForeignKey("ModelObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Model");

                    b.Navigation("ModelObject");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.ModelObject", b =>
                {
                    b.HasOne("RopeDetection.Entities.Models.ModelObjectType", "ModelObjectType")
                        .WithMany("ModelObjects")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ModelObjectType");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.TrainedModel", b =>
                {
                    b.HasOne("RopeDetection.Entities.Models.Model", "Model")
                        .WithOne("TrainedModel")
                        .HasForeignKey("RopeDetection.Entities.Models.TrainedModel", "ModelId");

                    b.Navigation("Model");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.AnalysisHistory", b =>
                {
                    b.Navigation("Result");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.Model", b =>
                {
                    b.Navigation("AnalysisHistories");

                    b.Navigation("ModelAndObjects");

                    b.Navigation("TrainedModel");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.ModelObject", b =>
                {
                    b.Navigation("ModelAndObjects");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.ModelObjectType", b =>
                {
                    b.Navigation("ModelObjects");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.TrainedModel", b =>
                {
                    b.Navigation("AnalyzedObjects");
                });

            modelBuilder.Entity("RopeDetection.Entities.Models.User", b =>
                {
                    b.Navigation("AnalyzedObjects");

                    b.Navigation("FileDatas");

                    b.Navigation("Models");
                });
#pragma warning restore 612, 618
        }
    }
}
