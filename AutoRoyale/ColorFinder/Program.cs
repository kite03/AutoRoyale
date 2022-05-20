using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using System.Runtime.InteropServices;
using System.Windows;
using System.Drawing;

namespace ColorFinder
{
    internal class Program
    {
        public struct POINT
        {
            public int X;
            public int Y;
        }


        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        static void Main(string[] args)
        {
            while (true)
            {
                POINT p; 
                GetCursorPos(out p);
                Console.WriteLine($"{p.X}, {p.Y} Col: {getCol(p.X, p.Y)}");

            }
        }

        static int getCol(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                (int)(pixel & 0x0000FF00) >> 8,
                (int)(pixel & 0x00FF0000) >> 16);
            return color.ToArgb();
        }
    }
}
