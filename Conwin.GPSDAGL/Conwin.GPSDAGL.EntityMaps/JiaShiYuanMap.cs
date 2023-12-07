
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class JiaShiYuanMap :  BaseMap<JiaShiYuan>
    {
        public JiaShiYuanMap()
        {

            this.Property(t => t.Name)
                .HasMaxLength(32);

            this.Property(t => t.Cellphone)
                .HasMaxLength(20);

            this.Property(t => t.IDCard)
                .HasMaxLength(50);

            this.Property(t => t.OrgCode)
                .HasMaxLength(16);

            this.Property(t => t.CheLiangId)
                .HasMaxLength(50);

            this.Property(t => t.Certification)
                .HasMaxLength(50);

            this.Property(t => t.GuoJi)
                .HasMaxLength(50);

            this.Property(t => t.HuKouDiZhi)
                .HasMaxLength(50);

            this.Property(t => t.FaZhengJiGou)
                .HasMaxLength(255);

            this.Property(t => t.LianXiDiZhi)
                .HasMaxLength(255);

            this.Property(t => t.ShenFenZhengZhengMian)
                .HasMaxLength(255);

            this.Property(t => t.ShenFenZhengFanMian)
                .HasMaxLength(255);

            this.Property(t => t.JiaShiYuanZhengMian)
                .HasMaxLength(255);

            this.Property(t => t.ZhunJiaCheXing)
                .HasMaxLength(16);

            this.Property(t => t.JiaZhaoHaoMa)
                .HasMaxLength(50);

            this.Property(t => t.JiaZhaoBianHao)
                .HasMaxLength(50);

            this.Property(t => t.JiaShiZhengZhengMian)
                .HasMaxLength(255);

            this.Property(t => t.JiaShiZhengFanMian)
                .HasMaxLength(255);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_JiaShiYuan");
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

            this.Property(t => t.Name).HasColumnName("Name");

            this.Property(t => t.Cellphone).HasColumnName("Cellphone");

            this.Property(t => t.IDCardType).HasColumnName("IDCardType");

            this.Property(t => t.IDCard).HasColumnName("IDCard");

            this.Property(t => t.WorkingStatus).HasColumnName("WorkingStatus");

            this.Property(t => t.OrgCode).HasColumnName("OrgCode");

            this.Property(t => t.CheLiangId).HasColumnName("CheLiangId");

            this.Property(t => t.EntryDate).HasColumnName("EntryDate");

            this.Property(t => t.DismissalDate).HasColumnName("DismissalDate");

            this.Property(t => t.Certification).HasColumnName("Certification");

            this.Property(t => t.Sex).HasColumnName("Sex");

            this.Property(t => t.GuoJi).HasColumnName("GuoJi");

            this.Property(t => t.HuKouDiZhi).HasColumnName("HuKouDiZhi");

            this.Property(t => t.Birthday).HasColumnName("Birthday");

            this.Property(t => t.CertificationEndTime).HasColumnName("CertificationEndTime");

            this.Property(t => t.FaZhengJiGou).HasColumnName("FaZhengJiGou");

            this.Property(t => t.LianXiDiZhi).HasColumnName("LianXiDiZhi");

            this.Property(t => t.ShenFenZhengZhengMian).HasColumnName("ShenFenZhengZhengMian");

            this.Property(t => t.ShenFenZhengFanMian).HasColumnName("ShenFenZhengFanMian");

            this.Property(t => t.JiaShiYuanZhengMian).HasColumnName("JiaShiYuanZhengMian");

            this.Property(t => t.JiaZhaoChuCiShenLing).HasColumnName("JiaZhaoChuCiShenLing");

            this.Property(t => t.ZhunJiaCheXing).HasColumnName("ZhunJiaCheXing");

            this.Property(t => t.JiaZhaoHaoMa).HasColumnName("JiaZhaoHaoMa");

            this.Property(t => t.JiaZhaoBianHao).HasColumnName("JiaZhaoBianHao");

            this.Property(t => t.NianJianRiQi).HasColumnName("NianJianRiQi");

            this.Property(t => t.JiaZhaoYouXiaoQi).HasColumnName("JiaZhaoYouXiaoQi");

            this.Property(t => t.JiaShiZhengZhengMian).HasColumnName("JiaShiZhengZhengMian");

            this.Property(t => t.JiaShiZhengFanMian).HasColumnName("JiaShiZhengFanMian");

			this.CustomMap();
		}
	}

}
