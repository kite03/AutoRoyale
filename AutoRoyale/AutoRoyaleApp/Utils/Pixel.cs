using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRoyaleApp.Utils
{
    // Class to deal with pixels in screen
    sealed class Pixel
    {
        // Coordinates
        public int x;
        public int y;
        // Color 32-bit value
        public int color;
        public Pixel()
        {
            this.x = 0;
            this.y = 0;
            this.color = 0;
        }

        public Pixel(int[] Coords, int pColor = 0)
        {
            this.x = Coords[0];
            this.y = Coords[1];
            this.color = pColor;
        }
    }
}
