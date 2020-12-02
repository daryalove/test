using RopeDetection.Entities.DataContext;
using RopeDetection.Entities.Models;
using RopeDetection.Entities.Repository.Base;
using RopeDetection.Entities.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.Entities.Repository
{
    public class ModelTypeRepository : Repository<ModelObjectType>, IModelTypeRepository
    {
        public ModelTypeRepository(ModelContext dbContext) : base(dbContext)
        {
        }

        //public Task<Model> CreateModel(Model model)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<IReadOnlyList<ModelObjectType>> GetLabels()
        {
            var category = GetAllAsync();
            return category;
        }
    }
}
