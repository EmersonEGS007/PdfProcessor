using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using System.Data.SqlClient;
using PdfProcessor.Backend.src.Domain.Models;

namespace PdfProcessor.Backend.src.Infrastructure.ML
{
    public class PdfModelTrainer
    {
        private readonly MLContext _mlContext;
        private readonly string _connectionString;

        public PdfModelTrainer(string connectionString)
        {
            _mlContext = new MLContext();
            _connectionString = connectionString;
        }

        public ITransformer TrainModel(List<PdfData> trainingData, List<XmlData> xmlData)
        {
            var dataView = _mlContext.Data.LoadFromEnumerable(trainingData.Zip(xmlData, (pdf, xml) => new { pdf.Content, xml.XmlContent }));

            var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", "Content")
                .Append(_mlContext.Transforms.Text.FeaturizeText("Label", "XmlContent"))
                .Append(_mlContext.Transforms.CopyColumns("Features", "Label"))
                .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(_mlContext.Regression.Trainers.Sdca());

            var model = pipeline.Fit(dataView);

            SaveModel(model);

            return model;
        }

        private void SaveModel(ITransformer model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO TrainedModels (ModelData) VALUES (@ModelData)", connection);
                using (var stream = new MemoryStream())
                {
                    _mlContext.Model.Save(model, null, stream);
                    command.Parameters.AddWithValue("@ModelData", stream.ToArray());
                    command.ExecuteNonQuery();
                }
            }
        }

        public XmlData Predict(PdfData pdfData)
        {
            var model = LoadModel();
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<PdfData, XmlData>(model);
            return predictionEngine.Predict(pdfData);
        }

        private ITransformer LoadModel()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT TOP 1 ModelData FROM TrainedModels ORDER BY Id DESC", connection);
                var modelData = (byte[])command.ExecuteScalar();
                using (var stream = new MemoryStream(modelData))
                {
                    return _mlContext.Model.Load(stream, out var schema);
                }
            }
        }
    }
}
