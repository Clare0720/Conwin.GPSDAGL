define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'system', 'helper', 'tipdialog','uniform'], function ($, SystemConfig, helper, tipdialog) {

        var InitPage = function () {
            var userInfo = helper.GetUserInfo();
            if (typeof (userInfo.Id) == 'undefined') {
                window.location.href = '/Modules/Home/GpsTwLogin.html';
            }
            $('.page-header .caption').text(SystemConfig.SysName);
            InitData();

            $('.tuichuxitong').click(function () {
                helper.AdaptLogout(function () {
                    $.cookie('tokenTime', '', {
                        path: "/"
                    });
                    $.cookie('lastOperateTime', '', {
                        path: "/"
                    });
                    $.cookie('token', '', {
                        path: "/"
                    });
                    sessionStorage.clear();
                    if ($.cookie('LoginUrl') != null && $.cookie('LoginUrl') != '' && typeof $.cookie('LoginUrl') != 'undefined') {
                        this.location.href = $.cookie('LoginUrl');
                    }
                    else {
                        this.location.href = SystemConfig.LoginUrl;
                    }
                });
            });


            $('.page-header .info').on('click', '.ChangePwd', function () {
                changepwd.ShowChangePwd();
            });

            $("a[data-key='安全日报']").attr('href', 'http://zgservice.gdunis.com/Modules/WeiXin/index.html#/ServiceCenter/SmartService/DataReportList/DaoLuYunShuAnQuanFengKongPC?token=' + helper.GetToken());
            $("a[data-key='联网联控']").attr('href', 'http://zgservice.gdunis.com/Modules/WeiXin/index.html#/ServiceCenter/SmartService/DataReportList/LianWangLianKongPC?token=' + helper.GetToken());

        };

        var InitData = function () {
            SetUserInfo();
            SetTiles();
        };

        var SetTiles = function () {
            helper.Ajax("00000030006", { SysId: SystemConfig.SysId, AppId: SystemConfig.AppId, Token: helper.GetToken() }, function (result) {
                if (result.publicresponse.statuscode == 0) {
                    if (result.body != null && result.body.SubMenu.length > 0) {
                        var ModuleItems = $('a[data-key]');
                        for (var i = 0; i < result.body.SubMenu.length; i++) {
                            searchModulesMenu(result.body.SubMenu[i], ModuleItems);
                        }
                    }
                }
                else {
                    tipdialog.errorDialog(result.publicresponse.message);
                }
            });
        }

        var searchModulesMenu = function (menuData, ModuleItems) {
            $.each(ModuleItems, function (i, item) {
                var keys = $(item).data('key');
                $.each(keys.split(','), function (k, keyItem) {
                    if (keyItem == menuData.Code) {
                        $(item).removeClass('bg-disabled');
                        $(item).attr('href', GetTilesUrl(menuData));
                    }
                })
            });
            if (menuData.SubMenu.length > 0) {
                for (var i = 0; i < menuData.SubMenu.length; i++) {
                    searchModulesMenu(menuData.SubMenu[i], ModuleItems);
                }
            }
        };

        var GetTilesUrl = function (menu) {
            var url = "";
            if (menu.SubMenu.length > 0) {
                url = GetTilesUrl(menu.SubMenu[0]);
            }
            else {
                url = menu.Url;
            }
            return url;
        }


        var SetUserInfo = function () {
            var UserInfo = helper.GetUserInfo();
            if (!!UserInfo) {
                $('.username').text(UserInfo.UserName);
            }
        }

        InitPage();
    })
})
