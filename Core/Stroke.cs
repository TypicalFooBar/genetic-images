using System.Collections.Generic;
using SkiaSharp;

namespace GeneticImages.Core
{
    public class Stroke
    {
		public SKPaint Paint { get; set; }
        public List<SKPoint> Points { get; set; }

        public void InitRandomly(int width, int height)
        {
            this.Paint = Stroke.RandomPaint();
            this.Points = Stroke.RandomPoints(width, height);
        }

        public static SKPaint RandomPaint()
        {
            SKPaint paint =  new SKPaint();
			paint.StrokeWidth = Utilities.Random.Next(1, 10);
			paint.IsAntialias = true;
			paint.Color = new SKColor(
				(byte)Utilities.Random.Next(0, 255),
				(byte)Utilities.Random.Next(0, 255),
				(byte)Utilities.Random.Next(0, 255),
				(byte)Utilities.Random.Next(0, 255)
			);
			//paint.BlendMode = SKBlendMode.Darken;

			return paint;
        }

        public static List<SKPoint> RandomPoints(int width, int height)
        {
            List<SKPoint> points = new List<SKPoint>();

            for (int j = 0; j < 2; j++)
            {
                points.Add(new SKPoint(
                    Utilities.Random.Next(0, width),
                    Utilities.Random.Next(0, height)
                ));
            }

            return points;
        }
    }
}