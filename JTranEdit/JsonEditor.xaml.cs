using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ICSharpCode.AvalonEdit;

namespace JTranEdit
{
    /// <summary>
    /// Interaction logic for JsonEditor.xaml
    /// </summary>
    public partial class JsonEditor : UserControl, ICodeEditor
    {
        public JsonEditor()
        {
            InitializeComponent();

            winEditBox.TextArea.TextEntering += winEditBox_TextArea_TextEntering;
            winEditBox.TextArea.TextEntered += winEditBox_TextArea_TextEntered;

            winEditBox.TextArea.SelectionCornerRadius = 0;
            winEditBox.TextArea.SelectionForeground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            winEditBox.TextArea.SelectionBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(153, 201, 239));
            winEditBox.TextArea.SelectionBorder = new System.Windows.Media.Pen(winEditBox.TextArea.SelectionBrush, 0);

            this.IsReadOnly = this.IsReadOnly;
        }

        public Preferences Preferences 
        { 
            get { return (Preferences)GetValue(PreferencesDependencyProperty); }
            set { SetValue(PreferencesDependencyProperty, value); }
        }

        public TextEditor         TextBox          => winEditBox;
        public string             SelectedText     { get; set; }
        public string             CurrentFileName  { get; set; }
        public JsonEditViewModel  ViewModel        
        { 
            get
            {
                return this.DataContext as JsonEditViewModel;
            }

            set
            {
                value.IsEditable = !this.IsReadOnly;
                winEditBox.DataContext = value;
                this.DataContext = value;
            }
        }

        public string Title 
        { 
            get { return (string)GetValue(TitleDependencyProperty); }
            set { SetValue(TitleDependencyProperty, value); }
        }

        public string Button1Tooltip 
        { 
            get { return (string)GetValue(Button1TooltipDependencyProperty); }
            set { SetValue(Button1TooltipDependencyProperty, value); }
        }

       public string Button1Image
        { 
            get { return (string)GetValue(Button1ImageDependencyProperty); }
            set { SetValue(Button1ImageDependencyProperty, value); }
        }

        public string Button2Tooltip 
        { 
            get { return (string)GetValue(Button2TooltipDependencyProperty); }
            set { SetValue(Button2TooltipDependencyProperty, value); }
        }

        public string Button3Tooltip 
        { 
            get { return (string)GetValue(Button3TooltipDependencyProperty); }
            set { SetValue(Button3TooltipDependencyProperty, value); }
        }

        public string JsonContent 
        { 
            get
            { 
                 return winEditBox.Text;
            }

            set 
            { 
                winEditBox.Text = value;
                this.ViewModel.UpdateFoldings(winEditBox.Document);
            }
        }
 
        public Thickness EditBoxMargin 
        { 
            get { return (Thickness)GetValue(EditBoxMarginDependencyProperty); }
            set 
            { 
                SetValue(EditBoxMarginDependencyProperty, value); 
            }
        }

        public bool IsReadOnly 
        { 
            get { return (bool)GetValue(ReadOnlyDependencyProperty); }
            set 
            { 
                SetValue(ReadOnlyDependencyProperty, value); 

                if(this.ViewModel != null)
                     this.ViewModel.IsEditable = !value;
            }
        }

        public void UpdateFoldings()
        {
          this.ViewModel.UpdateFoldings(winEditBox.Document);
        }

        #region Dependencies

        public static readonly DependencyProperty TitleDependencyProperty = 
            DependencyProperty.Register("Title", typeof(string), typeof(JsonEditor), new PropertyMetadata("Title"));

        public static readonly DependencyProperty Button1TooltipDependencyProperty = 
            DependencyProperty.Register("Button1Tooltip", typeof(string), typeof(JsonEditor), new PropertyMetadata("Load File"));

        public static readonly DependencyProperty Button2TooltipDependencyProperty = 
            DependencyProperty.Register("Button2Tooltip", typeof(string), typeof(JsonEditor), new PropertyMetadata("Save File"));

        public static readonly DependencyProperty Button3TooltipDependencyProperty = 
            DependencyProperty.Register("Button3Tooltip", typeof(string), typeof(JsonEditor), new PropertyMetadata("Save As File"));

        public static readonly DependencyProperty Button1ImageDependencyProperty = 
            DependencyProperty.Register("Button1Image", typeof(string), typeof(JsonEditor), new PropertyMetadata("Assets/Buttons/glyphicons-openfile.png"));

        public static readonly DependencyProperty EditBoxMarginDependencyProperty = 
            DependencyProperty.Register("EditBoxMargin", typeof(Thickness), typeof(JsonEditor), new PropertyMetadata(new Thickness(0, 0, 0, 0)));

        public static readonly DependencyProperty PreferencesDependencyProperty = 
            DependencyProperty.Register("Preferences", typeof(Preferences), typeof(JsonEditor), new PropertyMetadata(null));

        public static readonly DependencyProperty ReadOnlyDependencyProperty = 
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(JsonEditor), new PropertyMetadata(false));

        #endregion

        public event RoutedEventHandler FirstButtonClick;
        public event RoutedEventHandler SecondButtonClick;
        public event RoutedEventHandler ThirdButtonClick;

        #region Code Completion

        void winEditBox_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            this.ViewModel.OnTextEntered(e, winEditBox.Document);
        }

        void winEditBox_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            this.ViewModel.OnTextEntering(e);
        }

        #endregion

        #region Event Handlers

        #region Button Click Handlers

        protected void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            FirstButtonClick(sender, e);
        }

        protected void btnSecond_Click(object sender, RoutedEventArgs e)
        {
            SecondButtonClick(sender, e);
        }

        protected void btnThird_Click(object sender, RoutedEventArgs e)
        {
            ThirdButtonClick(sender, e);
        }

        #endregion

        #region Menu Item Handlers

        private void menuCollapse_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.CollapseAll();
        }

        private void menuExpand_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.ExpandAll();
        }

        #endregion

        #endregion
    }
}
