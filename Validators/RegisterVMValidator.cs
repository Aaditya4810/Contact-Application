using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ContactApp.Models;
using FluentValidation;

namespace ContactApp.Validators
{
    public class RegisterVMValidator:AbstractValidator<RegisterVM>
    {
         public RegisterVMValidator()
         {
            RuleFor(x => x.Name)
              .NotEmpty().WithMessage("Name is required.")
              .MinimumLength(3).WithMessage("Name must be at least 3 characters.");


            RuleFor(x => x.Email)
              .NotEmpty().WithMessage("Email is required.")
              .EmailAddress().WithMessage("Invalid Email Format.");


            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Password is required.")
               .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$")
               .WithMessage("Password must contain at least one lowercase letter, one uppercase letter, one number, and one special character.");
           
         }
    }
}