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
        //private System.IO.StreamReader PlayerStreamReader;

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
                }
            }
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
