using Parivar.Data.DbContext;
using Parivar.Dto.ViewModel;
using System.Collections.Generic;

namespace Parivar.Repository.Interface
{
    public interface IFamilyUserService : IGenericService<FamilyUser>
    {
        List<DropdownModel> GetCityList(long stateId);
        List<DropdownModel> GetCountryList();
        List<DropdownModel> GetCountyList(long districtId);
        List<DropdownModel> GetDistrictList(long stateId);
        List<DropdownModel> GetStateList(long countryId);
        List<DropdownModel> GetVillageList(long countyId);
    }
}
