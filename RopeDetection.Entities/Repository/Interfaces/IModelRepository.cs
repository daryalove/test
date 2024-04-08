using RopeDetection.CommonData.ViewModels.LabelViewModel;
using RopeDetection.CommonData.ViewModels.UserViewModel;
using RopeDetection.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.Entities.Repository.Interfaces
{
    public interface IModelRepository
    {
        Task<Model> CreateModel(CreateModel model);
        Task<IReadOnlyList<Model>> GetModels();
        Task<Model> UpdateModel(bool status);
        Task<Model> GetModel(Guid id);
        Task<Model> GetActualModel();
    }
}
