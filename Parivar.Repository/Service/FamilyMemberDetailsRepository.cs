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
    public class FamilyMemberDetailsRepository : GenericRepository<FamilyMemberDetails>, IFamilyMemberDetailsService
    {
        private readonly ParivarDb _db;
        public FamilyMemberDetailsRepository(ParivarDb db) : base(db)
        {
            _db = db;
        }

        public async Task<List<FamilyMemberList>> GetFamilyList(SqlParameter[] parameters)
        {
            var result = await _db.GetQueryDatatableAsync("GetFamilyList", parameters);
            return Common.ConvertDataTable<FamilyMemberList>(result.Tables[0]);
        }

        public async Task<List<FamilyMemberDetailList>> GetFamilyMemberList(SqlParameter[] parameters)
        {
            var result = await _db.GetQueryDatatableAsync("GetFamilyDetailList", parameters);
            return Common.ConvertDataTable<FamilyMemberDetailList>(result.Tables[0]);
        }
    }
}
