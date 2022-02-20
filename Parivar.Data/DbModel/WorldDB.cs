using Parivar.Data.DbContext;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parivar.Data.DbModel
{
    [Table("Countries")]
    public class Countries
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required, Column(TypeName = "nvarchar(3)")]
        public string SortName { get; set; }

        [Required, Column(TypeName = "nvarchar(150)")]
        public string Name { get; set; }
        public long PhoneCode { get; set; }
        public virtual ICollection<States> States { get; set; }
        public virtual ICollection<FamilyUser> ApplicationUsers { get; set; }
    }

    [Table("States")]
    public class States
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required, Column(TypeName = "nvarchar(150)")]
        public string Name { get; set; }
        public long CountryId { get; set; }
        [ForeignKey("CountryId")] public virtual Countries Countries { get; set; }

        public virtual ICollection<Cities> Cities { get; set; }
        public virtual ICollection<Districts> Districts { get; set; }
        public virtual ICollection<FamilyUser> ApplicationUsers { get; set; }
    }

    [Table("Cities")]
    public class Cities
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required, Column(TypeName = "nvarchar(150)")]
        public string Name { get; set; }

        public long StateId { get; set; }
        [ForeignKey("StateId")] public virtual States States { get; set; }
        public virtual ICollection<FamilyUser> ApplicationUsers { get; set; }
    }

    [Table("Districts")]
    public class Districts
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required, Column(TypeName = "nvarchar(150)")]
        public string Name { get; set; }

        public long StateId { get; set; }
        [ForeignKey("StateId")] public virtual States States { get; set; }

        public virtual ICollection<Countys> Countys { get; set; }
        public virtual ICollection<FamilyUser> ApplicationUsers { get; set; }
    }

    [Table("Countys")]
    public class Countys
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required, Column(TypeName = "nvarchar(150)")]
        public string Name { get; set; }

        public long DistrictId { get; set; }
        [ForeignKey("DistrictId")] public virtual Districts District { get; set; }
        public virtual ICollection<Villages> Villages { get; set; }
        public virtual ICollection<FamilyUser> ApplicationUsers { get; set; }
    }

    [Table("Villages")]
    public class Villages
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required, Column(TypeName = "nvarchar(150)")]
        public string Name { get; set; }

        public long CountyId { get; set; }
        [ForeignKey("CountyId")] public virtual Countys Countys { get; set; }
        public virtual ICollection<FamilyUser> ApplicationUsers { get; set; }
    }

}