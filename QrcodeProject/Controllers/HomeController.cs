using Microsoft.AspNetCore.Mvc;
using QrcodeProject.Models;
using QRCoder;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using static System.Net.Mime.MediaTypeNames;
//using ZXing.QrCode.Internal;

namespace QrcodeProject.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public IActionResult CreateQRCode()
		{
			return View();
		}

		[HttpPost]
		public IActionResult CreateQRCode(QRCodeModel qRCode)
		{
            //QRCodeGenerator QrGenerator = new QRCodeGenerator();
            //QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(qRCode.QRCodeText, QRCodeGenerator.ECCLevel.Q);
            //QRCode QrCode = new QRCode(QrCodeInfo);
            //Bitmap QrBitmap = QrCode.GetGraphic(60);
            //byte[] BitmapArray = QrBitmap.BitmapToByteArray();
            //string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
            //ViewBag.QrCodeUri = QrUri;


            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qRCode.QRCodeText, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            // Load logo image
            string logoFilePath = ".\\wwwroot\\tel.png";
            System.Drawing.Image logoImage = System.Drawing.Image.FromFile(logoFilePath);

            // Calculate the size of the logo to fit inside the QR code
            int logoSize = (int)(qrCodeImage.Width * 0.2);
            logoImage = new Bitmap(logoImage, new Size(logoSize, logoSize));

            // Create a new bitmap to hold the QR code with logo
            Bitmap qrCodeWithLogo = new Bitmap(qrCodeImage.Width, qrCodeImage.Height);

            // Create a new graphics object to draw the QR code and logo
            using (Graphics g = Graphics.FromImage(qrCodeWithLogo))
            {
                // Draw the QR code onto the new bitmap
                g.DrawImage(qrCodeImage, 0, 0, qrCodeImage.Width, qrCodeImage.Height);

                // Draw the logo in the center of the QR code
                int logoX = (qrCodeImage.Width - logoSize) / 2;
                int logoY = (qrCodeImage.Height - logoSize) / 2;
                g.DrawImage(logoImage, logoX, logoY, logoSize, logoSize);
            }

            // Save the QR code with logo as a PNG file
            //qrCodeWithLogo.Save(outputFilePath, ImageFormat.Png);

            byte[] BitmapArray = qrCodeWithLogo.BitmapToByteArray();
            string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
            ViewBag.QrCodeUri = QrUri;

            return View();
		}
	}

	//Extension method to convert Bitmap to Byte Array
	public static class BitmapExtension
	{
		public static byte[] BitmapToByteArray(this Bitmap bitmap)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				bitmap.Save(ms, ImageFormat.Png);
				return ms.ToArray();
			}
		}
	}

    //public IActionResult GenerateQRCode(string link)
    //{
    //    string text = $"Hello! Scan this QR code to open the link: {link}";

    //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
    //    QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
    //    QRCode qrCode = new QRCode(qrCodeData);
    //    Bitmap qrCodeImage = qrCode.GetGraphic(200);

    //    // Save the QR code image as a PNG file and return it as a file result
    //    MemoryStream ms = new MemoryStream();
    //    qrCodeImage.Save(ms, ImageFormat.Png);
    //    ms.Position = 0;
    //    return File(ms, "image/png", "qrcode.png");
    //}
}