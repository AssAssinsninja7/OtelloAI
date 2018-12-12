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
        private int maxEval, minEval;
        private NodeOtelloPiece evalNode, maxEvalNode;
        private NodeOtelloPiece[] placeableNodes;


        public Agent(GraphPlayingfield playingField)
        {
            this.playingField = playingField;
            maxEval = 1000;
            minEval = -1000;
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

       
        public void SetPlaceables()
        {
           placeableNodes = playingField.GetEmptyTiles();

            
        }



        private NodeOtelloPiece MiniMax(NodeOtelloPiece node, int depth, bool isAgent)
        {

            if (depth == 0)
                return node;

            if (isAgent)
            {
                foreach (var neighbor in node.Neighbors)
                {
                    if(neighbor != null)
                    {
                        evalNode = MiniMax(neighbor, depth - 1, false);
                        // maxEvalNode = best flipps between this maxEvalNode compared to evalNode (måste ända ta ut bästa flipriktningen för denna noden)
                    }
                }
            }

            return null;
        }
    }
}
