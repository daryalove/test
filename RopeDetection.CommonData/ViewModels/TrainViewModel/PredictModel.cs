using RopeDetection.Shared.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.CommonData.ViewModels.TrainViewModel
{
    public class PredictModel
    {
        public Guid ModelId { get; set; }
        public Guid UserId { get; set; }
        public ModelInput Image { get; set; }
    }
}
