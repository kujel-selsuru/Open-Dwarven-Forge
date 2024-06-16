//====================================================
//Written by Kujel Selsuru
//Last Updated 16/06/24
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenoLib
{
    public class SCRTSSelectedPanel
    {
        //protected
        protected Rectangle panelBox;
        protected Rectangle drawBox;
        protected Rectangle srcBox;
        protected Rectangle healthBar;

        //public
        /// <summary>
        /// SelectedPanel constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Width in pixels</param>
        /// <param name="h">Height in pixels</param>
        public SCRTSSelectedPanel(int x, int y, int w = 704, int h = 160)
        {
            panelBox = new Rectangle(x, y, w, h);
            drawBox = new Rectangle(0, 0, 32, 32);
            srcBox = new Rectangle(0, 0, 32, 32);
            healthBar = new Rectangle(0, 0, 32, 4);
        }
        /// <summary>
        /// Updates SelectedPanel internal state
        /// </summary>
        /// <param name="selected">List of SCRTSUnits</param>
        public void update(List<SCRTSUnit> selected)
        {
            if(selected != null)
            {
                if(panelBox.pointInRect(MouseHandler.getTip()) == true)
                {
                    if(MouseHandler.getLeft() == true)
                    {
                        for(int i = 0; i < selected.Count; i++)
                        {
                            if(i > 55)//don't check past 56th object
                            {
                                break;
                            }
                            int renderY = i / 22;
                            int renderX = i - (renderY * 22);
                            drawBox.X = panelBox.X + renderX * 32;
                            drawBox.Y = panelBox.Y + renderY * 32;
                            if(drawBox.pointInRect(MouseHandler.getTip()) == true)
                            {
                                SCRTSUnit tmp = selected[i];
                                selected.Clear();
                                selected.Add(tmp);
                                break;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Draws SelectedPanel
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="selected">List of SCRTSUnits</param>
        public void draw(IntPtr renderer, List<SCRTSUnit> selected)
        {
            int renderX = 0;
            int renderY = 0;
            DrawRects.drawRect(renderer, panelBox, ColourBank.getColour(XENOCOLOURS.BLACK), true);
            if(selected != null)
            {
                if(selected.Count > 0)
                {
                    for(int i = 0; i < selected.Count; i++)
                    {
                        if(i > 55)//stop rendering units beyond 56
                        {
                            break;
                        }
                        renderY = i / 22;
                        renderX = i - (renderY * 22);
                        drawBox.X = panelBox.X + renderX * 32;
                        drawBox.Y = panelBox.Y + renderY * 32;
                        srcBox.Width = selected[i].SrcRect.w;
                        srcBox.Height = selected[i].SrcRect.h;
                        SimpleDraw.draw(renderer, selected[i].Source, srcBox, drawBox);
                        healthBar.Width = 32 * (selected[i].HP / selected[i].MaxHP);
                        healthBar.X = renderX;
                        healthBar.Y = renderY - 4;
                        DrawRects.drawRect(renderer, healthBar, ColourBank.getColour(XENOCOLOURS.LIGHT_GREEN), true);
                    }
                }
            }
            DrawRects.drawRect(renderer, panelBox, ColourBank.getColour(XENOCOLOURS.GRAY), false);
        }
        /// <summary>
        /// PanelBox property
        /// </summary>
        public Rectangle PanelBox
        {
            get { return panelBox; }
        }
    }
}
