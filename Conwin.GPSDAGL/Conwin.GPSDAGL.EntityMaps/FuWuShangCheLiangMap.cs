
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class FuWuShangCheLiangMap :  BaseMap<FuWuShangCheLiang>
    {
        public FuWuShangCheLiangMap()
        {

            this.Property(t => t.ChePaiHao)
                .HasMaxLength(16);

            this.Property(t => t.ChePaiYanSe)
                .HasMaxLength(16);

            this.Property(t => t.XiaQuSheng)
                .HasMaxLength(16);

            this.Property(t => t.XiaQuShi)
                .HasMaxLength(16);

            this.Property(t => t.XiaQuXian)
                .HasMaxLength(16);

            this.Property(t => t.YeHuOrgCode)
                .HasMaxLength(16);

            this.Property(t => t.FuWuShangOrgCode)
                .HasMaxLength(16);

            this.Property(t => t.Remark)
                .HasMaxLength(255);

            this.Property(t => t.ChuangJianRenOrgCode)
                .HasMaxLength(16);

            this.Property(t => t.ZuiJinXiuGaiRenOrgCode)
                .HasMaxLength(16);

            this.Property(t => t.CheJiaHao)
                .HasMaxLength(24);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_FuWuShangCheLiang");
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

            this.Property(t => t.ChePaiHao).HasColumnName("ChePaiHao");

            this.Property(t => t.ChePaiYanSe).HasColumnName("ChePaiYanSe");

            this.Property(t => t.CheLiangZhongLei).HasColumnName("CheLiangZhongLei");

            this.Property(t => t.XiaQuSheng).HasColumnName("XiaQuSheng");

            this.Property(t => t.XiaQuShi).HasColumnName("XiaQuShi");

            this.Property(t => t.XiaQuXian).HasColumnName("XiaQuXian");

            this.Property(t => t.YeHuOrgCode).HasColumnName("YeHuOrgCode");

            this.Property(t => t.YeHuOrgType).HasColumnName("YeHuOrgType");

            this.Property(t => t.FuWuShangOrgCode).HasColumnName("FuWuShangOrgCode");

            this.Property(t => t.Remark).HasColumnName("Remark");

            this.Property(t => t.BeiAnZhuangTai).HasColumnName("BeiAnZhuangTai");

            this.Property(t => t.ChuangJianRenOrgCode).HasColumnName("ChuangJianRenOrgCode");

            this.Property(t => t.ZuiJinXiuGaiRenOrgCode).HasColumnName("ZuiJinXiuGaiRenOrgCode");

            this.Property(t => t.CheJiaHao).HasColumnName("CheJiaHao");

			this.CustomMap();
		}
	}

}
