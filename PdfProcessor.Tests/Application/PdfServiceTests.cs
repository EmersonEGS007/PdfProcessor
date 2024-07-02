using Moq;
using PdfProcessor.Backend.src.Application.Interfaces;
using PdfProcessor.Backend.src.Application.Services;
using PdfProcessor.Backend.src.Domain.Models;
using PdfProcessor.Backend.src.Infrastructure.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfProcessor.Tests.Application
{
    public class PdfServiceTests
    {
        private readonly Mock<PdfModelTrainer> _modelTrainerMock;
        private readonly IPdfService _pdfService;

        public PdfServiceTests()
        {
            _modelTrainerMock = new Mock<PdfModelTrainer>(null);
            _pdfService = new PdfService(_modelTrainerMock.Object);
        }

        [Fact]
        public async Task ProcessPdfAsync_ShouldReturnXmlData()
        {
            var pdfData = new PdfData { Content = "test content" };
            var xmlData = new XmlData { XmlContent = "test xml content" };
            _modelTrainerMock.Setup(x => x.Predict(It.IsAny<PdfData>())).Returns(xmlData);

            var result = await _pdfService.ProcessPdfAsync(pdfData);

            Assert.Equal("test xml content", result.XmlContent);
        }

        [Fact]
        public async Task TrainModelAsync_ShouldTrainModel()
        {
            var pdfData = new List<PdfData> { new PdfData { Content = "test content" } };
            var xmlData = new List<XmlData> { new XmlData { XmlContent = "test xml content" } };

            await _pdfService.TrainModelAsync(pdfData, xmlData);

            _modelTrainerMock.Verify(x => x.TrainModel(pdfData, xmlData), Times.Once);
        }
    }
}
