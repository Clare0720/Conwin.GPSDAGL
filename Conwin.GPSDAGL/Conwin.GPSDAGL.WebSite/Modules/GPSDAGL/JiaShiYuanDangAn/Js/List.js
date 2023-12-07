define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'searchbox', 'customtable', 'bootstrap-datepicker.zh-CN', 'permission'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system) {
            var userInfo = helper.GetUserInfo();
            if (!userInfo || JSON.stringify(userInfo) == "{}") {
                tipdialog.errorDialog('获取用户信息失败');
                return;
            }
            //操作按钮的显示逻辑
            if (userInfo.OrganizationType == 2) {
                //企业
                $("#btnConfirm").addClass("hide");
                $("#btnDetail").removeClass("hide");
                $("#btnCreate").removeClass("hide");
                $("#btnEdit").removeClass("hide");
                $("#btnHire").removeClass("hide");
                $("#btnDismissal").removeClass("hide");
                $("#btnDel").removeClass("hide");
                $("#btnBind").removeClass("hide");
                $("#btnExport").removeClass("hide");
                $("#btnLink").removeClass("hide");
                $("#tb_JiaShiYuanTable").find('th[data-name="Cellphone"]').removeClass("hide");
                $('.searchpanel-form').find('div[data-name="QiYeMingCheng"]').removeClass("show").addClass("hide");
                $('.searchpanel-form').find('div[data-name="LianXiDianHua"]').removeClass("hide").addClass("show");
            }
            else {
                //主管
                $("#btnCreate").addClass("hide");
                $("#btnEdit").addClass("hide");
                $("#btnHire").addClass("hide");
                $("#btnDismissal").addClass("hide");
                $("#btnDel").addClass("hide");
                $("#btnBind").addClass("hide");
                $("#btnLink").addClass("hide");
                $("#btnDetail").removeClass("hide");
                $("#btnConfirm").removeClass("hide");
                $("#btnExport").removeClass("hide");
                $("#tb_JiaShiYuanTable").find('th[data-name="OrgName"]').removeClass("hide");
                $('.searchpanel-form').find('div[data-name="QiYeMingCheng"]').removeClass("hide").addClass("show");
                $('.searchpanel-form').find('div[data-name="LianXiDianHua"]').removeClass("show").addClass("hide");
            }
            //初始化
            var initPage = function () {
                //初始化table
                initlizableTable();
                //查询
                $('#btnSearch').click(function (e) {
                    e.preventDefault();
                    $("#tb_JiaShiYuanTable").CustomTable("reload");
                });
                //重置
                $("#btnReset").click(function (e) {
                    e.preventDefault();
                    $('.searchpanel-form').find('input[type=text]:not(:disabled), select:not(:disabled)').val('');
                    $('.searchpanel-form').find('#ZhuangTai').val(-1);
                });
                //查看
                $('#btnDetail').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_JiaShiYuanTable").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要查看的行');
                        return false;
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('每次只能查看一条记录');
                        return false;
                    }
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                    });
                    $('#hdIDS').val(ids.join(','));
                    $('#operationType').val(1);
                    popdialog.showIframe({
                        'url': 'View.html',
                        head: false
                    });
                });
                //确认
                $("#btnConfirm").click(function (e) {
                    e.preventDefault();
                    //TODO:编写逻辑
                    tipdialog.errorDialog('此功能暂未开通');
                });
                //新增
                $("#btnCreate").click(function (e) {
                    e.preventDefault();
                    $('#operationType').val(2);
                    popdialog.showIframe({
                        'url': 'Add.html',
                        head: false
                    });
                });
                //修改
                $('#btnEdit').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_JiaShiYuanTable").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要修改的行');
                        return false;
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('每次只能修改一条记录');
                        return false;
                    }
                    //TODO:编写逻辑
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                    });
                    $('#hdIDS').val(ids.join(','));
                    $('#operationType').val(3);
                    popdialog.showIframe({
                        'url': 'Edit.html',
                        head: false
                    });
                });
                //聘用
                $('#btnHire').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_JiaShiYuanTable").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要操作的行');
                        return false;
                    }
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                    });
                    tipdialog.confirm("确定要聘用选中的记录？", function (r) {
                        if (r) {
                            helper.Ajax("006600200034", { Id: ids, OperationType: 1 }, function (data) {
                                if (data.body) {
                                    toast.success("聘用成功");
                                    $("#tb_JiaShiYuanTable").CustomTable("reload");
                                }
                                else {
                                    tipdialog.alertMsg(data.publicresponse.message);
                                }
                            }, false);
                        }
                    });
                });
                //解聘
                $('#btnDismissal').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_JiaShiYuanTable").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要操作的行');
                        return false;
                    }
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                    });
                    tipdialog.confirm("确定要解聘选中的记录？", function (r) {
                        if (r) {
                            helper.Ajax("006600200034", { Id: ids, OperationType: 2 }, function (data) {
                                if (data.body) {
                                    toast.success("解聘成功");
                                    $("#tb_JiaShiYuanTable").CustomTable("reload");
                                }
                                else {
                                    tipdialog.alertMsg(data.publicresponse.message);
                                }
                            }, false);
                        }
                    });
                });
                //删除
                $('#btnDel').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_JiaShiYuanTable").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要操作的行');
                        return false;
                    }
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                    });
                    tipdialog.confirm("确定要删除选中的" + ids.length + "条记录？", function (r) {
                        if (r) {
                            helper.Ajax("006600200033", ids, function (data) {
                                if (data.body) {
                                    toast.success("删除成功");
                                    $("#tb_JiaShiYuanTable").CustomTable("reload");
                                }
                                else {
                                    tipdialog.alertMsg(data.publicresponse.message);
                                }
                            }, false);
                        }
                    });
                });
                //绑定车辆
                $('#btnBind').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_JiaShiYuanTable").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要操作的行');
                        return false;
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('每次只能绑定一条记录');
                        return false;
                    }
                    $(rows).each(function (i, item) {
                        if (item.data.WorkingStatus != 1) {
                            tipdialog.errorDialog('非聘用状态不允许绑定车辆');
                            return false;
                        }
                        else {
                            ids.push(item.data.Id);
                        }
                    });
                    if (ids.length > 0) {
                        $('#hdIDS').val(ids.join(','));
                        $('#operationType').val(4);
                        popdialog.showIframe({
                            'url': 'Bind.html',
                            head: false
                        });
                    }
                });
                //导出
                $("#btnExport").on('click', function (e) {
                    e.preventDefault();
                    var params = $('#tb_JiaShiYuanTable').DataTable().ajax.params();
                    if (params != undefined) {
                        helper.Ajax('006600200035', params.data, function (data) {
                            if (data.body) {
                                var resFileId = data.body.File;
                                window.location.href = helper.Route('00000080005', '1.0', system.ServerAgent) + '?id=' + resFileId;
                            } else {
                                if (data.publicresponse.message == "不存在记录") {
                                    tipdialog.errorDialog('导出失败,不存在记录');
                                } else {
                                    tipdialog.errorDialog('导出失败');
                                }
                            }
                        });
                    } else {
                        tipdialog.errorDialog('请先查询');
                    }
                });
                //在线教育报名
                $('#btnLink').on('click', function (e) {
                    e.preventDefault();
                    //TODO:编写逻辑
                    //location.href = "";
                    tipdialog.errorDialog('此功能暂未开通');
                });
            };
            function initlizableTable() {
                $("#tb_JiaShiYuanTable").CustomTable({
                    ajax: helper.AjaxData("006600200030",
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
                    ordering: false, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [
                        {
                            render: function (data, type, row) {
                                return '<input type=checkbox class=checkboxes data-id="' + data + '" />';
                            }
                        },
                        { data: 'Name' },
                        { data: 'IDCard' },
                        { data: userInfo.OrganizationType == 2 ? 'Cellphone' : 'OrgName' },
                        {
                            render: function (data, type, row) {
                                var bindVehicle = row.ChePaiHao;
                                if (row.ChePaiYanSe && row.ChePaiYanSe != '') {
                                    bindVehicle += ('（' + row.ChePaiYanSe + '）');
                                }
                                return bindVehicle;
                            }
                        },
                        { data: 'WorkingStatusText' },
                        { data: 'WorkingDate' }
                    ],
                    pageLength: 10
                });
            };
            initPage();
        });
});