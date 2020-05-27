using LegendOfTygydykForms.Model;
using LegendOfTygydykForms.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LegendOfTygydykForms
{
    public partial class Form3 : Form
    {
        public Game game;
        private Timer timer;
        private Font menuFont;
        private Point _spritePosition;
        private Point _menuFishesPosition;
        public Form3(Game game)
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            Text = "SHOP";
            this.game = game;
            button1.Click += game._shopController.ButtonHandler;
            timer = new Timer();
            timer.Interval = 15;
            timer.Enabled = true;
            timer.Tick += Timer_Tick;
            menuFont = new Font("Microsoft Sans", 24f);
            _spritePosition = new Point(this.Width / 2, 128);
            _menuFishesPosition = new Point(256, 32);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            game._shopController.InvokeGameTick(timer.Interval);
            Refresh();
        }

        private void Form3_KeyDown(object sender, KeyEventArgs e)
        {
            game._shopController.keyDown = e.KeyCode;
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void Form3_KeyUp(object sender, KeyEventArgs e)
        {
            if (game._shopController.keyDown == e.KeyCode)
                game._shopController.keyDown = default(Keys);
        }

        private void Form3_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            button1.Text = (game._shopController.ItemDisplayed.IsAvailable) ? (game._gameData.CurrentItem == game._gameData.CurrentSprite) ? "EQUIPPED" : "EQUIP" : "PURCHASE";
            var spr = VisualData.CatSprites[game._shopController.ItemDisplayed.SpriteInd];
            spr.Position = _spritePosition;
            DrawSprite(g, spr);
            g.DrawString(game._shopController.ItemDisplayed.Name, menuFont, Brushes.White, new Point(_spritePosition.X - 100, _spritePosition.Y + 64));
            g.DrawString(game._shopController.ItemDisplayed.Description, menuFont, Brushes.White, new Point(_spritePosition.X - 128, _spritePosition.Y + 200));
            g.DrawImage(Assets.fishIcon, _menuFishesPosition);
            g.DrawString("YOU HAVE:", menuFont, Brushes.White, new Point(_menuFishesPosition.X - 200, _menuFishesPosition.Y));
            g.DrawString(game._gameData.Fishes.ToString(), menuFont, Brushes.White, new Point(_menuFishesPosition.X + 64, _menuFishesPosition.Y));

            if (!game._shopController.ItemDisplayed.IsAvailable) 
            {
                g.DrawImage(Assets.fishIcon, new Point(button1.Location.X + 16, button1.Location.Y + 48));
                g.DrawString(game._shopController.ItemDisplayed.Price.ToString(), menuFont, Brushes.White, new Point(button1.Location.X + 80, button1.Location.Y + 48));
            }
        }
        private void DrawSprite(Graphics g, Sprite s)
        {
            if (s != null)
                g.DrawImage(s.currentFrame, s.Frame);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            game._shopController.NextItem();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            game._shopController.PrevItem();
        }
    }
}
