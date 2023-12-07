using System;
using QRCoder;

namespace WebScraping;

internal class QrCode
{
    public static void WriteQrCode()
    {
        string inputText = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";

        // Generate QR code
        string qrCodeAscii = GenerateQRCode(inputText);

        // Display QR code in the console
        Console.WriteLine("\n\n\n!!!!!! Scan this super secure QR Code, 100% totally not a virus !!!!!");
        Console.WriteLine(qrCodeAscii);
    }

    private static string GenerateQRCode(string inputText)
    {
        QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(inputText, QRCodeGenerator.ECCLevel.Q);

        AsciiQRCode qrCode = new AsciiQRCode(qrCodeData);

        return qrCode.GetGraphic(1);
    }

    // Ask for menu
    public static void PrintQRCode()
    {
        // Console Write QR code
        WriteQrCode();
    }
}
