using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet]
        public IActionResult Authorize(string response_type, string client_id, string redirect_uri, string scope, string state)
        {

            var query = new QueryBuilder();
            query.Add("redirectUri", redirect_uri);
            query.Add("state", state);
            return View(model:query.ToString());
        }

        [HttpPost]
        public IActionResult Authorize(   
            string redirectUri, 
            string state, 
            string username)
        {

            const string code = "ssssssssssssssss";

            var query = new QueryBuilder();
            query.Add("code", code);
            query.Add("state", state);

            return Redirect($"{redirectUri}{query.ToString()}");
        }

        public async Task<IActionResult> Token(
            string grant_type,
            string code,
            string redirect_uri,
            string client_id,
            string refresh_token)
        {

            var claim = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, "test_id")
            };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Secret)), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audiance,
                claim,
                notBefore: System.DateTime.Now,
                expires: grant_type=="refresh_token"?System.DateTime.Now.AddMinutes(5):System.DateTime.Now.AddMilliseconds(1),
                signingCredentials
                );

            var access_token = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new { access_token, token_type = "Bearer", raw_claim = "oauthTutorial", refresh_token = "RefreshToken011" };

            var responseJson = JsonConvert.SerializeObject(response);

            var responseBytes = Encoding.UTF8.GetBytes(responseJson);

            await Response.Body.WriteAsync(responseBytes);

            return Redirect(redirect_uri);
        }

    }
}
