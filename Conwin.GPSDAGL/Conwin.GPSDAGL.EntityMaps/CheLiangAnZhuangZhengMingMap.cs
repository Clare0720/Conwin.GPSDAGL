
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class CheLiangAnZhuangZhengMingMap :  BaseMap<CheLiangAnZhuangZhengMing>
    {
        public CheLiangAnZhuangZhengMingMap()
        {

            this.Property(t => t.ZhengMingBianHao)
                .HasMaxLength(20);

            this.Property(t => t.ShuiYinPDFFileId)
                .HasMaxLength(255);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_CheLiangAnZhuangZhengMing");
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

            this.Property(t => t.ZhengMingBianHao).HasColumnName("ZhengMingBianHao");

            this.Property(t => t.GongZhangId).HasColumnName("GongZhangId");

            this.Property(t => t.ZhengMingLeiXin).HasColumnName("ZhengMingLeiXin");

            this.Property(t => t.ZhengMingFileId).HasColumnName("ZhengMingFileId");

            this.Property(t => t.ShuiYinPDFFileId).HasColumnName("ShuiYinPDFFileId");

			this.CustomMap();
		}
	}

}
