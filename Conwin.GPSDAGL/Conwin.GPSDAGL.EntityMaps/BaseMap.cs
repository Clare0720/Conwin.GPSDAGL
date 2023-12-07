using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Conwin.EntityFramework;
namespace Conwin.GPSDAGL.EntityMaps
{
    public abstract class BaseMap<T> : EntityTypeConfiguration<T> where T : BaseEntity
    {
        public BaseMap()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("Id");
        }
        public virtual void CustomMap()
        {
        }
    }
}
