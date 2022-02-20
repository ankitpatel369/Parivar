using System.ComponentModel.DataAnnotations;

namespace Parivar.Dto.ViewModel
{
    public class ContactUsModel : BaseModel
    {
        [Required(ErrorMessage = "Your name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required"), EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }

        public bool IsRead { get; set; }
    }
}
