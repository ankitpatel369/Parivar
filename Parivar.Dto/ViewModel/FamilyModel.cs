using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parivar.Dto.ViewModel
{
    public class FamilyModel : BaseModel
    {
        [Required(ErrorMessage = "Please enter first name.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }
        [NotMapped] public string FullName => $@"{FirstName} {LastName}";

        [Required(ErrorMessage = "Please enter father name.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string FatherName { get; set; }

        [Required(ErrorMessage = "Please enter no of members in family.")]
        public int NoOfMembers { get; set; }

        [Required(ErrorMessage = "Please enter email."), EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please select gender.")]
        public int Gender { get; set; }

        //[Required, StringLength(32, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        [MaxLength(50)]
        public string ProfilePic { get; set; }

        public string AdvanceKills { get; set; }

        #region Location
        [Required(ErrorMessage = "Please enter Resident Address."),
            StringLength(300, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string ResidentAddress { get; set; }
        public long? CountryId { get; set; }
        public long? StateId { get; set; }
        public long? CityId { get; set; }
        public long? DistrictId { get; set; }
        public long? CountyId { get; set; }
        public long? VillageId { get; set; }
        #endregion

        [Required(ErrorMessage = "Please enter mobile."), MaxLength(20)]
        public string PhoneNumber { get; set; }

        public List<FamilyMemberDetailsModel> FamilyMemberDetails { get; set; }
    }

    public class FamilyMemberDetailsModel : BaseModel
    {
        public long MainMemberId { get; set; }

        [Required(ErrorMessage = "Please enter full name.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string FullName { get; set; }

        [Required]
        public bool IsMarried { get; set; }

        [NotMapped]
        public DateTime DateOfBirth => Convert.ToDateTime(DateOfBirthInStr);

        [Required(ErrorMessage = "Please enter birth date.")]
        public string DateOfBirthInStr { get; set; }

        [Required(ErrorMessage = "Please select relationship.")]
        public long RelationShipId { get; set; }

        [Required(ErrorMessage = "Please select education.")]
        public long EducationId { get; set; }

        [Required(ErrorMessage = "Please select bussioness.")]
        public long BussionessId { get; set; }

        [Required(ErrorMessage = "Please select blood group.")]
        public long BloodGroupId { get; set; }

        public string AdvanceKills { get; set; }

        [Required(ErrorMessage = "Please enter mosal surname.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string MosalSurname { get; set; }

        [Required(ErrorMessage = "Please enter mosal village.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string MosalVillage { get; set; }

        [Required(ErrorMessage = "Please select gender.")]
        public int Gender { get; set; }
    }

    public class FamilyMemberDetailsForChartModel : BaseModel
    {
        public long MainMemberId { get; set; }
        public string MainMemberName { get; set; }

        public string FullName { get; set; }

        public string DateOfBirthInStr { get; set; }

        public string RelationShip { get; set; }

        public string Education { get; set; }

        public string Bussioness { get; set; }

        public string BloodGroup { get; set; }

        public string AdvanceKills { get; set; }

        public string MosalSurname { get; set; }

        public string MosalVillage { get; set; }

        public int Gender { get; set; }
    }

    public class FamilyMemberList : BaseModel
    {
        public string FullName { get; set; }
        public int Gender { get; set; }
        public string PhoneNumber { get; set; }

        public string VillageName { get; set; }
    }

    public class FamilyMemberDetailList : BaseModel
    {
        public string FullName { get; set; }
        public int Gender { get; set; }
        public bool IsMarried { get; set; }
        public string MosalSurname { get; set; }
        public string MosalVillage { get; set; }
        public string AdvanceKills { get; set; }
        public string BloodGroup { get; set; }
        public string Education { get; set; }
        public string RelationShip { get; set; }
        public string Bussioness { get; set; }
    }
}
