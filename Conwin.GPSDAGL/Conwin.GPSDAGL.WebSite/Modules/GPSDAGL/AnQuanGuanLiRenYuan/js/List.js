define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'searchbox', 'customtable', 'bootstrap-datepicker.zh-CN', 'permission'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system) {
            var positionTypeArr = [];           
            var userInfo = helper.GetUserInfo();
            if (userInfo.OrganizationType == 2) {
                $("#LianXiDianHuaInput").show();
            }
            else {
                $("#WorkingStatusInput").hide();
                $("#ZhiWuInput").hide();
                $("#QiYeMingChengInput").show();
            }
            var initPage = function () {

                initlizableTable();
                listButtonInit();

            };


            // 初始化表格
            function initlizableTable() {
                if (userInfo.OrganizationType == 0 || userInfo.OrganizationType == 11 || userInfo.OrganizationType == 12) {
                    $("#QiYeMingChengTab").attr("style", "width:15%");
                    $("#QuYuTab").attr("style", "width:5%");
                }
                else if (userInfo.OrganizationType == 2) {
                    $("#LianXiDianHuaTab").attr("style", "width:8%");
                    $("#ZhiWuTab").attr("style", "width:7%");
                    $("#WorkingStatusTab").attr("style", "width:5%");
                }
                var columns = [];
                var columnsObj = {
                    'Id': {
                        data: 'id',
                        render: function (data, type, row) {
                            return '<input type="checkbox" class="checkboxes">';
                        }
                    },
                    'Name': { data: 'Name' },
                    'IDCard': { data: 'IDCard' },
                    'QuYu': { data: 'QuYu' },
                    'LianXiDianHua': { data: 'LianXiDianHua' },
                    'WorkingStatus': {
                        data: 'WorkingStatus',
                        render: function (data, type, row) {
                            var statusStr = ""
                            switch (data) {
                                case 2:
                                    statusStr = "在职";
                                    break;
                                case 3:
                                    statusStr = "离职";
                                    break;
                                default:
                                    statusStr = "";
                                    break;
                            }
                            return statusStr;
                        }
                    },
                    'ZhiWu': { data: 'ZhiWu' },
                    'OrgName': { data: 'OrgName' },
                    'ZhuanYeMingCheng': { data: 'ZhuanYeMingCheng' },
                    'KeMuMingCheng': { data: 'KeMuMingCheng' },
                    'KaoHeZhuangTai': {
                        data: 'KaoHeZhuangTai',
                        render: function (data, type, row) {
                            var statusStr = ""
                            switch (data) {
                                case 0:
                                    statusStr = "待考核";
                                    break;
                                case 1:
                                    statusStr = "已通过";
                                    break;
                                default:
                                    statusStr = "";
                                    break;
                            }
                            return statusStr;
                        }
                    },
                    'KaoHeRiQi': {
                        data: 'KaoHeRiQi',
                        render: function (data, type, row) {
                            if (!data) {
                                return "";
                            } else {
                                //return data.replace(/T/g, " ");
                                return new Date(data.toString()).Format("yyyy-MM-dd");
                            }
                        }
                    },
                }
                switch (userInfo.OrganizationType) {
                    case 0:
                    case 11:
                    case 12:
                        columns.push(columnsObj['Id']);
                        columns.push(columnsObj['Name']);
                        columns.push(columnsObj['IDCard']);
                        columns.push(columnsObj['QuYu']);
                        columns.push(columnsObj['OrgName']);
                        columns.push(columnsObj['ZhuanYeMingCheng']);
                        columns.push(columnsObj['KeMuMingCheng']);
                        columns.push(columnsObj['KaoHeZhuangTai']);
                        columns.push(columnsObj['KaoHeRiQi']);
                        break;
                    case 2:
                        columns.push(columnsObj['Id']);
                        columns.push(columnsObj['Name']);
                        columns.push(columnsObj['IDCard']);
                        columns.push(columnsObj['LianXiDianHua']);
                        columns.push(columnsObj['ZhiWu']);
                        columns.push(columnsObj['ZhuanYeMingCheng']);
                        columns.push(columnsObj['KeMuMingCheng']);
                        columns.push(columnsObj['KaoHeZhuangTai']);
                        columns.push(columnsObj['WorkingStatus']);
                        columns.push(columnsObj['KaoHeRiQi']);
                        break;
                }
                $("#tb_AnQuanYuanInfo").CustomTable({
                    ajax: helper.AjaxData("006600200025",
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
                    filter: true,
                    //ordering: true, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: columns,
                    pageLength: 10,

                });
            };

            // 列表按钮初始化
            function listButtonInit() {
                //查询
                $('#btnSearch').click(function (e) {
                    e.preventDefault();
                    $("#tb_AnQuanYuanInfo").CustomTable("reload");
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

                //修改
                $("#btnEdit").click(function (e) {
                    e.preventDefault();
                    var rows = $("#tb_AnQuanYuanInfo").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要修改的行');
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
                        'url': 'Edit.html',
                        head: false
                    });

                });

                //查看
                $("#btnView").click(function (e) {
                    e.preventDefault();
                    var rows = $("#tb_AnQuanYuanInfo").CustomTable('getSelection');
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要查看的行');
                        return false;
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('只能选择一行');
                        return;
                    }

                    var row = GetSelectRow();
                    if (!row) {
                        return false;
                    }
                    popdialog.showIframe({
                        'url': 'View.html',
                        head: false
                    });
                });

                //删除
                $('#btnDel').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_AnQuanYuanInfo").CustomTable('getSelection'), ids = [];
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
                            helper.Ajax("006600200028", ids, function (data) {
                                if ($.type(data) == "string") {
                                    data = helper.StrToJson(data);
                                }
                                if (data.publicresponse.statuscode == 0) {
                                    if (data.body) {
                                        toast.success("删除成功");
                                        $("#tb_AnQuanYuanInfo").CustomTable("reload");
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
                    // }, ids);
                });

                //除了企业以外的组织只能新增和查看
                if (userInfo.OrganizationType == 0 || userInfo.OrganizationType == 2) {
                    var btnlist = $("#ControlPanel").children();
                    $(btnlist).each(function (i, item) {
                        $(item).show();
                    });
                }
            }

            function GetSelectRow() {
                var rows = $('#tb_AnQuanYuanInfo').CustomTable('getSelection');
                if (rows == undefined) {
                    tipdialog.errorDialog('请选择需要操作的一行');
                    return false;
                }
                var id = rows[0].data.Id;
                $('#hdIDS').val(id);
                return rows[0].data;
            };

            initPage();
        });
});