using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Conwin.GPSDAGL.Services.Dtos
{
    [Serializable]
    [DataContract(IsReference = true)]
    public partial class BaseDto
    {
        [DataMember(EmitDefaultValue = false)]
        public virtual string Id { get; set; }
    }
}
