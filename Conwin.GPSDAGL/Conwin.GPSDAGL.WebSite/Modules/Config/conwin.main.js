var pathStaticResource = "http://10.0.64.247:11000/StaticResource/",
    pathStaticTheme1 = pathStaticResource + "Theme1/",
    pathStaticFramework = pathStaticResource + "Framework/",
    pathStaticAssets = pathStaticResource + "Framework/Assets/",
    pathStaticPlugins = pathStaticResource + "Framework/Assets/global/plugins/",
    pathComponent = "/Modules/Component/";
pathMapComponent = "/Modules/GPSVMS/SJFXGZSZ/common/";

require.config({
    baseUrl: '',
    paths: {
        "system": "/Modules/Config/conwin.system",
        "menu": pathComponent + "conwin.menu",
        "header": pathComponent + "conwin.header",
        "userinfo": pathComponent + "conwin.userinfo",
        "changepwd": pathComponent + "conwin.changepwd",
        'selectunit': pathComponent + "conwin.selectunit",
        "common": pathStaticTheme1 + "Js/conwin.common",
        "helper": pathStaticTheme1 + "Js/conwin.helper",
        "metronic": pathStaticAssets + "global/scripts/metronic",
        "layout": pathStaticAssets + "admin/layout/scripts/layout",
        "profile": pathStaticAssets + "admin/pages/scripts/profile",
        "bootstrap": pathStaticPlugins + "bootstrap/js/bootstrap.min",
        "jquery": pathStaticPlugins + "jquery.min",
        "jquery-migrate": pathStaticPlugins + "jquery-migrate.min",
        "jquery-ui": pathStaticPlugins + "jquery-ui/jquery-ui.min",
        "cokie": pathStaticPlugins + "jquery.cokie.min",
        "datatables": pathStaticPlugins + "datatables/media/js/jquery.dataTables.min",
        "dataTables.bootstrap": pathStaticPlugins + "datatables/plugins/bootstrap/dataTables.bootstrap",
        "bootbox": pathStaticPlugins + "bootbox/bootbox.min",
        "bootstrap-modal": pathStaticPlugins + "bootstrap-modal/js/bootstrap-modal",
        "bootstrap-toastr": pathStaticPlugins + "bootstrap-toastr/toastr.min",
        "bootstrap-modalmanager": pathStaticPlugins + "bootstrap-modal/js/bootstrap-modalmanager",
        'bootstrap3-typeahead': pathStaticPlugins + 'bootstrap3-typeahead',
        "blockui": pathStaticPlugins + "jquery.blockui.min",
        "uniform": pathStaticPlugins + "uniform/jquery.uniform.min",
        "slimscroll": pathStaticPlugins + "jquery-slimscroll/jquery.slimscroll.min",
        "respond": pathStaticPlugins + "respond.min",
        "excanvas": pathStaticPlugins + "excanvas.min",
        "sparkline": pathStaticPlugins + "jquery.sparkline.min",
        "pin": pathStaticPlugins + "jquery.pin",
        'json2': pathStaticFramework + 'Plugins/json2',
        'md5': pathStaticFramework + 'Plugins/md5',
        "bootstrap-datepicker": pathStaticPlugins + "bootstrap-datepicker/js/bootstrap-datepicker",
        "bootstrap-datetimepicker": pathStaticPlugins + "bootstrap-datetimepicker/js/bootstrap-datetimepicker.min",
        'bootstrap-datepicker.zh-CN': pathStaticPlugins + 'bootstrap-datepicker/js/locales/bootstrap-datepicker.zh-CN',
        'bootstrap-datetimepicker.zh-CN': pathStaticPlugins + 'bootstrap-datetimepicker/js/locales/bootstrap-datetimepicker.zh-CN',
        'login': pathComponent + "conwin.login",
        'jstree': pathStaticPlugins + 'jstree/dist/jstree.min',
        'newjstree': pathStaticPlugins + 'jstree/dist/jstree.min.3.3.5',
        'crypto': pathStaticFramework + 'plugins/crypto/crypto',
        'valid': pathStaticTheme1 + 'UIControls/conwin.valid',
        'warning': pathStaticTheme1 + 'UIControls/conwin.warning',
        "popdialog": pathStaticTheme1 + "UIControls/conwin.popdialog",
        //"customtable": pathStaticTheme1 + "UIControls/conwin.customtable",
        "customtable": pathComponent + "conwin.customtable",
        "areas": pathStaticTheme1 + "UIControls/conwin.areas.select",
        "prevNextpage": pathStaticTheme1 + "UIControls/conwin.prenextpage",
        'toast': pathStaticTheme1 + 'UIControls/conwin.toast',
        "tipdialog": pathStaticTheme1 + "UIControls/conwin.tipdialog",
        'textbox': pathStaticTheme1 + 'UIControls/conwin.textbox',
        'textarea': pathStaticTheme1 + 'UIControls/conwin.textarea',
        'radio': pathStaticTheme1 + 'UIControls/conwin.radio',
        'checkbox': pathStaticTheme1 + 'UIControls/conwin.checkbox',
        'button': pathStaticTheme1 + 'UIControls/conwin.button',
        'dropdown': pathStaticTheme1 + 'UIControls/conwin.dropdown',
        "selectcity": pathStaticTheme1 + "UIControls/conwin.selectcity",
        "formcontrol": pathStaticTheme1 + 'UIControls/conwin.formcontrol',
        "tableheadfix": pathStaticTheme1 + "UIControls/conwin.tableheadfix",
        'permission': pathStaticTheme1 + "UIControls/conwin.permission.1",
        "amap": "http://webapi.amap.com/maps?		v=1.4.5&key=d132a01ae68b7c80fb38a26d874f9c38&&plugin=AMap.Scale,AMap.OverView,AMap.ToolBar,AMap.Geolocation,AMap.RangingTool,AMap.Geocoder,AMap.DistrictSearch,AMap.Driving&callback=init",
        'map': pathStaticTheme1 + 'UIControls/conwin.gdmap',
        'CbersMap': pathMapComponent + 'CbersMap/javascript/conwin.components.gis',
        'Cbers': pathMapComponent + 'CbersMap/javascript/CbersMap',
        'vms': pathMapComponent + 'CbersMap/javascript/vms',
        'mapUI': 'http://webapi.amap.com/ui/1.0/main-async',
        'bootstrap-switch': pathStaticPlugins + 'bootstrap-switch/js/bootstrap-switch.min',
        'fileupload': pathStaticResource + "Theme1/UIControls/conwin.fileupload",
        "searchbox": pathStaticResource + "/Theme1/UIControls/conwin.searchbox.1",
        "select2cn": pathStaticPlugins + "select2/4.0.6_rc.1/js/i18n/zh-CN",
        "select2": pathStaticPlugins + "select2/4.0.6_rc.1/js/select2.full",
        'filelist': pathStaticResource + "Theme1/UIControls/conwin.filelist",
        'selectCity2': '/Modules/GPSDAGL/selectCity/js/conwin.selectcity2',
        'draggabilly': pathStaticFramework + 'plugins/draggabilly/draggabilly.pkgd.min',
        'laydate': pathStaticFramework + 'plugins/laydate/laydate',
        'bluebird': pathStaticFramework + 'plugins/bluebird/bluebird.min',
        'signlar': pathComponent + 'signlar',
        'hubs': pathComponent + 'signlarhub',
        'echarts': '../../../Modules/GPSVMS/QYJKGJHF/Js/echarts',
        'layer': "/Modules/Common/Js/layer/layer",
        'gpsjcfxcommon': "/Modules/GPSTJFX/Js/gpsjcfxcommon"
    },
    shim: {
        'vms': {
            deps: ['jquery', 'Cbers']
        },
        'CbersMap': {
            deps: ['jquery', 'blockui', 'Cbers', 'vms']
        },
        "helper": {
            deps: ['system', 'cokie'],
        },
        "jquery-migrate": ['jquery'],
        "pin": ['jquery'],
        "jquery-ui": ['jquery', 'jquery-migrate'],
        "uniform": ['jquery', 'jquery-migrate'],
        "layout": {
            deps: ['jquery', 'jquery-migrate', 'metronic'],
            exports: "Layout"
        },
        "queueutility": ['jquery', 'jquery-migrate'],
        'dataTables.bootstrap': {
            deps: ['jquery', 'jquery-migrate', 'bootstrap', 'datatables']
        },
        "customtable": {
            deps: ['dataTables.bootstrap', 'helper', 'uniform']
        },
        "blockui": {
            deps: ['jquery', 'bootstrap-toastr'],
            exports: 'jquery.blockui'
        },
        "metronic": {
            deps: ['jquery', 'jquery-migrate', 'bootstrap', 'bootstrap-toastr', 'blockui', 'slimscroll'],
            exports: "Metronic"
        },
        "bootstrap": ['jquery', 'jquery-migrate'],
        "bootstrap-modal": {
            deps: ['jquery', 'jquery-migrate', 'bootstrap'],
            exports: "Modal"
        },
        "bootstrap-modalmanager": {
            deps: ['jquery', 'jquery-migrate', 'bootstrap'],
            exports: "ModalManager"
        },
        "bootbox": {
            deps: ['jquery', 'jquery-migrate', 'bootstrap'],
            exports: "bootbox"
        },
        "popdialog": {
            deps: ['jquery', 'jquery-migrate', 'bootstrap']
        },
        "tipdialog": {
            deps: ['jquery', 'jquery-migrate', 'bootstrap', "bootstrap-modal", "bootstrap-modalmanager"]
        },
        "slimscroll": {
            deps: ['jquery']
        },
        "bootstrap-datepicker": {
            deps: ['jquery']
        },
        "bootstrap-datetimepicker": {
            deps: ['jquery']
        },
        "bootstrap-datepicker.zh-CN": {
            deps: ['bootstrap-datepicker']
        },
        "bootstrap-datetimepicker.zh-CN": {
            deps: ['bootstrap-datetimepicker']
        },
        "draggabilly": {
            deps: ['jquery']
        },
        "signlar": {
            deps: ["jquery"],
            exports: ""
        },
        "hubs": {
            deps: ["signlar"],
            exports: ""
        },
        'searchbox': {
            deps: ['jquery', 'select2']
        },
        'select2cn': {
            deps: ['select2']
        },
        "layer": {
            deps: ['jquery'],
            exports: "layer"
        }
    }
});
require(['jquery', 'helper', 'layer'], function ($, t) {
    layer.config({
        path: '/Modules/Common/Js/layer'
    });

    if (t.BrowserVersion.browser == "IE" && t.BrowserVersion.version == 8) {
        require(['respond', 'excanvas']);
    };
    //无需登录页面需加标记：<meta property="login" content="false" />
    if ($("meta[property=login]").attr('content') !== 'false') {
        t.PageLogin();
    };
    if ($('.page-sidebar-icon').length > 0 && $('.page-sidebar-menu').length == 0) {
        t.UserMenu();
        setTimeout(SetUserMenuPageLink, 2 * 1000);
    };
    if ($('.page-header-inner').length > 0 && $('.page-sidebar-menu .row').length == 0) {
        t.Header();
    };

    function PageLogin() {
        try {
            var token = t.GetToken();
            var tokenTime = new Date($.cookie('tokenTime'));
            var lastOperateTime = new Date($.cookie('lastOperateTime'));
            var now = new Date();

            if (token == "") {
                redirtDefault();
                return;
            }

            //当前时间处于 TOKEN时间 +2小时之后 ，和最后操作时间2小时内
            if ((now.getTime() - tokenTime.getTime() > 0) &&
                now.getTime() - lastOperateTime.getTime() < 0) {
                //延长TOKEN时间
                t.Ajax('00000030005', token, function (response) {
                    if (response.publicresponse.statuscode === 0 && response.body === "true") {
                        t.SetToken(token);
                    } else {
                        redirtDefault();
                    }
                }, false);
            } else if ((now.getTime() - tokenTime.getTime() > 0) &&
                now.getTime() - lastOperateTime.getTime() > 0) {
                redirtDefault();
            }
        } catch (error) {
            redirtDefault();
        }
    };

    //指向默认登录页面
    function redirtDefault() {
        sessionStorage.clear();
        var w = top || this;
        w.location.href = $.cookie('LoginUrl') + '?ReturnUrl=' + encodeURIComponent(GetUrlRelativePath(w.location.href));
    }

    //获取相对路径
    function GetUrlRelativePath() {
        var url = document.location.toString();
        var arrUrl = url.split("//");
        var start = arrUrl[1].indexOf("/");
        var relUrl = arrUrl[1].substring(start);
        if (relUrl.indexOf("?") != -1) {
            relUrl = relUrl.split("?")[0];
        }
        return relUrl;
    }
    //控制某些菜单跳转方式（新页面打开）
    function SetUserMenuPageLink() {
        var customLinkPage = [];
        customLinkPage.push("/Modules/GPSVMS/QYJKCLJK/mapList.html");//车辆定位
        customLinkPage.push("/Modules/CLSSAQJCKB/Index.html");//重点车辆风险监测
        customLinkPage.push("/Modules/VideoMonitor/page2/index.html");//视频看板
        customLinkPage.push("/Modules/GPSAQDAGL/QiYe/List.html");//电子档案
        customLinkPage.push("/Modules/GPSAQDAGL/QiYe/List.html");//安全管理档案评分统计
        customLinkPage.push("/Modules/KuaJingCheLiang/JianKongMapList.html");//跨境车辆定位

        for (var i = 0; i < customLinkPage.length; i++) {
            $(".page-sidebar-menu").find("a[href='" + customLinkPage[i] + "']").attr("target", "_blank")
        }

    }
});
