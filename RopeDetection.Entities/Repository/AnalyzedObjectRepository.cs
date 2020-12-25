using Microsoft.EntityFrameworkCore;
using RopeDetection.CommonData.ViewModels.TrainViewModel;
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
    public class AnalyzedObjectRepository : Repository<AnalyzedObject>, IAnalyzedObjectRepository
    {
        public AnalyzedObjectRepository(ModelContext dbContext) : base(dbContext)
        {
        }

        public async Task<FileData> LoadSingleFileForPredictAsync(PredictModel model)
        {
            Model trainig_model = await _dbContext.Models.Where(s => s.Id == model.ModelId).Include(s => s.TrainedModel).FirstOrDefaultAsync();
            if (trainig_model == null)
                throw new Exception("Модель не найдена. Просьба создать новую модель.");

            var a_object = new AnalyzedObject
            {
                DownloadedDate = DateTime.Now,
                Characteristic = "",
                Owner = model.UserId.ToString(),
                TrainedModelId = trainig_model.TrainedModel.Id,
                UserId = model.UserId,
                trainedModel = trainig_model.TrainedModel
            };

            _dbContext.AnalyzedObjects.Add(a_object);
            await _dbContext.SaveChangesAsync();

            var file = new FileData
            {
                FileContent = model.Image.Image,
                FileName = "",
                FileType = "",
                ParentCode = a_object.Id,
                ParentType = CommonData.ModelEnums.Parent.AnalyzedObject,
                UserId = model.UserId,
                FileIndex = 0
            };

            //добавление файлa
            _dbContext.FileDatas.Add(file);
            await _dbContext.SaveChangesAsync();

            //сохранение обученной модели
            trainig_model.TrainedModel.ChangedDate = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return file;
        }
    }
}
