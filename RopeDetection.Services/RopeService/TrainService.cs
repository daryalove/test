using Microsoft.ML.Data;
using RopeDetection.CommonData.ViewModels.Base;
using RopeDetection.CommonData.ViewModels.TrainViewModel;
using RopeDetection.Entities.Models;
using RopeDetection.Entities.Repository.Interfaces;
using RopeDetection.Predict;
using RopeDetection.Services.Interfaces;
using RopeDetection.Services.Mapper;
using RopeDetection.Shared;
using RopeDetection.Shared.DataModels;
using RopeDetection.Train;
using RopeDetection.Train.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RopeDetection.Services.RopeService
{
    public class TrainService: ITrainService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly ITrainRepository _trainRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IAnalyzedObjectRepository _analysisRepository;
        private readonly IAppLogger<TrainService> _logger;

        private DateTime startedDate;

        public TrainService(IFileRepository fileRepository,
            IAppLogger<TrainService> logger, IHistoryRepository historyRepository, ITrainRepository trainRepository
            , IModelRepository modelRepository, IAnalyzedObjectRepository analyzedObject)
        {
            _historyRepository = historyRepository ?? throw new ArgumentNullException(nameof(historyRepository));
            _trainRepository = trainRepository ?? throw new ArgumentNullException(nameof(trainRepository));
            _fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
            _modelRepository = modelRepository ?? throw new ArgumentNullException(nameof(modelRepository));
            _analysisRepository = analyzedObject ?? throw new ArgumentNullException(nameof(analyzedObject));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //анализ изображения
        public async Task<ModelOutput> Predict(PredictModel modelToPredict)
        {
            try
            {
                startedDate = DateTime.Now;
                var file = await _analysisRepository.LoadSingleFileForPredictAsync(modelToPredict);

                var model = await _modelRepository.GetModel(modelToPredict.ModelId);
                if (model == null)
                    throw new Exception("Модель не найдена.");

                StaticModel.ModelId = modelToPredict.ModelId.ToString();

                // Measure #1 prediction execution time.
                var watch = System.Diagnostics.Stopwatch.StartNew();

                var prediction = ConsumeModel.PredictSingleImage(modelToPredict.Image);

                // Stop measuring time.
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("First Prediction took: " + elapsedMs + "mlSecs");

                if (prediction == null)
                    throw new Exception("Ошибка анализа. Пожалуйста, попробуйте снова.");

                // Get the highest score and its index
                var maxScore = prediction.Score.Max();

                ////////
                // Double-check using the index
                //var maxIndex = prediction.Score.ToList().IndexOf(maxScore);
                //VBuffer<ReadOnlyMemory<char>> keys = default;
                //predictionEngine.OutputSchema[3].GetKeyValues(ref keys);
                //var keysArray = keys.DenseValues().ToArray();
                //var predictedLabelString = keysArray[maxIndex];
                ////////

                Console.WriteLine($"Image Path : [{prediction.ImagePath}], " +
                                  $"Predicted Label : [{prediction.PredictedLabel}], " +
                                  $"Probability : [{maxScore}] "
                                  );


                AnalysisHistory beAddedEntry = new AnalysisHistory
                {
                    DetectionType = CommonData.ModelEnums.DetectionType.Analysis,
                    FinishedDate = DateTime.Now,
                    StartedDate = startedDate,
                    AnalysisResult = CommonData.DefaultEnums.Result.OK,
                    Message = "Изображение успешно проанализировано.",
                    ModelId = modelToPredict.ModelId,
                    UserId = modelToPredict.UserId,
                    Model = model
                };

                AnalysisResult result = new AnalysisResult
                {
                    MaxScore = (int)maxScore,
                    DownloadDate = startedDate,
                    Characteristic = "",
                    FileId = file.Id,
                    Label = prediction.Label,
                    PredictedValue = prediction.PredictedLabel,
                    HistoryId = beAddedEntry.Id
                    //History = beAddedEntry
                };

              //  var entry = await _historyRepository.CreateHistoryEntry(beAddedEntry, result);
                _logger.LogInformation($"Model successfully analysed");
                return prediction;

                ////Predict all images in the folder
                ////
                //Console.WriteLine("");
                //Console.WriteLine("Predicting several images...");

                //foreach (var currentImageToPredict in imagesToPredict)
                //{
                //    var currentPrediction = predictionEngine.Predict(currentImageToPredict);

                //    Console.WriteLine(
                //        $"Image Filename : [{currentImageToPredict.ImageFileName}], " +
                //        $"Predicted Label : [{currentPrediction.PredictedLabel}], " +
                //        $"Probability : [{currentPrediction.Score.Max()}]");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Модель не обучена. Просьба обучить модель. " + ex.ToString());
                return null;
            }
        }

        //обучение модели
        public async Task<TrainResponse> TrainModel(Guid modelId, Guid userId)
        {
            try
            {
                //загрузка файлов для обучения
                startedDate = DateTime.Now;
                var files = await _fileRepository.GetFilesContentForTraining(modelId);
                if (files == null)
                    throw new Exception("Для обучения модели просьба загрузить файлы.");
                
                //сохранение файлов в определенную папку для обучения
                var compressResult = Compress.SaveFilesInFolder(files);
                if (compressResult != true)
                    throw new Exception("Произошла ошибка записи файлов.");

                //начало обучения
                Console.WriteLine("Starting training...");
   
                // Measure #1 prediction execution time.
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var result = Task.Run(() => ModelBuilder.CreateModel(modelId.ToString()));

                while (!result.IsCompleted)
                {
                    Thread.Sleep(10000);
                }
    
                if (result.IsFaulted)
                    throw new Exception(result.Exception.Message);

                // Stop measuring time.
                watch.Stop();

                var elapsedMs = watch.ElapsedMilliseconds;
                var minutes = TimeSpan.FromMilliseconds(elapsedMs).TotalMinutes;
                Console.WriteLine("First Training took: " + minutes + " minutes");

                BaseModel model = new BaseModel
                {
                    Result = CommonData.DefaultEnums.Result.OK,
                    SuccessInfo = "Модель успешно обучена!"
                };
             
                var trainResult = await _trainRepository.UpdateTrainedModelAsync(modelId, ModelBuilder.ModelPath, CommonData.ModelEnums.ModelType.Classification);

                AnalysisHistory beAddedEntry = new AnalysisHistory
                {
                    DetectionType = CommonData.ModelEnums.DetectionType.Training,
                    FinishedDate = DateTime.Now,
                    StartedDate = startedDate,
                    AnalysisResult = CommonData.DefaultEnums.Result.OK,
                    Message = "Модель успешно обучена.",
                    ModelId = modelId,
                    UserId = userId
                };

                var entry = await _historyRepository.CreateHistoryEntry(beAddedEntry);
                _logger.LogInformation($"Model successfully trained");
                var mapped = ObjectMapper.Mapper.Map<TrainResponse>(entry);
                mapped.TrainTime = minutes.ToString();
                mapped.SuccessInfo = entry.Message;
                return mapped;
            }
            catch (Exception exp)
            {
                AnalysisHistory beAddedEntry = new AnalysisHistory
                {
                    DetectionType = CommonData.ModelEnums.DetectionType.Training,
                    FinishedDate = DateTime.Now,
                    StartedDate = startedDate,
                    AnalysisResult = CommonData.DefaultEnums.Result.Error,
                    Message = exp.Message,
                    ModelId = modelId,
                    UserId = userId
                };

                var category = await _historyRepository.CreateHistoryEntry(beAddedEntry);
                _logger.LogWarning("Ошибка обучения ", exp);
                var mapped = ObjectMapper.Mapper.Map<TrainResponse>(category);
                return mapped;
            }
        }
    }
}
