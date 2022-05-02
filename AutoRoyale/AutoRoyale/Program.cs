using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AutoRoyale
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Auto Royale";

            string title = @"
 █████  ██    ██ ████████  ██████      ██████   ██████  ██    ██  █████  ██      ███████ 
██   ██ ██    ██    ██    ██    ██     ██   ██ ██    ██  ██  ██  ██   ██ ██      ██      
███████ ██    ██    ██    ██    ██     ██████  ██    ██   ████   ███████ ██      █████   
██   ██ ██    ██    ██    ██    ██     ██   ██ ██    ██    ██    ██   ██ ██      ██      
██   ██  ██████     ██     ██████      ██   ██  ██████     ██    ██   ██ ███████ ███████ 
Public Version | Created by kite1101";

            Console.WriteLine(title);

            log("Loading config file");

            string cfgPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Config\\cfg.txt";

            if (!File.Exists(cfgPath))
            {
                log("Config file is in the wrong place or does not exist!");
                Console.ReadKey();
                return;
            }

            System.IO.StreamReader cfgFile = new System.IO.StreamReader(cfgPath);

            Console.WriteLine("------- Buttons where color is required -------");

            Point partyButton = new Point();
            partyButton.x = Convert.ToInt32(cfgFile.ReadLine());
            partyButton.y = Convert.ToInt32(cfgFile.ReadLine());
            partyButton.setCol(Convert.ToInt32(cfgFile.ReadLine()));
            Console.WriteLine($"Party Button: {partyButton.x}, {partyButton.y}, Col: {partyButton.getCol()}");

            Point challengeButton = new Point();
            challengeButton.x = Convert.ToInt32(cfgFile.ReadLine());
            challengeButton.y = Convert.ToInt32(cfgFile.ReadLine());
            challengeButton.setCol(Convert.ToInt32(cfgFile.ReadLine()));
            Console.WriteLine(
                $"Challenge Button: {challengeButton.x}, {challengeButton.y} Col: {challengeButton.getCol()}");

            Point restartButton = new Point();
            restartButton.x = Convert.ToInt32(cfgFile.ReadLine());
            restartButton.y = Convert.ToInt32(cfgFile.ReadLine());
            restartButton.setCol(Convert.ToInt32(cfgFile.ReadLine()));
            Console.WriteLine($"Restart Button: {restartButton.x}, {restartButton.y} Col: {restartButton.getCol()}");

            Point noMoreRewards = new Point();
            noMoreRewards.x = Convert.ToInt32(cfgFile.ReadLine());
            noMoreRewards.y = Convert.ToInt32(cfgFile.ReadLine());
            noMoreRewards.setCol(Convert.ToInt32(cfgFile.ReadLine()));
            Console.WriteLine(
                $"No More Rewards Alert: {noMoreRewards.x}, {noMoreRewards.y} Col: {restartButton.getCol()}");

            Console.WriteLine("------- Buttons where color is not required -------");

            Point point1 = new Point();
            point1.x = Convert.ToInt32(cfgFile.ReadLine());
            point1.y = Convert.ToInt32(cfgFile.ReadLine());
            Console.WriteLine($"Placement point 1: {point1.x}, {point1.y}");

            Point point2 = new Point();
            point2.x = Convert.ToInt32(cfgFile.ReadLine());
            point2.y = Convert.ToInt32(cfgFile.ReadLine());
            Console.WriteLine($"Placement point 2: {point2.x}, {point2.y}");

            Point troop1 = new Point();
            troop1.x = Convert.ToInt32(cfgFile.ReadLine());
            Console.WriteLine($"Troop 1: {troop1.x}");

            Point troop2 = new Point();
            troop2.x = Convert.ToInt32(cfgFile.ReadLine());
            Console.WriteLine($"Troop 2: {troop2.x}");

            Point troop3 = new Point();
            troop3.x = Convert.ToInt32(cfgFile.ReadLine());
            Console.WriteLine($"Troop 3: {troop3.x}");

            Point troop4 = new Point();
            troop4.x = Convert.ToInt32(cfgFile.ReadLine());
            Console.WriteLine($"Troop 4: {troop4.x}");

            int troopY = Convert.ToInt32(cfgFile.ReadLine());
            Console.WriteLine($"Troop Y: {troopY}");

            Win32 windowsTools = new Win32();

            log("Waiting for party button to be visible");

            bool inGame = false;
            bool switcher = false;
            Random r = new Random();
            while (true)
            {
                if (!inGame)
                {
                    if (partyButton.getCol() == windowsTools.GetPixelColor(partyButton.x, partyButton.y))
                    {
                        log("Clicking party button");
                        SendClick(partyButton);

                        bool waiting = true;
                        while (waiting)
                        {
                            if (challengeButton.getCol() ==
                                windowsTools.GetPixelColor(challengeButton.x, challengeButton.y))
                            {
                                log("Clicking challenge button");
                                SendClick(challengeButton);
                                waiting = false;
                                inGame = true;
                            }
                        }
                    }
                }
                else
                {
                    // check if shit is broken
                    if (partyButton.getCol() == windowsTools.GetPixelColor(partyButton.x, partyButton.y))
                    {
                        inGame = false;
                    }
                    else
                    {
                        // note to self: bot can click on cancel button and break
                        if (restartButton.getCol() == windowsTools.GetPixelColor(restartButton.x, restartButton.y))
                        {
                            log("Game has ended");
                            SendClick(restartButton);
                            inGame = false;
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(2000);

                            Point chosenTroop = new Point();
                            int rand = r.Next(1, 5);

                            switch (rand)
                            {
                                case 1:
                                    chosenTroop.x = troop1.x;
                                    chosenTroop.y = troopY;
                                    break;
                                case 2:
                                    chosenTroop.x = troop2.x;
                                    chosenTroop.y = troopY;
                                    break;
                                case 3:
                                    chosenTroop.x = troop3.x;
                                    chosenTroop.y = troopY;
                                    break;
                                case 4:
                                    chosenTroop.x = troop4.x;
                                    chosenTroop.y = troopY;
                                    break;
                            }

                            SendClick(chosenTroop);
                            log($"Playing Troop {rand}");
                            SendClick(switcher ? point1 : point2);
                            switcher = !switcher;
                        }
                    }
                }
            }
        }


        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;

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

        public static void SendClick(Point location)
        {
            SetCursorPos(location.x, location.y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
        }


        static void log(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[" + DateTime.Now + "] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);
        }
    }

    class Point
    {
        public int x;
        public int y;

        private Color color;

        public void setCol(int col)
        {
            color = Color.FromArgb(col);
        }

        public int getCol()
        {
            return color.ToArgb();
        }
    }

    sealed class Win32
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        public int GetPixelColor(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int) (pixel & 0x000000FF),
                (int) (pixel & 0x0000FF00) >> 8,
                (int) (pixel & 0x00FF0000) >> 16);
            return color.ToArgb();
        }
    }
}