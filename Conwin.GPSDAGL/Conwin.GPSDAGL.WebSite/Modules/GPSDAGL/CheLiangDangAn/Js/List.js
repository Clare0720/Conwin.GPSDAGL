define(['/Modules/Config/conwin.main.js', '/Modules/GPSDAGL/CheLiangDangAn/Config/config.js', '/Modules/GPSDAGL/CheLiangShouFeiJiLu/Js/Config/config.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'UserConfig', 'layer', 'searchbox', 'customtable', 'bootstrap-datepicker.zh-CN', 'permission', 'fileupload2'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system, selectcity, UserConfig, layer, fileupload2) {
            var userInfo = helper.GetUserInfo();
            if (userInfo.OrganizationType != 2) {
                $("#QiYeMingChengSearch").attr("style", "");
            }
            var initPage = function () {
                $(".date-picker").datepicker();
                //初始化table
                initlizableTable();
                //查询
                $('#btnSearch').click(function (e) {
                    e.preventDefault();
                    if ($("#ChePaiHao").val().trim() != "" && $("#ChePaiHao").val().trim().length < 3) {
                        tipdialog.errorDialog('请输入合法的车牌号码（至少三位）');
                        return;
                    }
                    $("#tb_CheLiangXinXi").CustomTable("reload");
                });
                //重置
                $("#btnReset").click(function (e) {
                    e.preventDefault();
                    $('.searchpanel-form').find('input[type=text]:not(:disabled), select:not(:disabled)').val('');
                });
                //新增
                $("#btnAdd").click(function (e) {
                    e.preventDefault();
                    //TODO:编写逻辑
                    popdialog.showIframe({
                        'url': 'Add.html',
                        head: false
                    });
                });
                
                
                //修改
                $("#btnEdit").click(function (e) {
                    e.preventDefault();

                    var rows = $("#tb_CheLiangXinXi").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要修改的行');
                        return false;
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('只能选择一行');
                        return false;
                    }
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                    });
                    $('#hdIDS').val(ids.join(','));
                    //TODO:编写逻辑
                    popdialog.showIframe({
                        'url': 'Edit.html',
                        head: false
                    });
                });

                //删除
                $('#btnDel').on('click', function (e) {
                    e.preventDefault();
                    var flag = true;
                    var rows = $("#tb_CheLiangXinXi").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要删除的行');
                        return false;
                    }
                    $(rows).each(function (i, item) {

                        if (item.data.ZhongDuanAnZhangZhunTai == "已安装") {
                            flag = false;
                            return;
                        }
                        ids.push(item.data.Id);
                    });

                    if (!flag) {
                        //tipdialog.errorDialog('不能选择已安装终端的车辆!');
                        tipdialog.errorDialog('不能删除已安装终端的车辆!');
                        return;
                    }

                    //编写逻辑
                    tipdialog.confirm("确定要删除选中的记录？", function (r) {
                        if (r) {
                            //调停用接口
                            helper.Ajax("006600200006", ids, function (data) {
                                if (data.body) {
                                    toast.success("删除成功");
                                    $("#tb_CheLiangXinXi").CustomTable("reload");
                                } else {
                                    tipdialog.errorDialog('删除失败！' + data.publicresponse.message);
                                }
                            }, false);
                        }
                    });
                });
                //查看
                $('#btnView').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_CheLiangXinXi").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要查看的行');
                        return false;
                    }

                    if (rows.length > 1) {
                        tipdialog.errorDialog('只能选择一行');
                        return;
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

                //核查安装
                $("#btnConfirm").click(function (e) {
                    var rows = $("#tb_CheLiangXinXi").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要核查安装的行');
                        return false;
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('只能选择一行');
                        return false;
                    }
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                    });
                    $('#hdIDS').val(ids.join(','));
                    //TODO:编写逻辑
                    popdialog.showIframe({
                        'url': 'Confirm.html',
                        head: false
                    });
                });

                //除了企业以外的组织只能查看
                if (userInfo.OrganizationType == 0 || userInfo.OrganizationType == 2) {
                    var btnlist = $("#ControlPanel").children();
                    $(btnlist).each(function (i, item) {
                        $(item).attr("style", "");
                    });
                }

            };
            //列表初始化
            function initlizableTable() {
                $("#tb_CheLiangXinXi").CustomTable({
                    ajax: helper.AjaxData("006600200001",
                        function (data) {
                            var pageInfo = {
                                Page: parseInt(data.start / data.length + 1),
                                Rows: data.length
                            };
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
                    columns: [{
                        render: function (data, type, row) {
                            return '<input type=checkbox class=checkboxes />';
                        }
                    },
                    { data: 'ChePaiHao' },
                    { data: 'ChePaiYanSe' },
                    {
                        data: 'CheLiangZhongLei',
                        render: function (data, type, row) {
                            switch (data) {
                                case 1:
                                    return "客运班车";
                                    break;
                                case 2:
                                    return "旅游包车";
                                    break;
                                case 3:
                                    return "危险货运";
                                    break;
                                case 4:
                                    return "重型货车";
                                    break;
                                case 5:
                                    return "公交客运";
                                    break;
                                case 6:
                                    return "出租客运";
                                    break;
                                case 7:
                                    return "教练员车";
                                    break;
                                case 8:
                                    return "普通货运";
                                    break;
                                case 9:
                                    return "其它车辆";
                                case 10:
                                    return "校车";
                                default:
                                    return "";
                            }
                        }
                    },
                    { data: 'CheJiaHao' },

                    {
                        data: 'SuoShuQuYu',
                        render: function (data, type, row) {
                            return (row.XiaQuSheng || "广东") + "  " + (row.XiaQuShi || "") + "  " + (row.XiaQuXian || "")
                        }
                    },
                    { data: 'QiYeMingCheng' },
                    { data: 'YunZhengZhuangTai' },
                    {
                        data: 'NianShenZhuangTai',
                        render: function (data, type, row) {

                            switch (data) {

                                case 0: return "未通过"; break;
                                case 1: return "通过"; break;
                                case 2: return "未审核"; break;
                                default: return "";
                            }

                        }
                    },
                    ],
                    pageLength: 10,
                    //"fnDrawCallback": function (oSettings) {
                    //    $("#tb_CheLiangXinXi_filter").css("z-index", "1000");
                    //    tableheadfix.ResetFix();
                    //}
                });
                //tableheadfix.InitFix(system.OnlyTableFix);
            };


            //个性化代码块
            //region
            //endregion
            initPage();

        });
});