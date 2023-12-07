define(['/Modules/Config/conwin.main.js', '/Modules/GPSDAGL/CheLiangShouFeiJiLu/Js/Config/config.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'tableheadfix', 'system', 'selectcity', 'fileupload2', 'searchbox', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN', 'permission'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, tableheadfix, system, selectcity, fileupload2, Metronic) {
            var initPage = function () {
                common.AutoFormScrollHeight("#JiChuXinXi", function (hg) {
                    //var boxHeight = hg - $('.portlet').outerHeight(true) - $('.nav-tabs').outerHeight(true) - $('.reduce2').outerHeight(true);
                    //var me = $(".scroller");
                    //me.parent().css('height', boxHeight);
                    //me.css('height', boxHeight);
                });
            
                ChuShiHuaShouFeiLieBiao();
                initData();
                $(".date-picker").datepicker();
                //保存
                $('#saveBtnPLShouFei').on('click', function (e) {
                    e.preventDefault();
                    var flags = CheckBiTian();
                    if (flags) {
                        if (!$('#tb_Template_zb1').DataTable().$("tr").data()) {
                            tipdialog.errorDialog('请选择车辆信息');
                            return;
                        }
                        SaveInfo();
                    }
                });
                //删除
                $('#btnDel').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template_zb1").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要删除的行');
                        return false;
                    }

                });
                //关闭
                $('#btnclose').click(function () {
                    tipdialog.confirm("确定关闭？", function (r) {
                        if (r) {
                            parent.window.$("#btnSearch").click();
                            popdialog.closeIframe();
                        }
                    });
                });
                //保存
                function SaveInfo() {
                    //TODO: 校验数据
                    var jsonData1 = $('#FormPLSF').serializeObject();
                    var jsonData2 = [];
                    jsonData2.push({
                        Id:jsonData1.Id,
                        ShouFeiNeiRong:jsonData1.ShouFeiNeiRong,
                        ShouFeiJinE:jsonData1.ShouFeiJinE,
                        ShouFeiRiQi:jsonData1.ShouFeiRiQi,
                        ShiShouJinE:jsonData1.ShiShouJinE,
                        HuiKuanJinE:jsonData1.HuiKuanJinE,
                        HuiKuanRiQi:jsonData1.HuiKuanRiQi,
                        ZhiFuFangShi:jsonData1.ZhiFuFangShi,
                        CaiWuShenHeRen:jsonData1.CaiWuShenHeRen,
                        CaiWuShenHeRiQi:jsonData1.CaiWuShenHeRiQi,
                        ShouKuanRen:jsonData1.ShouKuanRen,
                        ShouKuanBiaoZhun:jsonData1.ShouKuanBiaoZhun,
                        BeiZhu:jsonData1.BeiZhu,
                     })
                    //var table_zbData1 = $('#tb_Template_zb1').DataTable().$('input, select').serializeObject();
                    //  var table_zbData1 = $('#tb_Template_zb1').DataTable().rows().data();
                    // var commit_data = saveCredits();
                    //  var commit_data = [];
                    var commit_data = [];
                    var table = $("#tb_Template_zb1").DataTable();
                    table.$("tr").each(function () {
                        var data = table.row(this).data();
                        commit_data.push({
                            ChePaiHao: $(this).find('[name=ChePaiHao]').val(),
                            ChePaiYanSe: $(this).find('[name=ChePaiYanSe]').val(),
                            FuWuQiShiRiQi: $(this).find('[name=FuWuQiShiRiQi]').val(),
                            FuWuJieShuRiQi: $(this).find('[name=FuWuJieShuRiQi]').val(),
                            ShouFeiMoShi: $(this).find('[name=ShouFeiMoShi]').val(),
                            ShouFeiYouXiaoQi: $(this).find('[name=ShouFeiYouXiaoQi]').val(),
                            XiaQuSheng: data.XiaQuSheng,
                            XiaQuShi: data.XiaQuShi,
                            XiaQuXian: data.XiaQuXian,
                            YeHuXinXi: data.YeHuXinXi
                        });
                    });


                    //调用新增接口
                    helper.Ajax("003300300139", { commonShouFeiXinXi: jsonData2, uncommonShouFeiXinXi: commit_data }, function (data) {
                        if (data.body) {
                         parent.window.require(['toast'], function (Toastr) {
                            Toastr.success("档案保存成功");
                        });
                                //toast.success("档案保存成功");
                                parent.window.$("#btnSearch").click();
                                popdialog.closeIframe();

                        }
                        else {
                            tipdialog.alertMsg("档案保存失败");
                        }
                    }, false);
                };

                //选择车辆按钮
                $('#btnXuanZeCL').on('click', function (e) {
                    e.preventDefault();
                    popdialog.showModal({
                        'url': 'SearchCarList.html',
                        'width': '1220px',
                        'showSuccess': initSearchCarListZiBiao

                    });

                });
                //选择车辆
                function initSearchCarListZiBiao() {
                    //查询车辆
                    $('#btnZiBiao').click(function (e) {
                        if ($("#ChePaiHaoZiBiao").val().trim() != "" && $("#ChePaiHaoZiBiao").val().trim().length < 3) {
                            tipdialog.errorDialog('请输入合法的车牌号码（至少三位）');
                            return;
                        }
                        $("#tb_Template_zb").CustomTable("reload");

                    });
                    //确定车辆
                    $('#btnGetCarInfo').click(function (e) {
                        e.preventDefault();
                        var rows = $("#tb_Template_zb").CustomTable('getSelection'), ids = [];
                        if (rows == undefined) {
                            tipdialog.errorDialog('请选择需要查看的行');
                            return false;
                        }
                        $(rows).each(function (i, item) {
                            ids.push(item.data.Id);
                        });
                        //if (ids.length > 1) {
                        //    tipdialog.errorDialog('只允许选择一行');
                        //    return false;
                        //}
                        helper.Ajax("003300300141", { rows: 999999, page: 1, data: { ExCheckId: ids } }, function (data) {
                            if (data.body) {

                                var table = $("#tb_Template_zb1").DataTable();
                                table.rows.add(data.body.items).draw();

                            } else {
                                //请求错误处理。
                                tipdialog.alertMsg("车辆信息获取失败");
                            }
                        });
                        popdialog.closeModal();
                    });
                    ChuShiHuaCheLiangLieBiao();
                }
            }
            //initPage

            //选择车辆列表
            function ChuShiHuaCheLiangLieBiao() {
                //子表车辆列表
                $("#tb_Template_zb").CustomTable({
                    ajax: helper.AjaxData("003300300091",///"00020003"为接口服务地址，需要在/Config/conwin.system.js中配置
                        function (data) {
                            var pageInfo = { Page: parseInt(data.start / data.length + 1), Rows: data.length };
                            for (var i in data) {
                                delete data[i];
                            }
                            var para = { 'ChePaiHao': $('#ChePaiHaoZiBiao').val(), 'ChePaiYanSe': $('#ChePaiYanSeZiBiao').val() };
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
                        { data: 'XiaQuShi' },
                        { data: 'XiaQuXian' }

                    ],
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        $('#tb_Template_zb_wrapper .table-scrollable').css({ 'height': '420px', 'overflow-y': 'auto' });
                        // tableheadfix.ResetFix();
                    }
                });
            }
            //批量收费列表
            function ChuShiHuaShouFeiLieBiao() {
                $("#tb_Template_zb1").CustomTable({
                    ajax: {},
                    single: false,
                    deferRender: true,
                    // filter: true,
                    ordering: false, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row">',
                    //"dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [
                        {
                            data: 'XuHao',
                            render: function (data, type, row, meta) {
                                return meta.row + 1 + meta.settings._iDisplayStart;
                            }
                        },
                        {
                            data: 'ChePaiHao',
                            render: function (data, type, row) {
                                var $input = $('<input type="text" readonly  name="ChePaiHao" id="ChePaiHao" style="background-color:#fff"   class="form-control" value="' + (row.ChePaiHao ? row.ChePaiHao : '') + '"/>');
                                return $input.prop("outerHTML")
                            }

                        },
                        {
                            data: 'ChePaiYanSe',
                            render: function (data, type, row) {
                                var $input = $('<input type="text" readonly  name="ChePaiYanSe" id="ChePaiYanSe" style="background-color:#fff"  class="form-control" value="' + (row.ChePaiYanSe ? row.ChePaiYanSe : '') + '"/>');
                                return $input.prop("outerHTML")
                            }
                        },
                        {
                            data: 'FuWuQiShiRiQi',
                            render: function (data, type, row) {

                                var $input = $('<input type="text" name="FuWuQiShiRiQi"  id="FuWuQiShiRiQi"   class="form-control datepicker" autocomplete="off" value="' + (row.FuWuQiShiRiQi ? new Date(row.FuWuQiShiRiQi).Format('yyyy-MM-dd') : '') + '"/>');
                                return $input.prop("outerHTML")
                            }
                        },
                        {
                            data: 'FuWuJieShuRiQi',
                            render: function (data, type, row) {
                                var $input = $('<input type="text" name="FuWuJieShuRiQi" id="FuWuJieShuRiQi"  class="form-control datepicker" autocomplete="off" value="' + (row.FuWuJieShuRiQi ? new Date(row.FuWuJieShuRiQi).Format('yyyy-MM-dd') : '') + '"/>');
                                return $input.prop("outerHTML")
                            }
                        },
                        {
                            data: 'ShouFeiMoShi',
                            render: function (data, type, row) {
                                var $input = $('<select  name="ShouFeiMoShi" id="ShouFeiMoShi"  class="form-control" > <option  value="" >请选择收费模式</option ><option  value="1" > 季度</option ><option  value="2" > 半年</option ><option  value="3" selected> 一年</option ><option  value="4" > 二年</option ><option  value="5" > 三年</option ></select >');
                                return $input.prop("outerHTML")

                            }
                        },
                        {
                            data: 'ShouFeiYouXiaoQi',
                            render: function (data, type, row) {
                                var $input = $('<input type="text" name="ShouFeiYouXiaoQi" id="ShouFeiYouXiaoQi" autocomplete="off" class="form-control datepicker"    value="' + (row.ShouFeiYouXiaoQi ? new Date(row.ShouFeiYouXiaoQi).Format('yyyy-MM-dd') : '') + '"/>');
                                return $input.prop("outerHTML")

                            }
                        },
                        {
                            data: 'Do', render: function (data, type, row, meta) {
                                // debugger
                                return '<a href="#" class="Del"  data-del=' + row.Id + '>删除</a>';
                            }
                        }
                    ],
                    "pageLength": 99999,
                    "fnDrawCallback": function (oSettings,drawCallback) {
                        $(".datepicker").each(function (index, obj) {
                            if (!$(this).data("datepicker")) {
                                $(this).datepicker({
                                    language: 'zh-CN',
                                    format: 'yyyy-mm-dd',
                                    autoclose: true//选中之后自动隐藏日期选择框
                                });
                                $(this).datepicker("update", $(this).val());
                            }
                        });
                        $('.Del').click(function (e) {//点击删除
                            e.preventDefault();
                            var table = $('#tb_Template_zb1').DataTable();
                            table.row($(this).parents('tr')).remove();                           
                            var list = [];
                            for (var i = 0; i < table.data().length; i++) {
                                list.push(table.data()[i]);
                            }
                            table.clear().draw();
                            table.rows.add(list).draw();
                        });

                        //$('#tb_Template_zb1_wrapper .table-scrollable').css({ 'height': '350px', 'overflow-y': 'auto' });
                    }
                });
            }

            //初始化表单数据
            function initData() {
                $('#Id').val(helper.NewGuid());
                //跳转的传参.
                var tag = window.parent.document.getElementById('hdTag').value;
                var splits = [];
                var arr = tag.split(",");
                $(arr).each(function (i, item) {
                    splits.push(item);
                });
                if ( splits != '') {
                    helper.Ajax("003300300141", { rows: 999999, page: 1, data: { ExCheckId: splits } }, function (data) {
                        if (data.body) {
                            //查询到车辆信息。
                            //绑定信息到控件上。
                            //?并且将控件属性设置为只读？
                            var table = $("#tb_Template_zb1").DataTable();
                            table.rows.add(data.body.items).draw();

                        } else {
                            //请求错误处理。
                            tipdialog.alertMsg("车辆信息获取失败,请尝试点击【获取车辆资料】按钮添加收费记录");
                        }
                    }, false);
                }
            };

            //保存
            //function saveCredits() {
            //    var dataJson = "[";
            //    var ChePaiHao = "";
            //    var ChePaiYanSe = "";
            //    var FuWuQiShiRiQi = "";
            //    var FuWuJieShuRiQi = "";
            //    var ShouFeiMoShi = "";
            //    var ShouFeiYouXiaoQi = "";
            //    $("#tb_Template_zb1 tr").each(function (index, domEle) {// mainTable 下的tr     
            //        var ChePaiHao = "";
            //        var ChePaiYanSe = "";
            //        var FuWuQiShiRiQi = "";
            //        var FuWuJieShuRiQi = "";
            //        var ShouFeiMoShi = "";
            //        var ShouFeiYouXiaoQi = "";
            //        if (index != 0) {//遍历除去第一行的之外的所有input作为json数据传入后台  
            //            console.log($(domEle).find("input[type='text']"))
            //            $(domEle).find("input[type='text'],select").each(function (index, data) {
            //                if (index == 0) {
            //                    ChePaiHao = $(data).val();
            //                };
            //                if (index == 1) {
            //                    ChePaiYanSe = $(data).val();
            //                };
            //                if (index == 2) {
            //                    FuWuQiShiRiQi = $(data).val();
            //                };
            //                if (index == 3) {
            //                    FuWuJieShuRiQi = $(data).val();
            //                };
            //                if (index == 4) {
            //                    ShouFeiMoShi = $(data).val();
            //                };
            //                if (index == 5) {
            //                    ShouFeiYouXiaoQi = $(data).val();
            //                };
            //            });
            //            dataJson += "{" + "\"ChePaiHao\":\"" + ChePaiHao + "\"," + "\"ChePaiYanSe\":\"" + ChePaiYanSe + "\"," + "\"FuWuQiShiRiQi\":\"" + FuWuQiShiRiQi + "\"," + "\"FuWuJieShuRiQi\":\"" + FuWuJieShuRiQi + "\"," + "\"ShouFeiMoShi\":\"" + ShouFeiMoShi + "\"," + "\"ShouFeiYouXiaoQi\":\"" + ShouFeiYouXiaoQi + "\"},";
            //        }
            //    });

            //    if (dataJson.lastIndexOf(",")) {
            //        dataJson = dataJson.substring(0, dataJson.length - 1);
            //        if (dataJson != "") {
            //            dataJson += "]";
            //        } else { dataJson = ""; }
            //    }

            //    return JSON.parse(dataJson);
            //}


            //校验必填
            function CheckBiTian(Data) {
                var msg = "";

                if ($("#ShouFeiNeiRong").val() == "") {
                    msg += "收费内容是必填项<br/>"
                }
                if ($("#ShouFeiJinE").val() == "") {
                    msg += "收费金额(元)是必填项<br/>"
                } else {
                    var re1 = /^[0-9]+(.[0-9]{1,2})?$/
                    if (!re1.test($("#ShouFeiJinE").val())) {
                        msg += "收费金额(元)请输入正确的数字,不能超过两位小数!<br/>";
                    }
                }
                if ($("#ShouFeiRiQi").val() == "") {
                    msg += "收费日期是必填项<br/>"
                }
                if ($("#ShiShouJinE").val() == "") {
                    msg += "实收金额(元)是必填项<br/>"
                } else {
                    var re1 = /^[0-9]+(.[0-9]{1,2})?$/
                    if (!re1.test($("#ShiShouJinE").val())) {
                        msg += "实收金额(元)请输入正确的数字,不能超过两位小数!<br/>";
                    }
                }
                if ($("#HuiKuanJinE").val() == "") {
                    msg += "汇款金额(元)是必填项<br/>"
                } else {
                    var re1 = /^[0-9]+(.[0-9]{1,2})?$/
                    if (!re1.test($("#HuiKuanJinE").val())) {
                        msg += "汇款金额(元)请输入正确的数字,不能超过两位小数!<br/>";
                    }
                }
                if ($("#HuiKuanRiQi").val() == "") {
                    msg += "汇款日期是必填项<br/>"
                }
                //if (Data["XingShiZhengDengJiRiQi"] == "") {
                //    msg += "行驶证登记日期是必填项<br/>"
                //} else {
                //    var re1 = /(([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})-(((0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)-(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8]))))|((([0-9]{2})(0[48]|[2468][048]|[13579][26])|((0[48]|[2468][048]|[3579][26])00))-02-29)/
                //    if (!re1.test(Data["XingShiZhengDengJiRiQi1"])) {
                //        msg += "请输入正确的日期格式!<br/>";
                //    }
                //}
                if ($("#ZhiFuFangShi").val() == "") {
                    msg += "支付方式是必填项<br/>"
                }
                if ($("#ShouKuanRen").val() == "") {
                    msg += "收款人是必填项<br/>"
                }
                if ($("#ShouKuanBiaoZhun").val() == "") {
                    msg += "收款标准是必填项<br/>"
                }
                var fwqsrq = false;
                var fwjzrq = false;
                var sfms = false;
                var sfyxq = false;
                $("#tb_Template_zb1 tr").each(function (index, domEle) {// mainTable 下的tr     
                    if (index != 0) {//遍历除去第一行的之外的所有input作为json数据传入后台  

                        $(domEle).find("td").each(function (index1, data) {

                            if (index1 == 3) {
                                if ($(data).find('input').val() == "") {

                                    if (!fwqsrq) {
                                        msg += "服务起始日期是必填项<br/>";
                                        fwqsrq = true;
                                    }
                                }
                            };
                            if (index1 == 4) {
                                if ($(data).find('input').val() == "") {

                                    if (!fwjzrq) {
                                        msg += "服务截止日期是必填项<br/>";
                                        fwjzrq = true;
                                    }
                                }
                            };
                            if (index1 == 5) {
                                if ($(data).find('select').val() == "") {

                                    if (!sfms) {
                                        msg += "收费模式是必填项<br/>";
                                        sfms = true;
                                    }
                                }
                            };
                            if (index1 == 6) {
                                if ($(data).find('input').val() == "") {

                                    if (!sfyxq) {
                                        msg += "收费有效期是必填项<br/>";
                                        sfyxq = true;
                                    }
                                }
                            };
                        });

                    }
                });
                if (msg != "") {
                    tipdialog.errorDialog(msg);
                    return false;
                } else {
                    return true
                }

            }
            initPage();
        });
});