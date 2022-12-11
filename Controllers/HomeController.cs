using System.Drawing;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BarCodeQRCodeGenerate.Models;
using BarcodeLib;
using QRCoder;

namespace BarCodeQRCodeGenerate.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;

	public HomeController(ILogger<HomeController> logger)
	{
		_logger = logger;
	}

	public IActionResult Index()
	{
		return View();
	}

	public IActionResult Privacy()
	{
		return View();
	}
	
	// BarCode 
	public IActionResult GenerateBarCode(string code ="256987")
	{
		Barcode barcode = new Barcode();
		Image img = barcode.Encode(TYPE.CODE39, code, Color.Black, Color.White, 250, 100);
		var data = ConvertImageToBytes(img);
		return File(data, "image/jpeg");
	}
	
	public byte[] ConvertImageToBytes(Image img)
	{
		using(MemoryStream ms = new MemoryStream())
		{
			img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
			return ms.ToArray();
		}
	}
	
	// QRCode
	public IActionResult GenerateQRCode(string code = "Welcome To SV School Of Tech")
	{
		QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
		QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
		QRCode qRCode = new QRCode(qRCodeData);
		Bitmap bitmap = qRCode.GetGraphic(15);
		var bitmapBytes = ConvertBitmapToBytes(bitmap);
		return File(bitmapBytes, "image/jpeg");
	}
	
	private byte[] ConvertBitmapToBytes(Bitmap bitmap)
	{
		using (MemoryStream ms = new MemoryStream())
		{
			bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
			return ms.ToArray();
		}
	}
	
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
