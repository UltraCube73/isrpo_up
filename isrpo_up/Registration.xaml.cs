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

namespace isrpo_up
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void b_register_Click(object sender, RoutedEventArgs e)
        {
            if(tb_login.Text == "" || tb_password.Text == "" || tb_mail.Text == "")
            {
                tbl_error.Text = "Некоторые данные не введены!";
                return;
            }
            var context = new AppDbContext();
            User user = new User();
            user.Login = tb_login.Text;
            user.Password = tb_password.Text;
            user.Email = tb_mail.Text;
            context.Users.Add(user);
            context.SaveChanges();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            mainWindow.tb_login.Text = tb_login.Text;
            mainWindow.tb_password.Text = tb_password.Text;
            this.Close();
        }

        private void b_return_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
