using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace JTranEdit
{
    public interface ICodeCompletion
    {
         void OnTextEntering(TextCompositionEventArgs e);
         bool OnTextEntered(string text);
    }
}
