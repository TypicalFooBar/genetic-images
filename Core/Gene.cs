using System;
using SkiaSharp;

namespace GeneticImages.Core
{
    public abstract class Gene
    {
		public SKBitmap Bitmap { get; protected set; }
        public int Fitness { get; protected set; }
		public int MutationRangeMax { get; protected set; }
		protected int MutationNumber { get; set; }

        public Gene(int width, int height, int mutationRangeMax)
        {
            this.Bitmap = new SKBitmap(width, height);
			this.MutationRangeMax = mutationRangeMax;
			this.MutationNumber = Utilities.Random.Next(0, this.MutationRangeMax);
        }

        public abstract void InitRandomly();

        public abstract Gene Crossover(Gene mate);

        public virtual void Draw() {}

		public virtual void SaveSteps() {}

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