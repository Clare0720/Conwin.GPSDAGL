define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'searchbox', 'customtable', 'bootstrap-datepicker.zh-CN', 'permission'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system, selectcity) {
            var initPage = function () {
                //初始化table
                initlizableTable();
                //查询
                $('#btnSearch').click(function (e) {
                    e.preventDefault();
                    $("#tb_Template").CustomTable("reload");
                });
                //重置
                $("#btnReset").click(function (e) {
                    e.preventDefault();
                    $('.searchpanel-form').find('input[type=text]:not(:disabled), select:not(:disabled)').val('');
                });
                //新增
                $("#btnCreate").click(function (e) {
                    e.preventDefault();
                    //TODO:编写逻辑
                    popdialog.showIframe({
                        'url': 'Add.html',
                        head: false
                    });
                });
                //启用
                $("#btnOn").click(function (e) {
                    e.preventDefault();
                    On_Off(true);
                });
                //停用
                $("#btnOff").click(function (e) {
                    e.preventDefault();
                    On_Off(false);
                });
                //更新
                $('#btnEdit').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要更新的行');
                        return false;
                    }
                    //TODO:编写逻辑
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                    });
                    $('#hdIDS').val(ids.join(','));
                    popdialog.showIframe({
                        'url': 'Edit.html',
                        head: false
                    });
                });
                //删除
                $('#btnDel').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要删除的行');
                        return false;
                    }
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                    });
                    //TODO:编写逻辑
                    /*tipdialog.confirm("确定要删除选中的记录？", function (r) {
                        if (r) {
                            //TODO:调作废接口
                            helper.Ajax("003300300080", ids, function (data) {
                                if (data.body) {
                                    toast.success("删除成功");
                                    $("#tb_Template").CustomTable("reload");
                                } else {
                                    if (data.publicresponse.message == '删除失败,无法删除已安装终端.') {
                                        tipdialog.errorDialog('删除失败,无法删除已安装终端.');
                                    } else {
                                        tipdialog.errorDialog('删除失败');
                                    }
                                }
                            }, false);
                        }
                    });*/
                    //TODO:调作废接口
                    helper.Ajax("003300300526", ids, function (data) {
                        if (data.body) {
                            tipdialog.confirm("确定要删除选中的记录？", function (r) {
                                if (r) {
                                    toast.success("删除成功");
                                    $("#tb_Template").CustomTable("reload");
                                }
                            });
                        } else {
                            if (data.publicresponse.message == '删除失败,无法删除已安装终端.') {
                                tipdialog.errorDialog('删除失败,无法删除已安装终端.');
                            } else {
                                tipdialog.errorDialog('删除失败');
                            }
                        }
                    }, false);


                });
                //查看
                $('#btnView').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要查看的行');
                        return false;
                    }
                    //TODO:编写逻辑
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                    });
                    $('#hdIDS').val(ids.join(','));
                    popdialog.showIframe({
                        'url': 'View.html',
                        head: false
                    });
                });
                //个性化代码块
                //region
                selectCity();
                //endregion
            };
            function initlizableTable() {
                $("#tb_Template").CustomTable({
                    ajax: helper.AjaxData("003300300521",///"00020003"为接口服务地址，需要在/Config/conwin.system.js中配置
                        function (data) {
                            var pageInfo = { Page: parseInt(data.start / data.length + 1), Rows: data.length };
                            for (var i in data) {
                                delete data[i];
                            }
                            var para = $('.searchpanel-form').serializeObject();
                            $('.searchpanel-form').find('[disabled]').each(function (i, item) {
                                para[$(item).attr('name')] = $(item).val();
                            });
                            pageInfo.data = para;
                            $.extend(data, pageInfo);
                        }, null),
                    single: false,
                    filter: true,
                    ordering: true, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [
                        {
                            render: function (data, type, row) {
                                return '<input type=checkbox class=checkboxes />';
                            }
                        },
                        { data: 'SheBeiXingHao' },
                        { data: 'ShengChanChangJia' },
                        { data: 'ChangJiaBianHao' },
                        { data: 'XingHaoBianMa' },
                        { data: 'ShiYongCheXing' },
                        { data: 'DingWeiMoKuai' },
                        { data: 'TongXunMoShi' },
                        { data: 'GuoJianPiCi' },
                        { data: 'ZhongDuanBianMa' },
                        {
                            data: 'ZhuangTai', render: function (data, type, row) {
                                return data == 1 ? "启用" : "停用";
                            }
                        }
                    ],
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        tableheadfix.ResetFix();
                    }
                });
                tableheadfix.InitFix(system.OnlyTableFix);
            };
            function On_Off(status) {
                var ServerCode = status ? '003300300528' : '003300300527';// 启用：停用
                var flag = status ? 1 : 2;
                var rows = $("#tb_Template").CustomTable('getSelection'), ids = [];
                var msg = status ? "启用" : "停用";
                var zhuangTai = true;
                if (rows == undefined) {
                    tipdialog.errorDialog('请选择需要'+msg+'的行');
                    return false;
                }
                if (rows.length > 1) {
                    tipdialog.errorDialog('只能选择一行!');
                    return;
                }
                $(rows).each(function (i, item) {
                    if (item.data.ZhuangTai == flag) {
                        zhuangTai = false;
                        return;
                    }
                    ids.push(item.data.Id);
                });
                if (zhuangTai) {
                    tipdialog.confirm("确定要" + msg + "选中的记录?", function (r) {
                        if (r) {
                            //TODO:调作Off接口
                            helper.Ajax(ServerCode, ids, function (data) {
                                if (data.body) {
                                    toast.success(status ? "启用" : "停用" + "成功");
                                    $("#tb_Template").CustomTable("reload");
                                } else {
                                    tipdialog.errorDialog(status ? "启用" : "停用" + '失败');
                                }
                            }, false);
                        }
                    });
                } else {
                    tipdialog.errorDialog('已是' + msg + '状态！');
                }
                //TODO:编写逻辑
                
            }
            //个性化代码块
            //region
            function selectCity() {
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#XiaQuShi, #XiaQuXian').empty().append(defaultOption);
                selectcity.setXiaQu('00000020005', { "Province": "广东" }, '#XiaQuShi', 'GetCityList', 'CityName');
                $('#XiaQuShi').change(function () {
                    $('#XiaQuXian,#XiaQuZhen').empty().append(defaultOption);
                    var data = { "City": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化区县下拉框
                        selectcity.setXiaQu('00000020006', data, '#XiaQuXian', 'GetDistrictList', 'DistrictName');
                    }
                });
                $('#XiaQuXian').change(function () {
                    $('#XiaQuZhen').empty().append(defaultOption);
                    var data = { "District": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化区镇下拉框
                        selectcity.setXiaQu('00000020007', data, '#XiaQuZhen', 'GetTownList', 'TownName');
                    }
                });
            };
            //endregion
            initPage();
        });
});