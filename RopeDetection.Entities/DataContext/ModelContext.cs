using Microsoft.EntityFrameworkCore;
using RopeDetection.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.Entities.DataContext
{
    public class ModelContext: DbContext
    {
        public ModelContext()
        {

        }
        public ModelContext(DbContextOptions<ModelContext> options) : base(options)
        {
            Database.Migrate();
        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ModelObject>()
                .HasOne<ModelObjectType>(s => s.ModelObjectType)
                .WithMany(g => g.ModelObjects)
                .HasForeignKey(s => s.TypeId);

            modelBuilder.Entity<AnalysisHistory>()
                .HasOne<Model>(s => s.Model)
                .WithMany(g => g.AnalysisHistories)
                .HasForeignKey(s => s.ModelId);

            modelBuilder.Entity<AnalyzedObject>()
               .HasOne<TrainedModel>(s => s.trainedModel)
               .WithMany(g => g.AnalyzedObjects)
               .HasForeignKey(s => s.TrainedModelId);

            modelBuilder.Entity<ModelAndObject>()
            .HasOne<Model>(sc => sc.Model)
            .WithMany(s => s.ModelAndObjects)
            .HasForeignKey(sc => sc.ModelId);

            modelBuilder.Entity<ModelAndObject>()
            .HasOne<ModelObject>(sc => sc.ModelObject)
            .WithMany(s => s.ModelAndObjects)
            .HasForeignKey(sc => sc.ModelObjectId);

            // modelBuilder.Entity<AnalyzedObject>()
            //.HasOne<FileData>(s => s.FileData)
            //.WithOne(ad => ad.AnalyzedObject)
            //.HasForeignKey<FileData>(ad => ad.ParentCode).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TrainedModel>()
           .HasOne<Model>(s => s.Model)
           .WithOne(ad => ad.TrainedModel)
           .HasForeignKey<TrainedModel>(ad => ad.ModelId);

            modelBuilder.Entity<AnalysisHistory>()
               .HasOne<AnalysisResult>(s => s.Result)
               .WithOne(ad => ad.History)
               .HasForeignKey<AnalysisResult>(ad => ad.HistoryId);

            modelBuilder.Entity<FileData>()
            .HasOne<User>(sc => sc.User)
            .WithMany(s => s.FileDatas)
            .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<Model>()
           .HasOne<User>(sc => sc.User)
           .WithMany(s => s.Models)
           .HasForeignKey(sc => sc.UserId);


            modelBuilder.Entity<AnalyzedObject>()
           .HasOne<User>(sc => sc.User)
           .WithMany(s => s.AnalyzedObjects)
           .HasForeignKey(sc => sc.UserId);

            //modelBuilder.Entity<ModelObject>()
            //  .HasOne<FileData>(s => s.FileData)
            //  .WithOne(ad => ad.ModelObject)
            //  .HasForeignKey<FileData>(ad => ad.ParentCode);

            //modelBuilder.Entity<AnalyzedObject>()
            //.HasOne<FileData>(s => s.FileData)
            //.WithOne(ad => ad.AnalyzedObject)
            //.HasForeignKey<FileData>(ad => ad.ParentCode);

            modelBuilder.Entity<FileData>().HasIndex(u => u.FileIndex).IsUnique();
            //modelBuilder.Entity<ModelObject>().OwnsOne(u => u.FileData);

            //modelBuilder.Entity<AnalysisHistory>().HasAlternateKey(p => new { p.UserId });
            //modelBuilder.Entity<FileData>().HasAlternateKey(p => new { p.UserId });
            //modelBuilder.Entity<Model>().HasAlternateKey(p => new { p.UserId });
            //modelBuilder.Entity<AnalyzedObject>().HasAlternateKey(p => new { p.UserId });
            modelBuilder.Entity<ModelAndObject>().HasKey(sc => new { sc.ModelObjectId, sc.ModelId });
        }

        public DbSet<FileData> FileDatas { get; set; }
        public DbSet<AnalysisHistory> AnalysisHistories { get; set; }
        public DbSet<AnalysisResult> AnalysisResults { get; set; }
        public DbSet<AnalyzedObject> AnalyzedObjects { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<ModelObject> ModelObjects { get; set; }
        public DbSet<ModelAndObject> ModelAndObjects { get; set; }
        public DbSet<TrainedModel> TrainedModels { get; set; }
        public DbSet<ModelObjectType> ModelObjectTypes { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
