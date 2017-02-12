using System.IO;
using System.Threading.Tasks;
using SkiaSharp;

namespace GeneticImages.Core
{
    public class Engine
    {
        public class Status
        {
            public int CurrentGeneration { get; set; } = 1;
			public bool IsRunning { get; set; }
			public bool ResultsAvailable { get; set; }
			public string Message { get; set; }
        }

		public class RunConfig
		{
			public SKBitmap TargetBitmap { get; set; }
			public int Generations { get; set; }
			public int GenesPerGeneration { get; set; }
			public int GenesToReproduce { get; set; }
		}

        private Population population;
		private Status status;
		private Task runInstance;
		private bool cancel;

        public Engine()
        {
			this.status = new Status();
        }

        public void Run(RunConfig runConfig)
        {
			// Create a new run instance
			this.runInstance = Task.Run(() => {
				// Update engine status
				this.UpdateStatus(true, false, "Engine is running");

				// Remove the current engine output directory if it exists
				if (Directory.Exists(Utilities.EngineOutputDirectory))
					Directory.Delete(Utilities.EngineOutputDirectory, true);

				this.population = new Population(runConfig.TargetBitmap, runConfig.GenesPerGeneration, runConfig.GenesPerGeneration);
				//population.GenerateStaticGenePopulation();
				this.population.GeneratePaintGenePopulation();

				var watch = System.Diagnostics.Stopwatch.StartNew();

				for (int i = 1; i <= runConfig.Generations; i++)
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
					Gene topGene = this.population.NaturalSelection();

					// Save the image of the top gene
					Utilities.SaveFittestGeneForGeneration(topGene.Bitmap, i);

					// Update engine status
					this.status.CurrentGeneration = i ;
				}

				watch.Stop();
				var elapsedMs = watch.ElapsedMilliseconds;
				System.Console.WriteLine($"Milliseconds: {elapsedMs}");

				this.population.EvaluateFitness();
				Utilities.SaveBitmap(this.population.BestGene.Bitmap, $"{Utilities.EngineOutputDirectory}/result.png");

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