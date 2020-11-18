using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

using Newtonsoft.Json;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

using JTran.Syntax;

namespace JTranEdit
{
    /*************************************************************************/
    /*************************************************************************/
    public class JTranCodeCompletion : JsonCodeCompletion
    {
        private SyntaxSchema _schema;
        private CompletionWindow _completionWindow;

        /*************************************************************************/
        public JTranCodeCompletion(ICodeEditor winEditor) : base(winEditor)
        {
        }

        /*************************************************************************/
        public override void OnTextEntering(TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && _completionWindow != null) 
            {
                if (!char.IsLetterOrDigit(e.Text[0])) 
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    _completionWindow.CompletionList.RequestInsertion(e);
                }
            }
            else
                base.OnTextEntering(e);
        }

        /*************************************************************************/
        public override bool OnTextEntered(string text)
        {
            if(text == "#") 
            {
                if(_schema == null)
                    _schema = JsonConvert.DeserializeObject<SyntaxSchema>("Syntax.json".LoadResource());

                // Open code completion after the user has pressed #
                _completionWindow = new CompletionWindow(_winEditor.TextBox.TextArea);
                IList<ICompletionData> data = _completionWindow.CompletionList.CompletionData;

                foreach(var element in _schema.Elements)
                    data.Add(new CompletionData(_winEditor, element));

                _completionWindow.Show();

                _completionWindow.Closed += delegate 
                {
                    _completionWindow = null;
                };
            }
            else
                return base.OnTextEntered(text);
         
            return false;
        }
    }
}
