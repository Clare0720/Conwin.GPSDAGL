define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'searchbox', 'customtable', 'bootstrap-datepicker.zh-CN', 'permission'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system, selectcity) {
            var initPage = function () {
                initlizableTable();
                $('#btnSearch').click(function (e) {
                    e.preventDefault();
                    $("#tb_Template").CustomTable("reload")
                });
                $("#btnReset").click(function (e) {
                    e.preventDefault();
                    $('.searchpanel-form').find('input[type=text]:not(:disabled), select:not(:disabled)').val('')
                });
                $("#btnCreate").click(function (e) {
                    e.preventDefault();
                    popdialog.showIframe({
                        'url': 'Add.html',
                        head: false
                    })
                });
                $('#btnEdit').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要修改的行');
                        return false
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('一次仅允许修改一条');
                        return false;
                    }
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id)
                    });
                    $('#hdIDS').val(ids.join(','));
                    popdialog.showIframe({
                        'url': 'Edit.html',
                        head: false
                    });                   
                });
                $('#btnDel').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要删除的行');
                        return false
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('一次仅允许删除一条');
                        return false;
                    }
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id)
                    });
                    tipdialog.confirm("确定要删除选中的记录？", function (r) {
                        if (r) {
                            helper.Ajax("003300300118", ids, function (data) {
                                if (data.body) {
                                    toast.success("删除成功");
                                    $("#tb_Template").CustomTable("reload")
                                } else {
                                    tipdialog.errorDialog('删除失败')
                                }
                            }, false)
                        }
                    });
                    
                });
                $('#btnView').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要查看的行');
                        return false
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('一次仅允许查看一条');
                        return false;
                    }
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id)
                    });
                    $('#hdIDS').val(ids.join(','));
                    popdialog.showIframe({
                        'url': 'View.html',
                        head: false
                    })
                });
                $(".date-picker").datepicker();
                selectCity()
            }

            function initlizableTable() {
                $("#tb_Template").CustomTable({
                    ajax: helper.AjaxData("003300300115", function (data) {
                        var pageInfo = {
                            Page: Math.ceil(data.start / data.length + 1),
                            Rows: data.length
                        };
                        for (var i in data) {
                            delete data[i]
                        }
                        var para = $('.searchpanel-form').serializeObject();
                        $('.searchpanel-form').find('[disabled]').each(function (i, item) {
                            para[$(item).attr('name')] = $(item).val()
                        });
                        pageInfo.data = para;
                        $.extend(data, pageInfo)
                    }, null),
                    single: false,
                    filter: true,
                    ordering: true,
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [
                        {
                            render: function (data, type, row) {
                                return '<input type=checkbox class=checkboxes />'
                            }
                        },
                        { data: 'JieRuMingCheng' },
                        { data: 'XiaQuShi' },
                        { data: 'PingTaiBianHao' },
                        { data: 'JieRuMa' },
                        { data: 'JieRuMiMa' },
                        { data: 'ZhuLianLuIP' },
                        { data: 'ZhuLianLuDuanKou' },
                        {
                            data: 'JieRuShiJian', render: function (data, type, row) {
                                return data == undefined ? '' : data.substring(0, 10);
                            }
                        }],
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        tableheadfix.ResetFix();
                    }
                });
                tableheadfix.InitFix(system.OnlyTableFix);
            }

            function selectCity() {
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#XiaQuShi').empty().append(defaultOption);
                selectcity.setXiaQu('00000020005', {
                    "Province": "广东"
                }, '#XiaQuShi', 'GetCityList', 'CityName');
            }

            initPage();
        })
});