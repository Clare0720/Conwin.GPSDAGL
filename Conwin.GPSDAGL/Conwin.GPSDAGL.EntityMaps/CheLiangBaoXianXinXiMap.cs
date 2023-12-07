
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class CheLiangBaoXianXinXiMap :  BaseMap<CheLiangBaoXianXinXi>
    {
        public CheLiangBaoXianXinXiMap()
        {

            this.Property(t => t.JiaoQiangXianOrgName)
                .HasMaxLength(100);

            this.Property(t => t.ShangYeXianOrgName)
                .HasMaxLength(100);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_CheLiangBaoXianXinXi");
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

            this.Property(t => t.JiaoQiangXianOrgName).HasColumnName("JiaoQiangXianOrgName");

            this.Property(t => t.JiaoQiangXianEndTime).HasColumnName("JiaoQiangXianEndTime");

            this.Property(t => t.ShangYeXianOrgName).HasColumnName("ShangYeXianOrgName");

            this.Property(t => t.ShangYeXianEndTime).HasColumnName("ShangYeXianEndTime");

			this.CustomMap();
		}
	}

}
