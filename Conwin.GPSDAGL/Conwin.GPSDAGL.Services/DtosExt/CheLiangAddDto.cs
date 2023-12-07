using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Conwin.GPSDAGL.Services.Dtos
{
    [DataContract(IsReference = true)]
    public class CheLiangAddDto : EntityMetadataDto 
    {

        [DataMember(EmitDefaultValue = false)]
        public string JingYingFanWei { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Nullable<int> YeWuBanLiZhuangTai { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Nullable<int> ShiFouGuFei { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ChuangJianRenOrgCode { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ZuiJinXiuGaiRenOrgCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? YeHuOrgType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CheDuiOrgCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? CheDuiOrgType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FuWuShangOrgCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [Description("���ƺ�")]
        public string ChePaiHao { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        [Description("������ɫ")]
        public string ChePaiYanSe { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        [Description("��������")]
        public Nullable<int> CheLiangLeiXing { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        [Description("��������")]
        public Nullable<int> CheLiangZhongLei { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        [Description("���ص绰")]
        public string CheZaiDianHua { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("Ӫ��״̬")]
        public string YunZhengZhuangTai { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("����ƽ̨")]
        public string SuoShuPingTai { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("���ܺ�")]
        public string CheJiaHao { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("Ӫ��֤��")]
        public string YunYingZhengHao { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("����״̬")]
        public int NianShenZhuangTai { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("Ͻ��ʡ")]
        public string XiaQuSheng { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        [Description("Ͻ����")]
        public string XiaQuShi { get; set; }
    	[DataMember(EmitDefaultValue = false)]
        [Description("Ͻ����")]
        public string XiaQuXian { get; set; }
        [DataMember(EmitDefaultValue = false)]
        [Description("ҵ������")]
        public string YeHuOrgCode { get; set; }
        [Description("��ע")]
        [DataMember(EmitDefaultValue = false)]
        public string Remark { get; set; }

        [Description("�˹����״̬")]
        [DataMember(EmitDefaultValue = false)]
        public  int ManualApprovalStatus { get; set; }













    }


   public class CheLiangExInfoDto
    {
        public string CheLiangId { get; set; }
        public string ChuangJianRenOrgCode { get; set; }
        public string ZuiJinXiuGaiRenOrgCode { get; set; }
        public Nullable<System.Guid> CheLiangBiaoZhiId { get; set; }
        public Nullable<int> ZhuangTai { get; set; }
        public Nullable<int> CheZhouShu { get; set; }

        [Description("������ɫ")]
        public Nullable<int> CheShenYanSe { get; set; }
        [Description("��������")]
        public string CheLiangNengLi { get; set; }
        [Description("����Ʒ��")]
        public string CheLiangPinPai { get; set; }
        [Description("�����ͺ�")]
        public string XingHao { get; set; }
        [Description("��·����֤��")]
        public string DaoLuYunShuZhengHao { get; set; }
        [Description("������·����֤����ʱ��")]
        public Nullable<System.DateTime> DaoLuYunShuZhengNianShenRiQi { get; set; }
        [Description("������·����֤����Ч��")]
        public Nullable<System.DateTime> DaoLuYunShuZhengYouXiaoQi { get; set; }
        [Description("������·����֤��������")]
        public Nullable<int> DaoLuYunShuZhengTiXingTianShu { get; set; }
        [Description("��ʻ֤��")]
        public string XingShiZhengHao { get; set; }
        [Description("��ʻ֤����ʱ��")]
        public Nullable<System.DateTime> XingShiZhengNianShenRiQi { get; set; }
        [Description("��ʻ֤������Ч��")]
        public Nullable<System.DateTime> XingShiZhengYouXiaoQi { get; set; }
        [Description("��ʻ֤������������")]
        public Nullable<int> XingShiZhengTiXingTianShu { get; set; }
        [Description("��ʻ֤�Ǽ�����")]
        public Nullable<System.DateTime> XingShiZhengDengJiRiQi { get; set; }
        [Description("��ʻ֤ɨ�����Ƭ")]
        public string XingShiZHengSaoMiaoJianId { get; set; }
        [Description("��ʻ֤��ַ")]
        public string XingShiZhengDiZhi { get; set; }
        [Description("��ά����")]
        public Nullable<System.DateTime> ErWeiRiQi { get; set; }
        [Description("�´ζ�ά����")]
        public Nullable<System.DateTime> XiaCiErWeiRiQi { get; set; }
        [Description("������Ч��")]
        public Nullable<System.DateTime> ShenYanYouXiaoQi { get; set; }
        [Description("������������")]
        public string CheLiangBaoXiaoZhongLei { get; set; }
        [Description("�������յ���ʱ��")]
        public Nullable<System.DateTime> CheLiangBaoXiaoDaoJieZhiRiQi { get; set; }
        [Description("��������")]
        public Nullable<System.DateTime> ChuChangRiQi { get; set; }
        [Description("���������")]
        public string FaDongJiHao { get; set; }
        [Description("������(kg)")]
        public string ZongZhiLiang { get; set; }
        [Description("����(mm)")]
        public Nullable<decimal> CheGao { get; set; }
        [Description("����(mm)")]
        public Nullable<decimal> CheChang { get; set; }
        [Description("����(mm)")]
        public Nullable<decimal> CheKuan { get; set; }
        [Description("��λ")]
        public Nullable<double> DunWei { get; set; }
        [Description("׼ǣ��������")]
        public Nullable<double> ZhunQianYinZongZhiLiang { get; set; }
        [Description("��������(Kg)")]
        public Nullable<double> ZhengBeiZhiLiang { get; set; }
        [Description("��������(Kg)")]
        public Nullable<double> HeZaiZhiLiang { get; set; }
        [Description("������")]
        public string PaiQiLiang { get; set; }
        [Description("ȼ��")]
        public Nullable<int> RanLiao { get; set; }
        [Description("����")]
        public string ZuoXing { get; set; }
        [Description("��λ")]
        public Nullable<int> ZuoWei { get; set; }
        [Description("�����ȼ�")]
        public string JiShuDengJi { get; set; }
        [Description("װ���ȼ�")]
        public string AnZhuangDengJi { get; set; }
        [Description("����")]
        public Nullable<int> ZhouShu { get; set; }

        [Description("��Ӫ��Χ")]
        public string JingYingFanWei { get; set; }
        [Description("��������")]
        public string HuoWuMingCheng { get; set; }
        [Description("�����λ")]
        public Nullable<double> HuoWuDunWei { get; set; }
        [Description("ʼ����")]
        public string ShiFaDi { get; set; }

        [Description("ʼ��վ")]
        public string ShiFaZhan { get; set; }
        [Description("������")]
        public string QiFaDi { get; set; }
        [Description("����վ")]
        public string QiDianZhan { get; set; }
        [Description("�Ӳ�����")]
        public string JieBoCheLiang { get; set; }

    }
}
