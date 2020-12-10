using RopeDetection.CommonData.ViewModels.LabelViewModel;
using RopeDetection.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.Entities.Repository.Interfaces
{
    public interface IModelObjectRepository: IRepository<ModelObject>
    {
        Task LoadFilesForTrainigAsync(CreateFilesModel model);
    }
}
