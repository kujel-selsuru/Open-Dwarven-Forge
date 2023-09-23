//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using XenoLib;

namespace XenoLib
{
    public class SCAbilityPanel
    {
        //protected
        protected List<SCRTSAbility> abilities;
        protected Rectangle panelBox;
        protected Point2D point;
        protected SimpleButton4 nextButton;
        protected SimpleButton4 backButton;
        protected int iconPage;
        protected int iconPageSize;
        protected SCRTSAbility clickedAbility;
        protected CoolDown clickDelay;
        //public 
        /// <summary>
        /// SCAbilitiesPanel constructor
        /// </summary>
        public SCAbilityPanel()
        {
            abilities = new List<SCRTSAbility>();
            panelBox = new Rectangle(0, 752, 128, 128);
            point = null;
            nextButton = new SimpleButton4(TextureBank.getTexture("right arrow"),
                TextureBank.getTexture("right arrow"), 128, 752, "next");
            backButton = new SimpleButton4(TextureBank.getTexture("right arrow"),
                TextureBank.getTexture("right arrow"), 160, 752, "back");
            iconPage = 0;
            iconPageSize = 12;
            clickedAbility = null;
            clickDelay = new CoolDown(20);
        }
        /// <summary>
        /// Updates AbilityPanel internal state
        /// </summary>
        public void update()
        {
            point = new Point2D(MouseHandler.getMouseX(), MouseHandler.getMouseY());
            clickedAbility = null;
            if (panelBox.pointInRect(point) == true)
            {
                for (int i = (12 * iconPage); i < iconPageSize; i++)
                {
                    if (abilities[i].clicked() == true)
                    {
                        clickedAbility = abilities[i];
                        break;
                    }
                    if(MouseHandler.getRight() == true)
                    {
                        if(clickDelay.Active == false)
                        {
                            if(abilities[i].AutoCast == true)
                            {
                                abilities[i].AutoCast = false;
                            }
                            else
                            {
                                abilities[i].AutoCast = true;
                            }
                            clickDelay.activate();
                        }
                    }
                }
            }
            if (nextButton.clicked() == true)
            {
                if (abilities.Count > ((iconPage + 1) * 12))
                {
                    iconPage++;
                }
            }
            if (backButton.clicked() == true)
            {
                if (iconPage > 0)
                {
                    iconPage--;
                }
            }
            nextButton.update();
            backButton.update();
            clickDelay.update();
        }
        /// <summary>
        /// Draws AbilitiesPanel
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            int x = 0;
            int y = 0;
            if (abilities.Count > 12)
            {
                iconPageSize = (12 * iconPage) + (abilities.Count - (12 * iconPage));
                if (iconPageSize > (12 * iconPage) + 12)
                {
                    iconPageSize = (12 * iconPage) + 12;
                }
                for (int i = (12 * iconPage); i < iconPageSize; i++)
                {
                    y = i / 4;
                    x = i - (y * 4);
                    abilities[i].drawAtPos(renderer, 0, x * 32, y * 32);
                }
            }
            else
            {
                for (int i = 0; i < abilities.Count - 1; i++)
                {
                    y = i / 4;
                    x = i - (y * 4);
                    abilities[i].drawAtPos(renderer, 0, x * 32, y * 32);
                }
            }
            nextButton.draw(renderer);
            backButton.draw(renderer);
        }
        /// <summary>
        /// Populates panel's list of abilities
        /// </summary>
        /// <param name="unit">SCRTSUnit reference</param>
        public void populatePanel(SCRTSUnit unit)
        {
            abilities.Clear();
            for (int i = 0; i < unit.Abilities.Count; i++)
            {
                abilities.Add(unit.Abilities[i]);
            }
            iconPage = 0;
        }
        /// <summary>
        /// Clears the clicked ability
        /// </summary>
        public void clearLastSelected()
        {
            clickedAbility = null;
        }
        /// <summary>
        /// Abilities property
        /// </summary>
        public List<SCRTSAbility> Abilities
        {
            get { return abilities; }
            set { abilities = value; }
        }
        /// <summary>
        /// ClickedAbility property
        /// </summary>
        public SCRTSAbility ClickedAbility
        {
            get { return clickedAbility; }
        }
    }
}
