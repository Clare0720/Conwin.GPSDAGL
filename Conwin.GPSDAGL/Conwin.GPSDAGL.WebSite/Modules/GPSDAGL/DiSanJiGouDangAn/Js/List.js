define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'searchbox', 'customtable', 'bootstrap-datepicker.zh-CN', 'permission'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system) {
            var initPage = function () {

                var userInfo = helper.GetUserInfo();


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

                    var isDaiTiJiao = false;
                    var isShenHeTongGuoOrDaiShenHe = false;
                    //TODO:编写逻辑
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                        if (item.data.ShenHeZhuangTai == "1") {
                            isDaiTiJiao = true;
                        }
                        if (item.data.ShenHeZhuangTai == "2" || item.data.ShenHeZhuangTai == "3") {
                            isShenHeTongGuoOrDaiShenHe = true;
                        }
                    });
                    if ((userInfo.OrganizationType == "0" || userInfo.OrganizationType == "4") && isDaiTiJiao) {
                        tipdialog.errorDialog('不允许修改待提交的记录');
                        return false;
                    }
                    if ((userInfo.OrganizationType == "5" || userInfo.OrganizationType == "6") && isShenHeTongGuoOrDaiShenHe) {
                        tipdialog.errorDialog('只允许修改待提交或审核不通过的记录');
                        return false;
                    }
                    checkPower(function () {
                        $('#hdIDS').val(ids.join(','));
                        popdialog.showIframe({
                            'url': 'Edit.html',
                            head: false
                        });
                    }, ids);
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
                    checkPower(function () {
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
                                            tipdialog.errorDialog('删除失败');
                                        }
                                    }
                                    else {
                                        tipdialog.alertMsg(data.publicresponse.message);
                                    }
                                }, false);
                            }
                        });
                    }, ids);
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
                //停用
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
                        tipdialog.errorDialog("只能停用有效状态为合作加盟的记录");
                        return false;
                    }
                    checkPower(function () {
                        //TODO:编写逻辑
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
                                            tipdialog.errorDialog('停用失败');
                                        }
                                    }
                                    else {
                                        tipdialog.alertMsg(data.publicresponse.message);
                                    }
                                }, false);
                            }
                        });
                    }, ids);
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
                    checkPower(function () {
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
                                            tipdialog.errorDialog('启用失败');
                                        }
                                    }
                                    else {
                                        tipdialog.alertMsg(data.publicresponse.message);
                                    }
                                }, false);
                            }
                        });
                    }, ids);
                });

                function checkPower(callback, ids) {
                    callback();
                    return true;
                    helper.Ajax("003300300111", { ids: ids, type: 2 }, function (data) {
                        if ($.type(data) == "string") {
                            data = helper.StrToJson(data);
                        }
                        if (data.publicresponse.statuscode == 0) {
                            if (data.body) {
                                if (typeof callback == 'function') {
                                    callback();
                                }
                            }
                            else {
                                tipdialog.errorDialog('出错了，请稍后重试');
                            }
                        }
                        else {
                            tipdialog.alertMsg("<div style='max-height:300px;'>" + data.publicresponse.message + "</div>");
                        }
                    }, false);
                };



            };
            function initlizableTable() {
                $("#tb_Template").CustomTable({
                    ajax: helper.AjaxData("006600200054",
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
                    single: true,
                    filter: false,
                    ordering: true, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [
                        {
                            render: function (data, type, row) {
                                return '<input type=checkbox class=checkboxes />';
                            }
                        },
                        {
                            render: function (data, type, row, meta) {
                                //debugger;
                                return (meta.row + 1);
                            }
                        },
                        { data: 'OrgName' },
                        {
                            data: 'OrgType',
                            render: function (data, type, row, meta) {
                                var value ='';
                                if (data == "11") {
                                    value = "市政府";
                                } else if (data == "12") {
                                    value = "县政府";
                                }
                                return value;
                            }
                        },
                        { data: 'LianXiRen' },
                        {
                            data: 'LianXiDianHua',
                            render: function (data, type, row, meta) {
                                var value = '';
                                if (row.ShouJiHaoMa !== undefined && row.ShouJiHaoMa !== '') {
                                    value = row.ShouJiHaoMa
                                } else if (row.GuDingDianHua !== undefined && row.GuDingDianHua !== '') {
                                    value = row.GuDingDianHua
                                }
                                return value;
                            }
                        },
                        { data: 'JingYingQuYu' },
                        {
                            data: 'YouXiaoZhuangTai',
                            render: function (data, type, row, meta) {
                                var value ="合约到期";
                                if (data == "1") {
                                    value = "正常营业";
                                }
                                else if (data == "2") {
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