/* 
*EnumsSet.枚举名称;//返回枚举名称
*EnumsSet.枚举名称.枚举值;//返回中文
*EnumsSet.枚举名称.枚举值.value();//返回值
*EnumsSet.枚举名称.枚举值.key();//返回中文
*EnumsSet.枚举名称.枚举值.name();//返回枚举值属性
*/
var EnumsSet = (function (obj, win) {
    //审核状态
    obj.ShenHeZhuangTai = {
        CaoGao: ['草稿', 1, 'CaoGao'],
        ZuoFei: ['作废', 2, 'ZuoFei'],
        DaiShenHe: ['待审核', 3, 'DaiShenHe'],
        ShenHeTongGuo: ['审核通过', 4, 'ShenHeTongGuo'],
        ShenHeBuTongGuo: ['审核不通过', 5, 'ShenHeBuTongGuo'],
        TuiHui: ['退回', 6, 'TuiHui'],
        _key: '审核状态'
    }
    //变更情况
    obj.BianGengQingKuang = {
        XinGouZhi: ['新购置', 1, 'XinGouZhi'],
        GuoHuZhuanRu: ['过户转入', 2, 'GuoHuZhuanRu'],
        GuoHuZhuanChu: ['过户转出', 3, 'GuoHuZhuanChu'],
        BianGengQingKuangBaoFei: ['报废', 4, 'BianGengQingKuangBaoFei'],
        BianGengQingKuangZhuXiao: ['注销', 5, 'BianGengQingKuangZhuXiao'],
        WuBianGeng: ['无变更', 8, 'WuBianGeng'],
        _key: '变更情况'
    }
    //变速器形式
    obj.BianSuQiXingShi = {
        ShouDongBianSuQi: ['手动变速器', 1, 'ShouDongBianSuQi'],
        ZiDongBianSuQi: ['自动变速器', 2, 'ZiDongBianSuQi'],
        ShouDongZiDongBianSuQi: ['手动/自动变速器', 3, 'ShouDongZiDongBianSuQi'],
        WuJiShiBianSuQi: ['无级式变速器', 4, 'WuJiShiBianSuQi'],
        _key: '变速器形式'
    }
    //车牌颜色
    obj.ChePaiYanSe = {
        HuangSe: ['黄色', 1, 'HuangSe'],
        //HeiSe: ['黑色', 2, 'HeiSe'],
        LanSe: ['蓝色', 3, 'LanSe'],
        //BaiSe: ['白色', 4, 'BaiSe'],
        JianBianLvSe: ['渐变绿色', 5, 'JianBianLvSe'],
        HuangLvShuangPinSe: ['黄绿双拼色', 6, 'HuangLvShuangPinSe'],
        _key: '车牌颜色'
    }
    //车辆类型
    obj.CZCheLiangLeiXing = {
        //XiaoXingKeChe: ['小型客车', 1, 'XiaoXingKeChe'],
        //ZhongXingKeChe: ['中型客车', 2, 'ZhongXingKeChe'],
        //DaXingKeChe: ['大型客车', 3, 'DaXingKeChe'],
        JiaoChe: ['轿车', 4, 'JiaoChe'],
        //DaXingShuangCengKeChe: ['大型双层客车', 5, 'DaXingShuangCengKeChe'],
        //DaXingJiaoJieKeChe: ['大型铰接客车', 6, 'DaXingJiaoJieKeChe'],
        _key: '车辆类型'
    }
    //非插电式混合动力燃料
    obj.FeiChaDianShiHunHeDongLiRanLiao = {
        FeiChaDianShiHunHeDongLiLNG: ['LNG', 1, 'FeiChaDianShiHunHeDongLiLNG'],
        FeiChaDianShiHunHeDongLiCNG: ['CNG', 2, 'FeiChaDianShiHunHeDongLiCNG'],
        FeiChaDianShiHunHeDongLiLPG: ['LPG', 3, 'FeiChaDianShiHunHeDongLiLPG'],
        FeiChaDianShiHunHeDongLiQiYou: ['汽油', 4, 'FeiChaDianShiHunHeDongLiQiYou'],
        FeiChaDianShiHunHeDongLiChaiYou: ['柴油', 5, 'FeiChaDianShiHunHeDongLiChaiYou'],
        _key: '非插电式混合动力燃料'
    }
    //车辆类型
    obj.GJCheLiangLeiXing = {
        XiaoXingKeChe: ['小型客车', 1, 'XiaoXingKeChe'],
        ZhongXingKeChe: ['中型客车', 2, 'ZhongXingKeChe'],
        DaXingKeChe: ['大型客车', 3, 'DaXingKeChe'],
        TeDaXingKeChe: ['特大型客车', 4, 'TeDaXingKeChe'],
        _key: '车辆类型'
    }
    //农客车辆类型
    obj.NKCheLiangLeiXing = {
        XiaoXingKeChe: ['小型客车', 1, 'XiaoXingKeChe'],
        ZhongXingKeChe: ['中型客车', 2, 'ZhongXingKeChe'],
        DaXingKeChe: ['大型客车', 3, 'DaXingKeChe'],
        TeDaXingKeChe: ['特大型客车', 4, 'TeDaXingKeChe'],
        _key: '农客车辆类型'
    }
    //公交车辆类型
    obj.GongJiaoCheLiangLeiXing = {
        YongQi: ['用气', 1, 'YongQi'],
        YongYou: ['用油', 2, 'YongYou'],
        XinNengYuan: ['新能源', 3, 'XinNengYuan'],
        FeiChaDianShiHunHeDongLi: ['非插电式混合动力', 4, 'FeiChaDianShiHunHeDongLi'],
        GongJiaoCheLiangLeiXingQiTa: ['其他', 5, 'GongJiaoCheLiangLeiXingQiTa'],
        _key: '公交车辆类型'
    }
    //行驶证车辆类型
    obj.XingShiZhengCheLiangLeiXing = {
        XiaoXingKeChe: ['小型客车', 1, 'XiaoXingKeChe'],
        ZhongXingKeChe: ['中型客车', 2, 'ZhongXingKeChe'],
        DaXingKeChe: ['大型客车', 3, 'DaXingKeChe'],
        TeDaXingKeChe: ['特大型客车', 4, 'TeDaXingKeChe'],
        _key: '行驶证车辆类型'
    }
    //公司类型
    obj.GongSiLeiXing = {
        本公司: ['本公司', 1, '本公司'],
        子公司: ['子公司', 2, '子公司'],
        分公司: ['分公司', 3, '分公司'],
        _key: '公司类型'
    }
    //经济类型
    obj.JingJiLeiXing = {
        SiYouDuZi: ['私有独资', 1, 'SiYouDuZi'],
        SiYouHeHuo: ['私有合伙', 2, 'SiYouHeHuo'],
        SiYingYouXianZeRenGongSi: ['私营有限责任公司', 3, 'SiYingYouXianZeRenGongSi'],
        SiYingGuFenYouXianGongSi: ['私营股份有限公司', 4, 'SiYingGuFenYouXianGongSi'],
        GeTiJingYing: ['个体经营', 5, 'GeTiJingYing'],
        QiTaSiYou: ['其他私有', 6, 'QiTaSiYou'],
        QiTaNeiZi: ['其他内资', 7, 'QiTaNeiZi'],
        GangAoTaiTouZi: ['港澳台投资', 8, 'GangAoTaiTouZi'],
        NeiDiHeGangAoHuoTaiHeZi: ['内地和港澳或台合资', 9, 'NeiDiHeGangAoHuoTaiHeZi'],
        GangAoHuoTaiDuZi: ['港澳或台独资', 10, 'GangAoHuoTaiDuZi'],
        GangAoHuoTaiTouZiGuFenYouXianGongSi: ['港澳或台投资股份有限公司', 11, 'GangAoHuoTaiTouZiGuFenYouXianGongSi'],
        QiTaGangAoTaiTouZi: ['其他港澳台投资', 12, 'QiTaGangAoTaiTouZi'],
        GuoWaiTouZi: ['国外投资', 13, 'GuoWaiTouZi'],
        ZhongWaiHeZi: ['中外合资', 14, 'ZhongWaiHeZi'],
        ZhongWaiHeZuo: ['中外合作', 15, 'ZhongWaiHeZuo'],
        WaiZi: ['外资', 16, 'WaiZi'],
        GuoWaiTouZiGuFenYouXianGongSi: ['国外投资股份有限公司', 17, 'GuoWaiTouZiGuFenYouXianGongSi'],
        QiTaGuoWaiTouZi: ['其他国外投资', 18, 'QiTaGuoWaiTouZi'],
        NeiDiHeGangAoHuoTaiHeZuo: ['内地和港澳或台合作', 19, 'NeiDiHeGangAoHuoTaiHeZuo'],
        QiTa: ['其他', 20, 'QiTa'],
        _key: '经济类型'
    }
    //经营方式
    obj.JingYingFangShi = {
        ZiYing: ['自营（公车公营）', 1, 'ZiYing'],
        FeiZiYing: ['非自营（承包，挂靠）', 2, 'FeiZiYing'],
        _key: '经营方式'
    }
    //排放标准
    obj.PaiFangBiaoZhun = {
        GuoYiBiaoZhun: ['国Ⅱ以下', 1, 'GuoYiBiaoZhun'],
        GuoErBiaoZhun: ['国Ⅱ', 2, 'GuoErBiaoZhun'],
        GuoSanBiaoZhun: ['国Ⅲ', 3, 'GuoSanBiaoZhun'],
        GuoSiBiaoZhun: ['国Ⅳ', 4, 'GuoSiBiaoZhun'],
        GuoWuBiaoZhun: ['国Ⅴ', 5, 'GuoWuBiaoZhun'],
        QiTaBiaoZhun: ['其他', 6, 'QiTaBiaoZhun'],
        _key: '排放标准'
    }
    //燃料二
    obj.RanLiaoEr = {
        LPG: ['LPG', 1, 'LPG'],
        CNG: ['CNG', 2, 'CNG'],
        LNG: ['LNG', 3, 'LNG'],
        RanLiaoErQiYou: ['汽油', 4, 'RanLiaoErQiYou'],
        RanLiaoErChaiYou: ['柴油', 5, 'RanLiaoErChaiYou'],
        _key: '燃料二'
    }
    //燃料类型
    obj.RanLiaoLeiXing = {
        YongQi: ['用气', 1, 'YongQi'],
        YongYou: ['用油', 2, 'YongYou'],
        XinNengYuan: ['新能源', 3, 'XinNengYuan'],
        FeiChaDianShiHunHeDongLi: ['非插电式混合动力', 4, 'FeiChaDianShiHunHeDongLi'],
        ShuangRanLiao: ['双燃料', 5, 'ShuangRanLiao'],
        QiTa: ['其他', 6, 'QiTa'],
        _key: '燃料类型'
    }
    //混合动力
    obj.RanLiaoWeiHunHeDongLi = {
        QiYouHeChaiYou: ['汽油和柴油', 0, 'QiYouHeChaiYou'],
        _key: '混合动力'
    }
    //设备名称
    obj.SheBeiMingCheng = {
        ChuZuCheBiaoZhiDeng: ['出租车标志灯', 1, 'ChuZuCheBiaoZhiDeng'],
        WeiXingDingWeiSheBei: ['卫星定位设备', 2, 'WeiXingDingWeiSheBei'],
        JiJiaQi: ['计价器', 3, 'JiJiaQi'],
        KongCheDaiZuBiaoZhi: ['空车待租标志', 4, 'KongCheDaiZuBiaoZhi'],
        AnQuanFangFanZhuangZhi: ['安全防范装置', 5, 'AnQuanFangFanZhuangZhi'],
        SheShiSheBeiQiTa: ['其他', 6, 'SheShiSheBeiQiTa'],
        _key: '设备名称'
    }
    //所属总公司属地
    obj.SuoShuZongGongSiShuDi = {
        BenSheng: ['本省', 1, 'BenSheng'],
        WaiSheng: ['外省', 2, 'WaiSheng'],
        _key: '所属总公司属地'
    }
    //辖区省
    obj.XiaQuSheng = {
        GuangDongSheng: ['广东', 1, 'GuangDongSheng'],
        _key: '辖区省'
    }
    //星级
    obj.XingJi = {
        WuXing: ['五星', 1, 'WuXing'],
        SiXing: ['四星', 2, 'SiXing'],
        SanXing: ['三星', 3, 'SanXing'],
        ErXing: ['二星', 4, 'ErXing'],
        YiXing: ['一星', 5, 'YiXing'],
        Wu: ['无', 6, 'Wu'],
        _key: '星级'
    }
    //新能源燃料
    obj.XinNengYuanRanLiao = {
        XinNengYuanChunDianDong: ['纯电动', 6, 'XinNengYuanChunDianDong'],
        XinNengYuanChaDianShiHunHeDongLi: ['插电式混合动力(含增程式)', 7, 'XinNengYuanChaDianShiHunHeDongLi'],
        XinNengYuanChaoJiDianRong: ['超级电容', 8, 'XinNengYuanChaoJiDianRong'],
        XinNengYuanRanLiaoDianChi: ['燃料电池', 9, 'XinNengYuanRanLiaoDianChi'],
        _key: '新能源燃料'
    }
    //新增原因
    obj.XinZengYuanYin = {
        XinZeng: ['新增', 1, 'XinZeng'],
        BaoFeiGengXin: ['报废更新', 2, 'BaoFeiGengXin'],
        ZhuXiaoGengXin: ['注销更新', 3, 'ZhuXiaoGengXin'],
        _key: '新增原因'
    }
    //用气燃料
    obj.YongQiRanLiao = {
        YongQiLNG: ['LNG', 1, 'YongQiLNG'],
        YongQiCNG: ['CNG', 2, 'YongQiCNG'],
        YongQiLPG: ['LPG', 3, 'YongQiLPG'],
        _key: '用气燃料'
    }
    //用油燃料
    obj.YongYouRanLiao = {
        YongYouQiYou: ['汽油', 4, 'YongYouQiYou'],
        YongYouChaiYou: ['柴油', 5, 'YongYouChaiYou'],
        _key: '用油燃料'
    }
    //运营方式
    obj.YunYingFangShi = {
        DingXianYunYing: ['定线运营', 1, 'DingXianYunYing'],
        HuanXianYunYing: ['循环运营', 2, 'HuanXianYunYing'],
        QuYuYunYing: ['区域运营', 3, 'QuYuYunYing'],
        _key: '运营方式'
    }
    //运营模式
    obj.YunYingMoShi = {
        DanBan: ['单班', 1, 'DanBan'],
        ShuangBan: ['双班', 2, 'ShuangBan'],
        QiTa: ['其他', 3, 'QiTa'],
        _key: '运营模式'
    }
    //运营区域
    obj.YunYingQuYu = {
        XianNei: ['县内', 1, 'XianNei'],
        YunYingQuYuXianJi: ['县际', 2, 'YunYingQuYuXianJi'],
        ShiNei: ['市内', 3, 'ShiNei'],
        YunYingQuYuShengJi: ['省际', 4, 'YunYingQuYuShengJi'],
        _key: '运营区域'
    }
    //状态
    obj.ZhuangTai = {
        YingYun: ['营运', 1, 'YingYun'],
        DaiYingYun: ['待营运', 2, 'DaiYingYun'],
        BaoTing: ['报停', 3, 'BaoTing'],
        ZhuXiao: ['注销', 4, 'ZhuXiao'],
        BaoFei: ['报废', 5, 'BaoFei'],
        BeiQiang: ['被抢', 6, 'BeiQiang'],
        BeiDao: ['被盗', 7, 'BeiDao'],
        _key: '状态'
    }
    //综合网站新闻
    obj.ZongHeWangZhanXinWen = {
        BangZhuWenDang: ['帮助文档', 0, 'BangZhuWenDang'],
        RuanJianXiaZai: ['软件下载', 0, 'RuanJianXiaZai'],
        YeWuGongShi: ['业务公示', 0, 'YeWuGongShi'],
        JianGuanGongGao: ['监管公告', 0, 'JianGuanGongGao'],
        HangYeXinWen: ['行业新闻', 0, 'HangYeXinWen'],
        ZhengCeFaGui: ['政策法规', 0, 'ZhengCeFaGui'],
        ZiLiaoXiaZai: ['资料下载', 0, 'ZiLiaoXiaZai'],
        JiShuGuiFan: ['技术规范', 0, 'JiShuGuiFan'],
        ZhongYaoXinWenJiTongZhi: ['重要文件及通知', 0, 'ZhongYaoXinWenJiTongZhi'],
        _key: '综合网站新闻'
    }
    //车辆装备等级
    obj.CheLiangZhuangBeiDengJi = {
        WeiDingJi: ['未定级', 1, 'WeiDingJi'],
        PuTongJi: ['普通级', 2, 'PuTongJi'],
        ZhongJi: ['中级', 3, 'ZhongJi'],
        GaoYiJi: ['高一级', 4, 'GaoYiJi'],
        GaoErJi: ['高二级', 5, 'GaoErJi'],
        GaoSanJi: ['高三级', 6, 'GaoSanJi'],
        _key: '车辆装备等级'
    }

    //人员档案 —— 职务
    obj.Position = {
        Legalperson: ['企业法人', 1, 'Legalperson'],
        Admin: ['管理员', 2, 'Admin'],
        Worker: ['工作人员', 3, 'Worker'],
        Driver: ['驾驶员', 4, 'Driver'],
        Other: ['其他', 99, 'Other'],
        _key: '人员档案职务'
    }

    //人员档案 —— 就职状态
    obj.WorkingStatus = {
        Entry: ['待确认', 1, 'WaitConfirm'],
        Work: ['在职', 2, 'Work'],
        Leave: ['离职', 3, 'Leave'],
        _key: '人员档案就职状态'
    }

    //人员档案 —— 证件类型
    obj.IDCardType = {
        Zero: ['居民身份证', 0, 'Zero'],
        One: ['香港特区护照/身份证明', 1, 'One'],
        Two: ['澳门特区护照/身份证明', 2, 'Two'],
        Three: ['港澳居民居住证', 3, 'Three'],
        Four: ['台湾居民居住证', 4, 'Four'],
        Five: ['台湾居民来往大陆通行证', 5, 'Five'],
        Six: ['外籍人士护照', 6, 'Six'],
        Seven: ['军官证', 7, 'Seven'],
        Nine: ['其它', 99, 'Nine'],
        _key: '人员档案证件类型'
    }


    this.Array.prototype.toString = function () {
        return this[0];
    }
    this.Array.prototype.name = function () {
        return this[2];
    }
    this.Array.prototype.key = this.Array.prototype.toString;
    this.Array.prototype.value = function () {
        return this[1];
    }
    return obj;
})(window.EnumsSet || {}, window);
