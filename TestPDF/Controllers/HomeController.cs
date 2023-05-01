using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using ImageMagick;
using TestPDF.Models;


namespace TestPDF.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var pdfFilePath = Path.Combine(buildDir, @"wwwroot\sample.pdf");
            // Convert all PDF pages to images using Ghostscript.NET
            var settings = new MagickReadSettings();
            // Settings the density to 300 dpi will create an image with a better quality
            settings.Density = new Density(300, 300);

            using (var images = new MagickImageCollection())
            {
                // Add all the pages of the pdf file to the collection
                images.Read(pdfFilePath, settings);

                var page = 1;
                foreach (var image in images)
                {
                    // Write page to file that contains the page number
                    image.Format = MagickFormat.Jpeg;
                    image.Write(buildDir + page + ".jpe");
                    // Writing to a specific format works the same as for a single image
                    
                    page++;
                }
            }

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}