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
                    $("#tb_JiaShiYuanTable").CustomTable("reload");
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
                    popdialog.showIframe({
                        'url': 'Edit.html',
                        head: false
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
                    tipdialog.confirm("确定要删除选中的记录？", function (r) {
                        if (r) {
                            helper.Ajax("003300300159", ids, function (data) {
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
            };
            function initlizableTable() {
                $("#tb_JiaShiYuanTable").CustomTable({
                    ajax: helper.AjaxData("003300300157",
                        function (data) {
                            var pageInfo = { Page: data.start / data.length + 1, Rows: data.length };
                            for (var i in data) {
                                delete data[i];
                            }
                            var para = $('.searchpanel-form').serializeObject();
                            $('.searchpanel-form').find('[disabled]').each(function (i, item) {
                                para[$(item).attr('name')] = $(item).val();
                            });
                            para["BenDanWeiOrgCode"] = helper.GetUserInfo().OrganizationCode;
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
                        { data: 'XingMing' },
                        { data: 'XingBie' },
                        { data: 'ShenFenZhengHaoMa' },
                        { data: 'CongYeZiGeZhengHao' },
                        { data: 'YiDongDianHua' },
                        { data: 'XiaQu' },
                        {
                            data: 'ZhaoPian',
                            render: function (data, type, row) {
                                result = '<a class="btn blue ViewZhaoPian" style="padding: 5px" >' + data + '</a>';
                                return result;
                            }
                        },
                    ],
                    pageLength: 10,
                    "fnDrawCallback": function () {
                        tableButtonInit();
                        tableheadfix.ResetFix();
                    },
                });
            };
            function tableButtonInit() {
                //查看驾驶员照片
                $(".ViewZhaoPian").click(function () {
                    var eventData = $('#tb_JiaShiYuanTable').DataTable().row($(this).parents('tr')).data();
                    helper.Ajax("003300300158", eventData.Id, function (data) {
                        if (data.publicresponse.statuscode == 0) {
                            if (data.body != "" || data.body != null) {
                                sessionStorage.setItem("_JiaShiYuanZhaoPian", data.body);
                                sessionStorage.setItem("_JiaShiYuanXingMing", eventData.XingMing);
                                popdialog.showModal({
                                    'url': '../JiaShiYuanXinXi/JiaShiYuanZhaoPian.html',
                                    'width': '420px',
                                    'showSuccess': showJiaShiYuanZhaoPian
                                });
                            }
                            else {
                                tipdialog.alertMsg("该驾驶员无照片");
                            }
                        }
                        else {
                            tipdialog.alertMsg(data.publicresponse.message);
                        }
                    }, false);
                });
            }
            function showJiaShiYuanZhaoPian() {
                $("#JiaShiYuanZhaoPian").attr("src", sessionStorage.getItem("_JiaShiYuanZhaoPian"));
                $("#JiaShiYuanXingMing").text(sessionStorage.getItem("_JiaShiYuanXingMing"))
            }
            initPage();
        });
});