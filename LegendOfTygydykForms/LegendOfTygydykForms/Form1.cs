using LegendOfTygydykForms.Control;
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
        private Bitmap _tileTxtr = Assets.FloorTile;
        private Bitmap _wallTxtr = Assets.BrickWall;

        private Game game;
        private Timer timer;

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            game = new Game(this);
            Game.PointsIncreased += PlayPointsSound;
            Game.GotHit += PlayHitSound;
            Game.MouseSquished += PlaySquishSound;
            //bgMusicPlayer.URL = "Resources/music_zapslat_tuff_enough.mp3";
            //bgMusicPlayer.controls.stop();
            //bgMusicPlayer.controls.play();
            timer = new Timer();
            timer.Interval = 15;
            timer.Enabled = true;
            timer.Tick += timer_Tick;
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
            var str = " ";
            //foreach (var k in game.CurrentWorld.jumpingPoints.Keys)
            //    str += k;
            foreach (var p in game.CurrentWorld.trail)
                str += p.ToString();
            Text = game.CurrentWorld.AbsPositionToRelaive(game.CurrentWorld.cat.Position).ToString() + str;
            Graphics g = e.Graphics;
            DrawTiles(g);
            foreach (var s in game.toDraw) 
            {
                DrawSprite(g, s);
            }
            game.gameHUD.DrawHUD(g);
        }

        private void DrawTiles(Graphics g) 
        {
            var borderLength = game.CurrentWorld.mapSize;
            var tileWidth = game.CurrentWorld.tileWidth;

            DrawBorder(g, borderLength, tileWidth, true, 0);
            DrawBorder(g, borderLength, tileWidth, true, borderLength);
            DrawBorder(g, borderLength, tileWidth, false, 0);
            DrawBorder(g, borderLength, tileWidth, false, borderLength);

            for (int i = 1; i < game.CurrentWorld.mapSize; i++)
            {
                for (int j = 1; j < game.CurrentWorld.mapSize; j++)
                {
                    g.DrawImage(_tileTxtr, new Rectangle(i * 64, j * 64, 64, 64));
                }
            }
        }

        private void DrawBorder(Graphics g, int borderLength, int tileWidth, bool isHorizontal, int startInd) 
        {
            
            for (int i = 0; i <= borderLength; i++) 
            {
                var rect = (isHorizontal) ? (new Rectangle(i * tileWidth, startInd * tileWidth, tileWidth, tileWidth))
                                          : (new Rectangle(startInd * tileWidth, i * tileWidth, tileWidth, tileWidth));
                g.DrawImage(_wallTxtr, rect);
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
    }
}
