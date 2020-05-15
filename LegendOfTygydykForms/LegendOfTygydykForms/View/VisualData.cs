using System;
using System.Collections.Generic;
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
        }
    }
}
