using AutoMapper;
using RopeDetection.CommonData.ViewModels.Base;
using RopeDetection.CommonData.ViewModels.LabelViewModel;
using RopeDetection.Entities.Repository.Interfaces;
using RopeDetection.Services.Interfaces;
using RopeDetection.Services.Mapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.Services.RopeService
{
    public class ModelService : IModelService
    {
        private readonly IModelTypeRepository _labelRepository;
        private readonly IModelRepository _modelReporitory;
        private readonly IAppLogger<ModelService> _logger;

        public ModelService(IModelTypeRepository labelRepository,
            IAppLogger<ModelService> logger, IModelRepository modelRepository)
        {
            _labelRepository = labelRepository ?? throw new ArgumentNullException(nameof(labelRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _modelReporitory = modelRepository;
        }

        public async Task<CreateModel> CreateModel(CreateModel model)
        {
            try
            {
                var mappedEntity = await _modelReporitory.CreateModel(model);
                _logger.LogInformation($"Entity successfully added - BaseProject");
                var newMappedEntity = ObjectMapper.Mapper.Map<CreateModel>(mappedEntity);
                newMappedEntity.SuccessInfo = "Модель успешно создана.";
                newMappedEntity.UserId = Guid.Empty;
                return newMappedEntity;
            }
            catch (Exception exp)
            {
                _logger.LogWarning("При создании модели возникла ошибка", exp);
                return BaseModelUtilities<CreateModel>.ErrorFormat(exp);
            }
        }

        public async Task<IEnumerable<LabelModel>> GetLabelList()
        {
            try
            {
                var category = await _labelRepository.GetAllAsync();
                var mapped = ObjectMapper.Mapper.Map<IEnumerable<LabelModel>>(category);
                return mapped;
            }
            catch (Exception exp)
            {
                _logger.LogWarning("Ошибка получения списка дефектов", exp);
                return new List<LabelModel>();
            }
        }
    }
}
