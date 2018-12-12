using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otello
{
    class NodeOtelloPiece
    {
        public Queue<NodeOtelloPiece> Neighbors { get; private set; }
        public bool EmptyTile { get; set; }
        public Rectangle MyHitbox { get; set; }
        public Texture2D OtelloTileTex { get; set; }
        public Color NodeColor { get; set; }
        Vector2 position;

       

        public NodeOtelloPiece(Vector2 pos, Texture2D tex)
        {
            Neighbors = new Queue<NodeOtelloPiece>();
            this.position = pos;
            this.OtelloTileTex = tex;
            EmptyTile = true;

        }

        public void AddNehigbors(NodeOtelloPiece node) //ta in min pos hitta grannar
        {
            Neighbors.Enqueue(node);      
        }       

        public bool HasValidNeighbor()
        {
            bool hasNeighbor = false;
            foreach (var neighbor in Neighbors)
            {
                if (neighbor.NodeColor != NodeColor)
                {
                    hasNeighbor = true;
                    break;
                }
            }
            return hasNeighbor;
        }
        //kanske en metod som kan kallas som kollar alla grannar och ser ifall de inte har samma färg & inte är tomma etc
    }
}
