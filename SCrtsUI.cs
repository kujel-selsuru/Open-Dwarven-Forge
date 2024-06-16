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
    public class SCrtsUI
    {
        //protected
        protected Rectangle selectBox;
        protected Rectangle drawBox;
        protected bool selectingBox;
        protected Point2D start;
        protected Point2D end;
        protected RTSWorld world;
        protected bool clicked;
        protected bool prevClicked;
        protected SCRadar miniMap;
        protected SCRTSAbilityPanel abilityPanel;
        protected SCRTSSelectedPanel selectedPanel;
        protected SimpleCursor cursor;
        protected Point2D mouseTip;

        //public

        public SCrtsUI(RTSWorld world)
        {
            this.world = world;
            start = new Point2D(0, 0);
            end = new Point2D(0, 0);
            selectBox = new Rectangle(0, 0, 1, 1);
            drawBox = new Rectangle(0, 0, 1, 1);
            selectingBox = false;
            clicked = false;
            prevClicked = false;
            miniMap = new SCRadar((32 * 32) - 192, 672 - 192, ((RTSCell)world.Alpha).CellWidthInPixels, ((RTSCell)world.Alpha).CellHeightInPixels);
            abilityPanel = new SCRTSAbilityPanel(0, 672 - (5 * 32), 128, (5 * 32));
            selectedPanel = new SCRTSSelectedPanel(128, 672 - 128, 704, 128);
            cursor = new SimpleCursor(TextureBank.getTexture("Invisible Button_32_#2"), 10);
            mouseTip = null;
            
        }

        public void update(IntPtr renderer, SCRTSCommander commander, bool singleCell = false, int shiftX = 0, int shiftY = 0)
        {
            mouseTip = MouseHandler.getTip();
            miniMap.trackWin(world, singleCell);
            if(miniMap.Box.pointInRect(MouseHandler.getTip()) == true)
            {
                miniMap.update(renderer, world, singleCell);
            }
            else if(miniMap.ButtonBack.pointInRect(MouseHandler.getTip()) == true)
            {
                miniMap.updateButtons(renderer, world);
            }
            else if(selectedPanel.PanelBox.pointInRect(MouseHandler.getTip()) == true)
            {
                selectedPanel.update(commander.SelectedObjects);
            }
            else if(abilityPanel.PanelBox.pointInRect(MouseHandler.getTip()) == true)
            {
                if(commander.SelectedObjects.Count > 0)
                {
                    abilityPanel.update(commander.SelectedObjects[0]);
                }
            }
            else
            {
                if(MouseHandler.getLeft() == true)//will not be checked if mouse is over command panels
                {
                    clicked = true;
                }
                else
                {
                    clicked = false;
                    selectingBox = false;
                }
                if(clicked == true && prevClicked == false)
                {
                    start.X = MouseHandler.getMouseX();
                    start.Y = MouseHandler.getMouseY();
                    end.X = start.X;
                    end.Y = start.Y;
                    world.Commanders[0].selectSingle(world, cursor);
                    selectingBox = true;
                }
                if(clicked == true && prevClicked == true)
                {
                    end.X = MouseHandler.getMouseX();
                    end.Y = MouseHandler.getMouseY() - 32;
                    selectBox.Width = Math.Abs(start.X - end.X);
                    selectBox.Height = Math.Abs(start.Y - end.Y);
                    drawBox.Width = selectBox.Width;
                    drawBox.Height = selectBox.Height;
                    if(start.X > end.X)
                    {
                        selectBox.X = end.X + world.WindowRect.X + shiftX;
                        //drawBox.X = (end.X) - world.WindowRect.X + shiftX;
                        drawBox.X = (end.X) + shiftX;
                    }
                    else
                    {
                        selectBox.X = start.X + world.WindowRect.X + shiftX;
                        //drawBox.X = (start.X) - world.WindowRect.X + shiftX;
                        drawBox.X = (start.X) + shiftX;
                    }
                    if(start.Y > end.Y)
                    {
                        selectBox.Y = end.Y + world.WindowRect.Y + shiftY;
                        //drawBox.Y = (end.Y) - world.WindowRect.Y + shiftY;
                        drawBox.Y = (end.Y) + shiftY;
                    }
                    else
                    {
                        selectBox.Y = start.Y + world.WindowRect.Y + shiftY;
                        //drawBox.Y = (start.Y) - world.WindowRect.Y + shiftY;
                        drawBox.Y = (start.Y)+ shiftY;
                    }
                }
                if(clicked == false && prevClicked == true)
                {
                    int sector = world.getSector(world.WindowRect.IX + MouseHandler.getMouseX(),
                        world.WindowRect.IY + MouseHandler.getMouseY());
                    world.Commanders[0].selectBoxedUnits(sector, selectBox);
                }
                prevClicked = clicked;
                if(MouseHandler.getRight() == true)
                {
                    //issue commands to selected units
                }
            }
            if (miniMap.Box.pointInRect(mouseTip) == false &&
                abilityPanel.PanelBox.pointInRect(mouseTip) == false &&
                selectedPanel.PanelBox.pointInRect(mouseTip) == false)
            {
                if (MouseHandler.getMouseX() < ((RTSWorld)world).WindowInnerRect.X && MouseHandler.getMouseX() > 0)
                {
                    ((RTSCell)world.Alpha).scrollCellWin(-2, 0);//scroll left
                    miniMap.trackCellWin(world, -2, 0);
                }
                if (MouseHandler.getMouseX() > ((RTSWorld)world).WindowInnerRect.X + ((RTSWorld)world).WindowInnerRect.Width &&
                    MouseHandler.getMouseX() < ((RTSWorld)world).WindowRect.Width + ((RTSWorld)world).WindowRect.X)
                {
                    ((RTSCell)world.Alpha).scrollCellWin(2, 0);//scroll right
                    miniMap.trackCellWin(world, 2, 0);
                }
                if (MouseHandler.getMouseY() < ((RTSWorld)world).WindowInnerRect.Y && MouseHandler.getMouseY() > 0)
                {
                    ((RTSCell)world.Alpha).scrollCellWin(0, -2);//scroll up
                    miniMap.trackCellWin(world, 0, -2);
                }
                if (MouseHandler.getMouseY() > ((RTSWorld)world).WindowInnerRect.Y + ((RTSWorld)world).WindowInnerRect.Height &&
                    MouseHandler.getMouseY() < ((RTSWorld)world).WindowRect.Y + ((RTSWorld)world).WindowRect.Height)
                {
                    ((RTSCell)world.Alpha).scrollCellWin(0, 2);//scroll down
                    miniMap.trackCellWin(world, 0, 2);
                }
            }
        }

        public void draw(IntPtr renderer, SCRTSCommander commander, RTSWorld world)
        {
            if(clicked == true && prevClicked == true)
            {
                DrawRects.drawRect(renderer, drawBox, ColourBank.getColour(XENOCOLOURS.GREEN), false);
            }
            if (commander.SelectedObjects.Count > 0)
            {
                abilityPanel.draw(renderer, commander.SelectedObjects[0]);
            }
            else
            {
                abilityPanel.draw(renderer);
            }
            commander.drawSelectedBoxes(renderer, world);
            selectedPanel.draw(renderer, commander.SelectedObjects);
            miniMap.draw(renderer, world);
        }

        public void initRadar(IntPtr renderer, RTSWorld world)
        {
            miniMap.buildQuadrant(renderer, world, QUADRANT.ALPHA);
        }

        public void trackCellWin(int wx = 0, int wy = 0)
        {
            miniMap.trackCellWin(world, wx, wy);
        }
    }
}
