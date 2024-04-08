using System;
using System.Collections.Generic;
using System.Text;
using static RopeDetection.CommonData.ModelEnums;

namespace RopeDetection.CommonData.ViewModels.LabelViewModel
{
    public class ModelResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ModelType Type { get; set; }
        public bool LearningStatus { get; set; }
        public string LabelPath { get; set; }
    }
}
