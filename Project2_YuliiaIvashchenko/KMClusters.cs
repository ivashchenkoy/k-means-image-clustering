using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Drawing;

namespace Project2_YuliiaIvashchenko
{
    class KMClusters : IEnumerable<Frame>
    {
        private readonly System.Random rand = new System.Random();
        private static HashSet<Frame> clusters = new HashSet<Frame>();
        public void Init(Bitmap picBitmap, int size, int distance, int offset)
        {
            LockedBitmap frameBuffer = new LockedBitmap(picBitmap);
            List<Point<int>> centroids = new List<Point<int>>();

            this.Generate(ref centroids, frameBuffer, size, distance, offset);

            Point<int> mean = this.GetMean(frameBuffer, centroids);
            clusters.Add(new Frame(frameBuffer, centroids, mean));
        }
        public void Generate(ref List<Point<int>> centroids, LockedBitmap imageFrame, int size, int distance, int offset)
        {
            imageFrame.LockBits();

            for (int i = 0; i < size; i++)
            {
                int rand_x = rand.Next(0, imageFrame.width);
                int rand_y = rand.Next(0, imageFrame.height);

                Point<int> rand_p = new Point<int>(rand_x,
                              rand_y, imageFrame.GetPixel(rand_x, rand_y));

                if (!this.IsValidColor(centroids, rand_p, offset) && !this.IsValidDistance(centroids, rand_p, distance))
                {
                    if (!centroids.Contains(rand_p))
                    {
                        centroids.Add(rand_p);
                    }
                }
            }

            imageFrame.UnlockBits();
        }
        private bool IsValidDistance(List<Point<int>> points, Point<int> target, int distance)
        {
            int i = -1;
            bool exists = false;
            while (++i < points.Count() && !exists)
                exists = ((Math.Abs(target.X - points.ElementAt(i).X) <= distance) ||
                          (Math.Abs(target.Y - points.ElementAt(i).Y) <= distance)) ? true : false;

            return exists;
        }
        private bool IsValidColor(List<Point<int>> points, Point<int> target, int offset)
        {
            int i = -1;
            bool exists = false;
            while (++i < points.Count() && !exists)
                exists = (Math.Sqrt(Math.Pow(Math.Abs(points[i].Clr.R - target.Clr.R), 2) +
                                    Math.Pow(Math.Abs(points[i].Clr.G - target.Clr.G), 2) +
                                    Math.Pow(Math.Abs(points[i].Clr.B - target.Clr.B), 2))) <= offset ? true : false;

            return exists;
        }
        public Point<int> GetMean(LockedBitmap frameBuffer, List<Point<int>> centroids)
        {
            double mean_x = 0;
            double mean_y = 0;
            for (int i = 0; i < centroids.Count(); i++)
            {
                mean_x += centroids[i].X / (double)centroids.Count();
                mean_y += centroids[i].Y / (double)centroids.Count();
            }

            int x = Convert.ToInt32(mean_x);
            int y = Convert.ToInt32(mean_y);

            frameBuffer.LockBits();
            Color c = frameBuffer.GetPixel(x, y);
            frameBuffer.UnlockBits();

            return new Point<int>(x, y, c);
        }
        public void Add(LockedBitmap frameImage, List<Point<int>> centroids, Point<int> center)
        {
            clusters.Add(new Frame(frameImage, centroids, center));
        }

        public Frame this[int i]
        {
            get { return clusters.ElementAt(i); }
        }

        public IEnumerator<Frame> GetEnumerator()
        {
            return clusters.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
