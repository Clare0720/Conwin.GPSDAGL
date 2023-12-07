
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class YongHuCheLiangXinXiMap :  BaseMap<YongHuCheLiangXinXi>
    {
        public YongHuCheLiangXinXiMap()
        {

            this.Property(t => t.SysUserId)
                .HasMaxLength(255);

            this.Property(t => t.CheLiangId)
                .HasMaxLength(255);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_YongHuCheLiangXinXi");
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

            this.Property(t => t.SysUserId).HasColumnName("SysUserId");

            this.Property(t => t.CheLiangId).HasColumnName("CheLiangId");

			this.CustomMap();
		}
	}

}
