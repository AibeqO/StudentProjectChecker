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
using System.Net.Mail;
using System.Net;

namespace StudentsProjectChecker
{
    /// <summary>
    /// Interaction logic for mailSendWindow.xaml
    /// </summary>
    public partial class mailSendWindow : Window
    {
        SQLrequests sr;
        KursachWindow.inputProjectData projectData;
        //Мне нужны:
        //Дата получения темы и защиты
        //Тема
        //Название предмета
        //ФИО препода
        //ФИО студента
        public mailSendWindow(KursachWindow.inputProjectData projectData, SQLrequests sr)
        {
            InitializeComponent();
            this.projectData = projectData;
            this.sr = sr;
            got_date.Content = projectData.recieved_date.ToShortDateString();
            defence_date.Content = projectData.topic_defence_date.ToShortDateString();
            theme.Content = projectData.topic;
            teacherName.Content = sr.getTeacherNameByID(projectData.TeacherID);
            studentName.Content = sr.getStudentNameByID(projectData.StudentID);
            subjectName.Content = sr.getSubjectNameByID(projectData.SubjectID);
            teacherEmail.Text = sr.getTeacherEmail(projectData.TeacherID);
        }

        private void sendMailButton_Click(object sender, RoutedEventArgs e)
        {
            string studentEmail = sr.getStudentEmailByID(projectData.StudentID);
            string studentPassword = sr.getStudentPasswordByID(projectData.StudentID);
            string studentName = sr.getStudentNameByID(projectData.StudentID);
            string teacherName = sr.getTeacherNameByID(projectData.TeacherID);
            string subjectName = sr.getSubjectNameByID(projectData.SubjectID);
            
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress(studentEmail,
                studentName);
            

            // кому отправляем
            MailAddress to = new MailAddress(teacherEmail.Text);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Работа студента " + studentName;
            // текст письма
            m.Body = "<h2>Здравствуйте, " + teacherName + ". Отправляю вам свою работу по предмету " + subjectName + ", на тему " + projectData.topic + ".</h2>";
            //Вложение
            Attachment attachment = new System.Net.Mail.Attachment(filePath);
            m.Attachments.Add(attachment);


            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential(studentEmail, studentPassword);
            smtp.EnableSsl = true;
            smtp.Send(m);

            sr.setRecievedTrue(projectData.SubjectID);
        }
        KursachWindow.DefaultDialogService defaultDialog;
        string filePath;
        private void loadFile(object sender, RoutedEventArgs e)
        {
            defaultDialog = new KursachWindow.DefaultDialogService();
            defaultDialog.OpenFileDialog();
            filePath = defaultDialog.FilePath;
        }
    }
}
