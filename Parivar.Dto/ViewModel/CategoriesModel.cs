using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Parivar.Dto.ViewModel
{
    public class CategoriesModel : BaseModel
    {
        [Required(ErrorMessage = "Category name is required")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Category type is required")]
        public Categories Categories { get; set; }

        public string CategoryType { get; set; }
    }

    public enum Categories
    {
        [Description("Education")]
        Education = 1,
        [Description("Business")]
        Business = 2,
        [Description("Blood Group")]
        BloodGroup = 3,
        [Description("RelationShip")]
        RelationShip = 4
    }
}
