using Parivar.Data.DbContext;
using Parivar.Dto.ViewModel;
using Parivar.Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Parivar.Repository.Service
{
    public class FamilyUserRepository : GenericRepository<FamilyUser>, IFamilyUserService
    {
        private readonly ParivarDb _db;
        public FamilyUserRepository(ParivarDb db) : base(db)
        {
            _db = db;
        }

        public List<DropdownModel> GetCityList(long stateId)
        {
            return _db.Cities.Where(x => x.StateId == stateId).Select(x => new DropdownModel
            {
                Text = x.Name,
                Value = x.Id
            }).ToList();
        }

        public List<DropdownModel> GetCountryList()
        {
            return _db.Countries.Select(x => new DropdownModel
            {
                Extra = x.SortName,
                Text = x.Name,
                Value = x.Id
            }).ToList();
        }

        public List<DropdownModel> GetCountyList(long districtId)
        {
            return _db.Countys.Where(x => x.DistrictId == districtId)
                .Select(x => new DropdownModel
                {
                    Text = x.Name,
                    Value = x.Id
                }).ToList();
        }

        public List<DropdownModel> GetDistrictList(long stateId)
        {
            return _db.Districts.Where(x => x.StateId == stateId).Select(x => new DropdownModel
            {
                Text = x.Name,
                Value = x.Id
            }).ToList();
        }

        public List<DropdownModel> GetStateList(long countryId)
        {
            return _db.States.Where(x => x.CountryId == countryId).Select(x => new DropdownModel
            {
                Text = x.Name,
                Value = x.Id
            }).ToList();
        }

        public List<DropdownModel> GetVillageList(long countyId)
        {
            return _db.Villages.Where(x => x.CountyId == countyId)
                .Select(x => new DropdownModel
                {
                    Text = x.Name,
                    Value = x.Id
                }).ToList();
        }
    }
}
