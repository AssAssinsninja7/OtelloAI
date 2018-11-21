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

        public virtual void Update() { }

        public virtual void AssignStarter(bool starting) { }

        public virtual Color GetColor() { return myColor; }
    }
}
