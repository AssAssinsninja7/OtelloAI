using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otello
{
    class Agent : Player
    {

        public Agent(GraphPlayingfield playingField)
        {
            this.playingField = playingField;
        }

        public override void Update()
        {
            base.Update();
        }

        /// <summary>
        /// Assigns the color based on the players choice.
        /// </summary>
        /// <param name="starting"></param>
        public override void AssignStarter(bool starting) 
        {
            if (starting)
            {
                myColor = Color.White;
            }
            else
            {
                myColor = Color.Black;
            }
        }

        public override Color GetColor()
        {
            return myColor;
        }
    }
}
