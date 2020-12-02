using Microsoft.ML;
using RopeDetection.Shared.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RopeDetection.Predict
{
    public class ConsumeModel
    {
        private static string modelDirectory = Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "../../../../"));
        private static string path = System.IO.Path.Combine(modelDirectory, "RopeDetection.Predict", "MLNETModel", "model.zip");

        private static Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictionEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(CreatePredictionEngine);

        // For more info on consuming ML.NET models, visit https://aka.ms/mlnet-consume
        // Method for consuming model in your app
        public static ModelOutput PredictSingleImage(ModelInput input)
        {
            try
            {
                ModelOutput result = PredictionEngine.Value.Predict(input);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static PredictionEngine<ModelInput, ModelOutput> CreatePredictionEngine()
        {
            // Create new MLContext
            MLContext mlContext = new MLContext();

            // Load model & create prediction engine
            //var imageClassifierZip = Path.Combine(Environment.CurrentDirectory, "MLNETModel", "imageClassifier.zip");
            //string modelPath = @"C:\Users\Дарья\AppData\Local\Temp\MLVSTools\MyFirstMachineLearningModelML\MyFirstMachineLearningModelML.Model\MLModel.zip";
            ITransformer mlModel = mlContext.Model.Load(path, out var modelInputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            return predEngine;
        }

        //public static void ClassifySingleImage(MLContext mlContext, IDataView data, ITransformer trainedModel)
        //{
        //    PredictionEngine<ModelInput, ModelOutput> predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(trainedModel);

        //    ModelInput image = mlContext.Data.CreateEnumerable<ModelInput>(data, reuseRowObject: true).First();

        //    ModelOutput prediction = predictionEngine.Predict(image);

        //    Console.WriteLine("Classifying single image");
        //    OutputPrediction(prediction);
        //}

        //private static void OutputPrediction(ModelOutput prediction)
        //{
        //    string imageName = Path.GetFileName(prediction.ImagePath);
        //    Console.WriteLine($"Image: {imageName} | Actual Value: {prediction.Label} | Predicted Value: {prediction.PredictedLabel}");
        //}
    }
}
