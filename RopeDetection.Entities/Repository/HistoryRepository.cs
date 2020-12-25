using RopeDetection.CommonData.ViewModels.Base;
using RopeDetection.Entities.DataContext;
using RopeDetection.Entities.Models;
using RopeDetection.Entities.Repository.Base;
using RopeDetection.Entities.Repository.Interfaces;
using System;
using System.Threading.Tasks;

namespace RopeDetection.Entities.Repository
{
    public class HistoryRepository : Repository<AnalysisHistory>, IHistoryRepository
    {
        public HistoryRepository(ModelContext dbContext) : base(dbContext)
        {
        }

        public async Task<AnalysisHistory> CreateHistoryEntry(AnalysisHistory entry, AnalysisResult result = null)
        {
            var model = _dbContext.Models.Find(entry.ModelId);
            if (model == null)
                throw new Exception("Модель не найдена.");

            entry.Model = model;
            entry.ModelId = model.Id;

            var learningStatus = (entry.AnalysisResult == CommonData.DefaultEnums.Result.OK) ? true : false;
            model.UpdateChangedOn(learningStatus);
            await _dbContext.SaveChangesAsync();

            if (result != null)
            {
                _dbContext.AnalysisResults.Add(result);
                await _dbContext.SaveChangesAsync();
            }

            var newEntity = await AddAsync(entry);
            return newEntity;
        }

        public Task<AnalysisHistory> UpdateHistoryEntry(AnalysisHistory entry, BaseModel model)
        {
            throw new NotImplementedException();
        }
    }
}
