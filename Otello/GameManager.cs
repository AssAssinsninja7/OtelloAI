﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otello
{
    class GameManager
    {
        GraphPlayingfield playingfield;
        HumanPlayer humanPlayer;
        Agent agentPlayer;
        int totalAmounOfTurn = 0;
        bool humanTurn;

        public GameManager(GraphPlayingfield playingfield, HumanPlayer humanPlayer, Agent agentPlayer)
        {
            this.playingfield = playingfield;
            this.humanPlayer = humanPlayer;
            this.agentPlayer = agentPlayer;
            humanTurn = startingPlayer();
        }

        public void Update()
        {
            if(humanTurn)
            {
                if (humanPlayer.SetMouseInput()) //checks if the button has been pressed if so place tile
                {
                    Vector2 mousePos;
                    mousePos = humanPlayer.GetMove();
                    playingfield.PlaceTile((int)mousePos.X,(int) mousePos.Y, humanPlayer.GetColor());  //maybe returns a int if it works that acts as the score
                    humanTurn = !humanTurn;
                }
            }
            else
            {
                agentPlayer.SetPlaceables();
                humanTurn = !humanTurn;
                //agents turn here
            }

        }

        public void StartGame()
        {
            
        }

        public void Draw(SpriteBatch sb)
        {
            playingfield.Draw(sb);
        }

        /// <summary>
        /// Checks what color the player picked then returns the starting player
        /// </summary>
        /// <returns></returns>
        private bool startingPlayer()
        {
            bool playerStart;
            if (humanPlayer.GetColor() == Color.White)
            {
                playerStart = true;
            }
            else
            {
                playerStart = false;
            }
            return playerStart;
        }
        //Keep track of turns
        //Keep track of scores
        //Keep track of which scene
        //Communicate moves from player to actual move in Playingfield
    }
}
