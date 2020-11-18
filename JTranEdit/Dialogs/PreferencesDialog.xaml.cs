using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JTranEdit.Dialogs
{
    /// <summary>
    /// Interaction logic for PreferencesDialog.xaml
    /// </summary>
    public partial class PreferencesDialog : Window
    {
        public PreferencesDialog(Preferences preferences)
        {
            InitializeComponent();

            this.DataContext = preferences;
        }

        public bool ShowGeneral 
        { 
            get { return (bool)GetValue(ShowGeneralDependencyProperty); }
            set 
            { 
                SetValue(ShowGeneralDependencyProperty, value); 

                GeneralPane.Visibility = value ? Visibility.Visible : Visibility.Collapsed;

                if(value)
                {
                    this.ShowInclude = false;
                    this.ShowDocument = false;
                }
            }
        }

        public bool ShowInclude 
        { 
            get { return (bool)GetValue(ShowIncludeDependencyProperty); }
            set 
            { 
                SetValue(ShowIncludeDependencyProperty, value); 
                IncludePane.Visibility = value ? Visibility.Visible : Visibility.Collapsed;

                if(value)
                {
                    this.ShowGeneral = false;
                    this.ShowDocument = false;
                }
            }
        }

        public bool ShowDocument 
        { 
            get { return (bool)GetValue(ShowDocumentDependencyProperty); }
            set 
            { 
                SetValue(ShowDocumentDependencyProperty, value); 
                DocumentPane.Visibility = value ? Visibility.Visible : Visibility.Collapsed;

                if(value)
                {
                    this.ShowInclude = false;
                    this.ShowGeneral = false;
                }
            }
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
            this.Close();
        }
    }
}
