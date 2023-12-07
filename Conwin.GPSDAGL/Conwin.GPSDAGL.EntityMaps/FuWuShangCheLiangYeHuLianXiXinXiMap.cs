
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class FuWuShangCheLiangYeHuLianXiXinXiMap :  BaseMap<FuWuShangCheLiangYeHuLianXiXinXi>
    {
        public FuWuShangCheLiangYeHuLianXiXinXiMap()
        {

            this.Property(t => t.YeHuPrincipalName)
                .HasMaxLength(50);

            this.Property(t => t.YeHuPrincipalPhone)
                .HasMaxLength(30);

            this.Property(t => t.DriverName)
                .HasMaxLength(50);

            this.Property(t => t.DriverPhone)
                .HasMaxLength(30);

            this.Property(t => t.CongYeZiGeZhengHao)
                .HasMaxLength(50);

            this.Property(t => t.JiZhongAnZhuangDianMingCheng)
                .HasMaxLength(100);

            this.Property(t => t.SheBeiAnZhuangRenYuanXingMing)
                .HasMaxLength(50);

            this.Property(t => t.SheBeiAnZhuangDanWei)
                .HasMaxLength(100);

            this.Property(t => t.SheBeiAnZhuangRenYuanDianHua)
                .HasMaxLength(30);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_FuWuShangCheLiangYeHuLianXiXinXi");
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

            this.Property(t => t.CheLiangId).HasColumnName("CheLiangId");

            this.Property(t => t.YeHuPrincipalName).HasColumnName("YeHuPrincipalName");

            this.Property(t => t.YeHuPrincipalPhone).HasColumnName("YeHuPrincipalPhone");

            this.Property(t => t.DriverName).HasColumnName("DriverName");

            this.Property(t => t.DriverPhone).HasColumnName("DriverPhone");

            this.Property(t => t.CongYeZiGeZhengHao).HasColumnName("CongYeZiGeZhengHao");

            this.Property(t => t.JiZhongAnZhuangDianMingCheng).HasColumnName("JiZhongAnZhuangDianMingCheng");

            this.Property(t => t.SheBeiAnZhuangRenYuanXingMing).HasColumnName("SheBeiAnZhuangRenYuanXingMing");

            this.Property(t => t.SheBeiAnZhuangDanWei).HasColumnName("SheBeiAnZhuangDanWei");

            this.Property(t => t.SheBeiAnZhuangRenYuanDianHua).HasColumnName("SheBeiAnZhuangRenYuanDianHua");

			this.CustomMap();
		}
	}

}
