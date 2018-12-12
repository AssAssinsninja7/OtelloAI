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
        private MouseState mouseState, oldMouseState;
        private bool justClicked;

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
           
        }     

        /// <summary>
        /// returns the individual nodes color
        /// </summary>
        /// <returns></returns>
        public override Color GetColor()
        {
            return myColor;
        }

        /// <summary>
        /// Allows the GameManager to get the Position that has been clicked by the player
        /// </summary>
        /// <returns></returns>
        public override Vector2 GetMove()
        {
            return new Vector2(posX, posY);
        }

        /// <summary>
        /// Gets the input made by the user, sets the Position and then returns if it was clicked or nah so the 
        /// GameManager can use More efficently (Only call getmove if justclicked is true)
        /// </summary>
        public bool SetMouseInput()
        {
            mouseState = Mouse.GetState();
            posX = mouseState.X;
            posY = mouseState.Y;
            justClicked = false;

            if (mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                justClicked = true;
            }
            oldMouseState = mouseState;
            return justClicked;
        }
    }
}
