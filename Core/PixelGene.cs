using System;
using SkiaSharp;

namespace GeneticImages.Core
{
    public class PixelGene : Gene
    {
        public PixelGene(int width, int height, int mutationRangeMax)
            : base(width, height, mutationRangeMax)
        {
            
        }

        public override void InitRandomly()
        {
            for (int i = 0; i < this.Bitmap.Width; i++)
			{
				for (int j = 0; j < this.Bitmap.Height; j++)
				{
					this.Bitmap.SetPixel(i, j, new SKColor(
						Convert.ToByte(Utilities.Random.Next(0, 255)),	// Red
						Convert.ToByte(Utilities.Random.Next(0, 255)),	// Green
						Convert.ToByte(Utilities.Random.Next(0, 255)),	// Blue
						Convert.ToByte(Utilities.Random.Next(0, 255))	// Alpha
					));
				}
			}
        }

        public override Gene Crossover(Gene mate)
        {
			PixelGene childGene = new PixelGene(this.Bitmap.Width, this.Bitmap.Height, this.MutationRangeMax);
			PixelGene mateStaticGene = (PixelGene)mate;

			bool chooseFromThisGeneFirst = Utilities.Random.Next(0, 100) > 50;

			for (int i = 0; i < childGene.Bitmap.Width; i++)
			{
				for (int j = 0; j < childGene.Bitmap.Height; j++)
				{
					childGene.Bitmap.SetPixel(i, j, new SKColor(
						Utilities.Random.Next(0, this.MutationRangeMax) == this.MutationNumber ?
							Convert.ToByte(Utilities.Random.Next(0, 255)) :
							(chooseFromThisGeneFirst == true ? this.Bitmap.GetPixel(i, j).Red : mateStaticGene.Bitmap.GetPixel(i, j).Red),
						Utilities.Random.Next(0, this.MutationRangeMax) == this.MutationNumber ?
							Convert.ToByte(Utilities.Random.Next(0, 255)) :
							(chooseFromThisGeneFirst != true ? this.Bitmap.GetPixel(i, j).Green : mateStaticGene.Bitmap.GetPixel(i, j).Green),
						Utilities.Random.Next(0, this.MutationRangeMax) == this.MutationNumber ?
							Convert.ToByte(Utilities.Random.Next(0, 255)) :
							(chooseFromThisGeneFirst == true ? this.Bitmap.GetPixel(i, j).Blue : mateStaticGene.Bitmap.GetPixel(i, j).Blue),
						Utilities.Random.Next(0, this.MutationRangeMax) == this.MutationNumber ?
							Convert.ToByte(Utilities.Random.Next(0, 255)) :
							(chooseFromThisGeneFirst != true ? this.Bitmap.GetPixel(i, j).Alpha : mateStaticGene.Bitmap.GetPixel(i, j).Alpha)
					));
				}
			}

            return childGene;
        }
    }
}