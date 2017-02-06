using System;
using SkiaSharp;

namespace GeneticImages.Core
{
    public abstract class Gene
    {
		public SKBitmap Bitmap { get; protected set; }
        public int Fitness { get; protected set; }

        public Gene(int width, int height)
        {
            this.Bitmap = new SKBitmap(width, height);
        }

        public abstract void InitRandomly();

        public abstract Gene Crossover(Gene mate);

        public virtual void Draw() {}

        public void EvaluateFitness(SKBitmap targetBitmap)
        {
            this.Fitness = 0;

			SKColor[] pixels = this.Bitmap.Pixels;
			SKColor[] targetPixels = targetBitmap.Pixels;

			for (int i = 0; i < pixels.Length; i++)
			{
				// Add score for how close they are to the correct value
				this.Fitness += 255 - Math.Abs(targetPixels[i].Red - pixels[i].Red);
				this.Fitness += 255 - Math.Abs(targetPixels[i].Green - pixels[i].Green);
				this.Fitness += 255 - Math.Abs(targetPixels[i].Blue - pixels[i].Blue);
				this.Fitness += 255 - Math.Abs(targetPixels[i].Alpha - pixels[i].Alpha);
			}
        }
    }
}