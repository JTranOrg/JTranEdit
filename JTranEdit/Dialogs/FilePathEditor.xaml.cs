using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JTranEdit.Dialogs
{
    /// <summary>
    /// Interaction logic for FilePathEditor.xaml
    /// </summary>
    public partial class FilePathEditor : UserControl
    {
        public List<string> FilePaths
        { 
            get { return (List<string>)GetValue(FilePathsDependencyProperty); }
            set { SetValue(FilePathsDependencyProperty, value); }
        }

        public static readonly DependencyProperty FilePathsDependencyProperty = 
            DependencyProperty.Register("FilePaths", typeof(List<string>), typeof(FilePathEditor), new PropertyMetadata(new List<string>()));

        public FilePathEditor()
        {
            InitializeComponent();
        }
    }
}
