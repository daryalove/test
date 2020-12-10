using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Vision;
using RopeDetection.Shared;
using RopeDetection.Shared.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Microsoft.ML.DataOperationsCatalog;

namespace RopeDetection.Train
{
    public class ModelBuilder
    {
        private static string projectDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "../../../../"));
        private static string workspaceRelativePath = System.IO.Path.Combine(projectDirectory, "RopeDetection.Train", "workspace");
        private static string assetsRelativePath = System.IO.Path.Combine(projectDirectory, "RopeDetection.Train", "assets");

        private static string modelDirectory = Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "../../../../"));
        private static string path = System.IO.Path.Combine(modelDirectory, "RopeDetection.Predict", "MLNETModel", "model.zip");

        private static IDataView testSet;
        private static ITransformer trainedModel;
        private static IDataView shuffledData;

        private static MLContext mlContext = new MLContext(seed: 1);
        //public static int LTP = 0;
        //private static string PathToImage = "7001-21";


        public static void CreateModel()
        {
            // Load Data
            // Получение списка изображений, используемых для обучения.
            IEnumerable<ImageData> images = FileUtils.LoadImagesFromDirectory(folder: assetsRelativePath, useFolderNameAsLabel: true);

            // Загрузка избражений в IDataView
            IDataView imageData = mlContext.Data.LoadFromEnumerable(images);
            var DataList = images.ToList();
            // Данные загружаются в том порядке, в котором они были считаны из каталогов. 
            //Чтобы сбалансировать данные, перемешайте их в случайном порядке с помощью метода ShuffleRows.
            shuffledData = mlContext.Data.ShuffleRows(imageData);

            // Build training pipeline
            IEstimator<ITransformer> trainingPipeline = BuildTrainingPipeline(mlContext);

            //Используем метод Fit, чтобы применить данные к preprocessingPipelineEstimatorChain,
            //а затем метод Transform, который возвращает IDataView, содержащий предварительно обработанные данные.
            IDataView preProcessedData = trainingPipeline
                    .Fit(shuffledData)
                    .Transform(shuffledData);

            TrainTestData trainSplit = mlContext.Data.TrainTestSplit(data: preProcessedData, testFraction: 0.3);
            TrainTestData validationTestSplit = mlContext.Data.TrainTestSplit(trainSplit.TestSet);

            IDataView trainSet = trainSplit.TrainSet;
            IDataView validationSet = validationTestSplit.TrainSet;
            testSet = validationTestSplit.TestSet;

            // Defining the learning pipeline
            var classifierOptions = new ImageClassificationTrainer.Options()
            {
                FeatureColumnName = "Image",
                LabelColumnName = "LabelAsKey",
                ValidationSet = validationSet,
                Arch = ImageClassificationTrainer.Architecture.ResnetV2101,
                MetricsCallback = (metrics) => Console.WriteLine(metrics),
                TestOnTrainSet = false,
                ReuseTrainSetBottleneckCachedValues = true,
                ReuseValidationSetBottleneckCachedValues = true,
                WorkspacePath = workspaceRelativePath,

            };

            trainingPipeline = mlContext.MulticlassClassification.Trainers.ImageClassification(classifierOptions)
    .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            // Train Model
            trainedModel = TrainModel(trainingPipeline, trainSet);

            // Evaluate quality of Model
            //Evaluate(mlContext, imageData, trainingPipeline);

            // Save model
            SaveModel(trainSet);
        }


        public static IEstimator<ITransformer> BuildTrainingPipeline(MLContext mlContext)
        {
            //Модели машинного обучения ожидают входные данные в числовом формате. 
            //Поэтому перед обучением необходимо выполнить некоторую предварительную обработку данных.(Создание класса)
            IEstimator<ITransformer> preprocessingPipeline = mlContext.Transforms.Conversion.MapValueToKey(
                inputColumnName: "Label",
                outputColumnName: "LabelAsKey")
            .Append(mlContext.Transforms.LoadRawImageBytes(
                outputColumnName: "Image",
                imageFolder: assetsRelativePath,
                inputColumnName: "ImagePath"));
            //// Data process configuration with pipeline data transformations 
            //var dataProcessPipeline = mlContext.Transforms.Conversion.MapValueToKey("Sentiment", "Sentiment")
            //                          .Append(mlContext.Transforms.Categorical.OneHotEncoding(new[] { new InputOutputColumnPair("LoggedIn", "LoggedIn") }))
            //                          .Append(mlContext.Transforms.Text.FeaturizeText("SentimentText_tf", "SentimentText"))
            //                          .Append(mlContext.Transforms.Concatenate("Features", new[] { "LoggedIn", "SentimentText_tf" }))
            //                          .Append(mlContext.Transforms.NormalizeMinMax("Features", "Features"))
            //                          .AppendCacheCheckpoint(mlContext);
            //// Set the training algorithm 
            //var trainer = mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(labelColumnName: "Sentiment", featureColumnName: "Features")
            //                          .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));

            //var trainingPipeline = dataProcessPipeline.Append(trainer);

            return preprocessingPipeline;
        }

        public static ITransformer TrainModel(IEstimator<ITransformer> trainingPipeline, IDataView trainSet)
        {
            Console.WriteLine("=============== Training  model ===============");

            trainedModel = trainingPipeline.Fit(trainSet);

            Console.WriteLine("=============== End of training process ===============");
            return trainedModel;
        }

        private static void Evaluate(MLContext mlContext, IDataView trainingDataView, IEstimator<ITransformer> trainingPipeline)
        {
            // Cross-Validate with single dataset (since we don't have two datasets, one for training and for evaluate)
            // in order to evaluate and get the model's accuracy metrics
            Console.WriteLine("=============== Cross-validating to get model's accuracy metrics ===============");
            var crossValidationResults = mlContext.MulticlassClassification.CrossValidate(trainingDataView, trainingPipeline, numberOfFolds: 5, labelColumnName: "Label");
            PrintMulticlassClassificationFoldsAverageMetrics(crossValidationResults);
        }

        private static void SaveModel(IDataView trainSet)
        {
            // Save/persist the trained model to a .ZIP file
            Console.WriteLine($"=============== Saving the model  ===============");

            mlContext.Model.Save(trainedModel, trainSet.Schema, path);

            string current_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "model.zip");
            mlContext.Model.Save(trainedModel, trainSet.Schema, current_path);

            //mlContext.Model.Save(mlModel, modelInputSchema, GetAbsolutePath(modelRelativePath));
            Console.WriteLine("The model is saved to {0} and to {1}", path, current_path);
        }

        //public static string GetAbsolutePath(string relativePath)
        //{
        //    FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
        //    string assemblyFolderPath = _dataRoot.Directory.FullName;

        //    string fullPath = Path.Combine(assemblyFolderPath, relativePath);

        //    return fullPath;
        //}

        public static void PrintMulticlassClassificationMetrics(MulticlassClassificationMetrics metrics)
        {
            Console.WriteLine($"************************************************************");
            Console.WriteLine($"*    Metrics for multi-class classification model   ");
            Console.WriteLine($"*-----------------------------------------------------------");
            Console.WriteLine($"    MacroAccuracy = {metrics.MacroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
            Console.WriteLine($"    MicroAccuracy = {metrics.MicroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
            Console.WriteLine($"    LogLoss = {metrics.LogLoss:0.####}, the closer to 0, the better");
            for (int i = 0; i < metrics.PerClassLogLoss.Count; i++)
            {
                Console.WriteLine($"    LogLoss for class {i + 1} = {metrics.PerClassLogLoss[i]:0.####}, the closer to 0, the better");
            }
            Console.WriteLine($"************************************************************");
        }

        public static void PrintMulticlassClassificationFoldsAverageMetrics(IEnumerable<TrainCatalogBase.CrossValidationResult<MulticlassClassificationMetrics>> crossValResults)
        {
            //var metricsInMultipleFolds = crossValResults.Select(r => r.Metrics);

            //var microAccuracyValues = metricsInMultipleFolds.Select(m => m.MicroAccuracy);
            //var microAccuracyAverage = microAccuracyValues.Average();
            //var microAccuraciesStdDeviation = CalculateStandardDeviation(microAccuracyValues);
            //var microAccuraciesConfidenceInterval95 = CalculateConfidenceInterval95(microAccuracyValues);

            //var macroAccuracyValues = metricsInMultipleFolds.Select(m => m.MacroAccuracy);
            //var macroAccuracyAverage = macroAccuracyValues.Average();
            //var macroAccuraciesStdDeviation = CalculateStandardDeviation(macroAccuracyValues);
            //var macroAccuraciesConfidenceInterval95 = CalculateConfidenceInterval95(macroAccuracyValues);

            //var logLossValues = metricsInMultipleFolds.Select(m => m.LogLoss);
            //var logLossAverage = logLossValues.Average();
            //var logLossStdDeviation = CalculateStandardDeviation(logLossValues);
            //var logLossConfidenceInterval95 = CalculateConfidenceInterval95(logLossValues);

            //var logLossReductionValues = metricsInMultipleFolds.Select(m => m.LogLossReduction);
            //var logLossReductionAverage = logLossReductionValues.Average();
            //var logLossReductionStdDeviation = CalculateStandardDeviation(logLossReductionValues);
            //var logLossReductionConfidenceInterval95 = CalculateConfidenceInterval95(logLossReductionValues);

            //Console.WriteLine($"*************************************************************************************************************");
            //Console.WriteLine($"*       Metrics for Multi-class Classification model      ");
            //Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
            //Console.WriteLine($"*       Average MicroAccuracy:    {microAccuracyAverage:0.###}  - Standard deviation: ({microAccuraciesStdDeviation:#.###})  - Confidence Interval 95%: ({microAccuraciesConfidenceInterval95:#.###})");
            //Console.WriteLine($"*       Average MacroAccuracy:    {macroAccuracyAverage:0.###}  - Standard deviation: ({macroAccuraciesStdDeviation:#.###})  - Confidence Interval 95%: ({macroAccuraciesConfidenceInterval95:#.###})");
            //Console.WriteLine($"*       Average LogLoss:          {logLossAverage:#.###}  - Standard deviation: ({logLossStdDeviation:#.###})  - Confidence Interval 95%: ({logLossConfidenceInterval95:#.###})");
            //Console.WriteLine($"*       Average LogLossReduction: {logLossReductionAverage:#.###}  - Standard deviation: ({logLossReductionStdDeviation:#.###})  - Confidence Interval 95%: ({logLossReductionConfidenceInterval95:#.###})");
            //Console.WriteLine($"*************************************************************************************************************");

        }
    }
}
