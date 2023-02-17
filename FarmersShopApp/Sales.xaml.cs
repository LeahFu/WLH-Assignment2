using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Diagnostics;
using System.Collections;
using Newtonsoft.Json;
using ProductApi.Models;
using System.Net.Http;

namespace FarmersShopApp
{
    public partial class Sales : Window
    {
        SqlConnection con;
        public static HttpClient client=new HttpClient();
        public ArrayList productNames { get; set; }
        public string updateSqls { get; set; }
        public double totalAll { get; set; }

        public Sales()
        {
            InitializeComponent();

            productNames = GetProductDataAsync().Result;
            DataContext = this;
        }

        private async Task<ArrayList> GetProductDataAsync()
        {           
            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7282/api/Product/GetAllProduct");
            responseMessage.EnsureSuccessStatusCode();
            string response = await responseMessage.Content.ReadAsStringAsync();
            Response res = JsonConvert.DeserializeObject<Response>(response);

            List<Product> products = res.listProduct;

            ArrayList productNameList = new ArrayList();

            foreach (Product product in products)
            {
                productNameList.Add(product.ProductName);
            }
            return productNameList;

            //string[] productNames = new string[productNameList.Count];

            //for (int i = 0; i < productNameList.Count; i++)
            //{
            //    productNames[i] = (string)productNameList[i];

            //}

            ////con.Close();
            //return productNames;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (this.productComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("please select product what you want.");
                return;
            }
            if (this.AmountTextBox.Text.Length == 0)
            {
                MessageBox.Show("please input the amount what you need.");
                return;
            }

            string productName = (string)productComboBox.SelectedItem;
            double amount = Convert.ToDouble(this.AmountTextBox.Text);
            double price = 0;
            double total = 0;

            con.Open();
            string querySql = "select ProductName, Amount, Price from Product where ProductName=@ProductName";
            SqlCommand cmd = new SqlCommand(querySql, con);
            cmd.Parameters.AddWithValue("@ProductName", productName);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();

            double amountInventory = 0;
            while (sqlDataReader.Read())
            {
                amountInventory = Convert.ToDouble(sqlDataReader.GetValue(1).ToString());
                price = Convert.ToDouble(sqlDataReader.GetValue(2).ToString());
            }

            if (amount > amountInventory)
            {
                MessageBox.Show("this product is out of stock, please reduce the amount.");
                con.Close();
                return;
            }

            updateSqls = updateSqls + "update Product set Amount = Amount - " + this.AmountTextBox.Text + "where ProductName = '" + this.productComboBox.SelectedItem + "'; ";
            total = Math.Round(amount * price, 2);
            if (this.BillTextBox.Text.Length == 0)
            {
                this.BillTextBox.Text = productName + "   " + amount.ToString() + "     $" + total.ToString() + "\n";
                totalAll = Math.Round(totalAll + total, 2);
                this.BillTextBox.Text = this.BillTextBox.Text + "Total price: " + totalAll;
            }
            else
            {
                string tempStr = this.BillTextBox.Text;

                tempStr = tempStr.Substring(0, tempStr.Length - (tempStr.Length - tempStr.LastIndexOf("\n")));
                tempStr = tempStr + "\n";
                tempStr = tempStr + productName + "    " + amount.ToString() + "    $" + total.ToString() + "\n";
                totalAll += total;

                this.BillTextBox.Text = tempStr + "total price: " + "$" + totalAll;

            }
            this.productComboBox.SelectedIndex = -1;
            this.AmountTextBox.Text = "";

            con.Close();
        }

        private void CheckOut_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(updateSqls, con);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();

            updateSqls = "";
            totalAll = 0;

            MessageBox.Show(this.BillTextBox.Text);
            this.BillTextBox.Clear();
            con.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWin = new MainWindow();
            this.Close();
            MainWin.Show();
        }
    }
}
