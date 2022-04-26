using Microsoft.Data.SqlClient;
using Parivar.Data.DbContext;
using Parivar.Data.DbModel;
using Parivar.Data.Extensions;
using Parivar.Dto.ViewModel;
using Parivar.Repository.Interface;
using Parivar.Repository.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parivar.Repository.Service
{
    public class CountryRepository : GenericRepository<Countries>, ICountryService
    {
        private readonly ParivarDb _db;
        public CountryRepository(ParivarDb db) : base(db)
        {
            _db = db;
        }

        public async Task<List<CountryModel>> GetCountryList(SqlParameter[] parameters)
        {
            var result = await _db.GetQueryDatatableAsync("GetCountryList", parameters);
            return Common.ConvertDataTable<CountryModel>(result.Tables[0]);
        }
    }

    public class StateRepository : GenericRepository<States>, IStateService
    {
        private readonly ParivarDb _db;
        public StateRepository(ParivarDb db) : base(db)
        {
            _db = db;
        }

        public async Task<List<StateModel>> GetStateList(SqlParameter[] parameters)
        {
            var result = await _db.GetQueryDatatableAsync("GetStateList", parameters);
            return Common.ConvertDataTable<StateModel>(result.Tables[0]);
        }
    }

    public class CityRepository : GenericRepository<Cities>, ICityService
    {
        private readonly ParivarDb _db;
        public CityRepository(ParivarDb db) : base(db)
        {
            _db = db;
        }

        public async Task<List<CityModel>> GetCityList(SqlParameter[] parameters)
        {
            var result = await _db.GetQueryDatatableAsync("GetCityList", parameters);
            return Common.ConvertDataTable<CityModel>(result.Tables[0]);
        }
    }

    public class DistrictRepository : GenericRepository<Districts>, IDistrictService
    {
        private readonly ParivarDb _db;
        public DistrictRepository(ParivarDb db) : base(db)
        {
            _db = db;
        }

        public async Task<List<DistrictModel>> GetDistrictList(SqlParameter[] parameters)
        {
            var result = await _db.GetQueryDatatableAsync("GetDistrictList", parameters);
            return Common.ConvertDataTable<DistrictModel>(result.Tables[0]);
        }
    }

    public class CountyRepository : GenericRepository<Countys>, ICountyService
    {
        private readonly ParivarDb _db;
        public CountyRepository(ParivarDb db) : base(db)
        {
            _db = db;
        }

        public async Task<List<CountyModel>> GetCountyList(SqlParameter[] parameters)
        {
            var result = await _db.GetQueryDatatableAsync("GetCountyList", parameters);
            return Common.ConvertDataTable<CountyModel>(result.Tables[0]);
        }
    }

    public class VillageRepository : GenericRepository<Villages>, IVillageService
    {
        private readonly ParivarDb _db;
        public VillageRepository(ParivarDb db) : base(db)
        {
            _db = db;
        }

        public async Task<List<VillageModel>> GetVillageList(SqlParameter[] parameters)
        {
            var result = await _db.GetQueryDatatableAsync("GetVillageList", parameters);
            return Common.ConvertDataTable<VillageModel>(result.Tables[0]);
        }
    }
}
