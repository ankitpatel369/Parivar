using Microsoft.Data.SqlClient;
using Parivar.Data.DbContext;
using Parivar.Data.DbModel;
using Parivar.Data.Extensions;
using Parivar.Dto.ViewModel;
using Parivar.Repository.Interface;
using Parivar.Repository.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parivar.Repository.Service
{
    public class CategoriesMasterRepository : GenericRepository<CategoriesMaster>, ICategoriesMasterService
    {
        private readonly ParivarDb _db;
        public CategoriesMasterRepository(ParivarDb db) : base(db)
        {
            _db = db;
        }

        public List<DropdownModel> GetBloodGroupList(string label)
        {
            List<DropdownModel> BloodGroupList = new List<DropdownModel>();
            BloodGroupList.Add(new DropdownModel { Text = label });
            BloodGroupList.AddRange(_db.CategoriesMasters.Where(x => x.Categories == (int)Categories.BloodGroup && x.IsActive).Select(x => new DropdownModel { Text = x.CategoryName, Value = x.Id }).ToList());
            return BloodGroupList;
        }

        public List<DropdownModel> GetBusinessList(string label)
        {
            List<DropdownModel> BusinessList = new List<DropdownModel>();
            BusinessList.Add(new DropdownModel { Text = label });
            BusinessList.AddRange(_db.CategoriesMasters.Where(x => x.Categories == (int)Categories.Business && x.IsActive).Select(x => new DropdownModel { Text = x.CategoryName, Value = x.Id }).ToList());
            return BusinessList;
        }

        public async Task<List<CategoriesModel>> GetCategoriesMasterList(SqlParameter[] parameters)
        {
            var result = await _db.GetQueryDatatableAsync("GetCategoriesList", parameters);
            return Common.ConvertDataTable<CategoriesModel>(result.Tables[0]);
        }

        public List<DropdownModel> GetEducationList(string label)
        {
            List<DropdownModel> EducationList = new List<DropdownModel>();
            EducationList.Add(new DropdownModel { Text = label });
            EducationList.AddRange(_db.CategoriesMasters.Where(x => x.Categories == (int)Categories.Education && x.IsActive).Select(x => new DropdownModel { Text = x.CategoryName, Value = x.Id }).ToList());
            return EducationList;
        }

        public List<DropdownModel> GetRelationShipList(string label)
        {
            List<DropdownModel> RelationShipList = new List<DropdownModel>();
            RelationShipList.Add(new DropdownModel { Text = label });
            RelationShipList.AddRange(_db.CategoriesMasters.Where(x => x.Categories == (int)Categories.RelationShip && x.IsActive).Select(x => new DropdownModel { Text = x.CategoryName, Value = x.Id }).ToList());
            return RelationShipList;
        }
    }
}
