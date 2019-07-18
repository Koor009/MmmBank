using System.ComponentModel.DataAnnotations;
using System;

namespace MmmBankDb.Models
{    /// <summary>
     /// class LoginViewModel
     /// describes the filters for the form
     /// </summary>
    public sealed class LoginViewModel
    {
        [Required(ErrorMessage = "Enter your email")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// class AddModeratorViewModel
    /// describes the filters for the form
    /// </summary>
    public sealed class AddModeratorViewModel
    {
        [Required(ErrorMessage = "Enter your email")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// class RegisterViewModel
    /// describes the filters for the form
    /// </summary>
    public sealed class RegisterViewModel
    {
        [Required(ErrorMessage = "Enter your email")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your name")]
        [Display(Name = "Name")]
        [RegularExpression("[A-Za-zА-Яа-яЁё]{2,}", ErrorMessage = "Name must be from strings and at least 2 symbol")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Type your date of birth")]
        [DataType(DataType.Date)]
        [Display(Name ="Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Enter your surname")]
        [Display(Name ="Last name")]
        [RegularExpression("[A-Za-zА-Яа-яЁё]{2,}", ErrorMessage = "Surname must be from strings and at least 2 symbol")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Enter your country")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Enter your sex")]
        [Display(Name = "Sex")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Enter your phone of number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("[0-9]{10,}", ErrorMessage = "Invalid phone number")]
        [Display(Name ="Phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Enter your sity")]
        [Display(Name ="Sity")]
        public string Sity { get; set; }

        [Required(ErrorMessage = "Enter your sity")]
        [Display(Name = "Street address")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "Enter your password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// class ResetPasswordViewModel
    /// describes the filters for the form
    /// </summary>
    public sealed class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

}
