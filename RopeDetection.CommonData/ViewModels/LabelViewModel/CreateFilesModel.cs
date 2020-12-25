using RopeDetection.CommonData.ViewModels.Base;
using RopeDetection.CommonData.ViewModels.FileViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.CommonData.ViewModels.LabelViewModel
{
    public class CreateFilesModel: BaseModel
    {
        public CreateFilesModel()
        {
            this.Files = new List<FileModel>();
        }
        public Guid UserId { get; set; }
        public Guid ModelId { get; set; }
        public Guid TypeId { get; set; }
        public List<FileModel> Files { get; set; }
    }
}
