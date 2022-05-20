using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;

namespace AutoRoyaleV2
{
    class Program
    {
        // Function used to get key presses
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        static void Main(string[] args)
        {
            // Variables
            #region
            string title = @"
 █████  ██    ██ ████████  ██████      ██████   ██████  ██    ██  █████  ██      ███████ 
██   ██ ██    ██    ██    ██    ██     ██   ██ ██    ██  ██  ██  ██   ██ ██      ██     
███████ ██    ██    ██    ██    ██     ██████  ██    ██   ████   ███████ ██      █████   
██   ██ ██    ██    ██    ██    ██     ██   ██ ██    ██    ██    ██   ██ ██      ██      
██   ██  ██████     ██     ██████      ██   ██  ██████     ██    ██   ██ ███████ ███████ 
Public Version | Created by ";

            // ASCII value of e character
            Int32 e = 69;
            #endregion

            // Set console name
            Console.Title = "Auto Royale V2";
            Console.Write(title);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("kite1101");
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.Write("With help from ");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("Wisp");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" and ");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Chobani");

            // Directory to config file
            string dir = Environment.CurrentDirectory + "\\config\\config.json";

            // Loading config file
            Utils.Config config = Utils.LoadJsonToConfig(dir);

            if (config == null)
            {
                Console.ReadLine();
                return;
            }

            // Print config information and set variables
            Pixel partyPixel = new Pixel(config.PartyButton.coords, config.PartyButton.color);
            Console.WriteLine($"Party Button: {partyPixel.x}, {partyPixel.y}, Col: {partyPixel.color}");
            Pixel challengePixel = new Pixel(config.ChallengeButton.coords, config.ChallengeButton.color);
            Console.WriteLine($"Challenge Button: {challengePixel.x}, {challengePixel.y}, Col: {challengePixel.color}");
            Pixel okEndPixel = new Pixel(config.OkEndButton.coords, config.OkEndButton.color);
            Console.WriteLine($"Restart Button: {okEndPixel.x}, {okEndPixel.y} Col: {okEndPixel.color}");
            Pixel rewardsPixel = new Pixel(config.RewardsButton.coords, config.RewardsButton.color);
            Console.WriteLine($"No More Rewards Alert: {rewardsPixel.x}, {rewardsPixel.y} Col: {rewardsPixel.color}");

            Console.WriteLine("------- Buttons where color is not required -------");
            // Print more config info and set variables
            // Placement positions
            Pixel pos1 = new Pixel(config.Pos1);
            Pixel pos2 = new Pixel(config.Pos2);
            Console.WriteLine($"Placement point 1: {pos1.x}, {pos1.y}");
            Console.WriteLine($"Placement point 2: {pos2.x}, {pos2.y}");

            // Cards positions
            List<Pixel> cards = new List<Pixel>();
            for (int i = 0; i < 4; i++)
            {
                cards.Add(new Pixel(new int[] { config.CardsXs[i], config.CardsY }));
                Console.WriteLine($"Troop {i}: {config.CardsXs[i]}");
            }

            // Variables to store game states
            bool inGame = false;
            bool switcher = false;
            int keyState;
            Random random = new Random();

            Utils.log("Waiting for party button to be visible");

