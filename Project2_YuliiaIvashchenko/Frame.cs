using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2_YuliiaIvashchenko
{
    class Frame
    {
        public Frame(LockedBitmap bFrame, List<Point<int>> centroids, Point<int> center)
        {
            this.bitmapFrame = bFrame;
            this.imCentroids = centroids;
            this.imCenter = center;
        }
        public LockedBitmap bitmapFrame
        {
            get { return imBitmapFrame; }
            set { imBitmapFrame = value; }
        }
        public List<Point<int>> centroids
        {
            get { return imCentroids; }
            set { imCentroids = value; }
        }
        public Point<Int32> imCenter
        {
            get { return center; }
            set { center = value; }
        }

        private LockedBitmap imBitmapFrame = null;
        private Point<Int32> center;
        private List<Point<int>> imCentroids = null;
    }
}
