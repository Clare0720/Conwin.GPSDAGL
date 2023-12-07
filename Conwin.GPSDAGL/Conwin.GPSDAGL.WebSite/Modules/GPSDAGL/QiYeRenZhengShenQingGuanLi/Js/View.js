define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'fileupload', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, fileupload) {
            var viewYuanGongId = '';
            var initPage = function () {
                //初始化页面样式
                common.AutoFormScrollHeight('#Form1', function (hg) {
                    var boxHeight = hg - $('.portlet-title').outerHeight(true) - $('.nav-tabs').outerHeight(true) - 100;
                    var me = $(".scroller", '#Form1').eq(0);
                    me.parent().css('height', boxHeight);
                    me.css('height', boxHeight);
                });
                formcontrol.initial();
                initlizableTable();

                //翻页控件
                var ids = window.parent.document.getElementById('hdIDS').value;
                prevNextpage.initPageInfo(ids.split(','));
                prevNextpage.bindPageClass();
                //关闭
                $('#btnclose').click(function () {
                    popdialog.closeIframe();
                });
                //上一条
                $('#prevBtn').click(function (e) {
                    e.preventDefault();
                    prevNextpage.prev();
                    updateData();
                    $("#tb_Template").CustomTable("reload");
                });
                //下一条
                $('#nextBtn').click(function (e) {
                    e.preventDefault();
                    prevNextpage.next();
                    updateData();
                    $("#tb_Template").CustomTable("reload");
                });
                //查询员工列表
                $('#btnSearchYuanGong').click(function (e) {
                    e.preventDefault();
                    $("#tb_Template").CustomTable("reload");
                });
                //查看员工
                $('#btnViewYuanGong').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要查看的行');
                        return false;
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('只能选择一行进行查看');
                        return false;
                    }
                    //TODO:编写逻辑
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                    });
                    viewYuanGongId = ids.join(',');
                    popdialog.showModal({
                        'url': 'ViewYuanGong.html',
                        'width': '800px',
                        'showSuccess': viewYuanGong
                    });
                });
                updateData();
            };
            //查看员工
            function viewYuanGong() {
                getYuanGongXinXi(viewYuanGongId, function (serviceData) {
                    if (serviceData.publicresponse.statuscode == 0) {
                        fillFormDataYuanGong(serviceData.body);
                        // 文件组件
                        var YG_ShenFenZhengZhaoId = $("#YG_ShenFenZhengZhaoId").val();
                        if (YG_ShenFenZhengZhaoId) {
                            $("#" + YG_ShenFenZhengZhaoId + "View").remove();
                        }
                        fileupload.rebindFileButtonView(['YG_ShenFenZhengZhaoId']);
                    } else {
                        tipdialog.errorDialog("请求数据失败");
                    }
                });
            }
            //获取数据
            function getYuanGongXinXi(id, callback) {
                //调用获取单条信息接口
                helper.Ajax("000000000005", id, function (resultdata) {
                    if (typeof callback == 'function') {
                        callback(resultdata);
                    }
                }, false);
            };
            //绑定数据
            function fillFormDataYuanGong(resource) {
                $('#FormYuanGong').find('input[name],select[name],textarea[name]').each(function (i, item) {
                    $(item).val('');
                    var tempValue = resource[$(item).attr('name')];
                    if (tempValue != undefined) {
                        $(item).val(tempValue.toString() == '' ? '' : tempValue);
                    } else {
                        $(item).val('');
                    }
                });
                $('#FormYuanGong').find('.form-control-static').each(function (i, item) {
                    $(item).html('');
                    var index = $(item).attr('for');
                    var tempValue = resource[index];
                    if (tempValue != undefined) {
                        $(item).html(tempValue == '' ? '' : tempValue);
                    } else {
                        $(item).html('');
                    }
                });
                $('#Id').val(resource.Id);
            };


            //列表初始化
            function initlizableTable() {
                $("#tb_Template").CustomTable({
                    ajax: helper.AjaxData("000000000002",
                        function (data) {
                            var pageInfo = {
                                Page: parseInt(data.start / data.length + 1),
                                Rows: data.length
                            };
                            for (var i in data) {
                                delete data[i];
                            }
                            /*
                            var para = $('.searchpanel-form').serializeObject();
                            $('.searchpanel-form').find('[disabled]').each(function (i, item) {
                                para[$(item).attr('name')] = $(item).val();
                            });
                            */
                            var para = {
                                QiYeId: prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index],
                                YuanGongBianHao: $('#YuanGongBianHao').val(),
                                YuanGongXingMing: $('#YuanGongXingMing').val(),
                                RenZhengZhuangTai: $('#YuanGongRenZhengZhuangTai').val()
                            };
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
                    { data: 'YuanGongBianHao' },
                    {
                        data: 'ZhiWu',
                        render: function (data, type, row) {
                            switch (data) {
                                case 1:
                                    return "企业法人";
                                    break;
                                case 2:
                                    return "管理人员";
                                    break;
                                case 3:
                                    return "工作人员";
                                    break;
                                case 4:
                                    return "司机";
                                    break;
                                default:
                                    return "";
                            }
                        }
                    },
                    { data: 'YuanGongXingMing' },
                    { data: 'ShenFenZhengHao' },
                    { data: 'ShouJiHaoMa' },
                    { data: 'QiYeZhangHao' },
                    { data: 'ShuJuLaiYuan' },
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
                    //paging: false,
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        $("#tb_CheLiangXinXi_filter").css("z-index", "1000");
                        tableheadfix.ResetFix();
                    }
                });
                tableheadfix.InitFix(system.OnlyTableFix);
            };
            //刷新数据
            function updateData() {
                var id = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                getQiYeRenZhengXinXi(id, function (serviceData) {
                    if (serviceData.publicresponse.statuscode == 0) {
                        fillFormData(serviceData.body);
                        // 文件组件
                        var YunYingZhiZhaoId = $("#YunYingZhiZhaoId").val();
                        if (YunYingZhiZhaoId) {
                            $("#" + YunYingZhiZhaoId + "View").remove();
                        }
                        var FaRenShenFenZhengZhaoId = $("#FaRenShenFenZhengZhaoId").val();
                        if (FaRenShenFenZhengZhaoId) {
                            $("#" + FaRenShenFenZhengZhaoId + "View").remove();
                        }
                        fileupload.rebindFileButtonView(['YunYingZhiZhaoId','FaRenShenFenZhengZhaoId']);
                    } else {
                        tipdialog.errorDialog("请求数据失败");
                    }
                });
            };
            //获取数据
            function getQiYeRenZhengXinXi(id, callback) {
                //调用获取单条信息接口
                helper.Ajax("000000000004", id, function (resultdata) {
                    if (typeof callback == 'function') {
                        callback(resultdata);
                    }
                }, false);
            };
            //绑定数据
            function fillFormData(resource) {
                $('#Form1').find('input[name],select[name],textarea[name]').each(function (i, item) {
                    $(item).val('');
                    var tempValue = resource[$(item).attr('name')];
                    if (tempValue != undefined) {
                        $(item).val(tempValue.toString() == '' ? '' : tempValue);
                    } else {
                        $(item).val('');
                    }
                });
                $('#Form1').find('.form-control-static').each(function (i, item) {
                    $(item).html('');
                    var index = $(item).attr('for');
                    var tempValue = resource[index];
                    if (tempValue != undefined) {
                        $(item).html(tempValue == '' ? '' : tempValue);
                    } else {
                        $(item).html('');
                    }
                });
                $('#Id').val(resource.Id);
            };
            initPage();
        });
});