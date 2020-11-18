using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace JTranEdit
{
        /****************************************************************************/
        /****************************************************************************/
        internal class FileIncludeRepository : IDictionary<string, string>
        {
            private readonly IList<string> _paths;
            private IDictionary<string, string> _cache = new Dictionary<string, string>();

            /****************************************************************************/
            internal FileIncludeRepository(IList<string> paths)
            {
                _paths = paths;
            }

             /****************************************************************************/
             public string this[string key] 
            { 
                get 
                { 
                    if(_cache.ContainsKey(key))
                        return _cache[key];

                    foreach(var path in _paths)
                    { 
                        var fullPath =  Path.Combine(path, key); 

                        if(File.Exists(fullPath))
                        {
                            var result = File.ReadAllText(fullPath); 

                            _cache.Add(key, result);

                            return result;
                        }
                    }

                    throw new FileNotFoundException($"The include file named {key} was not found.");
                }

                set { throw new NotSupportedException(); } 
             }

            /****************************************************************************/
            public bool IsReadOnly => true;

            /****************************************************************************/
            public bool ContainsKey(string key)
            {
                if(_cache.ContainsKey(key))
                    return true;

                foreach(var path in _paths)
                { 
                    var fullPath =  Path.Combine(path, key); 

                    if(File.Exists(fullPath))
                        return true;
                }

                return false;
            }

            #region NotSupported

            public ICollection<string> Keys => throw new NotSupportedException();

            public ICollection<string> Values => throw new NotSupportedException();

            public int Count => throw new NotSupportedException();


            public void Add(string key, string value)
            {
                throw new NotSupportedException();
            }

            public void Add(KeyValuePair<string, string> item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public bool Contains(KeyValuePair<string, string> item)
            {
                throw new NotSupportedException();
            }

            public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
            {
                throw new NotSupportedException();
            }

            public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
            {
                throw new NotSupportedException();
            }

            public bool Remove(string key)
            {
                throw new NotSupportedException();
            }

            public bool Remove(KeyValuePair<string, string> item)
            {
                throw new NotSupportedException();
            }

            public bool TryGetValue(string key, out string value)
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
