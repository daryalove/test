using RopeDetection.CommonData.ViewModels.TrainViewModel;
using RopeDetection.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.Entities.Repository.Interfaces
{
    public interface IAnalyzedObjectRepository: IRepository<AnalyzedObject>
    {
        Task<FileData> LoadSingleFileForPredictAsync(PredictModel model);
    }
}
