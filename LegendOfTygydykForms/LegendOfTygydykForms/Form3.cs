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
            button1.Text = (game._shopController.ItemDisplayed.IsAvailable) ? "EQUIP" : "PURCHASE";
            var spr = VisualData.CatSprites[game._gameData.CurrentItem];
            spr.Position = _spritePosition;
            DrawSprite(g, spr);
            g.DrawString(game._shopController.ItemDisplayed.Name, menuFont, Brushes.White, new Point(_spritePosition.X - 100, _spritePosition.Y + 64));
            g.DrawString(game._shopController.ItemDisplayed.Description, menuFont, Brushes.White, new Point(_spritePosition.X - 100, _spritePosition.Y + 200));
            //g.DrawString("<", menuFont, Brushes.White, new Point(_spritePosition.X - 100, _spritePosition.Y));
            //g.DrawString(">", menuFont, Brushes.White, new Point(_spritePosition.X + 80, _spritePosition.Y));
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
