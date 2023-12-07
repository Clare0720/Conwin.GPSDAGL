
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class YongHuJianKongShuMap :  BaseMap<YongHuJianKongShu>
    {
        public YongHuJianKongShuMap()
        {

            this.Property(t => t.NodeId)
                .HasMaxLength(255);

            this.Property(t => t.SysUserId)
                .HasMaxLength(255);

            this.Property(t => t.Remark)
                .HasMaxLength(500);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_YongHuJianKongShu");
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

            this.Property(t => t.NodeId).HasColumnName("NodeId");

            this.Property(t => t.SysUserId).HasColumnName("SysUserId");

            this.Property(t => t.Enabled).HasColumnName("Enabled");

            this.Property(t => t.Remark).HasColumnName("Remark");

			this.CustomMap();
		}
	}

}
