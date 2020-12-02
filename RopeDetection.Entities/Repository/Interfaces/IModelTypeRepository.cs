using RopeDetection.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.Entities.Repository.Interfaces
{
    public interface IModelTypeRepository: IRepository<ModelObjectType>
    {
        //Task<User> GetUserWithDailyRowsAsync(Guid userId);
        Task<IReadOnlyList<ModelObjectType>> GetLabels();
        //Task<Model> CreateModel(Model model);
    }
}
