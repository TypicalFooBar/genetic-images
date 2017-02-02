using System;
using ImageSharp;

namespace GeneticImages.Core
{
    public abstract class Gene
    {
        public Image Image { get; protected set; }
        public int Fitness { get; protected set; }

        public Gene(int width, int height)
        {
            this.Image = new Image(width, height);
        }

        public abstract void InitRandomly();

        public abstract Gene Crossover(Gene mate);

        public virtual void Draw() {}

        public void EvaluateFitness(Image targetImage)
        {
            this.Fitness = 0;

            using (PixelAccessor<Color> targetPixels = targetImage.Lock())
            using (PixelAccessor<Color> pixels = this.Image.Lock())
            {
                for (int i = 0; i < pixels.Width; i++)
                {
                    for (int j = 0; j < pixels.Height; j++)
                    {
                        // Add score for how close they are to the correct value
                        this.Fitness += 255 - Math.Abs(targetPixels[i, j].R - pixels[i, j].R);
                        this.Fitness += 255 - Math.Abs(targetPixels[i, j].G - pixels[i, j].G);
                        this.Fitness += 255 - Math.Abs(targetPixels[i, j].B - pixels[i, j].B);
                        this.Fitness += 255 - Math.Abs(targetPixels[i, j].A - pixels[i, j].A);
                    }
                }
            }
        }
    }
}