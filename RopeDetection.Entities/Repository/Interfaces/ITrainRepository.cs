using RopeDetection.Entities.Models;
using System;
using System.Threading.Tasks;

namespace RopeDetection.Entities.Repository.Interfaces
{
    public interface ITrainRepository: IRepository<TrainedModel>
    {
        Task<TrainedModel> UpdateTrainedModelAsync(Guid modelId, string path, CommonData.ModelEnums.ModelType type);
    }
}
