using PdfProcessor.Backend.src.Domain.Models;

namespace PdfProcessor.Backend.src.Application.Interfaces
{
    public interface IPdfService
    {
        Task<bool> TrainModelAsync(string pdfFile, string xmlTemplate);
    }
}
