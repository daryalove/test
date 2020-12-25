using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.Shared.DataModels
{
    public class ImageByteContent
    {
        //полный путь, по которому хранится изображение
        public string ImageName { get; set; }

        //изображение
        public byte[] ImageContent { get; set; }

        //расширение файла
        public string FileType { get; set; }

        //прогнозируемое значение
        public string Label { get; set; }
    }
}
