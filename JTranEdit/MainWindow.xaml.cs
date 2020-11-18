using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.Win32;
using System.Diagnostics;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;

using MondoCore.Common;
using System.Reflection.Metadata.Ecma335;

using JTranEdit.Dialogs;
using System.ComponentModel;

namespace JTranEdit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Preferences _preferences = new Preferences();

        public MainWindow()
        {
            var docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            _preferences.IncludePaths.Add(Path.Combine(docPath, "JTran Samples\\Includes")); // ???
            _preferences.DocumentPaths.Add(Path.Combine(docPath, "JTran Samples\\Documents")); // ???

            _preferences.ShowLineNumbers = JTranEdit.user.Default.ShowLineNumbers;
            _preferences.ShowOutlining   = JTranEdit.user.Default.ShowOutlining;
            _preferences.SaveOnTransform = JTranEdit.user.Default.SaveOnTransform;
            _preferences.AutoSave        = JTranEdit.user.Default.AutoSave;

            LoadHighlighting();
            InitializeComponent();

            InitEditor(SourcePane,      JTranEdit.user.Default.LastSourceFile);
            InitEditor(TransformPane,   JTranEdit.user.Default.LastTransformFile);
            InitEditor(OutputPane,      JTranEdit.user.Default.LastOutputFile);
            InitEditor(DocumentsPane,   JTranEdit.user.Default.LastDocumentFile);
            InitEditor(IncludesPane,    JTranEdit.user.Default.LastIncludeFile);
        }

        private void InitEditor(ICodeEditor editor, string lastFile)
        {        
            editor.Preferences = _preferences;
            editor.ViewModel   = new JsonEditViewModel(editor.TextBox, new JsonCodeCompletion(editor));

            if(!string.IsNullOrWhiteSpace(lastFile))
            {
                LoadFile(editor, lastFile);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            JTranEdit.user.Default.ShowLineNumbers  = _preferences.ShowLineNumbers;
            JTranEdit.user.Default.ShowOutlining    = _preferences.ShowOutlining;
            JTranEdit.user.Default.SaveOnTransform  = _preferences.SaveOnTransform;
            JTranEdit.user.Default.AutoSave         = _preferences.AutoSave;

            if(!string.IsNullOrWhiteSpace(this.SourcePane.CurrentFileName))
                JTranEdit.user.Default.LastSourceFile = this.SourcePane.CurrentFileName;

            if(!string.IsNullOrWhiteSpace(this.TransformPane.CurrentFileName))
                JTranEdit.user.Default.LastTransformFile = this.TransformPane.CurrentFileName;

            if(!string.IsNullOrWhiteSpace(this.DocumentsPane.CurrentFileName))
                JTranEdit.user.Default.LastDocumentFile = this.DocumentsPane.CurrentFileName;

            if(!string.IsNullOrWhiteSpace(this.IncludesPane.CurrentFileName))
                JTranEdit.user.Default.LastIncludeFile = this.IncludesPane.CurrentFileName;

            if(!string.IsNullOrWhiteSpace(this.OutputPane.CurrentFileName))
                JTranEdit.user.Default.LastOutputFile = this.OutputPane.CurrentFileName;

            JTranEdit.user.Default.Save();

            base.OnClosing(e);
        }

        public static void NavigateToUrl(string url)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        #region Button Click Handlers

        private void SourcePane_FirstButtonClick(object sender, RoutedEventArgs e)
        {
            LoadFile(SourcePane, "Json Files (*.json)|*.json", JTranEdit.user.Default.LastSourceFile);
        }

        private void TransformPane_FirstButtonClick(object sender, RoutedEventArgs e)
        {
            LoadFile(TransformPane, "Json Files (*.json)|*.json|JTran Files (*.jtran)|*.jtran", JTranEdit.user.Default.LastTransformFile);
        }

        private void DocumentsPane_FirstButtonClick(object sender, RoutedEventArgs e)
        {
            LoadFile(DocumentsPane, "Json Files (*.json)|*.json", JTranEdit.user.Default.LastDocumentFile);
        }

        private void IncludesPane_FirstButtonClick(object sender, RoutedEventArgs e)
        {
            LoadFile(IncludesPane, "Json Files (*.json)|*.json|JTran Files (*.jtran)|*.jtran", JTranEdit.user.Default.LastIncludeFile);
        }

        private void OutputPane_FirstButtonClick(object sender, RoutedEventArgs e)
        {
            DoTransform();
        }

        private void SourcePane_SecondButtonClick(object sender, RoutedEventArgs e)
        {
            SaveFile(SourcePane, JTranEdit.user.Default.LastSourceFile);
        }

        private void TransformPane_SecondButtonClick(object sender, RoutedEventArgs e)
        {
            SaveFile(TransformPane, JTranEdit.user.Default.LastTransformFile);
        }

        private void DocumentsPane_SecondButtonClick(object sender, RoutedEventArgs e)
        {
            SaveFile(DocumentsPane, JTranEdit.user.Default.LastDocumentFile);
        }

        private void IncludesPane_SecondButtonClick(object sender, RoutedEventArgs e)
        {
            SaveFile(IncludesPane, JTranEdit.user.Default.LastIncludeFile);
        }

        private void OutputPane_SecondButtonClick(object sender, RoutedEventArgs e)
        {
            SaveFile(OutputPane, JTranEdit.user.Default.LastOutputFile);
        }

        private void SourcePane_ThirdButtonClick(object sender, RoutedEventArgs e)
        {
            SaveAsFile(SourcePane, JTranEdit.user.Default.LastSourceFile);
        }

        private void TransformPane_ThirdButtonClick(object sender, RoutedEventArgs e)
        {
            SaveAsFile(TransformPane, JTranEdit.user.Default.LastTransformFile);
        }

        private void OutputPane_ThirdButtonClick(object sender, RoutedEventArgs e)
        {
            SaveAsFile(OutputPane, JTranEdit.user.Default.LastOutputFile);
        }

        private void DocumentsPane_ThirdButtonClick(object sender, RoutedEventArgs e)
        {
            SaveAsFile(DocumentsPane, JTranEdit.user.Default.LastDocumentFile);
        }

        private void IncludesPane_ThirdButtonClick(object sender, RoutedEventArgs e)
        {
            SaveAsFile(IncludesPane, JTranEdit.user.Default.LastIncludeFile);
        }

        #endregion

        #region Menu Item Click Handlers

        private void MenuItem_Preferences_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new PreferencesDialog(_preferences);

            dlg.ShowDialog();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Help_Click(object sender, RoutedEventArgs e)
        {
            NavigateToUrl("https://github.com/JTranOrg/JTranEdit/blob/master/README.md");
        }

        private void MenuItem_Reference_Click(object sender, RoutedEventArgs e)
        {
            NavigateToUrl("https://github.com/JTranOrg/JTran/blob/master/docs/reference.md");
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AboutDialog();

            dlg.ShowDialog();
        }

        #endregion

        #region Private

        private void DoTransform()
        {
            try
            { 
                if(_preferences.SaveOnTransform)
                {
                    SaveFile(SourcePane,    "Sources");
                    SaveFile(TransformPane, "Transforms");
                    SaveFile(DocumentsPane, "Documents");
                    SaveFile(IncludesPane,  "Includes");
                }

                var source    = SourcePane.JsonContent;
                var transform = TransformPane.JsonContent;

                if(string.IsNullOrWhiteSpace(source))
                {
                    MessageBox.Show("Load or enter in a source document", "JTranEdit", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if(string.IsNullOrWhiteSpace(transform))
                {
                    MessageBox.Show("Load or enter in a transform", "JTranEdit", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var transformer = new Transformer(_preferences);
                var result      = transformer.Transform(source,  transform);

                OutputPane.JsonContent = result;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "JTranEdit", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Load a file and set text of json editor
        /// </summary>
        /// <param name="editor"></param>
        private void LoadFile(ICodeEditor editor, string filter, string lastFile)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if(!string.IsNullOrWhiteSpace(editor.CurrentFileName))
                openFileDialog.InitialDirectory = Path.GetDirectoryName(editor.CurrentFileName);
            else if(!string.IsNullOrWhiteSpace(lastFile))
                openFileDialog.InitialDirectory = Path.GetDirectoryName(lastFile);

            openFileDialog.Filter = filter;

			if(openFileDialog.ShowDialog() == true)
            { 
                LoadFile(editor, openFileDialog.FileName);
            }

            return;
        }        
        
        private void LoadFile(ICodeEditor editor, string fileName)
        {
            var result = File.ReadAllText(fileName);

            if(!string.IsNullOrWhiteSpace(result))
            {
                editor.CurrentFileName = fileName;
                editor.JsonContent     = result;
            }

            return;
        }

        /// <summary>
        /// Save a file and set text of json editor
        /// </summary>
        /// <param name="editor"></param>
        private void SaveAsFile(ICodeEditor editor, string lastFile)
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
                    File.WriteAllText(saveFileDialog.FileName, content);
                    editor.CurrentFileName =  saveFileDialog.FileName;
                }
            }
            return;
        }

        /// <summary>
        /// Save a file and set text of json editor
        /// </summary>
        /// <param name="editor"></param>
        private void SaveFile(ICodeEditor editor, string lastFile)
        {
            var content = editor.JsonContent;

            if(!string.IsNullOrWhiteSpace(content))
            { 
			    if(!string.IsNullOrWhiteSpace(editor.CurrentFileName))
                {                 
                    File.WriteAllText(editor.CurrentFileName, editor.JsonContent);
                }
                else
                    SaveAsFile(editor, lastFile);
            }

            return;
        }

        private void LoadHighlighting()
        {
			IHighlightingDefinition customHighlighting;

			using (Stream s = typeof(MainWindow).Assembly.GetManifestResourceStream("JTranEdit.Assets.CustomHighlighting.xml")) 
            {
				if (s == null)
					throw new InvalidOperationException("Could not find embedded resource");

				using (XmlReader reader = new XmlTextReader(s)) 
                {
					customHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
						HighlightingLoader.Load(reader, HighlightingManager.Instance);
				}
			}

			// and register it in the HighlightingManager
			HighlightingManager.Instance.RegisterHighlighting("JTran", new string[] { ".jtran" }, customHighlighting);
	    }

        #endregion
    }
}
