using RopeDetection.Entities.DataContext;
using RopeDetection.Entities.Models.Base;
using System;
using static RopeDetection.CommonData.DefaultEnums;
using static RopeDetection.CommonData.ModelEnums;

namespace RopeDetection.Entities.Models
{
    public class AnalysisHistory: EntityBase
    {
        public DateTime StartedDate { get; set; }
        public Guid ModelId { get; set; }
        public DateTime FinishedDate { get; set; }
        public Result AnalysisResult { get; set; }
        public string Message { get; set; }
        public DetectionType DetectionType { get; set; }
        public Guid UserId { get; set; }

        public Model Model { get; set; }
        public AnalysisResult Result { get; set; }

        public void Insert()
        {
            using (var db = new ModelContext())
            {
                db.AnalysisHistories.Add(this);
                db.SaveChanges();
            }
        }

        public void UpdatedMessageOn(string newMessage)
        {
            this.Message = newMessage;
        }

        public void UpdatedResultOn(Result newResult)
        {
            this.AnalysisResult = newResult;
        }

        public void UpdatedFinishedOn()
        {
            this.FinishedDate = DateTime.Now;
        }
    }
}
