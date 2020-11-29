using System;
using System.Windows.Input;
using System.Windows;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;

namespace JTranEdit
{
    public class JsonEditViewModel
    {
        private readonly IFoldingStrategy _foldingStrategy;
        private readonly ICodeCompletion _codeCompletion;
        private bool _enabled = true;

        public JsonEditViewModel(TextEditor winEditor, ICodeCompletion codeCompletion, Preferences preferences)
        {
            _foldingStrategy = new JsonFoldingStrategies(winEditor, preferences);
            _codeCompletion  = codeCompletion;
        }

        public Preferences Preferences
        { 
            get { return _foldingStrategy.Preferences; }
            set { _foldingStrategy.Preferences = value; }
        }

        public bool IsEditable        
        { 
            get { return _enabled; } 
            set { _enabled = value; this.EditVisibility = _enabled ? Visibility.Visible : Visibility.Collapsed; }  
        }

        public Visibility EditVisibility    { get; set; } = Visibility.Visible;
        public string     SettingName       { get; set; }

        public void UpdateFoldings(TextDocument doc)
        {
            _foldingStrategy.UpdateFoldings(doc);
        }

        public void OnTextEntering(TextCompositionEventArgs e)
        {
            _codeCompletion.OnTextEntering(e);
        }

        public void OnTextEntered(TextCompositionEventArgs e, TextDocument doc)
        {
            if(_codeCompletion.OnTextEntered(e.Text))
                _foldingStrategy.UpdateFoldings(doc);
        }

        public void CollapseAll()
        {
            _foldingStrategy.CollapseAll();
        }

        public void ExpandAll()
        {
            _foldingStrategy.ExpandAll();
        }
    }
}
