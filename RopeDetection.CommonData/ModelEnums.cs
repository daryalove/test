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
            Regression
        }

        public enum TrainStatus
        {
            NotTrained,
            InProgress,
            Completed
        }
    }
}
