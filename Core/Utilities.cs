using System;
using System.IO;
using ImageSharp;

namespace GeneticImages.Core
{
    public static class Utilities
    {
        private static string OutputDirectoryName = "engine-output";

        public static Random Random = new Random();

        public static void SaveImage(Image image, string filename)
        {
            if (!Directory.Exists(OutputDirectoryName))
                Directory.CreateDirectory(OutputDirectoryName);

            using (FileStream stream = new FileStream(OutputDirectoryName + "/" + filename, FileMode.Create))
            {
                image.Save(stream);
            }
        }
    }
}