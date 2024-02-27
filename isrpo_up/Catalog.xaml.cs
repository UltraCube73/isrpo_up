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
using Microsoft.EntityFrameworkCore;

namespace isrpo_up
{
    /// <summary>
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : Window
    {
        public User user = null;
        public Catalog()
        {
            InitializeComponent();
            Render();
        }

        private void Render()
        {
            sp_menu.Children.Clear();
            var context = new AppDbContext();
            List<Product> products = context.Products.ToList();
            int column = 1;
            Grid grid = new Grid();
            foreach (Product product in products)
            {
                if (column == 5) column = 1;
                if (column == 1)
                {
                    grid = new Grid();

                    grid.ColumnDefinitions.Add(new ColumnDefinition() { });
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(150) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(150) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(150) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(150) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { });

                    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(10) });
                    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(150) });
                    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });

                    sp_menu.Children.Add(grid);
                }

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
                nameTextBlock.FontSize = 12;
                nameTextBlock.TextWrapping = TextWrapping.Wrap;

                TextBlock costTextBlock = new TextBlock();
                costTextBlock.Text = product.Cost.ToString();

                Button button = new Button();
                button.Name = product.Code;
                button.Content = "Добавить";
                button.Click += new RoutedEventHandler(ItemClickHandler);

                grid.Children.Add(image);
                grid.Children.Add(nameTextBlock);
                grid.Children.Add(costTextBlock);
                grid.Children.Add(button);

                image.SetValue(Grid.RowProperty, 1);
                nameTextBlock.SetValue(Grid.RowProperty, 2);
                costTextBlock.SetValue(Grid.RowProperty, 3);
                button.SetValue(Grid.RowProperty, 4);

                nameTextBlock.SetValue(Grid.ColumnProperty, column);
                costTextBlock.SetValue(Grid.ColumnProperty, column);
                image.SetValue(Grid.ColumnProperty, column);
                button.SetValue(Grid.ColumnProperty, column);

                column++;
            }
        }

        private void ItemClickHandler(object sender, RoutedEventArgs e)
        {
            string code = ((FrameworkElement)sender).Name;
            var context = new AppDbContext();
            user = context.Users.Include(x => x.CartItems).Where(x => x.Id == user.Id).FirstOrDefault();
            CartItem cartItem = context.CartItems.Include(x => x.Product).Where(x => user.CartItems.Contains(x) && x.Product.Code == code).FirstOrDefault();
            if (cartItem != null)
            {
                context.Users.Include(x => x.CartItems).Where(x => x.Id == user.Id).FirstOrDefault().CartItems.Where(x => x.Id == cartItem.Id).FirstOrDefault().Amount++;
            }
            else
            {
                CartItem newCartItem = new CartItem();
                newCartItem.Product = context.Products.Where(x => x.Code == code).FirstOrDefault();
                newCartItem.Amount = 1;
                context.Users.Include(x => x.CartItems).Where(x => x.Id == user.Id).FirstOrDefault().CartItems.Add(newCartItem);
            }
            context.SaveChanges();
        }

        private void b_cart_Click(object sender, RoutedEventArgs e)
        {
            var context = new AppDbContext();
            Cart cart = new Cart(context.Users.Include(x => x.CartItems).Where(x => x.Id == user.Id).FirstOrDefault());
            cart.Show();
            this.Close();
        }

        private void b_logoff_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.tb_login.Text = user.Login;
            mainWindow.Show();
            this.Close();
        }
    }
}
