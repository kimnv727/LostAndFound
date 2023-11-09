using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.API.Extensions
{
    public static class QRCodeExtensions
    {
        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        public static async Task<byte[]> GenerateQRCode(string QRCodeText)
        {
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(QRCodeText, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Image qrCodeImage = qrCode.GetGraphic(20);

            var bytes = ImageToByteArray(qrCodeImage);
            //return File(bytes, "image/bmp");
            return bytes;
        }
    }
}
