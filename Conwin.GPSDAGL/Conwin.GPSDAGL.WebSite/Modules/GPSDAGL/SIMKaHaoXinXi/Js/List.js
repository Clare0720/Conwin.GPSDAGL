define(['/Modules/Config/conwin.main.js', 'Js/Config.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'searchbox', 'customtable', 'bootstrap-datepicker.zh-CN', 'permission', 'Import'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system, selectcity) {
            var initPage = function () {
                initlizableTable();
                $('#btnSearch').on('click', function (e) {
                    e.preventDefault();
                    $("#tb_SIM").CustomTable("reload");
                });

                $("#btnReset").on('click', function (e) {
                    e.preventDefault();
                    $('.searchpanel-form').find('input[type=text]:not(:disabled), select:not(:disabled)').val('')
                });

                $('#btnExport').on('click', function (e) {
                    e.preventDefault();
                    $("#tb_SIM").CustomTable("reload");
                    var params = $('#tb_SIM').DataTable().ajax.params();
                    if (params != undefined) {
                        helper.Ajax('003300300138', params.data, function (data) {
                            var resFileId = data.body;
                            if (resFileId != undefined) {
                                if (resFileId != '') {
                                    window.location.href = helper.Route('00000080005', '1.0', system.ServerAgent) + '?id=' + resFileId;
                                } else {
                                    tipdialog.errorDialog('不存在记录');
                                }
                            } else {
                                tipdialog.errorDialog('导出失败');
                            }
                        });
                    } 
                });

                $("#btnCreate").on('click', function (e) {
                    e.preventDefault();
                    popdialog.showIframe({
                        'url': 'Add.html',
                        head: false
                    })
                });

                $('#btnEdit').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_SIM").CustomTable('getSelection');
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要修改的行');
                        return false
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('一次仅允许修改一条');
                        return false;
                    }
                    if (rows[0].data.ShiYongZhuangTai == 1) {
                        tipdialog.errorDialog('不允许修改已使用的记录');
                        return false;
                    }
                    $('#hdIDS').val(rows[0].data.Id);
                    popdialog.showIframe({
                        'url': 'Edit.html',
                        head: false
                    });                   
                });

                $('#btnDel').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_SIM").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要删除的行');
                        return false
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('一次仅允许删除一条');
                        return false;
                    }
                    if (rows[0].data.ShiYongZhuangTai == 1) {
                        tipdialog.errorDialog('不允许删除已使用的记录');
                        return false;
                    }
                    ids.push(rows[0].data.Id);
                    tipdialog.confirm("确定要删除选中的记录？", function (r) {
                        if (r) {
                            helper.Ajax("003300300134", ids, function (data) {
                                if (data.body) {
                                    toast.success("删除成功");
                                    $("#tb_SIM").CustomTable("reload");
                                } else {
                                    tipdialog.errorDialog('删除失败')
                                }
                            }, false)
                        }
                    });                    
                });

                $('#btnJieBang').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_SIM").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要解绑的行');
                        return false
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('一次仅允许解绑一条');
                        return false;
                    }
                    if (rows[0].data.ShiYongZhuangTai == 2) {
                        tipdialog.errorDialog('不允许解绑未使用的记录');
                        return false;
                    }
                    check(rows[0].data.SIMKaHao, function (list) {
                        var chePaiStr = "";
                        if (list.length > 0) {
                            chePaiStr = "已绑定车辆" + list.join();
                        }                       
                        tipdialog.confirm(chePaiStr + "确定要解绑此卡吗？", function (r) {
                            if (r) {
                                helper.Ajax("003300300136", rows[0].data.Id, function (data) {
                                    if (data.body == true) {
                                        toast.success("解绑成功");
                                        $("#tb_SIM").CustomTable("reload")
                                    } else {
                                        tipdialog.errorDialog('解绑失败');
                                    }
                                }, false)
                            }
                        });
                    });                    
                });

                $('#btnView').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_SIM").CustomTable('getSelection'),
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

                $('#btnImport').fileupload({
                    businessId: helper.NewGuid(),
                    modelUrl: 'Xls/SIM卡导入模板.xlsx',
                    callback: function (data) {
                        popdialog.closeModal();
                        helper.Ajax('003300300137', data.FileId, function (data) {
                            if (data.publicresponse.statuscode == 0) {
                                if (data.body.result == true) {
                                    toast.success("导入成功");
                                    $("#tb_SIM").CustomTable("reload");
                                } else {
                                    tipdialog.errorDialog(data.body.msg);
                                }
                            }
                            else {
                                tipdialog.errorDialog('导入失败,模板内容格式或网络发生错误，请检查后重试!');
                            }                           
                        }, false);
                    }
                });

                $(".date-picker").datepicker();

                selectCity()
            }

            function initlizableTable() {
                $("#tb_SIM").CustomTable({
                    ajax: helper.AjaxData("003300300131", function (data) {
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
                                return '<input type=checkbox class=checkboxes value=' + row.Id + ' />';
                            }
                        },
                        { data: 'SIMKaHao' },
                        { data: 'SIMXuLieHao' },
                        {
                            data: 'KaiKaRiQi',
                            render: function (data, type, row) {
                                return data == undefined ? '' : data.substring(0, 10);
                            }
                        },
                        {
                            data: 'XiaQuShi',
                            width: 30
                        },
                        {
                            data: 'LiuLiang',
                            render: function (data, type, row) {
                                return data == undefined ? '' : formatInt(data) + 'MB';
                            }
                        },
                        {
                            data: 'DanJia',
                            render: function (data, type, row) {
                                return data == undefined ? '' : formatDecimal(data) + '元/月';
                            }
                        },
                        {
                            data: 'ZhuangTai',
                            width: 30,
                            render: function (data, type, row) {
                                switch (data) {
                                    case 1:
                                        return '有效';
                                    case 2:
                                        return '无效';
                                    default:
                                        return '';
                                }
                            }
                        },
                        {
                            data: 'ShiYongZhuangTai',
                            width: 30,
                            render: function (data, type, row) {
                                switch (data) {
                                    case 1:
                                        return '已使用';
                                    case 2:
                                        return '未使用';
                                    default:
                                        return '';
                                }
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

            function formatDecimal(data) {
                return (Math.round(data * 100) / 100).toFixed(2);
            }

            function formatInt(data) {
                return Math.round(data).toString();
            }

            function initImport() {
                $('#fileUploadForm').attr('action', helper.Route('00000080000', '1.0', system.ServerAgent));
                $('#downLoadFile').attr('href', 'Xls/SIM卡导入模板.xlsx');
            }

            function check(SIMStr, callback) {
                helper.Ajax("003300300145", { SIMKaHao: SIMStr }, function (data) {
                    if (data.body != null) {
                        callback && callback(data.body);
                    } else {
                        tipdialog.errorDialog('解绑失败');
                    }
                });
            }

            initPage();
        })
});