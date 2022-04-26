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
    /// <summary>
    /// Логика взаимодействия для ShowResults.xaml
    /// </summary>
    
    public partial class ShowResults : Window
    {
        
        public ShowResults(checker ch)
        {
            InitializeComponent();
            
            for (int i = 0; i < ch.rLog.Count; i++) //Количество типа ошибок
            {
                resultsView.AppendText(ch.rLog[i].errorType + ":" + Environment.NewLine);
                for (int j = 0; j < ch.rLog[i].errorMessages.Count; j++)
                {
                    string str = (ch.rLog[i].errorMessages[j] + Environment.NewLine);
                    resultsView.AppendText(str);
                }
            }
        }
    }
}
