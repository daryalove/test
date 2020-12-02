using RopeDetection.Entities.DataContext;
using RopeDetection.Entities.Models.Base;
using RopeDetection.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.Entities.Models
{
    public class AnalyzedObject: EntityBase
    {
        public string Type { get; set; }
        public Guid? TrainedModelId { get; set; }
        public string Owner { get; set; }
        public DateTime DownloadedDate { get; set; }
        public string Characteristic { get; set; }
        public Guid UserId { get; set; }

        public TrainedModel trainedModel;
        public User User { get; set; }

        public void Insert()
        {
            using (var db = new ModelContext())
            {
                db.AnalyzedObjects.Add(this);
                db.SaveChanges();
            }
        }
    }
}
