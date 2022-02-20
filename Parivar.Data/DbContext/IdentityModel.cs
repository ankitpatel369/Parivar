using Microsoft.AspNetCore.Identity;
using Parivar.Data.DbModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parivar.Data.DbContext
{
    public class UserRole : IdentityUserRole<long> { }

    public class UserClaim : IdentityUserClaim<long> { }

    public class UserLogin : IdentityUserLogin<long> { }

    public class Role : IdentityRole<long>
    {
        public Role()
        {
            DisplayRoleName = "";
        }
        public string DisplayRoleName { get; set; }
    }

    public class FamilyUser : IdentityUser<long>
    {
        public FamilyUser()
        {
            ProfilePic = "";
        }

        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }

        [NotMapped] public string FullName => $@"{FirstName} {LastName}";

        [Column(TypeName = "nvarchar(50)")]
        public string FatherName { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string ProfilePic { get; set; }

        [Required]
        public int NoOfMembers { get; set; }

        public int? Gender { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string AdvanceKills { get; set; }

        #region Location
        [Column(TypeName = "nvarchar(300)")]
        public string ResidentAddress { get; set; }

        public long? CountryId { get; set; }
        [ForeignKey("CountryId")] public virtual Countries Countries { get; set; }

        public long? StateId { get; set; }
        [ForeignKey("StateId")] public virtual States States { get; set; }

        public long? CityId { get; set; }
        [ForeignKey("CityId")] public virtual Cities Cities { get; set; }

        public long? DistrictId { get; set; }
        [ForeignKey("DistrictId")] public virtual Districts Districts { get; set; }

        public long? CountyId { get; set; }
        [ForeignKey("CountyId")] public virtual Countys Countys { get; set; }

        public long? VillageId { get; set; }
        [ForeignKey("VillageId")] public virtual Villages Villages { get; set; }

        #endregion

        public bool IsActive { get; set; }

        public DateTime? LastLogin { get; set; }

        public virtual ICollection<FamilyMemberDetails> FamilyMemberDetails { get; set; }
    }
}
