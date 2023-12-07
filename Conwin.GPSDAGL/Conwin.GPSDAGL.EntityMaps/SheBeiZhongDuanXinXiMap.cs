
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class SheBeiZhongDuanXinXiMap :  BaseMap<SheBeiZhongDuanXinXi>
    {
        public SheBeiZhongDuanXinXiMap()
        {

            this.Property(t => t.SheBeiXingHao)
                .HasMaxLength(50);

            this.Property(t => t.ShengChanChangJia)
                .HasMaxLength(50);

            this.Property(t => t.ChangJiaBianHao)
                .HasMaxLength(20);

            this.Property(t => t.XingHaoBianMa)
                .HasMaxLength(20);

            this.Property(t => t.ShiYongCheXing)
                .HasMaxLength(50);

            this.Property(t => t.DingWeiMoKuai)
                .HasMaxLength(50);

            this.Property(t => t.TongXunMoShi)
                .HasMaxLength(50);

            this.Property(t => t.ZhongDuanBianMa)
                .HasMaxLength(50);

            this.Property(t => t.GongGaoPiWenFuJianId)
                .HasMaxLength(255);

            this.Property(t => t.ChuangJianRenOrgCode)
                .HasMaxLength(16);

            this.Property(t => t.ZuiJinXiuGaiRenOrgCode)
                .HasMaxLength(16);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_SheBeiZhongDuanXinXi");
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

            this.Property(t => t.SheBeiLeiBie).HasColumnName("SheBeiLeiBie");

            this.Property(t => t.ZhongDuanLeiXing).HasColumnName("ZhongDuanLeiXing");

            this.Property(t => t.SheBeiXingHao).HasColumnName("SheBeiXingHao");

            this.Property(t => t.ShengChanChangJia).HasColumnName("ShengChanChangJia");

            this.Property(t => t.ChangJiaBianHao).HasColumnName("ChangJiaBianHao");

            this.Property(t => t.XingHaoBianMa).HasColumnName("XingHaoBianMa");

            this.Property(t => t.ShiYongCheXing).HasColumnName("ShiYongCheXing");

            this.Property(t => t.DingWeiMoKuai).HasColumnName("DingWeiMoKuai");

            this.Property(t => t.TongXunMoShi).HasColumnName("TongXunMoShi");

            this.Property(t => t.GuoJianPiCi).HasColumnName("GuoJianPiCi");

            this.Property(t => t.ZhongDuanBianMa).HasColumnName("ZhongDuanBianMa");

            this.Property(t => t.GongGaoRiQi).HasColumnName("GongGaoRiQi");

            this.Property(t => t.GongGaoPiWenFuJianId).HasColumnName("GongGaoPiWenFuJianId");

            this.Property(t => t.Remark).HasColumnName("Remark");

            this.Property(t => t.ChuangJianRenOrgCode).HasColumnName("ChuangJianRenOrgCode");

            this.Property(t => t.ZuiJinXiuGaiRenOrgCode).HasColumnName("ZuiJinXiuGaiRenOrgCode");

            this.Property(t => t.ZhuangTai).HasColumnName("ZhuangTai");

			this.CustomMap();
		}
	}

}
