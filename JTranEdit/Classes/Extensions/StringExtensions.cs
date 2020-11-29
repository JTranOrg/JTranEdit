using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;

namespace JTranEdit
{
    public static class StringExtensions
    {
        /****************************************************************************/
        public static string ReplaceEnding(this string s, string ending, string replace)
        {
            if(!s.EndsWith(ending))
                return s;
            
            return s.Substring(0, s.Length - ending.Length) + replace;
        }  
        
        /****************************************************************************/
        public static string LoadResource(this string resName)
        {
			using(Stream s = typeof(MainWindow).Assembly.GetManifestResourceStream("JTranEdit.Assets." + resName)) 
            {
				if (s == null)
					throw new InvalidOperationException("Could not find embedded resource");

				return s.ReadString();
			}
        }

        /****************************************************************************/
        public static int PreviousIndexOf(this string s, int position, string find)
        {
            if(string.IsNullOrEmpty(s))
                return -1;

            if(s.IndexOf(find) == -1)
                return -1;

            for(int i = position - find.Length; i >= 0; --i)
            {
                if(s.Substring(i, find.Length) == find)
                    return i;
            }
            
            return -1;
        }  
        

    }
}
