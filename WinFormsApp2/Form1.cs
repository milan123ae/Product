
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient;
        public Form1()
        {
            InitializeComponent();
            _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var product = new ProductDto
            {
                ProductName = textBox1.Text,
                Description = textBox2.Text,
                Price = decimal.Parse(textBox3.Text),
                StockQuantity = int.Parse(textBox4.Text)
            };

            var json = JsonSerializer.Serialize(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response =  _httpClient.PostAsync("products", content);
/*
            if (response.IsCompletedSuccessfully)
            {
                MessageBox.Show("Product added successfully!");
            }
            else
            {
                MessageBox.Show("Error adding product.");
            }
*/
        }

        private async void LoadProducts()
        {
            var response = await _httpClient.GetAsync("product");
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadAsStringAsync();
                // Bind products to UI, e.g., a DataGridView
            }
        }

        private async void UpdateProduct(ProductDto product)
        {
            var response = await _httpClient.PutAsJsonAsync($"product/{product.ProductId}", product);
            if (response.IsSuccessStatusCode)
            {
                LoadProducts();
            }
        }

        private async void DeleteProduct(int productId)
        {
            var response = await _httpClient.DeleteAsync($"product/{productId}");
            if (response.IsSuccessStatusCode)
            {
                LoadProducts();
            }
        }
    }
}
