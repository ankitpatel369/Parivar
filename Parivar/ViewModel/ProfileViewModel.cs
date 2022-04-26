using Microsoft.AspNetCore.Mvc.Rendering;
using Parivar.Dto.ViewModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace Parivar.ViewModel
{
    public class ProfileViewModel : BaseModel
    {
        public string FullName { get; set; }
        public int? Gender { get; set; }
        public string GenderString { get; set; }
        public bool IsMarried { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Location { get; set; }
        public string AdvanceSkills { get; set; }
        public string MosalSurname { get; set; }
        public string MosalVillage { get; set; }
        public string BloodGroup { get; set; }
        public string Education { get; set; }
        public string RelationShip { get; set; }
        public string Bussioness { get; set; }

        [Required]
        public string UserName { get; set; }

        [MinLength(6)]
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
