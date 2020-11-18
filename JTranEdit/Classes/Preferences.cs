using System;
using System.Collections.Generic;
using System.IO;

namespace JTranEdit
{
    /****************************************************************************/
    /****************************************************************************/
    public class Preferences
    {
        /****************************************************************************/
        public Preferences()
        {
        }

        /****************************************************************************/
        public List<string> IncludePaths        { get; set; } = new List<string>();
        public List<string> DocumentPaths       { get; set; } = new List<string>();
        public bool         BracketCompletion   { get; set; } = true;
        public int          Indent              { get; set; } = 2;

        public bool         ShowLineNumbers     { get; set; } = true;
        public bool         ShowOutlining       { get; set; } = true;
        public bool         SaveOnTransform     { get; set; } = true;
        public bool         AutoSave            { get; set; } = false;
        
    }
}
