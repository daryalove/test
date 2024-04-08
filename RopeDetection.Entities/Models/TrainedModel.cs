﻿using RopeDetection.Entities.DataContext;
using RopeDetection.Entities.Models.Base;
using System;
using System.Collections.Generic;
using static RopeDetection.CommonData.ModelEnums;

namespace RopeDetection.Entities.Models
{
    public class TrainedModel: EntityBase
    {
        public TrainedModel()
        {
            this.AnalyzedObjects = new List<AnalyzedObject>();
        }

        public Guid? ModelId { get; set; }
        public string Name { get; set; }
        public ModelType Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ChangedDate { get; set; }
        public string ZipPath { get; set; }
        public string LabelPath { get; set; }
        public TrainStatus LearningStatus { get; set; }

        public List<AnalyzedObject> AnalyzedObjects { get; set; }
        public Model Model { get; set; }

        public void Insert()
        {
            using (var db = new ModelContext())
            {
                db.TrainedModels.Add(this);
                db.SaveChanges();
            }
        }

        public void UpdatedProgressOn(TrainStatus newStatus, string path, ModelType type)
        {
            this.LearningStatus = newStatus;
            this.ZipPath = path;
            this.Type = type;
            this.ChangedDate = DateTime.Now;
        }

        public void UpdateLabeledOn(string labelPath)
        {
            LearningStatus = TrainStatus.Labeling;
            LabelPath = labelPath;
        }
    }
}
