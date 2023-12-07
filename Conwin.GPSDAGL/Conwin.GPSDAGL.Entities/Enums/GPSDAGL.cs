using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Conwin.GPSDAGL.Entities.Enums
{
    public enum XiTongZhuangTaiEnum
    {
        正常 = 0, 作废 = 1
    }
    public enum ZhuangTaiEnum
    {
        正常营业 = 1, 合约到期 = 2
    }
    public enum ZhuangTaiExEnum
    {
        启用 = 1, 禁用 = 2
    }

    public enum BeiAnZhuangTai
    {
        已备案 = 1, 未备案 = 2,
    }

    public enum ZhongDuanBeiAnZhuangTai
    {
        不通过备案 = 0,
        通过备案 = 1,
        未审核 = 2,
        取消备案 = 3,
        待提交 = 4
    }

    public enum ManualApprovalStatus
    {
        不通过 = 0,
        通过 = 1
    }

    public enum SelectThirdParty
    {
        否 = 0,
        是 = 1
    }

    public enum RegistrationStatus
    {
        不通过 = 0,
        已提交审核 = 1,
        通过 = 2
    }
    public enum FuRegistrationStatus
    {
        市级审核中 = 0,
        市级审核通过 = 1,
        市级审核驳回 = 2
    }
    public enum JingJiLeiXing
    {
        国有企业 = 1, 民营企业 = 2, 外资企业 = 3, 中外合资 = 4, 其他
    }

    public enum GongSiLeiXing
    {
        总公司 = 1, 分公司 = 2, 子公司 = 3
    }

    public enum ShenHeZhuangTai
    {
        待提交 = 1, 待审核 = 2, 审核通过 = 3, 审核不通过 = 4
    }

    public enum CheLiangZhongLeiEnum
    {
        客运班车 = 1,
        旅游包车 = 2,
        危险货运 = 3,
        重型货车 = 4,
        公交客运 = 5,
        出租客运 = 6,
        教练员车 = 7,
        普通货运 = 8,
        其它车辆 = 9,
        校车 = 10,
        网约车 = 11,
    }

    /// <summary>
    /// 车辆类型
    /// </summary>
    public enum CheLiangLeiXing
    {
        其他 = 0,
        重型货车 = 1,
        大型货车 = 2,
        中型货车 = 3,
        小型货车 = 4,
        特大型客车 = 5,
        大型客车 = 6,
        中型客车 = 7,
        小型客车 = 8,
        特大型卧铺 = 9,
        大型卧铺 = 10,
        中型卧铺 = 11,
        出租的士 = 12,
        公交车 = 13
    }

    /// <summary>
    /// 车辆种类
    /// </summary>
    public enum CheLiangZhongLei
    {
        客运班车 = 1,
        旅游包车 = 2,
        危险货运 = 3,
        重型货车 = 4,
        公交客运 = 5,
        出租客运 = 6,
        教练员车 = 7,
        普通货运 = 8,
        其它车辆 = 9,
        校车 = 10,
        网约车 = 11,
    }

    /// <summary>
    /// 车主类型
    /// </summary>
    public enum CheZhuLeiXing
    {
        企业所有 = 1,
        个人所有 = 2
    }

    /// <summary>
    /// 车主类型
    /// </summary>
    public enum ShiFouZaiXian
    {
        在线 = 1,
        离线 = 2
    }

    public enum HeZuoLeiXing
    {
        合作加盟 = 1,
        业务代理 = 2
    }

    /// <summary>
    /// 组织类型
    /// 1=道路运输企业
    /// 2=个体户
    /// 3=车队
    /// 4=本地服务商
    /// 5=平台代理商
    /// 6=平台运营商
    /// 7=第三方监测服务中心
    /// 8=保险机构
    /// 9=政府机构
    /// </summary>
    public enum OrgType
    {
        道路运输企业 = 1,
        个体户 = 2,
        车队 = 3,
        本地服务商 = 4,
        平台代理商 = 5,
        平台运营商 = 6,
        第三方监测服务中心 = 7,
        保险机构 = 8,
        政府机构 = 9
    }

    /// <summary>
    /// 是否历史记录
    /// </summary>
    public enum ShiFouLiShiJiLu
    {
        历史记录 = 1,
        当前数据 = 2
    }

    public enum LianXiRenLeiBie
    {
        企业法人 = 1,
        本地负责人 = 2,
        其他 = 3
    }

    /// <summary>
    /// 终端类型
    /// </summary>
    public enum ZhongDuanLeiXing
    {
        部标 = 1,
        部标北斗 = 2,
        DB44 = 3,
        其他 = 4,
        智能视频 = 5
    }

    /// <summary>
    /// 终端安装状态
    /// </summary>
    public enum AnZhuangZhuangTaiEnum
    {
        已安装 = 1,
        未安装 = 0
    }

    /// <summary>
    /// 设备类别
    /// </summary>
    public enum SheBeiLeiBieEnum
    {
        GPS = 0,
        智能视频 = 1
    }

    /// <summary>
    /// 组织角色
    /// </summary>
    public enum ZuZhiJueSe
    {
        企业审核填报用户=101,
        第三方机构审核填报用户 = 102,
        系统管理员 = 1,
        组织管理员 = 2,
        平台代理商 = 3,
        //分公司 = 4,
        //GPS运营商 = 5,
        服务商 = 4,
        GPS企业 = 6,
        GPS车队 = 7,
        车辆监控员 = 8,
        市级政府 = 11,
        县级政府 = 12,
    }


    /// <summary>
    /// 车辆状态枚举
    /// </summary>
    public enum CheLiangZhuangTai
    {
        上线 = 1,
        离线 = 2,
        营运 = 3,
        停用 = 4

    }

    /// <summary>
    /// 车辆燃料枚举
    /// </summary>
    public enum RanLiao
    {
        柴油 = 1,
        油气双燃料 = 2,
        节能油电混合动力 = 3,
        纯电动 = 4,
        插电式混合动力 = 5,
        氢燃料 = 6,
        燃油 = 7,
        其他 = 8

    }


    /// <summary>
    /// 车辆车身颜色枚举
    /// </summary>
    public enum CheShenYanSe
    {
        黄色 = 1,
        黑色 = 2,
        蓝色 = 3,
        白色 = 4,
        其他 = 5
    }

    /// <summary>
    /// 企业信息经营许可证状态
    /// </summary>
    public enum JingYingXuKeZhengYouXiaoZhuangTai
    {
        有效 = 1,
        过期 = 2
    }

    /// <summary>
    /// 企业有效状态
    /// </summary>
    public enum YouXiaoZhuangTai
    {
        正常营业 = 1,
        合约到期 = 2
    }

    /// <summary>
    /// 平台代理商企业类型
    /// </summary>
    public enum QiYeLeiXing
    {
        平台代理商 = 1,
        分支机构 = 2,
        道路运输企业 = 3,
        道路运输车队 = 4
    }

    /// <summary>
    /// 分支机构合作方式
    /// </summary>
    public enum HeZuoFangShi
    {
        合作加盟 = 1,
        业务代理 = 2
    }
    ///收费模式
    public enum ShouFeiMoShi
    {
        季度 = 1,
        半年 = 2,
        一年 = 3,
        二年 = 4,
        三年 = 5

    }

    public enum SIMZhuangTai
    {
        有效 = 1,
        无效 = 2
    }

    public enum SIMShiYongZhuangTai
    {
        已使用 = 1,
        未使用 = 2
    }
    public enum CheLiangShouFeiZhuangTai
    {
        未收费 = 1,
        已收费 = 2,
        待续费 = 3
    }
    public enum CheLiangFuWuZhuangTai
    {
        正常 = 1,
        预警 = 2,
        过期 = 3
    }

    public enum QiYeRenZhengZhuangTai
    {
        认证中 = 1,
        未认证 = 2,
        已认证 = 3
    }

    public enum ZhiWu
    {
        企业法人 = 1,
        管理人员 = 2,
        工作人员 = 3,
        司机 = 4
    }

    public enum YeHuXingZhi
    {
        运输企业 = 1,
        GPS运营商 = 2,
        个体户 = 3,
        驾培企业 = 4
    }

    public enum GeRenZhuCeXinXiLeiBie
    {
        微信 = 1,
        QQ = 2
    }
    public enum VideoZhongDuanImgType
    {
        显示屏 = 1,
        声光报警系统 = 2,
        主机存储器 = 3,
    }

    /// <summary>
    /// 车辆在线状态枚举
    /// </summary>
    public enum ZaiXianZhuangTai
    {
        离线 = 0,
        在线 = 1
    }

    public enum KaoHeZhuangTai
    {
        待考核 = 0,
        已通过 = 1,
    }

    public enum WorkingStatus
    {
        在职 = 2,
        离职 = 3
    }

    /// <summary>
    /// 终端数据通讯版本号
    /// </summary>
    public enum ZhongDuanShuJuTongXunBanBenHao
    {
        /// <summary>
        /// 苏标
        /// </summary>
        [Description("苏标")]
        SuBiao = 0,
        /// <summary>
        /// 粤标
        /// </summary>
        [Description("粤标")]
        YueBiao = 1
    }

    public enum ZhuangTai
    {
        第三方审核中 = 1,
        第三方审核失败 = 2,
        市级审核中 = 3,
        市级审核失败 = 4,
        市级审核成功 = 5,
        市级驳回 = 6
    }
    /// <summary>
    /// 外部注册账号审核流程
    /// </summary>
    public enum RegisterInfoApprovalStatus
    {
        注册账号 = 10,

        提交审核 = 20,

        审核通过 = 30,

        审核不通过 = 40,

        驳回 = 50,
    }
    /// <summary>
    /// 企业，第三方机构合作状态
    /// </summary>
    public enum CooperationStatus
    {
        企业发起待审核 = 1,
        第三方发起待审核 = 2,
        审批通过 = 3,
        审批不通过 = 4,
        企业发起取消合作 = 5,
        第三方发起取消合作 = 6,
        取消合作 = 7,
        取消申请=8
    }
    public enum ExportCooperationStatus
    {
        待第三方审批 = 1,
        待企业审批 = 2,
        已绑定关系 = 3,
        审批不通过 = 4,
        企业申请取消绑定 = 5,
        第三方申请取消绑定 = 6,
        取消绑定 = 7,
        取消申请 = 8
    }
    /// <summary>
    /// 车辆，企业，第三方机构合作状态
    /// </summary>
    public enum VehicleCooperationStatus
    {
        企业发起待审核 = 1,
        第三方发起待审核 = 2,
        审批通过 = 3,
        审批不通过 = 4,
        企业发起取消合作 = 5,
        第三方发起取消合作 = 6,
        取消合作 = 7,
        取消申请 = 8
    }

    public enum MonitorType
    {
        第三方监控=1,
        自主监控=2
    }
    /// <summary>
    /// 第三方机构审核状态
    /// </summary>
    public enum ThirdPartyRegistrationStatus
    {
        市级审核中=1,
        市级审核通过 =2,
        市级审核不通过 = 3,
        市级审核驳回 =4
    }
    /// <summary>
    /// 第三方机构类型
    /// </summary>
    public enum CompanyType
    {
        总公司=1,
        分公司=2
    }
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var attr = value.GetType().GetField(value.ToString()).GetCustomAttributes(false);
            var desAttr = (DescriptionAttribute)attr.Where(a => a is DescriptionAttribute).FirstOrDefault();

            return desAttr == null || string.IsNullOrEmpty(desAttr.Description) ? value.ToString() : desAttr.Description;
        }
    }

    public enum GPSAuditStatus
    {
        待审核 = 0,
        通过备案 = 1,
        未通过备案 = 2
    }


    public enum VehicleQualificationStatus
    {
        不予办理 = 0,
        给予办理 = 1
    }
}
