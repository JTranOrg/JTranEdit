using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JTran;

namespace JTranEdit
{
    /****************************************************************************/
    /****************************************************************************/
    public class FileDocumentRepository : IDocumentRepository
    {
        private readonly string _path;

        /****************************************************************************/
        internal FileDocumentRepository(string path)
        {
            _path = path;
        }

        /****************************************************************************/
        public string GetDocument(string name)
        {
            var fullPath = Path.Combine(_path, name + ".json");

            return File.ReadAllText(fullPath);
        }
    }
}
