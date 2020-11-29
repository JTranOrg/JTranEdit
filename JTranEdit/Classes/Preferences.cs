using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;

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
        public Preferences(Preferences copy)
        {
            this.IncludePath          = copy.IncludePath;      
            this.DocumentPath         = copy.DocumentPath;     
            this.BracketCompletion    = copy.BracketCompletion;
            this.Indent               = copy.Indent;           
            this.ShowLineNumbers      = copy.ShowLineNumbers;  
            this.ShowOutlining        = copy.ShowOutlining;    
            this.SaveOnTransform      = copy.SaveOnTransform;  
            this.AutoSave             = copy.AutoSave;         
        }

        /****************************************************************************/
        public string   IncludePath         { get; set; }
        public string   DocumentPath        { get; set; }
        public bool     BracketCompletion   { get; set; } = true;
        public int      Indent              { get; set; } = 2;
                        
        public bool     ShowLineNumbers     
        { 
            get { return _showLineNumbers; } 
            set { _showLineNumbers = value; OnPropertyChanged(nameof(ShowLineNumbers)); } 
        } 
        private bool _showLineNumbers = true;

        public bool     ShowOutlining       { get; set; } = true;
        public bool     SaveOnTransform     { get; set; } = true;
        public bool     AutoSave            { get; set; } = false;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName); 
                handler(this, e);
            }
        }
    }
}
