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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestWPF.testdbDataSetTableAdapters;

namespace TestWPF
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {

        private categoriesTableAdapter _categories;
        private manufacturesTableAdapter _manufacturers;
        private measurementsTableAdapter _measurements;
        private suppliersTableAdapter _suppliers;

        private testdbDataSet.productsRow _productRow;
        public AddEditPage(int selectedID)
        {
            InitializeComponent();

            LoadData(); // Загружаем списки в ComboBox

            var productAdapter = new productsTableAdapter();
            var productTable = productAdapter.GetDataByID(selectedID); // <--- заменили productId на selectedID

            if (productTable.Count > 0)
            {
                _productRow = productTable[0];
                FillFields(); // Заполняем поля
            }

        }

        private void FillFields()
        {
            ArticleTextBox.Text = _productRow.articul;
            NameTextBox.Text = _productRow.productName;
            PriceTextBox.Text = _productRow.price.ToString();
            DescriptionTextBox.Text = _productRow.Discription;
            QuantityTextBox.Text = _productRow.Quantity.ToString();
            DiscountTextBox.Text = _productRow.Discount.ToString();
            MaxDiscountTextBox.Text = _productRow.maxDiscount.ToString();

            // Выставляем значения для ComboBox
            CategoryComboBox.SelectedValue = _productRow.CategoryID;
            SupplierComboBox.SelectedValue = _productRow.SupplierID;
            ManufactureComboBox.SelectedValue = _productRow.ManufactureID;
            MeasurementComboBox.SelectedValue = _productRow.MeasurementID;
        }

        public AddEditPage()
        {
            InitializeComponent();

            LoadData();
        }

        private void LoadData()
        {
            _categories = new categoriesTableAdapter();
            _manufacturers = new manufacturesTableAdapter();
            _measurements = new measurementsTableAdapter();
            _suppliers = new suppliersTableAdapter();

            CategoryComboBox.ItemsSource = _categories.GetData();
            CategoryComboBox.DisplayMemberPath = "CategoryName";
            CategoryComboBox.SelectedValuePath = "CategoryID";

            SupplierComboBox.ItemsSource = _suppliers.GetData();
            SupplierComboBox.DisplayMemberPath = "SupplierName";
            SupplierComboBox.SelectedValuePath = "SupplierID";

            ManufactureComboBox.ItemsSource = _manufacturers.GetData();
            ManufactureComboBox.DisplayMemberPath = "ManufactureName";
            ManufactureComboBox.SelectedValuePath = "ManufactureID";

            MeasurementComboBox.ItemsSource = _measurements.GetData();
            MeasurementComboBox.DisplayMemberPath = "MeasurementName";
            MeasurementComboBox.SelectedValuePath = "MeasurementID";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Проверка: редактируем или создаём
            var adapter = new productsTableAdapter();

            try
            {
                if (string.IsNullOrWhiteSpace(ArticleTextBox.Text) ||
                    string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
                    string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
                    string.IsNullOrWhiteSpace(QuantityTextBox.Text) ||
                    string.IsNullOrWhiteSpace(DiscountTextBox.Text) ||
                    CategoryComboBox.SelectedItem == null ||
                    SupplierComboBox.SelectedItem == null ||
                    ManufactureComboBox.SelectedItem == null ||
                    MeasurementComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, заполните все поля перед сохранением.");
                    return;
                }

                // Если редактируем существующий товар
                if (_productRow != null)
                {
                    _productRow.articul = ArticleTextBox.Text;
                    _productRow.productName = NameTextBox.Text;
                    _productRow.price = float.Parse(PriceTextBox.Text);
                    _productRow.Discription = DescriptionTextBox.Text;
                    _productRow.Quantity = int.Parse(QuantityTextBox.Text);
                    _productRow.Discount = int.Parse(DiscountTextBox.Text);

                    _productRow.CategoryID = Convert.ToInt32(CategoryComboBox.SelectedValue);
                    _productRow.SupplierID = Convert.ToInt32(SupplierComboBox.SelectedValue);
                    _productRow.ManufactureID = Convert.ToInt32(ManufactureComboBox.SelectedValue);
                    _productRow.MeasurementID = Convert.ToInt32(MeasurementComboBox.SelectedValue);

                    adapter.Update(_productRow); // сохраняем изменения
                    MessageBox.Show("Товар успешно обновлён!");
                }
                else // если добавление
                {
                    adapter.InsertQuery(
                            ArticleTextBox.Text,
                            NameTextBox.Text,
                            Convert.ToInt32(MeasurementComboBox.SelectedValue), // ← правильно
                            float.Parse(PriceTextBox.Text),
                            int.Parse(MaxDiscountTextBox.Text),
                            Convert.ToInt32(ManufactureComboBox.SelectedValue),
                            Convert.ToInt32(SupplierComboBox.SelectedValue),
                            Convert.ToInt32(CategoryComboBox.SelectedValue),
                            int.Parse(DiscountTextBox.Text),
                            int.Parse(QuantityTextBox.Text),
                            DescriptionTextBox.Text
                        );
                    MessageBox.Show("Товар успешно добавлен!");
                }

                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }
    }
}
