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
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;
using Window = System.Windows.Window;
namespace StudentsProjectChecker
{
    /// <summary>
    /// Логика взаимодействия для HeadingsList.xaml
    /// </summary>
    public partial class HeadingsList : Window
    {
        WordFile _file;

        public System.Array getHeadings()
        {
            return (System.Array)_file.getDocument.GetCrossReferenceItems(WdReferenceType.wdRefTypeHeading);
        }

        public HeadingsList(WordFile file)
        {
            InitializeComponent();

            _file = file;

            if (_file.getDocument != null)
            {
                headings.ItemsSource = getHeadings().Cast<string>().ToList();
            }
        }
    }
}
