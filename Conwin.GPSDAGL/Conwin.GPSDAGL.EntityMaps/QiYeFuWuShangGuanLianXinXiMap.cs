
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class QiYeFuWuShangGuanLianXinXiMap :  BaseMap<QiYeFuWuShangGuanLianXinXi>
    {
        public QiYeFuWuShangGuanLianXinXiMap()
        {

            this.Property(t => t.QiYeCode)
                .HasMaxLength(50);

            this.Property(t => t.FuWuShangCode)
                .HasMaxLength(50);

            this.Property(t => t.ZhuLianLuIP)
                .HasMaxLength(20);

            this.Property(t => t.CongLianLuIP)
                .HasMaxLength(20);

            this.Property(t => t.XiaQuSheng)
                .HasMaxLength(20);

            this.Property(t => t.PingTaiJieRuMa)
                .HasMaxLength(50);

            this.Property(t => t.LoginName)
                .HasMaxLength(50);

            this.Property(t => t.LoginPassWord)
                .HasMaxLength(50);

            this.Property(t => t.XiaQuShi)
                .HasMaxLength(20);

            this.Property(t => t.XiaQuXian)
                .HasMaxLength(20);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_QiYeFuWuShangGuanLianXinXi");
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

            this.Property(t => t.QiYeCode).HasColumnName("QiYeCode");

            this.Property(t => t.FuWuShangCode).HasColumnName("FuWuShangCode");

            this.Property(t => t.ZhuLianLuIP).HasColumnName("ZhuLianLuIP");

            this.Property(t => t.ZhuLianLuDuanKou).HasColumnName("ZhuLianLuDuanKou");

            this.Property(t => t.CongLianLuIP).HasColumnName("CongLianLuIP");

            this.Property(t => t.CongLianLuDuanKou).HasColumnName("CongLianLuDuanKou");

            this.Property(t => t.XiaQuSheng).HasColumnName("XiaQuSheng");

            this.Property(t => t.PingTaiJieRuMa).HasColumnName("PingTaiJieRuMa");

            this.Property(t => t.LoginName).HasColumnName("LoginName");

            this.Property(t => t.LoginPassWord).HasColumnName("LoginPassWord");

            this.Property(t => t.M1).HasColumnName("M1");

            this.Property(t => t.IA1).HasColumnName("IA1");

            this.Property(t => t.IC1).HasColumnName("IC1");

            this.Property(t => t.ZhuangTai).HasColumnName("ZhuangTai");

            this.Property(t => t.XiaQuShi).HasColumnName("XiaQuShi");

            this.Property(t => t.XiaQuXian).HasColumnName("XiaQuXian");

			this.CustomMap();
		}
	}

}
