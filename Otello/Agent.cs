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
        //private NodeOtelloPiece[] placeableNodes;


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

        //private void SetHighestFlipValue(NodeOtelloPiece[] placeableNodes) //skica
        //{
        //    for (int i = 0; i < placeableNodes.Length; i++) //går igenom alla pacerbara platser
        //    {

        //        for (int j = 0; j < placeableNodes[i].Neighbors.Count; j++) //går igenom den placerbara nodens grannar kollar viability 
        //        {
        //            if (!placeableNodes[i].Neighbors.ToArray()[i].EmptyTile) //om grannarna till den placerbara noden inte är tomma (för de är fortfarande grannar även om de inte håller en tile)
        //            {
        //                if (placeableNodes[i].Neighbors.ToArray()[j].NodeColor != myColor) //om grannen inte har samma färg
        //                {
        //                    //behöver kolla den direction som hittades och adda poängen 
        //                    Vector2 dir = new Vector2(placeableNodes[i].Position.X - placeableNodes[i].Neighbors.ToArray()[j].Position.X, )
        //                    placeableNodes[i].FlipScore++;
        //                }
        //            }
        //            else if (placeableNodes[i].Neighbors.ToArray()[j].NodeColor == myColor && placeableNodes[i].FlipScore > 0) // kanske inte behöver kolla flipscoren här då 
        //            {
        //                //if a node of my color is found and if the score is higher than 0 then this is a valid move the val will be positiv but if the color of the node is 
        //                //opposite then the flipvalue will be "negative" in the min max. This is because the smaller values will be "prioretised" by the agent as the players 
        //                //best moves
        //            }
        //        }
        //    }
           
        //}

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
