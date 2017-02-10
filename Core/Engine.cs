using System.Threading;
using System.Threading.Tasks;
using SkiaSharp;

namespace GeneticImages.Core
{
    public class Engine
    {
        public class Status
        {
            public int CurrentGeneration { get; set; }
			public bool IsRunning { get; set; }
			public bool ResultsAvailable { get; set; }
			public string Message { get; set; }
        }

        private Population population;
		private Status status;
		private Task runInstance;
		private bool cancel;

        public Engine()
        {
			this.status = new Status();
        }

        public void Run(SKBitmap targetBitmap)
        {
			// Create a new run instance
			this.runInstance = Task.Run(() => {
				// Update engine status
				this.UpdateStatus(true, false, "Engine is running");

				// Remove the current engine output directory
				//Directory.Delete("engine-output");

				this.population = new Population(targetBitmap);
				//population.GenerateStaticGenePopulation();
				this.population.GeneratePaintGenePopulation();

				var watch = System.Diagnostics.Stopwatch.StartNew();

				while (this.population.CurrentGeneration <= 1000)
				{
					// Check to see if this run has been cancelled
					if (this.cancel)
					{
						// Update engine status
						this.UpdateStatus(false, true, "Engine was cancelled");
						this.cancel = false;
						break;
					}

					this.population.EvaluateFitness();
					this.population.NaturalSelection();

					// Update engine status
					this.status.CurrentGeneration = this.population.CurrentGeneration - 1;
				}

				watch.Stop();
				var elapsedMs = watch.ElapsedMilliseconds;
				System.Console.WriteLine($"Milliseconds: {elapsedMs}");

				this.population.EvaluateFitness();
				Utilities.SaveBitmapAsPng(this.population.BestGene.Bitmap, "result");

				// Update engine status
				this.UpdateStatus(false, true, "Results are available");
				this.cancel = false;
			});
        }

		public void Cancel()
		{
			this.cancel = true;
		}

		private void UpdateStatus(bool isRunning, bool resultsAvailable, string message)
		{
			this.status.IsRunning = isRunning;
			this.status.ResultsAvailable = resultsAvailable;
			this.status.Message = message;
		}

        public Engine.Status GetStatus()
        {
            return this.status;
        }
    }
}