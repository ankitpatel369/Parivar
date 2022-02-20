using Parivar.Data.DbModel;
using Parivar.Dto.ViewModel;
using System.Collections.Generic;

namespace Parivar.Repository.Interface
{
    public interface IRelationShipMasterService : IGenericService<RelationShipMaster>
    {
        List<DropdownModel> GetRelationShip();
    }
}
