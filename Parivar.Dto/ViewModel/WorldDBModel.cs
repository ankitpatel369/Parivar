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
    }

    public class CityModel : BaseModel
    {
        public string Name { get; set; }
        public string State { get; set; }
    }
}
