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
using Microsoft.Win32;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;
using Window = System.Windows.Window;
using Range = Microsoft.Office.Interop.Word.Range;
using Word = Microsoft.Office.Interop.Word;
using Paragraph = Microsoft.Office.Interop.Word.Paragraph;

namespace StudentsProjectChecker
{
    /// <summary>
    /// Логика взаимодействия для KursachWindow.xaml
    /// </summary>
    /// 
    public partial class KursachWindow : Window
    {
        int loggedStudentID = 0;
        //===========================================
        collector collectorDataFromFile;
        checker fileChecker;

        //===========================================
        DefaultDialogService defaultDialog;
        public WordFile file;
        //===========================================

        static HeadingsList secondWindow;
        static SelectionWindow selectionWindow;
        static mailSendWindow mailSendWindow;
        //===========================================
        SQLrequests sr;
        public interface IDialogService
        {
            void ShowMessage(string message);   // показ сообщения
            bool OpenFileDialog();  // открытие файла
            bool SaveFileDialog();  // сохранение файла
        }

        public class DefaultDialogService : IDialogService
        {
            public string FilePath { get; set; }
            OpenFileDialog openFileDialog;
            SaveFileDialog saveFileDialog;
            public bool OpenFileDialog()
            {
                openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Word Files|*.doc;*.docx";
                if (openFileDialog.ShowDialog() == true)
                {
                    FilePath = openFileDialog.FileName;
                    return true;
                }
                return false;
            }

            public bool SaveFileDialog()
            {
                saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                {
                    FilePath = saveFileDialog.FileName;
                    return true;
                }
                return false;
            }

            public void ShowMessage(string message)
            {
                MessageBox.Show(message);
            }
        }


        public KursachWindow(int studentID)
        {
            loggedStudentID = studentID;
        }

        public KursachWindow()
        {
            InitializeComponent();
            file = new WordFile();
            defaultDialog = new DefaultDialogService();
            sr = new SQLrequests();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //file = new WordFile();
            defaultDialog.OpenFileDialog();
            file.openFile(defaultDialog.FilePath);
            collectorDataFromFile = new collector();
            fileChecker = new checker();
            fileChecker.fileToCollectDataFrom = file;
            collectorDataFromFile.fileToCollectDataFrom = file;
            secondWindow = new HeadingsList(file);
        }

        //Начать проверку
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            fileChecker.Init(file);
            fileChecker.Run(checkBoxes);
            MessageBox.Show("Проверка закончена!");
            file.closeFile();
            file.closeApp();
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ShowResults secondWindow = new ShowResults(fileChecker);
            secondWindow.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //Открыть окно со списком оглавлений
            secondWindow = new HeadingsList(file);
            secondWindow.Show();
        }

        //Запомнить данные о проекте
        
        public struct inputProjectData
        {
            public int TeacherID;
            public int SubjectID;
            public int StudentID;
            public string topic;
            public DateTime recieved_date;
            public DateTime topic_defence_date;
            public inputProjectData(int TeacherID, 
                                    int SubjectID, 
                                    string topic, 
                                    DateTime rd, 
                                    DateTime tdd,
                                    int studentID) 
            {
                this.TeacherID = TeacherID;
                this.SubjectID = SubjectID;
                this.topic = topic;
                recieved_date = rd;
                topic_defence_date = tdd;
                this.StudentID = studentID;
            }
        };

        public inputProjectData projectData;

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            projectData = new inputProjectData
                (selectionWindow.getTeacherIDandSubjectID().Item1,
                selectionWindow.getTeacherIDandSubjectID().Item2,
                topicName.Text,
                (DateTime)recievedDate.SelectedDate,
                (DateTime)topicDefenceDate.SelectedDate,
                loggedStudentID);
            sr.InsertProjectData(projectData);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (file != null && file.isOpen())
            {
                file.closeFile();
                file.closeApp();
            }
            System.Windows.Application.Current.Shutdown();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            //Открыть форму с выбором списка предметов
            selectionWindow.Show();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            //Открыть форму с выбором преподавателей которые ведут выбранный предмет
            selectionWindow = new SelectionWindow();
            selectionWindow.Show();
        }

        private void sendMail(object sender, RoutedEventArgs e)
        {
            //Мне нужны:
            //Дата получения темы и защиты
            //Тема
            //Название предмета
            //ФИО препода
            //ФИО студента
            //Открыть форму по отправке письма преподу
            mailSendWindow = new mailSendWindow(projectData, sr);
            mailSendWindow.Show();
            
        }
        List<bool> checkBoxes = new List<bool>();
        private void settingsPage(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings(this);
            settings.Show();
        }

        public void setCheckBoxes(List<bool> cb)
        {
            checkBoxes = cb;
        }
    }
}
