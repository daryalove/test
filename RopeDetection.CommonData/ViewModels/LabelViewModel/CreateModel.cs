using RopeDetection.CommonData.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using static RopeDetection.CommonData.ModelEnums;

namespace RopeDetection.CommonData.ViewModels.LabelViewModel
{
    public class CreateModel: BaseModel
    {
        public string Name { get; set; }
        public ModelType Type { get; set; }
        public Guid UserId { get; set; }
    }
}
