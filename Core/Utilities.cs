using System;
using System.IO;
using SkiaSharp;

namespace GeneticImages.Core
{
    public static class Utilities
    {
        private static string OutputDirectoryName = "engine-output";

        public static Random Random = new Random();

        public static void SaveBitmapAsPng(SKBitmap bitmap, string filename)
        {
            if (!Directory.Exists(OutputDirectoryName))
                Directory.CreateDirectory(OutputDirectoryName);

			using (SKImage image = SKImage.FromBitmap(bitmap))
			using (SKData data = image.Encode(SKImageEncodeFormat.Png, 100))
			using (Stream stream = File.OpenWrite($"{OutputDirectoryName}/{filename}.png"))
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