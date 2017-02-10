using System.IO;
using GeneticImages.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;

namespace GeneticImages.Controllers
{
    public class GeneticImages : Controller
    {
        private static Engine engine = new Engine();

        [HttpPost]
        public IActionResult RunStatic(IFormFile file)
        {
            if (GeneticImages.engine.GetStatus().IsRunning)
            {
                return BadRequest(GeneticImages.engine.GetStatus());
            }

            SKBitmap targetBitmap;
            using (Stream stream = file.OpenReadStream())
            {
                targetBitmap = Utilities.LoadBitmap(stream);
            }

			GeneticImages.engine.Run(targetBitmap);

            return this.EngineStatus();
        }

        public IActionResult BestImageFromGeneration(int id)
        {
            return new FileStreamResult(
                new FileStream($"engine-output/generation-{id}.png", FileMode.Open),
                "image/png"
            );
        }

		public IActionResult LatestImage()
		{
			return BestImageFromGeneration(GeneticImages.engine.GetStatus().CurrentGeneration);
		}

        public IActionResult TargetImage()
        {
            return new FileStreamResult(
                new FileStream("engine-output/target.png", FileMode.Open),
                "image/png"
            );
        }

		public IActionResult Cancel()
		{
			GeneticImages.engine.Cancel();

			return Ok(GeneticImages.engine.GetStatus());
		}

        public IActionResult EngineStatus()
        {
            return Ok(GeneticImages.engine.GetStatus());
        }
    }
}