using System;
using System.Collections.Generic;
using System.Text;

namespace JTranEdit
{
    public class SyntaxSchema
    {   
        public List<SyntaxElement> Elements { get; set; }
    }

    public class SyntaxElement
    {   
        public string Name        { get; set; }
        public bool   Params      { get; set; }
        public bool   Block       { get; set; }
        public string Description { get; set; }
    }
}
