using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tesseract;
using System.Drawing.Imaging;
using static System.Net.Mime.MediaTypeNames;
using LostAndFound.Infrastructure.Services.Interfaces;


namespace LostAndFound.API.Controllers
{
    [ApiController]
    [Route("api/ocr")]
    public class OcrController : Controller
    {
        private readonly IOcrService _ocrService;

        public OcrController(IOcrService ocrService)
        {
            _ocrService = ocrService;
        }


        /// <summary>
        /// Get Ocr from a picture url
        /// </summary>
        /// <returns></returns>
        [HttpPost("/eng")]
        public async Task<IActionResult> GetOcrEngAsync([FromForm] string imageUrl)
        {
            using (var httpClient = new HttpClient())
            {
                byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

                string text = await _ocrService.GetEngOcrFromImageUrlAsync(imageUrl);
                return Ok(text);
            }

        }

        /// <summary>
        /// Get Ocr from a picture url
        /// </summary>
        /// <returns></returns>
        [HttpPost("/vie")]
        public async Task<IActionResult> GetOcrVieAsync([FromForm] string imageUrl)
        {
            using (var httpClient = new HttpClient())
            {
                byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

                string text = await _ocrService.GetVieOcrFromImageUrlAsync(imageUrl);
                return Ok(text);
            }

        }
    }
}


