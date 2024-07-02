using Microsoft.AspNetCore.Mvc;
using PdfProcessor.Backend.src.Application.Interfaces;
using PdfProcessor.Backend.src.WebApi.Model;

namespace PdfProcessor.Backend.src.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly IPdfService _pdfService;

        public PdfController(IPdfService pdfService)
        {
            _pdfService = pdfService;
        }

        private readonly ILogger<PdfController> _logger;

        public PdfController(ILogger<PdfController> logger)
        {
            _logger = logger;
        }

        [HttpPost("GetPdfController")]
        public async Task<IActionResult> TrainModel([FromBody] TrainModelRequest request)
        {
            var result = await _pdfService.TrainModelAsync(request.PdfFile, request.XmlTemplate);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
