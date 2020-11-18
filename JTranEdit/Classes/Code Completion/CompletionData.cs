using System;
using System.Collections.Generic;
using System.Text;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace JTranEdit
{
    public class CompletionData : ICompletionData
    {
        private readonly ICodeEditor   _winEditor;
        private readonly SyntaxElement _element;

        public CompletionData(ICodeEditor winEditor, SyntaxElement element)
        {
            _winEditor      = winEditor;
            _element         = element;
            this.Text        = element.Name;
            this.Description = element.Description;
        }

        public System.Windows.Media.ImageSource Image => null;

        public string Text          { get; }
        public object Content       => this.Text; 
        public object Description   { get; }
        public double Priority      => 1.0;

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            var sb          = new StringBuilder();
            var currentLine = _winEditor.TextBox.GetLineBeforeCurrent(-1).Trim();
            var source      = _winEditor.TextBox.Text;
            var offset      = _winEditor.TextBox.CaretOffset;
            var newLine     = source.PreviousIndexOf(offset, "\r\n");
            var lineStart   = (newLine == -1) ? 0 : (newLine + "\r\n".Length);
            var indent      = (newLine == -1) ? 0 : (offset - (newLine + "\r\n".Length));

            textArea.Document.Remove(offset - "#".Length, "#".Length);

            if(currentLine == "" || currentLine.EndsWith(":"))
                sb.Append("\"");

            sb.Append("#");
            sb.Append(this.Text.Substring(1));

            if(_element.Params && !this.Text.EndsWith("()"))
                sb.Append("()");

            sb.Append("\":");

            if(_element.Block)
            { 
                sb.AppendLine("");
                sb.AppendLine("{".PadLeft(indent));
                sb.AppendLine("".PadLeft(indent + _winEditor.Preferences.Indent));
                sb.Append("}".PadLeft(indent));
            }

            textArea.Document.Replace(completionSegment, sb.ToString());

            var caret = _winEditor.TextBox.CaretOffset;

            if(_element.Block && _element.Params)
                caret -= indent*3 + _winEditor.Preferences.Indent + 9;
            else if(_element.Block)
                caret -= indent + 3;
            else if(_element.Params)
                caret -= 3;

            _winEditor.TextBox.CaretOffset = caret;

            if(_element.Block)
                _winEditor.UpdateFoldings();
        }
    }
}
