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
        private Queue<NodeOtelloPiece> biggestFlipQueue;
        private Queue<NodeOtelloPiece>[] allNodeFlipQueues;

        private int gridLength = 8;
        private Texture2D otelloTileTex;

        private int upperNeighbor;
        private int leftNeighbor;
        private int rightNeighbor;
        private int lowerNeighbor;

        private bool started = false;

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
            biggestFlipQueue = new Queue<NodeOtelloPiece>();
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
        public bool PlaceTile(int posX, int posY, Color playerColor)
        {
            bool placeable = false;
            for (int y = 0; y < gridLength; y++)
            {
                for (int x = 0; x < gridLength; x++)
                {
                    if (graph[x,y].MyHitbox.Contains(posX, posY) && graph[x, y].EmptyTile) //where i have clicked is empty, is k to place
                    {
                        graph[x, y].EmptyTile = false;                      
                        placeable = true;
                        graph[x, y].nodeColor = playerColor;                       
                    }                  
                }
            }
            return placeable;
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
                    if (graph[x,y].EmptyTile == false) //if it's not empty, DRAW
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
        public void StartGame()
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
                graph[(int)startingPositions[i].X, (int)startingPositions[i].Y].EmptyTile = false;
            }
         
        }
        //Borked
        private void Flip(Color flippersColor)
        {
            for (int y = 0; y  < gridLength; y++)
            {
                for (int x = 0; x < gridLength; x++)
                {
                    NodeOtelloPiece tempNode; //node bing flipped
                    OrthagonalFlipCheck(x, y);

                    for (int i = 0; i < allNodeFlipQueues.Count(); i++) // amount of flippable rows
                    {
                        for (int j= 0; j < allNodeFlipQueues[i].Count; j++) //the tiles themselves that can be flipped 
                        {
                           tempNode = allNodeFlipQueues[i].Dequeue();
                           tempNode.nodeColor = flippersColor;
                        }
                    }
                }
            }
        }

        private void OrthagonalFlipCheck(int x, int y)
        {
            upperNeighbor = y - 1;
            leftNeighbor = x - 1;
            rightNeighbor = x + 1;
            lowerNeighbor = y + 1;

            
            if (upperNeighbor >= 0 && graph[x, y].nodeColor != graph[x, upperNeighbor].nodeColor) //if(not outside, not empty, and not same team)
            {
                if (!graph[x, upperNeighbor].EmptyTile)
                {
                    currentFlipQueue.Enqueue(graph[x, upperNeighbor]);
                    OrthagonalFlipCheck(x, upperNeighbor);

                    if (currentFlipQueue.Count > biggestFlipQueue.Count) //
                    {
                        biggestFlipQueue = currentFlipQueue;
                    }
                }
                allNodeFlipQueues[allNodeFlipQueues.Count() + 1] = biggestFlipQueue; //Adds the amout "n" of the neighbors that can be flipped into the array of all flippable options.
            }
        }

        private void NonOrthagonalFlipCheck()
        {

        }
    
    }
}
