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
using SDL2;
using XenoLib;

namespace XenoLib
{
    public enum GAMESTATUS { VICTORY = 0, DEFEAT, INPROGRESS };
    public class SCRTSCommander
    {
        //protected
        protected GenericBank<SCRTSUnit> unitDB;
        protected GenericBank<SCRTSBuilding> buildingDB;
        protected GenericBank<SCRTSAction> actionDB;
        protected GenericBank<SCRTSStatus> statusDB;
        protected GenericBank<SCRTSAbility> abilityDB;
        protected GenericBank<SCRTSUpgrade> upgradeDB;
        protected GenericBank<SCRTSParticle> particleDB;
        protected GenericBank<SCRTSTurret> turretDB;
        protected GenericBank<SCRTSBuff> buffDB;
        protected List<XenoSprite> units;
        protected List<XenoSprite> buildings;
        protected List<XenoSprite> actions;
        protected List<XenoSprite> particles;
        protected List<XenoSprite> airParticles;
        protected int iff;
        protected string name;
        protected bool selecting;
        protected List<SCRTSUnit> selectedObjects;
        protected List<SCRTSUnit> group1;
        protected List<SCRTSUnit> group2;
        protected List<SCRTSUnit> group3;
        protected List<SCRTSUnit> group4;
        protected List<SCRTSUnit> group5;
        protected List<SCRTSUnit> group6;
        protected List<SCRTSUnit> group7;
        protected List<SCRTSUnit> group8;
        protected List<SCRTSUnit> group9;
        protected List<SCRTSUnit> group0;
        protected List<SCRTSUnit> groundUnits;
        protected List<SCRTSUnit> airUnits;
        protected Rectangle selectBox;
        protected Point2D topLeft;
        protected Point2D pointer;
        protected string resource1;
        protected int resourceCount1;
        protected string resource2;
        protected int resourceCount2;
        protected string resource3;
        protected int resourceCount3;
        protected bool targettingAbility;
        protected Texture2D flag;
        protected string flagName;
        protected int unitCap;
        protected GAMESTATUS gameStatus;

        //public

        public SCRTSCommander(string name = "default faction")
        {
            unitDB = new GenericBank<SCRTSUnit>();
            buildingDB = new GenericBank<SCRTSBuilding>();
            actionDB = new GenericBank<SCRTSAction>();
            statusDB = new GenericBank<SCRTSStatus>();
            abilityDB = new GenericBank<SCRTSAbility>();
            upgradeDB = new GenericBank<SCRTSUpgrade>();
            turretDB = new GenericBank<SCRTSTurret>();
            particleDB = new GenericBank<SCRTSParticle>();
            buffDB = new GenericBank<SCRTSBuff>();
            units = new List<XenoSprite>();
            buildings = new List<XenoSprite>();
            actions = new List<XenoSprite>();
            particles = new List<XenoSprite>();
            airParticles = new List<XenoSprite>();
            iff = 0;
            this.name = name;
            selectedObjects = new List<SCRTSUnit>();
            selectBox = new Rectangle(0, 0, 0, 0);
            selecting = false;
            topLeft = new Point2D(0, 0);
            pointer = new Point2D(0, 0);
            resource1 = "r1";
            resourceCount1 = 0;
            resource2 = "r2";
            resourceCount2 = 0;
            resource3 = "r3";
            resourceCount3 = 0;
            targettingAbility = false;
            flag = TextureBank.getTexture("flag 1");
            flagName = "flag 1";
            unitCap = 250;
            group1 = new List<SCRTSUnit>();
            group2 = new List<SCRTSUnit>();
            group3 = new List<SCRTSUnit>();
            group4 = new List<SCRTSUnit>();
            group5 = new List<SCRTSUnit>();
            group6 = new List<SCRTSUnit>();
            group7 = new List<SCRTSUnit>();
            group8 = new List<SCRTSUnit>();
            group9 = new List<SCRTSUnit>();
            group0 = new List<SCRTSUnit>();
            gameStatus = GAMESTATUS.INPROGRESS;
        }

        public SCRTSCommander(SCRTSCommander obj)
        {
            unitDB = obj.UnitDB;
            buildingDB = obj.BuildingDB;
            actionDB = obj.ActionDB;
            statusDB = obj.StatusDB;
            abilityDB = obj.AbilityDB;
            upgradeDB = obj.UpgradeDB;
            turretDB = obj.TurretDB;
            buffDB = obj.buffDB;
            units = obj.Units;
            buildings = obj.Buildings;
            actions = obj.Actions;
            particles = obj.Particles;
            airParticles = obj.AirParticles;
            iff = obj.IFF;
            name = obj.Name;
            group1 = obj.Group1;
            group2 = obj.Group2;
            group3 = obj.Group3;
            group4 = obj.Group4;
            group5 = obj.Group5;
            group6 = obj.Group6;
            group7 = obj.Group7;
            group8 = obj.Group8;
            group9 = obj.Group9;
            group0 = obj.Group0;
            selectedObjects = new List<SCRTSUnit>();
            selectBox = new Rectangle(0, 0, 0, 0);
            selecting = false;
            topLeft = new Point2D(0, 0);
            pointer = new Point2D(0, 0);
            targettingAbility = false;
            flag = obj.Flag;
            flagName = obj.FlagName;
            unitCap = obj.UnitCap;
            gameStatus = GAMESTATUS.INPROGRESS;
        }

