define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'system', 'helper', 'tipdialog', 'userinfo', 'changepwd'], function ($, SystemConfig, helper, tipdialog, userinfo, changepwd) {
        var InitPage = function () {
            $('.page-header .caption').text(SystemConfig.SysName);
            InitData();
            $('.tile').click(function () {
                var src = $(this).children('a').attr('href');
                if (!!src) {
                    window.location.href = src;
                }
                return false;
            });
            $('.tuichuxitong').click(function () {
                helper.Logout();
            });
            $('.page-header .info').on('click', '.UserInfo', function () {
                userinfo.ShowUserInfo();
            });
            $('.page-header .info').on('click', '.ChangePwd', function () {
                changepwd.ShowChangePwd();
            });
        };
        var InitData = function () {
            SetUserInfo();
            SetTiles();
        };
        var SetTiles = function () {
            helper.Ajax("00000030006", { SysId: SystemConfig.SysId, AppId: SystemConfig.AppId, Token: helper.GetToken() }, function (result) {
                if (result.publicresponse.statuscode == 0) {
                    if (result.body != null && result.body.SubMenu.length > 0) {
                        var ModuleItems = $('.tiles .tile');
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
                var keys = $(item).children('a').data('key');
                $.each(keys.split(','), function (k, keyItem) {
                    if (keyItem == menuData.Code) {
                        $(item).removeClass('bg-disabled');
                        $(item).children('a').attr('href', GetTilesUrl(menuData));
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
        // var searchModulesMenu = function (menuData, ModuleItems) {
        //     $.each(ModuleItems, function (i, item) {
        //         if ($(item).children('a').data('key') == menuData.Code) {
        //             $(item).removeClass('bg-disabled');
        //             $(item).children('a').attr('href', menuData.Url);
        //         }
        //     });
        //     if (menuData.SubMenu.length > 0) {
        //         for (var i = 0; i < menuData.SubMenu.length; i++) {
        //             searchModulesMenu(menuData.SubMenu[i], ModuleItems);
        //         }
        //     }
        // }
        var SetUserInfo = function () {
            var UserInfo = helper.GetUserInfo();
            if (!!UserInfo) {
                $('.username').text(UserInfo.UserName);
                $('.organizationname').text(UserInfo.OrganizationName);
            }
        }
        InitPage();
    })
})
