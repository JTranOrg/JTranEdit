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
            var transformer  = new JTran.Transformer(transform, includeSource: new FileIncludeRepository(_preferences.IncludePath)); 

            var context = new TransformerContext();

            context.DocumentRepositories = new DocumentsRepository(_preferences.DocumentPath); 

            var result  = transformer.Transform(source, context);
            var expando = result.JsonToExpando() as ExpandoObject;

            return _formatter.Format(expando);            
        }
    }
}
