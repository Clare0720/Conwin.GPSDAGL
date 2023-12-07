
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class CheLiangDingWeiXinXiMap :  BaseMap<CheLiangDingWeiXinXi>
    {
        public CheLiangDingWeiXinXiMap()
        {

            this.Property(t => t.RegistrationNo)
                .HasMaxLength(20);

            this.Property(t => t.RegistrationNoColor)
                .HasMaxLength(20);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_CheLiangDingWeiXinXi");
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

            this.Property(t => t.RegistrationNo).HasColumnName("RegistrationNo");

            this.Property(t => t.RegistrationNoColor).HasColumnName("RegistrationNoColor");

            this.Property(t => t.LatestGpsTime).HasColumnName("LatestGpsTime");

            this.Property(t => t.FirstUploadTime).HasColumnName("FirstUploadTime");

            this.Property(t => t.LatestRecvTime).HasColumnName("LatestRecvTime");

            this.Property(t => t.LatestLongtitude).HasColumnName("LatestLongtitude");

            this.Property(t => t.LatestLatitude).HasColumnName("LatestLatitude");

			this.CustomMap();
		}
	}

}
