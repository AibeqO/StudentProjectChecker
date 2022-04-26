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
using System.Windows.Shapes;

namespace StudentsProjectChecker
{

    public partial class Settings : Window
    {
        KursachWindow kw;
        List<bool> checkBoxes = new List<bool>();
        public Settings(KursachWindow kw)
        {
            InitializeComponent();
            this.kw = kw;
        }
        public Settings() { }
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            checkBoxes.Add(Convert.ToBoolean(FirstLvlHeading.IsChecked));
            checkBoxes.Add(Convert.ToBoolean(SecondLvlHeading.IsChecked));
            checkBoxes.Add(Convert.ToBoolean(ThirdLvlHeading.IsChecked));
            checkBoxes.Add(Convert.ToBoolean(FourthLvlHeading.IsChecked));
            checkBoxes.Add(Convert.ToBoolean(ParagraphColor.IsChecked));
            checkBoxes.Add(Convert.ToBoolean(ParagraphFontName.IsChecked));
            checkBoxes.Add(Convert.ToBoolean(ParagraphStyle.IsChecked));
            checkBoxes.Add(Convert.ToBoolean(ParagraphSize.IsChecked));
            checkBoxes.Add(Convert.ToBoolean(ParagraphAlignment.IsChecked));
            checkBoxes.Add(Convert.ToBoolean(FirstLineParagraphIndent.IsChecked));
            checkBoxes.Add(Convert.ToBoolean(IndentBeforeAndAfterImage.IsChecked));
            checkBoxes.Add(Convert.ToBoolean(ParagraphAboveTable.IsChecked));
            checkBoxes.Add(Convert.ToBoolean(BeforeAndAfterParagraphIndent.IsChecked));
            checkBoxes.Add(Convert.ToBoolean(ListProperties.IsChecked));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            kw.setCheckBoxes(checkBoxes);
        }
    }
}
