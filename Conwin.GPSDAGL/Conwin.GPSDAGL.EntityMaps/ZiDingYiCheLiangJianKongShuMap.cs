
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class ZiDingYiCheLiangJianKongShuMap :  BaseMap<ZiDingYiCheLiangJianKongShu>
    {
        public ZiDingYiCheLiangJianKongShuMap()
        {

            this.Property(t => t.NodeName)
                .HasMaxLength(20);

            this.Property(t => t.ChuangJianRenOrgCode)
                .HasMaxLength(50);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_ZiDingYiCheLiangJianKongShu");
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

            this.Property(t => t.NodeName).HasColumnName("NodeName");

            this.Property(t => t.ParentNodeId).HasColumnName("ParentNodeId");

            this.Property(t => t.Order).HasColumnName("Order");

            this.Property(t => t.IsEnabled).HasColumnName("IsEnabled");

            this.Property(t => t.NodeType).HasColumnName("NodeType");

            this.Property(t => t.ChuangJianRenOrgCode).HasColumnName("ChuangJianRenOrgCode");

            this.Property(t => t.Remark).HasColumnName("Remark");

            this.Property(t => t.NodeData).HasColumnName("NodeData");

			this.CustomMap();
		}
	}

}
