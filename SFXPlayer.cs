//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Threading;
using WMPLib;
using SDL2;
//using NAudio;
//using Microsoft.Xna.Framework.Audio;

namespace XenoLib
{
    /// <summary>
    /// SFXPlayer class
    /// </summary>
    public static class SFXPlayer
    {
        //private
        static List<Thread> threads;
        static int volume;
        //public
        /// <summary>
        /// SFXPlayer constructor
        /// </summary>
        static SFXPlayer()
        {
            threads = new List<Thread>();
            volume = 10;
        }
        /// <summary>
        /// Manually initicializes SFXPlayer
        /// </summary>
        public static void init()
        {
            threads = new List<Thread>();
            volume = 10;
        }
        /// <summary>
        /// updates SoundPlayer's internal state
        /// </summary>
        public static void update()
        {
            for(int i = 0; i < threads.Count; i++)
            {
                if(threads[i].IsAlive == false)
                {
                    threads.RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// Play a sound provided a file path
        /// </summary>
        /// <param name="path">Sound filePath</param>
        public static void play(string path)
        {
            if (path != "" && path != " " && path != null)
            {
                WindowsMediaPlayer player = new WindowsMediaPlayer();
                player.URL = path;
                player.settings.volume = volume;
                Thread thread = new Thread(delegate () { player.controls.play(); });
                thread.Start();
                threads.Add(thread);
            }
        }
        /// <summary>
        /// Volume property
        /// </summary>
        public static int Volume
        {
            get { return volume; }
            set { volume = value; }
        }
    }
}
