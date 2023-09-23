//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using WMPLib;
using SDL2;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Media;

namespace XenoLib
{
    /// <summary>
    /// MusicEngine class (never worked correctly)
    /// </summary>
    public static class MusicEngine
    {
        //private
        static bool randomize;
        static bool repeat;
        static Random rand;
        static bool musicOn;
        static int curSongIndex;
        static IntPtr song;
        static int volume;

        //public
        /// <summary>
        /// MusicEngine constructor
        /// </summary>
        static MusicEngine()
        {
            rand = new Random((int)System.DateTime.Today.Ticks);
            musicOn = false;
            curSongIndex = 0;
            song = default(IntPtr);
            volume = 10;
            randomize = false;
            repeat = false;
        }
        /// <summary>
        /// Play a specified song
        /// </summary>
        /// <param name="songName">Song name/key</param>
        /// <param name="vol">Volume level to play at</param>
        public static void playSong(string songName, int vol = 10)
        {
            if (musicOn)
            {
                SDL_mixer.Mix_HaltMusic();
                SDL_mixer.Mix_FreeMusic(song);
                song = SDL_mixer.Mix_LoadMUS(songName);
                SDL_mixer.Mix_VolumeMusic(vol);
                int tmp = SDL_mixer.Mix_PlayMusic(song, 0);
                volume = vol;
            }
        }
        /// <summary>
        /// Stop playing current song
        /// </summary>
        public static void stopSong()
        {
            SDL_mixer.Mix_HaltMusic();
        }
        /// <summary>
        /// Pause current playing song
        /// </summary>
        public static void pauseMusic()
        {
            SDL_mixer.Mix_PauseMusic();
        }
        /// <summary>
        /// Update MusicEngine internal state
        /// </summary>
        public static void update()
        {
            if (musicOn)
            {
                if (SDL_mixer.Mix_PlayingMusic() < 1)
                {
                    if (!randomize)
                    {
                        if (song != default(IntPtr))
                        {
                            SDL_mixer.Mix_FreeMusic(song);
                        }
                        song = SDL_mixer.Mix_LoadMUS(MP3Bank.getSong(curSongIndex));
                        if (repeat)
                        {
                            SDL_mixer.Mix_PlayMusic(song, -1);
                        }
                        else
                        {
                            SDL_mixer.Mix_PlayMusic(song, 1);
                        }
                        curSongIndex++;
                        if (curSongIndex >= MP3Bank.Count - 1)
                        {
                            curSongIndex = 0;
                        }
                    }
                    else
                    {
                        if (song != default(IntPtr))
                        {
                            SDL_mixer.Mix_FreeMusic(song);
                        }
                        song = SDL_mixer.Mix_LoadMUS(MP3Bank.Names[rand.Next(0, MP3Bank.Count - 1)]);
                        if (repeat)
                        {
                            SDL_mixer.Mix_PlayMusic(song, -1);
                        }
                        else
                        {
                            SDL_mixer.Mix_PlayMusic(song, 1);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Start music
        /// </summary>
        public static void startMusic()
        {
            if (musicOn)
            {
                if (song != default(IntPtr))
                {
                    if (repeat)
                    {
                        SDL_mixer.Mix_PlayMusic(song, -1);
                    }
                    else
                    {
                        SDL_mixer.Mix_PlayMusic(song, 1);
                    }
                }
            }
        }
        /// <summary>
        /// set the current selected song
        /// </summary>
        /// <param name="name">Name of song to set</param>
        public static void setSong(string name)
        {
            stopSong();
            SDL_mixer.Mix_FreeMusic(song);
            song = SDL_mixer.Mix_LoadMUS(MP3Bank.getSong(name));
        }
        /// <summary>
        /// Volume property
        /// </summary>
        public static int Volume
        {
            get { return volume; }
            set {
                    SDL_mixer.Mix_VolumeMusic(value);
                    volume = value;
                }
        }
        /// <summary>
        /// Isplaying property
        /// </summary>
        public static int IsPlaying
        {
            get { return SDL_mixer.Mix_PlayingMusic(); }
        }
        /// <summary>
        /// Repeat property
        /// </summary>
        public static bool Repeat
        {
            get { return repeat; }
            set { repeat = value; }
        }
        /// <summary>
        /// Randomize property
        /// </summary>
        public static bool Randomize
        {
            get { return randomize; }
            set { randomize = value; }
        }
        /// <summary>
        /// MusicOn property
        /// </summary>
        public static bool MusicOn
        {
            get { return musicOn; }
            set { musicOn = value; }
        }
    }
    /// <summary>
    /// Plays wave files for music
    /// </summary>
    public static class MusicPlayer
    {
        static WindowsMediaPlayer player;
        static bool isStopped;
        static string songPath;
        static bool musicOn;
        static bool randomize;
        static Random rand;
        static int songIndex;
        static int volume;
        //static int playStateChange;
        //static int newState;

        /// <summary>
        /// MusicPlayer constructor
        /// </summary>
        static MusicPlayer()
        {
            player = new WindowsMediaPlayer();
            isStopped = true;
            songIndex = 0;
            songPath = "";
            musicOn = false;
            rand = new Random((int)System.DateTime.Today.Ticks);
            randomize = false;
            //playStateChange = 0;
            //newState = 0;
        }
        /// <summary>
        /// Updates internal state of MusicPlayer
        /// </summary>
        public static void update()
        {
            if (musicOn == true)
            {
                if (randomize == true)
                {
                    if (isStopped == true)
                    {
                        songPath = MP3Bank.getSong(rand.Next(MP3Bank.Count - 1));
                        playSong();
                    }
                }
                else
                {
                    if (isStopped == true)
                    {
                        songPath = MP3Bank.getSong(songIndex);
                        if (songIndex < MP3Bank.Count - 1)
                        {
                            songIndex++;
                        }
                        else
                        {
                            songIndex = 0;
                        }
                        playSong();
                    }
                }
            }
        }
        /// <summary>
        /// Plays current set song
        /// </summary>
        private static void playSong()
        {
            if (songPath != "" && songPath != " " && songPath != null)
            {
                isStopped = false;
                player.URL = songPath;
                player.settings.volume = volume;
                player.PlayStateChange += new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(Player_PlayStateChange);
                player.controls.play();
            }
        }
        /// <summary>
        /// Plays a specified mp3 file at file path
        /// </summary>
        /// <param name="song">File path of song to play</param>
        public static void playSong(string song)
        {
            if (song != "" && song != " " && song != null)
            {
                isStopped = false;
                player.URL = song;
                player.settings.volume = volume;
                player.PlayStateChange += new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(Player_PlayStateChange);
                player.controls.play();
            }
        }
        /// <summary>
        /// Sets isStopped when end of mp3
        /// </summary>
        /// <param name="NewState"></param>
        private static void Player_PlayStateChange(int NewState)
        {
            if ((WMPLib.WMPPlayState)NewState == WMPLib.WMPPlayState.wmppsStopped)
            {
                isStopped = true;
            }
        }
        /// <summary>
        /// Stops current song playing
        /// </summary>
        public static void stopMusic()
        {
            player.controls.stop();
            isStopped = true;
        }
        /// <summary>
        /// Returns true if song playing else returns false
        /// </summary>
        public static bool Playing
        {
            get { return !isStopped; }
        }
        /// <summary>
        /// Randomize property
        /// </summary>
        public static bool Randomize
        {
            get { return randomize; }
            set { randomize = value; }
        }
        /// <summary>
        /// MusicOn property
        /// </summary>
        public static bool MusicOn
        {
            get { return musicOn; }
            set { musicOn = value; }
        }
        /// <summary>
        /// SongIndex property
        /// </summary>
        public static int SongIndex
        {
            get { return songIndex; }
            set { songIndex = value; }
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

    /// <summary>
    /// MP3Bank class
    /// </summary>
    public static class MP3Bank
    {
        //private
        static Dictionary<string, string> songs;
        static List<string> names;
        /// <summary>
        /// MP3Bank constructor
        /// </summary>
        static MP3Bank()
        {
            songs = new Dictionary<string, string>();
            names = new List<string>();
        }

        //public
        /// <summary>
        /// Add a song to the MP3Bank
        /// </summary>
        /// <param name="name">Name of song</param>
        /// <param name="song">FileName/Path of song to add</param>
        public static void addSong(string name, string song)
        {
            if (names == null)
            {
                names = new List<string>();
            }
            names.Add(name);
            if (songs == null)
            {
                songs = new Dictionary<string, string>();
            }
            songs.Add(name, song);
        }
        /// <summary>
        /// Return the fileName/path of song by name
        /// </summary>
        /// <param name="song">Song name in MP3Bank</param>
        /// <returns>string</returns>
        public static string getSong(string song)
        {
            string music;
            if (songs.ContainsKey(song))
            {
                songs.TryGetValue(song, out music);
            }
            else
            {
                return null;
            }
            return music;
        }
        /// <summary>
        /// Return the fileName/path of song by index
        /// </summary>
        /// <param name="song">Song name in MP3Bank</param>
        /// <returns>string</returns>
        public static string getSong(int index)
        {
            string name = getName(index);
            return getSong(name);
        }
        /// <summary>
        /// Returns the name of a song at provided index
        /// </summary>
        /// <param name="index">Index of song name</param>
        /// <returns>string</returns>
        public static string getName(int index)
        {
            if (index < songs.Count)
            {
                return names[index];
            }
            return "";
        }
        /// <summary>
        /// Returns the index of a song in the bank provided a name
        /// </summary>
        /// <param name="name">Name of song to get index of</param>
        /// <returns>Int</returns>
        public static int getSongIndex(string name)
        {
            if(names.Contains(name))
            {
                for(int i = 0; i < names.Count - 1; i++)
                {
                    if(names[i] == name)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// Count of songs in MP3Bank
        /// </summary>
        public static int Count
        {
            get { return songs.Count; }
        }
        /// <summary>
        /// Names property
        /// </summary>
        public static List<string> Names
        {
            get { return names; }
        }
    }
    /// <summary>
    /// OGGBank class
    /// </summary>
    public static class OGGBank
    {
        //private
        static Dictionary<string, string> songs;
        static List<string> names;
        /// <summary>
        /// MP3Bank constructor
        /// </summary>
        static OGGBank()
        {
            songs = new Dictionary<string, string>();
            names = new List<string>();
        }

        //public
        /// <summary>
        /// Add a song to the OGGBank
        /// </summary>
        /// <param name="name">Name of song</param>
        /// <param name="song">FileName/Path of song to add</param>
        public static void addSong(string name, string song)
        {
            if (names == null)
            {
                names = new List<string>();
            }
            names.Add(name);
            if (songs == null)
            {
                songs = new Dictionary<string, string>();
            }
            songs.Add(name, song);
        }
        /// <summary>
        /// Return the fileName/path of song by name
        /// </summary>
        /// <param name="song">Song name in OGGBank</param>
        /// <returns>string</returns>
        public static string getSong(string song)
        {
            string music;
            if (songs.ContainsKey(song))
            {
                songs.TryGetValue(song, out music);
            }
            else
            {
                return null;
            }
            return music;
        }
        /// <summary>
        /// Return the fileName/path of song by index
        /// </summary>
        /// <param name="song">Song name in OGGBank</param>
        /// <returns>string</returns>
        public static string getSong(int index)
        {
            string name = getName(index);
            return getSong(name);
        }
        /// <summary>
        /// Returns the name of a song at provided index
        /// </summary>
        /// <param name="index">Index of song name</param>
        /// <returns>string</returns>
        public static string getName(int index)
        {
            if (index < songs.Count)
            {
                return names[index];
            }
            return "";
        }
        /// <summary>
        /// Returns the index of a song in the bank provided a name
        /// </summary>
        /// <param name="name">Name of song to get index of</param>
        /// <returns>Int</returns>
        public static int getSongIndex(string name)
        {
            if (names.Contains(name))
            {
                for (int i = 0; i < names.Count - 1; i++)
                {
                    if (names[i] == name)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// Count of songs in OGGBank
        /// </summary>
        public static int Count
        {
            get { return songs.Count; }
        }
        /// <summary>
        /// Names property
        /// </summary>
        public static List<string> Names
        {
            get { return names; }
        }
    }
    /// <summary>
    /// WaveBank class
    /// </summary>
    public static class WaveBank
    {
        //private
        static Dictionary<string, string> songs;
        static List<string> names;
        /// <summary>
        /// WaveBank constructor
        /// </summary>
        static WaveBank()
        {
            songs = new Dictionary<string, string>();
            names = new List<string>();
        }

        //public
        /// <summary>
        /// Add a song to the WaveBank
        /// </summary>
        /// <param name="name">Name of song</param>
        /// <param name="song">FileName/Path of song to add</param>
        public static void addSong(string name, string song)
        {
            if (names == null)
            {
                names = new List<string>();
            }
            names.Add(name);
            if (songs == null)
            {
                songs = new Dictionary<string, string>();
            }
            songs.Add(name, song);
        }
        /// <summary>
        /// Return the fileName/path of song by name
        /// </summary>
        /// <param name="song">Song name in WaveBank</param>
        /// <returns>string</returns>
        public static string getSong(string song)
        {
            string music;
            if (songs.ContainsKey(song))
            {
                songs.TryGetValue(song, out music);
            }
            else
            {
                return null;
            }
            return music;
        }
        /// <summary>
        /// Return the fileName/path of song by index
        /// </summary>
        /// <param name="song">Song name in WaveBank</param>
        /// <returns>string</returns>
        public static string getSong(int index)
        {
            string name = getName(index);
            return getSong(name);
        }
        /// <summary>
        /// Returns the name of a song at provided index
        /// </summary>
        /// <param name="index">Index of song name</param>
        /// <returns>string</returns>
        public static string getName(int index)
        {
            if (index < songs.Count)
            {
                return names[index];
            }
            return "";
        }
        /// <summary>
        /// Returns the index of a song in the bank provided a name
        /// </summary>
        /// <param name="name">Name of song to get index of</param>
        /// <returns>Int</returns>
        public static int getSongIndex(string name)
        {
            if (names.Contains(name))
            {
                for (int i = 0; i < names.Count - 1; i++)
                {
                    if (names[i] == name)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// Count of songs in WaveBank
        /// </summary>
        public static int Count
        {
            get { return songs.Count; }
        }
        /// <summary>
        /// Names property
        /// </summary>
        public static List<string> Names
        {
            get { return names; }
        }
    }
}
