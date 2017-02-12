using System;
using System.IO;
using SkiaSharp;

namespace GeneticImages.Core
{
    public static class Utilities
    {
        public static string EngineOutputDirectory { get; } = "engine-output";
		public static string FittestGeneForGenerationDirectory { get; } = "fittest-gene-for-generation";
		public static string FittestGeneStepsDirectory { get; } = "fittest-gene-steps";

        public static Random Random = new Random();

        public static void SaveFittestGeneForGeneration(SKBitmap bitmap, int generation)
        {
			// Save the file
			SaveBitmap(bitmap, $"{EngineOutputDirectory}/{FittestGeneForGenerationDirectory}/{generation}.png");			
        }

		public static void SaveFittestGeneSteps(SKBitmap bitmap, int step)
		{
			// Save the file
			SaveBitmap(bitmap, $"{EngineOutputDirectory}/{FittestGeneStepsDirectory}/{step}.png");
		}

		public static void SaveBitmap(SKBitmap bitmap, string filename)
		{
			// Make sure the directories exist
			Directory.CreateDirectory(Path.GetDirectoryName(filename));

			using (SKImage image = SKImage.FromBitmap(bitmap))
			using (SKData data = image.Encode(SKImageEncodeFormat.Png, 100))
			using (Stream stream = File.OpenWrite(filename))
			{
				data.SaveTo(stream);
			}
		}

		public static SKBitmap LoadBitmap(Stream stream)
        {
            using (SKManagedStream s = new SKManagedStream(stream))
            {
                using (SKCodec codec = SKCodec.Create(s))
                {
                    SKImageInfo info = codec.Info;
                    SKBitmap bitmap = SKBitmap.Decode(codec, info);

					return bitmap;
                }
            }
        }
    }
}