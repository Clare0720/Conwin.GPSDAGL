using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.Dtos
{
    [DataContract(IsReference = true)]
    public class CustomMonitorTreeDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string NodeId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ParentNodeId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string NodeName { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string NodeData { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int NodeIndex { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool Enabled { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<JSTreeDTO> TreeNodes { get; set; }
    }


    public class JSTreeDTO
    {
        public string id { get; set; }
        public string text { get; set; }
        public dynamic data { get; set; }
        public List<JSTreeDTO> children { get; set; }

    }

    [DataContract(IsReference = true)]
    public class JSTreeNodeData
    {
        [DataMember(EmitDefaultValue = false)]
        public string OrgCode { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public int CarCount { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public int CarOnlineCount { get; set; }

    }

}
