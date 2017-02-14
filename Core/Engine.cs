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
			public int NumberOfSteps { get; set; }
			public int ImageWidth { get; set; }
			public int ImageHeight { get; set; }
        }

		public class RunConfig
		{
			public SKBitmap TargetBitmap { get; set; }
			public int Generations { get; set; }
			public int GenesPerGeneration { get; set; }
			public int GenesToReproduce { get; set; }
			public int GeneType { get; set; }
			public int MutationRangeMax { get; set; }
			public int NumberOfSteps { get; set; }
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
				this.UpdateStatus(true, false, "Engine is running", runConfig);

				// Remove the current engine output directory if it exists
				if (Directory.Exists(Utilities.EngineOutputDirectory))
					Directory.Delete(Utilities.EngineOutputDirectory, true);

				this.population = new Population(runConfig.TargetBitmap, runConfig);
				
				// Generate population
				switch (runConfig.GeneType)
				{
					case 1:
						population.GeneratePixelGenePopulation(runConfig);
						break;
					case 2:
						population.GenerateLineGenePopulation(runConfig);
						break;
				}

				var watch = System.Diagnostics.Stopwatch.StartNew();

				for (int i = 1; i <= runConfig.Generations; i++)
				{
					// Check to see if this run has been cancelled
					if (this.cancel)
					{
						// Update engine status
						this.UpdateStatus(false, true, "Engine was cancelled", runConfig);
						this.cancel = false;
						break;
					}

					this.population.EvaluateFitness();
					Gene topGene = this.population.NaturalSelection();

					// Save the image of the top gene
					Utilities.SaveFittestGeneForGeneration(topGene.Bitmap, i);

					// If this is the last generation, then save the step images for this top gene
					if (i == runConfig.Generations)
						topGene.SaveSteps();

					// Update engine status
					this.status.CurrentGeneration = i ;
				}

				watch.Stop();
				var elapsedMs = watch.ElapsedMilliseconds;
				System.Console.WriteLine($"Milliseconds: {elapsedMs}");

				this.population.EvaluateFitness();
				Utilities.SaveBitmap(this.population.BestGene.Bitmap, $"{Utilities.EngineOutputDirectory}/result.png");

				// Update engine status
				this.UpdateStatus(false, true, "Results are available", runConfig);
				this.cancel = false;
			});
        }

		public void Cancel()
		{
			this.cancel = true;
		}

		private void UpdateStatus(bool isRunning, bool resultsAvailable, string message, RunConfig runConfig)
		{
			this.status.IsRunning = isRunning;
			this.status.ResultsAvailable = resultsAvailable;
			this.status.Message = message;
			this.status.NumberOfSteps = runConfig.NumberOfSteps;
			this.status.ImageWidth = runConfig.TargetBitmap.Width;
			this.status.ImageHeight = runConfig.TargetBitmap.Height;
		}

        public Engine.Status GetStatus()
        {
            return this.status;
        }
    }
}