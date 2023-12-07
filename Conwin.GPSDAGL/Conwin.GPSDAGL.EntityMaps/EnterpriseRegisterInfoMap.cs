
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class EnterpriseRegisterInfoMap :  BaseMap<EnterpriseRegisterInfo>
    {
        public EnterpriseRegisterInfoMap()
        {

            this.Property(t => t.OrgCode)
                .HasMaxLength(50);

            this.Property(t => t.ContactName)
                .HasMaxLength(50);

            this.Property(t => t.ContactIDCard)
                .HasMaxLength(50);

            this.Property(t => t.ContactTel)
                .HasMaxLength(50);

            this.Property(t => t.ContactEMail)
                .HasMaxLength(50);

            this.Property(t => t.PrincipalName)
                .HasMaxLength(50);

            this.Property(t => t.PrincipalIDCard)
                .HasMaxLength(50);

            this.Property(t => t.PrincipalTel)
                .HasMaxLength(50);

            this.Property(t => t.UniformSocialCreditCode)
                .HasMaxLength(50);

            this.Property(t => t.BusinessLicenseFileId)
                .HasMaxLength(50);

            this.Property(t => t.BusinessPermitNumber)
                .HasMaxLength(50);

            this.Property(t => t.BusinessPermitIssuingUnit)
                .HasMaxLength(150);

            this.Property(t => t.BusinessPermitPhotoFIleId)
                .HasMaxLength(50);

            this.Property(t => t.ApprovalRemark)
                .HasMaxLength(50);

            this.Property(t => t.ContactIDCardFrontId)
                .HasMaxLength(50);

            this.Property(t => t.ContactIDCardBackId)
                .HasMaxLength(50);

            this.Property(t => t.PrincipalIDCardFrontId)
                .HasMaxLength(50);

            this.Property(t => t.PrincipalIDCardBackId)
                .HasMaxLength(50);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_EnterpriseRegisterInfo");
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

            this.Property(t => t.ContactName).HasColumnName("ContactName");

            this.Property(t => t.ContactIDCard).HasColumnName("ContactIDCard");

            this.Property(t => t.ContactTel).HasColumnName("ContactTel");

            this.Property(t => t.ContactEMail).HasColumnName("ContactEMail");

            this.Property(t => t.PrincipalName).HasColumnName("PrincipalName");

            this.Property(t => t.PrincipalIDCard).HasColumnName("PrincipalIDCard");

            this.Property(t => t.PrincipalTel).HasColumnName("PrincipalTel");

            this.Property(t => t.UniformSocialCreditCode).HasColumnName("UniformSocialCreditCode");

            this.Property(t => t.BusinessLicenseFileId).HasColumnName("BusinessLicenseFileId");

            this.Property(t => t.BusinessPermitNumber).HasColumnName("BusinessPermitNumber");

            this.Property(t => t.BusinessPermitStartDateTime).HasColumnName("BusinessPermitStartDateTime");

            this.Property(t => t.BusinessPermitEndDateTime).HasColumnName("BusinessPermitEndDateTime");

            this.Property(t => t.BusinessPermitIssuingUnit).HasColumnName("BusinessPermitIssuingUnit");

            this.Property(t => t.BusinessPermitPhotoFIleId).HasColumnName("BusinessPermitPhotoFIleId");

            this.Property(t => t.ApprovalStatus).HasColumnName("ApprovalStatus");

            this.Property(t => t.ApprovalRemark).HasColumnName("ApprovalRemark");

            this.Property(t => t.EnterpriseType).HasColumnName("EnterpriseType");

            this.Property(t => t.MonitorType).HasColumnName("MonitorType");

            this.Property(t => t.ContactIDCardFrontId).HasColumnName("ContactIDCardFrontId");

            this.Property(t => t.ContactIDCardBackId).HasColumnName("ContactIDCardBackId");

            this.Property(t => t.PrincipalIDCardFrontId).HasColumnName("PrincipalIDCardFrontId");

            this.Property(t => t.PrincipalIDCardBackId).HasColumnName("PrincipalIDCardBackId");

			this.CustomMap();
		}
	}

}
