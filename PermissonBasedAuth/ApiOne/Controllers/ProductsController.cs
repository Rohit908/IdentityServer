using ApiOne.Constants;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Authorize(Permissions.Products.View)]
        [HttpGet("Read")]
        public IActionResult Read()
        {
            return Ok("Reading Successful");
        }

        [Authorize(Permissions.Products.Create)]
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return Ok("Creating Successful");
        }

        [Authorize(Permissions.Products.Edit)]
        [HttpGet("Edit")]
        public IActionResult Edit()
        {
            return Ok("Editing Successful");
        }

        [Authorize(Permissions.Products.Delete)]
        [HttpGet("Delete")]
        public IActionResult Delete()
        {
            return Ok("Deleting Successful");
        }

    }
}
