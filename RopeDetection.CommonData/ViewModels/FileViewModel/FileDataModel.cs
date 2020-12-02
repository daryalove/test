using RopeDetection.CommonData.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.CommonData.ViewModels.FileViewModel
{
    public class FileDataModel : BaseModel
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileContent { get; set; }
        public Guid ParentCode { get; set; }
        public int FileIndex { get; set; }
    }
}
