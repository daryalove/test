using RopeDetection.CommonData.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using static RopeDetection.CommonData.DefaultEnums;
using static RopeDetection.CommonData.ModelEnums;

namespace RopeDetection.CommonData.ViewModels.TrainViewModel
{
    public class TrainResponse: BaseModel
    {
        public DateTime StartedDate { get; set; }
        public DateTime FinishedDate { get; set; }
        public Result AnalysisResult { get; set; }
        public string Message { get; set; }
        public DetectionType DetectionType { get; set; }
        public string TrainTime { get; set; }
    }
}
