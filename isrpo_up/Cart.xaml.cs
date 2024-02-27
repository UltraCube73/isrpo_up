using Microsoft.EntityFrameworkCore;
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
using System.IO;
using Microsoft.Identity.Client.NativeInterop;

namespace isrpo_up
{
    /// <summary>
    /// Логика взаимодействия для Cart.xaml
    /// </summary>
    public partial class Cart : Window
    {
        public User user = null;
        public Cart(User u)
        {
            this.user = u;
            InitializeComponent();
            Render();
        }
        private void Render()
        {
            sp_menu.Children.Clear();
            int sumCounter = 0;
            var context = new AppDbContext();
            List<CartItem> cartItems = context.CartItems.Include(x => x.Product).Where(x => user.CartItems.Contains(x)).ToList();
            foreach (CartItem cartItem in cartItems)
            {
                Product product = cartItem.Product;

                sumCounter += product.Cost * cartItem.Amount;

                Grid grid = new Grid();

                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(5) });

                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(10) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(10) });

                sp_menu.Children.Add(grid);

                Image image = new Image();
                try
                {
                    image.Source = new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory, "Images", product.Path)));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

                TextBlock nameTextBlock = new TextBlock();
                nameTextBlock.Text = product.Name;
                nameTextBlock.HorizontalAlignment = HorizontalAlignment.Center;

                Button incButton = new Button();
                incButton.Name = product.Code;
                incButton.Content = "+";
                incButton.Click += new RoutedEventHandler(IncrementClickHandler);

                TextBlock costTextBlock = new TextBlock();
                costTextBlock.Text = cartItem.Amount.ToString();
                costTextBlock.HorizontalAlignment = HorizontalAlignment.Center;

                Button decButton = new Button();
                decButton.Name = product.Code;
                decButton.Content = "-";
                decButton.Click += new RoutedEventHandler(DecrementClickHandler);

                grid.Children.Add(image);
                grid.Children.Add(nameTextBlock);
                grid.Children.Add(incButton);
                grid.Children.Add(costTextBlock);
                grid.Children.Add(decButton);

                image.SetValue(Grid.ColumnProperty, 0);
                image.SetValue(Grid.RowProperty, 0);
                image.SetValue(Grid.RowSpanProperty, 3);

                nameTextBlock.SetValue(Grid.ColumnProperty, 1);
                nameTextBlock.SetValue(Grid.RowProperty, 1);

                incButton.SetValue(Grid.ColumnProperty, 2);
                incButton.SetValue(Grid.RowProperty, 1);

                costTextBlock.SetValue(Grid.ColumnProperty, 3);
                costTextBlock.SetValue(Grid.RowProperty, 1);

                decButton.SetValue(Grid.ColumnProperty, 4);
                decButton.SetValue(Grid.RowProperty, 1);
            }

            tbl_total.Text = sumCounter.ToString();
        }

        private void IncrementClickHandler (object sender, EventArgs e)
        {
            string code = ((FrameworkElement)sender).Name;
            var context = new AppDbContext();
            user = context.Users.Include(x => x.CartItems).Where(x => x.Id == user.Id).FirstOrDefault();
            CartItem cartItem = context.CartItems.Include(x => x.Product).Where(x => user.CartItems.Contains(x) && x.Product.Code == code).FirstOrDefault();
            context.Users.Include(x => x.CartItems).Where(x => x.Id == user.Id).FirstOrDefault().CartItems.Where(x => x.Id == cartItem.Id).FirstOrDefault().Amount++;
            context.SaveChanges();
            Render();
        }

        private void DecrementClickHandler(object sender, EventArgs e)
        {
            string code = ((FrameworkElement)sender).Name;
            var context = new AppDbContext();
            user = context.Users.Include(x => x.CartItems).Where(x => x.Id == user.Id).FirstOrDefault();
            CartItem cartItem = context.CartItems.Include(x => x.Product).Where(x => user.CartItems.Contains(x) && x.Product.Code == code).FirstOrDefault();
            context.Users.Include(x => x.CartItems).Where(x => x.Id == user.Id).FirstOrDefault().CartItems.Where(x => x.Id == cartItem.Id).FirstOrDefault().Amount--;
            if (context.Users.Include(x => x.CartItems).Where(x => x.Id == user.Id).FirstOrDefault().CartItems.Where(x => x.Id == cartItem.Id).FirstOrDefault().Amount <= 0)
            {
                context.CartItems.Remove(cartItem);
            }
            context.SaveChanges();
            Render();
        }

        private void b_catalog_Click(object sender, RoutedEventArgs e)
        {
            var context = new AppDbContext();
            Catalog catalog = new Catalog();
            catalog.user = context.Users.Include(x => x.CartItems).Where(x => x.Id == user.Id).FirstOrDefault();
            catalog.Show();
            this.Close();
        }

        private void b_logoff_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.tb_login.Text = user.Login;
            mainWindow.Show();
            this.Close();
        }

        private void b_buy_Click(object sender, RoutedEventArgs e)
        {
            PaymentForm paymentForm = new PaymentForm();
            paymentForm.ShowDialog();
            var context = new AppDbContext();
            user = context.Users.Include(x => x.CartItems).Where(x => x.Id == user.Id).FirstOrDefault();
            foreach (CartItem cartItem in context.CartItems.Where(x => user.CartItems.Contains(x)))
            {
                context.CartItems.Remove(cartItem);
            }
            context.SaveChanges();
            Render();
        }

        private void b_clear_Click(object sender, RoutedEventArgs e)
        {
            var context = new AppDbContext();
            user = context.Users.Include(x => x.CartItems).Where(x => x.Id == user.Id).FirstOrDefault();
            foreach (CartItem cartItem in context.CartItems.Where(x => user.CartItems.Contains(x)))
            {
                context.CartItems.Remove(cartItem);
            }
            context.SaveChanges();
            Render();
        }
    }
}
