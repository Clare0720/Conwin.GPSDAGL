define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'searchbox', 'customtable', 'bootstrap-datepicker.zh-CN', 'permission'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system) {
            var initPage = function () {

                var userInfo = helper.GetUserInfo();
                if (userInfo.OrganizationType == 4) //代理商不需要待提交的查询条件
                {
                    $("#ShenHeZhuangTai option[value='1']").remove();
                }
                $("#btnDianZiQianZhang").on('click', function (e) {
                    e.preventDefault();
                    tipdialog.errorDialog('此功能暂未开通');
                });
                //初始化table
                initlizableTable();
                if (userInfo.OrganizationType == 2) {
                    $("#QiYeMingChengInput").hide();
                }
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
                    if (rows.length > 1) {
                        tipdialog.errorDialog('只能选择一行');
                        return;
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
                    //checkPower(function () {
                    $('#hdIDS').val(ids.join(','));
                    popdialog.showIframe({
                        'url': 'Edit.html',
                        head: false
                    });
                    //  }, ids);
                });
                //删除
                $('#btnDel').on('click', function (e) {
                    e.preventDefault();
                    var errorText = "";
                    var isdelete = true;
                    var rows = $("#tb_Template").CustomTable('getSelection'), ids = [], carids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要删除的行');
                        return false;
                    }
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                        //if (item.data.IsHavCar == 1) {
                        //    errorText += '企业【' + item.data.OrgName + "】存在车辆信息，无法删除<br/>";
                        //    isdelete = false;
                        //}
                    });
                    if (isdelete) {
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
                    }
                    else {
                        tipdialog.errorDialog(errorText);
                    }

                });
                //查看
                $('#btnView').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template").CustomTable('getSelection'), ids = [];
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
                        if (item.data.ZhuangTai == "2") {
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
                    //checkPower(function () {
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
                    // }, ids);
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
                        if (item.data.ZhuangTai == "1") {
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
                    // checkPower(function () {
                    //TODO:编写逻辑
                    tipdialog.confirm("确定要启用选中的记录？", function (r) {
                        if (r) {
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
                    //  }, ids);
                });
                //提交
                $('#btnSubmit').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要提交的行');
                        return false;
                    }
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                    });
                    // checkPower(function () {
                    //TODO:编写逻辑
                    tipdialog.confirm("确定要提交选中的记录？", function (r) {
                        if (r) {
                            //TODO:调作废接口
                            helper.Ajax("003300300030", ids, function (data) {
                                if ($.type(data) == "string") {
                                    data = helper.StrToJson(data);
                                }
                                if (data.publicresponse.statuscode == 0) {
                                    if (data.body) {
                                        toast.success("提交成功");
                                        $("#tb_Template").CustomTable("reload");
                                    }
                                    else {
                                        tipdialog.errorDialog('提交失败');
                                    }
                                }
                                else {
                                    tipdialog.alertMsg(data.publicresponse.message);
                                }
                            }, false);
                        }
                    });
                    // }, ids);
                });
                //审查
                $('#btnCheck').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要审核的行');
                        return false;
                    }
                    var validResult = true;
                    $(rows).each(function (i, item) {
                        if (item.data.ShenHeZhuangTai != "2") {
                            validResult = false;
                        }
                        else {
                            ids.push(item.data.Id);
                        }
                    });
                    if (!validResult) {
                        tipdialog.errorDialog("只能审核审核状态为待审核的记录");
                        return false;
                    }
                    //checkPower(function () {
                    popdialog.showModal({
                        'url': 'TanKuang.html',
                        'width': '300px',
                        'showSuccess': function () {
                            initSubmitCheck(ids);
                        }

                    });
                    //}, ids);
                });

                function checkPower(callback, ids) {
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

                function initSubmitCheck(ids) {
                    $('#AddZiBiaoSure').on('click', function (e) {
                        var isSure = $("input[name='isSure']:checked").val();
                        if (typeof (isSure) == 'undefined') {
                            tipdialog.errorDialog('请选择审核结果');
                            return false;
                        }
                        var jsonData = { "ids": ids, "isSure": isSure };
                        SubmitCheck(jsonData, function () {
                            $("#tb_Template").CustomTable("reload");
                            popdialog.closeModal();
                        });
                    });
                };

                function SubmitCheck(array, callback) {
                    helper.Ajax("003300300031", array, function (data) {
                        if ($.type(data) == "string") {
                            data = helper.StrToJson(data);
                        }
                        if (data.publicresponse.statuscode == 0) {
                            if (data.body) {
                                toast.success("审核成功");
                                if (typeof callback == 'function') {
                                    callback();
                                }
                            }
                            else {
                                tipdialog.alertMsg("审核失败");
                            }
                        }
                        else {
                            tipdialog.alertMsg(data.publicresponse.message);
                        }

                    }, false);

                };

            };
            function initlizableTable() {
                $("#tb_Template").CustomTable({
                    ajax: helper.AjaxData("006600200016",///"00020003"为接口服务地址，需要在/Config/conwin.system.js中配置
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
                        { data: 'QiYeGuanLiYuan' },
                        { data: 'JingYingXuKeZhengHao' },
                        {
                            data: 'SuoShuQuYu',
                            render: function (data, type, row) {
                                return (row.XiaQuSheng || "广东") + "  " + (row.XiaQuShi || "") + "  " + (row.XiaQuXian || "")
                            }
                        },
                        { data: 'LianXiRen' },
                        { data: 'LianXiFangShi' }
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