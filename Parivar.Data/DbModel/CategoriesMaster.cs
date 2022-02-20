using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parivar.Data.DbModel
{
    [Table("CategoriesMaster")]
    public class CategoriesMaster : EntityWithAudit
    {
        [Required,Column(TypeName = "nvarchar(250)")]
        public string CategoryName { get; set; }

        [Required]
        public int Categories { get; set; }

        [InverseProperty("BloodGroup")]
        public virtual ICollection<FamilyMemberDetails> BloodGroupOfFamilyMember { get; set; }

        [InverseProperty("Bussioness")]
        public virtual ICollection<FamilyMemberDetails> BussionessOfFamilyMember { get; set; }

        [InverseProperty("Education")]
        public virtual ICollection<FamilyMemberDetails> EducationOfFamilyMember { get; set; }

        [InverseProperty("RelationShip")]
        public virtual ICollection<FamilyMemberDetails> RelationShipOfFamilyMember { get; set; }
    }
}
