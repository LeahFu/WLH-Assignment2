using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using System.Data.SqlClient;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController:ControllerBase
    {
        private readonly IConfiguration _configuration;//
        public ProductController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        //to read the data from the server product table
        [HttpGet]
        [Route("GetAllProduct")]
        public Response GetAllProduct() 
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductCon").ToString());
            Response response = new Response();
            Applications applications = new Applications();
            response = applications.GetAllProducts(con);
            return response;
        }

        [HttpGet]
        [Route("GetProductById/{id}")]
        public Response GetProductById(int id)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductCon").ToString());
            Response response = new Response();
            Applications apl = new Applications();
            response = apl.GetProductById(con, id);
            return response;
        }

        [HttpPost]
        [Route("AddProduct")]
        public Response AddProduct(Product product)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductCon").ToString());
            Response response = new Response();
            Applications apl = new Applications();
            response = apl.AddProduct(con,product);
            return response;
        }

        [HttpPut]
        [Route("UpdateProduct")]
        public Response UpdateProduct(Product product)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductCon").ToString());
            Response response = new Response();
            Applications apl = new Applications();
            response = apl.UpdateProduct(con,product);
            return response;
        }

        [HttpDelete]
        [Route("DeleteProductById/{id}")]
        public Response DeleteProductById(int id)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductCon").ToString());
            Response response = new Response();
            Applications apl = new Applications();
            response = apl.DeleteProductById(con, id);
            return response;
        }
    }
}
