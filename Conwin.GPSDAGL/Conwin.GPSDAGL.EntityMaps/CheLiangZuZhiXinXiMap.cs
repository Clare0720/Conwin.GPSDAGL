
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class CheLiangZuZhiXinXiMap :  BaseMap<CheLiangZuZhiXinXi>
    {
        public CheLiangZuZhiXinXiMap()
        {

            this.Property(t => t.CheLiangId)
                .HasMaxLength(255);

            this.Property(t => t.OrgCode)
                .HasMaxLength(16);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_CheLiangZuZhiXinXi");
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

            this.Property(t => t.OrgCode).HasColumnName("OrgCode");

            this.Property(t => t.OrgType).HasColumnName("OrgType");

			this.CustomMap();
		}
	}

}
