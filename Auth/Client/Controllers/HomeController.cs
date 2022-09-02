using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Secret()
        {

            var token = await HttpContext.GetTokenAsync("access_token");
            var rtoken = await HttpContext.GetTokenAsync("refresh_token");

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            await RefreshToken();

            var serverResponse = await _client.GetAsync("https://localhost:44342/secret/index");

            return View();
        }

        [Authorize]
        public async Task<string> RefreshToken()
        {
            var rtoken = await HttpContext.GetTokenAsync("refresh_token");

            var refreshTokenClient = _httpClientFactory.CreateClient();

            var data = new Dictionary<string, string>()
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = rtoken
            };

            var request = new HttpRequestMessage(HttpMethod.Post,"https://localhost:44342/oauth/token") { 
            Content=new FormUrlEncodedContent(data)
            };

            var basic = "username:password";
            var encoded = Encoding.UTF8.GetBytes(basic);

            var b64 = Convert.ToBase64String(encoded);

            request.Headers.Add("Authorization", $"Basic {b64}");

            var resp = await refreshTokenClient.SendAsync(request);

            var respString = await resp.Content.ReadAsStringAsync();

            var respData = JsonConvert.DeserializeObject<Dictionary<string,string>>(respString);

            var newAccTok = respData.GetValueOrDefault("access_token");
            var newRefTok = respData.GetValueOrDefault("refresh_token");

            var authInfo = await HttpContext.AuthenticateAsync("ClientCookie");

            authInfo.Properties.UpdateTokenValue("access_token", newAccTok);
            authInfo.Properties.UpdateTokenValue("refresh_token", newRefTok);

            await HttpContext.SignInAsync("ClientCookie", authInfo.Principal, authInfo.Properties);



            return "";
        }

    }
}
