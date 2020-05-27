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
    public partial class Form4 : Form
    {
        private Timer timer;
        private Font menuFont;
        private Point _headlinePosition;
        public Form4()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            menuFont = new Font("Microsoft Sans", 24f);
            timer = new Timer();
            timer.Interval = 15;
            timer.Enabled = true;
            timer.Tick += Timer_Tick;
            _headlinePosition = new Point(200, 32);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form4_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString("TUTORIAL:", menuFont, Brushes.White, _headlinePosition);
            g.DrawString("AVOID", menuFont, Brushes.White, new Point(pictureBox2.Location.X + 100, pictureBox2.Location.Y + 16));
            g.DrawString("COLLECT", menuFont, Brushes.White, new Point(pictureBox3.Location.X - 180, pictureBox3.Location.Y + 16));
            g.DrawString("WATCH YOUR LIVES", menuFont, Brushes.White, new Point(pictureBox5.Location.X + 100, pictureBox5.Location.Y + 16));
            g.DrawString("YOU CAN SPEND FISHES\nYOU'VE COLLECTED IN THE SHOP", menuFont, Brushes.White, new Point(_headlinePosition.X - 150, _headlinePosition.Y + 350));

            g.DrawString("CONTROLS:", menuFont, Brushes.White, new Point(_headlinePosition.X + 600, _headlinePosition.Y));
            g.DrawString("ARROWS - MOVEMENT", menuFont, Brushes.White, new Point(_headlinePosition.X + 540, _headlinePosition.Y + 64));
            g.DrawString("SPACE - JUMP ONTO/FROM COUCH", menuFont, Brushes.White, new Point(_headlinePosition.X + 540, _headlinePosition.Y + 128));
            g.DrawString("Q - RETURN TO MENU", menuFont, Brushes.White, new Point(_headlinePosition.X + 540, _headlinePosition.Y + 192));
            g.DrawString("P - PAUSE", menuFont, Brushes.White, new Point(_headlinePosition.X + 540, _headlinePosition.Y + 256));
            g.DrawString("R - RESTART", menuFont, Brushes.White, new Point(_headlinePosition.X + 540, _headlinePosition.Y + 320));
            g.DrawString("ESC - EXIT GAME", menuFont, Brushes.White, new Point(_headlinePosition.X + 540, _headlinePosition.Y + 384));
            g.DrawString("S - GO TO SHOP", menuFont, Brushes.White, new Point(_headlinePosition.X + 540, _headlinePosition.Y + 448));
        }
    }
}
