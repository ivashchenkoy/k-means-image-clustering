using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Project2_YuliiaIvashchenko
{
    class Point<T>
    {
        public Point(T X, T Y, Color Clr) { this.X = X; this.Y = Y; this.Clr = Clr; }
        public T X { get { return xx; } set { xx = value; } }
        public T Y { get { return yy; } set { yy = value; } }
        public Color Clr { get { return c; } set { c = value; } }

        private T xx;
        private T yy;
        private Color c;
    }
}
