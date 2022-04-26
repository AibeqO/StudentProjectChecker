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
    /// Interaction logic for SelectionWindow.xaml
    /// </summary>
    public partial class SelectionWindow : Window
    {
        SQLrequests sr;
        int TeacherID, SubjectID;
        DataTable dt;
        public Tuple<int, int> getTeacherIDandSubjectID()
        {
            return new Tuple<int, int>(TeacherID, SubjectID);
        }
        public SelectionWindow()
        {
            InitializeComponent();
            button1.IsEnabled = false;
            button1_Copy.IsEnabled = false;
            sr = new SQLrequests();
            loadTeacherList();
        }

        void loadTeacherList()
        {
            dt = sr.getTeacherList();
            dt.Columns.Add("Выбор", typeof(bool));
            TeacherGrid.ItemsSource = dt.DefaultView;
        }

        void loadSubjectList()
        {
            //Получить предметы определенного препода
            dt = sr.getSubjectListOfTeacher(TeacherID);
            dt.Columns.Add("Выбор", typeof(bool));
            SubjectGrid.ItemsSource = dt.DefaultView;
        }

        void CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                TeacherGrid.IsEnabled = false;
                button1.IsEnabled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dt.Rows[i].ItemArray[2]))
                {
                    TeacherID = Convert.ToInt32(dt.Rows[i].ItemArray[0]);
                    MessageBox.Show(dt.Rows[i].ItemArray[1].ToString());
                    loadSubjectList();
                    return;
                }
            }
        }

        private void CellEditEndingSubject(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                SubjectGrid.IsEnabled = false;
                button1_Copy.IsEnabled = true;
            }
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((!Convert.IsDBNull(dt.Rows[i].ItemArray[2])) 
                    && Convert.ToBoolean(dt.Rows[i].ItemArray[2]))
                {
                    SubjectID = Convert.ToInt32(dt.Rows[i].ItemArray[0]);
                    MessageBox.Show(dt.Rows[i].ItemArray[1].ToString());
                    return;
                }
            }
        }
    }
}
