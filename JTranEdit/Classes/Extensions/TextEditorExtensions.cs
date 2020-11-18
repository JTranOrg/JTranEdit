using System;
using System.Collections.Generic;
using System.Text;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;

namespace JTranEdit
{
    public static class TextEditorExtensions
    {
        public static string GetLineBeforeCurrent(this TextEditor winEditor, int caretOffset = 0)
        {
            var source  = winEditor.Text;
            var offset  = winEditor.CaretOffset + caretOffset;
            var newLine = source.PreviousIndexOf(offset, "\r\n");
            
            if(newLine != -1)
                newLine += "\r\n".Length;
           else
                newLine = 0;

            return source.Substring(newLine, offset - newLine);
        }
    }
}
