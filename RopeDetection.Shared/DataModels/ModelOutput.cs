using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.Shared.DataModels
{
    public class ModelOutput
    {
        //полный путь, по которому хранится изображение
        public string ImagePath { get; set; }

        //прогнозируемое значение
        public string Label { get; set; }

        //точность анализа
        public float[] Score { get; set; }

        //значение, спрогнозированное моделью
        public string PredictedLabel { get; set; }
    }
}
