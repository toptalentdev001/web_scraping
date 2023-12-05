using System;
using QRCoder;

namespace WebScraping
{
    public class QrCode
    {
        public static void ShowCode()
        {
            string inputText = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";

            // Generate QR code
            string qrCodeAscii = GenerateQRCode(inputText);

            // Display QR code in the console
            Console.WriteLine("!!!!!! Scan this super secure QR Code, 100% totally not a virus !!!!!");
            Console.WriteLine(qrCodeAscii);
        }

        static string GenerateQRCode(string inputText)
        {
            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(inputText, QRCodeGenerator.ECCLevel.Q);

            AsciiQRCode qrCode = new AsciiQRCode(qrCodeData);

            return qrCode.GetGraphic(1);
        }
    }
}
