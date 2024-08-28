using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Utilities;

namespace WebApplication1.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        //[ValidEmailDomainAttribute(allowedDomain: "test.com", ErrorMessage = "Email domain must be test.com")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public int ActiveUsers { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
