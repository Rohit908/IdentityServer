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


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //retrive access token
            var serverClient = _httpClientFactory.CreateClient();

            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44373/");

            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = "client_id",
                    ClientSecret = "client_secret",
                    Scope = "ApiOne"
                });

            //retrive secret data
            var apiClient = _httpClientFactory.CreateClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("https://localhost:44371/api/Products/Read");

            var content = response.Content.ReadAsStringAsync();

            return Ok(new { access_token = tokenResponse.AccessToken, message = content });

        }


        //[Authorize(Permissions.Products.View)]
        [Authorize]
        [HttpGet("Read")]
        public IActionResult Read()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
            //return Ok("Reading Successful");
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
