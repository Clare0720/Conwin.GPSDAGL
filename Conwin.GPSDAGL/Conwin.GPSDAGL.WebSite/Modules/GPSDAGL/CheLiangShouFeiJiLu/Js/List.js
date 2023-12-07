define(['/Modules/Config/conwin.main.js', '/Modules/GPSDAGL/CheLiangShouFeiJiLu/Js/Config/config.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'tableheadfix', 'system', 'selectcity', 'fileupload2', 'searchbox', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN', 'permission'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, tableheadfix, system, selectcity, fileupload2, Metronic) {
            var initPage = function () {
                //初始化table
                initlizableTable();
                //查询
                $('#btnSearch').click(function (e) {
                    e.preventDefault();
                    if ($("#ChePaiHao").val().trim() != "" && $("#ChePaiHao").val().trim().length < 3) {
                        tipdialog.errorDialog('请输入合法的车牌号码（至少三位）');
                        return;
                    }
                    $('#XuFeiTiXin').val('');
                    $("#tb_Template").CustomTable("reload");
                    var para = $('.searchpanel-form').serializeObject();
                    $('#Count').text('');
                    helper.Ajax('003300300093', para, function (data) {
                        if (data.body) {
                            var ShouFeiCount = data.body.ShouFeiCount == null ? '0' : data.body.ShouFeiCount;
                            var ShouFeiJinECount = data.body.ShouFeiJinECount == null ? '0' : data.body.ShouFeiJinECount;
                            var ShiShouJinECount = data.body.ShiShouJinECount == null ? '0' : data.body.ShiShouJinECount;
                            var HuiKuanJinECount = data.body.HuiKuanJinECount == null ? '0' : data.body.HuiKuanJinECount;

                            var countText = '收费统计: ' + ShouFeiCount + '笔' +
                                ' 收费金额: ' + ShouFeiJinECount + '元' +
                                ' 实收金额: ' + ShiShouJinECount + '元' +
                                ' 汇款金额: ' + HuiKuanJinECount + '元';
                            $('#Count').text(countText);
                        }
                    }, false);
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

                if (window.location.search.indexOf('type=') != -1) {//1.如果是车辆新增成功跳转标记。

                    var type = Split(Split(window.location.search, 'type=')[1], '&')[0];
                    if (type == '1') {
                        var id = Split(window.location.search, 'id=')[1];
                        if (id != undefined && id != null) {
                            $('#hdTag').val(id);
                        }
                        $('#btnCreate').trigger('click');
                    }
                }
                //导入
                $('#btnImport').fileupload({
                    businessId: $('#Id').val(),
                    multi: false,
                    timeOut: 20000,
                    allowedContentType: 'xlsx|xls',
                    modelUrl: '收费记录导入模板.xls',
                    callback: function (data) {
                        //$("#ExcelID").val(data.FileId);
                        //$('#modelExecl').attr("href", '' + helper.Route('00000080004', '1.0', system.ServerAgent) + '?id=' + data.FileId);
                        //$('#' + data.FileId + 'View').hide();
                        //$('#' + data.FileId + 'Delete').hide();
                        //$('#modelExecl').hide();
                        //$('#msg').text('上传成功，请点击保存按钮添加数据.');
                        //$('#' + data.FileId + 'Delete').bind('click', function () {
                        //    $('#modelExecl').attr("src", '');
                        //});
                        $('#import-btn').attr('disabled', false);
                        $('#import-btn').click(function (e) {
                            e.preventDefault();
                            //alert('导入按钮');
                            //data-dismiss="modal"
                            popdialog.closeModal();
                            //var FileID = $("#ExcelID").val();
                            var FileID = data.FileId;
                            if ($.isEmptyObject(FileID) || FileID == '') {
                                tipdialog.errorDialog('请先上传文件!');
                                return;
                            }
                            //    popdialog.showModal({
                            //    'url': 'ImportExcel.html',
                            //    'width': '600px'
                            //});
                            helper.Ajax('003300300089', FileID, function (data) {
                                if (data.body.statuscode == 200) {
                                    //var json = response.data.result;
                                    var json = data.body.result;
                                    var html = '';
                                    html += '<div class="modal-dialog">';
                                    html += '<div class="modal-content">';
                                    html += '<div class="modal-header">';
                                    html += '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>';
                                    html += '<h4 class="modal-title">导入数据</h4>';
                                    html += '</div>';
                                    html += '<div class="modal-body">';
                                    html += "<p>完成：成功添加 " + json["SuccessCount"] + " 条记录</p>";
                                    html += '<p>失败: ' + json.ErrorLists.length + ' 条记录</p>';
                                    $.each(json.ErrorLists, function (i, item) {
                                        html += '<p style="font- size:10px;">车牌号: ' + item.ChePaiHao + " ,增加失败。错误信息:" + item.ErrorMsg + "</p>";
                                    });

                                    html += '</div>';
                                    html += '</div>';
                                    html += '</div>';
                                    $("#tb_Template").CustomTable("reload");
                                    popdialog.showModalHtml({
                                        'html': html,
                                        'width': 'normal'
                                    });
                                    //$('#msg').append('<p style="font-size:12px;">窗口将在3秒后自动关闭...</p>');
                                    //$('#btnImportSave').attr('disabled', 'disabled');
                                    //setTimeout(function () {
                                    //    $('.close').click();

                                    //}, 3000);
                                    //$(' .close').on('click', function () {
                                    //    setTimeout(function () { $("#tb_Template").CustomTable("reload"); }, 800);
                                    //});
                                } else {
                                    tipdialog.errorDialog('导入失败,模板内容格式或网络发生错误，请检查后重试!');
                                }
                            }, false);

                        });
                        //popdialog.showModal({
                        //    'url': 'ImportExcel.html',
                        //    'width': '600px',
                        //    'showSuccess': initImportExcel

                        //});
                        // });
                    }
                });
                //批量收费
                  $("#btnBatchShouFei").click(function (e) {
                    e.preventDefault();
                    //TODO:编写逻辑
                    popdialog.showIframe({
                        'url': 'PiLiangShouFeiList.html',
                        head: false
                    });
                });
                //导出
                $('#btnExport').click(function (e) {
                    e.preventDefault();
                   // var rows = $("#tb_Template").CustomTable('getSelection'), ids = [];
                    var para = $('.searchpanel-form').serializeObject();
                    console.log(para);
                    //if (rows != undefined) {
                    //    $(rows).each(function (i, item) {
                    //        ids.push(item.data.Id);
                    //    });
                    //}
                    //var req;
                    //if (ids.length >= 1) {
                    //    req = { 'ExCheckId': ids };
                    //} else {
                    //    req = para;
                    //}

                    helper.Ajax('003300300090', para, function (data) {
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

                //更新
                $('#btnEdit').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要更新的行');
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
                    var rows = $("#tb_Template").CustomTable('getSelection'), ids = [];
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
                            helper.Ajax("003300300087", ids, function (data) {
                                if (data.body) {
                                    toast.success("删除成功");
                                    //$("#tb_Template").CustomTable("reload");
                                    setTimeout(function () { $("#tb_Template").CustomTable("reload"); }, 800);
                                } else {
                                    tipdialog.errorDialog('删除失败');
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

                //个性化代码块
                //region
                $('.datepicker').datepicker({
                    language: 'zh-CN',
                    format: 'yyyy-mm-dd',
                    autoclose: true//选中之后自动隐藏日期选择框
                });
                $('.datetimepicker').datetimepicker({
                    language: 'zh-CN',
                    startView: 2,
                    maxView: 3,
                    format: 'yyyy-mm-dd hh:ii',
                    autoclose: true//选中之后自动隐藏日期选择框
                });
                selectCity();
                $(".date-picker").datepicker();
                //endregion            };

                //初始化选择车的界面

                //初始化列表界面
                function initlizableTable() {
                    $("#tb_Template").CustomTable({
                        ajax: helper.AjaxData("003300300083",///"00020003"为接口服务地址，需要在/Config/conwin.system.js中配置
                            function (data) {
                                var pageInfo = { Page: parseInt(data.start / data.length + 1), Rows: data.length };
                                for (var i in data) {
                                    delete data[i];
                                }
                                var para = $('.searchpanel-form').serializeObject();
                                $('.searchpanel-form').find('[disabled]').each(function (i, item) {
                                    para[$(item).attr('name')] = $(item).val();
                                });
                                para['XuFeiTiXin'] = $('#XuFeiTiXin').val();
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
                            { data: 'ChePaiHao' },
                            {
                                data: 'ChePaiYanSe'
                            },
                            {
                                data: 'YeHuXinXi', render: function (data, type, row) {
                                    if (data != null && data != undefined) {
                                        //return '<p style="width:100px;">' + data + '</p>';
                                        return data.toString();
                                    } else {
                                        return '';
                                    }
                                }
                            },
                            {
                                data: 'ShouFeiNeiRong'
                                , render: function (data, type, row) {
                                    switch (data) {
                                        case 1: return '卡流量费';
                                        case 2: return '终端费';
                                        case 3: return '续网';
                                        case 4: return '新装';
                                        case 5: return '转网';
                                        case 6: return '挂网';
                                        case 7: return '人工费';
                                        case 8: return '换机费';
                                        default: return ' ';
                                    }
                                }
                            },
                            {
                                data: 'ShouFeiJinE', render: function (data, type, row) {
                                    if (data != null && data != undefined) {
                                        return Math.round(data * 100) / 100;
                                    }
                                    return "";
                                }
                            }, 
                            {
                                data: 'ShiShouJinE', render: function (data, type, row) {
                                    if (data != null && data != undefined) {
                                        return Math.round(data * 100) / 100;
                                    }
                                    return "";
                                }
                            },
                            {
                                data: 'HuiKuanJinE', render: function (data, type, row) {
                                    if (data != null && data != undefined) {
                                        return Math.round(data * 100) / 100;
                                    }
                                    return "";
                                }
                            },
                            {
                                data: 'FuWuJieShuRiQi', render: function (data, type, row) {
                                    if (data != null && data != undefined) {
                                        return data.substr(0, 10);
                                    }
                                    return " ";
                                }
                            },
                            {
                                data: 'ShouFeiMoShi'
                                , render: function (data, type, row) {
                                    switch (data) {
                                        case 1: return '季度';
                                        case 2: return '半年';
                                        case 3: return '一年';
                                        case 4: return '二年';
                                        case 5: return '三年';
                                        default: return ' ';
                                    }
                                }
                            },
                            {
                                data: 'ShouFeiYouXiaoQi', render: function (data, type, row) {
                                    if (data != null && data != undefined) {
                                        return data.substr(0, 10);
                                    }
                                    return " ";
                                }
                            },
                            {
                                data: 'ShouFeiRiQi', render: function (data, type, row) {
                                    if (data != null && data != undefined) {
                                        return data.substr(0, 10);
                                    }
                                    return " ";
                                }
                            },
                        ],
                        "autoWidth": true
                        ,
                        pageLength: 10,
                        "fnDrawCallback": function (oSettings) {
                            tableheadfix.ResetFix();
                            //$('#XuFeiTiXin').val('');

                        }
                    });
                    tableheadfix.InitFix(system.OnlyTableFix);
                };
                //个性化代码块
                //region
                //分割方法
                function Split(str, tag) {
                    if (str == null || str == '' || typeof (str) != 'string') {
                        return '';
                    }
                    else if (tag == null || tag == '' || typeof (tag) != 'string') {
                        return str;
                    }
                    else {
                        return str.split(tag);
                    }
                }
                //选择城市
                function selectCity() {
                    var defaultOption = '<option value="" selected="selected">请选择</option>';
                    $('#XiaQuShi, #XiaQuXian').empty().append(defaultOption);
                    selectcity.setXiaQu('00000020005', { "Province": "广东" }, '#XiaQuShi', 'GetCityList', 'CityName');
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
                //endregion
                //批量收费
                //$("#btnBatchShouFei").on('click', function (e) {
                //    e.preventDefault();
                //    popdialog.showModal({
                //        'url': 'GetPiLiangShouFei.html',
                //        'width': '1220px',
                //        'showSuccess': GetPiLiangShouFei
                //    });
                //});
                //function GetPiLiangShouFei() {
                //    $(".date-picker").datepicker();
                //    //选择车辆按钮
                //    $('#btnXuanZeCL').on('click', function (e) {
                //        e.preventDefault();
                //        popdialog.showModal({
                //            'url': 'SearchCarList.html',
                //            'width': '1220px',
                //            'showSuccess': initSearchCarListZiBiao

                //        });

                //    });

                //}
               // var ciShu = 1;
               ////选择车辆
               // function initSearchCarListZiBiao() {
               //     //查询车辆
               //     $('#btnZiBiao').click(function (e) {
               //         $("#tb_Template_zb").CustomTable("reload");

               //     });
               //     //确定车辆
               //     $('#btnGetCarInfo').click(function (e) {
               //         e.preventDefault();
               //         var rows = $("#tb_Template_zb").CustomTable('getSelection'), ids = [];
               //         if (rows == undefined) {
               //             tipdialog.errorDialog('请选择需要查看的行');
               //             return false;
               //         }
               //         $(rows).each(function (i, item) {
               //             ids.push(item.data.Id);
               //         });
               //         //if (ids.length > 1) {
               //         //    tipdialog.errorDialog('只允许选择一行');
               //         //    return false;
               //         //}
               //         helper.Ajax("003300300131", ids, function (data) {
               //             if (data.body) {

               //                 var table = $("#tb_Template_zb1").DataTable();
               //                 table.rows.add([data.body]).draw();

               //             } else {
               //                 //请求错误处理。
               //                 tipdialog.alertMsg("车辆信息获取失败");
               //             }
               //         });
               //         popdialog.closeModal();
               //     });
               //     ChuShiHuaCheLiangLieBiao();

               //     if (ciShu) {
               //         ChuShiHuaShouFeiLieBiao();
               //         ciShu -= 1;
               //     }


               // }
            }
            //initPage
            ////批量收费列表
            //function ChuShiHuaShouFeiLieBiao() {
            //    $("#tb_Template_zb1").CustomTable({

            //        //single: false,
            //        //filter: true,
            //        //ordering: true, /////是否支持排序
            //        //"dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
            //        columns: [
            //            //{
            //            //    render: function (data, type, row) {
            //            //        return '<input type=checkbox class=checkboxes />';
            //            //    }
            //            //},
            //            {
            //                data: 'XuHao',
            //                render: function (data, type, row, meta) {
            //                    // return (meta.settings.oAjaxData.Page - 1) * meta.settings.oAjaxData.Rows + (meta.row + 1);
            //                    return 1;
            //                }
            //            },
            //            { data: 'ChePaiHao' },
            //            {data: 'ChePaiYanSe'},
            //            {
            //                render: function (data, type, row) {
            //                    return '<input type="text" name="FuWuQiShiRiQi"  id="FuWuQiShiRiQi" class="form-control datepicker" data-cwvalid=""/>';
            //                }
            //            },
            //            {
            //                render: function (data, type, row) {
            //                    return '<input type="text" name="FuWuJieShuRiQi"  id="FuWuJieShuRiQi" class="form-control required datepicker" data-cwvalid="{"blur":true, "required":true }" />';
            //                }
            //            },
            //            {
            //                render: function (data, type, row) {
            //                    return '<select name="ShouFeiMoShi" id="ShouFeiMoShi" class="form-control"> < option value= "1" > 季度</option ></select >';
            //                }
            //            },
            //            {
            //                render: function (data, type, row) {
            //                    return '<input type="text" name="ShouFeiYouXiaoQi" id="ShouFeiYouXiaoQi" class="form-control datepicker" data-cwvalid="" />';
            //                }
            //            },

            //        ],
            //        pageLength: 10,
            //        "fnDrawCallback": function (oSettings) {
            //            // tableheadfix.ResetFix();
            //        }
            //    });
            //}
            ////选择车辆列表
            //function ChuShiHuaCheLiangLieBiao() {
            //    //子表车辆列表
            //    $("#tb_Template_zb").CustomTable({
            //        ajax: helper.AjaxData("003300300091",///"00020003"为接口服务地址，需要在/Config/conwin.system.js中配置
            //            function (data) {
            //                var pageInfo = { Page: parseInt(data.start / data.length + 1), Rows: data.length };
            //                for (var i in data) {
            //                    delete data[i];
            //                }
            //                var para = { 'ChePaiHao': $('#ChePaiHaoZiBiao').val(), 'ChePaiYanSe': $('#ChePaiYanSeZiBiao').val() };
            //                pageInfo.data = para;
            //                $.extend(data, pageInfo);
            //            }, null),
            //        single: false,
            //        filter: true,
            //        ordering: true, /////是否支持排序
            //        "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
            //        columns: [
            //            {
            //                render: function (data, type, row) {
            //                    return '<input type=checkbox class=checkboxes />';
            //                }
            //            },
            //            { data: 'ChePaiHao' },
            //            {
            //                data: 'ChePaiYanSe'
            //            },
            //            { data: 'XiaQuShi' },
            //            { data: 'XiaQuXian' }

            //        ],
            //        pageLength: 10,
            //        "fnDrawCallback": function (oSettings) {
            //            // tableheadfix.ResetFix();
            //        }
            //    });
            //}
            initPage();
        });
});