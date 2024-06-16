using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenoLib
{
    public class ForgePlayer : XenoGame
    {
        //protected

        //public
        /// <summary>
        /// ForgePlayer constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Window width in pixels</param>
        /// <param name="height">Window height in pixels</param>
        /// <param name="gameName">Name of game displayed on title bar</param>
        /// <param name="showCursor">ShowCursor flag value</param>
        public ForgePlayer(int x, int y, int width, int height, string gameName, int showCursor = 0) : 
            base(x, y, width, height, gameName, showCursor)
        {

        }
    }
}
