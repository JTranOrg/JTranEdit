using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using JTran;

namespace JTranEdit
{
        /****************************************************************************/
        /****************************************************************************/
        internal class DocumentsRepository : IDictionary<string, IDocumentRepository>
        {
            private readonly string _path;
            private IDictionary<string, IDocumentRepository> _cache = new Dictionary<string, IDocumentRepository>();

            /****************************************************************************/
            internal DocumentsRepository(string path)
            {
                _path = path;
            }

             /****************************************************************************/
             public IDocumentRepository this[string key] 
            { 
                get 
                { 
                    if(_cache.ContainsKey(key))
                        return _cache[key];

                    var fullPath =  Path.Combine(_path, key); 

                    var repo = new FileDocumentRepository(fullPath);

                    _cache.Add(key, repo);

                    return repo;
                }

                set { throw new NotSupportedException(); } 
             }

            /****************************************************************************/
            public bool IsReadOnly => true;

            /****************************************************************************/
            public bool ContainsKey(string key)
            {
                return true;
            }

            #region NotSupported

            public ICollection<string> Keys => throw new NotSupportedException();

            public ICollection<IDocumentRepository> Values => throw new NotSupportedException();

            public int Count => throw new NotSupportedException();


            public void Add(string key, IDocumentRepository value)
            {
                throw new NotSupportedException();
            }

            public void Add(KeyValuePair<string, IDocumentRepository> item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public bool Contains(KeyValuePair<string, IDocumentRepository> item)
            {
                throw new NotSupportedException();
            }

            public void CopyTo(KeyValuePair<string, IDocumentRepository>[] array, int arrayIndex)
            {
                throw new NotSupportedException();
            }

            public IEnumerator<KeyValuePair<string, IDocumentRepository>> GetEnumerator()
            {
                throw new NotSupportedException();
            }

            public bool Remove(string key)
            {
                throw new NotSupportedException();
            }

            public bool Remove(KeyValuePair<string, IDocumentRepository> item)
            {
                throw new NotSupportedException();
            }

            public bool TryGetValue(string key, out IDocumentRepository value)
            {
                throw new NotSupportedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotSupportedException();
            }

            #endregion
        }
}
