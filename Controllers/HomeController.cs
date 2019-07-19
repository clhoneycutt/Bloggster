using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bloggster.Models;


namespace Bloggster.Controllers
{
    public class HomeController : Controller
    {
        private int? UserSession
        {
            get { return HttpContext.Session.GetInt32("UserId"); }
            set { HttpContext.Session.SetInt32("UserId", (int)value); }
        }
        private MyContext _dbContext;
        public HomeController(MyContext context)
        {
            _dbContext = context;
        }

        
        [HttpGet("")]
        public IActionResult Index()
        {

            // TODO: 

            ViewBag.UserSession = UserSession;
            return View();
        }


        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                if(_dbContext.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);

                _dbContext.Add(user);
                _dbContext.SaveChanges();

                User LastUserAdded = _dbContext.Users.LastOrDefault<User>();
                UserSession = LastUserAdded.UserID;
                HttpContext.Session.SetString("FirstName", user.FirstName);
                HttpContext.Session.SetString("RegOrLog","Registration");
                
                return RedirectToAction("Index", "Blog");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LogUser loginAttempt)
        {
            if(ModelState.IsValid)
            {
                var userInDB = _dbContext.Users.FirstOrDefault(u => u.Email == loginAttempt.Email);

                if(userInDB == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password.");
                    return View("Index");
                }
                else
                {
                    var hasher = new PasswordHasher<LogUser>();
                    var result = hasher.VerifyHashedPassword(loginAttempt, userInDB.Password, loginAttempt.Password);

                    if(result == 0)
                    {
                        ModelState.AddModelError("Email", "Invalid Email/Password");
                        return View("Index");
                    }

                    UserSession = userInDB.UserID;
                    HttpContext.Session.SetString("FirstName", userInDB.FirstName);
                    HttpContext.Session.SetString("RegOrLog", "Login");
                    return RedirectToAction("Index", "Blog");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("success")]
        public IActionResult Success()
        {
            if(UserSession == null)
                return RedirectToAction("Index");

            ViewBag.UserSession = UserSession;
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.RegOrLog = HttpContext.Session.GetString("RegOrLog");
            return View();
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
