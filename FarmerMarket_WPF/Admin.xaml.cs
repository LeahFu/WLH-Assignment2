using FarmerMarket_ASP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace FarmerMarket_WPF
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        HttpClient client = new HttpClient();
        public Admin()//Constructor
        {
            client.BaseAddress = new Uri("https://localhost:7213/api/Product/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
               new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
               );
            InitializeComponent();
        }

        private void Connection_Refresh_Click(object sender, RoutedEventArgs e)
        {
            this.displayProduct();
        }
        private async void displayProduct()
        {
            await Task.Run(async () =>
            {
                HttpResponseMessage responseMessage = await client.GetAsync("GetAllProduct");
                responseMessage.EnsureSuccessStatusCode();
                string response = await responseMessage.Content.ReadAsStringAsync();
                Response res = JsonConvert.DeserializeObject<Response>(response);
                List<Product> products = res.listProduct;
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    dataGrid.ItemsSource = products;
                }));
            });
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            this.InsertProduct();
        }
        private async void InsertProduct()
        {
            Product product = new Product();
            product.ProductID = int.Parse(ProductIDT.Text);
            product.ProductName = ProductNameT.Text;
            product.Amount = double.Parse(AmountT.Text);
            product.Price = double.Parse(PriceT.Text);

            HttpResponseMessage response = await client.PostAsJsonAsync<Product>("AddProduct", product);
            ServerStatus.Content = response.StatusCode.ToString();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateProduct();
        }
        private async void UpdateProduct()
        {
            Product product = new Product();
            product.ProductID = int.Parse(ProductIDT.Text);
            product.ProductName = ProductNameT.Text;
            product.Amount = double.Parse(AmountT.Text);
            product.Price = double.Parse(PriceT.Text);

            HttpResponseMessage response = await client.PostAsJsonAsync<Product>("UpdateProduct", product);
            ServerStatus.Content = response.StatusCode.ToString();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            this.SearchProduct();
        }
        private async void SearchProduct()
        {
            HttpResponseMessage responseMessage = await client.GetAsync("GetAllProductById/" + ProductIDT.Text);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            this.DeleteProduct();
        }
        private async void DeleteProduct()
        {
            HttpResponseMessage responseMessage = await client.DeleteAsync("DeleteProductById/" + int.Parse(ProductIDT.Text));
            responseMessage.EnsureSuccessStatusCode();
            string response = await responseMessage.Content.ReadAsStringAsync();
            Response res = JsonConvert.DeserializeObject<Response>(response);
           // ServerStatus.Content = response.StatusCode.ToString();
        }

        private void MainWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
