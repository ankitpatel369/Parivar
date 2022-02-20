using Microsoft.Data.SqlClient;
using Parivar.Data.DbModel;
using Parivar.Dto.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parivar.Repository.Interface
{
    public interface IContactUsService : IGenericService<ContactUs>
    {
        Task<List<ContactUsModel>> GetContactUsList(SqlParameter[] paraObjects);
    }
}
