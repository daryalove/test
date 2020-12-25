using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.CommonData.ViewModels.FileViewModel
{
    public class FileModel
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileContent { get; set; }
    }
}
