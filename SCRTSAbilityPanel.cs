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
    public class SCRTSAbilityPanel
    {
        //protected
        protected Rectangle panelBox;
        protected SimpleButton4 advance;
        protected SimpleButton4 reverse;
        protected bool secondPanel;

        //public
        /// <summary>
        /// SCRTSAbilityPanel constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Width in pixels</param>
        /// <param name="h">Height in pixels</param>
        public SCRTSAbilityPanel(int x, int y, int w = 3 * 32, int h = 5 * 32)
        {
            panelBox = new Rectangle(x, y, w, h);
            advance = new SimpleButton4(TextureBank.getTexture("advance button_32_32"), TextureBank.getTexture("advance button_32_32"), x + w, y + h, "Advance");
            reverse = new SimpleButton4(TextureBank.getTexture("reverse button_32_32"), TextureBank.getTexture("reverse button_32_32"), x + w, y + h, "Reverese");
            secondPanel = false;
        }
        /// <summary>
        /// Updates AbilityPanel internal state
        /// </summary>
        /// <param name="soldier">SCRTSUnit reference</param>
        public void update(SCRTSUnit soldier)
        {
            if(secondPanel == false)
            {
                if(soldier.Abilities.Count > 11)
                {
                    if(advance.clicked() == true)
                    {
                        secondPanel = true;
                    }
                    for(int i = 0; i < 11; i++)
                    {
                        if(soldier.Abilities[i].clicked() == true)
                        {
                            soldier.callAbility(soldier.Abilities[i].Name);
                        }
                    }
                }
                else
                {
                    for(int i = 0; i < soldier.Abilities.Count - 1; i++)
                    {
                        if(soldier.Abilities[i].clicked() == true)
                        {
                            soldier.callAbility(soldier.Abilities[i].Name);
                        }
                    }
                }
            }
            else
            {
                if(reverse.clicked() == true)
                {
                    secondPanel = false;
                }
                for(int i = 11; i < soldier.Abilities.Count - 1; i++)
                {
                    if(soldier.Abilities[i].clicked() == true)
                    {
                        soldier.callAbility(soldier.Abilities[i].Name);
                    }
                }
            }
        }
        /// <summary>
        /// Draws AbilityPanel
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="soldier">SCRTSUnit reference</param>
        public void draw(IntPtr renderer, SCRTSUnit soldier = null)
        {
            DrawRects.drawRect(renderer, panelBox, ColourBank.getColour(XENOCOLOURS.BLACK), true);
            int renderX = 0;
            int renderY = 0;
            if(soldier != null)
            {
                if(soldier.Abilities.Count > 11)
                {
                    if(secondPanel == false)
                    {
                        for(int i = 0; i < 11; i++)
                        {
                            renderY = i / 4;
                            renderX = i - (renderY * 4);
                            soldier.Abilities[i].drawAt(renderer, (renderX * 32) + panelBox.IX + 1, 
                                (renderY * 32) + panelBox.IY, 32, 32);
                        }
                        advance.draw(renderer);
                    }
                    else
                    {
                        for(int i = 0; i < 11; i++)
                        {
                            renderY = (i - 11) / 4;
                            renderX = (i - 11) - (renderY * 4);
                            soldier.Abilities[i].drawAt(renderer, (renderX * 32) + panelBox.IX + 1, (renderY * 32) + panelBox.IY);
                        }
                        reverse.draw(renderer);
                    }
                }
                else
                {
                    for(int i = 0; i < soldier.Abilities.Count - 1; i++)
                    {
                        renderY = i / 4;
                        renderX = i - (renderY * 4);
                        soldier.Abilities[i].drawAt(renderer, (renderX * 32) + panelBox.IX + 1, (renderY * 32) + panelBox.IY);
                    }
                }
            }
            DrawRects.drawRect(renderer, panelBox, ColourBank.getColour(XENOCOLOURS.GRAY), false);
        }
        /// <summary>
        /// Resets AbilityPanel to default state
        /// </summary>
        public void reset()
        {
            secondPanel = false;
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
