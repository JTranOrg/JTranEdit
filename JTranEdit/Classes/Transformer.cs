using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json.Linq;
using JTran;
using JTran.Extensions;
using System.Dynamic;

namespace JTranEdit
{
    /****************************************************************************/
    /****************************************************************************/
    public class Transformer
    {
         private readonly JsonFormatter _formatter   = new JsonFormatter();
         private readonly Preferences   _preferences;

        /****************************************************************************/
        public Transformer(Preferences preferences)
        {
            _preferences = preferences;
        }

        /****************************************************************************/
        public string Transform(string source, string transform)
        {
            var transformer  = new JTran.Transformer(transform, includeSource: new FileIncludeRepository(_preferences.IncludePaths)); 

            var context = new TransformerContext();

            context.DocumentRepositories["race"] = new FileDocumentRepository(_preferences.DocumentPaths[0]);

            var result  = transformer.Transform(source, context);
            var expando = result.JsonToExpando() as ExpandoObject;

            return _formatter.Format(expando);            
        }
    }
}
