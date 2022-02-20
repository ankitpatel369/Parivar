using Parivar.Data.DbContext;
using Parivar.Data.DbModel;
using Parivar.Dto.ViewModel;
using Parivar.Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Parivar.Repository.Service
{
    public class RelationShipMasterRepository : GenericRepository<RelationShipMaster>, IRelationShipMasterService
    {
        private readonly ParivarDb _db;
        public RelationShipMasterRepository(ParivarDb db) : base(db)
        {
            _db = db;
        }

        public List<DropdownModel> GetRelationShip()
        {
            return _db.RelationShipMasters.Where(x => x.IsActive).Select(x => new DropdownModel
            {
                Text = x.Relation,
                Value = x.Id
            }).ToList();
        }
    }
}