        public SCRTSCommander(StreamReader sr)
        {
            string buffer = sr.ReadLine();
            name = sr.ReadLine();
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            int num = Convert.ToInt32(buffer);
            unitDB = new GenericBank<SCRTSUnit>();
            for (int i = 0; i < num; i++)
            {
                unitDB.addData(sr.ReadLine(), new SCRTSUnit(sr, this));
            }
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            buildingDB = new GenericBank<SCRTSBuilding>();
            for (int i = 0; i < num; i++)
            {
                buildingDB.addData(sr.ReadLine(), new SCRTSBuilding(sr, this));
            }
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            actionDB = new GenericBank<SCRTSAction>();
            for (int i = 0; i < num; i++)
            {
                actionDB.addData(sr.ReadLine(), new SCRTSAction(sr));
            }
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            statusDB = new GenericBank<SCRTSStatus>();
            for (int i = 0; i < num; i++)
            {
                statusDB.addData(sr.ReadLine(), new SCRTSStatus(sr));
            }
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            abilityDB = new GenericBank<SCRTSAbility>();
            for (int i = 0; i < num; i++)
            {
                abilityDB.addData(sr.ReadLine(), new SCRTSAbility(sr));
            }
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            upgradeDB = new GenericBank<SCRTSUpgrade>();
            for (int i = 0; i < num; i++)
            {
                upgradeDB.addData(sr.ReadLine(), new SCRTSUpgrade(sr));
            }
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            turretDB = new GenericBank<SCRTSTurret>();
            for (int i = 0; i < num; i++)
            {
                turretDB.addData(sr.ReadLine(), new SCRTSTurret(sr));
            }
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            buffDB = new GenericBank<SCRTSBuff>();
            for (int i = 0; i < num; i++)
            {
                buffDB.addData(sr.ReadLine(), new SCRTSBuff(sr));
            }
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            units = new List<XenoSprite>();
            for (int i = 0; i < num; i++)
            {
                units.Add(new SCRTSUnit(sr, this));
            }
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            buildings = new List<XenoSprite>();
            for (int i = 0; i < num; i++)
            {
                buildings.Add(new SCRTSBuilding(sr, this));
            }
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            actions = new List<XenoSprite>();
            for (int i = 0; i < num; i++)
            {
                actions.Add(new SCRTSAction(sr));
            }
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            particles = new List<XenoSprite>();
            for (int i = 0; i < num; i++)
            {
                particles.Add(new SCRTSParticle(sr));
            }
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            airParticles = new List<XenoSprite>();
            for (int i = 0; i < num; i++)
            {
                airParticles.Add(new SCRTSParticle(sr));
            }
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            iff = Convert.ToInt32(buffer);
            loadGroups(sr);
            selectedObjects = new List<SCRTSUnit>();
            selectBox = new Rectangle(0, 0, 0, 0);
            selecting = false;
            topLeft = new Point2D(0, 0);
            pointer = new Point2D(0, 0);
            targettingAbility = false;
            flagName = sr.ReadLine();
            flag = TextureBank.getTexture(flagName);
            unitCap = Convert.ToInt32(sr.ReadLine());
            gameStatus = GAMESTATUS.INPROGRESS;
        }
        /// <summary>
        /// Saves SCRTSCommander data provided a StreamWriter reference
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void saveData(StreamWriter sw)
        {
            sw.WriteLine("======SCRTSCommander data======");
            sw.WriteLine(name);
            sw.WriteLine("======UnitDB======");
            sw.WriteLine(unitDB.Index);
            for (int i = 0; i < unitDB.Index; i++)
            {
                sw.WriteLine(unitDB.Names[i]);
                unitDB.at(i).saveData(sw);
            }
            sw.WriteLine("======BuildingDB======");
            sw.WriteLine(buildingDB.Index);
            for (int i = 0; i < buildingDB.Index; i++)
            {
                sw.WriteLine(buildingDB.Names[i]);
                buildingDB.at(i).saveData(sw);
            }
            sw.WriteLine("======ActionDB======");
            sw.WriteLine(actionDB.Index);
            for (int i = 0; i < actionDB.Index; i++)
            {
                sw.WriteLine(actionDB.Names[i]);
                actionDB.at(i).saveData(sw);
            }
            sw.WriteLine("======StatusDB======");
            sw.WriteLine(statusDB.Index);
            for (int i = 0; i < statusDB.Index; i++)
            {
                sw.WriteLine(statusDB.Names[i]);
                statusDB.at(i).saveData(sw);
            }
            sw.WriteLine("======AbilityDB======");
            sw.WriteLine(abilityDB.Index);
            for (int i = 0; i < abilityDB.Index; i++)
            {
                sw.WriteLine(abilityDB.Names[i]);
                abilityDB.at(i).saveData(sw);
            }
            sw.WriteLine("======UpgradeDB======");
            sw.WriteLine(upgradeDB.Index);
            for (int i = 0; i < upgradeDB.Index; i++)
            {
                sw.WriteLine(upgradeDB.Names[i]);
                upgradeDB.at(i).saveData(sw);
            }
            sw.WriteLine("======TurretDB======");
            sw.WriteLine(turretDB.Index);
            for (int i = 0; i < turretDB.Index; i++)
            {
                sw.WriteLine(turretDB.Names[i]);
                turretDB.at(i).saveData(sw);
            }
            sw.WriteLine("======BuffDB======");
            sw.WriteLine(buffDB.Index);
            for (int i = 0; i < buffDB.Index; i++)
            {
                sw.WriteLine(buffDB.Names[i]);
                buffDB.at(i).saveData(sw);
            }
            sw.WriteLine("======Units======");
            sw.WriteLine(units.Count);
            for (int i = 0; i < units.Count; i++)
            {
                units[i].saveData(sw);
            }
            sw.WriteLine("======Buildings======");
            sw.WriteLine(buildings.Count);
            for (int i = 0; i < buildings.Count; i++)
            {
                buildings[i].saveData(sw);
            }
            sw.WriteLine("======Actions======");
            sw.WriteLine(actions.Count);
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i].saveData(sw);
            }
            sw.WriteLine("======Particles======");
            sw.WriteLine(particles.Count);
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].saveData(sw);
            }
            sw.WriteLine("======AirParticles======");
            sw.WriteLine(airParticles.Count);
            for (int i = 0; i < airParticles.Count; i++)
            {
                airParticles[i].saveData(sw);
            }
            sw.WriteLine("======IFF======");
            sw.WriteLine(iff);
            saveGroups(sw);
            sw.WriteLine(flagName);
            sw.WriteLine(unitCap);
        }
        /// <summary>
        /// Saves groups data
        /// </summary>
        /// <param name="sw">StreamWriter referecne</param>
        protected void saveGroups(StreamWriter sw)
        {
            sw.WriteLine("Group 1");
            sw.WriteLine(group1.Count);
            for(int i = 0; i < group1.Count; i++)
            {
                if(group1[i] is SCRTSBuilding)
                {
                    for(int b = 0; b < buildings.Count; b++)
                    {
                        if(buildings[b].IX == group1[i].IX &&
                            buildings[b].IY == group1[i].IY)
                        {
                            sw.WriteLine("building");
                            sw.WriteLine(b);
                            break;
                        }
                    }
                }
                else
                {
                    for (int u = 0; u < units.Count; u++)
                    {
                        if (units[u].IX == group1[i].IX &&
                            units[u].IY == group1[i].IY)
                        {
                            sw.WriteLine("unit");
                            sw.WriteLine(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine("Group 2");
            sw.WriteLine(group2.Count);
            for (int i = 0; i < group2.Count; i++)
            {
                if (group2[i] is SCRTSBuilding)
                {
                    for (int b = 0; b < buildings.Count; b++)
                    {
                        if (buildings[b].IX == group2[i].IX &&
                            buildings[b].IY == group2[i].IY)
                        {
                            sw.WriteLine("building");
                            sw.WriteLine(b);
                            break;
                        }
                    }
                }
                else
                {
                    for (int u = 0; u < units.Count; u++)
                    {
                        if (units[u].IX == group2[i].IX &&
                            units[u].IY == group2[i].IY)
                        {
                            sw.WriteLine("unit");
                            sw.WriteLine(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine("Group 3");
            sw.WriteLine(group3.Count);
            for (int i = 0; i < group3.Count; i++)
            {
                if (group3[i] is SCRTSBuilding)
                {
                    for (int b = 0; b < buildings.Count; b++)
                    {
                        if (buildings[b].IX == group3[i].IX &&
                            buildings[b].IY == group3[i].IY)
                        {
                            sw.WriteLine("building");
                            sw.WriteLine(b);
                            break;
                        }
                    }
                }
                else
                {
                    for (int u = 0; u < units.Count; u++)
                    {
                        if (units[u].IX == group3[i].IX &&
                            units[u].IY == group3[i].IY)
                        {
                            sw.WriteLine("unit");
                            sw.WriteLine(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine("Group 4");
            sw.WriteLine(group4.Count);
            for (int i = 0; i < group4.Count; i++)
            {
                if (group4[i] is SCRTSBuilding)
                {
                    for (int b = 0; b < buildings.Count; b++)
                    {
                        if (buildings[b].IX == group4[i].IX &&
                            buildings[b].IY == group4[i].IY)
                        {
                            sw.WriteLine("building");
                            sw.WriteLine(b);
                            break;
                        }
                    }
                }
                else
                {
                    for (int u = 0; u < units.Count; u++)
                    {
                        if (units[u].IX == group4[i].IX &&
                            units[u].IY == group4[i].IY)
                        {
                            sw.WriteLine("unit");
                            sw.WriteLine(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine("Group 5");
            sw.WriteLine(group5.Count);
            for (int i = 0; i < group5.Count; i++)
            {
                if (group5[i] is SCRTSBuilding)
                {
                    for (int b = 0; b < buildings.Count; b++)
                    {
                        if (buildings[b].IX == group5[i].IX &&
                            buildings[b].IY == group5[i].IY)
                        {
                            sw.WriteLine("building");
                            sw.WriteLine(b);
                            break;
                        }
                    }
                }
                else
                {
                    for (int u = 0; u < units.Count; u++)
                    {
                        if (units[u].IX == group5[i].IX &&
                            units[u].IY == group5[i].IY)
                        {
                            sw.WriteLine("unit");
                            sw.WriteLine(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine("Group 6");
            sw.WriteLine(group6.Count);
            for (int i = 0; i < group6.Count; i++)
            {
                if (group6[i] is SCRTSBuilding)
                {
                    for (int b = 0; b < buildings.Count; b++)
                    {
                        if (buildings[b].IX == group6[i].IX &&
                            buildings[b].IY == group6[i].IY)
                        {
                            sw.WriteLine("building");
                            sw.WriteLine(b);
                            break;
                        }
                    }
                }
                else
                {
                    for (int u = 0; u < units.Count; u++)
                    {
                        if (units[u].IX == group6[i].IX &&
                            units[u].IY == group6[i].IY)
                        {
                            sw.WriteLine("unit");
                            sw.WriteLine(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine("Group 7");
            sw.WriteLine(group7.Count);
            for (int i = 0; i < group7.Count; i++)
            {
                if (group7[i] is SCRTSBuilding)
                {
                    for (int b = 0; b < buildings.Count; b++)
                    {
                        if (buildings[b].IX == group7[i].IX &&
                            buildings[b].IY == group7[i].IY)
                        {
                            sw.WriteLine("building");
                            sw.WriteLine(b);
                            break;
                        }
                    }
                }
                else
                {
                    for (int u = 0; u < units.Count; u++)
                    {
                        if (units[u].IX == group7[i].IX &&
                            units[u].IY == group7[i].IY)
                        {
                            sw.WriteLine("unit");
                            sw.WriteLine(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine("Group 8");
            sw.WriteLine(group8.Count);
            for (int i = 0; i < group8.Count; i++)
            {
                if (group8[i] is SCRTSBuilding)
                {
                    for (int b = 0; b < buildings.Count; b++)
                    {
                        if (buildings[b].IX == group8[i].IX &&
                            buildings[b].IY == group8[i].IY)
                        {
                            sw.WriteLine("building");
                            sw.WriteLine(b);
                            break;
                        }
                    }
                }
                else
                {
                    for (int u = 0; u < units.Count; u++)
                    {
                        if (units[u].IX == group8[i].IX &&
                            units[u].IY == group8[i].IY)
                        {
                            sw.WriteLine("unit");
                            sw.WriteLine(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine("Group 9");
            sw.WriteLine(group9.Count);
            for (int i = 0; i < group9.Count; i++)
            {
                if (group9[i] is SCRTSBuilding)
                {
                    for (int b = 0; b < buildings.Count; b++)
                    {
                        if (buildings[b].IX == group9[i].IX &&
                            buildings[b].IY == group9[i].IY)
                        {
                            sw.WriteLine("building");
                            sw.WriteLine(b);
                            break;
                        }
                    }
                }
                else
                {
                    for (int u = 0; u < units.Count; u++)
                    {
                        if (units[u].IX == group9[i].IX &&
                            units[u].IY == group9[i].IY)
                        {
                            sw.WriteLine("unit");
                            sw.WriteLine(u);
                            break;
                        }
                    }
                }
            }
            sw.WriteLine("Group 0");
            sw.WriteLine(group0.Count);
            for (int i = 0; i < group0.Count; i++)
            {
                if (group0[i] is SCRTSBuilding)
                {
                    for (int b = 0; b < buildings.Count; b++)
                    {
                        if (buildings[b].IX == group0[i].IX &&
                            buildings[b].IY == group0[i].IY)
                        {
                            sw.WriteLine("building");
                            sw.WriteLine(b);
                            break;
                        }
                    }
                }
                else
                {
                    for (int u = 0; u < units.Count; u++)
                    {
                        if (units[u].IX == group0[i].IX &&
                            units[u].IY == group0[i].IY)
                        {
                            sw.WriteLine("unit");
                            sw.WriteLine(u);
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Loads groups data
        /// </summary>
        /// <param name="sr">StreamReader</param>
        protected void loadGroups(StreamReader sr)
        {
            group1 = new List<SCRTSUnit>();
            string buffer = sr.ReadLine();//discard data
            buffer = sr.ReadLine();
            int num = Convert.ToInt32(buffer);
            for(int i = 0; i < num; i++)
            {
                buffer = sr.ReadLine();
                if(buffer == "building")
                {
                    group1.Add((SCRTSUnit)buildings[Convert.ToInt32(sr.ReadLine())]);
                }
                else
                {
                    group1.Add((SCRTSUnit)units[Convert.ToInt32(sr.ReadLine())]);
                }
            }
            group2 = new List<SCRTSUnit>();
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            for (int i = 0; i < num; i++)
            {
                buffer = sr.ReadLine();
                if (buffer == "building")
                {
                    group2.Add((SCRTSUnit)buildings[Convert.ToInt32(sr.ReadLine())]);
                }
                else
                {
                    group2.Add((SCRTSUnit)units[Convert.ToInt32(sr.ReadLine())]);
                }
            }
            group3 = new List<SCRTSUnit>();
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            for (int i = 0; i < num; i++)
            {
                buffer = sr.ReadLine();
                if (buffer == "building")
                {
                    group3.Add((SCRTSUnit)buildings[Convert.ToInt32(sr.ReadLine())]);
                }
                else
                {
                    group3.Add((SCRTSUnit)units[Convert.ToInt32(sr.ReadLine())]);
                }
            }
            group4 = new List<SCRTSUnit>();
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            for (int i = 0; i < num; i++)
            {
                buffer = sr.ReadLine();
                if (buffer == "building")
                {
                    group4.Add((SCRTSUnit)buildings[Convert.ToInt32(sr.ReadLine())]);
                }
                else
                {
                    group4.Add((SCRTSUnit)units[Convert.ToInt32(sr.ReadLine())]);
                }
            }
            group5 = new List<SCRTSUnit>();
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            for (int i = 0; i < num; i++)
            {
                buffer = sr.ReadLine();
                if (buffer == "building")
                {
                    group5.Add((SCRTSUnit)buildings[Convert.ToInt32(sr.ReadLine())]);
                }
                else
                {
                    group5.Add((SCRTSUnit)units[Convert.ToInt32(sr.ReadLine())]);
                }
            }
            group6 = new List<SCRTSUnit>();
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            for (int i = 0; i < num; i++)
            {
                buffer = sr.ReadLine();
                if (buffer == "building")
                {
                    group6.Add((SCRTSUnit)buildings[Convert.ToInt32(sr.ReadLine())]);
                }
                else
                {
                    group6.Add((SCRTSUnit)units[Convert.ToInt32(sr.ReadLine())]);
                }
            }
            group7 = new List<SCRTSUnit>();
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            for (int i = 0; i < num; i++)
            {
                buffer = sr.ReadLine();
                if (buffer == "building")
                {
                    group7.Add((SCRTSUnit)buildings[Convert.ToInt32(sr.ReadLine())]);
                }
                else
                {
                    group7.Add((SCRTSUnit)units[Convert.ToInt32(sr.ReadLine())]);
                }
            }
            group8 = new List<SCRTSUnit>();
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            for (int i = 0; i < num; i++)
            {
                buffer = sr.ReadLine();
                if (buffer == "building")
                {
                    group8.Add((SCRTSUnit)buildings[Convert.ToInt32(sr.ReadLine())]);
                }
                else
                {
                    group8.Add((SCRTSUnit)units[Convert.ToInt32(sr.ReadLine())]);
                }
            }
            group9 = new List<SCRTSUnit>();
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            for (int i = 0; i < num; i++)
            {
                buffer = sr.ReadLine();
                if (buffer == "building")
                {
                    group9.Add((SCRTSUnit)buildings[Convert.ToInt32(sr.ReadLine())]);
                }
                else
                {
                    group9.Add((SCRTSUnit)units[Convert.ToInt32(sr.ReadLine())]);
                }
            }
            group0 = new List<SCRTSUnit>();
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            for (int i = 0; i < num; i++)
            {
                buffer = sr.ReadLine();
                if (buffer == "building")
                {
                    group0.Add((SCRTSUnit)buildings[Convert.ToInt32(sr.ReadLine())]);
                }
                else
                {
                    group0.Add((SCRTSUnit)units[Convert.ToInt32(sr.ReadLine())]);
                }
            }
        }
        /// <summary>
        /// Returns true it unit is an air unit else false
        /// </summary>
        /// <param name="unit">SCRTSUnit referecne</param>
        /// <returns>Boolean</returns>
        public bool isAir(SCRTSUnit unit)
        {
            if(unit.UT == UNITTYPES.AIRCARRIER)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.AIRCOMBAT)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.AIRHARVESTER)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.AIRHERO)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.AIRSUPPORT)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.AIRTRANSPORT)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.AIRWORKER)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns true it unit is a ground unit else false
        /// </summary>
        /// <param name="unit">SCRTSUnit referecne</param>
        /// <returns>Boolean</returns>
        public bool isGround(SCRTSUnit unit)
        {
            if (unit.UT == UNITTYPES.GROUNDCARRIER)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.GROUNDCOMBAT)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.GROUNDHARVESTER)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.GROUNDHERO)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.GROUNDSUPPORT)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.GROUNDTRANSPORT)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.GROUNDWORKER)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns true it unit is a support unit else false
        /// </summary>
        /// <param name="unit">SCRTSUnit referecne</param>
        /// <returns>Boolean</returns>
        public bool isSupport(SCRTSUnit unit)
        {
            if (unit.UT == UNITTYPES.AIRHARVESTER)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.AIRSUPPORT)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.AIRTRANSPORT)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.AIRWORKER)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.GROUNDHARVESTER)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.GROUNDSUPPORT)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.GROUNDTRANSPORT)
            {
                return true;
            }
            if (unit.UT == UNITTYPES.GROUNDWORKER)
            {
                return true;
            }
            return false;
        }

        public void update(RTSWorld world)
        {
            for(int a = 0; a < actions.Count; a++)
            {
                ((SCRTSAction)actions[a]).update();
            }
            for (int u = 0; u < units.Count; u++)
            {
                ((SCRTSUnit)units[u]).update();
            }
            for (int g = 0; g < groundUnits.Count; g++)
            {
                ((SCRTSUnit)groundUnits[g]).update();
            }
            for (int a = 0; a < airUnits.Count; a++)
            {
                ((SCRTSUnit)airUnits[a]).update();
            }
            for (int b = 0; b < buildings.Count; b++)
            {
                ((SCRTSBuilding)buildings[b]).update();
            }
        }
        /// <summary>
        /// Draws commander
        /// </summary>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        public void draw(int winx = 0, int winy = 0, int winw = 20, int winh = 17)
        {
            for(int g = 0; g < groundUnits.Count; g++)
            {
                if(onScreen(groundUnits[g].Center.IX, groundUnits[g].Center.IY, winx, winy, winw, winh) == true)
                {
                    LayeredSpriteRenderer.sprites.Add(groundUnits[g]);
                }
            }
            for(int a = 0; a < airUnits.Count; a++)
            {
                if(onScreen(airUnits[a].Center.IX, airUnits[a].Center.IY, winx, winy, winw, winh) == true)
                {
                    LayeredSpriteRenderer.sprites.Add(airUnits[a]);
                }
            }
            for(int b = 0; b < buildings.Count; b++)
            {
                if(onScreen(buildings[b].Center.IX, buildings[b].Center.IY, winx, winy, winw, winh) == true)
                {
                    LayeredSpriteRenderer.sprites.Add(buildings[b]);
                }
            }
            for(int a = 0; a < actions.Count; a++)
            {
                if(onScreen(actions[a].Center.IX, actions[a].Center.IY, winx, winy, winw, winh) == true)
                {
                    LayeredSpriteRenderer.sprites.Add(actions[a]);
                }
            }
            for (int p = 0; p < particles.Count; p++)
            {
                if (onScreen(particles[p].Center.IX, particles[p].Center.IY, winx, winy, winw, winh) == true)
                {
                    LayeredSpriteRenderer.sprites.Add(particles[p]);
                }
            }
            for (int a = 0; a < airParticles.Count; a++)
            {
                if(onScreen(airParticles[a].Center.IX, airParticles[a].Center.IY, winx, winy, winw, winh) == true)
                {
                    LayeredSpriteRenderer.sprites.Add(airParticles[a]);
                }
            }
        }
        /// <summary>
        /// Draws selectionBox
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="world">RTSWorld reference</param>
        public void drawSelctionBox(IntPtr renderer, RTSWorld world)
        {
            Rectangle rect = new Rectangle(selectBox.X - world.Winx, 
                selectBox.X - world.Winy, selectBox.Width, 
                selectBox.Height);
            DrawRects.drawRect(renderer, rect, 
                ColourBank.getColour(XENOCOLOURS.LIME_GREEN), false);
        }

        public void clickPlayerUI(RTSWorld world, SimpleCursor cursor)
        {

        }

        public void selectArea(RTSWorld world, SimpleCursor cursor)
        {
            if(selecting == true)
            {
                selectBox.Width = topLeft.X - (world.Winx + cursor.X);
                selectBox.Height = topLeft.Y - (world.Winy + cursor.Y);
            }
            else
            {
                for(int u = 0; u < units.Count; u++)
                {
                    if(selectBox.pointInRect(units[u].Center) == true)
                    {
                        selectedObjects.Add((SCRTSUnit)units[u]);
                        break;
                    }
                }
                for(int b = 0; b < buildings.Count; b++)
                {
                    if(selectBox.pointInRect(buildings[b].Center) == true)
                    {
                        selectedObjects.Add((SCRTSBuilding)buildings[b]);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Selects a single unit or building
        /// </summary>
        /// <param name="world">RTSWorld reference</param>
        /// <param name="world">RTSWorld reference</param>
        public void selectSingle(RTSWorld world, SimpleCursor cursor)
        {
            selectedObjects.Clear();
            Point2D tmpPoint = new Point2D(cursor.X + world.Winx,
                cursor.Y + world.Winy);
            for (int g = 0; g < groundUnits.Count; g++)
            {
                if (groundUnits[g].HitBox.pointInRect(tmpPoint) == true)
                {
                    selectedObjects.Add(groundUnits[g]);
                    break;
                }
            }
            for (int a = 0; a < airUnits.Count; a++)
            {
                if(airUnits[a].HitBox.pointInRect(tmpPoint) == true)
                {
                    selectedObjects.Add(airUnits[a]);
                    break;
                }
            }
            if(selectedObjects.Count <= 0)
            {
                for(int b = 0; b < buildings.Count; b++)
                {
                    if(buildings[b].HitBox.pointInRect(tmpPoint) == true)
                    {
                        selectedObjects.Add((SCRTSBuilding)buildings[b]);
                        break;
                    }
                }
            }
        }

        public void playerUI(RTSWorld world, SimpleCursor cursor)
        {
            pointer.X = MouseHandler.getMouseX();
            pointer.Y = MouseHandler.getMouseY();
            if(world.Window.pointInRect(pointer) == true)
            {
                if(MouseHandler.getLeft() == true)
                {
                    if(selecting == false)
                    {
                        selecting = true;
                        selectArea(world, cursor);
                    }
                    else
                    {
                        selectSingle(world, cursor);
                    }
                }
                else
                {
                    if(selecting == true)
                    {
                        selecting = false;
                    }
                }
                if(MouseHandler.getRight() == true)
                {

                }
            }
            else if(world.Window.pointInRect(pointer) == true)
            {
                clickPlayerUI(world, cursor);
            }
        }
        /// <summary>
        /// Issue an attack command to selected units and buildings
        /// </summary>
        /// <param name="world">RTSWorld reference</param>
        public void issueAttackCmd(RTSWorld world)
        {
            bool targetFound = false;
            Point2D p = new Point2D(world.Winx + MouseHandler.getMouseX(),
                world.Winy + MouseHandler.getMouseY());
            for(int f = 0; f < world.Commanders.Count; f++)
            {
                if(f != iff)
                {
                    for(int t = 0; t < world.Commanders[f].Units.Count; t++)
                    {
                        if (targetFound == false)
                        {
                            if (world.Commanders[f].Units[t].HitBox.pointInRect(p) == true)
                            {
                                targetFound = true;
                                for (int s = 0; s < selectedObjects.Count; s++)
                                {
                                    if (selectedObjects[s].canAttack() == true)
                                    {
                                        selectedObjects[s].Target = new SCAttackTarget(selectedObjects[s],
                                            ((SCRTSUnit)world.Commanders[f].Units[t]));
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Issue an attack command to selected units and buildings
        /// </summary>
        /// <param name="world">Crucable reference</param>
        public void issueMoveCmd(RTSWorld world)
        {
            Point2D p = new Point2D(world.Winx + MouseHandler.getMouseX(),
                world.Winy + MouseHandler.getMouseY());
            if(world.MG.getCell(p.IX, p.IY) == true)
            {
                for(int i = 0; i < selectedObjects.Count; i++)
                {
                    if(selectedObjects[i] is SCRTSBuilding == false)
                    {
                        selectedObjects[i].Path = world.SPF.findPath(selectedObjects[i].Center, p);
                    }
                }
            }
        }
        /// <summary>
        /// Issue a buff command to selected units and buildings
        /// </summary>
        /// <param name="world">RTSWorld reference</param>
        public void issueBuffCmd(RTSWorld world)
        {
            bool targetFound = false;
            Point2D p = new Point2D(world.Winx + MouseHandler.getMouseX(),
                world.Winy + MouseHandler.getMouseY());
            for (int t = 0; t < units.Count; t++)
            {
                if (targetFound == false)
                {
                    if (units[t].HitBox.pointInRect(p) == true)
                    {
                        targetFound = true;
                        for (int s = 0; s < selectedObjects.Count; s++)
                        {
                            if (selectedObjects[s].canBuff() == true)
                            {
                                selectedObjects[s].Target = new SCBuffTarget((SCRTSUnit)units[t], selectedObjects[s]);
                            }
                        }
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Issue a repair command to selected units and buildings
        /// </summary>
        /// <param name="world">World reference</param>
        public void issueRepairCmd(RTSWorld world)
        {
            bool targetFound = false;
            Point2D p = new Point2D(world.Winx + MouseHandler.getMouseX(),
                world.Winy + MouseHandler.getMouseY());
            for (int t = 0; t < units.Count; t++)
            {
                if (targetFound == false)
                {
                    if (units[t].HitBox.pointInRect(p) == true)
                    {
                        targetFound = true;
                        for (int s = 0; s < selectedObjects.Count; s++)
                        {
                            if (selectedObjects[s].canRepair() == true)
                            {
                                selectedObjects[s].Target = new SCRepairTarget((SCRTSUnit)units[t], selectedObjects[s]);
                            }
                        }
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Issue a build command to selected units
        /// </summary>
        /// <param name="world">Crucable reference</param>
        public void issueBuildCmd(RTSWorld world)
        {
            bool targetFound = false;
            Point2D p = new Point2D(world.Winx + MouseHandler.getMouseX(),
                world.Winy + MouseHandler.getMouseY());
            for (int t = 0; t < buildings.Count; t++)
            {
                if (targetFound == false)
                {
                    if (buildings[t].HitBox.pointInRect(p) == true)
                    {
                        targetFound = true;
                        for (int s = 0; s < selectedObjects.Count; s++)
                        {
                            if (selectedObjects[s].canBuild() == true)
                            {
                                selectedObjects[s].Target = new SCBuildTarget((SCRTSBuilding)buildings[t], selectedObjects[s]);
                            }
                        }
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Issue a heal command to selected units and buildings
        /// </summary>
        /// <param name="world">Crucable reference</param>
        public void issueHealCmd(RTSWorld world)
        {
            bool targetFound = false;
            Point2D p = new Point2D(world.Winx + MouseHandler.getMouseX(),
                world.Winy + MouseHandler.getMouseY());
            for (int t = 0; t < units.Count; t++)
            {
                if (targetFound == false)
                {
                    if (units[t].HitBox.pointInRect(p) == true)
                    {
                        targetFound = true;
                        for (int s = 0; s < selectedObjects.Count; s++)
                        {
                            if (selectedObjects[s].canHeal() == true)
                            {
                                selectedObjects[s].Target = new SCHealTarget((SCRTSUnit)units[t], selectedObjects[s]);
                            }
                        }
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Issue a harvest command to selected units and buildings
        /// </summary>
        /// <param name="world">World reference</param>
        public void issueHarvestCmd(RTSWorld world)
        {
            bool targetFound = false;
            int hx = (int)((world.Winx + MouseHandler.getMouseX()) / world.TileWidth);
            int hy = (int)((world.Winy + MouseHandler.getMouseY()) / world.TileHeight);
            Point2D p = new Point2D();
            for(int t = 0; t < units.Count; t++)
            {
                if(targetFound == false)
                {
                    if(world.Alpha != null)
                    {
                        if(world.Alpha.inCell(world.Winx + MouseHandler.getMouseX(), world.Winy + MouseHandler.getMouseY()) == true)
                        {

                        }
                    }
                    if(((RTSCell)world.Alpha).ResourceGrid.Fields.Grid[hx, hy] != null)
                    {
                        targetFound = true;
                        p.X = world.Winx + MouseHandler.getMouseX();
                        p.Y = world.Winy + MouseHandler.getMouseY();
                        for (int s = 0; s < selectedObjects.Count; s++)
                        {
                            if(selectedObjects[s].canHarvest() == true)
                            {
                                selectedObjects[s].Target = new SCHarvestTarget(p, selectedObjects[s]);
                            }
                        }
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Issue a refine command to selected units and buildings
        /// </summary>
        /// <param name="world">RTSWorld reference</param>
        public void issueRefineCmd(RTSWorld world)
        {
            Point2D p = new Point2D(MouseHandler.getMouseX() + world.Winx, MouseHandler.getMouseY() + world.Winy);
            for(int i = 0; i < buildings.Count; i++)
            {
                if(((SCRTSBuilding)buildings[i]).BT == BUILDINGTYPES.REFINORY)
                {
                    if(buildings[i].HitBox.pointInRect(p) == true)
                    {
                        for(int k = 0; k < selectedObjects.Count; k++)
                        {
                            if(selectedObjects[k].UT == UNITTYPES.GROUNDHARVESTER ||
                                selectedObjects[k].UT == UNITTYPES.AIRHARVESTER)
                            {
                                selectedObjects[i].Target = new SCRefineTarget(((SCRTSBuilding)buildings[i]), selectedObjects[k]);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Returns the nearest refinory type building in 
        /// commander's building list to Point p and returns 
        /// a reference to it
        /// </summary>
        /// <param name="p">Point2D reference</param>
        /// <returns>SCRTSBuilding</returns>
        public SCRTSBuilding nearestRefinory(Point2D p)
        {
            SCRTSBuilding bld = null;
            double curDist = 0;
            double dist = 0;
            for(int i = 0; i < buildings.Count; i++)
            {
                if (((SCRTSBuilding)buildings[i]).BT == BUILDINGTYPES.REFINORY)
                {
                    if (bld == null)
                    {
                        bld = ((SCRTSBuilding)buildings[i]);
                        curDist = Point2D.AsqrtB(buildings[i].Center, p);
                    }
                    else
                    {
                        dist = Point2D.AsqrtB(buildings[i].Center, p);
                        if (curDist > dist)
                        {
                            bld = ((SCRTSBuilding)buildings[i]);
                            curDist = dist;
                        }
                    }
                }
            }

            return bld;
        }
        /// <summary>
        /// Unloads a unit's resource tank into commander
        /// </summary>
        /// <param name="unit">SCRTSUnit reference</param>
        public void unloadResource(SCRTSUnit unit)
        {
            if(unit.TankLoad == resource1)
            {
                resourceCount1 += unit.Tank;
                unit.Tank = 0;
            }
            else if(unit.TankLoad == resource2)
            {
                resourceCount2 += unit.Tank;
                unit.Tank = 0;
            }
            else if (unit.TankLoad == resource3)
            {
                resourceCount3 += unit.Tank;
                unit.Tank = 0;
            }
        }
        /// <summary>
        /// Returns a list of all units and buldings within sight
        /// range of a viewing unit/building
        /// </summary>
        /// <param name="viewer">SCRTSUnit reference</param>
        /// <param name="cmdrs">List of RTSCommanders</param>
        /// <returns>List of SCRTSUnit references</returns>
        public List<SCRTSUnit> scanAroundObject(SCRTSUnit viewer, List<SCRTSCommander> cmdrs)
        {
            List<SCRTSUnit> objs = new List<SCRTSUnit>();
            double rng = viewer.GetSightRange;
            rng = rng * rng;
            for(int i = 0; i < cmdrs.Count; i++)
            {
                if (i != iff)
                {
                    //identify all enemy units in rang
                    for (int k = 0; k < cmdrs[i].Units.Count; k++)
                    {
                        if (Point2D.AsqrtB(viewer.Center, cmdrs[i].Units[k].Center) <= rng)
                        {
                            objs.Add(((SCRTSUnit)cmdrs[i].Units[k]));
                        }
                    }
                    //identify all enemy buildings in range
                    for (int k = 0; k < cmdrs[i].Buildings.Count; k++)
                    {
                        if (Point2D.AsqrtB(viewer.Center, cmdrs[i].Buildings[k].Center) <= rng)
                        {
                            objs.Add(((SCRTSUnit)cmdrs[i].Buildings[k]));
                        }
                    }
                }
            }
            return objs;
        }
        /// <summary>
        /// Returns the number of units specified in commander
        /// </summary>
        /// <param name="unitName">Name of unit to check</param>
        /// <returns>Number of units</returns>
        public int getUnitCount(string unitName)
        {
            int num = 0;
            for(int i = 0; i < units.Count; i++)
            {
                if(((SCRTSUnit)units[i]).UnitName == unitName)
                {
                    num++;
                }
            }
            return num;
        }
        /// <summary>
        /// Returns the number of buildings specified in commander
        /// </summary>
        /// <param name="buildingName">Name of building to check</param>
        /// <returns>Number of buildings</returns>
        public int getBuildingCount(string buildingName)
        {
            int num = 0;
            for (int i = 0; i < buildings.Count; i++)
            {
                if (((SCRTSBuilding)buildings[i]).BuildingName == buildingName)
                {
                    num++;
                }
            }
            return num;
        }
        /// <summary>
        /// Counts the number of turret type buildings
        /// </summary>
        /// <returns>Integer</returns>
        public int countTurrets()
        {
            int num = 0;
            for (int i = 0; i < buildings.Count; i++)
            {
                if (((SCRTSBuilding)buildings[i]).BT == BUILDINGTYPES.TURRET)
                {
                    num++;
                }
            }
            return num;
        }
        /// <summary>
        /// Count number of worker type units
        /// </summary>
        /// <returns>Integer</returns>
        public int countWorkers()
        {
            int num = 0;
            for(int i = 0; i < units.Count; i++)
            {
                if(((SCRTSUnit)units[i]).UT == UNITTYPES.AIRWORKER)
                {
                    num++;
                }
                else if(((SCRTSUnit)units[i]).UT == UNITTYPES.GROUNDWORKER)
                {
                    num++;
                }
            }
            return num;
        }
        /// <summary>
        /// Count number of harvester units
        /// </summary>
        public int countHarvesters()
        {
            int num = 0;
            for(int i = 0; i < units.Count; i++)
            {
                if(((SCRTSUnit)units[i]).UT == UNITTYPES.AIRHARVESTER)
                {
                    num++;
                }
                else if(((SCRTSUnit)units[i]).UT == UNITTYPES.GROUNDHARVESTER)
                {
                    num++;
                }
            }
            return num;
        }
        /// <summary>
        /// Count number of refinories
        /// </summary>
        public int countRefinories()
        {
            int num = 0;
            for (int i = 0; i < buildings.Count; i++)
            {
                if (((SCRTSBuilding)buildings[i]).BT == BUILDINGTYPES.REFINORY)
                {
                    num++;
                }
            }
            return num;
        }
        /// <summary>
        /// Returns a list of the names of all unlocked upgrades
        /// </summary>
        /// <returns>List of names</returns>
        public List<string> getUnlockedUpgrades()
        {
            List<string> tmp = new List<string>();
            for(int i = 0; i < abilityDB.Index; i++)
            {
                if(abilityDB.getData(i).AT == ABILITIES.UPGRADE)
                {
                    if(abilityDB.getData(i).Locked == false)
                    {
                        tmp.Add(abilityDB.getData(i).AbilityName);
                    }
                }
            }
            return tmp;
        }
        /// <summary>
        /// Sets a specified ability as locked
        /// </summary>
        /// <param name="abilityName">Name of ability to lock</param>
        public void lockAbility(string abilityName)
        {
            for(int a = 0; a < abilityDB.Index; a++)
            {
                if(abilityDB.getData(a).AbilityName == abilityName)
                {
                    abilityDB.getData(a).Locked = true;
                }
            }
            for(int u = 0; u < units.Count; u++)
            {
                ((SCRTSUnit)units[u]).lockAbility(abilityName);
            }
            for(int b = 0; b < buildings.Count; b++)
            {
                ((SCRTSBuilding)buildings[b]).lockAbility(abilityName);
            }
        }
        /// <summary>
        /// Sets a specified ability to unlocked
        /// </summary>
        /// <param name="abilityName">Name of ability ot unlock</param>
        public void unlockAbility(string abilityName)
        {
            for(int a = 0; a < abilityDB.Index; a++)
            {
                if(abilityDB.getData(a).AbilityName == abilityName)
                {
                    abilityDB.getData(a).Locked = false;
                }
            }
            for(int u = 0; u < units.Count; u++)
            {
                ((SCRTSUnit)units[u]).unlockAbility(abilityName);
            }
            for(int b = 0; b < buildings.Count; b++)
            {
                ((SCRTSBuilding)buildings[b]).unlockAbility(abilityName);
            }
        }
        /// <summary>
        /// Sets a specified upgrade to researched
        /// </summary>
        /// <param name="abilityName">Name of upgrade to research</param>
        public void researchAbility(string abilityName)
        {
            for(int a = 0; a < abilityDB.Index; a++)
            {
                if(abilityDB.getData(a).AbilityName == abilityName)
                {
                    if(abilityDB.getData(a) is SCRTSUpgrade)
                    {
                        ((SCRTSUpgrade)abilityDB.getData(a)).Researched = true;
                    }
                }
            }
            for(int u = 0; u < units.Count; u++)
            {
                ((SCRTSUnit)units[u]).researchAbility(abilityName);
            }
            for(int b = 0; b < buildings.Count; b++)
            {
                ((SCRTSBuilding)buildings[b]).researchAbility(abilityName);
            }
        }
        /// <summary>
        /// Sets upgrade to unresearched
        /// </summary>
        /// <param name="abilityName">Name of upgrade to unresearch</param>
        public void unresearchAbility(string abilityName)
        {
            for(int a = 0; a < abilityDB.Index; a++)
            {
                if(abilityDB.getData(a).AbilityName == abilityName)
                {
                    if(abilityDB.getData(a) is SCRTSUpgrade)
                    {
                        ((SCRTSUpgrade)abilityDB.getData(a)).Researched = false;
                        ((SCRTSUpgrade)abilityDB.getData(a)).Researching = false;
                    }
                }
            }
            for(int u = 0; u < units.Count; u++)
            {
                ((SCRTSUnit)units[u]).unresearchAbility(abilityName);
            }
            for(int b = 0; b < buildings.Count; b++)
            {
                ((SCRTSBuilding)buildings[b]).unresearchAbility(abilityName);
            }
        }
        /// <summary>
        /// Returns true if specified unit is not a worker type
        /// </summary>
        /// <param name="buildName">Name of specified unit</param>
        /// <returns>Boolean</returns>
        public bool notWorker(string buildName)
        {
            SCRTSUnit tmp = UnitDB.getData(buildName);
            if(tmp.UT != UNITTYPES.AIRWORKER &&
                tmp.UT != UNITTYPES.GROUNDWORKER)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Creates a new RTSUnit object at specified position
        /// </summary>
        /// <param name="unitName">Unit name</param>
        /// <param name="x">X position in pixels</param>
        /// <param name="y">Y position in pixels</param>
        /// <returns></returns>
        public SCRTSUnit createUnit(string unitName, float x, float y)
        {
            SCRTSUnit tmp = unitDB.getData(unitName);
            if(tmp != null)
            {
                new SCRTSUnit(tmp);
                tmp.Commander = this;
                tmp.X = x - tmp.W / 2;
                tmp.Y = y;
            }
            return tmp;
        }
        /// <summary>
        /// Returns the commander's count of specified resource
        /// </summary>
        /// <param name="resourceName">Resource name</param>
        /// <returns>Integer</returns>
        public int getResourceCount(string resourceName)
        {
            if(resource1 == resourceName)
            {
                return resourceCount1;
            }
            if (resource2 == resourceName)
            {
                return resourceCount2;
            }
            if (resource3 == resourceName)
            {
                return resourceCount3;
            }
            return 0;
        }
        /// <summary>
        /// Adds specified value to resource count
        /// </summary>
        /// <param name="resourceName">Resource name</param>
        /// <param name="val">Value to add</param>
        public void addResource(string resourceName, int val)
        {
            if(resource1 == resourceName)
            {
                resourceCount1 += val;
            }
            else if(resource2 == resourceName)
            {
                resourceCount2 += val;
            }
            else if(resource3 == resourceName)
            {
                resourceCount3 += val;
            }
        }
        /// <summary>
        /// Returns the index of unit at specified pixel position
        /// </Integer</returns>
        public int getUnitIndex(int x, int y)
        {
            Point2D temp = new Point2D(x, y);
            for(int i = 0; i < units.Count; i++)
            {
                if(units[i].HitBox.pointInRect(temp))
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// Returns the index of building at specified pixel position
        /// </Integer</returns>
        public int getBuildingIndex(int x, int y)
        {
            Point2D temp = new Point2D(x, y);
            for (int i = 0; i < buildings.Count; i++)
            {
                if (buildings[i].HitBox.pointInRect(temp))
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// Issues a command to a building, if command is "attack" a 
        /// target must be specified else a null can be provided
        /// </summary>
        /// <param name="index">Index of building</param>
        /// <param name="cmd">Command to issue</param>
        /// <param name="target">Target for command if any</param>
        public void commandBuilding(int index, string cmd, SCRTSUnit target)
        {
            if (cmd == "attack")
            {
                if (((SCRTSBuilding)buildings[index]).canAttack() == true)
                {
                    ((SCRTSBuilding)buildings[index]).Target = new SCAttackTarget(target, ((SCRTSBuilding)buildings[index]));
                    ((SCRTSBuilding)buildings[index]).attemptAttackTarget();
                }
            }
            else
            {
                ((SCRTSBuilding)buildings[index]).callAbility(cmd);
            }
        }
        /// <summary>
        /// Issues a command to a unit, if command is "attack", 
        /// "Heal", "Repair", "Buff" or "Debuff" a target 
        /// must be specified else a null can be provided
        /// </summary>
        /// <param name="index">Unit index</param>
        /// <param name="cmd">Command to issue</param>
        /// <param name="target">Target of command</param>
        public void commandUnit(int index, string cmd, SCRTSUnit target)
        {
            switch(cmd)
            {
                case "Attack":
                    units[index].Target = new SCAttackTarget(target, ((SCRTSUnit)units[index]));
                    ((SCRTSUnit)units[index]).callAbility(cmd);
                    break;
                case "Heal":
                    units[index].Target = new SCHealTarget(target, ((SCRTSUnit)units[index]));
                    ((SCRTSUnit)units[index]).callAbility(cmd);
                    break;
                case "Repair":
                    units[index].Target = new SCRepairTarget(target, ((SCRTSUnit)units[index]));
                    ((SCRTSUnit)units[index]).callAbility(cmd);
                    break;
                case "Buff":
                    units[index].Target = new SCBuffTarget(target, ((SCRTSUnit)units[index]));
                    ((SCRTSUnit)units[index]).callAbility(cmd);
                    break;
                case "Debuff":
                    units[index].Target = new SCDebuffTarget(target, ((SCRTSUnit)units[index]));
                    ((SCRTSUnit)units[index]).callAbility(cmd);
                    break;
            }
        }
        /// <summary>
        /// Creates a specified unit at specified location
        /// </summary>
        /// <param name="unitName">Unit name</param>
        /// <param name="x">X position to create at</param>
        /// <param name="y">Y position to create at</param>
        public void createUnit(string unitName, int x, int y)
        {
            SCRTSUnit tmp = null;
            SCRTSUnit tmp2 = null;
            tmp = unitDB.getData(unitName);
            if(tmp != null)
            {
                tmp2 = new SCRTSUnit(tmp);
                tmp2.X = x;
                tmp2.Y = y;
                units.Add(tmp2);
            }
        }
        /// <summary>
        /// Teleports a specific unit to a target position
        /// </summary>
        /// <param name="unitIndex">Unit to teleport index</param>
        /// <param name="tx">Target X position</param>
        /// <param name="ty">Target Y position</param>
        public void teleportUnit(int unitIndex, int tx, int ty)
        {
            units[unitIndex].setPos(tx, ty);
        }
        /// <summary>
        /// Teleports a single unit who's hit box ecoupesses p1 to position p2, sector value is for speeding
        /// up the check and required
        /// </summary>
        /// <param name="sector">Secotr value</param>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        public void teleportUnit(int sector, Point2D p1, Point2D p2)
        {
            for (int g = 0; g < groundUnits.Count - 1; g++)
            {
                if (groundUnits[g].Sector == sector)
                {
                    if (groundUnits[g].HitBox.pointInRect(p1) == true)
                    {
                        groundUnits[g].setPos(p2.X, p2.Y);
                        return;
                    }
                }
            }
            for (int a = 0; a < airUnits.Count - 1; a++)
            {
                if (airUnits[a].Sector == sector)
                {
                    if (airUnits[a].HitBox.pointInRect(p1) == true)
                    {
                        airUnits[a].setPos(p2.X, p2.Y);
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// Removes a specific unit grom all groups
        /// </summary>
        /// <param name="unit">SCRTSUnit reference</param>
        public void removeUnitFromGroups(SCRTSUnit unit)
        {
            if(group0 != null)
            {
                if(group0.Count > 0)
                {
                    for (int i = 0; i < group0.Count; i++)
                    {
                        if (group0[i].X == unit.X &&
                            group0[i].Y == unit.Y)
                        {
                            group0.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            if (group1 != null)
            {
                if (group1.Count > 0)
                {
                    for (int i = 0; i < group1.Count; i++)
                    {
                        if (group1[i].X == unit.X &&
                            group1[i].Y == unit.Y)
                        {
                            group1.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            if (group2 != null)
            {
                if (group2.Count > 0)
                {
                    for (int i = 0; i < group2.Count; i++)
                    {
                        if (group2[i].X == unit.X &&
                            group2[i].Y == unit.Y)
                        {
                            group2.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            if (group3 != null)
            {
                if (group3.Count > 0)
                {
                    for (int i = 0; i < group3.Count; i++)
                    {
                        if (group3[i].X == unit.X &&
                            group3[i].Y == unit.Y)
                        {
                            group3.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            if (group4 != null)
            {
                if (group4.Count > 0)
                {
                    for (int i = 0; i < group4.Count; i++)
                    {
                        if (group4[i].X == unit.X &&
                            group4[i].Y == unit.Y)
                        {
                            group4.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            if (group5 != null)
            {
                if (group5.Count > 0)
                {
                    for (int i = 0; i < group5.Count; i++)
                    {
                        if (group5[i].X == unit.X &&
                            group5[i].Y == unit.Y)
                        {
                            group5.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            if (group6 != null)
            {
                if (group6.Count > 0)
                {
                    for (int i = 0; i < group6.Count; i++)
                    {
                        if (group6[i].X == unit.X &&
                            group6[i].Y == unit.Y)
                        {
                            group6.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            if (group7 != null)
            {
                if (group7.Count > 0)
                {
                    for (int i = 0; i < group7.Count; i++)
                    {
                        if (group7[i].X == unit.X &&
                            group7[i].Y == unit.Y)
                        {
                            group7.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            if (group8 != null)
            {
                if (group8.Count > 0)
                {
                    for (int i = 0; i < group8.Count; i++)
                    {
                        if (group8[i].X == unit.X &&
                            group8[i].Y == unit.Y)
                        {
                            group8.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            if (group9 != null)
            {
                if (group9.Count > 0)
                {
                    for (int i = 0; i < group9.Count; i++)
                    {
                        if (group9[i].X == unit.X &&
                            group9[i].Y == unit.Y)
                        {
                            group9.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            if (selectedObjects != null)
            {
                if (selectedObjects.Count > 0)
                {
                    for (int i = 0; i < selectedObjects.Count; i++)
                    {
                        if (selectedObjects[i].X == unit.X &&
                            selectedObjects[i].Y == unit.Y)
                        {
                            selectedObjects.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Creates a new action at specified position with specified angle
        /// </summary>
        /// <param name="actName">Action name</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="angle">Action angle</param>
        public void createAction(string actName, int x, int y, int angle)
        {
            SCRTSAction act = actionDB.getData(actName);
            SCRTSAction tmp = new SCRTSAction(act);
            tmp.setPos(x, y);
            tmp.SelfAngle = (double)angle;
            actions.Add(tmp);
        }
        /// <summary>
        /// Creates a particle at specified position
        /// </summary>
        /// <param name="partName">Particle name</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public void createParticle(string partName, int x, int y)
        {
            SCRTSParticle act = particleDB.getData(partName);
            SCRTSParticle tmp = new SCRTSParticle(act);
            tmp.setPos(x, y);
            particles.Add(tmp);
        }
        /// <summary>
        /// Creates an air particle at specified position
        /// </summary>
        /// <param name="partName">Particle name</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public void createAirParticle(string partName, int x, int y)
        {
            SCRTSParticle act = particleDB.getData(partName);
            SCRTSParticle tmp = new SCRTSParticle(act);
            tmp.setPos(x, y);
            airParticles.Add(tmp);
        }
        /// <summary>
        /// Creates an air particle at specified position
        /// </summary>
        /// <param name="partName">Particle name</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="isAir">Is air particle flag</param>
        public void genParticle(string partName, int x, int y, bool isAir)
        {
            if(isAir == true)
            {
                createAirParticle(partName, x, y);
            }
            else
            {
                createParticle(partName, x, y);
            }
        }
        /// <summary>
        /// Set a unit's move path to a target
        /// </summary>
        /// <param name="unitIndex">Unit index</param>
        /// <param name="tx">X position</param>
        /// <param name="ty">Y position</param>
        public void setUnitPath(int unitIndex, int tx, int ty)
        {
            SCMoveTarget objective = new SCMoveTarget(new Point2D(tx, ty), ((SCRTSUnit)units[unitIndex]));
        }
        /// <summary>
        /// Returns true if a point is within the screen space
        /// </summary>
        /// <param name="x">X point</param>
        /// <param name="y">Y point</param>
        /// <param name="winx">Window X position</param>
        /// <param name="winy">Window Y position</param>
        /// <param name="winw">Window width in pixels</param>
        /// <param name="winh">Window height in pixels</param>
        /// <returns>Boolean</returns>
        public bool onScreen(int x, int y, int winx, int winy, int winw, int winh)
        { 
            if(x - 32 >= winx && x < winx + winw + 32)
            {
                if(y - 32 >= winy && y < winy + winh + 32)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Adds a unit to the appropriate unit list provided a name and position values
        /// </summary>
        /// <param name="name">Unit name</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public void addUnit(string name, int x, int y)
        {
            SCRTSUnit obj = unitDB.getData(name);
            if(obj != null)
            {
                SCRTSUnit obj2 = new SCRTSUnit(obj);
                obj.setPos(x, y);
                switch(obj.UT)
                {
                    case UNITTYPES.AIRCARRIER:
                    case UNITTYPES.AIRCOMBAT:
                    case UNITTYPES.AIRHERO:
                    case UNITTYPES.AIRSUPPORT:
                    case UNITTYPES.AIRTRANSPORT:
                        airUnits.Add(obj);
                        break;
                    case UNITTYPES.GROUNDCARRIER:
                    case UNITTYPES.GROUNDCOMBAT:
                    case UNITTYPES.GROUNDHERO:
                    case UNITTYPES.GROUNDSUPPORT:
                    case UNITTYPES.GROUNDTRANSPORT:
                        airUnits.Add(obj);
                        break;
                    case UNITTYPES.AIRHARVESTER:
                    case UNITTYPES.GROUNDHARVESTER:
                        airUnits.Add(obj);
                        break;
                    case UNITTYPES.AIRWORKER:
                    case UNITTYPES.GROUNDWORKER:
                        airUnits.Add(obj);
                        break;
                }
            }
        }
        /// <summary>
        /// Adds a unit to the appropriate unit list provided a name and position values
        /// </summary>
        /// <param name="name">Unit name</param>
        /// <param name="index">Index value</param>
        public void removeUnit(string name, int index)
        {
            SCRTSUnit obj = unitDB.getData(name);
            if (obj != null)
            {
                switch (obj.UT)
                {
                    case UNITTYPES.AIRCARRIER:
                    case UNITTYPES.AIRCOMBAT:
                    case UNITTYPES.AIRHERO:
                    case UNITTYPES.AIRSUPPORT:
                    case UNITTYPES.AIRTRANSPORT:
                        airUnits.RemoveAt(index);
                        break;
                    case UNITTYPES.GROUNDCARRIER:
                    case UNITTYPES.GROUNDCOMBAT:
                    case UNITTYPES.GROUNDHERO:
                    case UNITTYPES.GROUNDSUPPORT:
                    case UNITTYPES.GROUNDTRANSPORT:
                        airUnits.RemoveAt(index);
                        break;
                    case UNITTYPES.AIRHARVESTER:
                    case UNITTYPES.GROUNDHARVESTER:
                        airUnits.RemoveAt(index);
                        break;
                    case UNITTYPES.AIRWORKER:
                    case UNITTYPES.GROUNDWORKER:
                        airUnits.RemoveAt(index);
                        break;
                }
            }
        }
        /// <summary>
        /// Returns the numerical value of unit's type for a unit at specified position,
        /// if none found returns -1
        /// </summary>
        /// <param name="sector">Sector value</param>
        /// <param name="p">Poistion value</param>
        /// <returns>Integer</returns>
        public int getUnitTypeCode(int sector, Point2D p)
        {
            for(int g = 0; g < groundUnits.Count - 1; g++)
            {
                if(groundUnits[g].Sector == sector)
                {
                    if(groundUnits[g].HitBox.pointInRect(p) == true)
                    {
                        return (int)groundUnits[g].UT;
                    }
                }
            }
            for (int a = 0; a < airUnits.Count - 1; a++)
            {
                if (airUnits[a].Sector == sector)
                {
                    if (airUnits[a].HitBox.pointInRect(p) == true)
                    {
                        return (int)airUnits[a].UT;
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// Creates a particle at position provided a name and whether it's an air particle or not
        /// </summary>
        /// <param name="name">Particle name</param>
        /// <param name="air">Air flag value</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position </param>
        public void addParticle(string name, bool air, float x, float y)
        {
            SCRTSParticle par1 = particleDB.getData(name);
            SCRTSParticle par2 = null;
            if (par1 != null)
            {
                par2 = new SCRTSParticle(par1);
                par2.setPos(x, y);
                if(air == true)
                {
                    airParticles.Add(par2);
                }
                else
                {
                    particles.Add(par2);
                }
            }
        }
        /// <summary>
        /// Spawns an action at specified position moving in specified direction
        /// </summary>
        /// <param name="name">Action name</param>
        /// <param name="angle">Angle of movement</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public void spawnAction(string name, float angle, float x, float y)
        {
            SCRTSAction act1 = actionDB.getData(name);
            SCRTSAction act2 = null;
            if (act1 != null)
            {
                act2 = new SCRTSAction(act1);
                act2.SelfAngle = angle;
                act2.setPos(x, y);
                actions.Add(act2);
            }
        }
        /// <summary>
        /// Spawns a squad of psuedo random unit variation
        /// </summary>
        /// <param name="world">OpenWorld reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="scalerx">X scaler value, assume 32</param>
        /// <param name="scalery">Y scaler value, assume 32</param>
        public void spawnSquad(OpenWorld world, float x, float y, float scalerx = 32f, float scalery = 32f)
        {
            SCRTSUnit cre1 = null;  
            SCRTSUnit cre2 = null;
            SCRTSUnit cre3 = null;
            SCRTSUnit cre4 = null;
            SCRTSUnit cre5 = null;
            SCRTSUnit temp = null;
            Random rand = new Random((int)System.DateTime.Today.Ticks);
            if(rand.Next(0, 1000) < 500)
            {
                cre1 = getCheapestGroundUnit();
                if(cre1 != null)
                {
                    cre2 = getCheapestGroundUnit(cre1.Name);
                }
                else
                {
                    cre2 = getCheapestGroundUnit();
                    cre1 = getCheapestGroundUnit(cre2.Name);
                }
                if(cre1 != null && cre2 != null)
                {
                    cre3 = getRandomGroundUnit(cre1.Name, cre2.Name);
                    cre4 = getRandomGroundUnit(cre1.Name, cre2.Name);
                    cre5 = getRandomGroundUnit(cre1.Name, cre2.Name);
                }
                else
                {
                    cre3 = getRandomGroundUnit();
                    cre4 = getRandomGroundUnit();
                    cre5 = getRandomGroundUnit();
                }
                List<Point2D> positions = world.getArea((int)(x / scalerx), (int)(y / scalery), 5);
                temp = new SCRTSUnit(cre1);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre1);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre1);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre2);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre2);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre2);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre3);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre3);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre4);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre4);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre5);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre5);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
            }
            else
            {
                cre1 = getCheapestAirUnit();
                if (cre1 != null)
                {
                    cre2 = getCheapestAirUnit(cre1.Name);
                }
                else
                {
                    cre2 = getCheapestAirUnit();
                    cre1 = getCheapestAirUnit(cre2.Name);
                }
                if (cre1 != null && cre2 != null)
                {
                    cre3 = getRandomAirUnit(cre1.Name, cre2.Name);
                    cre4 = getRandomAirUnit(cre1.Name, cre2.Name);
                    cre5 = getRandomAirUnit(cre1.Name, cre2.Name);
                }
                else
                {
                    cre3 = getRandomAirUnit();
                    cre4 = getRandomAirUnit();
                    cre5 = getRandomAirUnit();
                }
                List<Point2D> positions = world.getArea((int)(x / scalerx), (int)(y / scalery), 5);
                temp = new SCRTSUnit(cre1);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre1);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre1);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre2);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre2);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre2);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre3);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre3);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre4);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre4);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre5);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
                temp = new SCRTSUnit(cre5);
                temp.setPos(positions[0].X, positions[0].Y);
                positions.RemoveAt(0);
                units.Add(temp);
            }
        }
        /// <summary>
        /// Returns the reference in the unitDB to the lowest cost ground unit
        /// </summary>
        /// <returns>SCRTSUnit reference</returns>
        public SCRTSUnit getCheapestGroundUnit()
        {
            SCRTSUnit cre = unitDB.at(0);
            if(cre == null)
            {
                return cre;
            }
            for(int i = 1; i < unitDB.Index; i++)
            {
                if(unitDB.at(i).UT == UNITTYPES.GROUNDCOMBAT)
                {
                    int cost = unitDB.at(i).Cost1 + unitDB.at(i).Cost2 + unitDB.at(i).Cost3;
                    if(cost < cre.Cost1 + cre.Cost2 + cre.Cost3)
                    {
                        cre = unitDB.at(i);
                    }
                }
            }
            return cre;
        }
        /// <summary>
        /// Returns reference in unitDB lowest cost ground unit that's not a worker or harvester, 
        /// excluding the unit of the provided name
        /// </summary>
        /// <param name="name">Name in unitDB</param>
        /// <returns>SCRTSUnit reference</returns>
        public SCRTSUnit getCheapestGroundUnit(string name)
        {
            SCRTSUnit cre = unitDB.at(0);
            if(cre.Name == name)
            {
                return null;
            }
            if(cre == null)
            {
                return cre;
            }
            for(int i = 1; i < unitDB.Index; i++)
            {
                if(cre.Name != name)
                {
                    if(unitDB.at(i).UT == UNITTYPES.GROUNDCOMBAT)
                    {
                        int cost = unitDB.at(i).Cost1 + unitDB.at(i).Cost2 + unitDB.at(i).Cost3;
                        if(cost < cre.Cost1 + cre.Cost2 + cre.Cost3)
                        {
                            cre = unitDB.at(i);
                        }
                    }
                }
            }
            return cre;
        }
        /// <summary>
        /// Returns the reference in the unitDB to the lowest cost air unit
        /// </summary>
        /// <returns>SCRTSUnit reference</returns>
        public SCRTSUnit getCheapestAirUnit()
        {
            SCRTSUnit cre = unitDB.at(0);
            if (cre == null)
            {
                return cre;
            }
            for (int i = 1; i < unitDB.Index; i++)
            {
                if (unitDB.at(i).UT == UNITTYPES.AIRCOMBAT)
                {
                    int cost = unitDB.at(i).Cost1 + unitDB.at(i).Cost2 + unitDB.at(i).Cost3;
                    if (cost < cre.Cost1 + cre.Cost2 + cre.Cost3)
                    {
                        cre = unitDB.at(i);
                    }
                }
            }
            return cre;
        }
        /// <summary>
        /// Returns reference in unitDB lowest cost air unit that's not a worker or harvester, 
        /// excluding the unit of the provided name
        /// </summary>
        /// <param name="name">Name in unitDB</param>
        /// <returns>SCRTSUnit reference</returns>
        public SCRTSUnit getCheapestAirUnit(string name)
        {
            SCRTSUnit cre = unitDB.at(0);
            if (cre.Name == name)
            {
                return null;
            }
            if (cre == null)
            {
                return cre;
            }
            for (int i = 1; i < unitDB.Index; i++)
            {
                if (cre.Name != name)
                {
                    if (unitDB.at(i).UT == UNITTYPES.AIRCOMBAT)
                    {
                        int cost = unitDB.at(i).Cost1 + unitDB.at(i).Cost2 + unitDB.at(i).Cost3;
                        if (cost < cre.Cost1 + cre.Cost2 + cre.Cost3)
                        {
                            cre = unitDB.at(i);
                        }
                    }
                }
            }
            return cre;
        }
        /// <summary>
        /// Returns a random ground unit that's not named name1 or name2
        /// </summary>
        /// <param name="name1">Name 1 to exclude</param>
        /// <param name="name2">Name 2 to exclude</param>
        /// <returns>SCRTSUnit reference</returns>
        public SCRTSUnit getRandomGroundUnit(string name1 = "", string name2 = "")
        {
            Random rand = new Random((int)System.DateTime.Today.Ticks);
            SCRTSUnit cre = unitDB.at(rand.Next(0, unitDB.Index));
            if(cre == null)
            {
                return cre;
            }
            while(cre.UT != UNITTYPES.GROUNDCOMBAT || namesNoMatch(cre, name1, name2) == false)
            {
                cre = unitDB.at(rand.Next(0, unitDB.Index));
            }
            return cre;
        }
        /// <summary>
        /// Returns a random air unit that's not named name1 or name2
        /// </summary>
        /// <param name="name1">Name 1 to exclude</param>
        /// <param name="name2">Name 2 to exclude</param>
        /// <returns>SCRTSUnit reference</returns>
        public SCRTSUnit getRandomAirUnit(string name1 = "", string name2 = "")
        {
            Random rand = new Random((int)System.DateTime.Today.Ticks);
            SCRTSUnit cre = unitDB.at(rand.Next(0, unitDB.Index));
            if (cre == null)
            {
                return cre;
            }
            while (cre.UT != UNITTYPES.AIRCOMBAT || namesNoMatch(cre, name1, name2) == false)
            {
                cre = unitDB.at(rand.Next(0, unitDB.Index));
            }
            return cre;
        }
        /// <summary>
        /// Returns the index of unit that is at point p, returns -1 if no unit at that point 
        /// </summary>
        /// <param name="p">Point2D reference</param>
        /// <returns>Integer</returns>
        public int unitAtPoint(Point2D p)
        {
            for(int i = 0; i < units.Count - 1; i++)
            {
                if(units[i].HitBox.pointInRect(p))
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// Checks if either provided name matches that of SCRTSUnit's name value, returns true
        /// in the event neither name matches else returns false
        /// </summary>
        /// <param name="cre">SCRTSUnit reference</param>
        /// <param name="name1">Name value 1</param>
        /// <param name="name2">Name value 2</param>
        /// <returns>Boolean</returns>
        protected bool namesNoMatch(SCRTSUnit cre, string name1, string name2)
        {
            if(cre == null)
            {
                return true;
            }
            if(cre.Name == name1)
            {
                return false;
            }
            if(cre.Name == name2)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Returns true if any unit center within trigger rectangle
        /// </summary>
        /// <param name="rect">Rectange reference</param>
        /// <returns>Boolean</returns>
        public bool touchTrigger(Rectangle rect)
        {
            for(int i = 0; i < units.Count - 1; i++)
            {
                if(rect.pointInRect(units[i].Center) == true)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Returns true if unit at index center within trigger rect
        /// </summary>
        /// <param name="index">Index of unit to check</param>
        /// <param name="rect">Rectangle reference</param>
        /// <returns>Boolean</returns>
        public bool unitTouchTrigger(int index, Rectangle rect)
        {
            if(rect.pointInRect(units[index].Center) == true)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// UnitDB property
        /// </summary>
        public GenericBank<SCRTSUnit> UnitDB
        {
            get { return unitDB; }
        }
        /// <summary>
        /// BuildingDB property
        /// </summary>
        public GenericBank<SCRTSBuilding> BuildingDB
        {
            get { return buildingDB; }
        }
        /// <summary>
        /// ActionDB property
        /// </summary>
        public GenericBank<SCRTSAction> ActionDB
        {
            get { return actionDB; }
        }
        /// <summary>
        /// StatusDB property
        /// </summary>
        public GenericBank<SCRTSStatus> StatusDB
        {
            get { return statusDB; }
        }
        /// <summary>
        /// AbilityDB property
        /// </summary>
        public GenericBank<SCRTSAbility> AbilityDB
        {
            get { return abilityDB; }
        }
        /// <summary>
        /// UpgradeDB property
        /// </summary>
        public GenericBank<SCRTSUpgrade> UpgradeDB
        {
            get { return upgradeDB; }
        }
        /// <summary>
        /// ParticleDB property
        /// </summary>
        public GenericBank<SCRTSParticle> ParticleDB
        {
            get { return particleDB; }
        }
        /// <summary>
        /// Units property
        /// </summary>
        public List<XenoSprite> Units
        {
            get { return units; }
        }
        /// <summary>
        /// Units property
        /// </summary>
        public List<XenoSprite> Buildings
        {
            get { return buildings; }
        }
        /// <summary>
        /// GroundToGround property
        /// </summary>
        public List<XenoSprite> Actions
        {
            get { return actions; }
        }
        /// <summary>
        /// Particles property
        /// </summary>
        public List<XenoSprite> Particles
        {
            get { return particles; }
        }
        /// <summary>
        /// AirParticles property
        /// </summary>
        public List<XenoSprite> AirParticles
        {
            get { return airParticles; }
        }
        /// <summary>
        /// TurretDB property
        /// </summary>
        public GenericBank<SCRTSTurret> TurretDB
        {
            get { return turretDB; }
        }
        /// <summary>
        /// BuffDB property
        /// </summary>
        public GenericBank<SCRTSBuff> BuffDB
        {
            get { return buffDB; }
        }
        /// <summary>
        /// IFF property
        /// </summary>
        public int IFF
        {
            get { return iff; }
            set { iff = value; }
        }
        /// <summary>
        /// Name property
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// Group1 property
        /// </summary>
        public List<SCRTSUnit> Group1
        {
            get { return group1; }
        }
        /// <summary>
        /// Group2 property
        /// </summary>
        public List<SCRTSUnit> Group2
        {
            get { return group2; }
        }
        /// <summary>
        /// Group3 property
        /// </summary>
        public List<SCRTSUnit> Group3
        {
            get { return group3; }
        }
        /// <summary>
        /// Group4 property
        /// </summary>
        public List<SCRTSUnit> Group4
        {
            get { return group4; }
        }
        /// <summary>
        /// Group5 property
        /// </summary>
        public List<SCRTSUnit> Group5
        {
            get { return group5; }
        }
        /// <summary>
        /// Group6 property
        /// </summary>
        public List<SCRTSUnit> Group6
        {
            get { return group6; }
        }
        /// <summary>
        /// Group7 property
        /// </summary>
        public List<SCRTSUnit> Group7
        {
            get { return group7; }
        }
        /// <summary>
        /// Group8 property
        /// </summary>
        public List<SCRTSUnit> Group8
        {
            get { return group8; }
        }
        /// <summary>
        /// Group9 property
        /// </summary>
        public List<SCRTSUnit> Group9
        {
            get { return group9; }
        }
        /// <summary>
        /// Group0 property
        /// </summary>
        public List<SCRTSUnit> Group0
        {
            get { return group0; }
        }
        /// <summary>
        /// GroundUnits property
        /// </summary>
        public List<SCRTSUnit> GroundUnits
        {
            get
            {
                if(groundUnits != null)
                {
                    groundUnits.Clear();
                }
                else
                {
                    groundUnits = new List<SCRTSUnit>();
                }
                for(int i = 0; i < units.Count; i++)
                {
                    switch(((SCRTSUnit)units[i]).UT)
                    {
                        case UNITTYPES.GROUNDCARRIER:
                            groundUnits.Add(((SCRTSUnit)units[i]));
                            break;
                        case UNITTYPES.GROUNDCOMBAT:
                            groundUnits.Add(((SCRTSUnit)units[i]));
                            break;
                        case UNITTYPES.GROUNDHARVESTER:
                            groundUnits.Add(((SCRTSUnit)units[i]));
                            break;
                        case UNITTYPES.GROUNDHERO:
                            groundUnits.Add(((SCRTSUnit)units[i]));
                            break;
                        case UNITTYPES.GROUNDSUPPORT:
                            groundUnits.Add(((SCRTSUnit)units[i]));
                            break;
                        case UNITTYPES.GROUNDTRANSPORT:
                            groundUnits.Add(((SCRTSUnit)units[i]));
                            break;
                        case UNITTYPES.GROUNDWORKER:
                            groundUnits.Add(((SCRTSUnit)units[i]));
                            break;
                    }
                }
                return groundUnits;
            }
        }
        /// <summary>
        /// AirUnits property
        /// </summary>
        public List<SCRTSUnit> AirUnits
        {
            get
            {
                if (airUnits != null)
                {
                    airUnits.Clear();
                }
                else
                {
                    airUnits = new List<SCRTSUnit>();
                }
                for (int i = 0; i < units.Count; i++)
                {
                    switch (((SCRTSUnit)units[i]).UT)
                    {
                        case UNITTYPES.AIRCARRIER:
                            airUnits.Add(((SCRTSUnit)units[i]));
                            break;
                        case UNITTYPES.AIRCOMBAT:
                            airUnits.Add(((SCRTSUnit)units[i]));
                            break;
                        case UNITTYPES.AIRHARVESTER:
                            airUnits.Add(((SCRTSUnit)units[i]));
                            break;
                        case UNITTYPES.AIRHERO:
                            airUnits.Add(((SCRTSUnit)units[i]));
                            break;
                        case UNITTYPES.AIRSUPPORT:
                            airUnits.Add(((SCRTSUnit)units[i]));
                            break;
                        case UNITTYPES.AIRTRANSPORT:
                            airUnits.Add(((SCRTSUnit)units[i]));
                            break;
                        case UNITTYPES.AIRWORKER:
                            airUnits.Add(((SCRTSUnit)units[i]));
                            break;
                    }
                }
                return airUnits;
            }
        }
        /// <summary>
        /// Resource1 property
        /// </summary>
        public string Resource1
        {
            get { return resource1; }
            set { resource1 = value; }
        }
        /// <summary>
        /// ResourceCount1 property
        /// </summary>
        public int ResourceCount1
        {
            get { return resourceCount1; }
            set { resourceCount1 = value; }
        }
        /// <summary>
        /// Resource2 property
        /// </summary>
        public string Resource2
        {
            get { return resource2; }
            set { resource2 = value; }
        }
        /// <summary>
        /// ResourceCount2 property
        /// </summary>
        public int ResourceCount2
        {
            get { return resourceCount2; }
            set { resourceCount2 = value; }
        }
        /// <summary>
        /// Resource3 property
        /// </summary>
        public string Resource3
        {
            get { return resource3; }
            set { resource3 = value; }
        }
        /// <summary>
        /// ResourceCount3 property
        /// </summary>
        public int ResourceCount3
        {
            get { return resourceCount3; }
            set { resourceCount3 = value; }
        }
        /// <summary>
        /// TargettingAbility property
        /// </summary>
        public bool TargettingAbility
        {
            get { return targettingAbility; }
            set { targettingAbility = value; }
        }
        /// <summary>
        /// SelectedObjects property
        /// </summary>
        public List<SCRTSUnit> SelectedObjects
        {
            get { return selectedObjects; }
        }
        /// <summary>
        /// Flag property
        /// </summary>
        public Texture2D Flag
        {
            get { return flag; }
            set { flag = value; }
        }
        /// <summary>
        /// FlagName property
        /// </summary>
        public string FlagName
        {
            get { return flagName; }
            set { flagName = value; }
        }
        /// <summary>
        /// UnitCap property
        /// </summary>
        public int UnitCap
        {
            get { return unitCap; }
            set { unitCap = value; }
        }
        /// <summary>
        /// GameStatus property
        /// </summary>
        public GAMESTATUS GameStatus
        {
            get { return gameStatus; }
            set { gameStatus = value; }
        }
    }
}
