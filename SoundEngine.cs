//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrrKlang;

namespace XenoLib
{
    public static class SoundEngine
    {
        //private
        static ISoundEngine soundEng;
        //public
        /// <summary>
        /// SoundEngine constructor
        /// </summary>
        static SoundEngine()
        {
            soundEng = new ISoundEngine();
        }
        /// <summary>
        /// Plays a specified wave file provided a path
        /// </summary>
        /// <param name="path">Wave file path</param>
        public static void playSFX(string path)
        {
            soundEng.Play2D(path, false);
        }
        /// <summary>
        /// Puases all waves playing
        /// </summary>
        /// <param name="puase">Puase or unpuase flag</param>
        public static void puaseSFX(bool puase)
        {
            soundEng.SetAllSoundsPaused(puase);
        }
        /// <summary>
        /// Stops all waves playing
        /// </summary>
        public static void stopSFX()
        {
            soundEng.StopAllSounds();
        }
        /// <summary>
        /// Sets wave volume
        /// </summary>
        /// <param name="volume">Wave volume value</param>
        public static void setVolume(float volume)
        {
            soundEng.SoundVolume = volume;
        }
    }
}
