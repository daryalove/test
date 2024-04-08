using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RopeDetection.CommonData.ViewModels.TrainViewModel;
using RopeDetection.Services.Interfaces;
using RopeDetection.Shared.DataModels;
using RopeDetection.Train.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using RopeDetection.Entities.Configuration;
using Microsoft.Extensions.Options;

namespace RopeDetection.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MatlabObjectDetectionController : ControllerBase
    {
        private readonly ITrainService _trainService;
        private readonly ILogger<MatlabObjectDetectionController> _logger;
        private readonly AppSettings _appSettings;

        public MatlabObjectDetectionController(ILogger<MatlabObjectDetectionController> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Сохранение модели
        /// </summary>
        [HttpPost]
        [Route("SaveModel")]
        public async Task<IActionResult> SaveModel(Guid modelId)
        {
            try
            {
                var userId = getUserId();
                if (userId == Guid.Empty)
                    return NotFound(new { message = "Такого пользователя нет в базе данных!" });

                var result = await _trainService.TrainModel(modelId, userId);
                return Ok(result);
            }
            catch (Exception exp)
            {
                return BadRequest(new { message = exp.Message });
            }
        }

        /// <summary>
        /// Подготовка рабочего пространства для обучения
        /// </summary>
        [HttpPost]
        [Route("PrepareWorkspace")]
        public async Task<IActionResult> PrepareWorkspace(Guid modelId)
        {
            try
            {
                await _trainService.PrepareWorkspace(modelId, _appSettings.MatlabWorkspace);
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest(new { message = exp.Message });
            }
        }

        /// <summary>
        /// Прогноз модели
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("ClassifyImage")]
        public async Task<IActionResult> ClassifyImage(PredictModel model)
        {
            if (model.Image.Image.Length == 0)
                return BadRequest(new { message = "Загрузите фото для анализа." });

            //if (!IsValidImage(model.Image.Image))
            //    return StatusCode(StatusCodes.Status415UnsupportedMediaType);

            var userId = getUserId();
            if (userId == Guid.Empty)
                return NotFound(new { message = "Такого пользователя нет в базе данных!" });
            model.UserId = userId;

            _logger.LogInformation("Start processing image...");
            var imageBestLabelPrediction = await _trainService.Predict(model);
            return Ok(imageBestLabelPrediction);
        }

        [HttpGet]
        public IActionResult DownloadZip()
        {
            List<ImageByteContent> images = new List<ImageByteContent>();
            var zipStream = Compress.Zip(images);
            return File(zipStream, "application/octet-stream");
        }

        //Получение ID пользователя
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
