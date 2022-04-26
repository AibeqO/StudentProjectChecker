using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Npgsql;
using System.Text.RegularExpressions;

namespace StudentsProjectChecker
{
    class inputDataChecker
    {
        Regex emailPattern = new Regex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");

        Regex fullNamePattern = new Regex(@"^[^a - zа - яё].*[\s\.]*$");

        public inputDataChecker (){ }

        bool isGoodFullName(string fullName)
        {
            return fullNamePattern.IsMatch(fullName);
        }

        bool isGoodEmail(string email)
        {
            return emailPattern.IsMatch(email);
        }

        bool isGoodPwd(string password)
        {
            return password.Length >= 8;
        }

        bool isGoodGroupName(string groupName)
        {
            return groupName != "";
        }

        public void checkInputData(string email, string password, string fullName)
        {
            if (!isGoodEmail(email))
            {
                MessageBox.Show("Неправильный формат электронной почты");
                return;
            }
            if (!isGoodPwd(password))
            {
                MessageBox.Show("Пароль должен состоять как минимум из 8 символов");
                return;
            }
            if (!isGoodFullName(fullName))
            {
                MessageBox.Show("ФИО должно состоять как минимум из трёх слов");
                return;
            }
        }

        public void checkInputData(string email, string password, string fullName, string groupName)
        {
            if (!isGoodEmail(email))
            {
                MessageBox.Show("Неправильный формат электронной почты");
                return;
            }
            if (!isGoodPwd(password))
            {
                MessageBox.Show("Пароль должен состоять как минимум из 8 символов");
                return;
            }
            if (!isGoodFullName(fullName))
            {
                MessageBox.Show("ФИО должно состоять как минимум из трёх слов");
                return;
            }
            if (!isGoodGroupName(groupName))
            {
                MessageBox.Show("Вы не выбрали группу");
                return;
            }
        }
    }

