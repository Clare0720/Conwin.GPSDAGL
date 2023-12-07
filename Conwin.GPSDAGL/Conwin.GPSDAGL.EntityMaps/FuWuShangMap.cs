
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class FuWuShangMap :  BaseMap<FuWuShang>
    {
        public FuWuShangMap()
        {

            this.Property(t => t.BaseId)
                .HasMaxLength(255);

            this.Property(t => t.OrgCode)
                .HasMaxLength(16);

            this.Property(t => t.OrgShortName)
                .HasMaxLength(50);

            this.Property(t => t.OrgName)
                .HasMaxLength(50);

            this.Property(t => t.YingYeZhiZhaoHao)
                .HasMaxLength(20);

            this.Property(t => t.TongYiSheHuiXinYongDaiMa)
                .HasMaxLength(20);

            this.Property(t => t.YouBian)
                .HasMaxLength(10);

            this.Property(t => t.LianXiRenName)
                .HasMaxLength(50);

            this.Property(t => t.LianXiRenPhone)
                .HasMaxLength(50);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_FuWuShang");
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

            this.Property(t => t.YingYeZhiZhaoHao).HasColumnName("YingYeZhiZhaoHao");

            this.Property(t => t.TongYiSheHuiXinYongDaiMa).HasColumnName("TongYiSheHuiXinYongDaiMa");

            this.Property(t => t.YouBian).HasColumnName("YouBian");

            this.Property(t => t.LianXiRenName).HasColumnName("LianXiRenName");

            this.Property(t => t.LianXiRenPhone).HasColumnName("LianXiRenPhone");

			this.CustomMap();
		}
	}

}
