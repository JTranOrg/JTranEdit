using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JTranEdit.Classes.Extensions
{
    public static class StreamExtensions
    {
        public static string ReadString(this Stream stream, Encoding encoder = null)
        {
            encoder = encoder ?? UTF8Encoding.UTF8;

            if(stream.CanSeek)
                stream.Seek(0, SeekOrigin.Begin);

            try
            { 
                using(var mem = new MemoryStream())
                { 
                    stream.CopyTo(mem);

                    return encoder.GetString(mem.ToArray());
                }
            }
            finally
            { 
                if(stream.CanSeek)
                    stream.Seek(0, SeekOrigin.Begin);
            }
        }    }
}
