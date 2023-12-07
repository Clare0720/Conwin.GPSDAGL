
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class CheLiangYeHuMap :  BaseMap<CheLiangYeHu>
    {
        public CheLiangYeHuMap()
        {

            this.Property(t => t.BaseId)
                .HasMaxLength(255);

            this.Property(t => t.OrgCode)
                .HasMaxLength(16);

            this.Property(t => t.OrgShortName)
                .HasMaxLength(50);

            this.Property(t => t.OrgName)
                .HasMaxLength(50);

            this.Property(t => t.SuoShuJianKongPingTai)
                .HasMaxLength(50);

            this.Property(t => t.JingYingXuKeZhengHao)
                .HasMaxLength(50);

            this.Property(t => t.GongShangYingYeZhiZhaoHao)
                .HasMaxLength(50);

            this.Property(t => t.QiYeXingZhi)
                .HasMaxLength(50);

            this.Property(t => t.LianXiRen)
                .HasMaxLength(50);

            this.Property(t => t.ChuanZhenHaoMa)
                .HasMaxLength(16);

            this.Property(t => t.LianXiFangShi)
                .HasMaxLength(100);

            this.Property(t => t.JingJiLeiXing)
                .HasMaxLength(30);

            this.Property(t => t.SuoShuQiYe)
                .HasMaxLength(30);

            this.Property(t => t.SheHuiXinYongDaiMa)
                .HasMaxLength(50);

            this.Property(t => t.GeTiHuShenFenZhengHaoMa)
                .HasMaxLength(50);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_CheLiangYeHu");
			});

            this.Property(t => t.SYS_ShuJuLaiYuan).HasColumnName("SYS_ShuJuLaiYuan");

            this.Property(t => t.SYS_ChuangJianRenID).HasColumnName("SYS_ChuangJianRenID");

            this.Property(t => t.SYS_ChuangJianRen).HasColumnName("SYS_ChuangJianRen");

            this.Property(t => t.SYS_ChuangJianShiJian).HasColumnName("SYS_ChuangJianShiJian");

            this.Property(t => t.SYS_ZuiJinXiuGaiRenID).HasColumnName("SYS_ZuiJinXiuGaiRenID");

            this.Property(t => t.SYS_ZuiJinXiuGaiRen).HasColumnName("SYS_ZuiJinXiuGaiRen");

            this.Property(t => t.SYS_ZuiJinXiuGaiShiJian).HasColumnName("SYS_ZuiJinXiuGaiShiJian");

            this.Property(t => t.SYS_XiTongZhuangTai).HasColumnName("SYS_XiTongZhuangTai");

            this.Property(t => t.SYS_XiTongBeiZhu).HasColumnName("SYS_XiTongBeiZhu");

            this.Property(t => t.BaseId).HasColumnName("BaseId");

            this.Property(t => t.OrgCode).HasColumnName("OrgCode");

            this.Property(t => t.OrgType).HasColumnName("OrgType");

            this.Property(t => t.OrgShortName).HasColumnName("OrgShortName");

            this.Property(t => t.OrgName).HasColumnName("OrgName");

            this.Property(t => t.SuoShuJianKongPingTai).HasColumnName("SuoShuJianKongPingTai");

            this.Property(t => t.JingYingXuKeZhengHao).HasColumnName("JingYingXuKeZhengHao");

            this.Property(t => t.JingYingXuKeZhengYouXiaoQi).HasColumnName("JingYingXuKeZhengYouXiaoQi");

            this.Property(t => t.JingYingXuKeZhengChangQiYouXiao).HasColumnName("JingYingXuKeZhengChangQiYouXiao");

            this.Property(t => t.GongShangYingYeZhiZhaoHao).HasColumnName("GongShangYingYeZhiZhaoHao");

            this.Property(t => t.GongShangYingYeZhiZhaoYouXiaoQi).HasColumnName("GongShangYingYeZhiZhaoYouXiaoQi");

            this.Property(t => t.GongShangYingYeZhiZhaoChangQiYouXiao).HasColumnName("GongShangYingYeZhiZhaoChangQiYouXiao");

            this.Property(t => t.QiYeXingZhi).HasColumnName("QiYeXingZhi");

            this.Property(t => t.LianXiRen).HasColumnName("LianXiRen");

            this.Property(t => t.ChuanZhenHaoMa).HasColumnName("ChuanZhenHaoMa");

            this.Property(t => t.LianXiFangShi).HasColumnName("LianXiFangShi");

            this.Property(t => t.ShiFouGeTiHu).HasColumnName("ShiFouGeTiHu");

            this.Property(t => t.ShenHeZhuangTai).HasColumnName("ShenHeZhuangTai");

            this.Property(t => t.JingJiLeiXing).HasColumnName("JingJiLeiXing");

            this.Property(t => t.SuoShuQiYe).HasColumnName("SuoShuQiYe");

            this.Property(t => t.QiYeBiaoZhiId).HasColumnName("QiYeBiaoZhiId");

            this.Property(t => t.IsConfirmInfo).HasColumnName("IsConfirmInfo");

            this.Property(t => t.SheHuiXinYongDaiMa).HasColumnName("SheHuiXinYongDaiMa");

            this.Property(t => t.GeTiHuShenFenZhengHaoMa).HasColumnName("GeTiHuShenFenZhengHaoMa");

			this.CustomMap();
		}
	}

}
