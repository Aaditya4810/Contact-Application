using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using ContactApp.Models;
using ContactApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContactApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserInterface _user;

        public UserController(IUserInterface user)
        {
            _user = user;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<ActionResult> Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }




        [HttpPost]
        public async Task<ActionResult> Create(t_User user)
        {
            if (ModelState.IsValid)
            {
                if (user.ProfilePicture != null && user.ProfilePicture.Length > 0)
                {
                    // Save the uploaded file
                    var fileName = user.c_Email + Path.GetExtension(user.ProfilePicture.FileName);
                    var filePath = Path.Combine("../ContactApp/wwwroot/profile_images", fileName);
                    Directory.CreateDirectory(Path.Combine("../ContactApp/wwwroot/profile_images"));
                    user.c_Image = fileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        user.ProfilePicture.CopyTo(stream);
                    }
                }
                int result = await _user.Add(user);
                if (result == -1)
                {
                    TempData["Error"] = "There is some error in add or updating contact";
                    // return View("Register");
                    
                }
                else if (result == 1)
                {
                    TempData["Success"] = "Added/Updated Successfully";
                    // return View("Register");
                    
                }
                else
                {
                    TempData["Error"] = "User already exist with same email id";
                    // return View("Register");
                
                }
            }
            return View("Register");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(vm_Login login)
        {


            if (ModelState.IsValid)
            {
                t_User userData = await _user.Login(login);
                if (userData.c_UserId != 0)
                {
                    HttpContext.Session.SetInt32("UserId", userData.c_UserId);
                    HttpContext.Session.SetString("UserName", userData.c_UserName);
                    HttpContext.Session.SetString("UserEmail", userData.c_Email);
                    HttpContext.Session.SetString("Address", userData.c_Address);
                    HttpContext.Session.SetString("img", userData.c_Image);

                    // return RedirectToAction("Index", "User");
                    TempData["Success"]="Login Successfully";
                    
                }
                else
                {
                    TempData["Error"] = "Invalid Username and Password";
                  
                }
            }
            return View("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}