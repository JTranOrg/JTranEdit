using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

using JTran.Syntax;

namespace JTranEdit
{
    /*************************************************************************/
    /*************************************************************************/
    public class JsonCodeCompletion : ICodeCompletion
    {
        protected readonly ICodeEditor   _winEditor;
        private readonly ILanguageSyntax _syntax = new JsonSyntax();

        /*************************************************************************/
        public JsonCodeCompletion(ICodeEditor winEditor)
        {
            _winEditor = winEditor;
        }

        /*************************************************************************/
        public virtual void OnTextEntering(TextCompositionEventArgs e)
        {
            _winEditor.SelectedText = _winEditor.TextBox.SelectedText;
        }

        /*************************************************************************/
        public virtual bool OnTextEntered(string text)
        {
            if(_winEditor.Preferences.BracketCompletion)
            {
                switch(text)
                {
                    case "{":
                        CompleteBracket("{", "}");
                        break;

                    case "[":
                        CompleteBracket("[", "]", _winEditor.TextBox.GetLineBeforeCurrent(-1).Trim() != "");
                        break;
                              
                    case "(":
                        CompleteBracket("(", ")", true);
                        break;

                    default:
                        return false;
               }

               return true;
            }
         
            return false;
        }

        #region Private 

        /*************************************************************************/
        private void CompleteBracket(string startBracket, string endBracket, bool inline = false)
        {
            var source  = _winEditor.TextBox.Text;
            var offset  = _winEditor.TextBox.CaretOffset;
            var newLine = source.PreviousIndexOf(offset, "\r\n");
            var indent  = (newLine == -1) ? 0 : (offset - (newLine + "\r\n".Length) - startBracket.Length);
    
            if(_syntax.IsCompleted(source, offset, startBracket))
                return;
    
            var sb = new StringBuilder();
    
            if(!string.IsNullOrWhiteSpace(_winEditor.SelectedText) && _syntax.StartsWithBracket(_winEditor.SelectedText, out string oldStartBracket, out string oldEndBracket))
            {
                var endIndex = _syntax.FindEndBracket(source, offset, oldStartBracket);
        
                if(endIndex != -1)
                { 
                    _winEditor.TextBox.Document.Remove(endIndex, oldEndBracket.Length);
                    _winEditor.TextBox.Document.Insert(endIndex, endBracket);
                }
            }
            else if(!inline)
            {
                sb.AppendLine("");
                sb.AppendLine("".PadLeft(indent + _winEditor.Preferences.Indent));
                sb.Append(endBracket.PadLeft(indent+1));
    
                _winEditor.TextBox.Document.Insert(_winEditor.TextBox.CaretOffset, sb.ToString());
                _winEditor.TextBox.CaretOffset = offset + indent + _winEditor.Preferences.Indent + "\r\n".Length;
            }
            else
            {
                _winEditor.TextBox.Document.Insert(_winEditor.TextBox.CaretOffset, endBracket);
                _winEditor.TextBox.CaretOffset = offset;
            }
    
            _winEditor.SelectedText = "";
        }
 
        #endregion
    }
}
