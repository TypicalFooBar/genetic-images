using System.IO;
using System.Threading.Tasks;
using GeneticImages.Core;
using ImageSharp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeneticImages.Controllers
{
    public class GeneticImages : Controller
    {
        private class EngineStatusResponse
        {
            public bool EngineIsRunning { get; set; }
            public bool ResultsAvailable { get; set; }
            public int CurrentGeneration { get; set; }
            public string Message { get; set; }
        }

        private static bool engineIsRunning = false;
        private static bool resultsAvailable = false;

        private static Engine engine = new Engine();

        [HttpPost]
        public IActionResult RunStatic(IFormFile file)
        {
            if (GeneticImages.engineIsRunning)
            {
                return BadRequest(this.GetEngineStatusResponse());
            }

            GeneticImages.engineIsRunning = true;
            GeneticImages.resultsAvailable = false;

            Image targetImage;
            using (Stream stream = file.OpenReadStream())
            {
                targetImage = new Image(stream);
            }

            Task.Run(() => {
                GeneticImages.engine.Run(targetImage);

                GeneticImages.engineIsRunning = false;
                GeneticImages.resultsAvailable = true;

            });

            return this.EngineStatus();
        }

        public IActionResult BestImageFromGeneration(int id)
        {
            return new FileStreamResult(
                new FileStream("engine-output/generation-" + id + "-gene.png", FileMode.Open),
                "image/png"
            );
        }

        public IActionResult TargetImage()
        {
            return new FileStreamResult(
                new FileStream("engine-output/target.png", FileMode.Open),
                "image/png"
            );
        }

        public IActionResult EngineStatus()
        {
            return Ok(this.GetEngineStatusResponse());
        }

        private EngineStatusResponse GetEngineStatusResponse()
        {
            string message = "";

            if (!engineIsRunning && !resultsAvailable)
            {
                message = "Engine is idle";
            }
            else if (engineIsRunning && !resultsAvailable)
            {
                message = "Engine is running";
            }
            else if (!engineIsRunning && resultsAvailable)
            {
                message = "Results are available";
            }

            // TODO: Get all engine status from this object
            Engine.Status engineStatus = GeneticImages.engine.GetStatus();

            return new EngineStatusResponse {
                EngineIsRunning = GeneticImages.engineIsRunning,
                ResultsAvailable = GeneticImages.resultsAvailable,
                CurrentGeneration = engineStatus.CurrentGeneration,
                Message = message
            };
        }
    }
}