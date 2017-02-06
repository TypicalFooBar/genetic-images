using SkiaSharp;

namespace GeneticImages.Core
{
    public class Engine
    {
        public class Status
        {
            public int CurrentGeneration { get; set; }
        }

        Population population;

        public Engine()
        {

        }

        public void Run(SKBitmap targetBitmap)
        {
            // Remove the current engine output directory
            //Directory.Delete("engine-output");

            population = new Population(targetBitmap);
            //population.GenerateStaticGenePopulation();
            population.GeneratePaintGenePopulation();

            while (population.CurrentGeneration <= 1000)
            {
                population.EvaluateFitness();

                //Console.WriteLine("Current Best Fitness [Generation " + population.CurrentGeneration + "]: " + population.BestFitness);

                population.NaturalSelection();
            }

            population.EvaluateFitness();
            Utilities.SaveBitmapAsPng(population.BestGene.Bitmap, "result");
        }

        public Engine.Status GetStatus()
        {
            return new Engine.Status {
                CurrentGeneration = this.population == null ? 0 : this.population.CurrentGeneration - 1
            };
        }
    }
}