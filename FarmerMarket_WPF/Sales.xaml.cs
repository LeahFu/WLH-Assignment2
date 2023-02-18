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
    /// Interaction logic for Sales.xaml
    /// </summary>
   
    public partial class Sales : Window
    {
        HttpClient client = new HttpClient();
        public Sales()
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

        private void MainWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
