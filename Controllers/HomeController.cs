using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ps3g.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace ps3g.Controllers
{
    
    public class HomeController : Controller
    {
        private HttpContextAccessor _Accessor;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //objSession.SetSession("MenuId", "");
            _Accessor.HttpContext.Session.SetString("MenuId", "");
            if (_Accessor.HttpContext.Session.GetString("UserId") == null)                
            {
                clearcookie();
                return View("Login");
            }
            string returnurl = Convert.ToString(Request.Query["ReturnUrl"]);
            if (returnurl != null)
            {
                return Redirect(returnurl);
            }
            else
            {
                TempData.Keep();
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UserLogin(UserData usermodel)
        {
            try
            {
                DbModel objhome = new DbModel();
                bool result = objhome.ValidateLogin(usermodel);
                if (result == true)
                {
                    //Set cookie for authentication 
                    setcookie(usermodel);                    
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.message = "Invalid Credentials.";
                    return View("Login");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AllowAnonymous]
        public async void clearcookie()
        {
            if (CookieAuthenticationDefaults.AuthenticationScheme != null)
            {
                await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }

        public async void setcookie(UserData usermodel)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usermodel.UserName),                        
                        new Claim(ClaimTypes.SerialNumber, usermodel.Password),                        
                    };
            var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
        }


    }
}
