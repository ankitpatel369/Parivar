using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parivar.Data.DbModel
{
    [Table("RelationShipMasters")]
    public class RelationShipMaster : EntityWithAudit
    {
        [Required, Column(TypeName = "nvarchar(250)")]
        public string Relation { get; set; }

        public virtual ICollection<FamilyMemberDetails> FamilyMemberDetails { get; set; }
    }
}
