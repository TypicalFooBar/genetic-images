using System.Collections.Generic;
using SkiaSharp;

namespace GeneticImages.Core
{
    public class LineGene : Gene
    {
        public List<Stroke> Strokes { get; set; } = new List<Stroke>();

        public LineGene(int width, int height) : base(width, height) {}

        public override void InitRandomly()
        {
            int numberOfStrokes = 250;

            for (int i = 0; i < numberOfStrokes; i++)
            {
                Stroke stroke = new Stroke();
                stroke.InitRandomly(this.Bitmap.Width, this.Bitmap.Height);
                
                this.Strokes.Add(stroke);
            }
        }

        public override Gene Crossover(Gene mate)
        {
            LineGene matePaintGene = (LineGene)mate;
            LineGene childGene = new LineGene(this.Bitmap.Width, this.Bitmap.Height);
            int mutationMax = 1000;

            int numberOfStrokes = 250;
            for (int i = 0; i < numberOfStrokes; i++)
            {
                Stroke stroke = new Stroke();
                stroke.Paint = 
                    Utilities.Random.Next(0, mutationMax) == 675 ?
                        Stroke.RandomPaint() :
                        (Utilities.Random.Next(0, 100) > 50 ? this.Strokes[i].Paint : matePaintGene.Strokes[i].Paint);
                stroke.Points =
                    Utilities.Random.Next(0, mutationMax) == 675 ?
                        Stroke.RandomPoints(this.Bitmap.Width, this.Bitmap.Height) :
                        (Utilities.Random.Next(0, 100) > 50 ? this.Strokes[i].Points : matePaintGene.Strokes[i].Points);

                childGene.Strokes.Add(stroke);
            }

            return childGene;
        }

        public override void Draw()
        {
			SKCanvas canvas = new SKCanvas(this.Bitmap);
			canvas.Clear(SKColors.Transparent);
			
            foreach (Stroke stroke in this.Strokes)
            {
				canvas.DrawLine(
					stroke.Points[0].X, stroke.Points[0].Y,
					stroke.Points[1].X, stroke.Points[1].Y,
					stroke.Paint
				);
            }
        }

		public override void SaveSteps()
		{
			SKCanvas canvas = new SKCanvas(this.Bitmap);
			canvas.Clear(SKColors.Transparent);
			
            for (int i = 0; i < this.Strokes.Count; i++)
            {
				canvas.DrawLine(
					this.Strokes[i].Points[0].X, this.Strokes[i].Points[0].Y,
					this.Strokes[i].Points[1].X, this.Strokes[i].Points[1].Y,
					this.Strokes[i].Paint
				);

				Utilities.SaveFittestGeneSteps(this.Bitmap, i+1);
            }
		}
    }
}