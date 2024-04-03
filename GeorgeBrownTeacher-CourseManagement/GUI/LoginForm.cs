using GeorgeBrownTeacher_CourseManagement.Business_DataAccess;
using GeorgeBrownTeacher_CourseManagement.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeorgeBrownTeacher_CourseManagement
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public static int userID;

        private void btn_Login_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {


                    int userIDToLogin = Int32.Parse(txt_userID.Text);

                    bool userExists = context.Users.Any(user => user.UserID == userIDToLogin);
                    bool pwdExists = context.Users.Any(user => user.Password == txt_password.Text);


                    if (userExists && pwdExists)
                    {
                        var userToSearch = from user in context.Users
                                           where user.UserID.ToString() == txt_userID.Text && user.Password == txt_password.Text
                                           select user;

                        foreach (var user in userToSearch)
                        {
                            if (user.JobTitle == "Program Coordinator")
                            {
                                userID = user.UserID;
                                txt_userID.Text = "";
                                txt_password.Text = "";

                                CourseAssignmentForm obj = new CourseAssignmentForm();
                                obj.ShowDialog();


                            }
                            else
                            {
                                MessageBox.Show($"Access denied. User is not the program coordinator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else 
                    {
                        MessageBox.Show($"User and/or password not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

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
