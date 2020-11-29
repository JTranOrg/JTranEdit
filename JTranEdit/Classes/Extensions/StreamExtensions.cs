using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JTranEdit
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
        }    

        public static void WriteString(this Stream stream, string text, Encoding encoder = null)
        {
            encoder = encoder ?? UTF8Encoding.UTF8;

            if(stream.CanSeek)
                stream.Seek(0, SeekOrigin.Begin);

            try
            { 
                var bytes = encoder.GetBytes(text);
                { 
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
            finally
            { 
                if(stream.CanSeek)
                    stream.Seek(0, SeekOrigin.Begin);
            }
        }    
    }
}
