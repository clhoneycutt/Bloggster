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


        // Connection to the database
        private BlogContext _dbContext;
        public HomeController(BlogContext context)
        {
            _dbContext = context;
        }

        // Landing Page GET Route
        [HttpGet("")]
        public IActionResult Index()
        {
            // Query for most recent 6 blog posts
            List<Post> RecentPosts = _dbContext.Posts
                                        .OrderByDescending(p => p.CreatedAt)
                                        .Take(6)
                                        .ToList();
            return View(RecentPosts);
        }

        // Single blog post route
        // TODO: Update route to be a variable for postID
        [HttpGet("show")]
        public IActionResult Show()
        {
            return View();
        }

        // Blog Create GET Route
        [HttpGet("new")]
        public IActionResult New()
        {
            // if(UserSession == null)
            // {
            //     return RedirectToAction(nameof(Index));
            // }
            return View();
        }


        // Blog Create POST Route
        [HttpPost("create")]
        public IActionResult Create(Post post)
        {

            // if(UserSession == null)
            // {
            //     return RedirectToAction(nameof(Index));
            // }

            if(ModelState.IsValid)
            {
                _dbContext.Posts.Add(post);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("New");
        }

        [HttpGet("edit")]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost("update")]
        public IActionResult UpdatePost()
        {
            return RedirectToAction("Index");
        }

        [HttpGet("delete")]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost("remove")]
        public IActionResult Remove()
        {
            return RedirectToAction("Index");
        }

        // About page
        [HttpGet("about")]
        public IActionResult About()
        {
            return View();
        }

        // Contact page
        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return View();
        }

    }
}