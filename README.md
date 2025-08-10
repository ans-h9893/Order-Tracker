# Order-Tracker
A WPF application with database connectivity using the disconnected model. Allows viewing customer orders, adding new orders, and deleting existing orders through an interactive UI with popup-based order creation.

# WPF Orders Management Application

## 📌 Overview
This is a **WPF (Windows Presentation Foundation)** desktop application with **database connectivity using the disconnected model**.  
It enables users to:
- Select a **customer** and view their **orders**.
- View **items** in the selected order and calculate the **total payment**.
- **Add** new orders through a popup window.
- **Delete** existing orders directly from the main page.

---

## 🚀 Features
### Main Page
- Dropdown to select **Customer Name**.
- List to select **Order**.
- Displays all **items** in the selected order.
- Shows **Total Payment** for that order.
- **Delete Order** button.
- **Add Order** button (opens a popup window).

### Add Order Popup
- Select **Customer Name** drop-down.
- Select **Product Name**(drop-down), Add **Price** and **Quantity**.
- **Add New Row**: Insert additional products into the order.
- **Submit Order**: Save the new order to the database.
- **Cancel**: Close the popup without saving.

---

## 🛠 Technologies Used
- **C#** with **WPF**
- **ADO.NET** (Disconnected Model)
- **DataTable / DataSet** for managing data in memory
- **SQL Server** database

---

## 📂 Project Structure
- **MainWindow.xaml**: Main UI for viewing and managing orders.
- **AddOrderWindow.xaml**: Popup window for adding new orders.
- **Database Layer**: Handles disconnected model operations.
- **Models**: Represents Customers, Orders, and Order Items.

---

## 📸 Screenshot Gallery

<p float="left">
  <img src="screenshots/main-window.png" />
  <img src="screenshots/addOrder-window.png" />
  <img src="screenshots/orderAdded-window.png" />
</p>

## ⚙️ How to Run
1. Clone this repository:
   bash
   git clone https://github.com/yourusername/your-repo-name.git
   
2. Open the project in Visual Studio.
3. Update the connection string in App.config to match your SQL Server database.
4. Build and run the application.  
