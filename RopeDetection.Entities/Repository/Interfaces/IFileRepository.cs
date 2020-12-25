using RopeDetection.CommonData.ViewModels.FileViewModel;
using RopeDetection.CommonData.ViewModels.LabelViewModel;
using RopeDetection.Entities.Models;
using RopeDetection.Shared.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.Entities.Repository.Interfaces
{
    public interface IFileRepository: IRepository<FileData>
    {
        Task<IEnumerable<ImageByteContent>> GetFilesContentForTraining(Guid modelId);
    }
}
