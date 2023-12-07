using System.Linq;
using Conwin.EntityFramework;
using Conwin.GPSDAGL.Entities.Repositories;
namespace Conwin.GPSDAGL.Repository
{
    public class EntityRepository<TEntity> : GenericRepository<TEntity>, IEntityRepository<TEntity> where TEntity : class
    {
        public EntityRepository()
            : base()
        {
        }
        public IQueryable<TEntity> FindAll()
        {
            return GetQuery();
        }
        public TEntity Get(object key)
        {
            return GetByKey(key);
        }
    }
}
