using System.Collections.Generic;
using System.Numerics;
using ImageSharp;
using ImageSharp.Drawing.Brushes;

namespace GeneticImages.Core
{
    public class Stroke
    {
        public SolidBrush Brush { get; set; }
        public Vector2[] Points { get; set; }
        public float Thickness { get; set; }

        public void InitRandomly(int width, int height)
        {
            this.Brush = Stroke.RandomBrush();
            this.Points = Stroke.RandomPoints(width, height);
            this.Thickness = Stroke.RandomThickness();
        }

        public static SolidBrush RandomBrush()
        {
            return new SolidBrush(new Color(
                (float) Utilities.Random.Next(0, 255) / 255,
                (float) Utilities.Random.Next(0, 255) / 255,
                (float) Utilities.Random.Next(0, 255) / 255,
                (float) Utilities.Random.Next(0, 220) / 255
            ));
        }

        public static Vector2[] RandomPoints(int width, int height)
        {
            List<Vector2> points = new List<Vector2>();

            for (int j = 0; j < 2; j++)
            {
                points.Add(new Vector2(
                    Utilities.Random.Next(0, width - 2),
                    Utilities.Random.Next(0, height - 2)
                ));
            }

            return points.ToArray();
        }

        public static float RandomThickness()
        {
            return (float) Utilities.Random.Next(1, 5);
        }
    }

    public class PaintGene : Gene
    {
        public List<Stroke> Strokes { get; set; } = new List<Stroke>();

        public PaintGene(int width, int height) : base(width, height) {}

        public override void InitRandomly()
        {
            int numberOfStrokes = 250;

            for (int i = 0; i < numberOfStrokes; i++)
            {
                Stroke stroke = new Stroke();
                stroke.InitRandomly(this.Image.Width, this.Image.Height);
                
                this.Strokes.Add(stroke);
            }
        }

        public override Gene Crossover(Gene mate)
        {
            PaintGene matePaintGene = (PaintGene)mate;
            PaintGene childGene = new PaintGene(this.Image.Width, this.Image.Height);
            int mutationMax = 1000;

            int numberOfStrokes = 250;
            for (int i = 0; i < numberOfStrokes; i++)
            {
                Stroke stroke = new Stroke();
                stroke.Brush = 
                    Utilities.Random.Next(0, mutationMax) == 675 ?
                        Stroke.RandomBrush() :
                        (Utilities.Random.Next(0, 100) > 50 ? this.Strokes[i].Brush : matePaintGene.Strokes[i].Brush);
                stroke.Points =
                    Utilities.Random.Next(0, mutationMax) == 675 ?
                        Stroke.RandomPoints(this.Image.Width, this.Image.Height) :
                        (Utilities.Random.Next(0, 100) > 50 ? this.Strokes[i].Points : matePaintGene.Strokes[i].Points);
                stroke.Thickness =
                    Utilities.Random.Next(0, mutationMax) == 675 ?
                        Stroke.RandomThickness() :
                        (Utilities.Random.Next(0, 100) > 50 ? this.Strokes[i].Thickness : matePaintGene.Strokes[i].Thickness);

                childGene.Strokes.Add(stroke);
            }

            return childGene;
        }

        public override void Draw()
        {
            foreach (Stroke stroke in this.Strokes)
            {
                this.Image.BackgroundColor(Color.White).DrawLines(
                    stroke.Brush,
                    stroke.Thickness,
                    stroke.Points
                );
            }
        }
    }
}