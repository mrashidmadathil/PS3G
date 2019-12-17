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
using Microsoft.Extensions.Configuration;

namespace ps3g.Controllers
{
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]

    public class HomeController : Controller
    {



        private readonly SessionUtility objSession;

        public HomeController()
        {
            objSession = new SessionUtility();

        }

        public IActionResult Index()
        {
            //objSession.SetSession("MenuId", "");
            //_Accessor.HttpContext.Session.SetString("MenuId", "");
            if (objSession.GetSession("UserName") == null && objSession.GetSession("Password") == null)
            {
                clearcookie();                
                return View("Login");
            }
            else
            {
                ViewData["UserName"] = objSession.GetSession("UserName");
                ViewData["Password"] = objSession.GetSession("Password");
                return View("Index");
            }

        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();

        }

        [AllowAnonymous]
        public IActionResult LogOut()
        {
            clearcookie();
            return RedirectToAction("Index");

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
                    ViewBag.message = "username/password do not match.";
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

        [AllowAnonymous]
        [HttpPost]
        public Response Signup(UserData model)
        {
            DbModel objmodel = new DbModel();
            Response objres = new Response();
            objres = objmodel.Signup(model);
            return objres;
        }
    }
}
