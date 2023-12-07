
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class FuWuShangCheLiangVideoZhongDuanXinXiMap :  BaseMap<FuWuShangCheLiangVideoZhongDuanXinXi>
    {
        public FuWuShangCheLiangVideoZhongDuanXinXiMap()
        {

            this.Property(t => t.FuWuShangCheLiangId)
                .HasMaxLength(255);

            this.Property(t => t.ZhongDuanMDT)
                .HasMaxLength(20);

            this.Property(t => t.SheBeiXingHao)
                .HasMaxLength(20);

            this.Property(t => t.SheBeiGouCheng)
                .HasMaxLength(128);

            this.Property(t => t.ChangJiaBianHao)
                .HasMaxLength(20);

            this.Property(t => t.ShengChanChangJia)
                .HasMaxLength(50);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_FuWuShangCheLiangVideoZhongDuanXinXi");
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

            this.Property(t => t.FuWuShangCheLiangId).HasColumnName("FuWuShangCheLiangId");

            this.Property(t => t.ZhongDuanMDT).HasColumnName("ZhongDuanMDT");

            this.Property(t => t.SheBeiXingHao).HasColumnName("SheBeiXingHao");

            this.Property(t => t.SheBeiJiShenLeiXing).HasColumnName("SheBeiJiShenLeiXing");

            this.Property(t => t.SheBeiGouCheng).HasColumnName("SheBeiGouCheng");

            this.Property(t => t.ChangJiaBianHao).HasColumnName("ChangJiaBianHao");

            this.Property(t => t.ShengChanChangJia).HasColumnName("ShengChanChangJia");

            this.Property(t => t.AnZhuangShiJian).HasColumnName("AnZhuangShiJian");

			this.CustomMap();
		}
	}

}
