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
public class RTSEditorBase
    {
        //protected
        protected string mapName;
        protected WorldScene world;
        protected RTSTilePallet pallet;
        protected ScrollingList2 objectList;
        protected DropDownList dropList;
        protected SimpleButton4 filesButton;
        protected SimpleButton4 objectsButton;
        protected SimpleButton4 factionsButton;
        protected SimpleButton4 exitButton;
        protected SimpleButton4 scrollUp;
        protected SimpleButton4 scrollRight;
        protected SimpleButton4 scrollDown;
        protected SimpleButton4 scrollLeft;
        protected Rectangle placementWin;
        protected Rectangle selectionWin;
        protected Rectangle mapWin;
        protected string currentMode;
        protected string faction;
        protected int cmdrIndex;
        protected RTSUnit selectedUnit;
        protected RTSBuilding selectedBuilding;
        protected RTSResource selectedResource;
        protected RTSScript selectedScript;
        protected RTSAction selectedAction;
        protected MBS mbs;
        protected GenericBank<RTSResource> resourceDB;
        protected GenericBank<RTSScript> scriptDB;
        protected GenericBank<RTSCommander> CMDRDB;
        protected GenericBank<RTSAction> ACTDB;
        protected bool exit;
        protected string[] dropListOptions;
        protected RTSMiniMap miniMap;
        protected CoolDown delay;
        protected SimpleStringBuilder ssb;
        protected CreationWindow creWin;
        protected FieldEditor fieldsMenu;
        protected CoolDown mbsDelay;
        protected GenericBank<Texture2D> gb;
        protected int mx;
        protected int my;
        protected bool renderObject;
        protected bool drawCursor;

        //public
        /// <summary>
        /// RTSEditorBase constructor 
        /// </summary>
        /// <param name="mapName">map's name</param>
        public RTSEditorBase(string mapName = "untitled")
        {
            this.mapName = mapName;
            currentMode = "tiles";
            faction = "Dwarves";
            cmdrIndex = 0;
            placementWin = new Rectangle(32, 96, 15 * 32, 15 * 32);
            selectionWin = new Rectangle(17 * 32, 96, 15 * 32, 15 * 32);
            mapWin = new Rectangle(17 * 32, 13 * 32, 200, 200);
            objectsButton = new SimpleButton4(TextureBank.getTexture("basic button"), TextureBank.getTexture("basic button"), 0, 0, "objects");
            factionsButton = new SimpleButton4(TextureBank.getTexture("basic button"), TextureBank.getTexture("basic button"), 192, 0, "factions");
            filesButton = new SimpleButton4(TextureBank.getTexture("basic button"), TextureBank.getTexture("basic button"), 2 * 192, 0, "files");
            exitButton = new SimpleButton4(TextureBank.getTexture("basic button"), TextureBank.getTexture("basic button"), 0, 192 + (15 * 32), "exit");
            scrollUp = new SimpleButton4(TextureBank.getTexture("up"), TextureBank.getTexture("up"), ((15 * 32) / 2) + 32, 64, "up");
            scrollRight = new SimpleButton4(TextureBank.getTexture("right"), TextureBank.getTexture("right"), (15 * 32) + 32, ((15 * 32) / 2) + 64, "right");
            scrollDown = new SimpleButton4(TextureBank.getTexture("down"), TextureBank.getTexture("down"), ((15 * 32) / 2) + 32, (15 * 32) + 96, "down");
            scrollLeft = new SimpleButton4(TextureBank.getTexture("left"), TextureBank.getTexture("left"), 0, ((15 * 32) / 2) + 64, "left");
            resourceDB = new GenericBank<RTSResource>();
            scriptDB = new GenericBank<RTSScript>();
            CMDRDB = new GenericBank<RTSCommander>();
            ACTDB = new GenericBank<RTSAction>();
            exit = false;
            world = new WorldScene(200, 200, 32, 32, TextureBank.getTexture("tiles"), "tiles");
            world.CMDRS.Add(new RTSCommander(0, world, "fog graphic"));
            world.CMDRS[0].loadDataBases(gb);
            world.CMDRS.Add(new RTSCommander(1, world, "fog graphic"));
            world.CMDRS[1].loadDataBases(gb);
            world.CMDRS.Add(new RTSCommander(2, world, "fog graphic"));
            world.CMDRS[2].loadDataBases(gb);
            world.CMDRS[0].clearAllFog();
            world.CMDRS[1].clearAllFog();
            world.CMDRS[2].clearAllFog();
            miniMap = new RTSMiniMap(200, 200, 17 * 32 + 1, 96 + (10 * 32) + 1, world, world.CMDRS[0].Fog, 32);
            delay = new CoolDown(5);
            selectedUnit = null;
            selectedBuilding = null;
            selectedResource = null;
            selectedScript = null;
            selectedAction = null;
            dropListOptions = new string[5];
            ssb = new SimpleStringBuilder(5);
            creWin = new CreationWindow(256, 128, gb);
            fieldsMenu = new FieldEditor(17 * 32 + 200, 13 * 32 + 1, 244, TextureBank.getTexture("background"), 1);
            dropList = new DropDownList(ColourBank.getColour(XENOCOLOURS.BLACK), ColourBank.getColour(XENOCOLOURS.WHITE), 0, 0, 160, 32, 5);
            pallet = new RTSTilePallet(17 * 32, 3 * 32, 5 * 32, 7 * 32, 32, 32, TextureBank.getTexture("tiles"), TextureBank.getTexture("pixel"), "tiles", 5 * 32, 7 * 32);
            objectList = new ScrollingList2(TextureBank.getTexture("dbp"), TextureBank.getTexture("dbd"), TextureBank.getTexture("udp"), TextureBank.getTexture("ubd"), TextureBank.getTexture("db"), 
                (17 * 32), 64, 9 * 32, 11 * 32, 32, 32, 7);
            mbsDelay = new CoolDown(7);
            drawCursor = true;
            //world.CMDRS[0].addUnit("scout", 96, 192, 0);
        }
        /// <summary>
        /// Updates the base editor
        /// </summary>
        public virtual void update(SimpleCursor cursor)
        {
            //handle getting mouse button presses
            renderObject = false;
            if (MouseHandler.getLeft() == true)
            {
                if (!mbsDelay.Active)
                {
                    mbsDelay.activate();
                    mbs = MBS.left;
                }
            }
            else if (MouseHandler.getRight() == true)
            {
                if (!mbsDelay.Active)
                {
                    mbsDelay.activate();
                    mbs = MBS.right;
                }
            }
            mbsDelay.update();
            if (creWin.Mode != CRESTATE.cs_off)
            {
                if (creWin.Mode == CRESTATE.cs_running)
                {
                    switch (creWin.update(cursor))
                    {
                        case CRESTATE.cs_running:
                            //do nothing special
                            break;
                        case CRESTATE.cs_create:
                            switch (creWin.Size)
                            {
                                case "Small":
                                    createNewMap(400, 400, 32, 32, creWin.Tiles);
                                    break;
                                case "Medium":
                                    createNewMap(1600, 1600, 32, 32, creWin.Tiles);
                                    break;
                                case "Large":
                                    createNewMap(3200, 3200, 32, 32, creWin.Tiles);
                                    break;
                            }
                            creWin.Mode = CRESTATE.cs_off;
                            currentMode = "tiles";
                            break;
                        case CRESTATE.cs_cancel:
                            creWin.Mode = CRESTATE.cs_off;
                            currentMode = "tiles";
                            break;
                    }
                }
            }
            else
            {
                //handle placing tiles and objects
                if (placementWin.pointInRect(new Point2D(MouseHandler.getMouseX(), MouseHandler.getMouseY())) == true)
                {
                    if (currentMode == "units")
                    {
                        if (selectedUnit != null)
                        {
                            drawCursor = false;
                            renderObject = true;
                        }
                    }
                    if (currentMode == "buildings")
                    {
                        if (selectedBuilding != null)
                        {
                            drawCursor = false;
                            renderObject = true;
                        }
                    }
                    if (currentMode == "resources")
                    {
                        if (selectedResource != null)
                        {
                            drawCursor = false;
                            renderObject = true;
                        }
                    }
                    if (MouseHandler.getLeft() == true)
                    {
                        if (!delay.Active)
                        {
                            delay.activate();
                            switch (currentMode)
                            {
                                case "tiles":
                                    //placeTile(ms, world.Winx + 32, world.Winy + 96);
                                    world.setTile(MouseHandler.getMouseX() + world.Winx - 32, MouseHandler.getMouseY() + world.Winy - 96,
                                        pallet.Stamp.sx, pallet.Stamp.sy, pallet.SourceName,
                                        pallet.Stamp.terrainValue);
                                    break;
                                case "resources":
                                    placeResource(MouseHandler.getMouseX(), MouseHandler.getMouseY());
                                    break;
                                case "units":
                                    placeUnit(MouseHandler.getMouseX(), MouseHandler.getMouseY());
                                    break;
                                case "buildings":
                                    placeBuilding(MouseHandler.getMouseX(), MouseHandler.getMouseY());
                                    break;
                            }
                        }
                    }
                    if (MouseHandler.getRight() == true)
                    {
                        if (!delay.Active)
                        {
                            delay.activate();
                            switch (currentMode)
                            {
                                case "tiles":
                                    world.eraseTile(MouseHandler.getMouseX() - 32 + world.Winx, MouseHandler.getMouseY() - 96 + world.Winy);
                                    break;
                                case "resources":
                                    eraseResource(MouseHandler.getMouseX(), MouseHandler.getMouseY());
                                    break;
                                case "units":
                                    eraseUnit(MouseHandler.getMouseX(), MouseHandler.getMouseY());
                                    break;
                                case "buildings":
                                    eraseBuilding(MouseHandler.getMouseX(), MouseHandler.getMouseY());
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    drawCursor = true;
                    renderObject = false;
                }
                //handle selecting tiles and objects to place
                if (selectionWin.pointInRect(new Point2D(MouseHandler.getMouseX(), MouseHandler.getMouseY())))
                {
                    string tmpOpt = "";
                    if (currentMode == "factions")
                    {
                        if (faction == "")
                        {
                            faction = objectList.update(MouseHandler.getLeft(), cursor.Box);
                            cmdrIndex = findCmdrIndex(faction);
                        }
                        else
                        {
                            tmpOpt = objectList.update(MouseHandler.getLeft(), cursor.Box);
                            if (tmpOpt != "-1")
                            {
                                faction = tmpOpt;
                                cmdrIndex = findCmdrIndex(tmpOpt);
                                faction = world.CMDRS[cmdrIndex].Faction;
                            }
                        }

                    }
                    else if (currentMode == "commanders")
                    {
                        if (cmdrIndex == -1)
                        {
                            cmdrIndex = findCmdrIndex(objectList.update(MouseHandler.getLeft(), cursor.Box));
                            faction = world.CMDRS[cmdrIndex].Faction;
                        }
                        else
                        {
                            int tmpIdx = findCmdrIndex(objectList.update(MouseHandler.getLeft(), cursor.Box));
                            if (tmpIdx != -1)
                            {
                                cmdrIndex = tmpIdx;
                                faction = world.CMDRS[cmdrIndex].Faction;
                            }
                        }
                    }
                    else if (currentMode == "units")
                    {
                        tmpOpt = objectList.update(MouseHandler.getLeft(), cursor.Box);
                        if (tmpOpt != "-1")
                        {
                            selectedUnit = world.CMDRS[cmdrIndex].UNITDB.getData(tmpOpt);
                        }
                    }
                    else if (currentMode == "buildings")
                    {
                        tmpOpt = objectList.update(MouseHandler.getLeft(), cursor.Box);
                        if (tmpOpt != "-1")
                        {
                            selectedBuilding = world.CMDRS[cmdrIndex].BLDDB.getData(tmpOpt);
                        }
                    }
                    else if (currentMode == "resources")
                    {
                        tmpOpt = objectList.update(MouseHandler.getLeft(), cursor.Box);
                        if (tmpOpt != "-1")
                        {
                            selectedResource = resourceDB.getData(tmpOpt);
                        }
                    }
                    else if (currentMode == "neutrals")
                    {
                        tmpOpt = objectList.update(MouseHandler.getLeft(), cursor.Box);
                        if (tmpOpt != "-1")
                        {
                            selectedAction = ACTDB.getData(tmpOpt);
                        }
                    }
                    else if (currentMode == "scripts")
                    {
                        tmpOpt = objectList.update(MouseHandler.getLeft(), cursor.Box);
                        if (tmpOpt != "-1")
                        {
                            selectedScript = scriptDB.getData(tmpOpt);
                        }
                    }
                    else if (currentMode == "tiles")
                    {
                        if (pallet.Box.pointInRect(new Point2D(MouseHandler.getMouseX(), MouseHandler.getMouseY())))
                        {
                            pallet.clicked();
                        }
                    }
                    /*
                    else if (currentMode == "new")
                    {
                        creWin.Mode = CRESTATE.cs_create;
                        currentMode = "tiles";
                    }
                    else if (currentMode == "save")
                    {
                        saveMap();
                        currentMode = "tiles";
                    }
                    else if (currentMode == "load")
                    {
                        string tmpMap = objectList.update(mbs, cursor.Box);
                        if (tmpMap != "")
                        {
                            mapName = tmpMap;
                            loadMap();
                            currentMode = "tiles";
                        }
                    }
                    */
                }

                string tmpStr = "";
                if (!dropList.Active)
                {
                    if (objectsButton.click() == "objects")
                    {
                        //update and activate drop down list for objects options
                        tmpStr = "units";
                        dropListOptions[0] = tmpStr;
                        tmpStr = "buildings";
                        dropListOptions[1] = tmpStr;
                        tmpStr = "resources";
                        dropListOptions[2] = tmpStr;
                        tmpStr = "neutrals";
                        dropListOptions[3] = tmpStr;
                        tmpStr = "tiles";
                        dropListOptions[4] = tmpStr;
                        dropList.TopLeft = new Point2D(objectsButton.X, objectsButton.Y + 32);
                        dropList.activate();
                    }
                    if (factionsButton.click() == "factions")
                    {
                        //update and activate drop down list for factions options
                        tmpStr = "factions";
                        dropListOptions[0] = tmpStr;
                        tmpStr = "commanders";
                        dropListOptions[1] = tmpStr;
                        tmpStr = "scripts";
                        dropListOptions[2] = tmpStr;
                        dropListOptions[3] = null;
                        dropListOptions[4] = null;
                        dropList.TopLeft = new Point2D(factionsButton.X, factionsButton.Y + 32);
                        dropList.activate();
                    }
                    if (filesButton.click() == "files")
                    {
                        //update and activate drop down list for files options
                        tmpStr = "new";
                        dropListOptions[0] = tmpStr;
                        tmpStr = "save";
                        dropListOptions[1] = tmpStr;
                        tmpStr = "load";
                        dropListOptions[2] = tmpStr;
                        dropListOptions[3] = null;
                        dropListOptions[4] = null;
                        dropList.TopLeft = new Point2D(filesButton.X, filesButton.Y + 32);
                        dropList.activate();
                    }
                }
                else
                {
                    tmpStr = dropList.update(dropListOptions);
                    if (tmpStr != "")
                    {
                        switch (tmpStr)
                        {
                            case "units":
                                currentMode = "units";
                                objectList.Clear();
                                for (int u = 0; u < world.CMDRS[cmdrIndex].UNITDB.Index; u++)
                                {
                                    objectList.addOption(world.CMDRS[cmdrIndex].UNITDB.at(u).Name);
                                }
                                break;
                            case "buildings":
                                currentMode = "buildings";
                                objectList.Clear();
                                for (int b = 0; b < world.CMDRS[cmdrIndex].BLDDB.Index; b++)
                                {
                                    objectList.addOption(world.CMDRS[cmdrIndex].BLDDB.at(b).Name);
                                }
                                break;
                            case "resources":
                                currentMode = "resources";
                                objectList.Clear();
                                for (int r = 0; r < resourceDB.Index; r++)
                                {
                                    objectList.addOption(resourceDB.at(r).Name);
                                }
                                break;
                            case "neutrals":
                                currentMode = "neutrals";
                                objectList.Clear();
                                for (int a = 0; a < ACTDB.Index; a++)
                                {
                                    objectList.addOption(ACTDB.at(a).Name);
                                }
                                break;
                            case "factions":
                                currentMode = "factions";
                                objectList.Clear();
                                for (int c = 0; c < CMDRDB.Index; c++)
                                {
                                    objectList.addOption(CMDRDB.at(c).Faction);
                                }
                                break;
                            case "commanders":
                                currentMode = "commanders";
                                objectList.Clear();
                                for (int c = 0; c < CMDRDB.Index; c++)
                                {
                                    objectList.addOption(CMDRDB.at(c).Faction);
                                }
                                break;
                            case "scripts":
                                currentMode = "scripts";
                                objectList.Clear();
                                for (int s = 0; s < scriptDB.Index; s++)
                                {
                                    objectList.addOption(scriptDB.at(s).Name);
                                }
                                break;
                            case "new":
                                currentMode = "new";
                                break;
                            case "save":
                                currentMode = "save";
                                break;
                            case "load":
                                currentMode = "load";
                                setLoadList();
                                break;
                            case "tiles":
                                currentMode = "tiles";
                                break;
                        }
                        dropList.Active = false;
                    }
                    if (cursor.Right)
                    {
                        dropList.Active = false;
                    }
                }
                switch (currentMode)
                {
                    case "new":
                        creWin.update(cursor);
                        break;
                    case "save":
                        saveMap();
                        currentMode = "tiles";
                        break;
                    case "load":
                        string tmpMap = objectList.update(MouseHandler.getLeft(), cursor.Box);
                        if (tmpMap != "")
                        {
                            mapName = tmpMap;
                            loadMap();
                            currentMode = "tiles";
                        }
                        break;

                }
                //user muse hover curor over fields menu to make it run update
                if (fieldsMenu.Box.pointInRect(new Point2D(MouseHandler.getMouseX(), MouseHandler.getMouseY())))
                {
                    fieldsMenu.update();
                }
            }
            //handle top button
            if (exitButton.click() == "exit")
            {
                exit = true;
            }
            //handle selecting miniMap
            if (mapWin.pointInRect(new Point2D(MouseHandler.getMouseX(), MouseHandler.getMouseY())))
            {
                miniMap.click2(MouseHandler.getMouseX(), MouseHandler.getMouseY(), world);
            }
            //scroll world view
            if (scrollUp.click() == "up")
            {
                world.shiftWindow(0, -world.TileHeight);
            }
            if (scrollRight.click() == "right")
            {
                world.shiftWindow(world.TileWidth, 0);
            }
            if (scrollDown.click() == "down")
            {
                world.shiftWindow(0, world.TileHeight);
            }
            if (scrollLeft.click() == "left")
            {
                world.shiftWindow(-world.TileWidth, 0);
            }
            delay.update();
            objectsButton.update();
            factionsButton.update();
            filesButton.update();
            scrollUp.update();
            scrollRight.update();
            scrollDown.update();
            scrollLeft.update();
        }
        /// <summary>
        /// Draws the base editor
        /// </summary>
        /// <param name="renderer">renderer reference</param>
        /// <param name="scaler">text scaling value</param>
        public virtual void draw(IntPtr renderer, float scaler = 1)
        {
            world.draw(renderer, 32, 96);
            world.drawTileNumbers(renderer, "white");
            for (int c = 0; c < world.CMDRS.Count; c++)
            {
                world.CMDRS[c].draw(renderer, world.TileHeight);
            }
            if (currentMode == "tiles")
            {
                pallet.draw(renderer, scaler);
            }
            else
            {
                objectList.drawNoButtons(renderer);//draw(sb, font);
                objectList.drawArrows(renderer);
            }
            miniMap.draw(renderer, world.CMDRS[cmdrIndex].Fog, world.Winx, world.Winy);
            scrollUp.draw(renderer);
            scrollRight.draw(renderer);
            scrollDown.draw(renderer);
            scrollLeft.draw(renderer);
            filesButton.draw(renderer);
            filesButton.darwName(renderer);
            objectsButton.draw(renderer);
            objectsButton.darwName(renderer);
            factionsButton.draw(renderer);
            factionsButton.darwName(renderer);
            exitButton.draw(renderer);
            exitButton.darwName(renderer);
            fieldsMenu.draw(renderer, scaler);
            if (dropList.Active)
            {
                dropList.draw(renderer, dropListOptions);
            }
            SimpleFont.DrawString(renderer, mapName, filesButton.X + 198, 0, scaler);
            SimpleFont.DrawString(renderer, faction, filesButton.X + 198 + 196, 0, scaler);
            SimpleFont.DrawString(renderer, currentMode, 0, 18 * 32, scaler);
            if (cmdrIndex >= 0 && cmdrIndex < world.CMDRS.Count)
            {
                SimpleFont.DrawString(renderer, world.CMDRS[cmdrIndex].Faction, (16 * 32) - 196, 18 * 32, scaler);
            }
            switch (currentMode)
            {
                case "units":
                    if (selectedUnit != null)
                    {
                        SimpleFont.DrawString(renderer, selectedUnit.Name, 0, 19 * 32, scaler);
                        selectedUnit.setPos(mx, my);
                        if (renderObject)
                        {
                            selectedUnit.draw(renderer, world.Winx, world.Winy);
                        }
                    }
                    break;
                case "buildings":
                    if (selectedBuilding != null)
                    {
                        SimpleFont.DrawString(renderer, selectedBuilding.Name, 0, 19 * 32, scaler);
                        selectedBuilding.setPos(mx, my);
                        if (renderObject)
                        {
                            selectedBuilding.draw(renderer, world.Winx, world.Winy);
                        }
                    }
                    break;
                case "resources":
                    if (selectedResource != null)
                    {
                        SimpleFont.DrawString(renderer, selectedResource.Name, 0, 19 * 32, scaler);
                        selectedResource.Position = new Point2D(mx, my);
                        if (renderObject)
                        {
                            selectedResource.draw(renderer);
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Creates a new world scene to work on provided some basic parameters 
        /// </summary>
        /// <param name="w">world width in tiles</param>
        /// <param name="h">world height in tiles</param>
        /// <param name="tw">tile width in pixels</param>
        /// <param name="th">tile height in pixels</param>
        /// <param name="tileName">name of tile sourceS</param>
        public virtual void createNewMap(int w, int h, int tw, int th, string tileName)
        {
            world = new WorldScene(w, h, tw, th, TextureBank.getTexture(tileName), tileName);
        }
        /// <summary>
        /// Place a tile from pallet stamp at mouse position
        /// </summary>
        /// <param name="ms"></param>
        public virtual void placeTile(float x, float y, int shiftx, int shifty)
        {
            TileStamp stamp = pallet.Stamp;
            world.setTile((int)((x - shiftx) / world.TileWidth) * world.TileWidth, (int)((y - shifty) / world.TileHeight) * world.TileHeight, stamp.sx, stamp.sy, world.SourceName);
        }
        /// <summary>
        /// Places a unit at loaction based on currently selected unit
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        public virtual void placeUnit(float x, float y)
        {
            if (world.getTile((int)(x + world.Winx - 32) / world.TileWidth, (int)(y + world.Winy - 96) / world.TileHeight) != null)
            {
                if (!world.getTile((int)(x + world.Winx - 32) / world.TileWidth, (int)(y + world.Winy - 96) / world.TileHeight).Occupied)
                {
                    if (selectedUnit != null)
                    {
                        RTSUnit tmpUnit = new RTSUnit(selectedUnit);
                        tmpUnit.X = x + world.Winx - 32;
                        tmpUnit.Y = y + world.Winy - 96;
                        world.CMDRS[cmdrIndex].Units.Add(tmpUnit);
                    }
                }
            }
        }
        /// <summary>
        /// Erases a unit at loaction
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        public virtual void eraseUnit(float x, float y)
        {
            for (int c = 0; c < world.CMDRS.Count; c++)
            {
                for (int u = 0; u < world.CMDRS[c].Units.Count; u++)
                {
                    if (world.CMDRS[c].Units[u].HitBox.pointInRect(new Point2D(x, y)))
                    {
                        world.CMDRS[c].Units.RemoveAt(u);
                    }
                }
            }
        }
        /// <summary>
        /// Add an action to selected commander
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        public virtual void placeAction(float x, float y)
        {
            if (selectedAction != null)
            {
                RTSAction tmpAction = new RTSAction(selectedAction);
                tmpAction.X = x + world.Winx - 32;
                tmpAction.Y = y + world.Winy - 96;
                world.CMDRS[cmdrIndex].Actions.Add(tmpAction);
            }
        }
        /// <summary>
        /// Erase an action
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        public virtual void eraseAction(float x, float y)
        {
            for (int c = 0; c < world.CMDRS.Count; c++)
            {
                for (int a = 0; a < world.CMDRS[c].Actions.Count; a++)
                {
                    if (world.CMDRS[c].Actions[a].HitBox.pointInRect(new Point2D(x, y)))
                    {
                        world.CMDRS[c].Actions.RemoveAt(a);
                    }
                }
            }
        }
        /// <summary>
        /// Places a building at loaction based on currently selected building
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        public virtual void placeBuilding(float x, float y)
        {
            if (world.checkSpace((int)x + world.Winx - 32, (int)y + world.Winy - 96, (int)selectedBuilding.W / world.TileWidth, (int)selectedBuilding.H / world.TileHeight))
            {
                if (selectedBuilding != null)
                {
                    RTSBuilding tmpBld = new RTSBuilding(selectedBuilding);
                    tmpBld.X = x + world.Winx - 32;
                    tmpBld.Y = y + world.Winy - 96;
                    world.CMDRS[cmdrIndex].Buildings.Add(tmpBld);
                }
            }
        }
        /// <summary>
        /// Erases a building at loaction
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        public virtual void eraseBuilding(float x, float y)
        {
            for (int c = 0; c < world.CMDRS.Count; c++)
            {
                for (int b = 0; b < world.CMDRS[c].Buildings.Count; b++)
                {
                    if (world.CMDRS[c].Buildings[b].HitBox.pointInRect(new Point2D(x, y)))
                    {
                        world.CMDRS[c].Buildings.RemoveAt(b);
                    }
                }
            }
        }
        /// <summary>
        /// Places a resource at loaction based on currently selected resource
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        public virtual void placeResource(float x, float y)
        {
            RTSResource tmpRsc = world.Ores.atPosition((int)(x / world.TileWidth) - 32 + world.Winx, (int)(y / world.TileHeight) - 96 + world.Winy);
            if (tmpRsc == null)
            {
                RTSResource rsc = new RTSResource(selectedResource);

                world.Ores.addResource((int)x - 32 + world.Winx, (int)y - 96 + world.Winy, rsc);
            }
        }
        /// <summary>
        /// Erases a resource at loaction
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        public virtual void eraseResource(float x, float y)
        {
            world.Ores.eraseResource((int)(x / world.TileWidth) - 32 + world.Winx, (int)(y / world.TileHeight) - 96 + world.Winy);
        }
        /// <summary>
        /// A empty virtual function to be implemented in child classes
        /// </summary>
        public virtual void buildDataBases()
        {

        }
        /// <summary>
        /// Build a current state of field menu, is a virtual function
        /// </summary>
        /// <param name="name">Name of object to add field access to</param>
        /// <param name="obj">Object reference to access fields of</param>
        public virtual void buildFieldsMenu(string name, RTSObject obj)
        {
            fieldsMenu.clear();
            //implemented in int child classes
        }
        /// <summary>
        /// Saves map data for currently active map
        /// </summary>
        public virtual void saveMap()
        {
            if (System.IO.Directory.Exists(System.IO.Directory.GetCurrentDirectory() +
                    "\\maps"))
            {
                System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() +
                    "\\maps");
            }
            System.IO.StreamWriter sw =
                new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() +
                    "\\maps\\" + mapName + ".rts");
            sw.WriteLine("======RTS Map Data======");
            sw.WriteLine(mapName);
            world.saveData(sw);

            sw.Close();
        }
        /// <summary>
        /// Loads a basic RTS map from a .rts file
        /// </summary>
        public virtual void loadMap()
        {
            if (System.IO.Directory.Exists(System.IO.Directory.GetCurrentDirectory() +
                    "\\maps"))
            {
                System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() +
                    "\\maps");
            }
            System.IO.StreamReader sr = new System.IO.StreamReader(
                System.IO.Directory.GetCurrentDirectory() + "\\maps" + mapName + ".rts");
            sr.ReadLine();
            mapName = sr.ReadLine();
            world = new WorldScene(sr);
            buildDataBases();
            sr.Close();
        }
        /// <summary>
        /// Sets the names of map files in local map folder
        /// </summary>
        public virtual void setLoadList()
        {
            char[] dot = { '.' };
            char[] slash = { '\\' };
            string[] line;
            string[] fileNames = System.IO.Directory.GetFiles(System.IO.Directory.CreateDirectory(
                System.IO.Directory.GetCurrentDirectory() + "\\maps").ToString());
            List<string> fileNamesList = fileNames.ToList();
            objectList.Clear();
            for (int f = 0; f < fileNamesList.Count; f++)
            {
                line = StringParser.parse(fileNamesList[f], dot);
                if (line[1] == "rts")
                {
                    List<string> tmpLine = StringParser.parse(line[0], slash).ToList();
                    objectList.addOption(tmpLine[tmpLine.Count - 1]);
                }
            }
        }
        /// <summary>
        /// Exit property
        /// </summary>
        public bool Exit
        {
            get { return exit; }
            set { exit = value; }
        }
        /// <summary>
        /// handles creating a new map, is a virtual function
        /// </summary>
        public virtual void createNewMap()
        {
            saveMap();
            creWin.Mode = CRESTATE.cs_running;
        }
        /// <summary>
        /// Virtual function for updating field editor
        /// </summary>
        public virtual void updateFieldEditor()
        {
            //basic layout for implemention in child class
            if (selectedAction != null)
            {
                switch (selectedAction.Name)
                {
                    case "Wormhole":
                        //((Wormhole)selectedAction).X2 = Convert.ToInt32(fieldsMenu.getValue("x"));
                        //((Wormhole)selectedAction).Y2 = Convert.ToInt32(fieldsMenu.getValue("Y"));
                        break;
                }
            }
        }
        /// <summary>
        /// Gets a commander's index based on the faction name
        /// </summary>
        /// <param name="name">Commander's faction name</param>
        /// <returns>Commander index or -1 if not found</returns>
        public int findCmdrIndex(string name)
        {
            for (int c = 0; c < world.CMDRS.Count; c++)
            {
                if (world.CMDRS[c].Faction == name)
                {
                    return c;
                }
            }
            return -1;
        }
        /// <summary>
        /// World property
        /// </summary>
        public WorldScene World
        {
            get { return world; }
            set
            {
                world = value;
                miniMap = new RTSMiniMap(5 * 32, 4 * 32, 32 + (15 * 32) + 32, 96 + (15 * 32), world, world.CMDRS[0].Fog, 32);
            }
        }
    }
}
