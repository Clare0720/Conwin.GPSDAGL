define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'searchbox', 'customtable', 'bootstrap-datepicker.zh-CN', 'permission'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system, selectcity) {
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
                //日志
                $('#btnLog').click(function (e) {
                    e.preventDefault();
                    popdialog.showModal({
                        'url': 'ZiBiaoView2.html',
                        'width': '1000px',
                        'showSuccess': initLogZiBiao
                    });
                });
                //导出
                $('#btnExport').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template").CustomTable('getSelection'), ids = [];
                    var para = $('.searchpanel-form').serializeObject();
                    if (rows != undefined) {
                        $(rows).each(function (i, item) {
                            ids.push(item.data.Id);
                        });
                    }
                    var req;
                    if (ids.length >= 1) {
                        req = { 'ExCheckId': ids };
                    } else {
                        req = para;
                    }

                    helper.Ajax('006600200092', req, function (data) {
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

                });
                //个性化代码块
                //region
                selectCity();
                //endregion
            };

            function initlizableTable() {
                $("#tb_Template").CustomTable({
                    ajax: helper.AjaxData("006600200091",///"00020003"为接口服务地址，需要在/Config/conwin.system.js中配置
                        function (data) {
                            var pageInfo = { Page: parseInt(data.start / data.length + 1), Rows: data.length };
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
                        { data: 'ZhengMingBianHao' },
                        { data: 'ChePaiHao' },
                        { data: 'ChePaiYanSe' },
                        { data: 'YeHuMingCheng' },
                        { data: 'XiaQuShi' },
                        { data: 'XiaQuXian' },
                        {
                            data: 'ZhengMingLeiXin', render: function (data, type, row) {
                                var text = "";
                                switch (data) {
                                    case 1: text = '卫星定位安装证明'; break;
                                    case 2: text = '智能视频安装承诺函'; break;
                                    case 3: text = '智能视频安装证明'; break;
                                    case 4: text = '卫星定位安装证明(佛山)'; break;
                                    case 5: text = '视频报警装置安装承诺函(佛山)'; break;
                                    case 6: text = '智能视频安装证明(佛山)'; break;
                                    case 7: text = 'GPS传输证明(佛山)'; break;
                                    case 8: text = 'GPS终端设备备案表'; break;
                                    default: text = '业务办理证明'; break;
                                }
                                return text;
                            }
                        },
                        {
                            data: 'SYS_ChuangJianShiJian', render: function (data, type, row) {
                                if (data.empty == '') return ' ';
                                var notHaveSAndT = data.split('.')[0].split('T');
                                return notHaveSAndT[0] + ' ' + notHaveSAndT[1];
                            }
                        },
                        {
                            data: 'Do', render: function (data, type, row) {
                                // debugger
                                if (row.ZhengMingLeiXin == 1 || row.ZhengMingLeiXin == 2) {
                                    return "<a href='" + helper.Route('00000080005', '1.0') + '?id=' + row.ZhengMingFileId + "' class='btn green' target='_blank'>下载</a>";
                                }
                                else if (row.ZhengMingLeiXin == 3 || row.ZhengMingLeiXin == 4 || row.ZhengMingLeiXin == 5 || row.ZhengMingLeiXin == 6 || row.ZhengMingLeiXin == 7 || row.ZhengMingLeiXin == 8) {
                                    var wzBtn = "<a href='" + helper.Route('00000080005', '1.0') + '?id=' + row.ZhengMingFileId + "' class='btn green' target='_blank'>下载无章打印</a>";
                                    var yzBtn = "<a style='margin-top:10px;' href='" + helper.Route('00000080005', '1.0') + '?id=' + row.ShuiYinPDFFileId + "' class='btn green' target='_blank'>下载有章打印</a>";
                                    if (row.ShuiYinPDFFileId) {
                                        return wzBtn + yzBtn;                                      
                                    }
                                    else {
                                        return wzBtn;
                                    }
                                }
                                var wzBtn = "<a href='#' class='btn green Print' name = 'WuZhang'  data-print='" +row.Id +"'target='_blank'>下载无章打印</a>";
                                var yzBtn = "<a style='margin-top:10px;' href='#' class='btn green Print' target='_blank' name='YouZhang' data-print='" + row.Id + "'>下载有章打印</a>";
                                return wzBtn + yzBtn; 
                            }
                        }

                    ],
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        tableheadfix.ResetFix();
                        //$('.Print').click(function (e) {//点击打印
                        //    e.preventDefault();

                        //    PrintPDF($(this).attr('name'), $(this).data('print'));
                        //});
                    }
                });
                tableheadfix.InitFix(system.OnlyTableFix);
            };


            function selectCity() {
                var userInfo = helper.GetUserInfo();
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#XiaQuShi, #XiaQuXian').empty().append(defaultOption);
                selectcity.setXiaQu('00000020005', { "Province": "广东" }, '#XiaQuShi', function () {
                    if (userInfo.OrganizationType != 10) {
                        var XiaQuShi = userInfo.OrganizationManageArea;
                        XiaQuShi = XiaQuShi.replace(/广东/g, "");
                        var list = XiaQuShi.split("|");
                        $("#XiaQuShi").find("option").each(function (index, item) {
                            if (list.indexOf($(item).val()) < 0 && $(item).val() != "") {
                                $(item).remove();
                            }
                        });
                    }
                }, 'GetCityList', 'CityName');
                $('#XiaQuShi').change(function () {
                    $('#XiaQuXian,#XiaQuZhen').empty().append(defaultOption);
                    var data = { "City": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化区县下拉框
                        selectcity.setXiaQu('00000020006', data, '#XiaQuXian', 'GetDistrictList', 'DistrictName');
                    }
                });
                $('#XiaQuXian').change(function () {
                    $('#XiaQuZhen').empty().append(defaultOption);
                    var data = { "District": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化区镇下拉框
                        selectcity.setXiaQu('00000020007', data, '#XiaQuZhen', 'GetTownList', 'TownName');
                    }
                });
            };
            initPage();
        });
});