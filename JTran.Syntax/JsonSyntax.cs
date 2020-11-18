using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JTran.Syntax
{
    /// <summary>
    /// Syntax for JSON
    /// </summary>
    public class JsonSyntax : ILanguageSyntax
    {
        private readonly IDictionary<string, string> _brackets    = new Dictionary<string, string> { {"{", "}" }, {"[", "]" }, {"(", ")" } };
        private readonly IDictionary<string, string> _endBrackets = new Dictionary<string, string> { {"}", "{" }, {"]", "[" }, {")", "(" } };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="position"></param>
        /// <param name="end"></param>
        /// <returns>True if </returns>
        public bool IsCompleted(string source, int position, string bracket)
        {
            if(!_brackets.ContainsKey(bracket))
                return false;

            var len        = source.Length;
            var endBracket = _brackets[bracket];
            var stack      = new Stack<string>();
            var quote      = "";

            for(var i = 0; i < len; ++i)
            {
                var ch = source[i].ToString();

                if(quote == ch)
                {
                   quote = "";
                   continue;
                }

                if(!string.IsNullOrWhiteSpace(quote))
                    continue;

                if(ch == "\"" || ch == "'")
                {
                   quote = ch;
                   continue;
                }

                if(_endBrackets.ContainsKey(ch))
                { 
                    if(stack.Peek() != _endBrackets[ch])
                        return false;

                    stack.Pop();
                    continue;
                }

                if(_brackets.ContainsKey(ch))
                { 
                    stack.Push(ch);
                    continue;
                }
            }

            return stack.Count == 0;
        }

        public bool StartsWithBracket(string source, out string startBracket, out string endBracket)
        {
            source = source.Trim();

            startBracket = "";
            endBracket = "";

            foreach(var bracket in _brackets.Keys)
            {
                if(source.StartsWith(bracket))
                {
                    startBracket = bracket;
                    endBracket = _brackets[bracket];
                    return true;
                }
            }

            return false;
        }

        public int FindEndBracket(string source, int offset, string startBracket)
        {
            if(!_brackets.ContainsKey(startBracket))
                return -1;

            var len        = source.Length;
            var endBracket = _brackets[startBracket];
            var stack      = new Stack<string>();
            var quote      = "";

            for(var i = offset; i < len; ++i)
            {
                var ch = source[i].ToString();

                if(quote == ch)
                {
                   quote = "";
                   continue;
                }

                if(!string.IsNullOrWhiteSpace(quote))
                    continue;

                if(ch == "\"" || ch == "'")
                {
                   quote = ch;
                   continue;
                }

                if(endBracket == ch && stack.Count == 0)
                    return i;

                if(_endBrackets.ContainsKey(ch))
                { 
                    if(stack.Peek() != _endBrackets[ch])
                        return -1;

                    stack.Pop();
                }

                if(_brackets.ContainsKey(ch))
                { 
                    stack.Push(ch);
                    continue;
                }
            }

            return -1;
        }
    }
}
