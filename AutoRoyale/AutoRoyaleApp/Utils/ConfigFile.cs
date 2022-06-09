using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutoRoyaleApp.Utils
{
    public class ConfigFile
    {
        [JsonPropertyName("PartyButtonLocation")]
        public IGButton PartyButtonLocation { get; set; }
        [JsonPropertyName("ChallengeButtonLocation")]
        public IGButton ChallengeButtonLocation { get; set; }
        [JsonPropertyName("OkEndButtonLocation")]
        public IGButton OkEndButtonLocation { get; set; }
        [JsonPropertyName("RewardsButtonLocation")]
        public IGButton RewardsButtonLocation { get; set; }
        [JsonPropertyName("PlacePositions")]
        public List<CardPlacePosition> PlacePositions { get; set; }
        [JsonPropertyName("SlotCardLocations")]
        public SlotCardPositions SlotCardLocations { get; set; }
        [JsonPropertyName("RandomPlaceLocations")]
        public RandomPlaceLoc RandomPlaceLocations { get; set; }

        public class RandomPlaceLoc
        {
            [JsonPropertyName("PlaceRandom")]
            public bool PlaceRandom { get; set; }
            [JsonPropertyName("TopLeft")]
            public Point TopLeft { get; set; }
            [JsonPropertyName("BottomRight")]
            public Point BottomRight { get; set; }
        }
        public class SlotCardPositions
        {
            [JsonPropertyName("X")]
            public List<int> X { get; set; }
            [JsonPropertyName("Y")]
            public int Y { get; set; }
        }
        public class CardPlacePosition
        {
            [JsonPropertyName("Name")]
            public string Name { get; set; }
            [JsonPropertyName("X")]
            public int X { get; set; }
            [JsonPropertyName("Y")]
            public int Y { get; set; }
        }
        public class IGButton
        {
            [JsonPropertyName("color")]
            public int color { get; set; }
            [JsonPropertyName("X")]
            public int X { get; set; }
            [JsonPropertyName("Y")]
            public int Y { get; set; }
        }
    }
    
}