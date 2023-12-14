using System;
using QRCoder;
using Figgle;

namespace WebScraping;

internal class QrCode
{
    public static void WriteQrCode()
    {
        string inputText = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";

        // Generate QR code
        string qrCodeAscii = GenerateQRCode(inputText);

        // Display QR code in the console
        Console.WriteLine("\n\n!!!!!! Scan this super secure QR Code, 100% totally not a virus !!!!!");
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
        Console.WriteLine("Konami (Arrow keys): ");

        ConsoleKey[] konamiCode = {
            ConsoleKey.UpArrow, ConsoleKey.UpArrow,
            ConsoleKey.DownArrow, ConsoleKey.DownArrow,
            ConsoleKey.LeftArrow, ConsoleKey.RightArrow,
            ConsoleKey.LeftArrow, ConsoleKey.RightArrow,
            ConsoleKey.B, ConsoleKey.A
        };

        ConsoleKeyInfo[] userInput = new ConsoleKeyInfo[konamiCode.Length];

        for (int i = 0; i < konamiCode.Length; i++)
        {
            userInput[i] = Console.ReadKey(true);
            if (userInput[i].Key != konamiCode[i])
            {
                Console.WriteLine("\nIncorrect code. Try again.");
                return;
            }
        }

        // Console Write QR code
        WriteQrCode();
    }
}
