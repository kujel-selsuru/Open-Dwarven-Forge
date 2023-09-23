//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenoLib
{
public class RTSMiniMap
    {
        protected Rectangle pixelBox;
        protected Rectangle box;
        protected Rectangle box2;
        protected Point2D scale;
        protected int fogScale;
        protected WorldScene engine;
        protected Texture2D brownPixel;
        protected Texture2D bluePixel;
        protected Texture2D redPixel;
        protected Texture2D greenPixel;
        protected Texture2D yellowPixel;
        protected Texture2D goldPixel;
        protected Texture2D silverPixel;
        protected Texture2D blackPixel;
        /// <summary>
        /// A minimap to display RTS world state, ie showing buildings, units and resources
        /// </summary>
        /// <param name="w">Minimap width</param>
        /// <param name="h">Minimap height</param>
        /// <param name="x">Minimap x position</param>
        /// <param name="y">Minimap y position</param>
        /// <param name="engine">World scene currently running</param>
        /// <param name="fog">Fog of war reference</param>
        /// <param name="scaler">scalling value, width of tiles</param>
        public RTSMiniMap(int w, int h, int x, int y, WorldScene engine, FogOfWar fog, int scaler)
        {
            this.engine = engine;
            scale = new Point2D(scaler / ((float)engine.WorldPixelsW / (float)w), (scaler / ((float)engine.WorldPixelsH / (float)h)));
            fogScale = (int)((fog.PatchWidth * 2) / scale.X);
            pixelBox = new Rectangle(0, 0, 1, 1);
            box = new Rectangle(x, y, w, h);
            box2 = new Rectangle(x, y, w, h);
            brownPixel = TextureBank.getTexture("brown pixel");
            bluePixel = TextureBank.getTexture("blue pixel");
            redPixel = TextureBank.getTexture("red pixel");
            greenPixel = TextureBank.getTexture("green pixel");
            goldPixel = TextureBank.getTexture("gold pixel");
            silverPixel = TextureBank.getTexture("silver pixel");
            blackPixel = TextureBank.getTexture("black pixel");
        }
        /// <summary>
        /// Draws the minimap object
        /// </summary>
        /// <param name="renderer">renderer reference</param>
        /// <param name="fog">Fog of war refereence</param>
        /// <param name="winx">Current winx value</param>
        /// <param name="winy">current winy value</param>
        public void draw(IntPtr renderer, FogOfWar fog, int winx = 0, int winy = 0)
        {
            pixelBox.Width = 1;
            pixelBox.Height = 1;
            SimpleDraw.draw(renderer, brownPixel, pixelBox);
            for (int x = 0; x < engine.Ores.Width; x++)
            {
                for (int y = 0; y < engine.Ores.Height; y++)
                {
                    pixelBox.X = (int)((x * scale.X) + box.X);
                    pixelBox.Y = (int)((y * scale.Y) + box.Y);
                    if (engine.Ores.atPosition(x, y) != null)
                    {
                        SimpleDraw.draw(renderer, silverPixel, pixelBox);
                    }
                }
            }
            for (int p = 0; p < engine.CMDRS.Count; p++)
            {
                pixelBox.Width = 2;
                pixelBox.Height = 2;
                /*
                for (int a = 0; a < engine.Players[p].Air.Count; a++)
                {
                    pixelBox.X = (int)((engine.Players[p].Air[a].X / engine.Region.TileWidth) * scale.X) + box.X;
                    pixelBox.Y = (int)((engine.Players[p].Air[a].Y / engine.Region.TileHeight) * scale.Y) + box.Y;
                    if (p == 0)
                    {
                        sb.Draw(pixel, pixelBox, Color.LightGreen);
                    }
                    else
                    {
                        sb.Draw(pixel, pixelBox, Color.Red);
                    }
                }
                for (int b = 0; b < engine.Players[p].Builders.Count; b++)
                {
                    pixelBox.X = (int)((engine.Players[p].Builders[b].X / engine.Region.TileWidth) * scale.X) + box.X;
                    pixelBox.Y = (int)((engine.Players[p].Builders[b].Y / engine.Region.TileHeight) * scale.Y) + box.Y;
                    if (p == 0)
                    {
                        sb.Draw(pixel, pixelBox, Color.LightGreen);
                    }
                    else
                    {
                        sb.Draw(pixel, pixelBox, Color.Red);
                    }
                }
                for (int h = 0; h < engine.Players[p].Harvesters.Count; h++)
                {
                    pixelBox.X = (int)((engine.Players[p].Harvesters[h].X / engine.Region.TileWidth) * scale.X) + box.X;
                    pixelBox.Y = (int)((engine.Players[p].Harvesters[h].Y / engine.Region.TileHeight) * scale.Y) + box.Y;
                    if (p == 0)
                    {
                        sb.Draw(pixel, pixelBox, Color.LightGreen);
                    }
                    else
                    {
                        sb.Draw(pixel, pixelBox, Color.Red);
                    }
                }
                    */
                for (int g = 0; g < engine.CMDRS[p].Units.Count; g++)
                {
                    pixelBox.X = (int)((engine.CMDRS[p].Units[g].X / engine.TileWidth) * scale.X) + box.X;
                    pixelBox.Y = (int)((engine.CMDRS[p].Units[g].Y / engine.TileHeight) * scale.Y) + box.Y;
                    if (p == 0)
                    {
                        SimpleDraw.draw(renderer, bluePixel, pixelBox);
                    }
                    else
                    {
                        SimpleDraw.draw(renderer, redPixel, pixelBox);
                    }
                }
                pixelBox.Width = 4;
                pixelBox.Height = 4;
                for (int b = 0; b < engine.CMDRS[p].Buildings.Count; b++)
                {
                    pixelBox.X = (int)((engine.CMDRS[p].Buildings[b].X / engine.TileWidth) * scale.X) + box.X;
                    pixelBox.Y = (int)((engine.CMDRS[p].Buildings[b].Y / engine.TileHeight) * scale.Y) + box.Y;
                    if (p == 0)
                    {
                        SimpleDraw.draw(renderer, bluePixel, pixelBox);
                    }
                    else
                    {
                        SimpleDraw.draw(renderer, redPixel, pixelBox);
                    }
                }
            }
            pixelBox.Width = 4;
            pixelBox.Height = 4;

            for (int x = 0; x < fog.Width; x++)
            {
                for (int y = 0; y < fog.Height; y++)
                {
                    pixelBox.X = (int)(x * scale.X) * 2 + box.X;
                    pixelBox.Y = (int)(y * scale.Y) * 2 + box.Y;
                    if (fog.grid(x, y) == true)
                    {
                        SimpleDraw.draw(renderer, blackPixel, pixelBox);
                    }
                }
            }
            int bx = (int)box.X + (int)((engine.Winx / engine.TileWidth) * scale.X);
            int by = (int)box.Y + (int)((engine.Winy / engine.TileHeight) * scale.Y);
            int bw = (int)((engine.WinWidth / scale.X) * scale.X);
            int bh = (int)((engine.WinHeight / scale.Y) * scale.Y);
            Rectangle winBox = new Rectangle(bx, by, bw, bh);
            DrawRects.drawRect(renderer, winBox, ColourBank.getColour(XENOCOLOURS.WHITE), false);
            DrawRects.drawRect(renderer, box, ColourBank.getColour(XENOCOLOURS.WHITE), false);
        }
        /// <summary>
        /// Clicking on minimap handling
        /// </summary>
        /// <param name="engine">World scene currently running</param>
        public void click(WorldScene engine)
        {
            if (MouseHandler.getLeft() == true)
            {
                if (box2.pointInRect(new Point2D(MouseHandler.getMouseX(), MouseHandler.getMouseY())))
                {
                    engine.setWindow((int)(((MouseHandler.getMouseX() - box.X) / scale.X) * engine.TileWidth),
                        (int)(((MouseHandler.getMouseY() - box.Y) / scale.Y) * engine.TileHeight));
                }
            }
            if (MouseHandler.getRight() == true)
            {
                if (box2.pointInRect(new Point2D(MouseHandler.getMouseX(), MouseHandler.getMouseY())))
                {
                    engine.CMDRS[0].sendSelected((int)((MouseHandler.getMouseX() - box.X) / scale.X) *
                        engine.TileWidth, (int)((MouseHandler.getMouseY() - box.Y) / scale.Y) *
                        engine.TileHeight);
                }
            }
        }
        /// <summary>
        /// Clicking on minimap handling
        /// </summary>
        /// <param name="mx">mouse x state</param>
        /// <param name="my">mouse y state</param>
        /// <param name="engine">World scene currently running</param>
        public void click2(float mx, float my, WorldScene engine)
        {
            if (MouseHandler.getLeft() == true)
            {
                if (box2.pointInRect(new Point2D(mx, my)))
                {
                    engine.setWindow(engine.multipleTW((int)(((mx - box.X) / scale.X) * engine.TileWidth)),
                        engine.multipleTH((int)(((my - box.Y) / scale.Y) * engine.TileHeight)));
                }
            }
            if (MouseHandler.getRight() == true)
            {
                if (box2.pointInRect(new Point2D(mx, my)))
                {
                    engine.CMDRS[0].sendSelected((int)((mx - box.X) / scale.X) *
                        engine.TileWidth, (int)((my - box.Y) / scale.Y) *
                        engine.TileHeight);
                }
            }
        }
        /// <summary>
        /// Tests if a rectangle intersects with the minimap's box rectangle
        /// </summary>
        /// <param name="rect">Rectangle to test</param>
        /// <returns>returns ture if collsion else returns false</returns>
        public bool intersecting(Rectangle rect)
        {
            if (box.intersects(rect))
            {
                return true;
            }
            return false;
        }
    }
}
