
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class VehiclePartnershipBindingMap :  BaseMap<VehiclePartnershipBinding>
    {
        public VehiclePartnershipBindingMap()
        {

            this.Property(t => t.EnterpriseCode)
                .HasMaxLength(255);

            this.Property(t => t.ServiceProviderCode)
                .HasMaxLength(255);

            this.Property(t => t.LicensePlateNumber)
                .HasMaxLength(255);

            this.Property(t => t.LicensePlateColor)
                .HasMaxLength(255);

            this.Property(t => t.Remarks)
                .HasMaxLength(255);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_VehiclePartnershipBinding");
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

            this.Property(t => t.EnterpriseCode).HasColumnName("EnterpriseCode");

            this.Property(t => t.ServiceProviderCode).HasColumnName("ServiceProviderCode");

            this.Property(t => t.ZhuangTai).HasColumnName("ZhuangTai");

            this.Property(t => t.LicensePlateNumber).HasColumnName("LicensePlateNumber");

            this.Property(t => t.LicensePlateColor).HasColumnName("LicensePlateColor");

            this.Property(t => t.Remarks).HasColumnName("Remarks");

			this.CustomMap();
		}
	}

}