            while (true)
            {
                // Stability
                Thread.Sleep(50);

                keyState = GetAsyncKeyState(e);
                if (keyState == 1 || keyState == -32767) // Min number of regular int
                {
                    Console.Write($"{(char)e} key typed in! Exiting program.");
                    Console.ReadLine();
                    Environment.Exit(0);
                }

                if (!inGame)
                {
                    if (partyPixel.color == Win32.GetCursorColorValue(partyPixel.x, partyPixel.y))
                    {
                        Utils.log("Clicking party button");
                        Win32.SendLeftClick(partyPixel);

                        bool waiting = true;
                        while (waiting)
                        {
                            if (challengePixel.color ==
                                Win32.GetCursorColorValue(challengePixel.x, challengePixel.y))
                            {
                                Utils.log("Clicking challenge button");
                                Win32.SendLeftClick(challengePixel);
                                waiting = false;
                                inGame = true;

                                System.Threading.Thread.Sleep(1000);
                                if (rewardsPixel.color ==
                                    Win32.GetCursorColorValue(rewardsPixel.x, rewardsPixel.y))
                                {
                                    Utils.log("Clicking no more rewards button");
                                    Win32.SendLeftClick(rewardsPixel);
                                }

                            }
                        }
                    }
                }
                else
                {
                    // Check if we are on first screen
                    if (partyPixel.color == Win32.GetCursorColorValue(partyPixel.x, partyPixel.y))
                    {
                        inGame = false;
                    }
                    else
                    {
                        // Check if Ok End button is visible
                        if (okEndPixel.color == Win32.GetCursorColorValue(okEndPixel.x, okEndPixel.y))
                        {
                            Utils.log("Game has ended");
                            Win32.SendLeftClick(okEndPixel);
                            Thread.Sleep(3000);
                            inGame = false;
                        }
                        else
                        {
                            // Play Loop
                            Thread.Sleep(2000);
                            // Pick number between 0 and 4 at random
                            int rand = random.Next(0, 4);
                            // Send click to such card
                            Win32.SendLeftClick(cards[rand]);
                            Utils.log($"Playing Troop {rand + 1}");
                            Thread.Sleep(300);
                            Win32.SendLeftClick(switcher ? pos1 : pos2);
                            switcher = !switcher;
                        }
                    }


                }
            }
        }

    }

    // Class to deal with pixels in screen
    sealed class Pixel
    {
        // Coordinates
        public int x;
        public int y;
        // Color 32-bit value
        public int color;

        public Pixel(int[] Coords, int pColor = 0)
        {
            this.x = Coords[0];
            this.y = Coords[1];
            this.color = pColor;
        }

    }

    // Class to handle low level windows interfacing
    sealed class Win32
    {
        // To read color from screen
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

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
        public static int GetCursorColorValue(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                (int)(pixel & 0x0000FF00) >> 8,
                (int)(pixel & 0x00FF0000) >> 16);

            return color.ToArgb();
        }

        // Sends left click to desired pixel lovarion
        public static void SendLeftClick(Pixel location)
        {
            SetCursorPos(location.x, location.y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
        }
    }


}

public class Utils
{
    // Function to write logs with date and time
    public static void log(object message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("[" + DateTime.Now + "] ");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(message);
    }

    // Config class to store configuration data
    public class Config
    {
        public Button PartyButton { get; set; }
        public Button ChallengeButton { get; set; }
        public Button OkEndButton { get; set; }
        public Button RewardsButton { get; set; }
        public int[] Pos1 { get; set; }
        public int[] Pos2 { get; set; }
        public int[] CardsXs { get; set; }
        public int CardsY { get; set; }

        public class Button
        {
            public int[] coords { get; set; }
            public int color { get; set; }

            // Defualt Button constructor
            public Button()
            {
                this.coords = new int[] { 0, 0 };
                this.color = 0;
            }
        }

        // Constructor that sets everything to zero
        public Config()
        {
            this.PartyButton = new Button();
            this.ChallengeButton = new Button();
            this.OkEndButton = new Button();
            this.RewardsButton = new Button();

            this.Pos1 = new int[] { 0, 0 };
            this.Pos2 = new int[] { 0, 0 };
            this.CardsXs = new int[] { 0, 0, 0, 0 }; ;
            this.CardsY = 0;
        }
    }

    public static Config LoadJsonToConfig(string filePath)
    {
        // Create config object
        Config config = new Config();

        // Check if json file with Path exists
        if (!File.Exists(filePath))
        {
            log("Config file is in the wrong place or does not exist!");
            log($"File to check: {filePath}");
            Console.ReadKey();
            return null;
        }

        StreamReader r = new StreamReader(filePath);
        string json = r.ReadToEnd();

        // Try to read config from json, if not posible, return empty config
        try
        {
            config = JsonSerializer.Deserialize<Config>(json);
        }
        catch
        {
            log("Unable to read config file! Make sure to fill with proper information.");
            config = new Config();
            return null;
        }

        log("Configuration was a success!");
        return config;

    }
}