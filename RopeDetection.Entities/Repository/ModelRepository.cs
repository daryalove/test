using RopeDetection.CommonData.ViewModels.LabelViewModel;
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
    public class ModelRepository : Repository<Model>, IModelRepository
    {
        public ModelRepository(ModelContext dbContext) : base(dbContext)
        {
        }

        public async Task<Model> CreateModel(CreateModel model)
        {
            var GetData = GetAsyncIQueryable(m => m.Name.ToLower() == model.Name.ToLower());
            if (GetData.Count() != 0)
            {
                throw new Exception("Модель с таким именем существует");
            }

            Model mappedEntity = new Model
            {
                ChangedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                LearningStatus = false,
                Name = model.Name,
                Type = model.Type,
                UserId = model.UserId
            };

            var newEntity = await AddAsync(mappedEntity);
            return newEntity;
        }

        public async Task<IReadOnlyList<Model>> GetModels()
        {
            var category = await GetAllAsync();
            return category;
        }
    }
}
