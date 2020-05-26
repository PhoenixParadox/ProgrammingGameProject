using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendOfTygydykForms.Control
{
    /// <summary>
    /// Class representing game data which is shared between different game sessions.
    /// </summary>
    public class GameData 
    {
        public LedearboardEntry[][] TopPlayers;
        public int LedearboardIndex;
        public int LedearboardLength = 5;
        public ShopItem[] Items;
        public int NumberOfItems;
        public int CurrentItem;
        public int CurrentSprite;
        public int Fishes;
    }
    public struct LedearboardEntry 
    {
        public int Score;
        public string Name;
    }

    /// <summary>
    /// Class for saving and loading data.
    /// </summary>
    public class DataManager
    {
        private System.Xml.Serialization.XmlSerializer PlayerWriter;
        private System.Xml.Serialization.XmlSerializer PlayerReader;
        private string PlayerSavePath;
        private System.IO.FileStream PlayerSaveFile;

        private Game game;

        public DataManager(Game game) 
        {
            this.game = game;

            PlayerWriter = new System.Xml.Serialization.XmlSerializer(typeof(GameData));
            PlayerReader = new System.Xml.Serialization.XmlSerializer(typeof(GameData));

            PlayerSavePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//GameData.xml";
            if (!System.IO.File.Exists(PlayerSavePath))
            {
                PlayerSaveFile = System.IO.File.Create(PlayerSavePath);
                LoadShopItems();
            }
            else
            {
                try
                {
                    PlayerSaveFile = new System.IO.FileStream(PlayerSavePath, System.IO.FileMode.Open);
                    LoadFromSave();
                }
                catch 
                {
                    PlayerSaveFile.Close();
                    System.IO.File.Delete(PlayerSavePath);
                    PlayerSaveFile = System.IO.File.Create(PlayerSavePath);
                    LoadShopItems();
                }
            }
        }

        private void LoadShopItems() 
        {
            game._gameData.Items = new ShopItem[]
            {
                new ShopItem {Name = "TYGYDYK", Description = "A Legendary hero.", Price = 0, IsAvailable = true, SpriteInd = 0 },
                new ShopItem {Name = "PEACH", Description = "Orange is the new cat.", Price = 50, IsAvailable = false, SpriteInd = 1 },
                new ShopItem {Name = "COAL", Description = "Back in black.", Price = 75, IsAvailable = false, SpriteInd = 2 },
                new ShopItem {Name = "ALIEN", Description = "In space no one\n can hear you meow.", Price = 150, IsAvailable = false, SpriteInd = 3 },
                new ShopItem {Name = "PIZZA-CAT", Description = "Now with a crispy crust.", Price = 300, IsAvailable = false, SpriteInd = 4 },
                new ShopItem {Name = "TER-MEOW-NATOR", Description = "He'll be back.", Price = 500, IsAvailable = false, SpriteInd = 5 },
            };
            game._gameData.NumberOfItems = game._gameData.Items.Length;
        }

        private void LoadFromSave()
        {
            game._gameData = (GameData)PlayerReader.Deserialize(PlayerSaveFile);
        }

        public void Save() 
        {
            PlayerSaveFile.Close();
            System.IO.File.Delete(PlayerSavePath);
            PlayerSaveFile = System.IO.File.Create(PlayerSavePath);
            PlayerWriter.Serialize(PlayerSaveFile, game._gameData);
        }
    }
}
