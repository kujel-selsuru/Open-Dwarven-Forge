//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;
//using Microsoft.Xna.Framework.Input;

namespace XenoLib
{
    /// <summary>
    /// DuelInput class
    /// </summary>
    public class DuelInput
    {
        //protected
        //protected SimpleInput simp;
        protected SDL.SDL_Event eve;
        protected Counter ticker;

        //public
        /// <summary>
        /// DuelInput constructor
        /// </summary>
        /// <param name="delay">Input delay value</param>
        public DuelInput(int delay = 2)
        {
            //simp = new SimpleInput(delay);
            ticker = new Counter(delay);
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        /// <returns>InputState enumeration</returns>
        public inputState update()
        {
            //inputState temp = simp.update();
            //if (temp != inputState.none)
            //{
            //    return temp;
            //}
            //else
            //{
            if (ticker.tick())
            {
                if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_a))
                {
                    return inputState.ll;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_s))
                {
                    return inputState.ld;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_d))
                {
                    return inputState.lr;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_w))
                {
                    return inputState.lu;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_LSHIFT))
                {
                    return inputState.lb;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_LCTRL))
                {
                    return inputState.lt;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_KP_1))
                {
                    return inputState.du;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_KP_2))
                {
                    return inputState.dr;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_KP_3))
                {
                    return inputState.dd;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_KP_4))
                {
                    return inputState.dl;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_RIGHT))
                {
                    return inputState.rr;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_DOWN))
                {
                    return inputState.rd;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_LEFT))
                {
                    return inputState.rl;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_UP))
                {
                    return inputState.ru;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_BACKSPACE))
                {
                    return inputState.y;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_RSHIFT))
                {
                    return inputState.b;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_RCTRL))
                {
                    return inputState.x;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_RETURN))
                {
                    return inputState.a;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_KP_7))
                {
                    return inputState.rb;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_KP_4))
                {
                    return inputState.rt;
                }
            }
            return inputState.none;
        }
    }
}
