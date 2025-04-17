using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ContactApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContactApp.Controllers
{

    public class ContactController : Controller
    {
       private readonly IContactInterface _contact;

        public ContactController(IContactInterface contact)
        {
            _contact = contact;
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Create(string id="")
        {
            t_Contact contact=new t_Contact();
            if (id != "")
            {
                contact = await _contact.GetOne(id);
            }
            return View(contact);
        }

        public async Task<ActionResult> List()
        {
             
            if(HttpContext.Session.GetInt32("UserId")!=null)
            {
                List<t_Contact> contact = await _contact.GetAllByUser(HttpContext.Session.GetInt32("UserId").ToString());
                
                return View(contact);
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
           
        }

        [HttpPost]
        public async Task<ActionResult> Create(t_Contact contact)
        {
            if(ModelState.IsValid)
            {
                if(contact.ContactPicture!=null && contact.ContactPicture.Length>0)
                {
                    var fileName=contact.c_Email+Path.GetExtension(contact.ContactPicture.FileName);
                    var filePath=Path.Combine("../ContactApp/wwwroot/contact_images",fileName);
                    Directory.CreateDirectory(Path.Combine("../ContactApp/wwwroot/contact_images"));
                    contact.c_Image=fileName;
                    using(var stream=new FileStream(filePath,FileMode.Create))
                    {
                        contact.ContactPicture.CopyTo(stream);
                    }
                }
                contact.C_UserId=(int)HttpContext.Session.GetInt32("UserId");
                var result=0;
                if (contact.c_ContactId == 0)
                {
                    result = await _contact.Add(contact);
                }
                else
                {
                    result = await _contact.Update(contact);
                }
                if (result == 0)
                {
                    TempData["Message"]="There is some error in add or updating contact";
                    return RedirectToAction("List","Contact");
                }
                else
                {
                    TempData["Message"] = "Added/Updated Successfully";
                    return RedirectToAction("List","Contact");
                }   
            }     
            return View();
        }
        
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            int status = await _contact.Delete(id);   
            if (status == 1)
            {
                ViewData["Message"] = "Deleted Successfully";
                return RedirectToAction("List");
            }
            else
            {
                ViewData["Message"] = "Not Deleted";
                return RedirectToAction("List");
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}  