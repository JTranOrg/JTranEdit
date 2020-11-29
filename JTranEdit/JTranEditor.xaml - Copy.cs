using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Text.Json;
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
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

using JTran.Syntax;
using System.CodeDom;

namespace JTranEdit
{
    /// <summary>
    /// Interaction logic for JTranEditor.xaml
    /// </summary>
    public partial class JTranEditor : UserControl, ICodeEditor
    {
        private readonly IFoldingStrategy _foldingStrategy;
        private readonly ICodeCompletion _codeCompletion;

        public JTranEditor()
        {
            InitializeComponent();

            _foldingStrategy = new JsonFoldingStrategies(winEditBox, null);
            _codeCompletion = new JTranCodeCompletion(this);

            winEditBox.TextArea.TextEntering += winEditBox_TextArea_TextEntering;
            winEditBox.TextArea.TextEntered += winEditBox_TextArea_TextEntered;

            winEditBox.TextArea.SelectionCornerRadius = 0;
            winEditBox.TextArea.SelectionForeground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            winEditBox.TextArea.SelectionBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(153, 201, 239));
            winEditBox.TextArea.SelectionBorder = new System.Windows.Media.Pen(winEditBox.TextArea.SelectionBrush, 0);
        }

        public Preferences Preferences 
        { 
            get { return (Preferences)GetValue(PreferencesDependencyProperty); }
            set 
            { 
                SetValue(PreferencesDependencyProperty, value); 
                _foldingStrategy.Preferences = value;
            }
        }

        public TextEditor  TextBox          => winEditBox;
        public string      SelectedText     { get; set; }
        public string      CurrentFileName  { get; set; }

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
                this.UpdateFoldings();
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

        #region Dependencies

        public static readonly DependencyProperty TitleDependencyProperty = 
            DependencyProperty.Register("Title", typeof(string), typeof(JTranEditor), new PropertyMetadata("Title"));

        public static readonly DependencyProperty Button1TooltipDependencyProperty = 
            DependencyProperty.Register("Button1Tooltip", typeof(string), typeof(JTranEditor), new PropertyMetadata("Load File"));

        public static readonly DependencyProperty Button2TooltipDependencyProperty = 
            DependencyProperty.Register("Button2Tooltip", typeof(string), typeof(JTranEditor), new PropertyMetadata("Save File"));

        public static readonly DependencyProperty Button3TooltipDependencyProperty = 
            DependencyProperty.Register("Button3Tooltip", typeof(string), typeof(JTranEditor), new PropertyMetadata("Save As File"));

        public static readonly DependencyProperty Button1ImageDependencyProperty = 
            DependencyProperty.Register("Button1Image", typeof(string), typeof(JTranEditor), new PropertyMetadata("Assets/Buttons/glyphicons-openfile.png"));

        public static readonly DependencyProperty EditBoxMarginDependencyProperty = 
            DependencyProperty.Register("EditBoxMargin", typeof(Thickness), typeof(JTranEditor), new PropertyMetadata(new Thickness(0, 0, 0, 0)));

        public static readonly DependencyProperty PreferencesDependencyProperty = 
            DependencyProperty.Register("Preferences", typeof(Preferences), typeof(JTranEditor), new PropertyMetadata(null));

        #endregion

        public event RoutedEventHandler FirstButtonClick;
        public event RoutedEventHandler SecondButtonClick;
        public event RoutedEventHandler ThirdButtonClick;

        public void UpdateFoldings()
        {
            _foldingStrategy.UpdateFoldings(winEditBox.Document);
        }

        #region Code Completion

        void winEditBox_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if(_codeCompletion.OnTextEntered(e.Text))
                _foldingStrategy.UpdateFoldings(winEditBox.Document);
        }

        void winEditBox_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            _codeCompletion.OnTextEntering(e);
        }        

        #endregion

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
    }
}
