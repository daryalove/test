using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.CommonData.ViewModels.FileViewModel
{
    public class FileShortInfo
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileContent { get; set; }
        public Guid ParentCode { get; set; }
        public string Label { get; set; }
    }
}
