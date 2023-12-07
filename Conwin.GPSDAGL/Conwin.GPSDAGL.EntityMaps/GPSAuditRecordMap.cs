
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class GPSAuditRecordMap :  BaseMap<GPSAuditRecord>
    {
        public GPSAuditRecordMap()
        {

            this.Property(t => t.VehicleId)
                .HasMaxLength(255);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_GPSAuditRecord");
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

            this.Property(t => t.VehicleId).HasColumnName("VehicleId");

            this.Property(t => t.ResultComment).HasColumnName("ResultComment");

            this.Property(t => t.FiledComment).HasColumnName("FiledComment");

            this.Property(t => t.GPSAuditStatus).HasColumnName("GPSAuditStatus");

            this.Property(t => t.FiledDate).HasColumnName("FiledDate");

			this.CustomMap();
		}
	}

}
