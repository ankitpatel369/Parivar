using Microsoft.Data.SqlClient;
using Parivar.Data.DbModel;
using Parivar.Dto.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parivar.Repository.Interface
{
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
    }

    public interface ICountyService : IGenericService<Countys>
    {
        Task<List<CountryModel>> GetCountryList(SqlParameter[] parameters);
    }

    public interface IVillageService : IGenericService<Villages>
    {
    }
}
