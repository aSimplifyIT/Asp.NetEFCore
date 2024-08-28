using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModel
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
