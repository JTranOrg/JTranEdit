using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Xml;

using System.Diagnostics;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;

using MondoCore.Common;
using System.Reflection.Metadata.Ecma335;

using JTranEdit.Dialogs;
using System.ComponentModel;
using System.Windows.Controls;

namespace JTranEdit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Preferences _preferences = new Preferences();
        public static MainWindow Instance;

        public MainWindow()
        {
            var docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            Instance = this;

            _preferences.ShowLineNumbers = JTranEdit.user.Default.ShowLineNumbers;
            _preferences.ShowOutlining   = JTranEdit.user.Default.ShowOutlining;
            _preferences.SaveOnTransform = JTranEdit.user.Default.SaveOnTransform;
            _preferences.AutoSave        = JTranEdit.user.Default.AutoSave;

            if(!string.IsNullOrWhiteSpace(JTranEdit.user.Default.LastIncludeFile))
                _preferences.IncludePath = Path.GetDirectoryName(JTranEdit.user.Default.LastIncludeFile);
            else
                _preferences.IncludePath = docPath;

            if(!string.IsNullOrWhiteSpace(JTranEdit.user.Default.LastDocumentFile))
                _preferences.DocumentPath = Path.GetDirectoryName(JTranEdit.user.Default.LastDocumentFile);
            else
                _preferences.DocumentPath = docPath;

            LoadHighlighting();
            InitializeComponent();

            InitEditor(SourcePane,      "LastSourceFile");
            InitEditor(TransformPane,   "LastTransformFile");
            InitEditor(OutputPane,      "LastOutputFile");
            InitEditor(DocumentsPane,   "LastDocumentFile");
            InitEditor(IncludesPane,    "LastIncludeFile");
        }

        private void InitEditor(ICodeEditor editor, string settingName)
        {        
            editor.Preferences = _preferences;
            editor.ViewModel   = new JsonEditViewModel(editor.TextBox, new JsonCodeCompletion(editor), _preferences) { SettingName = settingName };
            
            var lastFile = JTranEdit.user.Default[settingName]?.ToString() ?? "";

            if(!string.IsNullOrWhiteSpace(lastFile))
            {
                try
                { 
                    editor.LoadFile(lastFile);
                }
                catch
                {
                    // If can't load last file then just ignore

                    // Set last file to nothing
                    JTranEdit.user.Default[settingName] = "";
                }
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

        public void SetOutlining(bool outline)
        {
            SourcePane.Outlining    = outline;
            TransformPane.Outlining = outline;
            OutputPane.Outlining    = outline;
            DocumentsPane.Outlining = outline;
            IncludesPane.Outlining  = outline;
        }

        #region Button Click Handlers

        private const string JsonFilter  = "Json Files (*.json)|*.json";
        private const string JTranFilter = "Json Files (*.json)|*.json|JTran Files (*.jtran)|*.jtran";

        private void Pane_Click_LoadFile(object sender, RoutedEventArgs e)
        {
            var editor = (((sender as FrameworkElement).Parent as FrameworkElement).Parent as FrameworkElement).Parent as JsonEditor;

            editor.LoadFile(JTranFilter, JTranEdit.user.Default[editor.ViewModel.SettingName].ToString());
        }

        private void OutputPane_FirstButtonClick(object sender, RoutedEventArgs e)
        {
            DoTransform();
        }

        private void Pane_Click_Save(object sender, RoutedEventArgs e)
        {
            var editor = (((sender as FrameworkElement).Parent as FrameworkElement).Parent as FrameworkElement).Parent as JsonEditor;

            editor.SaveFile(JTranEdit.user.Default[editor.ViewModel.SettingName].ToString());
        }

        private void Pane_Click_SaveAs(object sender, RoutedEventArgs e)
        {
            var editor = (((sender as FrameworkElement).Parent as FrameworkElement).Parent as FrameworkElement).Parent as JsonEditor;

            editor.SaveAsFile(JTranEdit.user.Default[editor.ViewModel.SettingName].ToString());
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
                    SourcePane.SaveFile("Sources");
                    TransformPane.SaveFile("Transforms");
                    DocumentsPane.SaveFile("Documents");
                    IncludesPane.SaveFile("Includes");
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
