using System.Linq;
using Conwin.EntityFramework;
namespace Conwin.GPSDAGL.Entities.Repositories
{
    public interface IEntityRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> FindAll();
        TEntity Get(object key);
    }
}
