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
using System.Net.Http.Json;

namespace FarmersShopApp
{
    public partial class Sales : Window
    {
        HttpClient client = new HttpClient();
        public string[] productNames { get; set; }
        public float totalAll { get; set; }
        public List<Product> productList = new List<Product>();

        public Sales()
        {
            client.BaseAddress = new Uri("https://localhost:7282/api/Product");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
            InitializeComponent();

            GetProductList();
            DataContext = this;
        }

        private async void GetProductList()
        {
            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7282/api/Product/GetAllProduct");
            responseMessage.EnsureSuccessStatusCode();
            string response = await responseMessage.Content.ReadAsStringAsync();
            Response res = JsonConvert.DeserializeObject<Response>(response);

            productList = res.listProduct;

            productNames = new string[productList.Count];
            for (int i = 0; i < productList.Count; i++)
            {
                productNames[i] = productList[i].ProductName;
            }

        }

        private async void GetProductByName()
        {
            if (this.productComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("choose product.");
                return;
            }
            if (this.AmountTextBox.Text.Length == 0)
            {
                MessageBox.Show("input quantity");
                return;
            }

            string productName = (string)productComboBox.SelectedItem;
            float amount = float.Parse(this.AmountTextBox.Text);
            float totalAmount = amount;
            for (int i = 0; i < productList.Count; i++)
            {
                if (productList[i].ProductName.Equals(productName))
                {
                    totalAmount = totalAmount + productList[i].Amount;
                }
            }
            
            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7282/api/Product/GetProductByName/" + productName);
            responseMessage.EnsureSuccessStatusCode();
            string response = await responseMessage.Content.ReadAsStringAsync();
            Response res = JsonConvert.DeserializeObject<Response>(response);
            Product product = res.product;            

            float amountInventory = product.Amount;
            float price = product.Price;


            if (totalAmount > amountInventory)
            {
                MessageBox.Show("Stock is out, please re-enter amount.");
                return;
            }

            product.Amount = amount;

            productList.Add(product);

            float total = 0;
            total = amount * price;
            if (this.ProductTextBox.Text.Length == 0)
            {
                this.ProductTextBox.Text = productName + "   " + amount.ToString() + "      $" + total.ToString() + "\n";
                totalAll = totalAll + total;
                this.ProductTextBox.Text = this.ProductTextBox.Text + "Total price: " + totalAll;
            }
            else
            {
                string tempStr = this.ProductTextBox.Text;

                tempStr = tempStr.Substring(0, tempStr.Length - (tempStr.Length - tempStr.LastIndexOf("\n")));
                tempStr = tempStr + "\n";
                tempStr = tempStr + productName + "   " + amount.ToString() + "      $" + total.ToString() + "\n";
                totalAll = totalAll + total;

                this.ProductTextBox.Text = tempStr + "total price: " + totalAll;

            }
            this.productComboBox.SelectedIndex = -1;
            this.AmountTextBox.Text = "";
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            GetProductByName();
        }

        private async void ComfirmProduct()
        {
            HttpResponseMessage response = await client.PutAsJsonAsync<List<Product>>("CheckoutProduct/", productList);

            productList.Clear();
            totalAll = 0;

            MessageBox.Show(this.ProductTextBox.Text);
            this.ProductTextBox.Clear();
        }
        private void Comfirm_Click(object sender, RoutedEventArgs e)
        {

            ComfirmProduct();

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWin = new MainWindow();
            this.Close();
            MainWin.Show();
        }
    }
}
