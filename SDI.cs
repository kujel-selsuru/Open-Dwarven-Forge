//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// InputState enumeration
    /// </summary>
    public enum inputState { start = 0, back, du, dr, dd, dl, lb, lt, lu, lr, ld, ll, rb, rt, ru, rr, rd, rl, a, b, x, y, none }

    /// <summary>
    /// a class to handle simple input from a keyboard or gamepad
    /// </summary>
    public class SDI //SimpleDuelInput
    {
        //protected
        //protected SimpleInput gamepad;
        protected inputState gps;
        protected Counter delay;

        //public
        /// <summary>
        /// SDI constructor
        /// </summary>
        /// <param name="ticks">Input delay value</param>
        public SDI(int ticks = 6)
        {
            //gamepad = new SimpleInput(ticks);
            delay = new Counter(ticks);
        }
        /// <summary>
        /// updates internal state
        /// </summary>
        /// <returns>InputState enumeration</returns>
        public inputState update()
        {
            //gamepad input
            //gps = gamepad.update();
            //if (gps != inputState.none)
            //{
            //    return gps;
            //}
            //else
            //{
            if (delay.tick())
            {
                //left stick on keyboard
                if(KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_w))
                {
                    return inputState.lu;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_s))
                {
                    return inputState.ld;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_a))
                {
                    return inputState.ll;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_d))
                {
                    return inputState.lr;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_UP))//right stick on keyboard
                {
                    return inputState.ru;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_DOWN))
                {
                    return inputState.rd;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_RIGHT))
                {
                    return inputState.rl;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_LEFT))
                {
                    return inputState.rr;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_i))//buttons on keyboard
                {
                    return inputState.y;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_j))
                {
                    return inputState.x;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_k))
                {
                    return inputState.a;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_l))
                {
                    return inputState.b;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_q))//triggers and bumpers on keyboard
                {
                    return inputState.lt;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_e))
                {
                    return inputState.lb;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_DELETE))
                {
                    return inputState.rb;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_PAGEDOWN))
                {
                    return inputState.rt;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_ESCAPE))//back and start on keyboard
                {
                    return inputState.back;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_F8))
                {
                    return inputState.start;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_1))//D pad on keyboard
                {
                    return inputState.du;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_2))
                {
                    return inputState.dr;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_3))
                {
                    return inputState.dd;
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_4))
                {
                    return inputState.dl;
                }
                else
                {
                    return inputState.none;
                }
                //}
            }
            return inputState.none;
        }
        /*
        public Point2 LeftStick
        {
            get { return GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One).ThumbSticks.Left; }
        }
        public Point2 RightStick
        {
            get { return GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One).ThumbSticks.Right; }
        }
        public bool GamePadConnected
        {
            get
            {
                if (GamePad.GetState(PlayerIndex.One).IsConnected)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        */
    }
    public class SDI2 //SimpleDuelInput2
    {
        //protected SimpleInput gamepad;
        protected inputState gps;
        protected Counter delay;
        protected KeyBinder keyBinder;

        /// <summary>
        /// SDI2 constructor
        /// </summary>
        /// <param name="ticks">Inputdelay value</param>
        public SDI2(int ticks = 6)
        {
            //gamepad = new SimpleInput(ticks);
            delay = new Counter(ticks);
            keyBinder = new KeyBinder();
        }
        /// <summary>
        /// updates internal state
        /// </summary>
        /// <returns>InputState enumeration</returns>
        public inputState update()
        {
            //gamepad input
            //gps = gamepad.update();
            //if (gps != inputState.none)
            //{
            //    return gps;
            //}
            //else
            //{
            if (delay.tick())
            {
                //left stick on keyboard
                if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_w))
                {
                    return keyBinder.processKey(inputState.lu);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_s))
                {
                    return keyBinder.processKey(inputState.ld);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_a))
                {
                    return keyBinder.processKey(inputState.ll);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_d))
                {
                    return keyBinder.processKey(inputState.lr);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_UP))//right stick on keyboard
                {
                    return keyBinder.processKey(inputState.ru);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_DOWN))
                {
                    return keyBinder.processKey(inputState.rd);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_LEFT))
                {
                    return keyBinder.processKey(inputState.rl);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_RIGHT))
                {
                    return keyBinder.processKey(inputState.rr);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_i))//buttons on keyboard
                {
                    return keyBinder.processKey(inputState.y);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_j))
                {
                    return keyBinder.processKey(inputState.x);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_k))
                {
                    return keyBinder.processKey(inputState.a);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_l))
                {
                    return keyBinder.processKey(inputState.b);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_q))//triggers and bumpers on keyboard
                {
                    return keyBinder.processKey(inputState.lt);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_e))
                {
                    return keyBinder.processKey(inputState.lb);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_DELETE))
                {
                    return keyBinder.processKey(inputState.rb);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_PAGEDOWN))
                {
                    return keyBinder.processKey(inputState.rt);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_ESCAPE))//back and start on keyboard
                {
                    return keyBinder.processKey(inputState.back);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_F8))
                {
                    return keyBinder.processKey(inputState.start);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_1))//D pad on keyboard
                {
                    return keyBinder.processKey(inputState.du);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_2))
                {
                    return keyBinder.processKey(inputState.dr);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_3))
                {
                    return keyBinder.processKey(inputState.dd);
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_4))
                {
                    return keyBinder.processKey(inputState.dl);
                }
                else
                {
                    return keyBinder.processKey(inputState.none);
                }
            }
            //}
            return inputState.none;
        }
        /*
        public Vector2 LeftStick
        {
            get { return GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One).ThumbSticks.Left; }
        }
        public Vector2 RightStick
        {
            get { return GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One).ThumbSticks.Right; }
        }
        public KeyboardState KBS
        {
            get { return kbs; }
        }
        public bool GamePadConnected
        {
            get
            {
                if (GamePad.GetState(PlayerIndex.One).IsConnected)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        */
        public KeyBinder KB
        {
            get { return keyBinder; }
        }
    }
}
