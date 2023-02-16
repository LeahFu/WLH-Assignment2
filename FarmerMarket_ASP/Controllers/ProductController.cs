using FarmerMarket_ASP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace FarmerMarket_ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _configuraion; //receive and request the connection state with sql server
        public ProductController(IConfiguration configuration)
        {
            _configuraion = configuration;
        }
        //To read the data from the Server student table
        [HttpGet]
        [Route("GetAllProduct")]
        public Response GetAllProduct()
        {
            SqlConnection con = new SqlConnection(_configuraion.GetConnectionString("productCon").ToString());
            Response response = new Response();
            Application api = new Application();
            response = api.GetAllProduct(con);
            return response;
        }

        [HttpGet]
        [Route("GetAllProductByID/{id}")]
        public Response GetAllProductByID(int id)
        {
            SqlConnection con = new SqlConnection(_configuraion.GetConnectionString("productCon").ToString());
            Response response = new Response();
            Application api = new Application();
            response = api.GetAllProductByID(con, id);
            return response;
        }

        //To Insert Data in my Product Table
        [HttpPost]
        [Route("AddProduct")]
        public Response AddProduct(Product product)
        {
            SqlConnection con = new SqlConnection(_configuraion.GetConnectionString("productCon").ToString());
            Response response = new Response();
            Application api = new Application();
            response = api.AddProduct(con, product);
            return response;
        }

        [HttpPut]
        [Route("UpdateProduct")]
        public Response UpdateProduct(Product product)
        {
            SqlConnection con = new SqlConnection(_configuraion.GetConnectionString("productCon").ToString());
            Response response = new Response();
            Application api = new Application();
            response = api.UpdateProduct(con, product);
            return response;
        }

        [HttpDelete]
        [Route("DeleteStudent/{id}")]
        public Response DeleteStudent(int id)
        {
            SqlConnection con = new SqlConnection(_configuraion.GetConnectionString("productCon").ToString());
            Response response = new Response();
            Application api = new Application();
            response = api.DeleteProduct(con, id);
            return response;
        }
    }
}
