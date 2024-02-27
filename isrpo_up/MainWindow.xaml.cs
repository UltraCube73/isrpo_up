using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace isrpo_up
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void b_login_Click(object sender, RoutedEventArgs e)
        {
            var context = new AppDbContext();
            User user = context.Users.Include(x => x.CartItems).Where(x => x.Login == tb_login.Text && x.Password == tb_password.Text).FirstOrDefault();
            if (user == null)
            {
                tbl_error.Text = "Неверные данные!";
                return;
            }
            Catalog catalog = new Catalog();
            catalog.user = user;
            catalog.Show();
            this.Close();
        }

        private void b_register_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Close();
        }
    }
}