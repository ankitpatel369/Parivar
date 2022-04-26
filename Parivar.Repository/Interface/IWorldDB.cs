using Microsoft.Data.SqlClient;
using Parivar.Data.DbModel;
using Parivar.Dto.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parivar.Repository.Interface
{
    public interface ICountryService : IGenericService<Countries>
    {
        Task<List<CountryModel>> GetCountryList(SqlParameter[] parameters);
    }
    public interface IStateService : IGenericService<States>
    {
        Task<List<StateModel>> GetStateList(SqlParameter[] parameters);
    }

    public interface ICityService : IGenericService<Cities>
    {
        Task<List<CityModel>> GetCityList(SqlParameter[] parameters);
    }

    public interface IDistrictService : IGenericService<Districts>
    {
        Task<List<DistrictModel>> GetDistrictList(SqlParameter[] parameters);
    }

    public interface ICountyService : IGenericService<Countys>
    {
        Task<List<CountyModel>> GetCountyList(SqlParameter[] parameters);
    }

    public interface IVillageService : IGenericService<Villages>
    {
        Task<List<VillageModel>> GetVillageList(SqlParameter[] parameters);
    }
}
