using RopeDetection.CommonData.ViewModels.Base;
using RopeDetection.CommonData.ViewModels.LabelViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.Services.Interfaces
{
    public interface IModelService
    {
        Task<IEnumerable<LabelModel>> GetLabelList();
        Task<CreateModel> CreateModel(CreateModel model);
        Task<BaseModel> LoadFilesForTraining(CreateFilesModel model);
        Task<IEnumerable<ModelResponse>> GetModels();
    }
}
