﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Field Login is required")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Field Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Required(ErrorMessage = "Confirm Password is required")]
        //[StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        //[DataType(DataType.Password)]
        //[Compare("Password")]
        //public string ConfirmPassword { get; set; }

        public User(int id, string name,string surname,string email, string login, string password)
        {
            this.Id = id;
            this.Name = name;   
            this.Surname = surname;
            this.Email = email;
            this.Login = login;
            this.Password = password;
        }




        public string FullName
        {
            get
            {
                return Name + " " + Surname;
            }
        }

    }
}
