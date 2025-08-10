using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace OrderTracker
{
    /// <summary>
    /// Interaction logic for AddNewOrderWindow.xaml
    /// </summary>
    public partial class AddNewOrderWindow : Window
    {
        DatabaseHelper data = new DatabaseHelper();
        ObservableCollection<OrderLineItem> items = new ObservableCollection<OrderLineItem>();
        public DataTable products;

        public AddNewOrderWindow()
        {
            InitializeComponent();
            customersComboBox.ItemsSource = data.GetCustomers().DefaultView;
            customersComboBox.DisplayMemberPath = "CompanyName";
            customersComboBox.SelectedValuePath = "CustomerID";
            ProductList.ItemsSource = data.GetAllProducts().DefaultView;

            products = data.GetAllProducts();
            itemsDataGrid.ItemsSource = items;
        }

        private void btnAddRow_Click(object sender, RoutedEventArgs e)
        {
            items.Add(new OrderLineItem());
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (customersComboBox.SelectedValue == null || orderDataDatePicker.SelectedDate == null || items.Count == 0)
            {
                MessageBox.Show("Fill all details before submitting.");
                return;
            }

            DataTable orderDetails = new DataTable();
            orderDetails.Columns.Add("ProductID", typeof(int));
            orderDetails.Columns.Add("UnitPrice", typeof(decimal));
            orderDetails.Columns.Add("Quantity", typeof(int));
            orderDetails.Columns.Add("Discount", typeof(float));

            foreach (var i in items)
            {
                orderDetails.Rows.Add(i.ProductID, i.UnitPrice, i.Quantity, i.Discount);
            }

            data.InsertNewOrder(customersComboBox.SelectedValue.ToString(), orderDataDatePicker.SelectedDate.Value, orderDetails);
            MessageBox.Show("New Order added!");
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
