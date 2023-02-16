using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace FarmerMarket_ASP.Models
{
    public class Application
    {
        public Response GetAllProduct(SqlConnection con)
        {
            Response response = new Response();
            string Query = "Select * from ProductsInventory";
            SqlDataAdapter da = new SqlDataAdapter(Query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Product> listProdt = new List<Product>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Product product = new Product();//Creating the class Object
                    product.ProductID = (int)dt.Rows[i]["ProductID"];
                    product.ProductName = (string)dt.Rows[i]["ProductName"];
                    product.Amount = double.Parse(dt.Rows[i]["Amount"].ToString());
                    product.Price = double.Parse(dt.Rows[i]["Price"].ToString());

                    listProdt.Add(product);
                }
            }
            if (listProdt.Count > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Product Retrieved Perfectly";
                response.listProduct = listProdt;
            }
            else //Only works if your data table is Empty or your connention fails
            {
                response.StatusCode = 100;
                response.StatusMessage = "No product Found";
                response.listProduct = null;
            }
            return response;
        }

        public Response GetAllProductByID(SqlConnection con, int id)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("Select * from ProductsInventory Where ProductID = '" + id + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                Product product = new Product();
                product.ProductID = (int)dt.Rows[0]["ProductId"];
                product.ProductName = (string)dt.Rows[0]["ProductName"];
                product.Amount = double.Parse(dt.Rows[0]["Amount"].ToString());
                product.Price = double.Parse(dt.Rows[0]["Price"].ToString());
                response.StatusCode = 200;
                response.StatusMessage = "Data Found";
                response.product = product;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Found";
                response.listProduct = null;
            }
            return response;
        }

        public Response AddProduct(SqlConnection con, Product product)
        {
            Response response = new Response();

            SqlCommand cmd = new SqlCommand
            ("Insert into ProductsInventory(ProductID, ProductName, Amount, Price) Values('" + product.ProductID + "','" + product.ProductName + "', '" + product.Amount + "', '" + product.Price + "') ", con);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Data Inserted Properly";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Inserted";
            }
            return response;
        }

        public Response UpdateProduct(SqlConnection con, Product product)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand
            ("Update ProductsInventory set ProductName='" + product.ProductName + "', Amount='" + product.Amount + "', Price='" + product.Price + "' Where ProductID='" + product.ProductID + "'", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Product Added";
            }

            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Inserted";
            }
            return response;
        }

        public Response DeleteProduct(SqlConnection con, int id)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("Delete from ProductsInventory Where ProductID='" + id + "'", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Product Deleted";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Product Deleted";
            }
            return response;
        }
    }
}
