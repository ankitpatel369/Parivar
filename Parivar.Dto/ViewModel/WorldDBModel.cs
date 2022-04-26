namespace Parivar.Dto.ViewModel
{
    public class CountryModel : BaseModel
    {
        public string SortName { get; set; }
        public string Name { get; set; }
        public long PhoneCode { get; set; }
    }

    public class StateModel : BaseModel
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public long CountryId { get; set; }
    }

    public class CityModel : BaseModel
    {
        public string Name { get; set; }
        public string State { get; set; }
        public long StateId { get; set; }
    }

    public class DistrictModel : BaseModel
    {
        public string Name { get; set; }
        public string State { get; set; }
        public long StateId { get; set; }
    }

    public class CountyModel : BaseModel
    {
        public string Name { get; set; }
        public long DistrictId { get; set; }
        public string District { get; set; }
    }

    public class VillageModel : BaseModel
    {
        public string Name { get; set; }
        public string County { get; set; }
        public long CountyId { get; set; }
        public long DistrictId { get; set; }
    }
}
