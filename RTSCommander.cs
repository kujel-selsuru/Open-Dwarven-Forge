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
    public enum COMMANDERTYPE { ct_human = 0, ct_ai, ct_multiplayer };
    public class RTSCommander
    {
        //protected
        protected int index;
        protected string faction;
        protected GenericBank<RTSAction> actDB;
        protected GenericBank<RTSAbility> abDB;
        protected GenericBank<RTSBuilding> bldDB;
        protected GenericBank<RTSUnit> unitDB;
        protected List<RTSEffect> effects;
        protected List<RTSBuilding> buildings;
        protected List<RTSUnit> units;
        protected List<RTSObject> selected;
        protected List<RTSUnit> squad1;
        protected List<RTSUnit> squad2;
        protected List<RTSUnit> squad3;
        protected List<RTSUnit> squad4;
        protected List<RTSUnit> squad5;
        protected List<RTSUnit> squad6;
        protected List<RTSUnit> squad7;
        protected List<RTSUnit> squad8;
        protected List<RTSUnit> squad9;
        protected List<RTSUnit> squad0;
        protected List<RTSAction> actions;
        protected List<RTSAction> recycledActions;
        protected COMMANDERTYPE ct;
        protected WorldScene world;
        protected FogOfWar fog;
        protected string resource1;
        protected int resource1Amount;
        protected string resource2;
        protected int resource2Amount;
        protected string resource3;
        protected int resource3Amount;
        protected List<string> upgrades;
        protected int unitCap;
        protected int numUnits;
        protected SimpleClock simpleClock;
        //public
        /// <summary>
        /// RTS Commander instance
        /// </summary>
        /// <param name="index">Commander index</param>
        /// <param name="world">World scene</param>
        /// <param name="fogGraphic">Name of fog graphic</param>
        public RTSCommander(int index, WorldScene world, string fogGraphic = "fog graphic")
        {
            this.index = index;
            faction = "Untitled Faction";
            buildings = new List<RTSBuilding>();
            units = new List<RTSUnit>();
            actions = new List<RTSAction>();
            ct = COMMANDERTYPE.ct_human;
            this.world = world;
            actDB = new GenericBank<RTSAction>();
            abDB = new GenericBank<RTSAbility>();
            bldDB = new GenericBank<RTSBuilding>();
            unitDB = new GenericBank<RTSUnit>();
            selected = new List<RTSObject>();
            squad1 = new List<RTSUnit>();
            squad2 = new List<RTSUnit>();
            squad3 = new List<RTSUnit>();
            squad4 = new List<RTSUnit>();
            squad5 = new List<RTSUnit>();
            squad6 = new List<RTSUnit>();
            squad7 = new List<RTSUnit>();
            squad8 = new List<RTSUnit>();
            squad9 = new List<RTSUnit>();
            squad0 = new List<RTSUnit>();
            fog = new FogOfWar(world.WorldWidth, world.WorldHeight, world.TileWidth, world.TileHeight,
                TextureBank.getTexture(fogGraphic), fogGraphic);
            resource1 = "";
            resource1Amount = 0;
            resource2 = "";
            resource2Amount = 0;
            resource3 = "";
            resource3Amount = 0;
            upgrades = new List<string>();
            unitCap = 100;
            numUnits = 0;
            recycledActions = new List<RTSAction>();
            simpleClock = new SimpleClock(60);
            effects = new List<RTSEffect>();
        }
        /// <summary>
        /// RTSCommander copy constructor
        /// </summary>
        /// <param name="obj">RTSCommander instance to copy</param>
        public RTSCommander(RTSCommander obj)
        {
            index = obj.Index;
            faction = obj.Faction;
            for (int b = 0; b < obj.Buildings.Count; b++)
            {
                buildings.Add(new RTSBuilding(obj.Buildings[b]));
            }
            for (int u = 0; u < obj.Units.Count; u++)
            {
                units.Add(new RTSUnit(obj.Units[u]));
            }
            for (int a = 0; a < obj.Actions.Count; a++)
            {
                actions.Add(new RTSAction(obj.Actions[a]));
            }
            ct = obj.CT;
            world = obj.world;
            actDB = new GenericBank<RTSAction>();
            abDB = new GenericBank<RTSAbility>();
            bldDB = new GenericBank<RTSBuilding>();
            unitDB = new GenericBank<RTSUnit>();
            for (int a = 0; a < obj.ACTDB.Index; a++)
            {
                actDB.addData(obj.ACTDB.Names[a], new RTSAction(obj.ACTDB.at(a)));
            }
            for (int ab = 0; ab < obj.ABDB.Index; ab++)
            {
                abDB.addData(obj.ABDB.Names[ab], new RTSAbility(obj.ABDB.at(ab)));
            }
            for (int b = 0; b < obj.BLDDB.Index; b++)
            {
                bldDB.addData(obj.BLDDB.Names[b], new RTSBuilding(obj.BLDDB.at(b)));
            }
            for (int u = 0; u < obj.UNITDB.Index; u++)
            {
                unitDB.addData(obj.UNITDB.Names[u], new RTSUnit(obj.UNITDB.at(u)));
            }
            selected = new List<RTSObject>();
            squad1 = new List<RTSUnit>();
            squad2 = new List<RTSUnit>();
            squad3 = new List<RTSUnit>();
            squad4 = new List<RTSUnit>();
            squad5 = new List<RTSUnit>();
            squad6 = new List<RTSUnit>();
            squad7 = new List<RTSUnit>();
            squad8 = new List<RTSUnit>();
            squad9 = new List<RTSUnit>();
            squad0 = new List<RTSUnit>();
            fog = obj.Fog;
            resource1 = obj.Resource1;
            resource1Amount = obj.Resource1Amount;
            resource2 = obj.Resource2;
            resource2Amount = obj.Resource2Amount;
            resource3 = obj.Resource3;
            resource3Amount = obj.Resource3Amount;
            for (int n = 0; n < obj.Upgrades.Count; n++)
            {
                upgrades.Add(new string(obj.Upgrades[n].ToCharArray()));
            }
            unitCap = obj.UnitCap;
            numUnits = obj.NumUnits;
            recycledActions = new List<RTSAction>();
            simpleClock = new SimpleClock(obj.SC);
            for (int n = 0; n < obj.Effects.Count; n++)
            {
                effects.Add(new RTSEffect(obj.Effects[n]));
            }
        }
        /// <summary>
        /// RTSCommander constructor from file
        /// </summary>
        /// <param name="sr">Stream reader reference</param>
        /// <param name="gb">Graphics bank</param>
        public RTSCommander(System.IO.StreamReader sr, WorldScene world)
        {
            this.world = world;
            buildings = new List<RTSBuilding>();
            units = new List<RTSUnit>();
            actions = new List<RTSAction>();
            actDB = new GenericBank<RTSAction>();
            abDB = new GenericBank<RTSAbility>();
            bldDB = new GenericBank<RTSBuilding>();
            unitDB = new GenericBank<RTSUnit>();
            selected = new List<RTSObject>();
            squad1 = new List<RTSUnit>();
            squad2 = new List<RTSUnit>();
            squad3 = new List<RTSUnit>();
            squad4 = new List<RTSUnit>();
            squad5 = new List<RTSUnit>();
            squad6 = new List<RTSUnit>();
            squad7 = new List<RTSUnit>();
            squad8 = new List<RTSUnit>();
            squad9 = new List<RTSUnit>();
            squad0 = new List<RTSUnit>();
            sr.ReadLine();
            index = Convert.ToInt32(sr.ReadLine());
            faction = sr.ReadLine();
            string buffer = "";
            int num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                RTSBuilding bld = new RTSBuilding(sr);
                bld.Owner = this;
                buildings.Add(bld);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                buffer = sr.ReadLine();
                RTSBuilding bld = new RTSBuilding(sr);
                bld.Owner = this;
                bldDB.addData(buffer, bld);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                RTSUnit unit = new RTSUnit(sr);
                unit.Owner = this;
                units.Add(unit);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                buffer = sr.ReadLine();
                RTSUnit unit = new RTSUnit(sr);
                unit.Owner = this;
                unitDB.addData(buffer, unit);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                RTSAction act = new RTSAction(sr);
                act.Owner = this;
                actions.Add(act);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                buffer = sr.ReadLine();
                RTSAction act = new RTSAction(sr);
                act.Owner = this;
                actDB.addData(buffer, act);
            }
            ct = (COMMANDERTYPE)(Convert.ToInt32(sr.ReadLine()));
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad1.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad2.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad3.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad4.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad5.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad6.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad7.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad8.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad9.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad0.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            fog = new FogOfWar(sr);
            resource1 = sr.ReadLine();
            resource1Amount = Convert.ToInt32(sr.ReadLine());
            resource2 = sr.ReadLine();
            resource2Amount = Convert.ToInt32(sr.ReadLine());
            resource3 = sr.ReadLine();
            resource3Amount = Convert.ToInt32(sr.ReadLine());
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                upgrades.Add(new string(sr.ReadLine().ToCharArray()));
            }
            unitCap = Convert.ToInt32(sr.ReadLine());
            numUnits = units.Count;
            recycledActions = new List<RTSAction>();
            simpleClock = new SimpleClock(sr);
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                effects.Add(new RTSEffect(sr));
            }
        }
        /// <summary>
        /// Saves commander data
        /// </summary>
        /// <param name="sw">Stream writer reference</param>
        public virtual void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======RTSCommander Data======");
            sw.WriteLine(index);
            sw.WriteLine(faction);
            sw.WriteLine(buildings.Count);
            for (int b = 0; b < buildings.Count; b++)
            {
                buildings[b].saveData(sw);
            }
            sw.WriteLine(bldDB.Index);
            for (int b = 0; b < bldDB.Index; b++)
            {
                sw.WriteLine(bldDB.Names[b]);
                bldDB.at(b).saveData(sw);
            }
            sw.WriteLine(units.Count);
            for (int u = 0; u < units.Count; u++)
            {
                units[u].saveData(sw);
            }
            sw.WriteLine(unitDB.Index);
            for (int u = 0; u < unitDB.Index; u++)
            {
                sw.WriteLine(unitDB.Names[u]);
                unitDB.at(u).saveData(sw);
            }
            sw.WriteLine(actions.Count);
            for (int a = 0; a < actions.Count; a++)
            {
                actions[a].saveData(sw);
            }
            sw.WriteLine(actDB.Index);
            for (int a = 0; a < actDB.Index; a++)
            {
                sw.WriteLine(actDB.Names[a]);
                actDB.at(a).saveData(sw);
            }
            sw.WriteLine((int)ct);
            List<int> squadIndexies = new List<int>();
            for (int s = 0; s < squad1.Count; s++)
            {
                for (int u = 0; u < units.Count; u++)
                {
                    if (squad1[s].X == units[u].X)
                    {
                        if (squad1[s].Y == units[u].Y)
                        {
                            squadIndexies.Add(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine(squad1.Count);
            for (int i = 0; i < squadIndexies.Count; i++)
            {
                sw.WriteLine(squadIndexies[i]);
            }
            squadIndexies.Clear();
            for (int s = 0; s < squad2.Count; s++)
            {
                for (int u = 0; u < units.Count; u++)
                {
                    if (squad2[s].X == units[u].X)
                    {
                        if (squad2[s].Y == units[u].Y)
                        {
                            squadIndexies.Add(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine(squad2.Count);
            for (int i = 0; i < squadIndexies.Count; i++)
            {
                sw.WriteLine(squadIndexies[i]);
            }
            squadIndexies.Clear();
            for (int s = 0; s < squad3.Count; s++)
            {
                for (int u = 0; u < units.Count; u++)
                {
                    if (squad3[s].X == units[u].X)
                    {
                        if (squad3[s].Y == units[u].Y)
                        {
                            squadIndexies.Add(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine(squad3.Count);
            for (int i = 0; i < squadIndexies.Count; i++)
            {
                sw.WriteLine(squadIndexies[i]);
            }
            squadIndexies.Clear();
            for (int s = 0; s < squad4.Count; s++)
            {
                for (int u = 0; u < units.Count; u++)
                {
                    if (squad4[s].X == units[u].X)
                    {
                        if (squad4[s].Y == units[u].Y)
                        {
                            squadIndexies.Add(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine(squad4.Count);
            for (int i = 0; i < squadIndexies.Count; i++)
            {
                sw.WriteLine(squadIndexies[i]);
            }
            squadIndexies.Clear();
            for (int s = 0; s < squad5.Count; s++)
            {
                for (int u = 0; u < units.Count; u++)
                {
                    if (squad5[s].X == units[u].X)
                    {
                        if (squad5[s].Y == units[u].Y)
                        {
                            squadIndexies.Add(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine(squad5.Count);
            for (int i = 0; i < squadIndexies.Count; i++)
            {
                sw.WriteLine(squadIndexies[i]);
            }
            squadIndexies.Clear();
            for (int s = 0; s < squad6.Count; s++)
            {
                for (int u = 0; u < units.Count; u++)
                {
                    if (squad6[s].X == units[u].X)
                    {
                        if (squad6[s].Y == units[u].Y)
                        {
                            squadIndexies.Add(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine(squad6.Count);
            for (int i = 0; i < squadIndexies.Count; i++)
            {
                sw.WriteLine(squadIndexies[i]);
            }
            squadIndexies.Clear();
            for (int s = 0; s < squad7.Count; s++)
            {
                for (int u = 0; u < units.Count; u++)
                {
                    if (squad7[s].X == units[u].X)
                    {
                        if (squad7[s].Y == units[u].Y)
                        {
                            squadIndexies.Add(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine(squad7.Count);
            for (int i = 0; i < squadIndexies.Count; i++)
            {
                sw.WriteLine(squadIndexies[i]);
            }
            squadIndexies.Clear();
            for (int s = 0; s < squad8.Count; s++)
            {
                for (int u = 0; u < units.Count; u++)
                {
                    if (squad8[s].X == units[u].X)
                    {
                        if (squad8[s].Y == units[u].Y)
                        {
                            squadIndexies.Add(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine(squad8.Count);
            for (int i = 0; i < squadIndexies.Count; i++)
            {
                sw.WriteLine(squadIndexies[i]);
            }
            squadIndexies.Clear();
            for (int s = 0; s < squad9.Count; s++)
            {
                for (int u = 0; u < units.Count; u++)
                {
                    if (squad9[s].X == units[u].X)
                    {
                        if (squad9[s].Y == units[u].Y)
                        {
                            squadIndexies.Add(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine(squad9.Count);
            for (int i = 0; i < squadIndexies.Count; i++)
            {
                sw.WriteLine(squadIndexies[i]);
            }
            squadIndexies.Clear();
            for (int s = 0; s < squad0.Count; s++)
            {
                for (int u = 0; u < units.Count; u++)
                {
                    if (squad0[s].X == units[u].X)
                    {
                        if (squad0[s].Y == units[u].Y)
                        {
                            squadIndexies.Add(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine(squad0.Count);
            for (int i = 0; i < squadIndexies.Count; i++)
            {
                sw.WriteLine(squadIndexies[i]);
            }
            squadIndexies.Clear();
            sw.WriteLine(resource1);
            sw.WriteLine(resource1Amount);
            sw.WriteLine(resource2);
            sw.WriteLine(resource2Amount);
            sw.WriteLine(resource3);
            sw.WriteLine(resource3Amount);
            sw.WriteLine(upgrades.Count);
            for (int n = 0; n < upgrades.Count; n++)
            {
                sw.WriteLine(upgrades[n]);
            }
            sw.WriteLine(unitCap);
            simpleClock.saveData(sw);
            sw.WriteLine(effects.Count);
            for (int e = 0; e < effects.Count; e++)
            {
                effects[e].saveData(sw);
            }
        }
        /// <summary>
        /// Loads instance data from stream reader reference
        /// </summary>
        /// <param name="sr">Stream reader reference</param>
        /// <param name="gb">Graphics bank reference</param>
        /// <param name="world">World scene reference</param>
        public virtual void loadData(System.IO.StreamReader sr, WorldScene world)
        {
            this.world = world;
            sr.ReadLine();
            index = Convert.ToInt32(sr.ReadLine());
            faction = sr.ReadLine();
            string buffer = "";
            int num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                RTSBuilding bld = new RTSBuilding(sr);
                //bld.loadData(sr);
                bld.Owner = this;
                buildings.Add(bld);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                buffer = sr.ReadLine();
                RTSBuilding bld = new RTSBuilding(sr);
                //bld.loadData(sr);
                bld.Owner = this;
                bldDB.addData(buffer, bld);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                RTSUnit unit = new RTSUnit(sr);
                //unit.loadData(sr);
                unit.Owner = this;
                units.Add(unit);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                buffer = sr.ReadLine();
                RTSUnit unit = new RTSUnit(sr);
                //unit.loadData(sr);
                unit.Owner = this;
                unitDB.addData(buffer, unit);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                RTSAction act = new RTSAction(sr);
                //act.loadData(sr);
                act.Owner = this;
                actions.Add(act);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                buffer = sr.ReadLine();
                RTSAction act = new RTSAction(sr);
                //act.loadData(sr);
                act.Owner = this;
                actDB.addData(buffer, act);
            }
            ct = (COMMANDERTYPE)(Convert.ToInt32(sr.ReadLine()));
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad1.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad2.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad3.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad4.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad5.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad6.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad7.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad8.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad9.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int n = 0; n < num; n++)
            {
                squad0.Add(units[Convert.ToInt32(sr.ReadLine())]);
            }
            resource1 = sr.ReadLine();
            resource1Amount = Convert.ToInt32(sr.ReadLine());
            resource2 = sr.ReadLine();
            resource2Amount = Convert.ToInt32(sr.ReadLine());
            resource3 = sr.ReadLine();
            resource3Amount = Convert.ToInt32(sr.ReadLine());
            unitCap = Convert.ToInt32(sr.ReadLine());
            numUnits = units.Count;
            simpleClock = new SimpleClock(sr);
            num = Convert.ToInt32(sr.ReadLine());
            for (int e = 0; e < num; e++)
            {
                effects.Add(new RTSEffect(sr));
            }
        }
        /// <summary>
        /// Updates internal state of commander and all objects it posses 
        /// </summary>
        public virtual void update()
        {
            for (int a = 0; a < actions.Count; a++)
            {
                actions[a].update();
                if (actions[a].Dead)
                {
                    cullDeadAction(a);
                }
            }
            for (int u = 0; u < units.Count; u++)
            {
                units[u].update();
                if (units[u].Dead)
                {
                    units.RemoveAt(u);
                }
            }
            for (int b = 0; b < buildings.Count; b++)
            {
                buildings[b].update();
                if (buildings[b].Dead)
                {
                    buildings.RemoveAt(b);
                }
            }
            for (int e = 0; e < effects.Count; e++)
            {
                effects[e].update();
                if (effects[e].Dead)
                {
                    effects.RemoveAt(e);
                }
            }
            simpleClock.update();
        }
        /// <summary>
        /// Draws elements of the commander
        /// </summary>
        /// <param name="renderer">renderer reference</param>
        /// <param name="scaleValue">Tile hieght for scaling value</param>
        public virtual void draw(IntPtr renderer, int scaleValue = 32,
            int shiftx = 40, int shifty = 104)
        {
            for (int b = 0; b < buildings.Count; b++)
            {
                if (world.onScreen((int)buildings[b].X, (int)buildings[b].Y))
                {
                    //buildings[b].Depth = -((int)world.Winy / scaleValue) - ((int)buildings[b].Y / scaleValue);
                    buildings[b].draw(renderer, world.Winx - shiftx, world.Winy - shifty);
                }
            }
            for (int u = 0; u < units.Count; u++)
            {
                if (world.onScreen((int)units[u].X, (int)units[u].Y))
                {
                    //units[u].Depth = ((int)world.Winy / scaleValue) - ((int)units[u].Y / scaleValue);
                    units[u].draw(renderer, world.Winx - shiftx, world.Winy - shifty);
                }
            }
            for (int a = 0; a < actions.Count; a++)
            {
                if (world.onScreen((int)actions[a].X, (int)actions[a].Y))
                {
                    actions[a].draw(renderer, world.Winx - shiftx, world.Winy - shifty);
                }
            }
            for (int e = 0; e < effects.Count; e++)
            {
                if (world.onScreen((int)effects[e].X, (int)effects[e].Y))
                {
                    effects[e].draw(renderer, world.Winx - shiftx, world.Winy - shifty);
                }
            }
        }
        /// <summary>
        /// Draw the commander's clock
        /// </summary>
        /// <param name="renderer">renderer reference</param>
        /// <param name="colour">Colour of clock output drawn</param>
        /// <param name="scaler">text scaling value</param>
        public virtual void drawClock(IntPtr renderer, string colour, float scaler = 1)
        {
            simpleClock.draw(64, 32, colour, renderer, scaler);
        }
        /// <summary>
        /// Handle enemy actions
        /// </summary>
        /// <param name="cmdrs">List of all commanders</param>
        public void handleEnemyActions(List<RTSCommander> cmdrs)
        {
            for (int i = 0; i < cmdrs.Count; i++)
            {
                if (cmdrs[i].Index != index)
                {
                    for (int a = 0; a < cmdrs[i].Actions.Count; a++)
                    {
                        for (int u = 0; u < units.Count; u++)
                        {
                            if (units[u].Sector == cmdrs[i].Actions[a].Sector)
                            {
                                if (units[u].HitBox.intersects(cmdrs[i].Actions[a].HitBox))
                                {
                                    units[u].strike(cmdrs[i].Actions[a].Damage);
                                }
                            }
                        }
                        for (int b = 0; b < buildings.Count; b++)
                        {
                            if (buildings[b].Sector == cmdrs[i].Actions[a].Sector)
                            {
                                if (buildings[b].HitBox.intersects(cmdrs[i].Actions[a].HitBox))
                                {
                                    buildings[b].strike(cmdrs[i].Actions[a].Damage);
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Issues a move command to all currently selected units
        /// </summary>
        /// <param name="x">target x position</param>
        /// <param name="y">target y position</param>
        public virtual void sendSelected(int x, int y)
        {
            for (int s = 0; s < selected.Count; s++)
            {
                selected[s].Objective = new RTSMoveToTarget(x, y, selected[s], null);
            }
        }
        /// <summary>
        /// Command selected units to attack target RTS object
        /// </summary>
        /// <param name="obj">RTSObject to attack</param>
        public virtual void selectedToAttack(RTSObject obj)
        {
            for (int s = 0; s < selected.Count; s++)
            {
                switch (((RTSUnit)obj).UT)
                {
                    case UNITTYPE.ut_gndHarvester:
                    case UNITTYPE.ut_gndHero:
                    case UNITTYPE.ut_gndMedic:
                    case UNITTYPE.ut_gndMiner:
                    case UNITTYPE.ut_gndSpecial:
                    case UNITTYPE.ut_gndWorker:
                    case UNITTYPE.ut_ground:
                        if (selected[s].canStrikeGround())
                        {
                            selected[s].callAttack(obj);
                        }
                        break;
                    case UNITTYPE.ut_airHarvester:
                    case UNITTYPE.ut_airHero:
                    case UNITTYPE.ut_airMedic:
                    case UNITTYPE.ut_airMiner:
                    case UNITTYPE.ut_airSpecial:
                    case UNITTYPE.ut_airWorker:
                    case UNITTYPE.ut_air:
                        if (selected[s].canStrikeAir())
                        {
                            selected[s].callAttack(obj);
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Attempts to call abilities upon selected units or buildings
        /// </summary>
        /// <param name="abilityName">Name of ability to be called on selected</param>
        public virtual void callAbility(string abilityName)
        {
            for (int s = 0; s < selected.Count; s++)
            {
                selected[s].callAbility(abilityName);
            }
        }
        /// <summary>
        /// Add a new action to the actions list
        /// </summary>
        /// <param name="name">Name of action to add</param>
        /// <param name="x">X position to add at</param>
        /// <param name="y">Y position to add at</param>
        /// <param name="rotation">Starting rotation</param>
        public virtual void addAction(string name, float x, float y, float rotation)
        {
            RTSAction act = actDB.getData(name);
            RTSAction act2 = null;
            if (act != null)
            {
                if (recycledActions.Count > 0)
                {
                    act2 = recycledActions[0];
                    recycledActions.RemoveAt(0);
                    act2.reload(name, x, y, actDB);
                }
                else
                {
                    act2 = new RTSAction(act);
                    act2.X = x;
                    act2.Y = y;
                }
                act2.SelfAngle = rotation;
                actions.Add(act2);
            }
        }
        /// <summary>
        /// Recycles an action at specified index
        /// </summary>
        /// <param name="i">Action's index in actions list</param>
        public virtual void cullDeadAction(int i)
        {
            recycledActions.Add(actions[i]);
            actions.RemoveAt(i);
        }
        /// <summary>
        /// Adds a unit from the database 
        /// </summary>
        /// <param name="name">Name of unit to add</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="rotation">Rotation angle at creation</param>
        public virtual void addUnit(string name, float x, float y, float rotation)
        {
            RTSUnit unit = unitDB.getData(name);
            if (unit != null)
            {
                if (!world.getTile((int)x, (int)y).Occupied)
                {
                    unit.X = x;
                    unit.Y = y;
                    unit.SelfAngle = rotation;
                    unit.Owner = this;
                    units.Add(unit);
                    numUnits = units.Count;
                }
            }
        }
        /// <summary>
        /// Adds a building from the database
        /// </summary>
        /// <param name="name">Name of the building to add</param>
        /// <param name="x">X poistion</param>
        /// <param name="y">Y position</param>
        public virtual void addBuilding(string name, float x, float y, GenericBank<Texture2D> gb)
        {
            RTSBuilding bld = bldDB.getData(name);
            if (bld != null)
            {
                int col = (int)(bld.W / world.TileWidth);
                int row = (int)(bld.H / world.TileHeight);
                for (int columns = ((int)x / world.TileWidth); columns < ((int)x / world.TileWidth) + col; col++)
                {
                    for (int rows = ((int)y / world.TileHeight); rows < ((int)y / world.TileHeight) + row; rows++)
                    {
                        if (world.inDomain(columns * (int)world.TileWidth, rows * (int)world.TileHeight))
                        {
                            if (world.Ores.atPosition(columns * (int)world.TileWidth, rows * (int)world.TileHeight) != null)
                            {
                                //resource grid blocks
                                return;
                            }
                            if (world.getTile(columns * (int)world.TileWidth, rows * (int)world.TileHeight).Occupied)
                            {
                                //space occupied
                                return;
                            }
                        }
                    }
                }
                bld.X = x;
                bld.Y = y;
                bld.Owner = this;
                buildings.Add(bld);
            }
        }
        /// <summary>
        /// Selects one unit or building if at target position
        /// </summary>
        /// <param name="x">X positioin</param>
        /// <param name="y">Y position</param>
        public virtual void selectOne(int x, int y)
        {
            selected.Clear();
            for (int u = 0; u < units.Count; u++)
            {
                if (units[u].HitBox.pointInRect(new Point2D(x, y)))
                {
                    selected.Add(units[u]);
                    return;
                }
            }
            for (int b = 0; b < buildings.Count; b++)
            {
                if (buildings[b].HitBox.pointInRect(new Point2D(x, y)))
                {
                    selected.Add(buildings[b]);
                    return;
                }
            }
        }
        /// <summary>
        /// Returns a reference to a unit or building if at position specified or null if none there
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>RTSObject or null</returns>
        public virtual RTSObject getObjectAtPoint(int x, int y)
        {
            for (int u = 0; u < units.Count; u++)
            {
                if (units[u].HitBox.pointInRect(new Point2D(x, y)))
                {
                    return units[u];
                }
            }
            for (int b = 0; b < buildings.Count; b++)
            {
                if (buildings[b].HitBox.pointInRect(new Point2D(x, y)))
                {
                    return buildings[b];
                }
            }
            return null;
        }
        /// <summary>
        /// Unlocks an ability provided a name
        /// </summary>
        /// <param name="name">Name of ability to unlock</param>
        public void unlockAbility(string name)
        {
            for (int ab = 0; ab < abDB.Index; ab++)
            {
                if (abDB.at(ab).Name == name)
                {
                    if (abDB.at(ab).Locked)
                    {
                        abDB.at(ab).toggleLock();
                    }
                    break;
                }
            }
            for (int u = 0; u < units.Count; u++)
            {
                for (int a = 0; a < units[u].Abilities.Count; a++)
                {
                    if (units[u].Abilities[a].Name == name)
                    {
                        if (units[u].Abilities[a].Locked)
                        {
                            units[u].Abilities[a].toggleLock();
                        }
                    }
                }
            }
            for (int b = 0; b < buildings.Count; b++)
            {
                for (int a = 0; a < buildings[b].Abilities.Count; a++)
                {
                    if (buildings[b].Abilities[a].Name == name)
                    {
                        if (buildings[b].Abilities[a].Locked)
                        {
                            buildings[b].Abilities[a].toggleLock();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Locks an ability provided a name
        /// </summary>
        /// <param name="name">Name of ability to unlock</param>
        public void lockAbility(string name)
        {
            for (int ab = 0; ab < abDB.Index; ab++)
            {
                if (abDB.at(ab).Name == name)
                {
                    if (!abDB.at(ab).Locked)
                    {
                        abDB.at(ab).toggleLock();
                    }
                    break;
                }
            }
            for (int u = 0; u < units.Count; u++)
            {
                for (int a = 0; a < units[u].Abilities.Count; a++)
                {
                    if (units[u].Abilities[a].Name == name)
                    {
                        if (!units[u].Abilities[a].Locked)
                        {
                            units[u].Abilities[a].toggleLock();
                        }
                    }
                }
            }
            for (int b = 0; b < buildings.Count; b++)
            {
                for (int a = 0; a < buildings[b].Abilities.Count; a++)
                {
                    if (buildings[b].Abilities[a].Name == name)
                    {
                        if (!buildings[b].Abilities[a].Locked)
                        {
                            buildings[b].Abilities[a].toggleLock();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Removes a specified ability from all units and buildings
        /// </summary>
        /// <param name="name">Name of ability to remove</param>
        public void removeAbility(string name)
        {
            for (int u = 0; u < units.Count; u++)
            {
                for (int a = 0; a < units[u].Abilities.Count; a++)
                {
                    if (units[u].Abilities[a].Name == name)
                    {
                        if (!units[u].Abilities[a].Locked)
                        {
                            if (!upgrades.Contains(units[u].Abilities[a].Name))
                            {
                                upgrades.Add(units[u].Abilities[a].Name);
                            }
                            units[u].Abilities.RemoveAt(a);
                        }
                    }
                }
            }
            for (int b = 0; b < buildings.Count; b++)
            {
                for (int a = 0; a < buildings[b].Abilities.Count; a++)
                {
                    if (buildings[b].Abilities[a].Name == name)
                    {
                        if (!buildings[b].Abilities[a].Locked)
                        {
                            if (!upgrades.Contains(buildings[b].Abilities[a].Name))
                            {
                                upgrades.Add(buildings[b].Abilities[a].Name);
                            }
                            buildings[b].Abilities.RemoveAt(a);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// removes upgrade abilites from objects if already competed
        /// </summary>
        public void pruneAbilties()
        {
            for (int u = 0; u < units.Count; u++)
            {
                for (int a = 0; a < units[u].Abilities.Count; a++)
                {
                    for (int n = 0; n < upgrades.Count; n++)
                    {
                        if (units[u].Abilities[a].Name == upgrades[n])
                        {
                            units[u].Abilities.RemoveAt(a);
                        }
                    }
                }
            }
            for (int b = 0; b < buildings.Count; b++)
            {
                for (int a = 0; a < buildings[b].Abilities.Count; a++)
                {
                    for (int n = 0; n < upgrades.Count; n++)
                    {
                        if (buildings[b].Abilities[a].Name == upgrades[n])
                        {
                            buildings[b].Abilities.RemoveAt(a);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Selects a single unit at coordenates
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public void select(float x, float y)
        {
            selected.Clear();
            for (int u = 0; u < units.Count; u++)
            {
                if (units[u].HitBox.pointInRect(new Point2D(x, y)))
                {
                    selected.Add(units[u]);
                }
            }

        }
        /// <summary>
        /// Selects all units of specified name on screen 
        /// </summary>
        /// <param name="name">Name of unit type to select</param>
        public void selectGroup(string name)
        {
            selected.Clear();
            for (int u = 0; u < units.Count; u++)
            {
                if (units[u].Name == name)
                {
                    if (world.onScreen((int)units[u].Center.X, (int)units[u].Center.Y))
                    {
                        selected.Add(units[u]);
                    }
                }
            }
        }
        /// <summary>
        /// Count the number of instances of units provided a name
        /// </summary>
        /// <param name="name">Name of unit to count</param>
        /// <returns>Number of instances of unit to count</returns>
        public int countUnits(string name)
        {
            int tmp = 0;
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Name == name)
                {
                    tmp++;
                }
            }
            return tmp;
        }
        /// <summary>
        /// Count the number of instances of buildings provided a name
        /// </summary>
        /// <param name="name">Name of building to count</param>
        /// <returns>Number of instances of building to count</returns>
        public int countBuildings(string name)
        {
            int tmp = 0;
            for (int i = 0; i < buildings.Count; i++)
            {
                if (buildings[i].Name == name)
                {
                    tmp++;
                }
            }
            return tmp;
        }
        /// <summary>
        /// Selects units in provided box dimensions
        /// </summary>
        /// <param name="box">Selection box defined before calling</param>
        public void selectBox(Rectangle box)
        {
            selected.Clear();
            for (int u = 0; u < units.Count; u++)
            {
                if (units[u].HitBox.intersects(box))
                {
                    selected.Add(units[u]);
                }
            }
        }
        /// <summary>
        /// Fills all commander's fog
        /// </summary>
        public void fillFog()
        {
            fog.fillFog();
        }
        /// <summary>
        /// Clears all commander's fog
        /// </summary>
        public void clearAllFog()
        {
            fog.clearAllFog();
        }
        /// <summary>
        /// Index property
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        /// <summary>
        /// Faction property
        /// </summary>
        public string Faction
        {
            get { return faction; }
            set { faction = value; }
        }
        /// <summary>
        /// Buildings property
        /// </summary>
        public List<RTSBuilding> Buildings
        {
            get { return buildings; }
        }
        /// <summary>
        /// Units property
        /// </summary>
        public List<RTSUnit> Units
        {
            get { return units; }
        }
        /// <summary>
        /// Actions property
        /// </summary>
        public List<RTSAction> Actions
        {
            get { return actions; }
        }
        /// <summary>
        /// Commander type property
        /// </summary>
        public COMMANDERTYPE CT
        {
            get { return ct; }
            set { ct = value; }
        }
        /// <summary>
        /// World property
        /// </summary>
        public WorldScene World
        {
            get { return world; }
            set { world = value; }
        }
        /// <summary>
        /// Action database property
        /// </summary>
        public GenericBank<RTSAction> ACTDB
        {
            get { return actDB; }
        }
        /// <summary>
        /// Ability database property
        /// </summary>
        public GenericBank<RTSAbility> ABDB
        {
            get { return abDB; }
        }
        /// <summary>
        /// Building database property
        /// </summary>
        public GenericBank<RTSBuilding> BLDDB
        {
            get { return bldDB; }
        }
        /// <summary>
        /// Unit database property
        /// </summary>
        public GenericBank<RTSUnit> UNITDB
        {
            get { return unitDB; }
        }
        /// <summary>
        /// Effects property
        /// </summary>
        public List<RTSEffect> Effects
        {
            get { return effects; }
        }
        /// <summary>
        /// Selected list property
        /// </summary>
        public List<RTSObject> Selected
        {
            get { return selected; }
        }
        /// <summary>
        /// Fog of war property
        /// </summary>
        public FogOfWar Fog
        {
            get { return fog; }
            set { fog = value; }
        }
        /// <summary>
        /// Resource 1 property
        /// </summary>
        public string Resource1
        {
            get { return resource1; }
            set { resource1 = value; }
        }
        /// <summary>
        /// Resource 1 amount property
        /// </summary>
        public int Resource1Amount
        {
            get { return resource1Amount; }
            set { resource1Amount = value; }
        }
        /// <summary>
        /// Resource 2 property
        /// </summary>
        public string Resource2
        {
            get { return resource2; }
            set { resource2 = value; }
        }
        /// <summary>
        /// Resource 2 amount property
        /// </summary>
        public int Resource2Amount
        {
            get { return resource2Amount; }
            set { resource2Amount = value; }
        }
        /// <summary>
        /// Resource 3 property
        /// </summary>
        public string Resource3
        {
            get { return resource3; }
            set { resource3 = value; }
        }
        /// <summary>
        /// Resource 1 amount property
        /// </summary>
        public int Resource3Amount
        {
            get { return resource3Amount; }
            set { resource3Amount = value; }
        }
        /// <summary>
        /// Upgrades proerty
        /// </summary>
        public List<string> Upgrades
        {
            get { return upgrades; }
        }
        /// <summary>
        /// Unit cap property
        /// </summary>
        public int UnitCap
        {
            get { return unitCap; }
            set { unitCap = value; }
        }
        /// <summary>
        /// Num units property
        /// </summary>
        public int NumUnits
        {
            get { return numUnits; }
            set { numUnits = value; }
        }
        /// <summary>
        /// Simple Clock property
        /// </summary>
        public SimpleClock SC
        {
            get { return simpleClock; }
            set { simpleClock = value; }
        }
        /// <summary>
        /// Pure virtual interface for commanders to load faction specific databases
        /// <param name="gb">Graphics bank reference</param>
        /// </summary>
        public virtual void loadDataBases(GenericBank<Texture2D> gb)
        {
            //pure virtual
        }
    }
}
