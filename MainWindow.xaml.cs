using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestWPF.testdbDataSetTableAdapters;

namespace TestWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProductsPage _productsPage;


        private productsTableAdapter _productsTableAdapter;


        public MainWindow()
        {
            InitializeComponent();

            _productsPage = new ProductsPage(this.Delete, this.Edit, this.Add);


            _productsTableAdapter = new productsTableAdapter();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(_productsPage);

            this.Edit.IsEnabled = false;
            this.Delete.IsEnabled = false;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new AddEditPage(App.SelectedProductID));

            this.Add.IsEnabled = false;
            this.Edit.IsEnabled = false;
            this.Delete.IsEnabled = false;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить товар с ID {App.SelectedProductID}?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

            if (result == MessageBoxResult.Yes)
            {
                _productsTableAdapter.DeleteByProductID(App.SelectedProductID);

                _productsPage.UpdateItemSource();
                MessageBox.Show("Товар успешно удалён.");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

            this.Add.IsEnabled = false;
            this.Edit.IsEnabled = false;
            this.Delete.IsEnabled = false;

            mainFrame.Navigate( new AddEditPage());
        }

       



    }
}
