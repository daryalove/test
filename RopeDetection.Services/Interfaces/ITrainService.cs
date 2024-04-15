using RopeDetection.CommonData.ViewModels.TrainViewModel;
using RopeDetection.Shared.DataModels;
using System;
using System.Threading.Tasks;

namespace RopeDetection.Services.Interfaces
{
    public interface ITrainService
    {
        Task<TrainResponse> TrainModel(Guid modelId, Guid userId);
        Task<ModelOutput> Predict(PredictModel modelToPredict);
        Task PrepareWorkspace(Guid modelId, string assetsPath);
        Task SaveLabel(Guid modelId, string labelPath);
        Task<TrainResponse> SaveDetector(Guid modelId, Guid userId, string zipPath, string trainTime);
        Task SavePrediction(Guid modelId, Guid userId, int maxScore, Guid fileId, string predictedLabel);
    }
}
