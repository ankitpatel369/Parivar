using System;
using System.ComponentModel;
namespace Parivar.Dto.ViewModel
{
    public class BaseModel
    {
        public long Id { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DateCreated { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DateModified { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public long TotalRecords { get; set; }
    }
    public class DropdownModel
    {
        public long Value { get; set; }
        public string Text { get; set; }
        public string Extra { get; set; }
    }
    public class RoleDropdownModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
