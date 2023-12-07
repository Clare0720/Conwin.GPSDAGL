
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class CheLiangExMap :  BaseMap<CheLiangEx>
    {
        public CheLiangExMap()
        {

            this.Property(t => t.CheLiangId)
                .HasMaxLength(255);

            this.Property(t => t.JingYingFanWei)
                .HasMaxLength(255);

            this.Property(t => t.CheLiangNengLi)
                .HasMaxLength(50);

            this.Property(t => t.XingShiZhengHao)
                .HasMaxLength(50);

            this.Property(t => t.XingShiZhengDiZhi)
                .HasMaxLength(255);

            this.Property(t => t.CheLiangBaoXiaoZhongLei)
                .HasMaxLength(16);

            this.Property(t => t.XingShiZHengSaoMiaoJianId)
                .HasMaxLength(36);

            this.Property(t => t.PaiQiLiang)
                .HasMaxLength(50);

            this.Property(t => t.ZongZhiLiang)
                .HasMaxLength(50);

            this.Property(t => t.FaDongJiHao)
                .HasMaxLength(24);

            this.Property(t => t.XingHao)
                .HasMaxLength(50);

            this.Property(t => t.JiShuDengJi)
                .HasMaxLength(20);

            this.Property(t => t.AnZhuangDengJi)
                .HasMaxLength(20);

            this.Property(t => t.DaoLuYunShuZhengHao)
                .HasMaxLength(20);

            this.Property(t => t.ZuoXing)
                .HasMaxLength(20);

            this.Property(t => t.JieBoCheLiang)
                .HasMaxLength(20);

            this.Property(t => t.HuoWuMingCheng)
                .HasMaxLength(20);

            this.Property(t => t.ShiFaDi)
                .HasMaxLength(50);

            this.Property(t => t.QiFaDi)
                .HasMaxLength(50);

            this.Property(t => t.ShiFaZhan)
                .HasMaxLength(50);

            this.Property(t => t.QiDianZhan)
                .HasMaxLength(50);

            this.Property(t => t.ChuangJianRenOrgCode)
                .HasMaxLength(50);

            this.Property(t => t.ZuiJinXiuGaiRenOrgCode)
                .HasMaxLength(50);

            this.Property(t => t.CheLiangPinPai)
                .HasMaxLength(50);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_CheLiangEx");
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

            this.Property(t => t.JingYingFanWei).HasColumnName("JingYingFanWei");

            this.Property(t => t.CheLiangBiaoZhiId).HasColumnName("CheLiangBiaoZhiId");

            this.Property(t => t.CheShenYanSe).HasColumnName("CheShenYanSe");

            this.Property(t => t.CheLiangNengLi).HasColumnName("CheLiangNengLi");

            this.Property(t => t.XingShiZhengHao).HasColumnName("XingShiZhengHao");

            this.Property(t => t.XingShiZhengDiZhi).HasColumnName("XingShiZhengDiZhi");

            this.Property(t => t.XingShiZhengYouXiaoQi).HasColumnName("XingShiZhengYouXiaoQi");

            this.Property(t => t.XingShiZhengNianShenRiQi).HasColumnName("XingShiZhengNianShenRiQi");

            this.Property(t => t.CheLiangBaoXiaoZhongLei).HasColumnName("CheLiangBaoXiaoZhongLei");

            this.Property(t => t.CheLiangBaoXiaoDaoJieZhiRiQi).HasColumnName("CheLiangBaoXiaoDaoJieZhiRiQi");

            this.Property(t => t.XingShiZhengTiXingTianShu).HasColumnName("XingShiZhengTiXingTianShu");

            this.Property(t => t.XingShiZhengDengJiRiQi).HasColumnName("XingShiZhengDengJiRiQi");

            this.Property(t => t.XingShiZHengSaoMiaoJianId).HasColumnName("XingShiZHengSaoMiaoJianId");

            this.Property(t => t.RanLiao).HasColumnName("RanLiao");

            this.Property(t => t.PaiQiLiang).HasColumnName("PaiQiLiang");

            this.Property(t => t.ZongZhiLiang).HasColumnName("ZongZhiLiang");

            this.Property(t => t.ZhengBeiZhiLiang).HasColumnName("ZhengBeiZhiLiang");

            this.Property(t => t.HeZaiZhiLiang).HasColumnName("HeZaiZhiLiang");

            this.Property(t => t.FaDongJiHao).HasColumnName("FaDongJiHao");

            this.Property(t => t.CheGao).HasColumnName("CheGao");

            this.Property(t => t.CheChang).HasColumnName("CheChang");

            this.Property(t => t.CheKuan).HasColumnName("CheKuan");

            this.Property(t => t.ZhuangTai).HasColumnName("ZhuangTai");

            this.Property(t => t.XingHao).HasColumnName("XingHao");

            this.Property(t => t.JiShuDengJi).HasColumnName("JiShuDengJi");

            this.Property(t => t.AnZhuangDengJi).HasColumnName("AnZhuangDengJi");

            this.Property(t => t.ErWeiRiQi).HasColumnName("ErWeiRiQi");

            this.Property(t => t.XiaCiErWeiRiQi).HasColumnName("XiaCiErWeiRiQi");

            this.Property(t => t.ShenYanYouXiaoQi).HasColumnName("ShenYanYouXiaoQi");

            this.Property(t => t.DaoLuYunShuZhengHao).HasColumnName("DaoLuYunShuZhengHao");

            this.Property(t => t.DaoLuYunShuZhengYouXiaoQi).HasColumnName("DaoLuYunShuZhengYouXiaoQi");

            this.Property(t => t.DaoLuYunShuZhengNianShenRiQi).HasColumnName("DaoLuYunShuZhengNianShenRiQi");

            this.Property(t => t.DaoLuYunShuZhengTiXingTianShu).HasColumnName("DaoLuYunShuZhengTiXingTianShu");

            this.Property(t => t.ZuoXing).HasColumnName("ZuoXing");

            this.Property(t => t.ZuoWei).HasColumnName("ZuoWei");

            this.Property(t => t.DunWei).HasColumnName("DunWei");

            this.Property(t => t.ChuChangRiQi).HasColumnName("ChuChangRiQi");

            this.Property(t => t.CheZhouShu).HasColumnName("CheZhouShu");

            this.Property(t => t.JieBoCheLiang).HasColumnName("JieBoCheLiang");

            this.Property(t => t.HuoWuMingCheng).HasColumnName("HuoWuMingCheng");

            this.Property(t => t.ShiFaDi).HasColumnName("ShiFaDi");

            this.Property(t => t.QiFaDi).HasColumnName("QiFaDi");

            this.Property(t => t.ShiFaZhan).HasColumnName("ShiFaZhan");

            this.Property(t => t.QiDianZhan).HasColumnName("QiDianZhan");

            this.Property(t => t.HuoWuDunWei).HasColumnName("HuoWuDunWei");

            this.Property(t => t.ChuangJianRenOrgCode).HasColumnName("ChuangJianRenOrgCode");

            this.Property(t => t.ZuiJinXiuGaiRenOrgCode).HasColumnName("ZuiJinXiuGaiRenOrgCode");

            this.Property(t => t.ZhouShu).HasColumnName("ZhouShu");

            this.Property(t => t.ZhunQianYinZongZhiLiang).HasColumnName("ZhunQianYinZongZhiLiang");

            this.Property(t => t.CheLiangPinPai).HasColumnName("CheLiangPinPai");

			this.CustomMap();
		}
	}

}
