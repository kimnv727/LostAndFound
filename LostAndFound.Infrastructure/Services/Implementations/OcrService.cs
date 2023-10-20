using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tesseract;
using System.Drawing.Imaging;
using static System.Net.Mime.MediaTypeNames;
using LostAndFound.Infrastructure.Services.Interfaces;

namespace LostAndFound.Infrastructure.Services.Implementations
{
    public class OcrService : IOcrService
    {
        public async Task<string> GetEngOcrFromImageUrlAsync(string imageUrl)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

                    using (var engine = new TesseractEngine(@"./Tesseract/tessdata", "eng", EngineMode.Default))
                    {
                        using (var img = Pix.LoadFromMemory(imageBytes))
                        {
                            using (var page = engine.Process(img))
                            {
                                var text = page.GetText();
                                return text;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetVieOcrFromImageUrlAsync(string imageUrl)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

                    using (var engine = new TesseractEngine(@"./Tesseract/tessdata", "vie", EngineMode.Default))
                    {
                        using (var img = Pix.LoadFromMemory(imageBytes))
                        {
                            using (var page = engine.Process(img))
                            {
                                var text = page.GetText();
                                return text;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
