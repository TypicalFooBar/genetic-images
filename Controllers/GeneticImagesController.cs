using System;
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
		public IActionResult Run(IFormFile file, Engine.RunConfig runConfig)
        {
			// If the engine is running, do not continue
            if (GeneticImages.engine.GetStatus().IsRunning)
            {
                return BadRequest(GeneticImages.engine.GetStatus());
            }

			// Get the bitmap
            SKBitmap targetBitmap;
            using (Stream stream = file.OpenReadStream())
            {
                targetBitmap = Utilities.LoadBitmap(stream);
            }

			// Set the target bitmap
			runConfig.TargetBitmap = targetBitmap;

			// Start the engine
			GeneticImages.engine.Run(runConfig);

            return this.EngineStatus();
        }

        public IActionResult BestImageFromGeneration(int id)
        {
			try
			{
				return new FileStreamResult(
					new FileStream($"{Utilities.EngineOutputDirectory}/{Utilities.FittestGeneForGenerationDirectory}/{id}.png", FileMode.Open),
					"image/png"
				);
			}
			catch (Exception)
			{
				return BadRequest("File not found");
			}
        }

		public IActionResult LatestImage()
		{
			return BestImageFromGeneration(GeneticImages.engine.GetStatus().CurrentGeneration);
		}

        public IActionResult TargetImage()
        {
			try
			{
				return new FileStreamResult(
					new FileStream($"{Utilities.EngineOutputDirectory}/target.png", FileMode.Open),
					"image/png"
				);
				}
			catch (Exception)
			{
				return BadRequest("File not found");
			}
        }

		public IActionResult Step(int id)
		{
			try
			{
				return new FileStreamResult(
					new FileStream($"{Utilities.EngineOutputDirectory}/{Utilities.FittestGeneStepsDirectory}/{id}.png", FileMode.Open),
					"image/png"
				);
				}
			catch (Exception)
			{
				return BadRequest("File not found");
			}
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