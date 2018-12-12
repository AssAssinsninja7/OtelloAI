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
        //private Queue<NodeOtelloPiece>[] allNodeFlipQueues;

        int amountOfPoints = 0;

        private int gridLength = 8;
        private Texture2D otelloTileTex;

        private int upperNeighbor;
        private int leftNeighbor;
        private int rightNeighbor;
        private int lowerNeighbor;

        private bool rightChecked, downChecked;
        NodeOtelloPiece startNode;

        List<NodeOtelloPiece> validMoves;
        int countup = 0;

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

            validMoves = new List<NodeOtelloPiece>();
            //biggestFlipQueue = new Queue<NodeOtelloPiece>();
            //allNodeFlipQueues = new Queue<NodeOtelloPiece>[gridLength];    
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
        /// <param name="mouseX">Coordinate for the mouse x Position</param>
        /// <param name="posY">Coordinate for the mouse y Position</param>
        /// <param name="playerColor">Which player placed it</param>
        /// <returns></returns>
        public void PlaceTile(int posX, int posY, Color playerColor)
        {
            for (int y = 0; y < gridLength; y++)
            {
                for (int x = 0; x < gridLength; x++)
                {
                    if (graph[x,y].MyHitbox.Contains(posX, posY) && graph[x, y].EmptyTile) //where i have clicked is empty, is k to place
                    {
                        graph[x, y].EmptyTile = false;                      
                        graph[x, y].NodeColor = playerColor;
                        startNode = graph[x, y]; // is the node that 
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
                    if (graph[x,y].EmptyTile == false) //if it's not empty, DRAW
                    {
                        spriteBatch.Draw(graph[x,y].OtelloTileTex, graph[x, y].MyHitbox, graph[x,y].NodeColor); //smått reee att jag skickar texutren via klassen till noden så att jag sen kan hämta den igen c:
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

        /// <summary>
        /// Places the staring tiles for each team
        /// </summary>
        public void PlaceStartPositios()
        {
            for (int i = 0; i < startingPositions.Capacity; i++)
            {
                if (i == 1 || i == 2)
                {
                 
                    graph[(int)startingPositions[i].X, (int)startingPositions[i].Y].NodeColor = Color.Black;
                }
                else
                {
                    graph[(int)startingPositions[i].X, (int)startingPositions[i].Y].NodeColor = Color.White;
                }
                graph[(int)startingPositions[i].X, (int)startingPositions[i].Y].EmptyTile = false;
            }
         
        }

        //Works for x & y axis but not the tilted ones (bottom left corner to upper right corner)
        private void Flip(int x, int y)
        {
            upperNeighbor = y - 1;
            leftNeighbor = x - 1;
            rightNeighbor = x + 1;
            lowerNeighbor = y + 1;

            
            X_AxelFlip(rightNeighbor, y, true);
            X_AxelFlip(leftNeighbor, y, false);
            Y_AxelFlip(x, lowerNeighbor, true);
            Y_AxelFlip(x, upperNeighbor, false);

            FlipColor();                  
        }

       /// <summary>
       /// Goes through the grid and flips all the valid tiles in the queue 
       /// currentFlipQueue and then removes it. 
       /// </summary>
       private void FlipColor()
       {
            for (int y = 0; y < gridLength; y++)
            {
                for (int x = 0; x < gridLength; x++)
                {
                    if (currentFlipQueue.Count > 0 && currentFlipQueue.Peek() == graph[x, y])
                    {
                        currentFlipQueue.Dequeue();
                        graph[x, y].NodeColor = startNode.NodeColor;
                    }
                }
            }

        }

       /// <summary>
        /// Checks if there's any tiles that can be flipped based on the placement of the 
        /// players tile.
        /// </summary>
        /// <param name="x">Horizontal Axis in the graph</param>
        /// <param name="y">Vertical Axis in the graph</param>
        /// <param name="isRight"></param>
       private void X_AxelFlip(int x, int y, bool isRight)
        {
            if (isRight) //have i chekced the whole right side or nah?
            {
                if (!graph[x, y].EmptyTile && graph[x, y].NodeColor != startNode.NodeColor) // if this pos is not empty
                {
                    currentFlipQueue.Enqueue(graph[x, y]);
                    if (x + 1 < gridLength)
                    {
                        x++;
                        X_AxelFlip(x, y, true);
                    }
                }
                else if(!graph[x, y].EmptyTile && currentFlipQueue.Count > 0) //not sure if rright check the others as well
                {             
                    amountOfPoints += currentFlipQueue.Count;                                    
                }
            }

            else
            {
                if (!graph[x, y].EmptyTile && graph[x,y].NodeColor != startNode.NodeColor)
                {
                    currentFlipQueue.Enqueue(graph[x, y]);
                    if (x - 1 > 0)
                    {
                        x--; ;
                        X_AxelFlip(x, y, false);
                    }                
                    
                }
                else if (!graph[x, y].EmptyTile && currentFlipQueue.Count > 0)
                {
                    amountOfPoints += currentFlipQueue.Count;
                }
            }
        }

       private void Y_AxelFlip(int x, int y, bool isDown)
        {
            if (isDown)
            {
                if (!graph[x, y].EmptyTile && graph[x, y].NodeColor != startNode.NodeColor) //Down
                {
                    currentFlipQueue.Enqueue(graph[x, y]);
                    if (y + 1 < gridLength)
                    {
                        y++;
                        Y_AxelFlip(x, y, true);
                    }       
                }
                else if (!graph[x, y].EmptyTile && currentFlipQueue.Count > 0)
                {
                    amountOfPoints += currentFlipQueue.Count;
                }
            }
            else
            {
                if (!graph[x, y].EmptyTile && graph[x,y].NodeColor != startNode.NodeColor) //UP
                {
                    currentFlipQueue.Enqueue(graph[x, y]);
                    if (y - 1 > 0)
                    {
                        y--;
                        Y_AxelFlip(x, y, false);
                    }
                  
                }
                else if (!graph[x, y].EmptyTile && currentFlipQueue.Count > 0)
                {
                    amountOfPoints += currentFlipQueue.Count;
                }
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

        /// <summary>
        /// Goes through the whole graph/ playingfield and finds placeable Positions which has a neighbor next to the opponents color. 
        /// </summary>
        /// <returns></returns>
       public void GetEmptyTiles() //call this one before SetNodeVAlue
       {
            for (int y = 0; y < gridLength; y++)
            {
                for (int x = 0; x < gridLength; x++)
                {
                    if (graph[x ,y].EmptyTile)
                    {
                        //graph[x, y]. //om denna har grannar som inte har samma färg gör rekursiv kall på grannen tills det inte går eller en tom plats hittats
                        if (graph[x, y].HasValidNeighbor())
                        {
                            validMoves.Add(graph[x, y]);
                        }                    
                    }
                }
            }
       }

        /// <summary>
        /// First it finds the directon by looking at the diff in the positions from the PlaceableNode and one of the neighbors, 
        /// then it calls the method that finds the neighbors based on the placableNode and the "dir" (the directon we just found).
        /// </summary>
        private void SetNodeValue() //atm it only sets the directon but it's suppose to call a metod that then uses the placeable and directon to find if the neigbors dir is valid the  sets the highset val as the placeables value
       {        
            for (int i = 0; i < validMoves.Capacity; i++) //all playable moves (within the normal rules)
            {
                for (int j = 0; j < validMoves[i].Neighbors.Count; j++) //all neighbors of the current valid move
                {
                    //set direcon based on diff on pos
                    int x = 0;
                    int y = 0;

                    if (validMoves[i].Position.X > validMoves[i].Neighbors.ToArray()[j].Position.X)
                    {
                        x = 1;
                    }
                    else if (validMoves[i].Position.X < validMoves[i].Neighbors.ToArray()[j].Position.X)
                    {
                        x = -1;
                    }
                   
                    if (validMoves[i].Position.Y > validMoves[i].Neighbors.ToArray()[j].Position.Y)
                    {
                        y = 1;
                    }
                    else if (validMoves[i].Position.Y < validMoves[i].Neighbors.ToArray()[j].Position.Y)
                    {
                        y = -1;
                    }
                    
                    Vector2 dir = new Vector2(x, y);

                    FindNodeValue(validMoves[i], dir);
                }
            }
       }
        /// <summary>
        /// /irst takes in the placeable we're checking and dir then by using the positon of both i can iterate in the right direction using 
        /// the information from the previous method 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="directon"></param>
        private void FindNodeValue(NodeOtelloPiece node, Vector2 directon) 
        {
             
            if ((node.Position.X + directon.X) < gridLength && (node.Position.X + directon.X) <= 0 && (node.Position.Y + directon.Y) < gridLength && (node.Position.Y + directon.Y) <= 0) // så länge de är innanför planen go ahead
            { 
                if (graph[(int)(node.Position.X + directon.X), (int)(node.Position.Y + directon.Y)].EmptyTile) //if neigbor of certain dir is not empty
                {
                    if (graph[(int)(node.Position.X + directon.X), (int)(node.Position.Y + directon.Y)].NodeColor != node.NodeColor) //if the neighbor we're checking is not of the same color
                    {
                        countup++; //bara så att vi kan kolla att det faktiskt går att få dessa poängen
                        FindNodeValue(graph[(int)(node.Position.X + directon.X), (int)(node.Position.Y + directon.Y)], directon); //skicka in nästa nod och steg 
                        //kan behöva en brake här då jag inte är säker på att rekursionen är enough
                    }

                    if(graph[(int)(node.Position.X + directon.X), (int)(node.Position.Y + directon.Y)].NodeColor == node.NodeColor && countup > 0)
                    {
                        node.FlipScore = countup;
                    }                    
                }
            }
            countup = 0; // reset för next nod
        }
    }
}
