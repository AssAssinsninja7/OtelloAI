using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otello
{
    class HumanPlayer : Player
    {
        MouseState mouseState, oldMouseState;
        int mouseX, posY;
        bool tempOutBool;

        public HumanPlayer(GraphPlayingfield playingField)
        {
            this.playingField = playingField;
        }

        /// <summary>
        /// Sets the color for the player
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

        public override void Update()
        {
            mouseState = Mouse.GetState();
            mouseX = mouseState.X;
            posY = mouseState.Y;

            if (mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                tempOutBool = playingField.PlaceTile(mouseX, posY, myColor);
                System.Console.WriteLine(tempOutBool); //debug, remove later maybe switch so the method doesn't return a bool
            }
            oldMouseState = mouseState;
        }

        /// <summary>
        /// returns the individual nodes color
        /// </summary>
        /// <returns></returns>
        public override Color GetColor()
        {
            return myColor;
        }
    }
}
