using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace P06.Models
{
    public class PasswordUpdate
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Cannot be empty!")]
        [Remote("VerifyCurrentPassword", "Account", ErrorMessage ="Incorrect password!")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Cannot be empty!")]
        [Remote("VerifyNewPassword", "Account", ErrorMessage = "Cannot reuse password!")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Cannot be empty!")]
        [Compare("NewPassword",ErrorMessage ="Password not confirmed!")]
        public string ConfirmPassword { get; set; }
    }
}
