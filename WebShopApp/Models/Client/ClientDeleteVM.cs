
﻿using System.ComponentModel.DataAnnotations;

namespace WebShopApp.Models.Client
{
    public class ClientDeleteVM
    {
        public string? Id { get; set; }

        [Display(Name = "Username")]
        public string? UserName { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }


        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
    }
}