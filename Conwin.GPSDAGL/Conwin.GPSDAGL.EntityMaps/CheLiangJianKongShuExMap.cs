
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class CheLiangJianKongShuExMap :  BaseMap<CheLiangJianKongShuEx>
    {
        public CheLiangJianKongShuExMap()
        {

            this.Property(t => t.CheLiangId)
                .HasMaxLength(255);

            this.Property(t => t.ChePaiHao)
                .HasMaxLength(20);

            this.Property(t => t.ChePaiYanSe)
                .HasMaxLength(20);

            this.Property(t => t.NodeId)
                .HasMaxLength(255);

            this.Property(t => t.NodeName)
                .HasMaxLength(50);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_CheLiangJianKongShuEx");
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

            this.Property(t => t.ChePaiHao).HasColumnName("ChePaiHao");

            this.Property(t => t.ChePaiYanSe).HasColumnName("ChePaiYanSe");

            this.Property(t => t.NodeId).HasColumnName("NodeId");

            this.Property(t => t.NodeName).HasColumnName("NodeName");

			this.CustomMap();
		}
	}

}
