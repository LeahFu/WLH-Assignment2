using System.Data;
using System.Data.SqlClient;

namespace ProductApi.Models
{
    public class Applications
    {
        public Response GetAllProducts(SqlConnection con)
        {
            Response response= new Response();
            SqlDataAdapter dataAdapter= new SqlDataAdapter("Select * from Product",con);
            DataTable dataTable= new DataTable();
            List<Product> productsList= new List<Product>();
            dataAdapter.Fill(dataTable);
            if(dataTable.Rows.Count > 0 )
            {
                for(int i=0;i<dataTable.Rows.Count;i++)
                {
                    Product product= new Product();
                    product.ProductId = (int)dataTable.Rows[i]["ProductId"];
                    product.ProductName = (string)dataTable.Rows[i]["ProductName"];
                    product.Amount = float.Parse(dataTable.Rows[i]["Amount"].ToString());
                    product.Price = float.Parse(dataTable.Rows[i]["Price"].ToString());
                    productsList.Add(product);
                }
            }
            if (productsList.Count > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "OK";
                response.listProduct = productsList;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No product found";
                response.listProduct = null;
            }
            return response;
        }

        public Response AddProduct(SqlConnection con, Product product)
        {
            Response response = new Response();
            string Query = "Insert into Product(ProductId,ProductName,Amount,Price) values(@ProductId,@ProductName,@Amount,@Price)";
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(Query, con);
            DataTable dataTable = new DataTable();
            cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
            cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
            cmd.Parameters.AddWithValue("@Amount", product.Amount);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Insert Properly";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data Inserted";
            }

            return response;
        }

        public Response GetProductById(SqlConnection con, int id)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("Select * from Product Where ProductId = '" + id + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                Product product=new Product();
                product.ProductId = (int)dt.Rows[0]["ProductId"];
                product.ProductName = (string)dt.Rows[0]["ProductName"];
                product.Amount = float.Parse(dt.Rows[0]["Amount"].ToString());
                product.Price = float.Parse(dt.Rows[0]["Price"].ToString());

                response.StatusCode = 200;
                response.StatusMessage = "Data Found";
                response.product=product;
            }

            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Found";
                response.product = null;
            }
            return response;
        }

        public Response UpdateProduct(SqlConnection con, Product product)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("Update Product set ProductName='" + product.ProductName + "', Amount='" + product.Amount + "', Price='" + product.Price + "' Where ProductId='" + product.ProductId + "'", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Product updated";
            }

            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data updated";

            }
            return response;
        }

        public Response DeleteProductById(SqlConnection con, int Id)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("Delete from Product where ProductId='" + Id + "'", con);
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
                response.StatusMessage = "No Data Deleted";

            }
            return response;
        }

        public Response GetProductByName(SqlConnection con, string name)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("Select ProductId, ProductName, Amount, Price from Product Where ProductName = '" + name + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {

                Product product = new Product();
                product.ProductId = (int)dt.Rows[0]["ProductID"];
                product.ProductName = (string)dt.Rows[0]["ProductName"];
                product.Amount = float.Parse(dt.Rows[0]["Amount"].ToString());
                product.Price = float.Parse(dt.Rows[0]["Price"].ToString());

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

        public Response ComfirmProduct(SqlConnection con, List<Product> productList)
        {
            string updateSqls = new string("");
            for (int i = 0; i < productList.Count; i++)
            {
                updateSqls = updateSqls + "update product set Amount = Amount - " + productList[i].Amount + "where ProductName = '" + productList[i].ProductName + "'; ";
            }

            Response response = new Response();
            SqlCommand cmd = new SqlCommand(updateSqls, con);
            con.Open();
            int count = cmd.ExecuteNonQuery();
            con.Close();
            if (count > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Checkout Successful";
            }

            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Checkout Fail";

            }
            return response;
        }
    }
}
