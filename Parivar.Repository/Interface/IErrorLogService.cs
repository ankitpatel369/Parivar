using Parivar.Data.DbModel;
using System;

namespace Parivar.Repository.Interface
{

    public interface IErrorLogService : IGenericService<ErrorLog>
    {
        void AddErrorLog(Exception ex, string appType);
    }
}
