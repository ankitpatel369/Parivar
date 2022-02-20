using Microsoft.Data.SqlClient;
using Parivar.Data.DbModel;
using Parivar.Dto.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parivar.Repository.Interface
{
    public interface IFamilyMemberDetailsService : IGenericService<FamilyMemberDetails>
    {
        Task<List<FamilyMemberList>> GetFamilyList(SqlParameter[] parameters);
        Task<List<FamilyMemberDetailList>> GetFamilyMemberList(SqlParameter[] parameters);
    }
}
