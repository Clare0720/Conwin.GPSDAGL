(function (root, config) {
    'use strict';
    if (typeof define === 'function' && define.amd) {
        define([], function () {
            return config;
        });
    }
    else {
        root.SystemConfig = config;
    }
})(this,
    {
        SysId: '60190FC4-5103-4C76-94E4-12A54B62C92A',
        AppId: '938B7DFE-5DEA-4BC4-AFE3-1095385AB598',
        SysName: '卫星定位监控综合平台',
        AppName: '',
        IsTest: true,
        AdaptLogin: false,
        DefaultUrl: '/Modules/GPSVMS/QYJKCLJK/mapList.html',
        DaPingUrl: '/Modules/Home/LoginIndex.html',
        GovernmentUrl: '/Modules/GPSDAGL/CheLiangDangAn/List.html',
        LoginUrl: '/Modules/Home/Index.html',
        LogoutUrl: '/Modules/Home/Logout.html',
        Exponent: '010001',         //加密需要，Exponent参数
        Modulus: 'A85A7F6667773D8FB7013C482CDB5EFCC06A84E218454204B86CAF42313431116FBBDE0020B62EE91E970E6991340B34ED2A8C51B00B768B934BEF6E584528A7097DAD560C41F164A2A7AD8706E41C7346B5DFDD1D0E204A373A352F255BDFDD8DA4917551F3835FCEC56C72FDC8B38A783FEA8937E2C0A5B2D80750F3B7D3A9',        //加密需要，Modulus参数
        ServerAgent: "http://10.0.64.235:7007/api/ServiceGateway/DataService",
        //ServerAgent: "http://10.0.105.246:8007/api/ServiceGateway/DataService",

        ServiceCodeTable: [
            { code: "000000000001", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/QiYeRenZhengGuanLi/Query" },
            { code: "000000000002", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/QiYeRenZhengGuanLi/QueryYuanGong" },
            { code: "000000000003", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/QiYeRenZhengGuanLi/Create" },
            { code: "000000000004", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/QiYeRenZhengGuanLi/Get" },
            { code: "000000000005", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/QiYeRenZhengGuanLi/GetYuanGong" },
            { code: "000000000006", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/QiYeRenZhengGuanLi/CreateYuanGong" },
            { code: "000000000007", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/QiYeRenZhengGuanLi/Update" },
            { code: "000000000008", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/QiYeRenZhengGuanLi/UpdateYuanGong" },


            //{ code: "003300300155", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/JiaShiYuanXinXi/GetDaoLuYunShuCongYeRenYuanTaiZhang" },
            { code: "003300300155", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/JiaShiYuanXinXi/GetGuangDongShengCongYeRenYuan" },
            { code: "003300300156", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/JiaShiYuanXinXi/Create" },
            { code: "003300300157", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/JiaShiYuanXinXi/Query" },
            { code: "003300300158", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/JiaShiYuanXinXi/GetZhaoPian" },
            //{ code: "003300300158", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/JiaShiYuanXinXi/GetZhaoPianBase64" },
            { code: "003300300159", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/JiaShiYuanXinXi/Delete" },
            { code: "003300300160", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/JiaShiYuanXinXi/Get" },
            { code: "003300300161", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/JiaShiYuanXinXi/Update" },
            { code: "003300300163", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/JiaShiYuanXinXi/CustomerDriverCacheFailItems" },
            { code: "003300300164", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/JiaShiYuanXinXi/DriverCacheByKeys" },
            { code: "003300300165", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/JiaShiYuanXinXi/DriverCache" },
            { code: "003300300166", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/JiaShiYuanXinXi/QueryOnly" },
            { code: "006600200001", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/Query" },

            { code: "003300300001", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiDaiLiShangXinXi/Query" },
            { code: "003300300002", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiDaiLiShangXinXi/Get" },
            { code: "003300300004", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiDaiLiShangXinXi/Create" },
            { code: "003300300005", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiDaiLiShangXinXi/Update" },
            //{ code: "003300300006", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiDaiLiShangXinXi/Delete" },
            { code: "003300300006", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/Organization/Delete" },
           // { code: "003300300007", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiDaiLiShangXinXi/CancelStatus" },
            { code: "003300300007", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/Organization/CancelStatus" },
            //{ code: "003300300008", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiDaiLiShangXinXi/NormalStatus" },
            { code: "003300300008", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/Organization/NormalStatus" },

            //{ code: "003300300009", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/LianXiRenXinXi/Query" },
            //{ code: "003300300010", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/LianXiRenXinXi/Create" },
            //{ code: "003300300011", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/LianXiRenXinXi/Update" },
            //{ code: "003300300012", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/LianXiRenXinXi/Delete" },
            //{ code: "003300300013", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/LianXiRenXinXi/Get" },

            //{ code: "003300300014", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/FenZhiJiGouHeGPSYunYingShangXinXi/Query" },
            //{ code: "003300300015", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/FenZhiJiGouHeGPSYunYingShangXinXi/Get" },
            //{ code: "003300300017", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/FenZhiJiGouHeGPSYunYingShangXinXi/Create" },
            //{ code: "003300300018", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/FenZhiJiGouHeGPSYunYingShangXinXi/Update" },
            //{ code: "003300300019", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/FenZhiJiGouHeGPSYunYingShangXinXi/Delete" },
            //{ code: "003300300020", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/FenZhiJiGouHeGPSYunYingShangXinXi/CancelStatus" },
            //{ code: "003300300021", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/FenZhiJiGouHeGPSYunYingShangXinXi/NormalStatus" },
            { code: "003300300014", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/FuWuShangXinXi/Query" },
            { code: "003300300015", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/FuWuShangXinXi/Get" },
            { code: "003300300017", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/FuWuShangXinXi/Create" },
            { code: "003300300018", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/FuWuShangXinXi/Update" },
            { code: "003300300019", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/Organization/Delete" },
            { code: "003300300020", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/Organization/CancelStatus" },
            { code: "003300300021", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/Organization/NormalStatus" },
           

            { code: "003300300022", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/Query" },
            { code: "003300300023", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/Get" },
            { code: "003300300025", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/Create" },
            { code: "003300300026", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/Update" },
            { code: "003300300027", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/Delete" },
            { code: "003300300028", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/CancelStatus" },
            { code: "003300300029", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/NormalStatus" },
            { code: "003300300030", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/Submit" },
            { code: "003300300031", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/Check" },

            { code: "003300300032", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheDuiXinXi/Query" },
            { code: "003300300033", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheDuiXinXi/Get" },
            { code: "003300300035", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheDuiXinXi/Create" },
            { code: "003300300036", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheDuiXinXi/Update" },
            //{ code: "003300300037", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheDuiXinXi/Delete" },
            //{ code: "003300300038", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheDuiXinXi/CancelStatus" },
            //{ code: "003300300039", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheDuiXinXi/NormalStatus" },
            { code: "003300300037", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/Organization/Delete" },
            { code: "003300300038", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/Organization/CancelStatus" },
            { code: "003300300039", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/Organization/NormalStatus" },

            { code: "003300300040", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheDuiXinXi/Submit" },
            { code: "003300300041", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheDuiXinXi/Check" },





            { code: "003300300075", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanXinXi/Query" },
            { code: "003300300076", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanXinXi/Get" },
            { code: "003300300077", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanXinXi/IsExists" },
            { code: "003300300078", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanXinXi/Create" },
            { code: "003300300079", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanXinXi/Update" },
            { code: "003300300080", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanXinXi/Delete" },
            { code: "003300300081", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanXinXi/Off" },
            { code: "003300300082", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanXinXi/On" },

            //车辆收费
            { code: "003300300085", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangShouFeiJiLu/Create" },
            { code: "003300300086", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangShouFeiJiLu/Update" },
            { code: "003300300087", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangShouFeiJiLu/Delete" },
            { code: "003300300083", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangShouFeiJiLu/Query" },
            { code: "003300300084", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangShouFeiJiLu/Get" },
            { code: "003300300089", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangShouFeiJiLu/Import" },
            { code: "003300300090", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangShouFeiJiLu/Export" },
            { code: "003300300088", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangShouFeiJiLu/XuFeiTiXin" },
            { code: "003300300091", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangShouFeiJiLu/SearchCarList" },
            { code: "003300300092", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangShouFeiJiLu/GetCarInfoFromNum" },
            { code: "003300300093", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangShouFeiJiLu/ShouFeiSum " },
            { code: "003300300141", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangShouFeiJiLu/GetCarInfoFromNums" },
            { code: "003300300139", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangShouFeiJiLu/PiLiangShouFeiCreate" },

            { code: "003300300100", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/ZuZhiGongZhangXinXi/Query" },
            { code: "003300300102", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/ZuZhiGongZhangXinXi/Create" },
            { code: "003300300103", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/ZuZhiGongZhangXinXi/Update" },
            { code: "003300300101", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/ZuZhiGongZhangXinXi/Get" },
            { code: "003300300104", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/ZuZhiGongZhangXinXi/Delete" },

            { code: "003300300094", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangAnZhuangZhengMingJiLuXinXi/Query" },

            { code: "003300300098", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangAnZhuangZhengMingDaYinJiLu/Query" },
            { code: "003300300097", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangAnZhuangZhengMingDaYinJiLu/Create" },
            { code: "003300300099", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangAnZhuangZhengMingDaYinJiLu/Print" },
            // 新增车辆安装证明记录
            { code: "003300300096", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangAnZhuangZhengMingJiLuXinXi/Create" },
            { code: "003300300095", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangAnZhuangZhengMingJiLuXinXi/Export" },

            // 车辆业务办理
            // 校验车辆是否符合办理业务条件
            { code: "003300300073", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangYeWuBanLi/CheckCarInfoRight" },
            // 获取车辆上报所需信息
            { code: "003300300074", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangYeWuBanLi/GetCarInfoFromID" },
            //同步车辆信息到地市
            { code: "003300300140", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangYeWuBanLi/TongBuCheLiangXinXiToDiShi" },

            ///车辆列表查询
            { code: "003300300042", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/Query" },
            //车辆停用与启用
            { code: "003300300044", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/UpDateState" },
            //安装终端时写入终端关系
            { code: "003300300045", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/UpDataTerminalInfo" },

            //删除车辆信息
            { code: "003300300043", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/Delete" },
            //判断车辆是否已安装终端
            { code: "003300300107", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/CheckVechicleInstallTerminal" },

            //获取企业列表
            { code: "003300300071", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GetYeHu" },
            //获取GPS商和分公司列表
            { code: "003300300130", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GetYunYingShang" },
            //获取车队列表
            { code: "003300300072", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GetCheDui" },
            //删除白名单
            { code: "003300300070", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/DelVehicleWhiteList" },
            //车辆查看
            { code: "003300300046", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GetVehicleBasicInfo " },
            { code: "003300300047", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GetVehicleDetailedInfo " },
            { code: "003300300048", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GetVehicleDataReport " },
            { code: "003300300049", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GetVehicleEnterpriseInfo " },
            { code: "003300300050", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GetVehicleCJInfo " },
            { code: "003300300051", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GetVehicleServiceInfo " },
            { code: "003300300052", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GetVehicleTerminalInstallInfo " },

            //车辆新增
            { code: "003300300053", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/Create " },
            { code: "003300300054", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/AddVehicleDetailedInfo " },
            { code: "003300300106", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/AddVehicleDataReport " },
            { code: "003300300055", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/AddVehicleEnterpriseInfo " },
            { code: "003300300056", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/AddVehicleCJInfo " },
            { code: "003300300057", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/AddVehicleServiceInfo " },
            { code: "003300300058", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/AddVehicleTerminalInstallInfo " },
            { code: "003300300070", ver: "1.0", url: "http://localhost:1420/api/GPSDAGL/CheLiang/DelVehicleWhiteList" },
            { code: "003300300128", ver: "1.0", url: "http://localhost:1420/api/GPSDAGL/CheLiang/AddCheLiangKuSuLuRuInfo" },
            { code: "003300300129", ver: "1.0", url: "http://localhost:1420/api/GPSDAGL/CheLiang/ExportCheliangXinXi" },
            { code: "003300300146", ver: "1.0", url: "http://localhost:1420/api/GPSDAGL/CheLiang/ImportCheLiangInfo" },

            //车辆修改
            { code: "003300300059", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/Update" },
            { code: "003300200087", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/UpdateVehicleBasicInfo " },
            { code: "003300300060", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/UpDateVehicleDetailedInfo " },
            { code: "003300300061", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/UpDateVehicleDataReport " },
            { code: "003300300105", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/UpdateVehicleEnterpriseInfo " },
            { code: "003300300062", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/UpDateVehicleCJInfo " },
            { code: "003300200092", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/UpDateVehicleServiceInfo " },
            { code: "003300200093", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/UpDateVehicleTerminalInstallInfo " },

            //车辆监控
            { code: "003300200094", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GetVehicleInfoYeHu " },
            { code: "003300300066", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangJianKong/GetVehicleInfoByYeHu " },
            { code: "003300300067", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangJianKong/AddVehicleMonitoring " },
            { code: "003300300068", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangJianKong/DelVehicleMonitoring " },
            { code: "003300200098", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GetVehicleInfoCheDui " },
            //   { code: "0033002000130", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GetVehicleYiFenPeiJianKong " },

            // 车辆生成材料证明
            { code: "003300300400", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GenerateGPSInstallCert " },
            { code: "003300300401", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GenerateVideoDeviceInstallCert " },
            // 项目服务信息
            { code: "003300300402", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GetCheLiangMingDan " },
            { code: "003300300403", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/AddCheLiangMingDan " },

            //平台接入
            { code: "003300300115", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiJieRuXinXi/GetPingTaiJieRuXinXiList" },
            { code: "003300300116", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiJieRuXinXi/AddPingTaiJieRuXinXi" },
            { code: "003300300117", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiJieRuXinXi/UpdatePingTaiJieRuXinXi" },
            { code: "003300300118", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiJieRuXinXi/DeletePingTaiJieRuXinXi" },
            { code: "003300300119", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiJieRuXinXi/GetPingTaiJieRuXinXi" },
            { code: "003300300120", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiJieRuXinXi/CheckPingTaiJieRuXinXi" },
            { code: "003300300124", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiJieRuXinXi/QuerySubOrg" },

            //SIM卡
            { code: "003300300131", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SIMKaHaoXinXi/GetSIMKaHaoXinXiList" },
            { code: "003300300132", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SIMKaHaoXinXi/AddSIMKaHaoXinXi" },
            { code: "003300300133", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SIMKaHaoXinXi/UpdateSIMKaHaoXinXi" },
            { code: "003300300134", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SIMKaHaoXinXi/DeleteSIMKaHaoXinXi" },
            { code: "003300300135", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SIMKaHaoXinXi/GetSIMKaHaoXinXi" },
            { code: "003300300136", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SIMKaHaoXinXi/UnboundSIM" },
            { code: "003300300137", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SIMKaHaoXinXi/ImportSIM" },
            { code: "003300300138", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SIMKaHaoXinXi/ExportSIM" },
            { code: "003300300142", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SIMKaHaoXinXi/CheckSIM" },
            { code: "003300300143", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SIMKaHaoXinXi/CheckZhongDuan" },
            { code: "003300300145", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SIMKaHaoXinXi/GetZhongDuanChePaiHao" },

            { code: "003300300111", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/QiYeXinXi/CheckPower" },

            // 人员档案管理
            { code: "003300300300", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/RenYuanXinXi/Query" },
            { code: "003300300301", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/RenYuanXinXi/Create" },
            { code: "003300300302", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/RenYuanXinXi/GetPersonInfoDetail" },
            { code: "003300300303", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/RenYuanXinXi/Get" },
            { code: "003300300304", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/RenYuanXinXi/Update" },
            { code: "003300300305", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/RenYuanXinXi/GetBy" },
            { code: "003300300306", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/RenYuanXinXi/GetMyWorkMate" },
            { code: "003300300307", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/RenYuanXinXi/UpdateForMoblie" },
            //{ code: "003300300309", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/QiYeXinXi/QueryForRenYuanXinXiMobile" },
            { code: "003300300324", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/RenYuanReLianXinXi/Get" },
            { code: "003300300329", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/RenYuanXinXi/QueryForQiYe" },
            { code: "003300300330", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/RenYuanXinXi/CreateRenYuanXinXiAccount" },

            { code: "003300300332", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/RenYuanXinXi/QueryPosition" },
            
            //组织公章管理
            { code: "006600200085", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/ZuZhiGongZhangXinXi/Query" },
            { code: "006600200086", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/ZuZhiGongZhangXinXi/Create" },
            { code: "006600200087", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/ZuZhiGongZhangXinXi/Update" },
            { code: "006600200088", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/ZuZhiGongZhangXinXi/Delete" },
            { code: "006600200089", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/ZuZhiGongZhangXinXi/Get" },

            //安装证明管理
            { code: "006600200090", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/ZuZhiGongZhangXinXi/Delete" },
            { code: "006600200091", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangAnZhuangZhengMing/Query" },
            { code: "006600200092", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangAnZhuangZhengMing/ExportAnZhuangZhengMing" },

            { code: "00000080000", ver: '1.0', url: "http://10.0.64.235:7007/api/ServiceGateway/UploadFile" },//上传文件
            { code: "00000080003", ver: '1.0', url: "http://10.0.64.249:7009/api/FileModuleApi/DeleteFile" },//删除文件
            { code: "00000080004", ver: '1.0', url: "http://10.0.64.235:7007/api/ServiceGateway/GetFile" },//查看文件
            { code: "00000080005", ver: '1.0', url: "http://10.0.64.235:7007/api/ServiceGateway/DownloadFile" },


            //第三方机构档案管理
            //{ code: "003300300300", ver: '1.0', url: "http://192.168.88.100:8888/list" },
            //{ code: "003300300301", ver: '1.0', url: "http://192.168.88.100:8888/add" },
            //{ code: "003300300302", ver: '1.0', url: "http://192.168.88.100:8888/view" },
            //{ code: "003300300303", ver: '1.0', url: "http://192.168.88.100:8888/true" },
            //{ code: "003300300304", ver: '1.0', url: "http://192.168.88.100:8888/true" },
            //{ code: "003300300305", ver: '1.0', url: "http://192.168.88.100:8888/true" },
            //{ code: "003300300306", ver: '1.0', url: "http://192.168.88.100:8888/true" },


            { code: "true", ver: '1.0', url: "http://192.168.88.100:8888/true" },


            { code: "003300300310", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/DiSanFangJiGouXinXi/Query" },
            { code: "003300300311", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/DiSanFangJiGouXinXi/Create" },
            { code: "003300300312", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/DiSanFangJiGouXinXi/Get" },
            { code: "003300300313", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/DiSanFangJiGouXinXi/Update" },
            //{ code: "003300300314", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/DiSanFangJiGouXinXi/Delete" },
            //{ code: "003300300315", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/DiSanFangJiGouXinXi/NormalStatus" },
            //{ code: "003300300316", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/DiSanFangJiGouXinXi/CancelStatus" },
            { code: "003300300314", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/Organization/Delete" },
            { code: "003300300315", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/Organization/NormalStatus" },
            { code: "003300300316", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/Organization/CancelStatus" },



            { code: "003300300317", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/GeTiHuXinXi/Query" },
            { code: "003300300318", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/GeTiHuXinXi/Create" },
            { code: "003300300319", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/GeTiHuXinXi/Get" },
            { code: "003300300320", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/GeTiHuXinXi/Update" },
            { code: "003300300321", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/GeTiHuXinXi/Delete" },
            { code: "003300300322", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/GeTiHuXinXi/NormalStatus" },
            { code: "003300300323", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/GeTiHuXinXi/CancelStatus" },
            { code: "003300300331", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/GeTiHuXinXi/QueryForAddVehicleQuickly" },

            { code: "003300300333", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/QiYeCustomRulesUpSpeedDetails/GetUpSpeedEventInfo" },



            //-------------------------------------NEW-----------------------------------------------
            //删除
            { code: "003300300500", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/Organization/Delete" },
            //启用
            { code: "003300300501", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/Organization/NormalStatus" },
            //停用
            { code: "003300300502", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/Organization/CancelStatus" },

            //代理商
            { code: "003300300503", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiDaiLiShangXinXi/Query" },
            { code: "003300300504", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiDaiLiShangXinXi/Get" },
            { code: "003300300505", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiDaiLiShangXinXi/Create" },
            { code: "003300300506", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/PingTaiDaiLiShangXinXi/Update" },

            //服务商
            { code: "003300300507", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/FuWuShangXinXi/Query" },
            { code: "003300300508", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/FuWuShangXinXi/Get" },
            { code: "003300300509", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/FuWuShangXinXi/Create" },
            { code: "003300300510", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/FuWuShangXinXi/Update" },

            //车队
            { code: "003300300511", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheDuiXinXi/Query" },
            { code: "003300300512", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheDuiXinXi/Get" },
            { code: "003300300513", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheDuiXinXi/Create" },
            { code: "003300300514", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheDuiXinXi/Update" },
            { code: "003300300515", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheDuiXinXi/Submit" },
            { code: "003300300516", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheDuiXinXi/Check" },

            //第三方机构
            { code: "003300300517", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/DiSanFangJiGouXinXi/Query" },
            { code: "003300300518", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/DiSanFangJiGouXinXi/Create" },
            { code: "003300300519", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/DiSanFangJiGouXinXi/Get" },
            { code: "003300300520", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/DiSanFangJiGouXinXi/Update" },

            //终端管理
            { code: "003300300521", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanXinXi/Query" },
            { code: "003300300522", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanXinXi/Get" },
            { code: "003300300523", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanXinXi/IsExists" },
            { code: "003300300524", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanXinXi/Create" },
            { code: "003300300525", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanXinXi/Update" },
            { code: "003300300526", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanXinXi/Delete" },
            { code: "003300300527", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanXinXi/Off" },
            { code: "003300300528", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanXinXi/On" },

            //终端安装管理
            { code: "003300300529", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanAnZhuang/Query" },
            { code: "003300300530", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanAnZhuang/CreateGPS" },
            { code: "003300300531", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanAnZhuang/CreateVideo" },
            { code: "003300300532", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/SheBeiZhongDuanAnZhuang/Get" },


            //业户
            { code: "003300300533", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/Query" },
            { code: "003300300534", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/Get" },
            { code: "003300300535", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/Create" },
            { code: "003300300536", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/Update" },

            //车辆管理
            { code: "003300300537", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/Query" },
            { code: "003300300538", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/Create " },
            { code: "003300300539", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/Update" },
            { code: "003300300540", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/Delete" },
            { code: "003300300541", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/UpDateState" },
            { code: "003300300542", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GetVehicleBasicInfo " },
            { code: "003300300543", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GetVehicleDetailedInfo " },
            { code: "003300300544", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/AddVehicleDetailedInfo " },
            { code: "003300300545", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/UpDateVehicleDetailedInfo " },
            { code: "003300300546", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangJianKong/GetVehicleInfoYeHu" },
            { code: "003300300547", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangJianKong/GetVehicleInfoCheDui" },
            { code: "003300300548", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangJianKong/GetVehicleInfoByYeHu " },
            { code: "003300300549", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangJianKong/AddVehicleMonitoring " },
            { code: "003300300550", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiangJianKong/DelVehicleMonitoring " },
            //服务商关联信息
            { code: "006600200041", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/AddFuWuShangGuanLianXinXi " },
            //{ code: "003300300552", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/EditFuWuShangGuanLianXinXi " },
            //{ code: "003300300553", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/DeleteFuWuShangGuanLianXinXi " },
            //{ code: "003300300554", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/QueryFuWuShangGuanLianXinXi " },
            //{ code: "003300300555", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/GetFuWuShangGuanLianXinXi " },

            { code: "006600200001", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/Query " },
            { code: "006600200018", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/Update " },
            //根据组织代码获取组织基本信息
            { code: "006600200066", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/GetByOrgCode " },
            //根据组织代码获取企业确认状态
            { code: "006600200067", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/YeHu/GetYeHuConfirmInfoStatus " },

            //获取终端配置信息
            //{ code: "006600200068", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/GetZhongDuanXinXi " },
            //修改终端配置信息
            //{ code: "006600200069", ver: '1.0', url: "http://localhost:1420/api/GPSDAGL/CheLiang/UpdateZhongDuanXinXi " },


            
        ],
        GetFilePath: "http://10.0.64.235:7007/api/ServiceGateway/GetFile"
    });