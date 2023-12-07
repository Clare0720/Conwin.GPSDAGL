using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Conwin.GPSDAGL.Services.Dtos
{
    public partial class CheLiangYeHuDto
    {
        [DataMember(EmitDefaultValue = false)]
        public string ParentOrgId { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string JingYingFanWei { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string XiaQuSheng { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string XiaQuShi { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string XiaQuXian { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string DiZhi { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Nullable<int> ZhuangTai { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ChuangJianRenOrgCode { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ZuiJinXiuGaiRenOrgCode { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string BeiZhu { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string FuZheRen { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string FuZheRenDianHua { get; set; }

        /// <summary>
        /// ������id
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string TpNameId { get; set; }
        /// <summary>
        /// ������������������
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public  string TpCodeName { get; set; }
        /// <summary>
        /// ������ ���
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string FuWuShangOrgCode { get; set; }
        /// <summary>
        /// ������ ����
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string FuWuShangMingChengLabel { get; set; }
        /// <summary>
        /// ��������Ϣ
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string ServiceProviderInformation { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public  string   QyCodeId { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string SelectAThird { get; set; }
        /// <summary>
        /// �������Ƿ����
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool QYUpdate { get; set; }
    }


    public class ServiceProviderInformationModel{

        public List<FuWuShangMing> ServiceProviderInformation { get; set; }

    }

    public class FuWuShangMing
    {
        /// <summary>
        /// ����������
        /// </summary>
     public  string FuWuShangMingChengLabel { get; set; }

        /// <summary>
        /// ������id
        /// </summary>
     public string TpNameId { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
     public string TPCode { get; set; }
    }

}
