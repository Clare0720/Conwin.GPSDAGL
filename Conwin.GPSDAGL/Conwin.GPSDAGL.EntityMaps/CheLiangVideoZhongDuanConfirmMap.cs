
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class CheLiangVideoZhongDuanConfirmMap :  BaseMap<CheLiangVideoZhongDuanConfirm>
    {
        public CheLiangVideoZhongDuanConfirmMap()
        {

            this.Property(t => t.CheLiangId)
                .HasMaxLength(255);

            this.Property(t => t.ZhongDuanId)
                .HasMaxLength(255);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_CheLiangVideoZhongDuanConfirm");
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

            this.Property(t => t.CheLiangId).HasColumnName("CheLiangId");

            this.Property(t => t.ZhongDuanId).HasColumnName("ZhongDuanId");

            this.Property(t => t.ShuJuJieRu).HasColumnName("ShuJuJieRu");

            this.Property(t => t.SheBeiWanZheng).HasColumnName("SheBeiWanZheng");

            this.Property(t => t.NeiRong).HasColumnName("NeiRong");

            this.Property(t => t.BeiAnZhuangTai).HasColumnName("BeiAnZhuangTai");

            this.Property(t => t.TiJiaoBeiAnShiJian).HasColumnName("TiJiaoBeiAnShiJian");

            this.Property(t => t.ZuiJinShenHeShiJian).HasColumnName("ZuiJinShenHeShiJian");

			this.CustomMap();
		}
	}

}
