using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parivar.Data.DbModel
{
    [Table("ContactUs")]
    public class ContactUs : EntityWithAudit
    {
        [Required, Column(TypeName = "nvarchar(255)")]
        public string FullName { get; set; }

        [Required, Column(TypeName = "nvarchar(255)")]
        public string Email { get; set; }

        [Required, Column(TypeName = "nvarchar(255)")]
        public string Subject { get; set; }

        [Required, Column(TypeName = "nvarchar(255)")]
        public string Message { get; set; }

        public bool IsRead { get; set; }
    }
}