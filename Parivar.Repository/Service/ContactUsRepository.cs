using Microsoft.Data.SqlClient;
using Parivar.Data.DbContext;
using Parivar.Data.DbModel;
using Parivar.Data.Extensions;
using Parivar.Dto.Enum;
using Parivar.Dto.ViewModel;
using Parivar.Repository.Interface;
using Parivar.Repository.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parivar.Repository.Service
{
    public class ContactUsRepository : GenericRepository<ContactUs>, IContactUsService
    {
        private readonly ParivarDb _db;
        public ContactUsRepository(ParivarDb db) : base(db)
        {
            _db = db;
        }

        public async Task<List<ContactUsModel>> GetContactUsList(SqlParameter[] paraObjects)
        {
            var dataSet = await _db.GetQueryDatatableAsync(StoredProcedureList.GetContactUsList, paraObjects);
            return Common.ConvertDataTable<ContactUsModel>(dataSet.Tables[0]);
        }
    }
}
