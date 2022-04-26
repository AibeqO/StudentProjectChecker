using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for TeacherWindow.xaml
    /// </summary>
    public partial class TeacherWindow : Window
    {
        SQLrequests sr; int teacherID;
        public TeacherWindow(int teacherID)
        {
            InitializeComponent();
            sr = new SQLrequests();
            this.teacherID = teacherID;
        }

        private void finButton_Click(object sender, RoutedEventArgs e)
        {
            projectDataGrid.ItemsSource = sr.getDataOfProject(teacherID).DefaultView;
        }

        private void findByGroupName_Checked(object sender, RoutedEventArgs e)
        {
            string groupName = dataForFind.Text;

        }
    }
}
