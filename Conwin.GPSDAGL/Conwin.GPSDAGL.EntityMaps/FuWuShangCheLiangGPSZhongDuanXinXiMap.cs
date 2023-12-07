
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class FuWuShangCheLiangGPSZhongDuanXinXiMap :  BaseMap<FuWuShangCheLiangGPSZhongDuanXinXi>
    {
        public FuWuShangCheLiangGPSZhongDuanXinXiMap()
        {

            this.Property(t => t.FuWuShangCheLiangId)
                .HasMaxLength(255);

            this.Property(t => t.ShengChanChangJia)
                .HasMaxLength(50);

            this.Property(t => t.ChangJiaBianHao)
                .HasMaxLength(20);

            this.Property(t => t.SheBeiXingHao)
                .HasMaxLength(20);

            this.Property(t => t.ZhongDuanBianMa)
                .HasMaxLength(20);

            this.Property(t => t.SIMKaHao)
                .HasMaxLength(20);

            this.Property(t => t.ZhongDuanMDT)
                .HasMaxLength(20);

            this.Property(t => t.ShiPinTouAnZhuangXuanZe)
                .HasMaxLength(255);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_FuWuShangCheLiangGPSZhongDuanXinXi");
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

            this.Property(t => t.FuWuShangCheLiangId).HasColumnName("FuWuShangCheLiangId");

            this.Property(t => t.ZhongDuanLeiXing).HasColumnName("ZhongDuanLeiXing");

            this.Property(t => t.ShengChanChangJia).HasColumnName("ShengChanChangJia");

            this.Property(t => t.ChangJiaBianHao).HasColumnName("ChangJiaBianHao");

            this.Property(t => t.SheBeiXingHao).HasColumnName("SheBeiXingHao");

            this.Property(t => t.ZhongDuanBianMa).HasColumnName("ZhongDuanBianMa");

            this.Property(t => t.SIMKaHao).HasColumnName("SIMKaHao");

            this.Property(t => t.ZhongDuanMDT).HasColumnName("ZhongDuanMDT");

            this.Property(t => t.M1).HasColumnName("M1");

            this.Property(t => t.IA1).HasColumnName("IA1");

            this.Property(t => t.IC1).HasColumnName("IC1");

            this.Property(t => t.ShiFouAnZhuangShiPinZhongDuan).HasColumnName("ShiFouAnZhuangShiPinZhongDuan");

            this.Property(t => t.ShiPinTouAnZhuangXuanZe).HasColumnName("ShiPinTouAnZhuangXuanZe");

            this.Property(t => t.ShiPingChangShangLeiXing).HasColumnName("ShiPingChangShangLeiXing");

            this.Property(t => t.ShiPinTouGeShu).HasColumnName("ShiPinTouGeShu");

            this.Property(t => t.Remark).HasColumnName("Remark");

			this.CustomMap();
		}
	}

}
