//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ScriptLib;

namespace XenoLib
{
    public class SCScript
    {
        //protected
        protected List<string> lines;
        protected string scriptName;

        //public
        /// <summary>
        /// SCScript constructor
        /// </summary>
        /// <param name="name">Script name</param>
        public SCScript(string name)
        {
            lines = new List<string>();
            scriptName = name;
        }
        /// <summary>
        /// SCScript copy constructor
        /// </summary>
        /// <param name="obj">SCScript reference</param>
        public SCScript(SCScript obj)
        {
            scriptName = obj.ScriptName;
            lines = new List<string>();
            for (int i = 0; i < obj.Lines.Count; i++)
            {
                lines[i] = obj.Lines[i];
            }
        }
        /// <summary>
        /// loads a script's data
        /// </summary>
        /// <param name="loadPath">Path of data stream</param>
        public void loadData(string loadPath)
        {
            StreamReader sr = new StreamReader(loadPath);
            int num = Convert.ToInt32(sr.ReadLine());
            for (int i = 0; i < num; i++)
            {
                lines.Add(sr.ReadLine());
            }
            sr.Close();
        }
        /// <summary>
        /// Saves a script's data
        /// </summary>
        /// <param name="savePath">Path of data stream</param>
        public void saveData(string savePath)
        {
            StreamWriter sw = new StreamWriter(savePath);
            sw.WriteLine(lines.Count);
            for (int i = 0; i < lines.Count; i++)
            {
                sw.WriteLine(lines[i]);
            }
            sw.Close();
        }
        /// <summary>
        /// Lines property
        /// </summary>
        public List<string> Lines
        {
            get { return lines; }
        }
        /// <summary>
        /// ScriptName property
        /// </summary>
        public string ScriptName
        {
            get { return scriptName; }
            set { scriptName = value; }
        }
    }
    
    public static class ScriptHelpers
    {
        public static void addBuiltinFunctions(SymbolTable globalSymbols, Context context)
        {
            Token tmpTok = null;
            List<string> argNames = new List<string>();
            Position start = new Position();
            Position end = new Position();

            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "print", start, end);
            argNames.Add("str");
            globalSymbols.addSymbol("print", new PRINT(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "printRet", start, end);
            argNames.Clear();
            argNames.Add("str");
            globalSymbols.addSymbol("printRet", new PRINT_RET(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "input", start, end);
            argNames.Clear();
            globalSymbols.addSymbol("input", new INPUT(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "inputInt", start, end);
            argNames.Clear();
            globalSymbols.addSymbol("inputInt", new INPUT_INT(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "clear", start, end);
            argNames.Clear();
            globalSymbols.addSymbol("clear", new CLEAR(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "isNumber", start, end);
            argNames.Clear();
            argNames.Add("num");
            globalSymbols.addSymbol("isNumber", new IS_NUMBER(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "isString", start, end);
            argNames.Clear();
            argNames.Add("str");
            globalSymbols.addSymbol("isString", new IS_STRING(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "isList", start, end);
            argNames.Clear();
            argNames.Add("str");
            globalSymbols.addSymbol("isList", new IS_LIST(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "isFunction", start, end);
            argNames.Clear();
            argNames.Add("func");
            globalSymbols.addSymbol("isFunction", new IS_FUNCTION(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "append", start, end);
            argNames.Clear();
            argNames.Add("value");
            globalSymbols.addSymbol("append", new APPEND(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "pop", start, end);
            argNames.Clear();
            globalSymbols.addSymbol("pop", new POP(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "extend", start, end);
            argNames.Clear();
            argNames.Add("listA");
            argNames.Add("listB");
            globalSymbols.addSymbol("extend", new EXTEND(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "run", start, end);
            argNames.Clear();
            argNames.Add("fileName");
            globalSymbols.addSymbol("run", new RUN(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "length", start, end);
            argNames.Clear();
            argNames.Add("list");
            globalSymbols.addSymbol("length", new LENGTH(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "createFile", start, end);
            argNames.Clear();
            globalSymbols.addSymbol("createFile", new CREATEFILE(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "writeLine", start, end);
            argNames.Clear();
            argNames.Add("file");
            argNames.Add("str");
            globalSymbols.addSymbol("writeLine", new WRITELINE(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "readLine", start, end);
            argNames.Clear();
            argNames.Add("file");
            globalSymbols.addSymbol("readLine", new READLINE(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "openFile", start, end);
            argNames.Clear();
            argNames.Add("file");
            globalSymbols.addSymbol("openFile", new OPENFILE(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "closeFile", start, end);
            argNames.Clear();
            argNames.Add("file");
            globalSymbols.addSymbol("closeFile", new CLOSEFILE(tmpTok, argNames, start, end, context));
            tmpTok = new Token(TOKENS.TT_IDENTIFIER, "createFile", start, end);
            argNames.Clear();
            globalSymbols.addSymbol("createFile", new CREATEFILE(tmpTok, argNames, start, end, context));
        }
    }
}
