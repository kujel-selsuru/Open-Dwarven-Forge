//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace XenoLib
{
    public static class LayeredSpriteRenderer
    {
        public static List<XenoSprite> sprites;
        /// <summary>
        /// Draws sprites in a layered format to provide a depth effect
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="wx">Window X value in pixels</param>
        /// <param name="wy">Window Y value in pixels</param>
        /// <param name="ww">Window width value in pixels</param>
        /// <param name="wh">Window height value in pixels</param>
        /// <param name="scaler">Tile height/scaling value</param>
        public static void renderSprites(IntPtr renderer, 
            int wx, int wy, int ww, int wh, int scaler = 32)
        {
            if(sprites != null)
            {
                //range of layers to render
                for(int i = wy / scaler; i < (wy / scaler) + (wh / scaler); i++)
                {
                    //list of sprites to render
                    for(int k = 0; k < sprites.Count; k++)
                    {
                        //sprite is on screen
                        if(sprites[k].X >= wx && sprites[k].X < wx + ww)
                        {
                            if(sprites[k].Y >= wy && sprites[k].Y < wy + wh)
                            {
                                //sprite's bottom edge
                                if(sprites[k].Bottom / 32 == i)
                                {
                                    sprites[k].draw(renderer, wx, wy);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
