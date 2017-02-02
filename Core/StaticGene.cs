using ImageSharp;

namespace GeneticImages.Core
{
    public class StaticGene : Gene
    {
        public StaticGene(int width, int height)
            : base(width, height)
        {
            
        }

        public override void InitRandomly()
        {
            using (PixelAccessor<Color> pixels = this.Image.Lock())
            {
                for (int i = 0; i < pixels.Width; i++)
                {
                    for (int j = 0; j < pixels.Height; j++)
                    {
                        pixels[i, j] = new Color(
                            (float) Utilities.Random.Next(0, 255) / 255,
                            (float) Utilities.Random.Next(0, 255) / 255,
                            (float) Utilities.Random.Next(0, 255) / 255,
                            (float) Utilities.Random.Next(0, 255) / 255
                        );
                    }
                }
            }
        }

        public override Gene Crossover(Gene mate)
        {
            StaticGene childGene = new StaticGene(this.Image.Width, this.Image.Height);
            bool chooseFromThisGeneFirst = Utilities.Random.Next(0, 100) > 50;
            int mutationMax = 10000;

            using (PixelAccessor<Color> parent1Pixels = this.Image.Lock())
            using (PixelAccessor<Color> parent2Pixels = mate.Image.Lock())
            using (PixelAccessor<Color> childPixels = childGene.Image.Lock())
            {
                for (int i = 0; i < childPixels.Width; i++)
                {
                    for (int j = 0; j < childPixels.Height; j++)
                    {
                        childPixels[i, j] = new Color(
                            Utilities.Random.Next(0, mutationMax) == 675 ?
                                (float) Utilities.Random.Next(0, 255) / 255 :
                                (chooseFromThisGeneFirst == true ? (float) parent1Pixels[i, j].R / 255 : (float) parent2Pixels[i, j].R / 255),
                            Utilities.Random.Next(0, mutationMax) == 675 ?
                                (float) Utilities.Random.Next(0, 255) / 255 :
                                (chooseFromThisGeneFirst != true ? (float) parent1Pixels[i, j].G / 255 : (float) parent2Pixels[i, j].G / 255),
                            Utilities.Random.Next(0, mutationMax) == 675 ?
                                (float) Utilities.Random.Next(0, 255) / 255 :
                                (chooseFromThisGeneFirst == true ? (float) parent1Pixels[i, j].B / 255 : (float) parent2Pixels[i, j].B / 255),
                            Utilities.Random.Next(0, mutationMax) == 675 ?
                                (float) Utilities.Random.Next(0, 255) / 255 :
                                (chooseFromThisGeneFirst != true ? (float) parent1Pixels[i, j].A / 255 : (float) parent2Pixels[i, j].A / 255)
                        );
                    }
                }
            }

            return childGene;
        }
    }
}