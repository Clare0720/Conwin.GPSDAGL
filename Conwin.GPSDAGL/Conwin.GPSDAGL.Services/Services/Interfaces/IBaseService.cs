using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Conwin.GPSDAGL.Services.Interfaces
{
    public interface IBaseService<TEntity> : IDisposable where TEntity : class
    {
    }
}
