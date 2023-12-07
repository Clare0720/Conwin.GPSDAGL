
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class MaterialListOfServiceProviderMap :  BaseMap<MaterialListOfServiceProvider>
    {
        public MaterialListOfServiceProviderMap()
        {

            this.Property(t => t.IndustrialACBLicenseId)
                .HasMaxLength(255);

            this.Property(t => t.IndustrialACBLOTOfficeId)
                .HasMaxLength(255);

            this.Property(t => t.FilingApplicationFormId)
                .HasMaxLength(255);

            this.Property(t => t.StandardTOServiceId)
                .HasMaxLength(255);

            this.Property(t => t.SiteMaterialsId)
                .HasMaxLength(255);

            this.Property(t => t.PhotosOBPremisesId)
                .HasMaxLength(255);

            this.Property(t => t.PersonnelPDMaterialsId)
                .HasMaxLength(255);

            this.Property(t => t.PersonnelSSCertificateId)
                .HasMaxLength(255);

            this.Property(t => t.EquipmentIMaterialsId)
                .HasMaxLength(255);

            this.Property(t => t.SupervisorMaterialsId)
                .HasMaxLength(255);

            this.Property(t => t.ViolationsASMaterialsId)
                .HasMaxLength(255);

            this.Property(t => t.MonitoringSystemMaterialsId)
                .HasMaxLength(255);

            this.Property(t => t.SafetyGradeMaterialsId)
                .HasMaxLength(255);

            this.Property(t => t.Landline)
                .HasMaxLength(255);

            this.Property(t => t.Mailbox)
                .HasMaxLength(255);

            this.Property(t => t.IDCard)
                .HasMaxLength(255);

            this.Property(t => t.ApprovalRemark)
                .HasMaxLength(255);

            this.Property(t => t.OrgCode)
                .HasMaxLength(255);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_MaterialListOfServiceProvider");
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

            this.Property(t => t.IndustrialACBLicenseId).HasColumnName("IndustrialACBLicenseId");

            this.Property(t => t.IndustrialACBLOTOfficeId).HasColumnName("IndustrialACBLOTOfficeId");

            this.Property(t => t.FilingApplicationFormId).HasColumnName("FilingApplicationFormId");

            this.Property(t => t.StandardTOServiceId).HasColumnName("StandardTOServiceId");

            this.Property(t => t.SiteMaterialsId).HasColumnName("SiteMaterialsId");

            this.Property(t => t.PhotosOBPremisesId).HasColumnName("PhotosOBPremisesId");

            this.Property(t => t.PersonnelPDMaterialsId).HasColumnName("PersonnelPDMaterialsId");

            this.Property(t => t.PersonnelSSCertificateId).HasColumnName("PersonnelSSCertificateId");

            this.Property(t => t.EquipmentIMaterialsId).HasColumnName("EquipmentIMaterialsId");

            this.Property(t => t.SupervisorMaterialsId).HasColumnName("SupervisorMaterialsId");

            this.Property(t => t.ViolationsASMaterialsId).HasColumnName("ViolationsASMaterialsId");

            this.Property(t => t.MonitoringSystemMaterialsId).HasColumnName("MonitoringSystemMaterialsId");

            this.Property(t => t.SafetyGradeMaterialsId).HasColumnName("SafetyGradeMaterialsId");

            this.Property(t => t.Landline).HasColumnName("Landline");

            this.Property(t => t.Mailbox).HasColumnName("Mailbox");

            this.Property(t => t.IDCard).HasColumnName("IDCard");

            this.Property(t => t.ApprovalRemark).HasColumnName("ApprovalRemark");

            this.Property(t => t.ApprovalStatus).HasColumnName("ApprovalStatus");

            this.Property(t => t.CompanyType).HasColumnName("CompanyType");

            this.Property(t => t.OrgCode).HasColumnName("OrgCode");

			this.CustomMap();
		}
	}

}
