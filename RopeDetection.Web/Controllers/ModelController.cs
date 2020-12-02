using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RopeDetection.CommonData.ViewModels.LabelViewModel;
using RopeDetection.CommonData.ViewModels.UserViewModel;
using RopeDetection.Services.Interfaces;
using static RopeDetection.CommonData.ModelEnums;

namespace RopeDetection.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ModelController : ControllerBase
    {
        private readonly IModelService _modelService;
        private readonly ILogger<ModelController> _logger;

        public ModelController(ILogger<ModelController> logger, IModelService modelService)
        {
            _logger = logger;
            _modelService = modelService ?? throw new ArgumentNullException(nameof(modelService));
        }

        //Создание модели
        [HttpPost]
        [Route("CreateModel")]
        public async Task<IActionResult> CreateModel(string name, ModelType type)
        {
            try
            {
                var userId = getUserId();
                if (userId == Guid.Empty)
                    return NotFound(new { message = "Такого пользователя нет в базе данных!" });

                CreateModel model = new CreateModel
                {
                    UserId = userId,
                    Type = type,
                    Name = name
                };

                var modelData = await _modelService.CreateModel(model);
                if (modelData.Result == CommonData.DefaultEnums.Result.OK)
                {
                    
                }
                return Ok(modelData);
            }
            catch (Exception exp)
            {
                return BadRequest(new CreateModel() { Error = exp, Result = CommonData.DefaultEnums.Result.Error });
            }
        }

        //Получение списка всех дефектов
        [HttpGet]
        [Route("GetLabels")]
        public async Task<IActionResult> GetLabels()
        {
            try
            {
                var labels =  await _modelService.GetLabelList();
                if (labels == null)
                    return NotFound(new { message = "Список дефектов пуст!" });
                else
                    return Ok(labels);
            }
            catch (Exception exp)
            {
                return BadRequest(new { message = exp.Message });
            }
        }

        [HttpGet]
        [Route("GetUserId")]
        public IActionResult GetUserId()
        {
            Guid Id = getUserId();
            return Ok(Id);
        }

        private Guid getUserId()
        {
            try
            {
                var id = (Guid)HttpContext.Items["UserId"];
                return id;
            }
            catch
            {
                return Guid.Empty;
            }
        }
    }
}
