//====================================================
//Written by Kujel Selsuru
//Last Updated 16/12/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWshRuntimeLibrary;

namespace XenoLib
{
    /// <summary>
    /// Assumes "basic button" png file in installer folder
    /// </summary>
    public class SimpleInstaller
    {
        //protected
        protected SimpleStringBuilder ssb;
        protected string str;
        protected string destPath;
        protected string sourcePath;
        protected string shortcutDiscription;
        protected string installingMsg;
        protected Rectangle rectBack;
        protected Rectangle rectStr;
        protected Rectangle rectStr2;
        protected SimpleButton4 installButton;
        protected SimpleButton4 quitButton;
        protected bool quit;
        protected bool installing;
        //public
        /// <summary>
        /// SimpleInstaller constructor
        /// </summary>
        /// <param name="x">X position in window</param>
        /// <param name="y">Y position in window</param>
        /// <param name="w">Width of background in pixels</param>
        /// <param name="h">Height of background in pixels</param>
        /// <param name="destPath">Destition path defualt</param>
        /// <param name="sourcePath">Source path of files to install</param>
        public SimpleInstaller(int x, int y, int w, int h, string destPath = "", string sourcePath = "")
        {
            ssb = new SimpleStringBuilder();
            str = destPath;
            this.destPath = destPath;
            this.sourcePath = sourcePath;
            shortcutDiscription = "";
            installingMsg = "Game installing please wait";
            rectBack = new Rectangle(x, y, w, h);
            rectStr = new Rectangle(x + 32, y + 32, w - 64, 32);
            rectStr2 = new Rectangle(x + 32, y + 64, w - 64, 32);
            installButton = new SimpleButton4(TextureBank.getTexture("basic button"), TextureBank.getTexture("basic button"), 
                x + 32, (y + h) - 32, "Install");
            quitButton = new SimpleButton4(TextureBank.getTexture("basic button"), TextureBank.getTexture("basic button"),
                (x + w) - 32, (y + h) - 32, "Quit");
            quit = false;
            installing = false;
        }
        /// <summary>
        /// Draws SimpleInstaller
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public virtual void draw(IntPtr renderer)
        {
            DrawRects.drawRect(renderer, rectBack, ColourBank.getColour(XENOCOLOURS.GRAY), true);
            DrawRects.drawRect(renderer, rectStr, ColourBank.getColour(XENOCOLOURS.BLACK), true);
            SimpleFont.DrawString(renderer, ssb.Sequence, rectStr.IX, rectStr.IY, 0.75f);
            if(installing == true)
            {
                DrawRects.drawRect(renderer, rectStr2, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                SimpleFont.DrawString(renderer, installingMsg, rectStr.IX, rectStr.IY + 32, 0.75f);
            }
            installButton.draw(renderer);
            installButton.drawName(renderer);
            quitButton.draw(renderer);
            quitButton.drawName(renderer);
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        public virtual void update()
        {
            if (installing == true)
            {
                destPath = str;
                install();
            }
            else
            {
                ssb.Sequence = str;
                ssb.update();
                str = ssb.Sequence;

                if (installButton.clicked() == true)
                {
                    if (installing == false)
                    {
                        installing = true;
                    }
                    else
                    {
                        installing = false;
                    }
                }
                if (quitButton.clicked() == true)
                {
                    if (quit == false)
                    {
                        quit = true;
                    }
                    else
                    {
                        quit = false;
                    }
                }
            }
        }
        /// <summary>
        /// Decompresses data from the source path and creates a shortcut to exe file on desktop
        /// must be extended to properly work, this is just a base version
        /// </summary>
        public virtual void install()
        {
            //in extended version decompress every file
            Helpers<int>.decompressData(sourcePath, destPath, Helpers<int>.extractFileName(sourcePath), "ext", "outExt");
            //in extended version define discription
            shortcutDiscription = "Game";
            //creates a shortcut for game, in extended version define vlaues
            createShortcut("Game", "desktop", "exe file name", "icon graphic name");
            //quit installer when done
            quit = true;
        }
        /// <summary>
        /// Creates a desktop shortcut
        /// </summary>
        /// <param name="shortcutName">Name of shortcut</param>
        /// <param name="shortcutPath">Path to where shortcut is made</param>
        /// <param name="targetFileName">Target file path</param>
        /// <param name="iconPath">Path of shortcut icon graphic</param>
        protected void createShortcut(string shortcutName, string shortcutPath, string targetFileName, string iconPath)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "";
            shortcut.IconLocation = iconPath;
            shortcut.TargetPath = targetFileName;
            shortcut.Save();
        }
        /// <summary>
        /// Quit property
        /// </summary>
        public bool Quit
        {
            get { return quit; }
            set { quit = value; }
        }
    }

