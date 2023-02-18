using Newtonsoft.Json;
using ProductApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Nodes;
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

namespace FarmersShopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static HttpClient client = new HttpClient();
        public MainWindow()//constructor
        {
            client.BaseAddress = new Uri("https://localhost:7282/api/Product");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
            InitializeComponent();
            
            LoadGrid();
        }


        private async void LoadGrid()
        {
            await Task.Run(async () =>
            {
                HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7282/api/Product/GetAllProduct");
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
            this.addProduct();
        }

        private async void addProduct()
        {
            try
            {
                Product product = new Product();
                product.ProductId = int.Parse(ProductID.Text);
                product.ProductName = ProductName.Text;
                product.Amount = float.Parse(Amount.Text);
                product.Price = float.Parse(Price.Text);
                var response = await client.PostAsJsonAsync("https://localhost:7282/api/Product/AddProduct", product);
                MessageBox.Show("Insert successful!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Input error,Please input again");
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            this.searchProduct();
        }

        private async void searchProduct()
        {
            
            try
            {
                int pro_id = int.Parse(ProductID.Text);               
                HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7282/api/Product/GetAllProductById/" + pro_id);
                responseMessage.EnsureSuccessStatusCode();
                string response = await responseMessage.Content.ReadAsStringAsync();
                Response res = JsonConvert.DeserializeObject<Response>(response);
                Product product = res.product;                   
                //ProductID.Text = product.ProductId.ToString();
                ProductName.Text = product.ProductName;
                Amount.Text = product.Amount.ToString();
                Price.Text = product.Price.ToString();                   
            }
            catch (Exception)
            {
                MessageBox.Show("Input error,please input correct Product Id");
            }
        }


        private void Update_Click(object sender, RoutedEventArgs e)
        {
            this.updateProduct();
        }

        private async void updateProduct()
        {
            try
            {
                Product product=new Product();
                product.ProductId = int.Parse(ProductID.Text);
                product.ProductName = ProductName.Text;
                product.Amount = float.Parse(Amount.Text);
                product.Price = float.Parse(Price.Text);
                var response = await client.PutAsJsonAsync("https://localhost:7282/api/Product/UpdateProduct", product);
                MessageBox.Show("Update successful!");
            }
            catch(Exception)
            {
                MessageBox.Show("Input error,Please input again");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            this.deleteProduct();
        }

        private async void deleteProduct()
        {
            try
            {
                int pro_id = int.Parse(ProductID.Text);

                HttpResponseMessage responseMessage = await client.DeleteAsync("https://localhost:7282/api/Product/DeleteProductById/" + pro_id);
                responseMessage.EnsureSuccessStatusCode();
                string response = await responseMessage.Content.ReadAsStringAsync();
                Response res = JsonConvert.DeserializeObject<Response>(response);
                    
                MessageBox.Show("Deleted successful!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Input error,Please input again");
            }

        }

        //SelectionChanged="dataGrid_SelectionChanged"
        //private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (dataGrid.Items.Count > 0)
        //        {
        //            ProductID.Text = ((DataRowView)dataGrid.SelectedItem).Row["ProductID"].ToString();
        //            ProductName.Text = ((DataRowView)dataGrid.SelectedItem).Row["Productname"].ToString();
        //            Amount.Text = ((DataRowView)dataGrid.SelectedItem).Row["Amount"].ToString();
        //            Price.Text = ((DataRowView)dataGrid.SelectedItem).Row["Price"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //}

        public void clearData()
        {
            ProductID.Clear();
            ProductName.Clear();
            Amount.Clear();
            Price.Clear();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            clearData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Sales swin = new Sales();
            swin.Show();
            this.Close();
        }

    }

}
