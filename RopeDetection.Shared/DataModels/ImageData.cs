using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.Shared.DataModels
{
    public class ImageData
    {
        //полный путь, по которому хранится изображение
        public string ImagePath { get; set; }

        //прогнозируемое значение
        public string Label { get; set; }
    }
}