    public class SimpleInstaller2 : SimpleInstaller
    {
        //protected
        protected FolderExplorer fe;
        //public
        public SimpleInstaller2(int x, int y, int w, int h, Texture2D downButtonPressed,
            Texture2D downButtonDepressed, Texture2D upButtonPressed,
            Texture2D upButtonDepressed, Texture2D backButtonPressed,
            Texture2D backButtonDepressed, Texture2D exitButton,
            int size = 9, int shift = 32, int delay = 120, string destPath = "", 
            string sourcePath = "") : 
            base(x, y, w, h, destPath, sourcePath)
        {
            fe = new FolderExplorer(x, y + 32, w, h - 96, downButtonPressed, downButtonDepressed, 
                upButtonPressed, upButtonDepressed, backButtonPressed, backButtonDepressed, 
                exitButton, size, shift, delay);
            fe.Path = destPath;
        }
        /// <summary>
        /// Draws SimpleInstaller
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="cursor">SimpleCursor reference</param>
        public virtual void draw(IntPtr renderer, SimpleCursor cursor)
        {
            DrawRects.drawRect(renderer, rectBack, ColourBank.getColour(XENOCOLOURS.GRAY), true);
            DrawRects.drawRect(renderer, rectStr, ColourBank.getColour(XENOCOLOURS.BLACK), true);
            fe.draw(renderer, cursor);
            SimpleFont.DrawString(renderer, fe.Path, rectStr.IX, rectStr.IY, 0.75f);
            if (installing == true)
            {
                DrawRects.drawRect(renderer, rectStr2, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                SimpleFont.DrawString(renderer, installingMsg, rectStr.IX, rectStr.IY + 32, 0.75f);
            }
            installButton.draw(renderer);
            installButton.drawName(renderer);
            quitButton.draw(renderer);
            quitButton.drawName(renderer);
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        /// <param name="cursor">SimpleCursor reference</param>
        public virtual void update(SimpleCursor cursor)
        {
            if (installing == true)
            {
                destPath = fe.Path;
                install();
            }
            else
            {
                fe.update4(cursor);
                if (installButton.clicked() == true)
                {
                    if (installing == false)
                    {
                        installing = true;
                    }
                    else
                    {
                        installing = false;
                    }
                }
                if (quitButton.clicked() == true)
                {
                    if (quit == false)
                    {
                        quit = true;
                    }
                    else
                    {
                        quit = false;
                    }
                }
            }
        }
    }
    /// <summary>
    /// A base packager class, can be extended
    /// </summary>
    public class SimplePackager
    {
        //protected
        protected string sourcePath;
        protected string filePathOut;
        protected string outExt;
        protected string[] fileNames;
        //public
        /// <summary>
        /// SimplePackager constructor
        /// </summary>
        public SimplePackager(string sourcePath = "", string filePathOut = "", string outExt = "")
        {
            this.sourcePath = sourcePath;
            this.filePathOut = filePathOut;
            this.outExt = outExt;
            fileNames = null;
        }
        /// <summary>
        /// Packages all files in the source folder, set the sourcePath, filePathOut and 
        /// extOut before calling
        /// </summary>
        public virtual void package()
        {
            fileNames = System.IO.Directory.GetFiles(sourcePath);
            for(int f = 0; f < fileNames.Length - 1; f++)
            {
                Helpers<int>.compressData(fileNames[f], filePathOut + Helpers<int>.extractFileName(fileNames[f]), Helpers<int>.extractExtention(fileNames[f]), outExt);
            }
        }
        /// <summary>
        /// Copies the installer and shortcut icon graphic 
        /// </summary>
        /// <param name="installerName">Installer folder name</param>
        /// <param name="iconName">Shortcut graphic name</param>
        /// <param name="sourceFolder">Source folder for installer</param>
        /// <param name="destFolder">Destination folder to copy to</param>
        /// <param name="iconExt">Icon extention</param>
        public virtual void copyInstallerParts(string installerName, string iconName, string sourceFolder, 
            string destFolder, string iconExt)
        {
            Helpers<int>.copyFolder(sourceFolder + installerName, 
                destFolder + installerName);
            Helpers<int>.copyFolder(sourceFolder + iconName + iconExt,
                destFolder + iconName + iconExt);

        }
        /// <summary>
        /// SourcePath property
        /// </summary>
        public string SourcePath
        {
            get { return sourcePath; }
            set { sourcePath = value; }
        }
        /// <summary>
        /// FilePathOut property
        /// </summary>
        public string FilePathOut
        {
            get { return filePathOut; }
            set { filePathOut = value; }
        }
        /// <summary>
        /// OutExt property
        /// </summary>
        public string OutExt
        {
            get { return outExt; }
            set { outExt = value; }
        }
    }
}
