
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class CheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiMap :  BaseMap<CheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi>
    {
        public CheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXiMap()
        {


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_CheLiangGPSZhongDuanShuJuTongXunPeiZhiXinXi");
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

            this.Property(t => t.CheLiangID).HasColumnName("CheLiangID");

            this.Property(t => t.ZhongDuanID).HasColumnName("ZhongDuanID");

            this.Property(t => t.XieYiLeiXing).HasColumnName("XieYiLeiXing");

            this.Property(t => t.ZhuaBaoLaiYuan).HasColumnName("ZhuaBaoLaiYuan");

            this.Property(t => t.BanBenHao).HasColumnName("BanBenHao");

			this.CustomMap();
		}
	}

}
