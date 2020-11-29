using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Win32;

namespace JTranEdit
{
    public static class ICodeEditorExtensions
    {
        /// <summary>
        /// Load a file and set text of json editor
        /// </summary>
        /// <param name="editor"></param>
        public static void LoadFile(this ICodeEditor editor, string filter, string lastFile)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if(!string.IsNullOrWhiteSpace(editor.CurrentFileName))
                openFileDialog.InitialDirectory = Path.GetDirectoryName(editor.CurrentFileName);
            else if(!string.IsNullOrWhiteSpace(lastFile))
                openFileDialog.InitialDirectory = Path.GetDirectoryName(lastFile);

            openFileDialog.Filter = filter;

			if(openFileDialog.ShowDialog() == true)
            { 
                editor.LoadFile(openFileDialog.FileName);
            }

            return;
        }        

        /// <summary>
        /// Save a file and set text of json editor
        /// </summary>
        /// <param name="editor"></param>
        public static void SaveAsFile(this ICodeEditor editor, string lastFile)
        {
            var content = editor.JsonContent;

            if(!string.IsNullOrWhiteSpace(content))
            { 
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                if(!string.IsNullOrWhiteSpace(editor.CurrentFileName))
                    saveFileDialog.InitialDirectory = Path.GetDirectoryName(editor.CurrentFileName);
                else if(!string.IsNullOrWhiteSpace(lastFile))
                    saveFileDialog.InitialDirectory = Path.GetDirectoryName(lastFile);

			    if(saveFileDialog.ShowDialog() == true)
                {                 
                    WriteFile(saveFileDialog.FileName, content);
                    editor.CurrentFileName =  saveFileDialog.FileName;
                }
            }
            return;
        }

        /// <summary>
        /// Save a file and set text of json editor
        /// </summary>
        /// <param name="editor"></param>
        public static void SaveFile(this ICodeEditor editor, string lastFile)
        {
            var content = editor.JsonContent;

            if(!string.IsNullOrWhiteSpace(content))
            { 
			    if(!string.IsNullOrWhiteSpace(editor.CurrentFileName))
                {                 
                    WriteFile(editor.CurrentFileName, content);
                }
                else
                    editor.SaveAsFile(lastFile);
            }

            return;
        }    
        /// <summary>
        /// Save a file and set text of json editor
        /// </summary>
        /// <param name="editor"></param>
        private static void WriteFile(string fileName, string content)
        {
            using(var stream = File.OpenWrite(fileName))
            {
                stream.WriteString(content);
            }

            return;
        }        
        
        public static void LoadFile(this ICodeEditor editor, string fileName)
        {
            var result = "";
 
            using(var stream = File.OpenRead(fileName))
            {
                result = stream.ReadString();
            }

            if(!string.IsNullOrWhiteSpace(result))
            {
                editor.JsonContent     = result;
                editor.CurrentFileName = fileName;
            }

            return;
        }
    }
}
