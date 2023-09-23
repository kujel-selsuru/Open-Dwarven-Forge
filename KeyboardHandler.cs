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
    /// <summary>
    /// KeyboardHandler class
    /// </summary>
    public static class KeyboardHandler
    {
        //private
        static bool[] keyStates;

        //public
        /// <summary>
        /// KeyboardHandler constructor
        /// </summary>
        static KeyboardHandler()
        {
            keyStates = new bool[100];
            for(int i = 0; i < 100; i++)
            {
                keyStates[i] = false;
            }
        }
        /// <summary>
        /// Updates keystates, needs to be called each frame
        /// </summary>
        /// <param name="eve">SDL_Event reference</param>
        public static void update(SDL.SDL_Event eve)
        {
            if (eve.type == SDL.SDL_EventType.SDL_KEYDOWN)
            { 
                switch (eve.key.keysym.sym)
                {
                    case SDL.SDL_Keycode.SDLK_0:
                        keyStates[0] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_1:
                        keyStates[1] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_2:
                        keyStates[2] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_3:
                        keyStates[3] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_4:
                        keyStates[4] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_5:
                        keyStates[5] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_6:
                        keyStates[6] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_7:
                        keyStates[7] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_8:
                        keyStates[8] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_9:
                        keyStates[9] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_a:
                        keyStates[10] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_b:
                        keyStates[11] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_c:
                        keyStates[12] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_d:
                        keyStates[13] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_e:
                        keyStates[14] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_f:
                        keyStates[15] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_g:
                        keyStates[16] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_h:
                        keyStates[17] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_i:
                        keyStates[18] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_j:
                        keyStates[19] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_k:
                        keyStates[20] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_l:
                        keyStates[21] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_m:
                        keyStates[22] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_n:
                        keyStates[23] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_o:
                        keyStates[24] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_p:
                        keyStates[25] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_q:
                        keyStates[26] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_r:
                        keyStates[27] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_s:
                        keyStates[28] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_t:
                        keyStates[29] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_u:
                        keyStates[30] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_v:
                        keyStates[31] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_w:
                        keyStates[32] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_x:
                        keyStates[33] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_y:
                        keyStates[34] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_z:
                        keyStates[35] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_1:
                        keyStates[36] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_2:
                        keyStates[37] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_3:
                        keyStates[38] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_4:
                        keyStates[39] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_5:
                        keyStates[40] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_6:
                        keyStates[41] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_7:
                        keyStates[42] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_8:
                        keyStates[43] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_9:
                        keyStates[44] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_0:
                        keyStates[45] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_PERIOD:
                        keyStates[46] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_COMMA:
                        keyStates[47] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_LEFTBRACKET:
                        keyStates[48] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_RIGHTBRACKET:
                        keyStates[49] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_LSHIFT:
                        keyStates[50] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_RSHIFT:
                        keyStates[51] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_LCTRL:
                        keyStates[52] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_RCTRL:
                        keyStates[53] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_LALT:
                        keyStates[54] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_RALT:
                        keyStates[55] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_SPACE:
                        keyStates[56] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_RETURN:
                        keyStates[57] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_UP:
                        keyStates[58] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_RIGHT:
                        keyStates[59] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_DOWN:
                        keyStates[60] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_LEFT:
                        keyStates[61] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_ESCAPE:
                        keyStates[62] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_TAB:
                        keyStates[63] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_BACKSPACE:
                        keyStates[64] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_MINUS:
                        keyStates[65] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_EQUALS:
                        keyStates[66] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_PAGEUP:
                        keyStates[67] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_PAGEDOWN:
                        keyStates[68] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_F1:
                        keyStates[69] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_F2:
                        keyStates[70] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_F3:
                        keyStates[71] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_F4:
                        keyStates[72] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_F5:
                        keyStates[73] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_F6:
                        keyStates[74] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_F7:
                        keyStates[75] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_F8:
                        keyStates[76] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_F9:
                        keyStates[77] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_F10:
                        keyStates[78] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_F11:
                        keyStates[79] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_F12:
                        keyStates[80] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_SLASH:
                        keyStates[81] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_BACKSLASH:
                        keyStates[82] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_COLON:
                        keyStates[83] = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_QUOTE:
                        keyStates[84] = true;
                        break;
                }
            }
            else if (eve.type == SDL.SDL_EventType.SDL_KEYUP)
            {
                switch (eve.key.keysym.sym)
                {
                    case SDL.SDL_Keycode.SDLK_0:
                        keyStates[0] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_1:
                        keyStates[1] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_2:
                        keyStates[2] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_3:
                        keyStates[3] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_4:
                        keyStates[4] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_5:
                        keyStates[5] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_6:
                        keyStates[6] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_7:
                        keyStates[7] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_8:
                        keyStates[8] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_9:
                        keyStates[9] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_a:
                        keyStates[10] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_b:
                        keyStates[11] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_c:
                        keyStates[12] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_d:
                        keyStates[13] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_e:
                        keyStates[14] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_f:
                        keyStates[15] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_g:
                        keyStates[16] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_h:
                        keyStates[17] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_i:
                        keyStates[18] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_j:
                        keyStates[19] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_k:
                        keyStates[20] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_l:
                        keyStates[21] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_m:
                        keyStates[22] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_n:
                        keyStates[23] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_o:
                        keyStates[24] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_p:
                        keyStates[25] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_q:
                        keyStates[26] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_r:
                        keyStates[27] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_s:
                        keyStates[28] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_t:
                        keyStates[29] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_u:
                        keyStates[30] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_v:
                        keyStates[31] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_w:
                        keyStates[32] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_x:
                        keyStates[33] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_y:
                        keyStates[34] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_z:
                        keyStates[35] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_1:
                        keyStates[36] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_2:
                        keyStates[37] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_3:
                        keyStates[38] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_4:
                        keyStates[39] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_5:
                        keyStates[40] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_6:
                        keyStates[41] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_7:
                        keyStates[42] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_8:
                        keyStates[43] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_9:
                        keyStates[44] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_0:
                        keyStates[45] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_PERIOD:
                        keyStates[46] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_KP_COMMA:
                        keyStates[47] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_LEFTBRACKET:
                        keyStates[48] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_RIGHTBRACKET:
                        keyStates[49] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_LSHIFT:
                        keyStates[50] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_RSHIFT:
                        keyStates[51] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_LCTRL:
                        keyStates[52] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_RCTRL:
                        keyStates[53] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_LALT:
                        keyStates[54] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_RALT:
                        keyStates[55] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_SPACE:
                        keyStates[56] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_RETURN:
                        keyStates[57] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_UP:
                        keyStates[58] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_RIGHT:
                        keyStates[59] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_DOWN:
                        keyStates[60] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_LEFT:
                        keyStates[61] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_ESCAPE:
                        keyStates[62] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_TAB:
                        keyStates[63] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_BACKSPACE:
                        keyStates[64] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_MINUS:
                        keyStates[65] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_EQUALS:
                        keyStates[66] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_PAGEUP:
                        keyStates[67] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_PAGEDOWN:
                        keyStates[68] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_F1:
                        keyStates[69] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_F2:
                        keyStates[70] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_F3:
                        keyStates[71] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_F4:
                        keyStates[72] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_F5:
                        keyStates[73] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_F6:
                        keyStates[74] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_F7:
                        keyStates[75] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_F8:
                        keyStates[76] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_F9:
                        keyStates[77] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_F10:
                        keyStates[78] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_F11:
                        keyStates[79] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_F12:
                        keyStates[80] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_SLASH:
                        keyStates[81] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_BACKSLASH:
                        keyStates[82] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_COLON:
                        keyStates[83] = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_QUOTE:
                        keyStates[84] = false;
                        break;
                }
            }
        }
        /// <summary>
        /// Returns the current state of each listed key
        /// </summary>
        /// <param name="code">SDL_Keycode</param>
        /// <returns>Boolean</returns>
        public static bool getKeyState(SDL.SDL_Keycode code)
        {
            switch (code)
            {
                case SDL.SDL_Keycode.SDLK_0:
                    return keyStates[0];
                case SDL.SDL_Keycode.SDLK_1:
                    return keyStates[1];
                case SDL.SDL_Keycode.SDLK_2:
                    return keyStates[2];
                case SDL.SDL_Keycode.SDLK_3:
                    return keyStates[3];
                case SDL.SDL_Keycode.SDLK_4:
                    return keyStates[4];
                case SDL.SDL_Keycode.SDLK_5:
                    return keyStates[5];
                case SDL.SDL_Keycode.SDLK_6:
                    return keyStates[6];
                case SDL.SDL_Keycode.SDLK_7:
                    return keyStates[7];
                case SDL.SDL_Keycode.SDLK_8:
                    return keyStates[8];
                case SDL.SDL_Keycode.SDLK_9:
                    return keyStates[9];
                case SDL.SDL_Keycode.SDLK_a:
                    return keyStates[10];
                case SDL.SDL_Keycode.SDLK_b:
                    return keyStates[11];
                case SDL.SDL_Keycode.SDLK_c:
                    return keyStates[12];
                case SDL.SDL_Keycode.SDLK_d:
                    return keyStates[13];
                case SDL.SDL_Keycode.SDLK_e:
                    return keyStates[14];
                case SDL.SDL_Keycode.SDLK_f:
                    return keyStates[15];
                case SDL.SDL_Keycode.SDLK_g:
                    return keyStates[16];
                case SDL.SDL_Keycode.SDLK_h:
                    return keyStates[17];
                case SDL.SDL_Keycode.SDLK_i:
                    return keyStates[18];
                case SDL.SDL_Keycode.SDLK_j:
                    return keyStates[19];
                case SDL.SDL_Keycode.SDLK_k:
                    return keyStates[20];
                case SDL.SDL_Keycode.SDLK_l:
                    return keyStates[21];
                case SDL.SDL_Keycode.SDLK_m:
                    return keyStates[22];
                case SDL.SDL_Keycode.SDLK_n:
                    return keyStates[23];
                case SDL.SDL_Keycode.SDLK_o:
                    return keyStates[24];
                case SDL.SDL_Keycode.SDLK_p:
                    return keyStates[25];
                case SDL.SDL_Keycode.SDLK_q:
                    return keyStates[26];
                case SDL.SDL_Keycode.SDLK_r:
                    return keyStates[27];
                case SDL.SDL_Keycode.SDLK_s:
                    return keyStates[28];
                case SDL.SDL_Keycode.SDLK_t:
                    return keyStates[29];
                case SDL.SDL_Keycode.SDLK_u:
                    return keyStates[30];
                case SDL.SDL_Keycode.SDLK_v:
                    return keyStates[31];
                case SDL.SDL_Keycode.SDLK_w:
                    return keyStates[32];
                case SDL.SDL_Keycode.SDLK_x:
                    return keyStates[33];
                case SDL.SDL_Keycode.SDLK_y:
                    return keyStates[34];
                case SDL.SDL_Keycode.SDLK_z:
                    return keyStates[35];
                case SDL.SDL_Keycode.SDLK_KP_1:
                    return keyStates[36];
                case SDL.SDL_Keycode.SDLK_KP_2:
                    return keyStates[37];
                case SDL.SDL_Keycode.SDLK_KP_3:
                    return keyStates[38];
                case SDL.SDL_Keycode.SDLK_KP_4:
                    return keyStates[39];
                case SDL.SDL_Keycode.SDLK_KP_5:
                    return keyStates[40];
                case SDL.SDL_Keycode.SDLK_KP_6:
                    return keyStates[41];
                case SDL.SDL_Keycode.SDLK_KP_7:
                    return keyStates[42];
                case SDL.SDL_Keycode.SDLK_KP_8:
                    return keyStates[43];
                case SDL.SDL_Keycode.SDLK_KP_9:
                    return keyStates[44];
                case SDL.SDL_Keycode.SDLK_KP_0:
                    return keyStates[45];
                case SDL.SDL_Keycode.SDLK_PERIOD:
                    return keyStates[46];
                case SDL.SDL_Keycode.SDLK_KP_COMMA:
                    return keyStates[47];
                case SDL.SDL_Keycode.SDLK_LEFTBRACKET:
                    return keyStates[48];
                case SDL.SDL_Keycode.SDLK_RIGHTBRACKET:
                    return keyStates[49];
                case SDL.SDL_Keycode.SDLK_LSHIFT:
                    return keyStates[50];
                case SDL.SDL_Keycode.SDLK_RSHIFT:
                    return keyStates[51];
                case SDL.SDL_Keycode.SDLK_LCTRL:
                    return keyStates[52];
                case SDL.SDL_Keycode.SDLK_RCTRL:
                    return keyStates[53];
                case SDL.SDL_Keycode.SDLK_LALT:
                    return keyStates[54];
                case SDL.SDL_Keycode.SDLK_RALT:
                    return keyStates[55];
                case SDL.SDL_Keycode.SDLK_SPACE:
                    return keyStates[56];
                case SDL.SDL_Keycode.SDLK_RETURN:
                    return keyStates[57];
                case SDL.SDL_Keycode.SDLK_UP:
                    return keyStates[58];
                case SDL.SDL_Keycode.SDLK_RIGHT:
                    return keyStates[59];
                case SDL.SDL_Keycode.SDLK_DOWN:
                    return keyStates[60];
                case SDL.SDL_Keycode.SDLK_LEFT:
                    return keyStates[61];
                case SDL.SDL_Keycode.SDLK_ESCAPE:
                    return keyStates[62];
                case SDL.SDL_Keycode.SDLK_TAB:
                    return keyStates[63];
                case SDL.SDL_Keycode.SDLK_BACKSPACE:
                    return keyStates[64];
                case SDL.SDL_Keycode.SDLK_MINUS:
                    return keyStates[65];
                case SDL.SDL_Keycode.SDLK_EQUALS:
                    return keyStates[66];
                case SDL.SDL_Keycode.SDLK_PAGEUP:
                    return keyStates[67];
                case SDL.SDL_Keycode.SDLK_PAGEDOWN:
                    return keyStates[68];
                case SDL.SDL_Keycode.SDLK_F1:
                    return keyStates[69];
                case SDL.SDL_Keycode.SDLK_F2:
                    return keyStates[70];
                case SDL.SDL_Keycode.SDLK_F3:
                    return keyStates[71];
                case SDL.SDL_Keycode.SDLK_F4:
                    return keyStates[72];
                case SDL.SDL_Keycode.SDLK_F5:
                    return keyStates[73];
                case SDL.SDL_Keycode.SDLK_F6:
                    return keyStates[74];
                case SDL.SDL_Keycode.SDLK_F7:
                    return keyStates[75];
                case SDL.SDL_Keycode.SDLK_F8:
                    return keyStates[76];
                case SDL.SDL_Keycode.SDLK_F9:
                    return keyStates[77];
                case SDL.SDL_Keycode.SDLK_F10:
                    return keyStates[78];
                case SDL.SDL_Keycode.SDLK_F11:
                    return keyStates[79];
                case SDL.SDL_Keycode.SDLK_F12:
                    return keyStates[80];
                case SDL.SDL_Keycode.SDLK_SLASH:
                    return keyStates[81];
                case SDL.SDL_Keycode.SDLK_BACKSLASH:
                    return keyStates[82];
                case SDL.SDL_Keycode.SDLK_COLON:
                    return keyStates[83];
                case SDL.SDL_Keycode.SDLK_QUOTE:
                    return keyStates[84];
            }
            return false;
        }
    }
}
