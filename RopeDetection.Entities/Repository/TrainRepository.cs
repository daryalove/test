using RopeDetection.Entities.DataContext;
using RopeDetection.Entities.Models;
using RopeDetection.Entities.Repository.Base;
using RopeDetection.Entities.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.Entities.Repository
{
    public class TrainRepository : Repository<TrainedModel>, ITrainRepository
    {
        public TrainRepository(ModelContext dbContext) : base(dbContext)
        {
        }

        public async Task<TrainedModel> UpdateTrainedModelAsync(Guid modelId, string path, CommonData.ModelEnums.ModelType type)
        {
            var trainedModel = (await GetAsync(m => m.ModelId == modelId)).FirstOrDefault();
            trainedModel.UpdatedProgressOn(CommonData.ModelEnums.TrainStatus.Completed, path, type);
            await UpdateAsync(trainedModel);
            return trainedModel;
        }
    }
}
