using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LegendOfTygydykForms.Control
{
    public class ShopItem 
    {
        public string Name;
        public string Description;
        public int Price;
        public bool IsAvailable;
        public int SpriteInd;
    }

    public class ShopController
    {
        //public List<ShopItem> Items;
        public Game Game;
        public int NumberOfItems { get { return Game._gameData.Items.Length; } }
        private int currentitem;
        public int CurrentItem 
        {
            get { return currentitem; }
            set 
            {
                currentitem = value;
                Game._gameData.CurrentItem = value;
            }
        }
        public Keys keyDown;

        public ShopItem ItemDisplayed { get { return Game._gameData.Items[CurrentItem]; } }

        public ShopController(Game game) 
        {
            Game = game;
        }

        public void NextItem() 
        {
            CurrentItem = (CurrentItem + 1 == NumberOfItems) ? 0 : CurrentItem + 1;
        }
        public void PrevItem() 
        {
            CurrentItem = (CurrentItem == 0) ? NumberOfItems - 1 : CurrentItem - 1;
        }

        public void InvokeGameTick(int dt) 
        {
            switch (keyDown) 
            {
                case (Keys.Left):
                    PrevItem();
                    break;
                case (Keys.Right):
                    NextItem();
                    break;
            }
            
        }

        public void ButtonHandler(object sender, EventArgs e) 
        {
            if (ItemDisplayed.IsAvailable)
            {
                Game._gameData.CurrentItem = ItemDisplayed.SpriteInd;
                Game._gameData.CurrentSprite = ItemDisplayed.SpriteInd;
            }
            else 
            {
                if (Game._gameData.Fishes >= ItemDisplayed.Price) 
                {
                    Game._gameData.Fishes -= ItemDisplayed.Price;
                    ItemDisplayed.IsAvailable = true;
                }
            }
        }
    }
}
