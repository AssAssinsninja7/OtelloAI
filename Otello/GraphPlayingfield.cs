using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otello
{
    class GraphPlayingfield
    {
        private NodeOtelloPiece[,] graph;
        private List<Vector2> startingPositions;
        private Queue<NodeOtelloPiece> currentFlipQueue;
        //private Queue<NodeOtelloPiece> biggestFlipQueue;
        private Queue<NodeOtelloPiece>[] allNodeFlipQueues;

        int amountOfPoints = 0;

        private int gridLength = 8;
        private Texture2D otelloTileTex;

        private int upperNeighbor;
        private int leftNeighbor;
        private int rightNeighbor;
        private int lowerNeighbor;

        private bool rightChecked, downChecked;
        NodeOtelloPiece startNode;

        public GraphPlayingfield(Texture2D otelloTileTex)
        {
            this.otelloTileTex = otelloTileTex;

            graph = new NodeOtelloPiece[gridLength, gridLength];           

            CreateGraph();
            SetNeighbors();

            startingPositions = new List<Vector2>();
            startingPositions.Add(new Vector2(3, 3));
            startingPositions.Add(new Vector2(3, 4));
            startingPositions.Add(new Vector2(4, 3));
            startingPositions.Add(new Vector2(4, 4));

            currentFlipQueue = new Queue<NodeOtelloPiece>();
            //biggestFlipQueue = new Queue<NodeOtelloPiece>();
            allNodeFlipQueues = new Queue<NodeOtelloPiece>[gridLength];

         
        }

        /// <summary>
        /// Creates a two-dimensional graph.
        /// </summary>
        void CreateGraph()
        {
            for (int y = 0; y < gridLength; y++)
            {
                for (int x = 0; x < gridLength; x++)
                {
                    graph[x, y] = new NodeOtelloPiece(new Vector2(x,y), otelloTileTex);
                    graph[x, y].MyHitbox = new Rectangle(x * otelloTileTex.Width , y * otelloTileTex.Height, otelloTileTex.Width, otelloTileTex.Height);
                }
            }
        }

        /// <summary>
        /// Gets called by the players (agent/ human) and then places the tile with the correct color 
        /// where the human pressed or where the agent chose. 
        /// </summary>
        /// <param name="mouseX">Coordinate for the mouse x position</param>
        /// <param name="posY">Coordinate for the mouse y position</param>
        /// <param name="playerColor">Which player placed it</param>
        /// <returns></returns>
        public void PlaceTile(int posX, int posY, Color playerColor)
        {
            for (int y = 0; y < gridLength; y++)
            {
                for (int x = 0; x < gridLength; x++)
                {
                    if (graph[x,y].MyHitbox.Contains(posX, posY) && graph[x, y].isEmptyTile) //where i have clicked is empty, is k to place
                    {
                        graph[x, y].isEmptyTile = false;                      
                        graph[x, y].nodeColor = playerColor;
                        startNode = graph[x, y];
                        Flip(x, y); //I dont need the color since the one I'm checking will have the right one
                    }                  
                }
            }

        }     



        /// <summary>
        /// Draws the otello tiles that have been placed on the board
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) 
        {
            for (int y = 0; y < gridLength; y++)
            {
                for (int x = 0; x < gridLength; x++)
                {
                    if (graph[x,y].isEmptyTile == false) //if it's not empty, DRAW
                    {
                        spriteBatch.Draw(graph[x,y].OtelloTileTex, graph[x, y].MyHitbox, graph[x,y].nodeColor); //smått reee att jag skickar texutren via klassen till noden så att jag sen kan hämta den igen c:
                    }
                }
            }
        }

        /// <summary>
        /// Sets the neighbors by checking all the possible nodes around it (3x3 grid minus the center one which is the current node)
        /// </summary>
        void SetNeighbors()
        {
            for (int y = 0; y < gridLength; y++)
            {
                for (int x = 0; x < gridLength; x++)
                {
                    upperNeighbor = y - 1;
                    leftNeighbor = x - 1;
                    rightNeighbor = x + 1;
                    lowerNeighbor = y + 1;

                    if (upperNeighbor >= 0)
                    {
                        graph[x, y].AddNehigbors(graph[x, upperNeighbor]); //upper neighbor
                    }
                    if (leftNeighbor >= 0) // left neighbor
                    {
                        graph[x, y].AddNehigbors(graph[leftNeighbor, y]);
                    }
                    if (rightNeighbor < gridLength) // right neighbor
                    {
                        graph[x, y].AddNehigbors(graph[rightNeighbor, y]);
                    }
                    if (lowerNeighbor < gridLength) // lower neighbor
                    {
                        graph[x, y].AddNehigbors(graph[x, lowerNeighbor]);
                    }

                    if ((x-1) >= 0 && (y - 1) >= 0) //upperLeftNeighbor 
                    {
                        graph[x, y].AddNehigbors(graph[(x - 1), (y - 1)]);
                    }

                    if ((x + 1) < 8 && (y - 1) >= 0) //upperRightNeighbor
                    {
                        graph[x, y].AddNehigbors(graph[(x + 1), (y - 1)]);
                    }

                    if ((x - 1) >= 0 && (y + 1) < 8) //lowerLeftNeighbor
                    {
                        graph[x, y].AddNehigbors(graph[(x - 1), (y + 1)]);
                    }

                    if ((x + 1) < 8 && (y + 1) < 8) //lowerLeftNeighbor
                    {
                        graph[x, y].AddNehigbors(graph[(x + 1), (y + 1)]);
                    }
                }
            }
        }

        ///// <summary>
        ///// Preps upp the starting tiles for the game 
        ///// </summary>
        public void Start()
        {
            PlaceStartPositios();
        }

        private void PlaceStartPositios()
        {
            for (int i = 0; i < startingPositions.Capacity; i++)
            {
                if (i == 1 || i == 2)
                {
                 
                    graph[(int)startingPositions[i].X, (int)startingPositions[i].Y].nodeColor = Color.Black;
                }
                else
                {
                    graph[(int)startingPositions[i].X, (int)startingPositions[i].Y].nodeColor = Color.White;
                }
                graph[(int)startingPositions[i].X, (int)startingPositions[i].Y].isEmptyTile = false;
            }
         
        }

        //Borked
        private void Flip(int x, int y)
        {
            upperNeighbor = y - 1;
            leftNeighbor = x - 1;
            rightNeighbor = x + 1;
            lowerNeighbor = y + 1;

            

            if (rightNeighbor >= 0 && startNode.nodeColor != graph[x, rightNeighbor].nodeColor) //if(not outside, and not same team)
            {
                //X_AxelFlip(x, y, true);

                RightFlip(x, y);


                //allNodeFlipQueues[allNodeFlipQueues.Count() + 1] = currentFlipQueue; 
            }
          
        }

        private void OrthagonalFlipCheck(int x, int y)
        {
           
        }

        /// <summary>
        /// Checks if there's any tiles that can be flipped based on the placement of the 
        /// players tile.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isRight"></param>
        private void X_AxelFlip(int x, int y, bool isRight)
        {
            if (isRight && !rightChecked) //have i chekced the whole right side or nah?
            {
                if (!graph[rightNeighbor, y].isEmptyTile) // if this pos is not empty
                {
                    currentFlipQueue.Enqueue(graph[rightNeighbor, y]);
                    X_AxelFlip(rightNeighbor, y, true);
                }
                else if(graph[rightNeighbor, y].isEmptyTile && currentFlipQueue.Count > 0)
                {
                    amountOfPoints += currentFlipQueue.Count;
                    rightChecked = true;
                }
            }
            else
            {
                if (!graph[leftNeighbor, y].isEmptyTile)
                {
                    currentFlipQueue.Enqueue(graph[leftNeighbor, y]);
                    X_AxelFlip(leftNeighbor, y, false);
                }
                else if (graph[leftNeighbor, y].isEmptyTile && currentFlipQueue.Count > 0)
                {
                    amountOfPoints += currentFlipQueue.Count;
                }
                rightChecked = false;
            }
        }

        private void Y_AxelFlip(int x, int y, bool isDown)
        {
            if (isDown)
            {
                if (!graph[x, lowerNeighbor].isEmptyTile) //Down
                {
                    currentFlipQueue.Enqueue(graph[x, upperNeighbor]);
                    Y_AxelFlip(x, lowerNeighbor, true);
                }
                else if (graph[x, lowerNeighbor].isEmptyTile && currentFlipQueue.Count > 0)
                {
                    amountOfPoints += currentFlipQueue.Count;
                }
            }
            else
            {
                if (!graph[x, upperNeighbor].isEmptyTile) //UP
                {
                    currentFlipQueue.Enqueue(graph[x, upperNeighbor]);
                    Y_AxelFlip(x, upperNeighbor, false);
                }
                else if (graph[x, upperNeighbor].isEmptyTile && currentFlipQueue.Count > 0)
                {
                    amountOfPoints += currentFlipQueue.Count;
                }
            }
        }

        private void RightFlip(int x, int y)
        {
        
                if (graph[rightNeighbor, y].isEmptyTile) // if this pos is not empty
                {
                    currentFlipQueue.Enqueue(graph[rightNeighbor, y]);

                if (rightNeighbor + 1 < gridLength)       //funkar inte måste se till att den stoppar vid else if när den väl inte hittar fler         
                    rightNeighbor += 1;
              
                    RightFlip(rightNeighbor, y);
                }

                else if (graph[rightNeighbor, y].nodeColor == startNode.nodeColor && currentFlipQueue.Count > 0 )
                {
                    amountOfPoints += currentFlipQueue.Count;
                }           
        }


        /// <summary>
        /// Returns the points 
        /// </summary>
        /// <returns></returns>
        public int GetPoints()
        {
            int finalPoints = amountOfPoints;
            amountOfPoints = 0;
            return finalPoints;
        }

        private void NonOrthagonalFlipCheck()
        {

        }
    
    }
}
