using AutoMapper;
using RopeDetection.CommonData.ViewModels.Base;
using RopeDetection.CommonData.ViewModels.LabelViewModel;
using RopeDetection.Entities.Models;
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
        private readonly IModelObjectRepository _modelObjectRepository;
        private readonly IModelRepository _modelReporitory;
        private readonly IAppLogger<ModelService> _logger;

        public ModelService(IModelTypeRepository labelRepository,
            IAppLogger<ModelService> logger, IModelRepository modelRepository, IModelObjectRepository modelObjectRepository)
        {
            _labelRepository = labelRepository ?? throw new ArgumentNullException(nameof(labelRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _modelReporitory = modelRepository ?? throw new ArgumentNullException(nameof(modelRepository));
            _modelObjectRepository = modelObjectRepository ?? throw new ArgumentNullException(nameof(modelObjectRepository));
        }

        //Создание модели
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

        //Получение списка дефектов
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

        //Получение списка моделей
        public async Task<IEnumerable<ModelResponse>> GetModels()
        {
            try
            {
                var category = await _modelReporitory.GetModels();
                var mapped = ObjectMapper.Mapper.Map<IEnumerable<ModelResponse>>(category);
                return mapped;
            }
            catch (Exception exp)
            {
                _logger.LogWarning("Ошибка получения списка моделей", exp);
                return new List<ModelResponse>();
            }
        }

        //загрузка файлов для обучения
        public async Task<BaseModel> LoadFilesForTraining(CreateFilesModel model)
        {
            try
            {
                await _modelObjectRepository.LoadFilesForTrainigAsync(model);
                _logger.LogInformation($"Entity successfully added - Files");
                var result = new BaseModel
                {
                    Result = CommonData.DefaultEnums.Result.OK,
                    SuccessInfo = "Файлы для обучения успешно загружены."
                };
                return result;
            }
            catch (Exception exp)
            {
                _logger.LogWarning("При загрузке файлов возникли ошибки", exp);
                return BaseModelUtilities<BaseModel>.ErrorFormat(exp);
            }
        }
    }
}
