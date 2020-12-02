using RopeDetection.CommonData.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.CommonData.ViewModels.LabelViewModel
{
    public class LabelModel: BaseModel
    {
        public string Label { get; set; }
        public string TextContent { get; set; }
    }
}
