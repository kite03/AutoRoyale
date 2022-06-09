using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static AutoRoyaleApp.Utils.ConfigFile;

namespace AutoRoyaleApp.Utils
{
    // Class to handle low level windows interfacing
    sealed class Win32
    {
        // To read color from screen
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);



        // To carry out mouse movement and simulation
        [DllImport("user32.dll")]
        private static extern void mouse_event(
            UInt32 dwFlags, // motion and click options
            UInt32 dx, // horizontal position or change
            UInt32 dy, // vertical position or change
            UInt32 dwData, // wheel movement
            IntPtr dwExtraInfo // application-defined information
        );

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;




        // Reads color from cursor and converts it to int
        public static int GetCursorColorValue(IGButton i)
        {
            int x = i.X;
            int y = i.Y;   
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                (int)(pixel & 0x0000FF00) >> 8,
                (int)(pixel & 0x00FF0000) >> 16);

            return color.ToArgb();
        }

        // Sends left click to desired pixel lovarion
        public static void SendLeftClick(IGButton i)
        {
            int x = i.X;
            int y = i.Y;
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
        }

        // Sends left click to desired location
        public static void SendLeftClickXY(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
        }

        public static int getCol(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                (int)(pixel & 0x0000FF00) >> 8,
                (int)(pixel & 0x00FF0000) >> 16);
            return color.ToArgb();
        }

        public static int getColClass(IGButton e)
        {
            int x = e.X;
            int y = e.Y;
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
