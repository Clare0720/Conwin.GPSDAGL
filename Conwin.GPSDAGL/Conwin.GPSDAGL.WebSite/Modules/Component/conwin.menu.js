define(['jquery', 'layout', 'metronic', 'tipdialog', 'system', 'tableheadfix'], function ($, layout, Metronic, tipdialog, SystemConfig, tableheadfix) {

    var cwMenu = {};

    var currenturl = window.location.pathname;

    cwMenu.SetMenu = function (result) {
        if (result.publicresponse.statuscode == 0) {
            if (result.body != null && result.body.SubMenu.length > 0) {

                for (var i = 0; i < result.body.SubMenu.length; i++) {
                    ResetMenu(result.body.SubMenu[i]);
                }
                for (var i = 0; i < result.body.SubMenu.length; i++) {
                    if (searchModulesMenu(result.body.SubMenu[i]) == true) {
                        setShowMenu(result.body.SubMenu[i]);
                        layout.init();
                        ResetPage();
                    }
                }
            }
        }
        else {
            tipdialog.errorDialog(result.publicresponse.message);
        }
    };

    var ResetMenu = function (menuData) {
        if (!!menuData) {
            if (!!menuData.Url && menuData.Url.toLowerCase().indexOf('appid=' + SystemConfig.AppId.toLowerCase()) >= 0 && menuData.Url.toLowerCase().indexOf('url=') >= 0) {
                menuData.Url = menuData.Url.substring(menuData.Url.toLowerCase().indexOf('url=') + 4);
            }
            if (!!menuData.SubMenu && menuData.SubMenu.length > 0) {
                for (var i = 0; i < menuData.SubMenu.length; i++) {
                    ResetMenu(menuData.SubMenu[i]);
                }
            }
        }
    }

    var searchModulesMenu = function (menuData) {
        var isCurrentModulesMenu = false;
        if (menuData.Url == currenturl) {
            isCurrentModulesMenu = true;
        }
        else if (menuData.SubMenu.length > 0) {
            for (var i = 0; i < menuData.SubMenu.length; i++) {
                if (searchModulesMenu(menuData.SubMenu[i]) == true) {
                    isCurrentModulesMenu = true;
                    break;
                }
            }
        }
        return isCurrentModulesMenu;
    }

    //设置菜单
    var setShowMenu = function (menuData) {
        if (menuData != null) {
            if (menuData.SubMenu != null && menuData.SubMenu.length > 0) {
                $('.page-sidebar-basic').html('');
                var menuelement = {};
                menuelement.iconmenuelement = "<ul class='page-sidebar-menu'>";
                menuelement.basicmenuelement = "<ul class='page-sidebar-menu'>";
                for (var i = 0; i < menuData.SubMenu.length; i++) {
                    var startclassname = "";
                    var lastclassname = "";
                    if (i == 0) {
                        startclassname = " start ";
                    }
                    if (i == menuData.SubMenu.length - 1) {
                        lastclassname = " last ";
                    }

                    var activeclass = "";
                    if (currenturl == menuData.SubMenu[i].Url) {
                        activeclass = " active ";
                    }

                    menuelement.iconmenuelement += "<li class='" + startclassname + lastclassname + activeclass + "'>" +
                        "<a href='" + menuData.SubMenu[i].Url + "' data-rootid='" + menuData.SubMenu[i].Id + "'>" +
                        "<i class='" + menuData.SubMenu[i].Icon + "'></i>" +
                        "</a>" +
                        "</li>";

                    var childMenu = setChildMenu(menuData.SubMenu[i], currenturl, 1, startclassname, lastclassname);
                    menuelement.basicmenuelement += childMenu.menuelement;
                }

                $('.page-sidebar-icon').append(menuelement.iconmenuelement);
                $('.page-sidebar-icon').append("</ul>");

                $('.page-sidebar-basic').append(menuelement.basicmenuelement);

                $('.page-sidebar-basic').append("</ul>");
                $('.page-sidebar-basic .page-sidebar-menu').hide().fadeIn('slow');
            }


            var systemnameelement = '<div class="system-name">' +
                '<span class="title">' + menuData.Title + '</span>' +
                '<a href="javascript:;" class="fixedMenu-icon">' +
                '<i class="fa fa-bars"></i>' +
                '</a>' +
                '</div>';
            $('.page-sidebar-basic').prepend(systemnameelement);

            var backelement = '<div class="page-back">' +
                '<a href="' + (SystemConfig.DefaultUrl || menuData.Url) + '" title="返回首页">' +
                '<i class="iconfont icon-fanhuishouye"></i>' +
                '</a>' +
                '</div>';

            $('.page-sidebar-basic').append(backelement);
        }
    };

    //设置子菜单
    var setChildMenu = function (menuItem, currentUrl, isMoudle, startclassname, lastclassname) {
        var childMenu = { menuelement: "", menuactive: false };
        if (menuItem.SubMenu != null && menuItem.SubMenu.length > 0) {
            $.each(menuItem.SubMenu, function (i, childItem) {
                var returnresult = setChildMenu(childItem, currentUrl, 0, "", "");
                childMenu.menuelement += returnresult.menuelement;
                if (returnresult.menuactive) {
                    childMenu.menuactive = returnresult.menuactive;
                }
            });
        }
        var thisMenu = { menuelement: "", menuactive: false };
        var activeclass = "";
        var openclass = "";
        if (currentUrl == menuItem.Url || childMenu.menuactive == true) {
            activeclass = " active ";
            thisMenu.menuactive = true;
        }
        if (childMenu.menuactive == true) {
            openclass = " open ";
        }

        thisMenu.menuelement = "<li class='" + startclassname + lastclassname + (menuItem.ClassName == null ? '' : menuItem.ClassName) + activeclass + openclass + "'>" +
            "<a href='" + menuItem.Url + "' " + (isMoudle == 1 ? "rootid='" + menuItem.Id + "'" : "") + ">" +
            "<i class='" + menuItem.Icon + "'></i>" +
            getMenuTitle(menuItem.Title, isMoudle, thisMenu.menuactive) +
            ((menuItem.SubMenu != null && menuItem.SubMenu.length > 0) ? "<span class='arrow " + openclass + "'></span>" : "") +
            "</a>" + (childMenu.menuelement != '' ? "<ul class='sub-menu'>" + childMenu.menuelement + "</ul>" : '') +
            "</li>";
        return thisMenu;
    };

    var getMenuTitle = function (title, isMoudle, active) {
        var menustring = "";
        if (isMoudle == 1) {
            menustring = '<span class="title">' + title + '</span>';

            if (active) {
                menustring += '<span class="selected"></span>';
            }
        }
        else {
            //menustring = title;
            menustring = '<span class="title">' + title + '</span>';
        }
        return menustring;
    };

    var ResetPage = function () {

        //窗口变化自适应
        Metronic.addResizeHandler(handleFixedSidebar);

        Metronic.addResizeHandler(function () {
            var height = Metronic.getViewPort().height;

            $('.page-sidebar.page-sidebar-icon').css('height', height - 64 + 'px');
            $('.page-sidebar-basic').css('height', height - 64 + 'px');

        });

        function handleFixedSidebar() {
            var menu = $('.page-sidebar-menu');

            Metronic.destroySlimScroll(menu);
            menu.attr("data-height", _calculateFixedSidebarViewportHeight() - $('.page-back').outerHeight());
            Metronic.initSlimScroll(menu);
        };

        function _calculateFixedSidebarViewportHeight() {
            var sidebarHeight = Metronic.getViewPort().height - $('.page-header').outerHeight();
            sidebarHeight = sidebarHeight - $('.system-name').outerHeight();
            sidebarHeight = sidebarHeight - $('.page-footer').outerHeight();

            return sidebarHeight;
        }

        function initListPage() {
            //调整图标菜单高度
            handleFixedSidebar();

            var state = true;

            //左侧菜单滑进
            $('.page-sidebar-icon').mouseover(function () {
                $('.page-sidebar-closed .page-sidebar-basic').css({ 'left': '0px' });
                $('.page-sidebar-closed .page-sidebar-basic').css('minWidth', '235px');
            });


            //固定菜单
            $('.fixedMenu-icon').click(function () {
                if (state) {
                    state = false;
                    $('body').addClass('page-sidebar-closed');
                    $('.page-sidebar-basic').bind('mouseleave', accountMenuMouseLeave);
                } else {
                    state = true;
                    $('body').removeClass('page-sidebar-closed');
                    $('.page-sidebar-basic').unbind('mouseleave');
                }
                tableheadfix.ResetFix();
            });

            if ($('.page-sidebar-menu').attr("moduleIndex")) {
                $('.page-sidebar-menu ul').remove();
                $('.page-sidebar-menu li span.arrow').remove();
            }

            $('.page-sidebar-menu').delegate('.open', 'click', function () {
                $(this).removeClass('open');
            });
        }

        function accountMenuMouseLeave() {
            $('.page-sidebar-basic').css('left', '-9999px');
        }

        initListPage();
    };

    return cwMenu;
});