    public class SQLrequests
    {
        string connectitonString = "Host=localhost;" +
            "                       Port=5432;" +
                                    "Username=postgres;" +
            "                       Database=project_checkerdb;" +
            "                       PASSWORD = SA";
        private NpgsqlConnection connection;
        private NpgsqlCommand command;
        public SQLrequests()
        {
            command = new NpgsqlCommand();
            connection = new NpgsqlConnection(connectitonString);
            connection.Open();
        }
        ~SQLrequests() => connection.Close(); 
        //Авторизация препода
        public bool TeacherLogin(string email, string password, ref int teacherID)
        {
            int result = -1;
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectitonString))
                {
                    connection.Open();
                    string sql = @"Select t_login(:email, :password)";
                    command = new NpgsqlCommand(sql, connection);
                    command.Parameters.AddWithValue(":email", email);
                    command.Parameters.AddWithValue(":password", password);
                    result = Convert.ToInt32(command.ExecuteScalar());
                    if (result == 1)
                    {
                        sql = @"select id_teacher from teacher_auth where email = :mail";
                        command = new NpgsqlCommand(sql, connection);
                        command.Parameters.AddWithValue(":mail", email);
                        teacherID = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch(NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return (result == 1);
        }

        //Авторизация студента
        public bool StudentLogin(string email, string password, ref int id)
        {
            int result = -1;
            try
            {
                string sql = @"Select s_login(:email, :password)";
                command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue(":email", email);
                command.Parameters.AddWithValue(":password", password);
                result = Convert.ToInt32(command.ExecuteScalar());
                if (result == 1)
                {
                    sql = @"select id_student from student_auth where email = :mail";
                    command = new NpgsqlCommand(sql, connection);
                    command.Parameters.AddWithValue(":mail", email);
                    id = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return (result == 1);
        }

        public DataTable getAllSubjects()
        {
            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection connection = 
                    new NpgsqlConnection(connectitonString))
                {
                    connection.Open();
                    string sql = @"Select getAllSubjects()";
                    command = new NpgsqlCommand(sql, connection);

                    dt.Load(command.ExecuteReader());
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dt;
        }

        //Регистрация препода
        //=====================================================================
        int InsertedTeacher(string fullName, NpgsqlConnection connection)
        {
            string sqlCommand = "INSERT INTO teacher (name)" + " VALUES(@name) RETURNING id_teacher;";
            command = new NpgsqlCommand(sqlCommand, connection);

            command.Parameters.AddWithValue("@name", fullName);

            //ID препода
            return (int)command.ExecuteScalar();
        }
        
        int[] teachersSubjectID(DataTable dt, NpgsqlConnection connection, List<string> subjects)
        {
            string sqlCommand = "SELECT getSubjectIDsByName(:nameList);";
            command = new NpgsqlCommand(sqlCommand, connection);
            command.Parameters.AddWithValue("@nameList", subjects.ToArray());

            dt.Load(command.ExecuteReader());

            //ID предметов
            return dt.Rows[0].ItemArray[0] as int[];
        }

        void addTeachersSubjects(NpgsqlConnection connection, int[] sub_id, int t_id)
        {
            string sqlCommand = "CALL insertTeachersSubject(:id_sub, :id_teacher);";
            command = new NpgsqlCommand(sqlCommand, connection);
            command.Parameters.AddWithValue("@id_sub", sub_id);
            command.Parameters.AddWithValue("@id_teacher", t_id);
            command.ExecuteNonQuery();
        }

        void insertTeacherAuthData(string email, string pwd, int id)
        {
            string sqlCommand = "CALL insertTeacherAuthData(:id, :email, :pwd);";
            command = new NpgsqlCommand(sqlCommand, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@pwd", pwd);
            command.ExecuteNonQuery();
        }

        public void TeacherRegister(string lastName, string firstName, string patronymic,
                                        string email, string password, List<string> subjects)
        {
            inputDataChecker d_checker = new inputDataChecker();
            DataTable dt = new DataTable();
            try
            {
                    string fullName = lastName + ' ' + firstName + ' ' + patronymic;
                    //Проверка данных
                    d_checker.checkInputData(email, password, fullName);
                    
                    //Добавить ФИО препода, с полуением его ID
                    int insertedTeacherID = InsertedTeacher(fullName, connection);

                    //Добавить данные для авторизации препода
                    insertTeacherAuthData(email, password, insertedTeacherID);

                    //Получить айдишники предмета
                    int[] sub_id = teachersSubjectID(dt, connection, subjects);

                    //Добавить предметы препода
                    addTeachersSubjects(connection, sub_id, insertedTeacherID);
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        //=====================================================================

        //Регистрация студента

        //Получить список всех групп
        public DataTable getGroupList()
        {
            DataTable dataTable = new DataTable();

            string sqlCommand = "SELECT getallgroupname();";

            command = new NpgsqlCommand(sqlCommand, connection);

            dataTable.Load(command.ExecuteReader());

            return dataTable;
        }

        //Добавить студента в базу
        int insertStudent(string fullName, int groupID)
        {
            string sqlCommand = "INSERT INTO students (name, id_group)" + " VALUES(@fullName, @idGroup) RETURNING id_student;";
            command = new NpgsqlCommand(sqlCommand, connection);

            command.Parameters.AddWithValue("@fullName", fullName);
            command.Parameters.AddWithValue("@idGroup", groupID);

            //ID студента
            return (int)command.ExecuteScalar();
        }

        //Получить ID группы по названию
        private int getGroupIDByGroupName(string groupName)
        {
            string sqlCommand = "SELECT id_group FROM student_groups WHERE group_name = :g_name;";
            
            command = new NpgsqlCommand(sqlCommand, connection);

            command.Parameters.AddWithValue("@g_name", groupName);

            int groupID = Convert.ToInt32(command.ExecuteScalar());

            return groupID;
        }

        //Добавить его данные для авторизации
        void insertStudentAuthData(string email, string password, int insertedStudentID)
        {
            string sqlCommand = "CALL insertstudentauthdata(:email, :pwd, :id);";

            command = new NpgsqlCommand(sqlCommand, connection);

            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@pwd", password);
            command.Parameters.AddWithValue("@id", insertedStudentID);

            command.ExecuteNonQuery();
        }

        //Зарегистрировать студента
        public void StudentRegister(string lastName, string firstName, string patronymic,
                                    string email, string password, string groupName)
        {
            inputDataChecker d_checker = new inputDataChecker();
            try
            {
                string fullName = lastName + ' ' + firstName + ' ' + patronymic;

                //Проверка данных
                d_checker.checkInputData(email, password, fullName, groupName);

                //Получить ID группы
                int groupID = getGroupIDByGroupName(groupName);

                //Добавить студента в базу
                int studentID = insertStudent(fullName, groupID);

                //Добавить его данные для авторизации
                insertStudentAuthData(email, password, studentID);
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        //=====================================================================

        public DataTable getTeacherList()
        {
            DataTable dt = new DataTable();
            try
            {
                string sqlCommand = "SELECT * FROM getallteachers()";
                command = new NpgsqlCommand(sqlCommand, connection);

                dt.Load(command.ExecuteReader());
            }
            catch(NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dt;
        }

        public DataTable getSubjectListOfTeacher(int id)
        {
            string sqlCommand = "SELECT * FROM getSubjectsOfTeacher(:id)";
            command = new NpgsqlCommand(sqlCommand, connection);
            command.Parameters.AddWithValue(":id", id);
            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
            return dt;
        }

        public string getStudentEmailByID(int ID)
        {
            string sqlCommand = "SELECT email FROM student_auth where id_student = :ID";

            command = new NpgsqlCommand(sqlCommand, connection);
            command.Parameters.AddWithValue(":ID", ID);

            return command.ExecuteScalar().ToString();
        }

        public string getStudentPasswordByID(int ID)
        {
            string sqlCommand = "SELECT pwd FROM student_auth where id_student = :ID";

            command = new NpgsqlCommand(sqlCommand, connection);
            command.Parameters.AddWithValue(":ID", ID);

            return command.ExecuteScalar().ToString();
        }

        public string getTeacherNameByID(int ID)
        {
            
            string sqlCommand = "SELECT getTeacherNameByID(:ID)";

            command = new NpgsqlCommand(sqlCommand, connection);
            command.Parameters.AddWithValue(":ID", ID);

            return command.ExecuteScalar().ToString();
        }

        public string getStudentNameByID(int ID)
        {

            string sqlCommand = "SELECT getStudentNameByID(:ID)";

            command = new NpgsqlCommand(sqlCommand, connection);
            command.Parameters.AddWithValue(":ID", ID);

            return command.ExecuteScalar().ToString();
        }

        public string getSubjectNameByID(int ID)
        {
            string sqlCommand = "SELECT getSubjectNameByID(:ID)";

            command = new NpgsqlCommand(sqlCommand, connection);
            command.Parameters.AddWithValue(":ID", ID);

            return command.ExecuteScalar().ToString();
        }

        //=====================================================================


        public void InsertProjectData(KursachWindow.inputProjectData projectData)
        {
            try
            {
                string sqlCommand = "CALL addproject (:id_subject, :id_teacher, :id_student, :project_name" +
                ", :recieved_date, :completion_date)";

                command = new NpgsqlCommand(sqlCommand, connection);
                command.Parameters.AddWithValue(":id_subject", projectData.SubjectID);
                command.Parameters.AddWithValue(":id_teacher", projectData.TeacherID);
                command.Parameters.AddWithValue(":id_student", projectData.StudentID);
                command.Parameters.AddWithValue(":project_name", projectData.topic);
                command.Parameters.AddWithValue(":recieved_date", projectData.recieved_date);
                command.Parameters.AddWithValue(":completion_date", projectData.topic_defence_date);

                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public DataTable getDataOfProject(int teacherID)
        {
            DataTable dt = new DataTable();
            try
            {
                string sqlCommand = "SELECT * FROM getProjectQuery(:ID)";

                command = new NpgsqlCommand(sqlCommand, connection);
                command.Parameters.AddWithValue(":ID", teacherID);

                dt.Load(command.ExecuteReader());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dt;
        }

        public DataTable findProjectsByGroupName(string groupName, int teacherID)
        {
            DataTable dt = new DataTable();
            try
            {
                string sqlCommand = "SELECT * FROM getProjectByGroupName(:ID, :groupName)";

                command = new NpgsqlCommand(sqlCommand, connection);
                command.Parameters.AddWithValue(":ID", teacherID);
                command.Parameters.AddWithValue(":groupName", groupName);

                dt.Load(command.ExecuteReader());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dt;
        }

        public DataTable findProjectsByStudentName(string studentName, int teacherID)
        {
            DataTable dt = new DataTable();
            try
            {
                string sqlCommand = "SELECT * FROM getProjectByStudentName(:ID, :studentName)";

                command = new NpgsqlCommand(sqlCommand, connection);
                command.Parameters.AddWithValue(":ID", teacherID);
                command.Parameters.AddWithValue(":studentName", studentName);

                dt.Load(command.ExecuteReader());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dt;
        }

        public string getTeacherEmail(int teacherID)
        {
            string teacherEmail = "";
            try
            {
                string sqlCommand = "SELECT email FROM teacher_auth WHERE id_teacher = :ID";

                command = new NpgsqlCommand(sqlCommand, connection);
                command.Parameters.AddWithValue(":ID", teacherID);
                teacherEmail = command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return teacherEmail;
        }

        public void setRecievedTrue(int id_subject)
        {
            try
            {
                string sqlCommand = "UPDATE project SET recieved = true WHERE project.id_subject = :ID; ";

                command = new NpgsqlCommand(sqlCommand, connection);
                command.Parameters.AddWithValue(":ID", id_subject);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
