
using Conwin.GPSDAGL.Entities;
namespace Conwin.GPSDAGL.EntityMaps
{

public partial class DangYuanMap :  BaseMap<DangYuan>
    {
        public DangYuanMap()
        {

            this.Property(t => t.Name)
                .HasMaxLength(16);

            this.Property(t => t.IDCard)
                .HasMaxLength(20);

            this.Property(t => t.NativePlace)
                .HasMaxLength(50);

            this.Property(t => t.Degree)
                .HasMaxLength(16);

            this.Property(t => t.ContactNumber)
                .HasMaxLength(20);

            this.Property(t => t.OrgCode)
                .HasMaxLength(16);

            this.Property(t => t.Position)
                .HasMaxLength(16);

            this.Property(t => t.DangZuZhiSuoZaiDi)
                .HasMaxLength(50);

            this.Property(t => t.LiuDongDangYuan)
                .HasMaxLength(16);


			this.Map(m =>
			{
				m.MapInheritedProperties();
				m.ToTable("T_DangYuan");
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

            this.Property(t => t.Sex).HasColumnName("Sex");

            this.Property(t => t.IDCard).HasColumnName("IDCard");

            this.Property(t => t.NativePlace).HasColumnName("NativePlace");

            this.Property(t => t.Nation).HasColumnName("Nation");

            this.Property(t => t.Education).HasColumnName("Education");

            this.Property(t => t.Degree).HasColumnName("Degree");

            this.Property(t => t.ContactNumber).HasColumnName("ContactNumber");

            this.Property(t => t.OrgCode).HasColumnName("OrgCode");

            this.Property(t => t.Position).HasColumnName("Position");

            this.Property(t => t.EntryDate).HasColumnName("EntryDate");

            this.Property(t => t.DangZuZhiSuoZaiDi).HasColumnName("DangZuZhiSuoZaiDi");

            this.Property(t => t.LiuDongDangYuan).HasColumnName("LiuDongDangYuan");

			this.CustomMap();
		}
	}

}
