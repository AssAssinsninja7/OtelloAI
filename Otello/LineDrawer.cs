using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otello
{
    class LineDrawer
    {
        Texture2D lineTex;
        int windowRectSize = 400;
 
        int gridTileSize = 50; // should not be hardcoded

        public LineDrawer(Texture2D lineTex)
        {
            this.lineTex = lineTex;
            lineTex.SetData(new Color[] { Color.White });
        }

        public void DrawGrid(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < 8; x++)
            {
                Rectangle xRect = new Rectangle(x * gridTileSize, 0, 1, windowRectSize);
                spriteBatch.Draw(lineTex, xRect, Color.Black);
            }

            for (int y= 0; y < 8; y++)
            {
                Rectangle yRect = new Rectangle(0, y * gridTileSize, windowRectSize, 1);
                spriteBatch.Draw(lineTex, yRect, Color.Black);
            }
        }
    }
}
