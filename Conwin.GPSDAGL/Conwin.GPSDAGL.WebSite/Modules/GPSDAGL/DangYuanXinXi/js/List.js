define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'searchbox', 'customtable', 'bootstrap-datepicker.zh-CN', 'permission'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system) {
            var positionTypeArr = [];
            var userInfo = helper.GetUserInfo();
            if (userInfo.OrganizationType == 2) {
                $("#QiYeMingChengInput").hide();
                $("#QiYeMore").hide();
                
            }
            var initPage = function () {

                initlizableTable();
                listButtonInit();

            };


            // 初始化表格
            function initlizableTable() {

                $("#tb_DangYuanInfo").CustomTable({
                    ajax: helper.AjaxData("006600200020",
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
                        { data: 'Name' },
                        { data: 'IDCard' },
                        {
                            data: 'Sex',
                            render: function (data, type, row) {
                                var statusStr = ""
                                switch (data) {
                                    case 1:
                                        statusStr = "女";
                                        break;
                                    default:
                                        statusStr = "男";
                                        break;
                                }
                                return statusStr;
                            }

                        },
                        {
                            data: 'Nation',
                            render: function (data, type, row) {
                                var statusStr = ""
                                switch (data) {
                                    case 1: statusStr = "汉族"; break;
                                    case 2: statusStr = "蒙古族"; break;
                                    case 3: statusStr = "回族"; break;
                                    case 4: statusStr = "藏族"; break;
                                    case 5: statusStr = "维吾尔族"; break;
                                    case 6: statusStr = "苗族"; break;
                                    case 7: statusStr = "彝族"; break;
                                    case 8: statusStr = "壮族"; break;
                                    case 9: statusStr = "布依族"; break;
                                    case 10: statusStr = "朝鲜族"; break;
                                    case 11: statusStr = "满族"; break;
                                    case 12: statusStr = "侗族"; break;
                                    case 13: statusStr = "瑶族"; break;
                                    case 14: statusStr = "白族"; break;
                                    case 15: statusStr = "土家族"; break;
                                    case 16: statusStr = "哈尼族"; break;
                                    case 17: statusStr = "哈萨克族"; break;
                                    case 18: statusStr = "傣族"; break;
                                    case 19: statusStr = "黎族"; break;
                                    case 20: statusStr = "傈僳族"; break;
                                    case 21: statusStr = "佤族"; break;
                                    case 22: statusStr = "畲族"; break;
                                    case 23: statusStr = "高山族"; break;
                                    case 24: statusStr = "拉祜族"; break;
                                    case 25: statusStr = "水族"; break;
                                    case 26: statusStr = "东乡族"; break;
                                    case 27: statusStr = "纳西族"; break;
                                    case 28: statusStr = "景颇族"; break;
                                    case 29: statusStr = "柯尔克孜族"; break;
                                    case 30: statusStr = "土族"; break;
                                    case 31: statusStr = "达斡尔族"; break;
                                    case 32: statusStr = "仫佬族"; break;
                                    case 33: statusStr = "羌族"; break;
                                    case 34: statusStr = "布朗族"; break;
                                    case 35: statusStr = "撒拉族"; break;
                                    case 36: statusStr = "毛南族"; break;
                                    case 37: statusStr = "仡佬族"; break;
                                    case 38: statusStr = "锡伯族"; break;
                                    case 39: statusStr = "阿昌族"; break;
                                    case 40: statusStr = "普米族"; break;
                                    case 41: statusStr = "塔吉克族"; break;
                                    case 42: statusStr = "怒族"; break;
                                    case 43: statusStr = "乌孜别克族"; break;
                                    case 44: statusStr = "俄罗斯族"; break;
                                    case 45: statusStr = "鄂温克族"; break;
                                    case 46: statusStr = "德昂族"; break;
                                    case 47: statusStr = "保安族"; break;
                                    case 48: statusStr = "裕固族"; break;
                                    case 49: statusStr = "京族"; break;
                                    case 50: statusStr = "塔塔尔族"; break;
                                    case 51: statusStr = "独龙族"; break;
                                    case 52: statusStr = "鄂伦春族"; break;
                                    case 53: statusStr = "赫哲族"; break;
                                    case 54: statusStr = "门巴族"; break;
                                    case 55: statusStr = "珞巴族"; break;
                                    case 56: statusStr = "基诺族"; break;
                                        break;
                                    default:
                                        statusStr = "";
                                        break;
                                }
                                return statusStr;
                            }
                        },
                        { data: 'NativePlace' },
                        {
                            data: 'Education',
                            render: function (data, type, row) {
                                var statusStr = ""
                                switch (data) {
                                    case 1:
                                        statusStr = "高中，中专及以下"; break;
                                    case 2:
                                        statusStr = "专科"; break;
                                    case 3:
                                        statusStr = "本科"; break;
                                    case 4:
                                        statusStr = "硕士研究生"; break;
                                    case 5:
                                        statusStr = "博士研究生"; 
                                        break;
                                    default:
                                        statusStr = "";
                                        break;
                                }
                                return statusStr;
                            }
                        },

                    ],
                    pageLength: 10,

                });
            };

            // 列表按钮初始化
            function listButtonInit() {
                //查询
                $('#btnSearch').click(function (e) {
                    e.preventDefault();
                    $("#tb_DangYuanInfo").CustomTable("reload");
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
                    var rows = $("#tb_DangYuanInfo").CustomTable('getSelection'), ids = [];
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

                    var rows = $("#tb_DangYuanInfo").CustomTable('getSelection');
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
                    var rows = $("#tb_DangYuanInfo").CustomTable('getSelection'), ids = [];
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
                            helper.Ajax("006600200023", ids, function (data) {
                                if ($.type(data) == "string") {
                                    data = helper.StrToJson(data);
                                }
                                if (data.publicresponse.statuscode == 0) {
                                    if (data.body) {
                                        toast.success("删除成功");
                                        $("#tb_DangYuanInfo").CustomTable("reload");
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
                var rows = $('#tb_DangYuanInfo').CustomTable('getSelection');
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