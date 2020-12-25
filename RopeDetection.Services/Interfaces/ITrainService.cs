using RopeDetection.CommonData.ViewModels.Base;
using RopeDetection.CommonData.ViewModels.TrainViewModel;
using RopeDetection.Shared.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.Services.Interfaces
{
    public interface ITrainService
    {
        Task<TrainResponse> TrainModel(Guid modelId, Guid userId);
        Task<ModelOutput> Predict(PredictModel modelToPredict);
    }
}
