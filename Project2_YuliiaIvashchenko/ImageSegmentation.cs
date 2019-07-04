using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Project2_YuliiaIvashchenko
{
    class ImageSegmentation
    {
        private int distance = 5;
        private int offset = 50;

        private static KMClusters clusters = new KMClusters();
        public ImageSegmentation() { }
        public Bitmap Compute(Bitmap picBitmap, int size)
        {
            clusters.Init(picBitmap, size, distance, offset);
            LockedBitmap rsltBitmap = new LockedBitmap(clusters[0].bitmapFrame.width, clusters[0].bitmapFrame.height);

            int frameIndex = 0;
            for (int i = 0; i < clusters.Count(); i++)
            {
                List<Point<int>> centroids = clusters[i].centroids.ToList();
                LockedBitmap frameBuffer = new LockedBitmap(clusters[i].bitmapFrame.bit);

                frameBuffer.LockBits();
                for (int c = 0; c < centroids.Count(); c++)
                {
                    int width = frameBuffer.width;
                    int height = frameBuffer.height;

                    LockedBitmap targetFrame = new LockedBitmap(frameBuffer.width, frameBuffer.height);
                    targetFrame.LockBits();

                    for (int row = 0; row < frameBuffer.width; row++)
                    {
                        for (int column = 0; column < height; column++)
                        {
                            double offset = Euclidian(new Point<int>(row, column, frameBuffer.GetPixel(row, column)),
                                                          new Point<int>(centroids[c].X, centroids[c].Y, centroids[c].Clr));
                            if (offset <= 50)
                            {
                                targetFrame.SetPixel(row, column, centroids[c].Clr);
                            }
                            else
                                targetFrame.SetPixel(row, column, Color.FromArgb(255, 255, 255));
                        }
                    }

                    targetFrame.UnlockBits();
                    List<Point<int>> targetCnts = new List<Point<int>>();
                    targetCnts.Add(centroids[0]);
                    Point<int> mean = clusters.GetMean(targetFrame, targetCnts);

                    if (mean.X != clusters[i].imCenter.X && mean.Y != clusters[i].imCenter.Y)
                        clusters.Add(targetFrame, targetCnts, mean);

                    frameIndex++;
                }

                frameBuffer.UnlockBits();
            }

            rsltBitmap.LockBits();
            for (int i = 0; i < clusters.Count(); i++)
            {
                LockedBitmap frameOut = new LockedBitmap(clusters[i].bitmapFrame.bit);

                frameOut.LockBits();
                int width = frameOut.width;
                int height = frameOut.height;
                for (int row = 0; row < width; row++)
                {
                    for (int column = 0; column < height; column++)
                    {
                        if (frameOut.GetPixel(row, column) != Color.FromArgb(255, 255, 255))
                        {
                            rsltBitmap.SetPixel(row, column, frameOut.GetPixel(row, column));
                        }
                    }
                }

                frameOut.UnlockBits();
            }

            rsltBitmap.UnlockBits();
            return rsltBitmap.bit;
        }

        public double Euclidian(Point<int> Point1, Point<int> Point2)
        {
            return Math.Sqrt(Math.Pow(Math.Abs(Point1.Clr.R - Point2.Clr.R), 2) +
                             Math.Pow(Math.Abs(Point1.Clr.G - Point2.Clr.G), 2) +
                             Math.Pow(Math.Abs(Point1.Clr.B - Point2.Clr.B), 2));
        }
    }
}
