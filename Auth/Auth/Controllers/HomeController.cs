using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auth.Controllers
{
    public class HomeController:Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signManager;
        private readonly IAuthorizationService _authorizationService;

        public HomeController(
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _signManager = signInManager;
            _authorizationService = authorizationService;
            //_emailService = emailService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if(user != null)
            {
                var r = await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.DateOfBirth, "11/11/2000"));

                var signInResult = await _signManager.PasswordSignInAsync(user, password, false, false);

                if(signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return BadRequest();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new IdentityUser()
            {
                UserName = username,
                Email = ""
            };

            var result = await _userManager.CreateAsync(user, password);

            if(result.Succeeded)
            {

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                //var link = Url.Action(nameof(VerifyEmail), "Home", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());
                //await _emailService.SendAsync("rdtiwari441@gmail.com", "Verify Email", $"<a href=\"{link}\"></a>", true);

                


                return RedirectToAction("VerifyEmail", new { userId = user.Id, code });

                //var signInResult = await _signManager.PasswordSignInAsync(user, password, false, false);

                //if (signInResult.Succeeded)
                //{
                //    return RedirectToAction("Index");
                //}
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return BadRequest();

            var result = await _userManager.ConfirmEmailAsync(user, code);
            
            if(result.Succeeded)
            {
                return View();
            }

            return View();
        }

        public IActionResult EmailVerification()
        {
            return View();
        }


        [Authorize(Policy = "Claim.DOB")]
        public string Read()
        {
            return "Success";
        }

        public async Task<IActionResult> DoStuff()
        {

            var builder = new AuthorizationPolicyBuilder("Scheme");
            var customPolicy = builder.RequireClaim("Hello").Build();

            var authResult = await _authorizationService.AuthorizeAsync(User, customPolicy);

            if(authResult.Succeeded)
            {

            }

            return View("Index");
        }


        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Authenticate()
        {

            var claim1 = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Bob")
            };

            var claim2 = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Max")
            };

            var claim1Identity = new ClaimsIdentity(claim1, "c1");
            var claim2Identity = new ClaimsIdentity(claim2, "c2");

            var userPrincipal = new ClaimsPrincipal(new[] { claim1Identity, claim2Identity });

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }

    }
}
