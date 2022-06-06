using AutoRoyaleApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using static AutoRoyaleApp.Utils.ConfigFile;
using static AutoRoyaleApp.Utils.Utils;

namespace AutoRoyaleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region vars
        // Enums for passing wich button was clicked
        enum buttonClicked
        {
            PartyButton,
            ChallengeButton,
            OkEndButton,
            RewardsButton
        }
        // Function used to get key presses
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 vKey);

        // ASCII value of e character
        Int32 e = 69;

        // Directory to config file
        string dir = Environment.CurrentDirectory + "\\config\\config.json";

        // Config file
        private ConfigFile config = new ConfigFile();

        // Variables to store game states
        bool inGame = false;
        bool switcher = false;
        Point currentMousePosition = new Point(0, 0);
        int keyState;

        // Listener bools
        bool PartyButtonClicked = false;
        bool ChallengeButtonClicked = false;
        bool OkEndButtonClicked = false;
        bool RewardsButtonClicked = false;
        bool Cards = false;
        bool addPosition = false;
        bool BotRunning = false;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AutoRoyale1_Loaded(object sender, RoutedEventArgs e)
        {
            // Loading config file
            config = JsonSerializer.Deserialize<ConfigFile>(File.ReadAllText(dir));
            if (config is null)
            {
                MessageBox.Show("Something went wrong while loading config file.", "Error getting config");
            }
            else
            {
                // Placement positions
                foreach (CardPlacePosition pos in config.PlacePositions)
                {
                    CardPosList.Items.Add(pos.Name);
                }
            }
            updateBtnTxtInfo();
        }

        #region MousePositionLogic
        // Mouse position 
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        // Gets mouse position
        public static Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);

            return new Point(w32Mouse.X, w32Mouse.Y);
        }
        #endregion 

        // Occurs when form is closing
        private void AutoRoyale1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        // Methods for when buttons are clicked in application
        #region ButtonClicks

        // Occurs when select partybutton is clicked
        private void PB_Select_btn_Click(object sender, RoutedEventArgs e)
        {
            if (ChallengeButtonClicked || OkEndButtonClicked || RewardsButtonClicked)
            {
                MessageBox.Show("Please only select 1 button at a time.","Error");
                return;
            }
            PartyButtonClicked = !PartyButtonClicked;
            if (!PartyButtonClicked)
            {
                // Change button text
                PB_Select_btn.Content = "Select";
                return;
            }
            // Change button text
            PB_Select_btn.Content = "Cancel";
            // Start listener for button information
            setButtonLocation();
        }

        // Occurs when select challengebutton is clicked
        private void CB_Select_btn_Click(object sender, RoutedEventArgs e)
        {
            if (PartyButtonClicked || OkEndButtonClicked || RewardsButtonClicked)
            {
                MessageBox.Show("Please only select 1 button at a time.", "Error");
                return;
            }
            ChallengeButtonClicked = !ChallengeButtonClicked;
            if (!ChallengeButtonClicked)
            {
                // Change button text
                CB_Select_btn.Content = "Select";
                return;
            }
            // Change button text
            CB_Select_btn.Content = "Cancel";
            // Start listener for button information
            setButtonLocation();
        }

        // Occurs when select OkEndButton is clicked
        private void OB_Select_btn_Click(object sender, RoutedEventArgs e)
        {
            if (ChallengeButtonClicked || PartyButtonClicked || RewardsButtonClicked)
            {
                MessageBox.Show("Please only select 1 button at a time.", "Error");
                return;
            }
            OkEndButtonClicked = !OkEndButtonClicked;
            if (!OkEndButtonClicked)
            {
                // Change button text
                OB_Select_btn.Content = "Select";
                return;
            }
            // Change button text
            OB_Select_btn.Content = "Cancel";
            // Start listener for button information
            setButtonLocation();
        }

        // Occurs when select RewardsButton is clicked
        private void RB_Select_btn_Click(object sender, RoutedEventArgs e)
        {
            if (ChallengeButtonClicked || OkEndButtonClicked || PartyButtonClicked)
            {
                MessageBox.Show("Please only select 1 button at a time.", "Error");
                return;
            }
            RewardsButtonClicked = !RewardsButtonClicked;
            if (!RewardsButtonClicked)
            {
                // Change button text
                RB_Select_btn.Content = "Select";
                return;
            }
            // Change button text
            RB_Select_btn.Content = "Cancel";
            // Start listener for button information
            setButtonLocation();
        }
        
        // Save to config file
        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {
            string jsonFile = JsonSerializer.Serialize<ConfigFile>(config);
            File.WriteAllText(dir, jsonFile);
        }

        // Credits for making this
        private void Creds_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Made by iVyperion, with help from: kite1101, Wisp and Chobani", "Credits");
        }

        // Occurs when Cards slot position button is clicked
        private void CS_CardsY_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Cards == true)
            {
                CS_CardsY_btn.Content = "Select";
                Cards = false;
                return;
            }
            Cards = !Cards;
            CS_CardsY_btn.Content = "Stop";
            ActivateCardSlotListener();
        }

        // Occurs when add position button is clicked
        private void AddPos_btn_Click(object sender, RoutedEventArgs e)
        {
            if (addPosition)
            {
                AddPos_btn.Content = "Add position";
                addPosition = false;
                return;
            }
            addPosition = !addPosition;
            AddPos_btn.Content = "Selecting";
            addPostitionFunc();
        }

        // Occurs when delete position button is clicked
        private void DelPos_btn_Click(object sender, RoutedEventArgs e)
        {
            if (CardPosList.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a postition first.", "Error");
                return;
            }
            int selPosIndex = CardPosList.SelectedIndex;
            string selPosName = CardPosList.SelectedItem.ToString();
            foreach (CardPlacePosition pos in config.PlacePositions.ToArray())
            {
                if (pos.Name == selPosName)
                {
                    config.PlacePositions.Remove(pos);
                }
            }
            CardPosList.Items.RemoveAt(selPosIndex);
        }

        // Occurs when show details of position button is clicked
        private void ShowPosDet_btn_Click(object sender, RoutedEventArgs e)
        {
            if (CardPosList.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a postition first.", "Error");
                return;
            }
            string selPosName = (string)CardPosList.SelectedValue;
            foreach (CardPlacePosition pos in config.PlacePositions)
            {
                if (pos.Name == selPosName)
                {
                    MessageBox.Show($"Position {pos.Name} is located on: {pos.X} - {pos.Y}", $"Position {pos.Name}");
                }
            }
        }

        // Starts the bot
        private void StartBot_bnt_Click(object sender, RoutedEventArgs e)
        {
            if (BotRunning)
            {
                StartBot_bnt.Content = "Start Bot";
                BotRunning = false;
                return;
            }
            BotRunning = !BotRunning;
            StartBot_bnt.Content = "Stop Bot";
            StartTheBot();
        }
        #endregion

        // Logic for setting up ingame button
        #region Buttons logic
        // Method that when key is pressed passes through mouse location and color information 
        private void keyWasPressed(Point e)
        {
            int moX = Convert.ToInt32(e.X);
            int moY = Convert.ToInt32(e.Y);
            IGButton m = new IGButton();
            int[] coords = new int[2];
            coords[0] = moX;
            coords[1] = moY;
            int color = Win32.getCol(moX, moY);//Win32.GetCursorColorValue(m);
            Pixel temp = new Pixel(coords, color);
            setButtonInformation(temp);
        }

        // Async method that listens for key to set ingame buttons locations
        private async void setButtonLocation()
        {
            Point p = new Point(0, 0);
            bool pressed = false;
            await Task.Run(() =>
            {
                while (PartyButtonClicked || ChallengeButtonClicked || OkEndButtonClicked || RewardsButtonClicked)
                {
                    int state = GetAsyncKeyState(e);
                    if (state != 0)
                    {
                        p = GetMousePosition();
                        pressed = true;
                        break;
                    }
                }
            });
            if (pressed) { keyWasPressed(p); pressed = false; }
        }

        // Set button coords/color
        private void setButtonInformation(Pixel p)
        {
            if (PartyButtonClicked)
            {
                IGButton temp = new IGButton();
                temp.X = p.x;
                temp.Y = p.y;
                temp.color = p.color;
                config.PartyButtonLocation = temp;
                PartyButtonClicked = false;
                // Change button text
                PB_Select_btn.Content = "Select";
            }
            if (ChallengeButtonClicked)
            {
                IGButton temp = new IGButton();
                temp.X = p.x;
                temp.Y = p.y;
                temp.color = p.color;
                config.ChallengeButtonLocation = temp;
                ChallengeButtonClicked = false;
                // Change button text
                CB_Select_btn.Content = "Select";
            }
            if (OkEndButtonClicked)
            {
                IGButton temp = new IGButton();
                temp.X = p.x;
                temp.Y = p.y;
                temp.color = p.color;
                config.OkEndButtonLocation = temp;
                OkEndButtonClicked = false;
                // Change button text
                OB_Select_btn.Content = "Select";
            }
            if (RewardsButtonClicked)
            {
                IGButton temp = new IGButton();
                temp.X = p.x;
                temp.Y = p.y;
                temp.color = p.color;
                config.RewardsButtonLocation = temp;
                RewardsButtonClicked = false;
                // Change button text
                RB_Select_btn.Content = "Select";
            }
            updateBtnTxtInfo();
        }
        
        // Updates text boxes with button information
        private void updateBtnTxtInfo()
        {
            // Partybutton
            PB_Color_txt.Text = config.PartyButtonLocation.color.ToString();
            PB_X_txt.Text = config.PartyButtonLocation.X.ToString();
            PB_Y_txt.Text = config.PartyButtonLocation.Y.ToString();

            // Challengebutton
            CB_Color_txt.Text = config.ChallengeButtonLocation.color.ToString();
            CB_X_txt.Text = config.ChallengeButtonLocation.X.ToString();
            CB_Y_txt.Text = config.ChallengeButtonLocation.Y.ToString();

            // OkEndbutton
            OB_Color_txt.Text = config.OkEndButtonLocation.color.ToString();
            OB_X_txt.Text = config.OkEndButtonLocation.X.ToString();
            OB_Y_txt.Text = config.OkEndButtonLocation.Y.ToString();

            // Rewardbutton
            RB_Color_txt.Text = config.RewardsButtonLocation.color.ToString();
            RB_X_txt.Text = config.RewardsButtonLocation.X.ToString();
            RB_Y_txt.Text = config.RewardsButtonLocation.Y.ToString();

            // Card slot positions
            CS_CardsY_txt.Text = config.SlotCardLocations.Y.ToString();
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        CS_Card1_txt.Text = config.SlotCardLocations.X[i].ToString();
                        break;
                    case 1:
                        CS_Card2_txt.Text = config.SlotCardLocations.X[i].ToString();
                        break;
                    case 2:
                        CS_Card3_txt.Text = config.SlotCardLocations.X[i].ToString();
                        break;
                    case 3:
                        CS_Card4_txt.Text = config.SlotCardLocations.X[i].ToString();
                        break;
                }
            }
        }

        // Adds position for placing cards
        private async void addPostitionFunc()
        {
            Point p = new Point();
            bool pressed = false;
            await Task.Run(() =>
            {
                while (addPosition)
                {
                    int state = GetAsyncKeyState(e);
                    if (state != 0)
                    {
                        p = GetMousePosition();
                        pressed = true;
                        break;
                    }
                }
            });
            if (pressed)
            {
                AddPos_btn.Content = "Add position";
                pressed = false;
                Random rnd = new Random();
                string name = Microsoft.VisualBasic.Interaction.InputBox("Please enter a name for the position", "Position Name", $"{rnd.Next(9999)}");
                // Add to list
                CardPosList.Items.Add(name);
                // Create obj and add to config
                CardPlacePosition temp = new CardPlacePosition();
                temp.Name = name;
                temp.X = Convert.ToInt16(p.X);
                temp.Y = Convert.ToInt16(p.Y);
                config.PlacePositions.Add(temp);
            }
        }

        // Sets card slot positions
        private void SetCardSlotPos(Point p, int a)
        {
            if (p.X == 0 && p.Y == 0) { return; }
            switch (a)
            {
                case 0:
                    config.SlotCardLocations.X[0] = (int)p.X;
                    CS_Card1_txt.Text = p.X.ToString();
                    config.SlotCardLocations.Y = (int)p.Y;
                    CS_CardsY_txt.Text = p.Y.ToString();
                    break;
                case 1:
                    config.SlotCardLocations.X[1] = (int)p.X;
                    CS_Card2_txt.Text = p.X.ToString();
                    config.SlotCardLocations.Y = (int)p.Y;
                    CS_CardsY_txt.Text = p.Y.ToString();
                    break;
                case 2:
                    config.SlotCardLocations.X[2] = (int)p.X;
                    CS_Card3_txt.Text = p.X.ToString();
                    config.SlotCardLocations.Y = (int)p.Y;
                    CS_CardsY_txt.Text = p.Y.ToString();
                    break;
                case 3:
                    config.SlotCardLocations.X[3] = (int)p.X;
                    CS_Card4_txt.Text = p.X.ToString();
                    config.SlotCardLocations.Y = (int)p.Y;
                    CS_CardsY_txt.Text = p.Y.ToString();
                    break;
            }
            //Cards = false;
            // Change button text
            updateBtnTxtInfo();
        }

        // Async method that listens for key to set card slot locations
        private async void ActivateCardSlotListener()
        {
            List<Point> p = new List<Point>();
            bool pressed = false;
            for (int i = 0; i < 4; i++)
            {
                p.Add(new Point(0, 0));
            }
            await Task.Run(() =>
            {
                while (Cards)
                {
                    int state1 = GetAsyncKeyState(49);
                    if (state1 != 0)
                    {
                        p[0] = GetMousePosition();
                        pressed = true;
                    }
                    int state2 = GetAsyncKeyState(50);
                    if (state2 != 0)
                    {
                        p[1] = GetMousePosition();
                        pressed = true;
                    }
                    int state3 = GetAsyncKeyState(51);
                    if (state3 != 0)
                    {
                        p[2] = GetMousePosition();
                        pressed = true;
                    }
                    int state4 = GetAsyncKeyState(52);
                    if (state4 != 0)
                    {
                        p[3] = GetMousePosition();
                        pressed = true;
                    }
                }
            });
            if (pressed)
            {
                for (int i = 0; i < 4; i++)
                {
                    SetCardSlotPos(p[i], i);
                }
                pressed = false;
            }
        }

        // Bot logic
        private async void StartTheBot()
        {
            Random r = new Random();
            await Task.Run(() =>
            {
                while (BotRunning)
                {
                    // Stability
                    Thread.Sleep(50);
                    keyState = GetAsyncKeyState(69);
                    if (keyState != 0) // check if e key is pressed
                    {
                        BotRunning = false;
                        continue;
                    }
                    // Checks if party button is on screen
                    if (config.PartyButtonLocation.color == Win32.GetCursorColorValue(config.PartyButtonLocation))
                    {
                        // Click on party button
                        Win32.SendLeftClick(config.PartyButtonLocation);
                        System.Threading.Thread.Sleep(1000);
                        continue;
                    }
                    // Check if challenge button is visible
                    if (config.ChallengeButtonLocation.color == Win32.GetCursorColorValue(config.ChallengeButtonLocation))
                    {
                        // Click on challenge button
                        Win32.SendLeftClick(config.ChallengeButtonLocation);
                        System.Threading.Thread.Sleep(1000);
                        inGame = true;
                        continue;
                    }
                    // Check if game has finished
                    if (config.OkEndButtonLocation.color == Win32.GetCursorColorValue(config.OkEndButtonLocation))
                    {
                        // Click on Ok button
                        Win32.SendLeftClick(config.OkEndButtonLocation);
                        System.Threading.Thread.Sleep(1000);
                        inGame = false;
                        continue;
                    }
                    // Check if rewards button is visible
                    if (config.RewardsButtonLocation.color == Win32.GetCursorColorValue(config.RewardsButtonLocation))
                    {
                        Win32.SendLeftClick(config.RewardsButtonLocation);
                        System.Threading.Thread.Sleep(1000);
                        continue;
                    }
                    if (inGame)
                    {
                        Thread.Sleep(2000);
                        int ran = r.Next(0, 4);
                        Win32.SendLeftClickXY(config.SlotCardLocations.X[ran], config.SlotCardLocations.Y);
                        ran = r.Next(0, config.PlacePositions.Count);
                        int x = config.PlacePositions[ran].X;
                        int y = config.PlacePositions[ran].Y;
                        Win32.SendLeftClickXY(x, y);
                    }
                }
            });
            StartBot_bnt.Content = "Start Bot";
        }
        #endregion
    }
}
