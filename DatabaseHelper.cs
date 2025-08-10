using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTracker
{
    public class DatabaseHelper
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["NorthwindConnection"].ConnectionString;

        public DataTable GetCustomers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT CustomerID, CompanyName FROM Customers", conn))
            {
                DataTable tblCustomers = new DataTable();
                adapter.Fill(tblCustomers);
                return tblCustomers;
            }
        }

        public DataTable GetOrdersByCustomer(string customerId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter($"SELECT OrderID, OrderDate, ShipName, Freight FROM Orders WHERE CustomerID = @CustomerID", conn))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@CustomerID", customerId);
                DataTable tblCustId = new DataTable();
                adapter.Fill(tblCustId);
                return tblCustId;
            }
        }

        public DataTable GetOrderDetails(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT P.ProductID, P.ProductName, OD.UnitPrice, OD.Quantity, OD.Discount FROM [Order Details] OD JOIN Products P ON P.ProductID = OD.ProductID WHERE OrderID = @OrderID", conn))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@OrderID", orderId);
                DataTable tblDetails = new DataTable();
                adapter.Fill(tblDetails);
                return tblDetails;
            }
        }

        public DataTable GetAllProducts()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT ProductID, ProductName, UnitPrice FROM Products", conn))
            {
                DataTable tblProducts = new DataTable();
                adapter.Fill(tblProducts);
                return tblProducts;
            }
        }

        public void InsertNewOrder(string customerId, DateTime orderDate, DataTable orderDetails)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    SqlCommand commandOrder = new SqlCommand("INSERT INTO Orders (CustomerID, OrderDate) OUTPUT INSERTED.OrderID VALUES (@CustomerID, @OrderDate)", conn, transaction);
                    commandOrder.Parameters.AddWithValue("@CustomerID", customerId);
                    commandOrder.Parameters.AddWithValue("@OrderDate", orderDate);
                    int orderId = (int)commandOrder.ExecuteScalar();

                    foreach (DataRow row in orderDetails.Rows)
                    {
                        SqlCommand commandDetail = new SqlCommand("INSERT INTO [Order Details] (OrderID, ProductID, UnitPrice, Quantity, Discount) VALUES (@OrderID, @ProductID, @UnitPrice, @Quantity, @Discount)", conn, transaction);
                        commandDetail.Parameters.AddWithValue("@OrderID", orderId);
                        commandDetail.Parameters.AddWithValue("@ProductID", row["ProductID"]);
                        commandDetail.Parameters.AddWithValue("@UnitPrice", row["UnitPrice"]);
                        commandDetail.Parameters.AddWithValue("@Quantity", row["Quantity"]);
                        commandDetail.Parameters.AddWithValue("@Discount", row["Discount"]);
                        commandDetail.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void DeleteOrder(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command1 = new SqlCommand("DELETE FROM [Order Details] WHERE OrderID = @OrderID", conn);
                command1.Parameters.AddWithValue("@OrderID", orderId);
                command1.ExecuteNonQuery();

                SqlCommand command2 = new SqlCommand("DELETE FROM Orders WHERE OrderID = @OrderID", conn);
                command2.Parameters.AddWithValue("@OrderID", orderId);
                command2.ExecuteNonQuery();
            }
        }
    }
}
