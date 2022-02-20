using Parivar.Data.DbContext;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parivar.Data.DbModel
{
    [Table("FamilyMemberDetails")]
    public class FamilyMemberDetails : EntityWithAudit
    {
        public long MainMemberId { get; set; }
        [ForeignKey("MainMemberId")]
        public virtual FamilyUser MainMember { get; set; }

        [Required, Column(TypeName = "nvarchar(250)")]
        public string FullName { get; set; }

        public long? BloodGroupId { get; set; }
        [ForeignKey("BloodGroupId")]
        public virtual CategoriesMaster BloodGroup { get; set; }

        public long? BussionessId { get; set; }
        [ForeignKey("BussionessId")]
        public virtual CategoriesMaster Bussioness { get; set; }

        public long? EducationId { get; set; }
        [ForeignKey("EducationId")]
        public virtual CategoriesMaster Education { get; set; }
        public long? RelationShipId { get; set; }
        [ForeignKey("RelationShipId")]
        public virtual CategoriesMaster RelationShip { get; set; }

        public bool IsMarried { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string AdvanceKills { get; set; }

        [Required, Column(TypeName = "nvarchar(50)")]
        public string MosalSurname { get; set; }

        [Required, Column(TypeName = "nvarchar(50)")]
        public string MosalVillage { get; set; }
        public int? Gender { get; set; }
    }
}
