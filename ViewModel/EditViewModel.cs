using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModel
{
    public class EditViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public Dept? Department { get; set; }

        public IFormFile Photo { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}
