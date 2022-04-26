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
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        SQLrequests _sqlrequests;
        Registration regForm;
        KursachWindow kw;
        TeacherWindow tw;
        public Auth()
        {
            InitializeComponent();
            _sqlrequests = new SQLrequests();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int teacherID = 0;
            //teacherAuth
            if (_sqlrequests.TeacherLogin(email.Text, pwdBox.Password, ref teacherID))
            {
                MessageBox.Show("Авторизация прошла успешно.", "Успешная авторизация");
                tw = new TeacherWindow(teacherID);
                tw.Show();
                this.Close();
                return;
            }
            MessageBox.Show("Вы ввели неверные данные для авторизации.", "Ошибка авторизации");
            return;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (email.Text == "administrator" && pwdBox.Password == "FgaA2Mu65g4awXPT")
            {
                MessageBox.Show("Авторизация прошла успешно.", "Успешная авторизация");
                regForm = new Registration();
                regForm.Show();
                return;
            }
            MessageBox.Show("Вы ввели неверные данные для авторизации.", "Ошибка авторизации");
            return;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int loggedStudentID = 0;
            if (_sqlrequests.StudentLogin(email.Text, pwdBox.Password, ref loggedStudentID))
            {
                MessageBox.Show("Авторизация прошла успешно.", "Успешная авторизация");
                kw = new KursachWindow(loggedStudentID);
                kw.Show();
                this.Close();
                return;
            }
            MessageBox.Show("Вы ввели неверные данные для авторизации.", "Ошибка авторизации");
            return;
        }
    }
}
