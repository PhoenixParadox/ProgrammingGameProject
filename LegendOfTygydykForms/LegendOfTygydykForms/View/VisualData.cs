using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LegendOfTygydykForms.Model;

namespace LegendOfTygydykForms.View
{
    public static class VisualData
    {
        public static Dictionary<string, Animation> _catAnimations;
        public static Dictionary<string, Animation> _robotAnimations;
        public static Dictionary<string, Animation> _mouseAnimations;
        public static Bitmap _tileTxtr = Assets.FloorTile;
        public static Bitmap _wallTxtr = Assets.BrickWall;
        public static Bitmap _menuBackground = Assets.menuBackground;

        public static Sprite[] CatSprites;
        public static Dictionary<string, Animation> _couchTextures 
        { 
            get
            {
                var dict = new Dictionary<string, Animation>();
                dict["hasCat"] = new Animation(new[] { Assets.couchWithCat }, 0.3);
                dict["empty"] = new Animation(new[] { Assets.emptyCouch }, 0.3);
                return dict;
            }
        }

        public static void Load() 
        {
            var dict = new Dictionary<string, Animation>();
            dict["moveRight"] = new Animation(new[] { Assets.catFront, Assets._catMoveAnimation_f1, Assets._catMoveAnimation_f2, Assets._catMoveAnimation_f3 }, 0.2);
            dict["moveLeft"] = new Animation(new[] { Assets.catFront, Assets._catMoveAnimation_f3, Assets._catMoveAnimation_f2, Assets._catMoveAnimation_f1 }, 0.2);
            dict["moveUp"] = new Animation(new[] { Assets.catBack }, 0.2);
            dict["invincible"] = new Animation(new[] { Assets.BlinkingCat_f0, Assets.BlinkingCat_f1 }, 0.2);
            _catAnimations = dict;

            dict = new Dictionary<string, Animation>();
            dict["moveRight"] = new Animation(new[] { Assets.robotRight }, 0.3);
            dict["moveLeft"] = new Animation(new[] { Assets.robotLeft }, 0.3);
            dict["moveUp"] = new Animation(new[] { Assets.robotUp }, 0.3);
            dict["moveDown"] = new Animation(new[] { Assets.robotDown }, 0.3);
            dict["idle"] = new Animation(new[] { Assets.robotUp, Assets.robotRight, Assets.robotDown, Assets.robotLeft }, 0.2);
            _robotAnimations = dict;

            dict = new Dictionary<string, Animation>();
            dict["moveRight"] = new Animation(new[] { Assets.mouseRight }, 0.3);
            dict["moveLeft"] = new Animation(new[] { Assets.mouseLeft }, 0.3);
            dict["moveUp"] = new Animation(new[] { Assets.mouseUp }, 0.3);
            dict["moveDown"] = new Animation(new[] { Assets.mouseDown }, 0.3);
            _mouseAnimations = dict;


            CatSprites = new Sprite[6];
            // tygydyk
            dict = new Dictionary<string, Animation>();
            dict["moveRight"] = new Animation(new[] { Assets.catFront, Assets._catMoveAnimation_f1, Assets._catMoveAnimation_f2, Assets._catMoveAnimation_f3 }, 0.2);
            dict["moveLeft"] = new Animation(new[] { Assets.catFront, Assets._catMoveAnimation_f3, Assets._catMoveAnimation_f2, Assets._catMoveAnimation_f1 }, 0.2);
            dict["moveUp"] = new Animation(new[] { Assets.catBack }, 0.2);
            dict["invincible"] = new Animation(new[] { Assets.BlinkingCat_f0, Assets.BlinkingCat_f1 }, 0.2);
            CatSprites[0] = new Sprite(dict, Assets.catFront);
            // coal
            dict = new Dictionary<string, Animation>();
            dict["moveRight"] = new Animation(new[] { Assets.blackCat0, Assets.blackCat1, Assets.blackCat2, Assets.blackCat3 }, 0.2);
            dict["moveLeft"] = new Animation(new[] { Assets.blackCat0, Assets.blackCat3, Assets.blackCat2, Assets.blackCat1 }, 0.2);
            dict["moveUp"] = new Animation(new[] { Assets.blackCatBack }, 0.2);
            dict["invincible"] = new Animation(new[] { Assets.blackCat0, Assets.blinkingBlackCat }, 0.2);
            CatSprites[2] = new Sprite(dict, Assets.blackCat0);
            // peach
            dict = new Dictionary<string, Animation>();
            dict["moveRight"] = new Animation(new[] { Assets.peach0, Assets.peach1, Assets.peach2, Assets.peach3 }, 0.2);
            dict["moveLeft"] = new Animation(new[] { Assets.peach0, Assets.peach3, Assets.peach2, Assets.peach1 }, 0.2);
            dict["moveUp"] = new Animation(new[] { Assets.peachBack }, 0.2);
            dict["invincible"] = new Animation(new[] { Assets.peach0, Assets.blinkingPeach }, 0.2);
            CatSprites[1] = new Sprite(dict, Assets.peach0);
            // pizzaCat
            dict = new Dictionary<string, Animation>();
            dict["moveRight"] = new Animation(new[] { Assets.pizzaCat0, Assets.pizzaCat1, Assets.pizzaCat2, Assets.pizzaCat3 }, 0.2);
            dict["moveLeft"] = new Animation(new[] { Assets.pizzaCat0, Assets.pizzaCat3, Assets.pizzaCat2, Assets.pizzaCat1 }, 0.2);
            dict["moveUp"] = new Animation(new[] { Assets.pizzaCatBack}, 0.2);
            dict["invincible"] = new Animation(new[] { Assets.pizzaCat0, Assets.blinkingPizzaCat }, 0.2);
            CatSprites[4] = new Sprite(dict, Assets.pizzaCat0);
            // roboCat
            dict = new Dictionary<string, Animation>();
            dict["moveRight"] = new Animation(new[] { Assets.roboCat0, Assets.roboCat1, Assets.roboCat2, Assets.roboCat3 }, 0.2);
            dict["moveLeft"] = new Animation(new[] { Assets.roboCat0, Assets.roboCat3, Assets.roboCat2, Assets.roboCat1 }, 0.2);
            dict["moveUp"] = new Animation(new[] { Assets.roboCatBack }, 0.2);
            dict["invincible"] = new Animation(new[] { Assets.roboCat0, Assets.blinkingRoboCat }, 0.2);
            CatSprites[5] = new Sprite(dict, Assets.roboCat0);
            // alienCat
            dict = new Dictionary<string, Animation>();
            dict["moveRight"] = new Animation(new[] { Assets.greenCat0, Assets.greenCat2, Assets.greenCat2, Assets.greenCat3 }, 0.2);
            dict["moveLeft"] = new Animation(new[] { Assets.greenCat0, Assets.greenCat3, Assets.greenCat2, Assets.greenCat1 }, 0.2);
            dict["moveUp"] = new Animation(new[] { Assets.greenCatBack}, 0.2);
            dict["invincible"] = new Animation(new[] { Assets.greenCat0, Assets.blinkingGreenCat}, 0.2);
            CatSprites[3] = new Sprite(dict, Assets.greenCat0);
        }
    }
}
