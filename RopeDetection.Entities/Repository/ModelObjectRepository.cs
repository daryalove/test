using RopeDetection.CommonData.ViewModels.LabelViewModel;
using RopeDetection.Entities.DataContext;
using RopeDetection.Entities.Models;
using RopeDetection.Entities.Repository.Base;
using RopeDetection.Entities.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.Entities.Repository
{
    public class ModelObjectRepository : Repository<ModelObject>, IModelObjectRepository
    {
        public ModelObjectRepository(ModelContext dbContext) : base(dbContext)
        {
        }

        //загрузка файлов для обучения
        public async Task LoadFilesForTrainigAsync(CreateFilesModel model)
        {
            Model trainig_model = _dbContext.Models.Find(model.ModelId);
            if (trainig_model == null)
                throw new Exception("Модель не найдена. Просьба создать новую модель.");

            ModelObjectType founded_type = _dbContext.ModelObjectTypes.Find(model.TypeId);
            if (founded_type == null)
                throw new Exception("Такой тип дефекта не найден. Просьба указать другой.");

            List<ModelObject> objects = new List<ModelObject>();
            List<ModelAndObject> related_entities = new List<ModelAndObject>();
            List<FileData> files = new List<FileData>();

            for (int i = 0; i < model.Files.Count; i++)
            {
                objects.Add(new ModelObject
                {
                    TypeId = model.TypeId,
                    DownloadedDate = DateTime.Now,
                    ModelObjectType = founded_type
                }
                );

                related_entities.Add(new ModelAndObject
                {
                    ModelId = model.ModelId,
                    ModelObjectId = objects[i].Id
                });

                files.Add(new FileData
                {
                    FileContent = model.Files[i].FileContent,
                    FileName = model.Files[i].FileName,
                    FileType = model.Files[i].FileType,
                    ParentCode = objects[i].Id,
                    ParentType = CommonData.ModelEnums.Parent.ModelObject,
                    UserId = model.UserId,
                    FileIndex = 0
                });
            }

            //добавление объектов модели
            await AddRangeAsync(objects);

            //добавление связанных сущностей
            _dbContext.ModelAndObjects.AddRange(related_entities);
            await _dbContext.SaveChangesAsync();

            //добавление файлов
            _dbContext.FileDatas.AddRange(files);
            await _dbContext.SaveChangesAsync();

            //привязка модели
            trainig_model.ModelAndObjects = related_entities;
            trainig_model.UpdateChangedOn(false);
            await _dbContext.SaveChangesAsync();
        }
    }
}
