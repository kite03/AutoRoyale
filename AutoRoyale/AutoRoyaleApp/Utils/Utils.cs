using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoRoyaleApp.Utils
{
    public class Utils
    {
        // Function to write logs with date and time
        public static void log(object message)
        {
            /*Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[" + DateTime.Now + "] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);*/
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
                config = new Config();
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
            }

            log("Configuration was a success!");
            return config;

        }
    }
}
