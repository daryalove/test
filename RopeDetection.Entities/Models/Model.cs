using RopeDetection.Entities.DataContext;
using RopeDetection.Entities.Models.Base;
using RopeDetection.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using static RopeDetection.CommonData.ModelEnums;

namespace RopeDetection.Entities.Models
{
    public class Model: EntityBase
    {
        public Model()
        {
            this.AnalysisHistories = new List<AnalysisHistory>();
            this.ModelAndObjects = new List<ModelAndObject>();
            this.TrainedModel = new TrainedModel();
        }

        public string Name { get; set; }
        public ModelType Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool LearningStatus { get; set; }
        public DateTime ChangedDate { get; set; }
        public Guid UserId { get; set; }

        public List<ModelAndObject> ModelAndObjects { get; set; }
        public List<AnalysisHistory> AnalysisHistories { get; set; }
        public TrainedModel TrainedModel { get; set; }
        public User User { get; set; }

        public void Insert()
        {
            using (var db = new ModelContext())
            {
                db.Models.Add(this);
                db.SaveChanges();
            }
        }

        public void UpdateChangedOn()
        {
            this.ChangedDate = DateTime.Now;
        }
    }
}
