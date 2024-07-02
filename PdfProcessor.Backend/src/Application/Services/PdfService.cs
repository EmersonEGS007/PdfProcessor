using PdfProcessor.Backend.src.Application.Interfaces;
using PdfProcessor.Backend.src.Domain.Models;
using PdfProcessor.Backend.src.Infrastructure.ML;

namespace PdfProcessor.Backend.src.Application.Services
{
    public class PdfService : IPdfService
    {
        public async Task<bool> TrainModelAsync(string pdfFile, string xmlTemplate)
        {
            // Implementação do método de treinamento
            return await Task.FromResult(true);
        }
    }
}
