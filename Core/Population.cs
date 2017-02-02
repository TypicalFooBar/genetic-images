using System.Collections.Generic;
using ImageSharp;
using System.Linq;
using System.Threading.Tasks;

namespace GeneticImages.Core
{
    public class Population
    {
        private List<Gene> genes = new List<Gene>();
        public int CurrentGeneration { get; private set; } = 1;
        private int numberOfGenes = 100;
        private Image targetImage;

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

        public Population(Image targetImage)
        {
            this.targetImage = targetImage;

            Utilities.SaveImage(this.targetImage, "target.png");
        }

        public void GenerateStaticGenePopulation()
        {
            for (int i = 0; i < this.numberOfGenes; i++)
            {
                Gene gene = new StaticGene(this.targetImage.Width, this.targetImage.Height);
                gene.InitRandomly();
                this.genes.Add(gene);
            }
        }

        public void GeneratePaintGenePopulation()
        {
            for (int i = 0; i < this.numberOfGenes; i++)
            {
                Gene gene = new PaintGene(this.targetImage.Width, this.targetImage.Height);
                gene.InitRandomly();
                this.genes.Add(gene);
            }
        }

        public void NaturalSelection()
        {
            // Get the top genes
            List<Gene> topGenes = (
                from g in this.genes
                orderby g.Fitness descending
                select g
            ).Take(10).ToList();

            // Save the image of the top gene
            //if (this.CurrentGeneration % 100 == 0)
            Utilities.SaveImage(topGenes[0].Image, "/generation-" + this.CurrentGeneration + "-gene"+ ".png");

            // Set the current population to be the new genes
            this.genes = ReproduceGenes(topGenes);

            // Increment the generation counter
            this.CurrentGeneration++;
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
                    gene.EvaluateFitness(this.targetImage); 
                }));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}