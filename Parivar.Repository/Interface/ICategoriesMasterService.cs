using Microsoft.Data.SqlClient;
using Parivar.Data.DbModel;
using Parivar.Dto.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parivar.Repository.Interface
{
    public interface ICategoriesMasterService : IGenericService<CategoriesMaster>
    {
        Task<List<CategoriesModel>> GetCategoriesMasterList(SqlParameter[] parameters);
        List<DropdownModel> GetEducationList(string label);
        List<DropdownModel> GetBusinessList(string label);
        List<DropdownModel> GetBloodGroupList(string label);
        List<DropdownModel> GetRelationShipList(string label);
    }
}
