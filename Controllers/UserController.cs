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
    [Route("user")]
    public class UserController : Controller
    {
        private int? UserSession
        {
            get { return HttpContext.Session.GetInt32("UserId"); }
            set { HttpContext.Session.SetInt32("UserId", (int)value); }
        }
        private BlogContext _dbContext;
        public UserController(BlogContext context)
        {
            _dbContext = context;
        }

        // Registration Index GET Route
        
        [HttpGet("")]
        public IActionResult Index()
        {

            // TODO: 

            ViewBag.UserSession = UserSession;
            return View();
        }

        // Registration Index POST Route


        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            // If the POST request meets the validations
            if(ModelState.IsValid)
            {
                // Search the DB for conflicts
                if(_dbContext.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }

                // If no conflicts, generate hashed pw for storage in the database.
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);

                // Add and save new entry to the database.
                _dbContext.Add(user);
                _dbContext.SaveChanges();

                // Add new user to current user session.  AKA log them in.
                // This is a rudimentary login and should be added to.
                User LastUserAdded = _dbContext.Users.LastOrDefault<User>();
                UserSession = LastUserAdded.UserID;
                
                // Session variables for use on a confirmation page.
                // Either remove these lines or add confirmation pages.
                HttpContext.Session.SetString("FirstName", user.FirstName);
                HttpContext.Session.SetString("RegOrLog","Registration");
                

                return RedirectToAction("Index", "Blog");
            }
            // If POST request is not good return to registration page with errors.
            else
            {
                return View("Index");
            }
        }

        // Login POST route

        [HttpPost("login")]
        public IActionResult Login(LogUser loginAttempt)
        {
            // If POST request is good / meets expectations of the model.
            if(ModelState.IsValid)
            {
                
                var userInDB = _dbContext.Users.FirstOrDefault(u => u.Email == loginAttempt.Email);

                // If the user email address is not found.
                if(userInDB == null)
                {
                    // Return errors and go to index.
                    ModelState.AddModelError("Email", "Invalid Email/Password.");
                    return View("Index");
                }
                else
                {
                    // hash the entered password and check it against the stored pwhash.
                    var hasher = new PasswordHasher<LogUser>();
                    var result = hasher.VerifyHashedPassword(loginAttempt, userInDB.Password, loginAttempt.Password);

                    // 0 response indicates failed verification.
                    if(result == 0)
                    {
                        // return to index page with errors.
                        ModelState.AddModelError("Email", "Invalid Email/Password");
                        return View("Index");
                    }

                    // Used to verify logged in status.  
                    // Needs to be upgraded to a more secure login solution.
                    UserSession = userInDB.UserID;

                    // Session variables used for success page.  
                    // Need success page to be added or these variables to be removed.
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

        // Successful login or registration.  Needs viewpage or removed.
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

        // Logout get method.  clears session and returns to index.  
        // Maybe add a logout viewpage that redirects to index after a few seconds.
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // Default Error handler for MVC template.  This might be a better option, but I need to look into it.

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        // }
    }
}
