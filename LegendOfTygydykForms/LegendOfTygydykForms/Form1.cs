﻿using LegendOfTygydykForms.Control;
using LegendOfTygydykForms.Model;
using LegendOfTygydykForms.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace LegendOfTygydykForms
{
    public partial class Form1 : Form
    {
        private Game game;
        private Timer timer;
        private Button shopButton;
        private Button purchaseButton;
        private Font menuFont;

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            Text = "Legend Of Tygydyk";
            game = new Game(this);
            Game.PointsIncreased += PlayPointsSound;
            Game.GotHit += PlayHitSound;
            Game.MouseSquished += PlaySquishSound;
            Game.GameStateChanged += AdjustWindowSize;
            timer = new Timer();
            timer.Interval = 15;
            timer.Enabled = true;
            timer.Tick += timer_Tick;
            this.Size = game.DesiredWindowSize;
            pictureBox1.Location = new Point(this.Width - 128, 16);
            //button1.Click += game.VisitShop;
            shopButton = new Button();
            shopButton.Text = "VISIT SHOP";
            shopButton.BackgroundImage = Assets.menuBackground;
            menuFont = new Font("Microsoft Sans", 24f);

        }

        private void AdjustWindowSize()
        {
            this.Size = game.DesiredWindowSize;
        }

        private void PlaySquishSound()
        {
            new SoundPlayer(Assets.zapsplat_foley_wet_towel_squish_squelch_002_13836).Play();
        }

        private void PlayHitSound()
        {
            new SoundPlayer(Assets.jessey_drake_synth_space_weird_synthy_sci_fi_sting_accent_snth_jd).Play();
        }

        private void PlayPointsSound()
        {
            new SoundPlayer(Assets.esm_5_wickets_sound_fx_arcade_casino_kids_mobile_app).Play();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            switch (game.State) 
            {
                case (GameState.Pause):
                case (GameState.Playing):
                    pictureBox1.Visible = false;
                    DrawTiles(g);
                    foreach (var s in game.toDraw.OrderBy(td => td.Layer))
                    {
                        DrawSprite(g, s);
                    }
                    DrawWalls(g);
                    break;
                case (GameState.Menu):
                    pictureBox1.Visible = true;
                    g.DrawImage(VisualData._menuBackground, new Point(0, 0));
                    g.DrawString("Learn How To Play ->", menuFont, Brushes.White, new Point(pictureBox1.Location.X - 350, pictureBox1.Location.Y + 20));
                    break;
            }
            game.gameHUD.DrawHUD(g);
        }

        private void DrawWalls(Graphics g) 
        {
            var tileWidth = game._currentWorldTileSize;
            var worldSize = game._worldSize;

            DrawBorder(g, worldSize, tileWidth, true, 0);
            DrawBorder(g, worldSize, tileWidth, true, worldSize.Height);
            DrawBorder(g, worldSize, tileWidth, false, 0);
            DrawBorder(g, worldSize, tileWidth, false, worldSize.Width);
        }

        private void DrawTiles(Graphics g) 
        {
            var tileWidth = game._currentWorldTileSize;
            var worldSize = game._worldSize;

            //DrawBorder(g, worldSize, tileWidth, true, 0);
            //DrawBorder(g, worldSize, tileWidth, true, worldSize.Height);
            //DrawBorder(g, worldSize, tileWidth, false, 0);
            //DrawBorder(g, worldSize, tileWidth, false, worldSize.Width);

            for (int i = 1; i < worldSize.Width; i++)
            {
                for (int j = 1; j < worldSize.Height; j++)
                {
                    g.DrawImage(VisualData._tileTxtr, new Rectangle(i * 64, j * 64, 64, 64));
                }
            }
        }

        /// <summary>
        /// Draws a set of vertical or horizontal tiles which represent world border.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="worldSize"> Borders of given world. </param>
        /// <param name="tileWidth"></param>
        /// <param name="isHorizontal"> Type of a border. </param>
        /// <param name="startInd"></param>
        private void DrawBorder(Graphics g, Size worldSize, int tileWidth, bool isHorizontal, int startInd) 
        {
            if (isHorizontal)
            {
                for (int i = 0; i <= worldSize.Width; i++)
                {
                    var rect = (new Rectangle(i * tileWidth, startInd * tileWidth, tileWidth, tileWidth));
                    g.DrawImage(VisualData._wallTxtr, rect);
                }
            }
            else
            {
                for (int i = 0; i <= worldSize.Height; i++)
                {
                    var rect = new Rectangle(startInd * tileWidth, i * tileWidth, tileWidth, tileWidth);
                    g.DrawImage(VisualData._wallTxtr, rect);
                }
            }

        }

        private void DrawSprite(Graphics g, Sprite s) 
        {
            if(s != null)
                g.DrawImage(s.currentFrame, s.Frame);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            game.Update(timer.Interval);
            Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            game.keyDown = e.KeyCode;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (game.keyDown == e.KeyCode)
                game.keyDown = default(Keys);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            (new Form4()).ShowDialog();
        }
    }
}
