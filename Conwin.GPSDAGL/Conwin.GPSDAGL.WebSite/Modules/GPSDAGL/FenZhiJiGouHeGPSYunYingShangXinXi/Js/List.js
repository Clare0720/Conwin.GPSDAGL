define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'searchbox', 'customtable', 'bootstrap-datepicker.zh-CN', 'permission'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system) {
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
                //更新
                $('#btnEdit').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要修改的行');
                        return false;
                    }
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
                    tipdialog.confirm("确定要删除选中的记录？", function (r) {
                        if (r) {
                            //TODO:调作废接口
                            helper.Ajax("006600200014", ids, function (data) {
                                if ($.type(data) == "string") {
                                    data = helper.StrToJson(data);
                                }
                                if (data.publicresponse.statuscode == 0) {
                                    if (data.body) {
                                        toast.success("删除成功");
                                        $("#tb_Template").CustomTable("reload");
                                    }
                                    else {
                                        tipdialog.alertMsg("删除失败");
                                    }
                                }
                                else {
                                    tipdialog.alertMsg(data.publicresponse.message);
                                }
                            }, false);
                        }
                    });
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

                //注销
                $('#btnCancel').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要停用的行');
                        return false;
                    }
                    var validResult = true;
                    $(rows).each(function (i, item) {
                        if (item.data.YouXiaoZhuangTai == "2") {
                            validResult = false;
                        }
                        else {
                            ids.push(item.data.Id);
                        }
                    });
                    if (!validResult) {
                        tipdialog.errorDialog("只能停用有效状态为正常营业的记录");
                        return false;
                    }
                    tipdialog.confirm("确定要停用选中的记录？", function (r) {
                        if (r) {
                            //TODO:调作废接口
                            helper.Ajax("003300300502", ids, function (data) {
                                if ($.type(data) == "string") {
                                    data = helper.StrToJson(data);
                                }
                                if (data.publicresponse.statuscode == 0) {
                                    if (data.body) {
                                        toast.success("停用成功");
                                        $("#tb_Template").CustomTable("reload");
                                    }
                                    else {
                                        tipdialog.alertMsg("停用失败");
                                    }
                                }
                                else {
                                    tipdialog.alertMsg(data.publicresponse.message);
                                }
                            }, false);
                        }
                    });
                });
                //启用
                $('#btnUse').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要启用的行');
                        return false;
                    }
                    var validResult = true;
                    $(rows).each(function (i, item) {
                        if (item.data.YouXiaoZhuangTai == "1") {
                            validResult = false;
                        }
                        else {
                            ids.push(item.data.Id);
                        }
                    });
                    if (!validResult) {
                        tipdialog.errorDialog("只能启用有效状态为合约到期的记录");
                        return false;
                    }
                    //TODO:编写逻辑
                    tipdialog.confirm("确定要启用选中的记录？", function (r) {
                        if (r) {
                            //TODO:调作废接口
                            helper.Ajax("003300300501", ids, function (data) {
                                if ($.type(data) == "string") {
                                    data = helper.StrToJson(data);
                                }
                                if (data.publicresponse.statuscode == 0) {
                                    if (data.body) {
                                        toast.success("启用成功");
                                        $("#tb_Template").CustomTable("reload");
                                    }
                                    else {
                                        tipdialog.alertMsg("启用失败");
                                    }
                                }
                                else {
                                    tipdialog.alertMsg(data.publicresponse.message);
                                }
                            }, false);
                        }
                    });
                });
            };
            function initlizableTable() {
                $("#tb_Template").CustomTable({
                    ajax: helper.AjaxData("006600200048",///"00020003"为接口服务地址，需要在/Config/conwin.system.js中配置
                        function (data) {
                            var pageInfo = { Page: data.start / data.length + 1, Rows: data.length };
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
                        { data: 'OrgName' },
                        {
                            data: 'OrgCode'
                        },
                        { data: 'YingYeZhiZhaoHao' },
                        { data: 'JingYingQuYu' },
                        {
                            data: 'YouXiaoZhuangTai',
                            render: function (data, type, row, meta) {
                                var value="";
                                if (data == "1") {
                                    value = "正常营业";
                                }
                                if (data == "2") {
                                    value = "合约到期";
                                }
                                return value;
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
            initPage();
        });
});