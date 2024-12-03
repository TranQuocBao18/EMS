﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Model.Models.Others
{
    public class LoginModel
    {
		[Required(ErrorMessage = "Username is required.")]
		public string Username { get; set; }
		[Required(ErrorMessage = "Password is required.")]
		public string Password { get; set; }
    }
}
