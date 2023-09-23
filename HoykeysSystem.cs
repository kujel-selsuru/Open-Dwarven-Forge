//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;

namespace XenoLib
{
    /// <summary>
    /// HotKeys enumeration
    /// </summary>
    public enum hotkeys { a = 0, s, z, x, c, v, b, n, m, k, l, none }

    /// <summary>
    /// SimpleHotKeys class
    /// </summary>
    public class SimpleHotkeys
    {
        //protected
        protected CoolDown delay;

        //public
        /// <summary>
        /// SimpleHotKeys constructor
        /// </summary>
        /// <param name="delay"></param>
        public SimpleHotkeys(int delay = 7)
        {
            this.delay = new CoolDown(delay);
        }
        /// <summary>
        /// Updates internal state and returns value of hokey pressed if any
        /// </summary>
        /// <param name="eve"></param>
        /// <returns></returns>
        public hotkeys update()
        {
            delay.update();
            if (!delay.Active)
            {
                if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_a))
                {
                    delay.activate();
                    return hotkeys.a;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_s))
                {
                    delay.activate();
                    return hotkeys.s;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_z))
                {
                    delay.activate();
                    return hotkeys.z;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_x))
                {
                    delay.activate();
                    return hotkeys.x;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_c))
                {
                    delay.activate();
                    return hotkeys.c;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_v))
                {
                    delay.activate();
                    return hotkeys.v;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_b))
                {
                    delay.activate();
                    return hotkeys.b;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_n))
                {
                    delay.activate();
                    return hotkeys.n;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_m))
                {
                    delay.activate();
                    return hotkeys.m;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_k))
                {
                    delay.activate();
                    return hotkeys.k;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_l))
                {
                    delay.activate();
                    return hotkeys.l;
                }
            }
            return hotkeys.none;
        }
    }
}
