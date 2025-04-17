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

    public class AjaxController : Controller
    {
        private readonly IContactInterface _contactRepo;

        public AjaxController(IContactInterface contactRepo)
        {
            _contactRepo = contactRepo;
        }

        public async Task<IActionResult> GetContacts(int id = 0)
        {
            List<t_Contact> contacts = await _contactRepo.GetAllByUser(id.ToString());
            return Ok(contacts);
        }

        public async Task<ActionResult> Index()
        {
            // if (HttpContext.Session.GetInt32("UserId") != null)
            // {
            //     List<t_Contact> contacts = await

            //     _contactRepo.GetAllByUser(HttpContext.Session.GetInt32("UserId").ToString());

            //     return View(contacts);
            // }
            // else
            // {
            //     return RedirectToAction("Index", "Ajax");
            // }
            return View();
        }
        public async Task<ActionResult> Logout()
        {
            HttpContext.Session.Clear();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index", "Contact");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetContactById(string id)
        {
            t_Contact contact = new t_Contact();
            contact = await _contactRepo.GetOne(id);
            if (contact == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "There was no contact found"
                });
            }
            return Ok(contact);

        }
        [HttpPost]
        public async Task<ActionResult> Create(t_Contact contact)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetInt32("UserId") != null)
                {
                    if (contact.ContactPicture != null && contact.ContactPicture.Length > 0)
                    {

                        var fileName = Guid.NewGuid().ToString() +
                        Path.GetExtension(contact.ContactPicture.FileName);

                        var filePath = Path.Combine("../ContactApp/wwwroot/contact_images", fileName);
                        Directory.CreateDirectory(Path.Combine("../ContactApp/wwwroot/contact_images"));
                        contact.c_Image = fileName;
                        System.IO.File.Delete(filePath);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            contact.ContactPicture.CopyTo(stream);
                        }
                    }
                    contact.C_UserId = (int)HttpContext.Session.GetInt32("UserId");
                    var result = 0;
                    if (contact.c_ContactId == 0)
                    {
                        result = await _contactRepo.Add(contact);
                        if (result == 0)
                        {
                            return BadRequest(new
                            {
                                success = false,
                                message = "There was some error while adding the contact"
                            });


                        }
                        else
                        {
                            return Ok(new
                            {
                                success = true,
                                message = "Contact Insterted Successfully.."

                            });

                        }

                    }
                    else
                    {
                        result = await _contactRepo.Update(contact);
                        if (result == 0)
                        {
                            return BadRequest(new
                            {
                                success = false,
                                message = "There was some error while updating the contact"
                            });


                        }
                        else
                        {
                            return Ok(new
                            {
                                success = true,
                                message = "Contact Updated Successfully.."

                            });

                        }

                    }
                }
                else
                {

                    return BadRequest(new
                    {
                        success = false,
                        message = "There was some error while adding the contact"
                    });
                }
            }
            else
            {
                var errors = ModelState.Where(e => e.Value.Errors.Count > 0)
                 .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(err => err.ErrorMessage).ToArray()
                );
                foreach (var error in ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return BadRequest(new { success = false, message = errors });
            }

        }
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            int status = await _contactRepo.Delete(id);
            if (status == 1)
            {
                return Ok(new { success = true, message = "Contact Deleted Successfully!!!!!" });
            }
            else
            {
                return BadRequest(new
                {
                    success = false,
                    message = "There was some error while deleting the contact"
                });
            }
        }

        public IActionResult Error()
        {
            return View("Error!");
        }

    }
}

