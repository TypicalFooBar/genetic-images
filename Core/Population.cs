using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SkiaSharp;

namespace GeneticImages.Core
{
    public class Population
    {
        private List<Gene> genes = new List<Gene>();
        private int numberOfGenes;
		private int numberOfGenesToReproduce;
        private SKBitmap targetBitmap;

        public int BestFitness {
            get {
                return (
                    from g in this.genes
                    orderby g.Fitness descending
                    select g
                ).First().Fitness;
            }
        }

        public Gene BestGene {
            get {
                return (
                    from g in this.genes
                    orderby g.Fitness descending
                    select g
                ).FirstOrDefault();
            }
        }

        public Population(SKBitmap targetBitmap, Engine.RunConfig runConfig)
        {
            this.targetBitmap = targetBitmap;
			this.numberOfGenes = runConfig.GenesPerGeneration;
			this.numberOfGenesToReproduce = runConfig.GenesToReproduce;

            Utilities.SaveBitmap(this.targetBitmap, $"{Utilities.EngineOutputDirectory}/target.png");
        }

        public void GeneratePixelGenePopulation(Engine.RunConfig runConfig)
        {
            for (int i = 0; i < this.numberOfGenes; i++)
            {
                Gene gene = new PixelGene(this.targetBitmap.Width, this.targetBitmap.Height, runConfig.MutationRangeMax);
                gene.InitRandomly();
                this.genes.Add(gene);
            }
        }

        public void GenerateLineGenePopulation(Engine.RunConfig runConfig)
        {
            for (int i = 0; i < this.numberOfGenes; i++)
            {
                Gene gene = new LineGene(this.targetBitmap.Width, this.targetBitmap.Height, runConfig.MutationRangeMax, runConfig.NumberOfStrokes);
                gene.InitRandomly();
                this.genes.Add(gene);
            }
        }

        public Gene NaturalSelection()
        {
            // Get the top genes
            List<Gene> topGenes = (
                from g in this.genes
                orderby g.Fitness descending
                select g
            ).Take(this.numberOfGenesToReproduce).ToList();

            // Set the current population to be the new genes
            this.genes = ReproduceGenes(topGenes);

			return topGenes[0];
        }

        private static object _lock = new object();

        private List<Gene> ReproduceGenes(List<Gene> genePool)
        {
            // The list of new genes
            List<Gene> newGenes = new List<Gene>();

            // List<Task> tasks = new List<Task>();

            // int numberOfChildren = 0;
            // Reproduce the genes
            foreach (Gene gene1 in genePool)
            {
                foreach (Gene gene2 in genePool)
                {
                    if (gene1 != gene2)
                    {
                        // tasks.Add(Task.Run(() => {
                        //     Gene gene = gene1.Crossover(gene2);

                        //     lock(_lock)
                        //     {
                        //         newGenes.Add(gene);
                        //     }
                        // }));
                        Gene gene = gene1.Crossover(gene2);
                        newGenes.Add(gene);
                    }

                    // numberOfChildren++;

                    if (newGenes.Count >= this.numberOfGenes)
                    {
                        // Task.WaitAll(tasks.ToArray());

                        return newGenes;
                    }
                }
            }

            //throw new Exception("You shouldn't be here!!!");

            return newGenes;
        }

        public void EvaluateFitness()
        {
            List<Task> tasks = new List<Task>();

            foreach (Gene gene in this.genes)
            {
                tasks.Add(Task.Run(() => {
                    gene.Draw();
                    gene.EvaluateFitness(this.targetBitmap); 
                }));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}