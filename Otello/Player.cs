using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otello
{
    class Player
    {
        protected GraphPlayingfield playingField;
        protected Color myColor;
        protected int posX, posY;

        public virtual void Update() { }

        public virtual void AssignStarter(bool starting) { }

        public virtual Color GetColor() { return myColor; }

        public virtual Vector2 GetMove() { return new Vector2(posX, posY); }
    }
}
