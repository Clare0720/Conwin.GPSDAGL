define(['/Modules/Config/conwin.main.js', '/Modules/GPSDAGL/CheLiangDangAn/Config/config.js', '/Modules/GPSDAGL/CheLiangShouFeiJiLu/Js/Config/config.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'UserConfig', 'searchbox', 'customtable', 'bootstrap-datepicker.zh-CN', 'permission', 'fileupload2'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system, selectcity, UserConfig, fileupload2) {
            var initPage = function () {

                $(".date-picker").datepicker();
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
                //修改
                $("#btnEdit").click(function (e) {
                    e.preventDefault();

                    var rows = $("#tb_Template").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要查看的行');
                        return false;
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('只能选择单行数据!');
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
                    var rows = $("#tb_Template").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要删除的行!');
                        return false;
                    }
                    $(rows).each(function (i, item) {

                        if (item.data.RenZhengZhuangTai == 3) {
                            flag = false;
                            return;
                        }
                        ids.push(item.data.Id);
                    });

                    if (!flag) {
                        tipdialog.errorDialog('不能删除已认证的企业!');
                        return;
                    }

                    //编写逻辑
                    tipdialog.confirm("确定要删除选中的记录？", function (r) {
                        if (r) {
                            //调停用接口
                            helper.Ajax("000000000007", ids, function (data) {
                                if (data.body) {
                                    toast.success("删除成功");
                                    $("#tb_Template").CustomTable("reload");
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
                    var rows = $("#tb_Template").CustomTable('getSelection'),
                        ids = [];
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
            };

            //列表初始化
            function initlizableTable() {
                $("#tb_Template").CustomTable({
                    ajax: helper.AjaxData("000000000001",
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
                    { data: 'QiYeMingCheng' },
                    {
                        data: 'QiYeLeiXing',
                        render: function (data, type, row) {
                            switch (data) {
                                case 1:
                                    return "运输企业";
                                    break;
                                case 2:
                                    return "GPS营运商";
                                    break;
                                case 3:
                                    return "个体户";
                                    break;
                                case 4:
                                    return "驾培企业";
                                    break;
                                default:
                                    return "";
                            }
                        }
                    },
                    { data: 'TongYiSheHuiXinYongDaiMa' },
                    { data: 'FaRen' },
                    { data: 'FaRenShenFenZhengHaoMa' },
                    {
                        data: 'ChuangJianShiJian',
                        render: function (data, type, row) {
                            if (!data) {
                                return "";
                            } else {
                                //return data.replace(/T/g, " ");
                                return new Date(data.toString()).Format("yyyy-MM-dd HH:mm:ss");
                            }
                        }
                    },
                    { data: 'QiYeRenZhengMa' },
                    { data: 'ShuJuLaiYuan' },
                    {
                        data: 'RenZhengZhuangTai',
                        render: function (data, type, row) {
                            switch (data) {
                                case 1:
                                    return "认证中";
                                    break;
                                case 2:
                                    return "未认证";
                                    break;
                                case 3:
                                    return "已认证";
                                    break;
                                default:
                                    return "";
                            }
                        }
                    }
                    ],
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        $("#tb_CheLiangXinXi_filter").css("z-index", "1000");
                        tableheadfix.ResetFix();
                    }
                });
                tableheadfix.InitFix(system.OnlyTableFix);
            };
            //个性化代码块
            //region
            //endregion
            initPage();

        });
});