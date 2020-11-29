using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

using Microsoft.Win32;

namespace JTranEdit.Dialogs
{
    /// <summary>
    /// Interaction logic for PreferencesDialog.xaml
    /// </summary>
    public partial class PreferencesDialog : Window
    {
        private readonly Preferences _oldPreferences;
        private readonly Preferences _preferences;

        public PreferencesDialog(Preferences preferences)
        {
            InitializeComponent();

            _oldPreferences = new Preferences(preferences);
            this.DataContext = _preferences = preferences;

            ckShowOutlining.IsChecked = preferences.ShowOutlining;
        }

        #region Dependencies

        public static readonly DependencyProperty ShowGeneralDependencyProperty = 
            DependencyProperty.Register("ShowGeneral", typeof(bool), typeof(JsonEditor), new PropertyMetadata(true));

        public static readonly DependencyProperty ShowIncludeDependencyProperty = 
            DependencyProperty.Register("ShowInclude", typeof(bool), typeof(JsonEditor), new PropertyMetadata(false));

        public static readonly DependencyProperty ShowDocumentDependencyProperty = 
            DependencyProperty.Register("ShowDocument", typeof(bool), typeof(JsonEditor), new PropertyMetadata(false));

        #endregion

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _preferences.IncludePath       = _oldPreferences.IncludePath;      
            _preferences.DocumentPath      = _oldPreferences.DocumentPath;     
            _preferences.BracketCompletion = _oldPreferences.BracketCompletion;
            _preferences.Indent            = _oldPreferences.Indent;           
            _preferences.ShowLineNumbers   = _oldPreferences.ShowLineNumbers;  
            _preferences.ShowOutlining     = _oldPreferences.ShowOutlining;    
            _preferences.SaveOnTransform   = _oldPreferences.SaveOnTransform;  
            _preferences.AutoSave          = _oldPreferences.AutoSave;         

            this.Close();
        }

        private void Include_FolderOpen_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();

            dlg.SelectedPath = _preferences.IncludePath;

            if(dlg.ShowDialog() == true)
            {
                _preferences.IncludePath = dlg.SelectedPath;
                winIncludePath.Text = dlg.SelectedPath;
            }
        }

        private void Documents_FolderOpen_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();

            dlg.SelectedPath = _preferences.DocumentPath;

            if(dlg.ShowDialog() == true)
            {
                _preferences.DocumentPath = dlg.SelectedPath;
                winDocumentsPath.Text = dlg.SelectedPath;
            }
        }

        private void ckShowOutlining_Checked(object sender, RoutedEventArgs e)
        {
            _preferences.ShowOutlining = true;
            MainWindow.Instance.SetOutlining(true);
        }

        private void ckShowOutlining_Unchecked(object sender, RoutedEventArgs e)
        {
            _preferences.ShowOutlining = false;
            MainWindow.Instance.SetOutlining(false);
        }
    }
}
