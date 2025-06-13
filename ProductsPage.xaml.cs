using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestWPF.testdbDataSetTableAdapters;

namespace TestWPF
{
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        private ProductsDataTableAdapter _productsDataTableAdapter;
        private productsTableAdapter _productsTableAdapter;

        private Button _edit;
        private Button _delete;
        private Button _add;

        public ProductsPage(Button edit, Button delete, Button add)
        {
            InitializeComponent();

            _productsDataTableAdapter = new ProductsDataTableAdapter();
            _productsTableAdapter = new productsTableAdapter();

            UpdateItemSource();

            _edit = edit;
            _delete = delete;
            _add = add;

        }

        public void UpdateItemSource()
        {
            this.mainProductsList.ItemsSource = _productsDataTableAdapter.GetData();
        }

        private void mainProductsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainProductsList.SelectedItem is DataRowView row)
            {
                int id = Convert.ToInt32(row["ID"]);
                App.SelectedProductID = id;

                _edit.IsEnabled = true;
                _delete.IsEnabled = true;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateItemSource();

            _add.IsEnabled = true;
            _delete.IsEnabled = false;
            _edit.IsEnabled = false;
        }
    }
}
