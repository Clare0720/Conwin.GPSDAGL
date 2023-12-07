
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class ZuZhiGongZhangXinXiMap :  BaseMap<ZuZhiGongZhangXinXi>
    {
        public ZuZhiGongZhangXinXiMap()
        {

            this.Property(t => t.ChuangJianDanWeiOrgCode)
                .HasMaxLength(50);

            this.Property(t => t.OrgName)
                .HasMaxLength(200);

            this.Property(t => t.OrgCode)
                .HasMaxLength(50);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_ZuZhiGongZhangXinXi");
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

            this.Property(t => t.ChuangJianDanWeiOrgCode).HasColumnName("ChuangJianDanWeiOrgCode");

            this.Property(t => t.OrgName).HasColumnName("OrgName");

            this.Property(t => t.OrgCode).HasColumnName("OrgCode");

            this.Property(t => t.GongZhangZhaoPianId).HasColumnName("GongZhangZhaoPianId");

			this.CustomMap();
		}
	}

}
