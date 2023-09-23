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
    public enum CRESTATE { cs_running = 0, cs_create, cs_cancel, cs_off }
    public class CreationWindow
    {
        //protected
        protected Point2D p;
        protected Rectangle backRect;
        protected Texture2D pixel;
        protected CRESTATE mode;
        protected SimpleStringBuilder ssb;
        protected SimpleButton4 smallButton;
        protected SimpleButton4 mediumButton;
        protected SimpleButton4 largeButton;
        protected SimpleButton4 tiles1Button;
        protected SimpleButton4 tiles2Button;
        protected SimpleButton4 tiles3Button;
        protected SimpleButton4 tiles4Button;
        protected SimpleButton4 tiles5Button;
        protected SimpleButton4 tiles6Button;
        protected SimpleButton4 createButton;
        protected SimpleButton4 cancelButton;
        protected string tiles;
        protected string size;
        //public
        /// <summary>
        /// Creation window constructir
        /// </summary>
        /// <param name="x">Window x position</param>
        /// <param name="y">Window y position</param>
        public CreationWindow(int x, int y, GenericBank<Texture2D> gb)
        {
            p = new Point2D(x, y);
            backRect.X = x;
            backRect.Y = y;
            backRect.Width = 256;
            backRect.Height = 512;
            pixel = TextureBank.getTexture("pixel");
            mode = CRESTATE.cs_off;
            ssb = new SimpleStringBuilder();
            smallButton = new SimpleButton4(TextureBank.getTexture("basic button"),
                TextureBank.getTexture("basic button"), x + 128 - 48, y + 16, "Small");
            mediumButton = new SimpleButton4(TextureBank.getTexture("basic button"),
                TextureBank.getTexture("basic button"), x + 128 - 48, y + 48, "Medium");
            largeButton = new SimpleButton4(TextureBank.getTexture("basic button"),
                TextureBank.getTexture("basic button"), x + 128 - 48, y + 96, "Large");
            tiles1Button = new SimpleButton4(TextureBank.getTexture("basic button"),
                TextureBank.getTexture("basic button"), x + 128 - 48, y + 144, "Tiles1");
            tiles2Button = new SimpleButton4(TextureBank.getTexture("basic button"),
                TextureBank.getTexture("basic button"), x + 128 - 48, y + 162, "Tiles2");
            tiles3Button = new SimpleButton4(TextureBank.getTexture("basic button"),
                TextureBank.getTexture("basic button"), x + 128 - 48, y + 210, "Tiles3");
            tiles4Button = new SimpleButton4(TextureBank.getTexture("basic button"),
                TextureBank.getTexture("basic button"), x + 128 - 48, y + 248, "Tiles4");
            tiles5Button = new SimpleButton4(TextureBank.getTexture("basic button"),
                TextureBank.getTexture("basic button"), x + 128 - 48, y + 300, "Tiles5");
            tiles6Button = new SimpleButton4(TextureBank.getTexture("basic button"),
                TextureBank.getTexture("basic button"), x + 128 - 48, y + 348, "Tiles6");
            createButton = new SimpleButton4(TextureBank.getTexture("basic button"),
                TextureBank.getTexture("basic button"), x + 128 - 48, y + 396, "Create");
            cancelButton = new SimpleButton4(TextureBank.getTexture("basic button"),
                TextureBank.getTexture("basic button"), x + 128 - 48, y + 444, "Cancel");
        }
        /// <summary>
        /// Updates internal state of window
        /// </summary>
        /// <param name="cursor">Cursor reference</param>
        /// <returns>current mode at end of update</returns>
        public virtual CRESTATE update(SimpleCursor cursor)
        {
            if (createButton.click() == "Create")
            {
                mode = CRESTATE.cs_create;
            }
            else if (createButton.click() == "Cancel")
            {
                mode = CRESTATE.cs_cancel;
            }
            if (tiles1Button.click() == "Tiles1")
            {
                tiles = "Tiles 1";
            }
            else if (tiles2Button.click() == "Tiles2")
            {
                tiles = "Tiles 2";
            }
            else if (tiles3Button.click() == "Tiles3")
            {
                tiles = "Tiles 3";
            }
            else if (tiles4Button.click() == "Tiles4")
            {
                tiles = "Tiles 4";
            }
            else if (tiles5Button.click() == "Tiles5")
            {
                tiles = "Tiles 5";
            }
            else if (tiles6Button.click() == "Tiles6")
            {
                tiles = "Tiles 6";
            }

            if (smallButton.click() == "Small")
            {
                size = "Small";
            }
            else if (mediumButton.click() == "Medium")
            {
                size = "Medium";
            }
            else if (largeButton.click() == "Large")
            {
                size = "Large";
            }
            ssb.update();
            return mode;
        }
        /// <summary>
        /// Draws the window
        /// </summary>
        /// <param name="renderer">renderer reference</param>
        /// <param name="scaler">text scling value</param>
        public virtual void draw(IntPtr renderer, float scaler = 1)
        {
            DrawRects.drawRect(renderer, backRect, ColourBank.getColour(XENOCOLOURS.GRAY), true);
            SimpleFont.drawColourString(renderer, ssb.Sequence, p.X, p.Y, "gray", scaler);
            tiles1Button.draw(renderer);
            tiles1Button.darwName(renderer);
            tiles2Button.draw(renderer);
            tiles2Button.darwName(renderer);
            tiles3Button.draw(renderer);
            tiles3Button.darwName(renderer);
            tiles4Button.draw(renderer);
            tiles4Button.darwName(renderer);
            tiles5Button.draw(renderer);
            tiles5Button.darwName(renderer);
            tiles6Button.draw(renderer);
            tiles6Button.darwName(renderer);
            smallButton.draw(renderer);
            smallButton.darwName(renderer);
            mediumButton.draw(renderer);
            mediumButton.darwName(renderer);
            largeButton.draw(renderer);
            largeButton.darwName(renderer);
            createButton.draw(renderer);
            createButton.darwName(renderer);
            cancelButton.draw(renderer);
            cancelButton.darwName(renderer);
        }
        /// <summary>
        /// Mode property
        /// </summary>
        public CRESTATE Mode
        {
            get { return mode; }
            set { mode = value; }
        }
        /// <summary>
        /// Tiles property
        /// </summary>
        public string Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }
        /// <summary>
        /// Size property
        /// </summary>
        public string Size
        {
            get { return size; }
            set { size = value; }
        }
        /// <summary>
        /// Name property
        /// </summary>
        public string Name
        {
            get { return ssb.Sequence; }
            set { ssb.Sequence = value; }
        }
    }
}
