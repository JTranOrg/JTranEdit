using System;

namespace JTran.Syntax
{
    public interface ILanguageSyntax
    {
        bool IsCompleted(string source, int position, string bracket);
        bool StartsWithBracket(string source, out string startBracket, out string endBracket);
        int  FindEndBracket(string source, int offset, string startBracket);
    }
}
