using System;
using System.Collections.Generic;
using System.Text;

using ICSharpCode.AvalonEdit;

namespace JTranEdit
{
    /*************************************************************************/
    /*************************************************************************/
    public interface ICodeEditor
    { 
        TextEditor        TextBox         { get; }
        Preferences       Preferences     { get; set; }
        string            SelectedText    { get; set; }
        string            JsonContent     { get; set; }
        string            CurrentFileName { get; set; }
        JsonEditViewModel ViewModel       { get; set; }

        void UpdateFoldings();
    }
}
