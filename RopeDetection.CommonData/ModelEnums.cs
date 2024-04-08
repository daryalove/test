using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.CommonData
{
    public class ModelEnums
    {
        public enum DetectionType
        {
            Analysis,
            Training
        }

        public enum Parent
        {
            AnalyzedObject,
            ModelObject
        }

        public enum ModelType
        {
            Classification,
            Regression,
            ObjectDetection
        }

        public enum TrainStatus
        {
            NotTrained,
            Labeling,
            InProgress,
            Completed
        }
    }
}
