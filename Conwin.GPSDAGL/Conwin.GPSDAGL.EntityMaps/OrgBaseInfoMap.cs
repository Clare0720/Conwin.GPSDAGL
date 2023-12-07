
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class OrgBaseInfoMap :  BaseMap<OrgBaseInfo>
    {
        public OrgBaseInfoMap()
        {

            this.Property(t => t.OrgCode)
                .HasMaxLength(16);

            this.Property(t => t.OrgShortName)
                .HasMaxLength(50);

            this.Property(t => t.OrgName)
                .HasMaxLength(50);

            this.Property(t => t.XiaQuSheng)
                .HasMaxLength(30);

            this.Property(t => t.XiaQuShi)
                .HasMaxLength(30);

            this.Property(t => t.XiaQuXian)
                .HasMaxLength(30);

            this.Property(t => t.ChuangJianRenOrgCode)
                .HasMaxLength(16);

            this.Property(t => t.ZuiJinXiuGaiRenOrgCode)
                .HasMaxLength(16);

            this.Property(t => t.Street)
                .HasMaxLength(100);

            this.Property(t => t.YeWuJingYingFanWei)
                .HasMaxLength(2000);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_OrgBaseInfo");
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

            this.Property(t => t.OrgCode).HasColumnName("OrgCode");

            this.Property(t => t.OrgType).HasColumnName("OrgType");

            this.Property(t => t.OrgShortName).HasColumnName("OrgShortName");

            this.Property(t => t.OrgName).HasColumnName("OrgName");

            this.Property(t => t.ParentOrgId).HasColumnName("ParentOrgId");

            this.Property(t => t.JingYingFanWei).HasColumnName("JingYingFanWei");

            this.Property(t => t.XiaQuSheng).HasColumnName("XiaQuSheng");

            this.Property(t => t.XiaQuShi).HasColumnName("XiaQuShi");

            this.Property(t => t.XiaQuXian).HasColumnName("XiaQuXian");

            this.Property(t => t.DiZhi).HasColumnName("DiZhi");

            this.Property(t => t.ZhuangTai).HasColumnName("ZhuangTai");

            this.Property(t => t.ChuangJianRenOrgCode).HasColumnName("ChuangJianRenOrgCode");

            this.Property(t => t.ZuiJinXiuGaiRenOrgCode).HasColumnName("ZuiJinXiuGaiRenOrgCode");

            this.Property(t => t.Remark).HasColumnName("Remark");

            this.Property(t => t.Street).HasColumnName("Street");

            this.Property(t => t.YeWuJingYingFanWei).HasColumnName("YeWuJingYingFanWei");

			this.CustomMap();
		}
	}

}
