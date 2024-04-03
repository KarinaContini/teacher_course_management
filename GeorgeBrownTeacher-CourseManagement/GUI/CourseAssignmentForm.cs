using GeorgeBrownTeacher_CourseManagement.Business_DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeorgeBrownTeacher_CourseManagement.GUI
{
    public partial class CourseAssignmentForm : Form
    {
        public CourseAssignmentForm()
        {
            InitializeComponent();
            listTeachers();
            listCourses();
            listRegistrations();
            setUserTab();
        }


        // ------------------------------------ Teachers ---------------------------------
        private void listTeachers()
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {
                    var selectTeachers = from teachers in context.Teachers
                                         select teachers;

                    dataGridView_Teachers.ClearSelection();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Teacher ID");
                    dt.Columns.Add("First Name");
                    dt.Columns.Add("Last Name");
                    dt.Columns.Add("Email");


                    foreach (var teacher in selectTeachers)
                    {
                        dt.Rows.Add(teacher.TeacherId, teacher.FirstName, teacher.LastName, teacher.Email);
                    }

                    dataGridView_Teachers.DataSource = dt;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void clearTeacher()
        {
            txt_teacherID.Text = "";
            txt_firstName.Text = "";
            txt_LastName.Text = "";
            txt_Email.Text = "";
        }

        private void btn_AddTeacher_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {

                    int teacherId = Int32.Parse(txt_teacherID.Text);

                    bool teacherFound = context.Teachers.Any(teacher => teacher.TeacherId == teacherId);

                    if (!teacherFound)
                    {
                        //add teacher to database
                        var teacherToAdd = new Teachers();
                        teacherToAdd.TeacherId = Int32.Parse(txt_teacherID.Text);
                        teacherToAdd.FirstName = txt_firstName.Text;
                        teacherToAdd.LastName = txt_LastName.Text;
                        teacherToAdd.Email = txt_Email.Text;

                        context.Teachers.Add(teacherToAdd);
                        context.SaveChanges();

                        MessageBox.Show($"Teacher added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        listTeachers();
                    }
                    else
                    {
                        MessageBox.Show($"There is already a Teacher with this ID", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    clearTeacher();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void btn_EditTeacher_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {

                    int teacherId = Int32.Parse(txt_teacherID.Text);

                    bool teacherFound = context.Teachers.Any(teacher => teacher.TeacherId == teacherId);

                    if (teacherFound)
                    {
                        var teacherToEdit = from teacher in context.Teachers
                                            where teacher.TeacherId.ToString() == txt_teacherID.Text
                                            select teacher;

                        foreach (var teacher in teacherToEdit)
                        {
                            teacher.FirstName = txt_firstName.Text;
                            teacher.LastName = txt_LastName.Text;
                            teacher.Email = txt_Email.Text;
                        }

                        context.SaveChanges();
                        MessageBox.Show($"Teacher edited successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Teacher not found", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    clearTeacher();
                    listTeachers();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }

        private void btn_DeleteTeacher_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {

                    int teacherId = Int32.Parse(txt_teacherID.Text);

                    bool teacherFound = context.Teachers.Any(teacher => teacher.TeacherId == teacherId);

                    if (teacherFound)
                    {
                        var teacherToDelete = from teacher in context.Teachers
                                              where teacher.TeacherId.ToString() == txt_teacherID.Text
                                              select teacher;

                        foreach (var teacher in teacherToDelete)
                        {
                            context.Teachers.Remove(teacher);
                        }

                        context.SaveChanges();
                        MessageBox.Show($"Teacher removed successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show($"Teacher not found", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    clearTeacher();
                    listTeachers();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btn_SearchTeacher_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {

                    int teacherId = Int32.Parse(txt_teacherID_Search.Text);

                    bool teacherExists = context.Teachers.Any(teacher => teacher.TeacherId == teacherId);

                    if (teacherExists)
                    {
                        var teacherToSearch = from teacher in context.Teachers
                                              where teacher.TeacherId.ToString() == txt_teacherID_Search.Text
                                              select teacher;

                        foreach (var teacher in teacherToSearch)
                        {
                            lbl_teacherID.Text = teacher.TeacherId.ToString();
                            lbl_teacherName.Text = teacher.FirstName + " " + teacher.LastName;
                            lbl_teacherEmail.Text = teacher.Email;

                            txt_teacherID_Registration.Text = teacher.TeacherId.ToString();
                            lbl_teacherName_Registration.Text = teacher.FirstName + " " + teacher.LastName;

                        }

                        var searchCoursesByTeacher = from registration in context.Registration
                                                     join courses in context.Courses
                                                     on registration.CourseNumber equals courses.CourseNumber
                                                     where registration.TeacherID.ToString() == txt_teacherID_Search.Text
                                                     select registration;

                        dataGridView_Teacher_Courses.ClearSelection();

                        DataTable dt = new DataTable();
                        dt.Columns.Add("Teacher ID");
                        dt.Columns.Add("Course Number");


                        foreach (var registration in searchCoursesByTeacher)
                        {
                            dt.Rows.Add(registration.TeacherID, registration.CourseNumber);
                        }

                        dataGridView_Teacher_Courses.DataSource = dt;

                    }
                    else
                    {


                        MessageBox.Show($"Teacher not found", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        lbl_teacherID.Text = "Teacher ID";
                        lbl_teacherName.Text = "Teacher Name";
                        lbl_teacherEmail.Text = "Teacher Email";
                        txt_teacherID_Search.Text = "";

                        txt_teacherID_Registration.Text = "";
                        lbl_teacherName_Registration.Text = "";

                        dataGridView_Teacher_Courses.DataSource = null;

                    }

                }


            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        //---------------------------------------- Courses ------------------------------------

        private void listCourses()
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {
                    var selectCourses = from courses in context.Courses
                                        select courses;
                    dataGridView_Courses.ClearSelection();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Course Number");
                    dt.Columns.Add("Title");
                    dt.Columns.Add("Duration");


                    foreach (var course in selectCourses)
                    {
                        dt.Rows.Add(course.CourseNumber, course.CourseTitle, course.Duration);
                    }

                    dataGridView_Courses.DataSource = dt;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void clearCourse()
        {
            txt_courseNumber.Text = "";
            txt_courseTitle.Text = "";
            txt_courseDuration.Text = "";
        }

        private void btn_AddCourse_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {

                    string courseNumber = txt_courseNumber.Text;

                    bool courseExists = context.Courses.Any(course => course.CourseNumber == courseNumber);

                    if (!courseExists)
                    {
                        //add course to database
                        var courseToAdd = new Courses();
                        courseToAdd.CourseNumber = txt_courseNumber.Text;
                        courseToAdd.CourseTitle = txt_courseTitle.Text;
                        courseToAdd.Duration = Int32.Parse(txt_courseDuration.Text);

                        context.Courses.Add(courseToAdd);
                        context.SaveChanges();

                        MessageBox.Show($"Course added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"There is already a Course with this ID", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    clearCourse();
                    listCourses();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btn_EditCourse_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {
                    string courseNumber = txt_courseNumber.Text;

                    bool courseExists = context.Courses.Any(course => course.CourseNumber == courseNumber);

                    if (courseExists)
                    {
                        var courseToEdit = from course in context.Courses
                                           where course.CourseNumber == txt_courseNumber.Text
                                           select course;

                        foreach (var course in courseToEdit)
                        {
                            course.CourseTitle = txt_courseTitle.Text;
                            course.Duration = Int32.Parse(txt_courseDuration.Text);
                        }

                        context.SaveChanges();

                        MessageBox.Show($"Course edited successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show($"Course not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    clearCourse();
                    listCourses();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btn_DeleteCourse_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {

                    string courseNumber = txt_courseNumber.Text;

                    bool courseExists = context.Courses.Any(course => course.CourseNumber == courseNumber);

                    if (courseExists)
                    {
                        var courseToDelete = from course in context.Courses
                                             where course.CourseNumber == txt_courseNumber.Text
                                             select course;

                        foreach (var course in courseToDelete)
                        {
                            context.Courses.Remove(course);
                        }

                        context.SaveChanges();
                        MessageBox.Show($"Course removed successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show($"Course not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    clearCourse();
                    listCourses();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btn_SearchCourse_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {

                    string courseNumber = txt_courseNumber_Search.Text;

                    bool courseExists = context.Courses.Any(course => course.CourseNumber == courseNumber);

                    if (courseExists)
                    {
                        var courseToSearch = from course in context.Courses
                                             where course.CourseNumber == courseNumber
                                             select course;

                        foreach (var course in courseToSearch)
                        {
                            lbl_courseNumber.Text = course.CourseNumber;
                            lbl_courseTitle.Text = course.CourseTitle;
                            lbl_courseDuration.Text = course.Duration + " hours ";

                            txt_courseNumber_Registration.Text = courseNumber;
                            lbl_courseTitle_Registration.Text = course.CourseTitle;
                        }

                        var searchTeachersByCourse = from registration in context.Registration
                                                     join teachers in context.Teachers
                                                     on registration.TeacherID equals teachers.TeacherId
                                                     where registration.CourseNumber == txt_courseNumber_Search.Text
                                                     select registration;

                        dataGridView_Course_Teachers.ClearSelection();

                        DataTable dt = new DataTable();
                        dt.Columns.Add("Teacher ID");
                        dt.Columns.Add("Course Number");



                        foreach (var registration in searchTeachersByCourse)
                        {
                            dt.Rows.Add(registration.TeacherID, registration.CourseNumber);
                        }

                        dataGridView_Course_Teachers.DataSource = dt;

                    }
                    else
                    {
                        MessageBox.Show($"Course not found", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        lbl_courseNumber.Text = "Course Number";
                        lbl_courseTitle.Text = "Course Title";
                        lbl_courseDuration.Text = "Course Duration";
                        txt_courseNumber_Search.Text = "";

                        txt_courseNumber_Registration.Text = "";
                        lbl_courseTitle_Registration.Text = "";

                        dataGridView_Course_Teachers.DataSource = null;
                    }
                }

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        //---------------------------------------- Assignemnts --------------------------------------

        private void listRegistrations()
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {
                    var selectRegistrations = from registration in context.Registration
                                              select registration;

                    dataGridView_Registration.ClearSelection();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Registration ID");
                    dt.Columns.Add("Teacher ID");
                    dt.Columns.Add("Course Number");



                    foreach (var registration in selectRegistrations)
                    {
                        dt.Rows.Add(registration.RegistrationID, registration.TeacherID, registration.CourseNumber);
                    }

                    dataGridView_Registration.DataSource = dt;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void clearRegistration()
        {
            txt_registrationID.Text = "";
            txt_teacherID_Registration.Text = "";
            txt_courseNumber_Registration.Text = "";
           
        }
        private void txt_teacherID_Registration_TextChanged(object sender, EventArgs e)
        {
            lbl_teacherName_Registration.Text = "";
        }

        private void txt_courseNumber_Registration_TextChanged(object sender, EventArgs e)
        {
            lbl_courseTitle_Registration.Text = "";
        }

        private void btn_AddRegistration_Click(object sender, EventArgs e)
        {
            try
            {

                using (var context = new TeacherCourseDBEntities())
                {

                    int registrationID = Int32.Parse(txt_registrationID.Text);
                    int teacherId = Int32.Parse(txt_teacherID_Registration.Text);
                    string courseNumber = txt_courseNumber_Registration.Text;

                    bool registrationIdExists = context.Registration.Any(registration => registration.RegistrationID == registrationID);
                    bool teacherCourseAssigned = context.Registration.Any(registration => registration.TeacherID == teacherId && registration.CourseNumber == courseNumber);
                    int numberOfRegistrations = context.Registration.Count(registration => registration.TeacherID == teacherId);
                    bool teacherExists = context.Teachers.Any(teacher => teacher.TeacherId == teacherId);
                    bool courseExists = context.Courses.Any(course => course.CourseNumber == courseNumber);

                    if (!registrationIdExists)
                    {
                        if (!teacherCourseAssigned)
                        {
                            if (numberOfRegistrations < 4)
                            {
                                if (teacherExists)
                                {
                                    if (courseExists)
                                    {
                                        //add registration to database
                                        var registrationToAdd = new Registration();
                                        registrationToAdd.RegistrationID = registrationID;
                                        registrationToAdd.TeacherID = teacherId;
                                        registrationToAdd.CourseNumber = courseNumber;


                                        context.Registration.Add(registrationToAdd);
                                        context.SaveChanges();

                                        listRegistrations();
                                        MessageBox.Show($"Course and teacher assigned successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    }
                                    else
                                    {
                                        MessageBox.Show($"Course not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show($"Teacher not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Teacher can't have more than 4 courses assigned.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Course and Teacher already assigned", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Registration ID already in use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    clearRegistration();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btn_EditRegistration_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {

                    int registrationID = Int32.Parse(txt_registrationID.Text);
                    int teacherId = Int32.Parse(txt_teacherID_Registration.Text);
                    string courseNumber = txt_courseNumber_Registration.Text;

                    bool registrationIdExists = context.Registration.Any(registration => registration.RegistrationID == registrationID);
                    bool teacherCourseAssigned = context.Registration.Any(registration => registration.TeacherID == teacherId && registration.CourseNumber == courseNumber);
                    int numberOfRegistrations = context.Registration.Count(registration => registration.TeacherID == teacherId);
                    bool teacherExists = context.Teachers.Any(teacher => teacher.TeacherId == teacherId);
                    bool courseExists = context.Courses.Any(course => course.CourseNumber == courseNumber);

                    if (registrationIdExists)
                    {
                        if (!teacherCourseAssigned)
                        {
                            if (teacherExists)
                            {
                                if (courseExists)
                                {

                                    var registrationToEdit = from registration in context.Registration
                                                             where registration.RegistrationID == registrationID
                                                             select registration;


                                    foreach (var registration in registrationToEdit)
                                    {
                                        registration.TeacherID = teacherId;
                                        registration.CourseNumber = courseNumber;

                                    }

                                    context.SaveChanges();

                                    listRegistrations();
                                    MessageBox.Show($"Registration editted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                }
                                else
                                {
                                    MessageBox.Show($"Course not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Teacher not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Course and Teacher already assigned", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Registration ID not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    clearRegistration();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btn_DeleteRegistration_Click(object sender, EventArgs e)
        {
            try
            {

                using (var context = new TeacherCourseDBEntities())
                {

                    int registrationID = Int32.Parse(txt_registrationID.Text);

                    bool registrationIdExists = context.Registration.Any(registration => registration.RegistrationID == registrationID);


                    if (registrationIdExists)
                    {
                        var registrationToDelete = from registration in context.Registration
                                                   where registration.RegistrationID == registrationID
                                                   select registration;

                        foreach (var registration in registrationToDelete)
                        {
                            context.Registration.Remove(registration);
                        }

                        context.SaveChanges();
                        MessageBox.Show($"Registration removed successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        listRegistrations();
                    }
                    else
                    {
                        MessageBox.Show($"Registration ID not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    clearRegistration();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        // --------------------------- User ------------------------------

        static int userID = LoginForm.userID;

        private void setUserTab()
        {
            
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {
                    var selectUser = from user in context.Users
                                     where user.UserID == userID
                                     select user;

                    foreach (var user in selectUser)
                    {
                        lbl_JobTitle.Text = user.JobTitle;
                    }

                    var selecTeacher = from teacher in context.Teachers
                                     where teacher.TeacherId == userID
                                     select teacher;

                    foreach (var teacher in selecTeacher)
                    {
                        lbl_userID.Text = teacher.TeacherId.ToString();
                        lbl_userFirstName.Text = teacher.FirstName;
                        lbl_userLastName.Text = teacher.LastName;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_ChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {

                    var userToEdit = from user in context.Users
                                        where user.UserID == userID
                                        select user;

                    foreach (var user in userToEdit)
                    {
                        user.Password = txt_changePassword.Text;

                    }

                    context.SaveChanges();
                    MessageBox.Show($"Password changed successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_changePassword.Text = "";

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void btn_ExitApp_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
