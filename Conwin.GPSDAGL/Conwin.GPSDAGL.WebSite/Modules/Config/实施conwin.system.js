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
    SysId: '127B9AAB-2AFB-496E-A630-9741E55B4E98',
    AppId: '938B7DFE-5DEA-4BC4-AFE3-1095385AB598',
    SysName: '电子客票管理',
    AppName: '',
    IsTest: true,
    AdaptLogin: false,
    DefaultUrl: '/Modules/Template/List.html',
    LoginUrl: '/Modules/Template/Index.html',
    LogoutUrl: '/Modules/Template/Logout.html',
    Exponent: '010001',         //加密需要，Exponent参数
    Modulus: 'A85A7F6667773D8FB7013C482CDB5EFCC06A84E218454204B86CAF42313431116FBBDE0020B62EE91E970E6991340B34ED2A8C51B00B768B934BEF6E584528A7097DAD560C41F164A2A7AD8706E41C7346B5DFDD1D0E204A373A352F255BDFDD8DA4917551F3835FCEC56C72FDC8B38A783FEA8937E2C0A5B2D80750F3B7D3A9',        //加密需要，Modulus参数
    ServerAgent: "http://10.0.64.249:7007/api/ServiceGateway/DataService",
    ServiceCodeTable: [
    //{ code: "00000030001", ver: '1.0', url: "http://10.0.64.249:7005/Login" },
    //{ code: "00000030002", ver: '1.0', url: "http://10.0.64.249:7005/GetUserInfo" },
    //{ code: "00000030003", ver: '1.0', url: "http://10.0.64.249:7005/GetPermission" },
    //{ code: "00000030004", ver: '1.0', url: "http://10.0.64.249:7005/Logout" },
    //{ code: "00000030005", ver: '1.0', url: "http://10.0.64.249:7005/GetValidToken" },
    //{ code: "00000030006", ver: '1.0', url: "http://10.0.64.249:7005/GetUserMenus" },
    //{ code: "00000030007", ver: '1.0', url: "http://10.0.64.249:7005/GetOrgList" },
    //{ code: "00000030008", ver: '1.0', url: "http://10.0.64.249:7005/GetPermissionRegisterInfos" },
    { code: "00000030011", ver: '1.0', url: "http://10.0.64.249:7005/ValidCode" },
    //{ code: "00000010001", ver: '1.0', url: "http://10.0.64.249:7005/GetUserAccount" },
    //{ code: "00000010004", ver: '1.0', url: "http://10.0.64.249:7005/ChangUserPassword" },
    //{ code: "00000020001", ver: '1.0', url: "http://10.0.64.249:7006/GetSystemInfo" },
    //{ code: "00000020002", ver: '1.0', url: "http://10.0.64.249:7006/GetSystemMenus" },  
    //{ code: "00000020003", ver: '1.0', url: "http://10.0.64.249:7006/GetApplicationInfo" },
    //{ code: "00000020004", ver: '1.0', url: "http://10.0.64.249:7006/GetProvinces" },
    //{ code: "00000020005", ver: '1.0', url: "http://10.0.64.249:7006/GetCitys" },
    //{ code: "00000020006", ver: '1.0', url: "http://10.0.64.249:7006/GetDistricts" },
    //{ code: "00000020007", ver: '1.0', url: "http://10.0.64.249:7006/GetTowns" },
    //{ code: "00000020008", ver: '1.0', url: "http://10.0.64.249:7006/GetVillages" },
    //{ code: "00000020009", ver: '1.0', url: "http://10.0.64.249:7006/GetUIPage" },
    //{ code: "00000020010", ver: '1.0', url: "http://10.0.64.249:7006/GetUIControlsByPageId" },
    //{ code: "00020001", ver: '1.0', url: "http://10.0.64.249:7006/GetXianLu" },
    //{ code: "00020002", ver: '1.0', url: "http://10.0.64.249:7006/SaveXianLu" },
    //{ code: "00000070007", ver: '1.0', url: "http://10.0.64.249:7002/api/ConfigCenter/MQClientConnection/Query" }
    ]
});