//====================================================
//Written by Kujel Selsuru
//Last Updated 12/05/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenoLib
{
    /// <summary>
    /// Converts various RTS enumerations to string values and string values to enumerations
    /// </summary>
    public static class RTSTypeConverters
    {
        /// <summary>
        /// Converts UNITYPES to string value
        /// </summary>
        /// <param name="ut">UNITYPES value</param>
        /// <returns>string</returns>
        public static string convertUnitType(UNITTYPES ut)
        {
            switch (ut)
            {
                case UNITTYPES.AIRCARRIER:
                    return "AIRCARRIER";
                case UNITTYPES.AIRCOMBAT:
                    return "AIRCOMBAT";
                case UNITTYPES.AIRHARVESTER:
                    return "AIRHARVESTER";
                case UNITTYPES.AIRHERO:
                    return "AIRHERO";
                case UNITTYPES.AIRSUPPORT:
                    return "AIRSUPPORT";
                case UNITTYPES.AIRTRANSPORT:
                    return "AIRTRANSPORT";
                case UNITTYPES.AIRWORKER:
                    return "AIRWORKER";
                case UNITTYPES.GROUNDCARRIER:
                    return "GROUNDCARRIER";
                case UNITTYPES.GROUNDCOMBAT:
                    return "GROUNDCOMBAT";
                case UNITTYPES.GROUNDHARVESTER:
                    return "GROUNDHARVESTER";
                case UNITTYPES.GROUNDHERO:
                    return "GROUNDHERO";
                case UNITTYPES.GROUNDSUPPORT:
                    return "GROUNDSUPPORT";
                case UNITTYPES.GROUNDTRANSPORT:
                    return "GROUNDTRANSPORT";
                case UNITTYPES.GROUNDWORKER:
                    return "GROUNDWORKER";
            }
            return "NONE";
        }
        /// <summary>
        /// Converts string to UNITYPES value
        /// </summary>
        /// <param name="ut">String value</param>
        /// <returns>UNITTYPES</returns>
        public static UNITTYPES convertUnitType(string ut)
        {
            switch (ut)
            {
                case "AIRCARRIER":
                    return UNITTYPES.AIRCARRIER;
                case "AIRCOMBAT":
                    return UNITTYPES.AIRCOMBAT;
                case "AIRHARVESTER":
                    return UNITTYPES.AIRHARVESTER;
                case "AIRHERO":
                    return UNITTYPES.AIRHERO;
                case "AIRSUPPORT":
                    return UNITTYPES.AIRSUPPORT;
                case "AIRTRANSPORT":
                    return UNITTYPES.AIRTRANSPORT;
                case "AIRWORKER":
                    return UNITTYPES.AIRWORKER;
                case "GROUNDCARRIER":
                    return UNITTYPES.GROUNDCARRIER;
                case "GROUNDCOMBAT":
                    return UNITTYPES.GROUNDCOMBAT;
                case "GROUNDHARVESTER":
                    return UNITTYPES.GROUNDHARVESTER;
                case "GROUNDHERO":
                    return UNITTYPES.GROUNDHERO;
                case "GROUNDSUPPORT":
                    return UNITTYPES.GROUNDSUPPORT;
                case "GROUNDTRANSPORT":
                    return UNITTYPES.GROUNDTRANSPORT;
                case "GROUNDWORKER":
                    return UNITTYPES.GROUNDWORKER;
            }
            return UNITTYPES.NONE;
        }
        /// <summary>
        /// Converts STATUSES to string value
        /// </summary>
        /// <param name="st">STATUSES value</param>
        /// <returns>string</returns>
        public static string convertStatusesType(STATUSES st)
        {
            switch (st)
            {
                case STATUSES.BUFFARMOR:
                    return "BUFFARMOR";
                case STATUSES.BUFFDAMAGE:
                    return "BUFFDAMAGE";
                case STATUSES.BUFFSPEED:
                    return "BUFFSPEED";
                case STATUSES.DAMAGE:
                    return "DAMAGE";
                case STATUSES.DEBUFFARMOR:
                    return "DEBUFFARMOR";
                case STATUSES.DEBUFFDAMAGE:
                    return "DEBUFFDAMAGE";
                case STATUSES.DEBUFFSPEED:
                    return "DEBUFFSPEED";
                case STATUSES.HEAL:
                    return "HEAL";
                case STATUSES.POISON:
                    return "POISON";
                case STATUSES.REGEN:
                    return "REGEN";
            }
            return "NONE";
        }
        /// <summary>
        /// Converts string to STATUSES value
        /// </summary>
        /// <param name="st">String value</param>
        /// <returns>STATUSES</returns>
        public static STATUSES convertStatusType(string st)
        {
            switch (st)
            {
                case "BUFFARMOR":
                    return STATUSES.BUFFARMOR;
                case "BUFFDAMAGE":
                    return STATUSES.BUFFDAMAGE;
                case "BUFFSPEED":
                    return STATUSES.BUFFSPEED;
                case "DAMAGE":
                    return STATUSES.DAMAGE;
                case "DEBUFFARMOR":
                    return STATUSES.DEBUFFARMOR;
                case "DEBUFFDAMAGE":
                    return STATUSES.DEBUFFDAMAGE;
                case "DEBUFFSPEED":
                    return STATUSES.DEBUFFSPEED;
                case "HEAL":
                    return STATUSES.HEAL;
                case "POISON":
                    return STATUSES.POISON;
                case "REGEN":
                    return STATUSES.REGEN;
            }
            return STATUSES.NONE;
        }
        /// <summary>
        /// Converts BUILDINGTYPES to string value
        /// </summary>
        /// <param name="bt">BUILDINGTYPES value</param>
        /// <returns>string</returns>
        public static string convertBuildingType(BUILDINGTYPES bt)
        {
            switch (bt)
            {
                case BUILDINGTYPES.BUNKER:
                    return "BUNKER";
                case BUILDINGTYPES.COMMANDCENTER:
                    return "COMMANDCENTER";
                case BUILDINGTYPES.FACTORY:
                    return "FACTORY";
                case BUILDINGTYPES.FOUNDATION:
                    return "FOUNDATION";
                case BUILDINGTYPES.LAB:
                    return "LAB";
                case BUILDINGTYPES.MINE:
                    return "MINE";
                case BUILDINGTYPES.REFINORY:
                    return "REFINORY";
                case BUILDINGTYPES.TURRET:
                    return "TURRET";
            }
            return "NONE";
        }
        /// <summary>
        /// Converts string to BUILDINGYPES value
        /// </summary>
        /// <param name="bt">String value</param>
        /// <returns>BUILDINGTYPES</returns>
        public static BUILDINGTYPES convertBuildingType(string bt)
        {
            switch (bt)
            {
                case "BUNKER":
                    return BUILDINGTYPES.BUNKER;
                case "COMMANDCENTER":
                    return BUILDINGTYPES.COMMANDCENTER;
                case "FACTORY":
                    return BUILDINGTYPES.FACTORY;
                case "FOUNDATION":
                    return BUILDINGTYPES.FOUNDATION;
                case "LAB":
                    return BUILDINGTYPES.LAB;
                case "MINE":
                    return BUILDINGTYPES.MINE;
                case "REFINORY":
                    return BUILDINGTYPES.REFINORY;
                case "TURRET":
                    return BUILDINGTYPES.TURRET;
            }
            return BUILDINGTYPES.NONE;
        }
        /// <summary>
        /// Converts ACTIONS to string value
        /// </summary>
        /// <param name="at">ACTIONS value</param>
        /// <returns>string</returns>
        public static string convertActionType(ACTIONS at)
        {
            switch (at)
            {
                case ACTIONS.BEAM:
                    return "BEAM";
                case ACTIONS.BOMB:
                    return "BOMB";
                case ACTIONS.BUFF:
                    return "BUFF";
                case ACTIONS.BUILD:
                    return "BUILD";
                case ACTIONS.BULLET:
                    return "BULLET";
                case ACTIONS.DEBUFF:
                    return "DEBUFF";
                case ACTIONS.HARVEST:
                    return "HARVEST";
                case ACTIONS.HEAL:
                    return "HEAL";
                case ACTIONS.MELEE:
                    return "MELEE";
                case ACTIONS.MISSILE:
                    return "MISSILE";
                case ACTIONS.REPAIR:
                    return "REPAIR";
            }
            return "NONE";
        }
        /// <summary>
        /// Converts string to ACTIONS value
        /// </summary>
        /// <param name="at">String value</param>
        /// <returns>ACTIONS</returns>
        public static ACTIONS convertActionType(string at)
        {
            switch (at)
            {
                case "BEAM":
                    return ACTIONS.BEAM;
                case "BOMB":
                    return ACTIONS.BOMB;
                case "BUFF":
                    return ACTIONS.BUFF;
                case "BUILD":
                    return ACTIONS.BUILD;
                case "BULLET":
                    return ACTIONS.BULLET;
                case "DEBUFF":
                    return ACTIONS.DEBUFF;
                case "HARVEST":
                    return ACTIONS.HARVEST;
                case "HEAL":
                    return ACTIONS.HEAL;
                case "MELEE":
                    return ACTIONS.MELEE;
                case "MISSILE":
                    return ACTIONS.MISSILE;
                case "REPAIR":
                    return ACTIONS.REPAIR;
            }
            return ACTIONS.NONE;
        }
        /// <summary>
        /// Converts OBJECTTYPE to string value
        /// </summary>
        /// <param name="ot">OBJECTTYPE value</param>
        /// <returns>string</returns>
        public static string convertObjectType(OBJECTTYPE ot)
        {
            switch (ot)
            {
                case OBJECTTYPE.ot_action:
                    return "ACTION";
                case OBJECTTYPE.ot_building:
                    return "BUILDING";
                case OBJECTTYPE.ot_effect:
                    return "EFFECT";
                case OBJECTTYPE.ot_unit:
                    return "UNIT";
            }
            return "NONE";
        }
        /// <summary>
        /// Converts string to OBJECTTYPE value
        /// </summary>
        /// <param name="ot">String value</param>
        /// <returns>OBJECTTYPE</returns>
        public static OBJECTTYPE convertObjectType(string ot)
        {
            switch (ot)
            {
                case "ACTION":
                    return OBJECTTYPE.ot_action;
                case "BUILDING":
                    return OBJECTTYPE.ot_building;
                case "EFFECT":
                    return OBJECTTYPE.ot_effect;
                case "UNIT":
                    return OBJECTTYPE.ot_unit;
            }
            return OBJECTTYPE.ot_none;
        }
        /// <summary>
        /// Converts RTSOBJECTIVETYPE to string value
        /// </summary>
        /// <param name="ojt">RTSOBJECTIVETYPE value</param>
        /// <returns>string</returns>
        public static string convertObjectiveType(RTSOBJECTIVETYPE ojt)
        {
            switch (ojt)
            {
                case RTSOBJECTIVETYPE.ot_attack:
                    return "ACTTACK";
                case RTSOBJECTIVETYPE.ot_buff:
                    return "BUFF";
                case RTSOBJECTIVETYPE.ot_build:
                    return "BUILD";
                case RTSOBJECTIVETYPE.ot_debuff:
                    return "DEBUFF";
                case RTSOBJECTIVETYPE.ot_flee:
                    return "FLEE";
                case RTSOBJECTIVETYPE.ot_harvest:
                    return "HARVEST";
                case RTSOBJECTIVETYPE.ot_heal:
                    return "HEAL";
                case RTSOBJECTIVETYPE.ot_move:
                    return "MOVE";
                case RTSOBJECTIVETYPE.ot_refine:
                    return "REFINE";
                case RTSOBJECTIVETYPE.ot_repair:
                    return "REPAIR";
                case RTSOBJECTIVETYPE.ot_shoot:
                    return "SHOOT";
                case RTSOBJECTIVETYPE.ot_special:
                    return "SPECIAL";
                case RTSOBJECTIVETYPE.ot_train:
                    return "TRAIN";
            }
            return "NONE";
        }
        /// <summary>
        /// Converts string to OBJECTIVETYPE value
        /// </summary>
        /// <param name="ot">String value</param>
        /// <returns>OBJECTIVETYPE</returns>
        public static RTSOBJECTIVETYPE convertObjectiveType(string ojt)
        {
            switch (ojt)
            {
                case "ACTTACK":
                    return RTSOBJECTIVETYPE.ot_attack;
                case "BUFF":
                    return RTSOBJECTIVETYPE.ot_buff;
                case "BUILD":
                    return RTSOBJECTIVETYPE.ot_build;
                case "DEBUFF":
                    return RTSOBJECTIVETYPE.ot_debuff;
                case "FLEE":
                    return RTSOBJECTIVETYPE.ot_flee;
                case "HARVEST":
                    return RTSOBJECTIVETYPE.ot_harvest;
                case "HEAL":
                    return RTSOBJECTIVETYPE.ot_heal;
                case "MOVE":
                    return RTSOBJECTIVETYPE.ot_move;
                case "REFINE":
                    return RTSOBJECTIVETYPE.ot_refine;
                case "REPAIR":
                    return RTSOBJECTIVETYPE.ot_repair;
                case "SHOOT":
                    return RTSOBJECTIVETYPE.ot_shoot;
                case "SPECIAL":
                    return RTSOBJECTIVETYPE.ot_special;
                case "TRAIN":
                    return RTSOBJECTIVETYPE.ot_train;
            }
            return RTSOBJECTIVETYPE.ot_none;
        }
        /// <summary>
        /// Converts TURRETTYPE to string value
        /// </summary>
        /// <param name="tt">TURRETTYPE value</param>
        /// <returns>string</returns>
        public static string convertTurretType(TURRETTYPES tt)
        {
            switch (tt)
            {
                case TURRETTYPES.FIXED:
                    return "FIXED";
                case TURRETTYPES.FLIPS:
                    return "FLIPS";
                case TURRETTYPES.ORBITS:
                    return "ORBITS";
            }
            return "NONE";
        }
        /// <summary>
        /// Converts string to TURRETTYPES value
        /// </summary>
        /// <param name="tt">String value</param>
        /// <returns>TURRETTYPES</returns>
        public static TURRETTYPES convertTurretType(string tt)
        {
            switch (tt)
            {
                case "FIXED":
                    return TURRETTYPES.FIXED;
                case "FLIPS":
                    return TURRETTYPES.FLIPS;
                case "ORBITS":
                    return TURRETTYPES.ORBITS;
            }
            return TURRETTYPES.NONE;
        }
        /// <summary>
        /// Converts ABILITIES to string value
        /// </summary>
        /// <param name="at">ABILITIES value</param>
        /// <returns>string</returns>
        public static string convertAbilityType(ABILITIES at)
        {
            switch (at)
            {
                case ABILITIES.BOMBARD:
                    return "BOMBARD";
                case ABILITIES.PASSIVE:
                    return "PASSIVE";
                case ABILITIES.SELF:
                    return "SELF";
                case ABILITIES.SHOOT:
                    return "SHOOT";
                case ABILITIES.TARGET:
                    return "TARGET";
                case ABILITIES.TRAIN:
                    return "TRAIN";
                case ABILITIES.UPGRADE:
                    return "UPGRADE";
            }
            return "NONE";
        }
        /// <summary>
        /// Converts string to ABILITIES value
        /// </summary>
        /// <param name="at">String value</param>
        /// <returns>ABILITIES</returns>
        public static ABILITIES convertAbilityType(string at)
        {
            switch (at)
            {
                case "BOMBARD":
                    return ABILITIES.BOMBARD;
                case "PASSIVE":
                    return ABILITIES.PASSIVE;
                case "SELF":
                    return ABILITIES.SELF;
                case "SHOOT":
                    return ABILITIES.SHOOT;
                case "TARGET":
                    return ABILITIES.TARGET;
                case "TRAIN":
                    return ABILITIES.TRAIN;
                case "UPGRADE":
                    return ABILITIES.UPGRADE;
            }
            return ABILITIES.NONE;
        }
        /// <summary>
        /// Converts TARGETS to string value
        /// </summary>
        /// <param name="tar">TARGETS value</param>
        /// <returns>string</returns>
        public static string convertTargetType(TARGETS tar)
        {
            switch (tar)
            {
                case TARGETS.AIR:
                    return "AIR";
                case TARGETS.BUILDING:
                    return "BUILDING";
                case TARGETS.GROUND:
                    return "GROUND";
                case TARGETS.RESOURCE:
                    return "RESOURCE";
                case TARGETS.TILE:
                    return "TILE";
            }
            return "NONE";
        }
        /// <summary>
        /// Converts string to TARGETS value
        /// </summary>
        /// <param name="tar">String value</param>
        /// <returns>TARGETS</returns>
        public static TARGETS convertTargetType(string tar)
        {
            switch (tar)
            {
                case "AIR":
                    return TARGETS.AIR;
                case "BUILDING":
                    return TARGETS.BUILDING;
                case "GROUND":
                    return TARGETS.GROUND;
                case "RESOURCE":
                    return TARGETS.RESOURCE;
                case "TILE":
                    return TARGETS.TILE;
            }
            return TARGETS.NONE;
        }
    }
}
