
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class MonitorPersonInfoMap :  BaseMap<MonitorPersonInfo>
    {
        public MonitorPersonInfoMap()
        {

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.OrgCode)
                .HasMaxLength(50);

            this.Property(t => t.IDCard)
                .HasMaxLength(50);

            this.Property(t => t.Tel)
                .HasMaxLength(50);

            this.Property(t => t.LaborContractFileId)
                .HasMaxLength(50);

            this.Property(t => t.CertificatePassingExaminationFileId)
                .HasMaxLength(50);

            this.Property(t => t.SocialSecurityContractFileId)
                .HasMaxLength(50);

            this.Property(t => t.IDCardFrontId)
                .HasMaxLength(50);

            this.Property(t => t.IDCardBackId)
                .HasMaxLength(50);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_MonitorPersonInfo");
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

            this.Property(t => t.Name).HasColumnName("Name");

            this.Property(t => t.OrgCode).HasColumnName("OrgCode");

            this.Property(t => t.IDCard).HasColumnName("IDCard");

            this.Property(t => t.Tel).HasColumnName("Tel");

            this.Property(t => t.LaborContractFileId).HasColumnName("LaborContractFileId");

            this.Property(t => t.CertificatePassingExaminationFileId).HasColumnName("CertificatePassingExaminationFileId");

            this.Property(t => t.SocialSecurityContractFileId).HasColumnName("SocialSecurityContractFileId");

            this.Property(t => t.IDCardFrontId).HasColumnName("IDCardFrontId");

            this.Property(t => t.IDCardBackId).HasColumnName("IDCardBackId");

			this.CustomMap();
		}
	}

}
