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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        SQLrequests sr = new SQLrequests();
        DataTable dt = new DataTable();

        void initGroupName()
        {
            dt = sr.getGroupList();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                groupList.Items.Add(dt.Rows[i].ItemArray[0].ToString());
            }
            
        }

        void disableControls()
        {
            FirstName.IsEnabled = LastName.IsEnabled = patronymic.IsEnabled =
                groupList.IsEnabled = EMailtxt.IsEnabled = pwdTxt.IsEnabled =
               subjectGrid.IsEnabled = registerBtn.IsEnabled = false;
        }
        public Registration()
        {
            InitializeComponent();
            disableControls();
        }

        private void studentPicked_Checked(object sender, RoutedEventArgs e)
        {
            disableControls();
            initGroupName();

            FirstName.IsEnabled = LastName.IsEnabled = patronymic.IsEnabled =
                groupList.IsEnabled = EMailtxt.IsEnabled = pwdTxt.IsEnabled =
                registerBtn.IsEnabled = true;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //register
            if (teacherPicked.IsChecked.Value)
            {
                //Собрать названия предметов препода в контейнер
                List<string> _subjects = new List<string>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!dt.Rows[i].ItemArray[1].Equals(System.DBNull.Value))
                    {
                        if (Convert.ToBoolean(dt.Rows[i].ItemArray[1]))
                        {
                            _subjects.Add(Convert.ToString(dt.Rows[i].ItemArray[0]));
                        }
                    }
                }
                sr.TeacherRegister(LastName.Text, 
                                    FirstName.Text, 
                                    patronymic.Text, 
                                    EMailtxt.Text, 
                                    pwdTxt.Password, 
                                    _subjects);
            }
            if (studentPicked.IsChecked.Value)
            {
                sr.StudentRegister(LastName.Text,
                                    FirstName.Text,
                                    patronymic.Text,
                                    EMailtxt.Text,
                                    pwdTxt.Password,
                                    groupList.SelectedItem.ToString());
            }
            MessageBox.Show("Регистрация пользователя прошла успешно.");
        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            disableControls();
            FirstName.IsEnabled = LastName.IsEnabled = patronymic.IsEnabled =
                EMailtxt.IsEnabled = pwdTxt.IsEnabled =
                registerBtn.IsEnabled = subjectGrid.IsEnabled = true;

            subjectGrid.CanUserAddRows = false;
            dt = sr.getAllSubjects();
            dt.Columns["getallsubjects"].ColumnName = "Предмет";
            dt.Columns.Add("Выбор", typeof(bool));

            subjectGrid.ItemsSource = dt.DefaultView;

        }

        private void groupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedGroupName = groupList.SelectedItem.ToString();

        }
    }
}
