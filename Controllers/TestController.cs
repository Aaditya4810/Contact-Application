using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ContactApp.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace ContactApp.Controllers
{

    public class TestController : Controller
    {
        private readonly ILogger<TestController> _logger;
        private readonly IValidator<RegisterVM> _validator;

        public TestController(ILogger<TestController> logger,IValidator<RegisterVM> validator)
        {
            _logger = logger;
            _validator = validator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FRegister(RegisterVM model)
        {
            var result = _validator.Validate(instance: model);
            if (!result.IsValid)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model);
            }
            TempData["SuccessMessage"]="Register Successfully";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}