//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace XenoLib
{
    public static class MouseHandler
    {
        //private
        static int mouseX;
        static int mouseY;
        static bool leftButton;
        static bool middleButton;
        static bool rightButton;

        //public
        /// <summary>
        /// MouseHandler constructor
        /// </summary>
        static MouseHandler()
        {
            mouseX = 0;
            mouseY = 0;
            leftButton = false;
            middleButton = false;
            rightButton = false;
        }
        /// <summary>
        /// Updates the state of mouse position and mouse buttons
        /// Must be called each update
        /// </summary>
        /// <param name="eve">SDL_Event</param>
        public static void update(SDL.SDL_Event eve)
        {
            if(eve.type == SDL.SDL_EventType.SDL_MOUSEMOTION)
            {
                mouseX = eve.motion.x;
                mouseY = eve.motion.y;
            }
            if(eve.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
            {
                if(eve.button.button == SDL.SDL_BUTTON_LEFT)
                {
                    leftButton = true;
                }
                if (eve.button.button == SDL.SDL_BUTTON_MIDDLE)
                {
                    middleButton = true;
                }
                if (eve.button.button == SDL.SDL_BUTTON_RIGHT)
                {
                    rightButton = true;
                }
            }
            if (eve.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP)
            {
                if (eve.button.button == SDL.SDL_BUTTON_LEFT)
                {
                    leftButton = false;
                }
                if (eve.button.button == SDL.SDL_BUTTON_MIDDLE)
                {
                    middleButton = false;
                }
                if (eve.button.button == SDL.SDL_BUTTON_RIGHT)
                {
                    rightButton = false;
                }
            }
        }
        /// <summary>
        /// Returns the mouseX value
        /// </summary>
        /// <returns>Int</returns>
        public static int getMouseX()
        {
            return mouseX;
        }
        /// <summary>
        /// Returns the mouseY value
        /// </summary>
        /// <returns>Int</returns>
        public static int getMouseY()
        {
            return mouseY;
        }
        /// <summary>
        /// Returns the left mouse button state
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool getLeft()
        {
            return leftButton;
        }
        /// <summary>
        /// Returns the middle mouse button state
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool getMiddle()
        {
            return middleButton;
        }
        /// <summary>
        /// Returns the right mouse button state
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool getRight()
        {
            return rightButton;
        }
        /// <summary>
        /// Returns the mouse X and Y as a Point2D object
        /// </summary>
        /// <returns></returns>
        public static Point2D getTip()
        {
            return new Point2D(mouseX, mouseY);
        }
    }
}
