using System;
using System.Data;
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
using OrderTracker;

namespace OrderTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseHelper data = new DatabaseHelper();

        public MainWindow()
        {
            InitializeComponent();
            LoadCustomers();
        }

        void LoadCustomers()
        {
            customerComboBox.ItemsSource = data.GetCustomers().DefaultView;
            customerComboBox.DisplayMemberPath = "CompanyName";
            customerComboBox.SelectedValuePath = "CustomerID";
        }

        private void customerComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (customerComboBox.SelectedValue != null)
            {
                ordersDataGrid.ItemsSource = data.GetOrdersByCustomer(customerComboBox.SelectedValue.ToString()).DefaultView;
                orderDetailsDataGrid.ItemsSource = null;
            }
        }

        private void ordersDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ordersDataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)ordersDataGrid.SelectedItem;
                int orderId = Convert.ToInt32(row["OrderID"]);
                DataTable details = data.GetOrderDetails(orderId);
                orderDetailsDataGrid.ItemsSource = details.DefaultView;

                decimal total = 0;
                foreach (DataRow r in details.Rows)
                {
                    if (!r.IsNull("UnitPrice") && !r.IsNull("Quantity") && !r.IsNull("Discount"))
                    {
                        decimal price = Convert.ToDecimal(r["UnitPrice"]);
                        int qty = Convert.ToInt32(r["Quantity"]);
                        float discount = Convert.ToSingle(r["Discount"]);
                        total += price * qty * (1 - (decimal)discount);
                    }
                }

                totalTextBlock.Text = $"Total Amount: ${total:F2}";
            }
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            AddNewOrderWindow window = new AddNewOrderWindow();
            window.ShowDialog();
            LoadCustomers();
        }

        private void btnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (ordersDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select an order to delete.");
                return;
            }

            DataRowView row = (DataRowView)ordersDataGrid.SelectedItem;
            int orderId = Convert.ToInt32(row["OrderID"]);
            data.DeleteOrder(orderId);
            customerComboBox_SelectionChanged(null, null);
            totalTextBlock.Text = null;
        }
    }
}