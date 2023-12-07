define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'searchbox', 'customtable', 'bootstrap-datepicker.zh-CN', 'permission'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system) {
            var userInfo = helper.GetUserInfo(); 
            var initPage = function () {
                //初始化table
                initlizableTable();
                //查询
                $('#btnSearch').click(function (e) {
                    e.preventDefault();
                    var chepaihao = $("#ChePaiHao").val();
                    if (chepaihao && $.trim(chepaihao).length > 0 && $.trim(chepaihao).length < 3) {
                        tipdialog.errorDialog('车牌号不能小于三位');
                    }
                    else {
                        $("#tb_CheLiangTable").CustomTable("reload");
                    }
                });
                //重置
                $("#btnReset").click(function (e) {
                    e.preventDefault();
                    $('.searchpanel-form').find('input[type=text]:not(:disabled), select:not(:disabled)').val('');
                });
                //关闭
                $('#btnclose').click(function (e) {
                    e.preventDefault();
                    popdialog.closeIframe();
                });
                //绑定车辆
                $('#btnBindOk').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_CheLiangTable").CustomTable('getSelection'), clids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要绑定的行');
                        return false;
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('每次只能绑定一条记录');
                        return false;
                    }
                    $(rows).each(function (i, item) {
                        clids.push(item.data.Id);
                    });
                    $('#hdCLIDS').val(clids.join(','));
                    var clid = clids[0];
                    //判断驾驶员选择
                    var id;
                    var ids = window.parent.document.getElementById('hdIDS').value;
                    if (ids.split(',').length == 1) {
                        id = ids.split(',')[0];
                    }
                    else {
                        tipdialog.errorDialog('请先选择需要操作的驾驶员');
                    }
                    //绑定车辆
                    helper.Ajax("006600200036", { Id: id, CheLiangId: clid }, function (data) {
                        debugger
                        if ($.type(data) == "string") {
                            data = helper.StrToJson(data);
                        }
                        if (data.publicresponse.statuscode == 0) {
                            if (data.body) {
                                toast.success("绑定成功", { showDuration: 100 });
                                setTimeout(function () {
                                    parent.$("#btnSearch").click();
                                    popdialog.closeIframe();
                                }, 2000);
                            }
                            else {
                                tipdialog.alertMsg("驾驶员绑定车辆失败");
                            }
                        }
                        else {
                            tipdialog.alertMsg(data.publicresponse.message);
                        }
                    }, false);
                });
            };
            function initlizableTable() {
                $("#OrgType").val(userInfo.OrganizationType);
                //$("#OrgType").val(2);
                $("#tb_CheLiangTable").CustomTable({
                    ajax: helper.AjaxData("006600200007",
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
                        { data: 'ChePaiHao' },
                        { data: 'ChePaiYanSe' },
                        { data: 'XiaQuShi' },
                        { data: 'XiaQuXian' },
                        { data: 'YeHuMingCheng' }
                    ],
                    pageLength: 10
                });
            };
            initPage();
        });
});