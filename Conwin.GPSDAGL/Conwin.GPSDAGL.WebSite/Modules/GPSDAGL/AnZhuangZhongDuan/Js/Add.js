define(['/Modules/Config/conwin.main.js', '/Modules/GPSDAGL/CheLiangDangAn/Config/config.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'fileupload', 'btn', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, fileupload, btn) {

            var UserInfo = helper.GetUserInfo(); //用户信息

            var initPage = function () {
                //初始化页面样式
                common.AutoFormScrollHeight('#Form1');
                common.AutoFormScrollHeight('#Form2');
                common.AutoFormScrollHeight('#Form3');

                formcontrol.initial();

                initCameraNameData();
                //时间控件
                $('.datepicker').datepicker({
                    language: 'zh-CN',
                    format: 'yyyy-mm-dd',
                    autoclose: true //选中之后自动隐藏日期选择框
                });
                $('.datetimepicker').datetimepicker({
                    language: 'zh-CN',
                    startView: 1,
                    maxView: 0,
                    format: 'hh:ii',
                    autoclose: true //选中之后自动隐藏日期选择框
                });

                //GPS 装置服务期限
                $('#GPSDeviceInstallCertDate').datepicker({
                    language: 'zh-CN',
                    format: 'yyyy-mm-dd',
                    autoclose: true//选中之后自动隐藏日期选择框
                });
                $('#GPSDeviceInstallCertExpired').datepicker({
                    language: 'zh-CN',
                    format: 'yyyy-mm-dd',
                    autoclose: true//选中之后自动隐藏日期选择框
                });
                // 开始时间小于结束时间
                $('#GPSDeviceInstallCertDate').datepicker().on('changeDate', function (ev) {
                    if (ev.date == null) {
                        $('#GPSDeviceInstallCertExpired').datepicker('setStartDate', null);
                    } else {
                        if (ev.date.valueOf() != "") {
                            var d = new Date(ev.date.valueOf());
                            d.setFullYear(d.getFullYear() + 1);
                            $("#GPSDeviceInstallCertExpired").datepicker('setDate', d);
                            $("#GPSDeviceInstallCertExpired").datepicker('setStartDate', new Date(ev.date.valueOf()));
                        };
                    };
                });
                // 开始时间小于结束时间
                $('#GPSDeviceInstallCertExpired').datepicker().on('changeDate', function (ev) {
                    if (ev.date == null) {
                        $('#GPSDeviceInstallCertDate').datepicker('setEndDate', null);
                    } else {
                        if (ev.date.valueOf() != "") {
                            $("#GPSDeviceInstallCertDate").datepicker('setEndDate', new Date(ev.date.valueOf()));
                        };
                    };
                });
                //视频 装置服务期限
                $('#VideoDeviceInstallCertDate').datepicker({
                    language: 'zh-CN',
                    format: 'yyyy-mm-dd',
                    autoclose: true//选中之后自动隐藏日期选择框
                });
                $('#VideoDeviceInstallCertExpired').datepicker({
                    language: 'zh-CN',
                    format: 'yyyy-mm-dd',
                    autoclose: true//选中之后自动隐藏日期选择框
                });
                // 开始时间小于结束时间
                $('#VideoDeviceInstallCertDate').datepicker().on('changeDate', function (ev) {
                    if (ev.date == null) {
                        $('#VideoDeviceInstallCertExpired').datepicker('setStartDate', null);
                    } else {
                        if (ev.date.valueOf() != "") {
                            var d = new Date(ev.date.valueOf());
                            d.setFullYear(d.getFullYear() + 1);
                            $("#VideoDeviceInstallCertExpired").datepicker('setDate', d);
                            $("#VideoDeviceInstallCertExpired").datepicker('setStartDate', new Date(ev.date.valueOf()));
                        };
                    };
                });
                // 开始时间小于结束时间
                $('#VideoDeviceInstallCertExpired').datepicker().on('changeDate', function (ev) {
                    if (ev.date == null) {
                        $('#VideoDeviceInstallCertDate').datepicker('setEndDate', null);
                    } else {
                        if (ev.date.valueOf() != "") {
                            $("#VideoDeviceInstallCertDate").datepicker('setEndDate', new Date(ev.date.valueOf()));
                        };
                    };
                });

                //关闭
                $('#btnclose').click(function () {
                        popdialog.closeIframe();
                });

                //选择车辆
                $("#btnSelectVehicle").on('click', function (e) {
                    e.preventDefault();
                    popdialog.showModal({
                        'url': 'VehicleDialog.html',
                        'width': '1200px',
                        'showSuccess': GetVehicle
                    });
                });

                //选择代理商
                $("#btnSelectDaiLiShang").on('click', function (e) {
                    e.preventDefault();
                    popdialog.showModal({
                        'url': 'DaiLiShangDialog.html',
                        'width': '1200px',
                        'showSuccess': GetDaiLiShang
                    });
                });

                //选择本地服务商
                $("#btnSelectFuWuShang").on('click', function (e) {
                    e.preventDefault();
                    popdialog.showModal({
                        'url': 'FuWuShangDialog.html',
                        'width': '1200px',
                        'showSuccess': GetFuWuShang
                    });
                });

                //选择GPS 终端
                $("#btnSelectGPSZhongDuan").on('click', function (e) {
                    e.preventDefault();
                    popdialog.showModal({
                        'url': 'ZhongDuanDialog.html',
                        'width': '1200px',
                        'showSuccess': GetGPSZhongDuan
                    });
                });
                //选择智能视频 终端
                $("#btnSelectVideoZhongDuan").on('click', function (e) {
                    e.preventDefault();
                    popdialog.showModal({
                        'url': 'ZhongDuanDialog.html',
                        'width': '1200px',
                        'showSuccess': GetVideoZhongDuan
                    });
                });

                //GPS
                $('#btnSave1').on('click', function (e) {
                    e.preventDefault();

                    //必填项校验
                    if (!btn.initial({ Id: "Form1" })) {
                        return;
                    }
                    //必填项校验
                    if (!btn.initial({ Id: "Form2" })) {
                        return;
                    }
                    var Data = $("#Form1").serializeObject();
                    Data["ChePaiHao"] = $("#ChePaiHao").val();
                    Data["ChePaiYanSe"] = $("#ChePaiYanSe").val();
                    Data["DaiLiShangOrgName"] = $("#DaiLiShangOrgName").val();
                    Data["DaiLiShangOrgCode"] = $("#DaiLiShangOrgCode").val();
                    Data["FuWuShangOrgName"] = $("#FuWuShangOrgName").val();
                    Data["FuWuShangOrgCode"] = $("#FuWuShangOrgCode").val();

                    var Data2 = $("#Form2").serializeObject();
                    Data2["SheBeiXingHao"] = $("#SheBeiXingHao").val();
                    Data2["ZhongDuanLeiXing"] = $("#ZhongDuanLeiXing").val();

                    $.extend(Data, Data2);

                    var reg = /^[0-9]*$/;
                    if (!reg.test(Data["M1"])) {
                        tipdialog.errorDialog('M1必须为数字!');
                        return;
                    }

                    if (!reg.test(Data["IA1"])) {
                        tipdialog.errorDialog('IA1必须为数字!');
                        return;
                    }

                    if (!reg.test(Data["IC1"])) {
                        tipdialog.errorDialog('IC1必须为数字!');
                        return;
                    }

                    var SIMKaHao = Data["SIMKaHao"];
                    if (SIMKaHao != "") {
                        var re1 = /^\d{11,15}$/;
                        if (!re1.test(SIMKaHao)) {
                            tipdialog.errorDialog('SIM卡号应该只允许为11位到15位数字!');
                            return;
                        }
                    }

                    Data.CameraSelected = null;
                    if ($("#ShiFouAnZhuangShiPingZhongDuan").val() == "1") {
                        if ($("#ShiPingTouGeShu").val() == "" || $("#ShiPingTouGeShu").val() == undefined) {
                            tipdialog.errorDialog('视频头个数必填!');
                            return;

                        }
                        if ($("#VideoServiceKind").val() == "") {
                            tipdialog.errorDialog('没有选择厂商类型!');
                            return;
                        }
                        var camearselect = [];
                        if (!checkCamera()) {
                            return;
                        }
                        $('#CameraSelectedBox').find('[type="checkbox"]').each(function (i, item) {
                            if ($(item).parent().attr("class") == "checked") {
                                var lemp = $(item).attr("val") + "|" + $("select[cameraIndex=" + $(item).attr("val") + "]").val();
                                camearselect.push(lemp);
                            }
                        });
                        Data.CameraSelected = camearselect.join(",");  //摄像头数量
                    }
                    if (CheckSubmitfrm5(Data) == false) {
                        return;
                    };

                    console.log(Data);

                    //保存数据
                    helper.Ajax("003300300530", Data, function (data) {
                        if (data.body) {
                            toast.success("保存成功!");
                        } else {
                            tipdialog.errorDialog(data.publicresponse.message);
                        }
                    }, false);
                });

                //智能视频
                $('#btnSave2').on('click', function (e) {
                    e.preventDefault();

                    //必填项校验
                    if (!btn.initial({ Id: "Form1" })) {
                        return;
                    }

                    //必填项校验
                    if (!btn.initial({ Id: "Form3" })) {
                        return;
                    }

                    var Data = $("#Form1").serializeObject();
                    Data["ChePaiHao"] = $("#ChePaiHao").val();
                    Data["ChePaiYanSe"] = $("#ChePaiYanSe").val();
                    Data["DaiLiShangOrgName"] = $("#DaiLiShangOrgName").val();
                    Data["DaiLiShangOrgCode"] = $("#DaiLiShangOrgCode").val();
                    Data["FuWuShangOrgName"] = $("#FuWuShangOrgName").val();
                    Data["FuWuShangOrgCode"] = $("#FuWuShangOrgCode").val();

                    var Data2 = $("#Form3").serializeObject();
                    Data2["SheBeiXingHao"] = $("#SheBeiXingHao_v").val();
                    Data2["ShengChanChangJia"] = $("#ShengChanChangJia_v").val();
                    Data2["ChangJiaBianHao"] = $("#ChangJiaBianHao_v").val();
                    Data2["ZhongDuanBianMa"] = $("#ZhongDuanBianMa_v").val();

                    $.extend(Data, Data2);
                    var SIMKaHao = Data["SIMKaHao"];
                    if (SIMKaHao != "") {
                        var re1 = /^\d{11,15}$/;
                        if (!re1.test(SIMKaHao)) {
                            tipdialog.errorDialog('SIM卡号应该只允许为11位到15位数字!');
                            return;
                        }
                    }
                    console.log(Data);
                    //保存数据
                    helper.Ajax("003300300531", Data, function (data) {
                        if (data.body) {
                            toast.success("保存成功!");
                        } else {
                            tipdialog.errorDialog(data.publicresponse.message);
                        }
                    }, false);
                });

                // 是否安装摄像头
                $("#ShiFouAnZhuangShiPingZhongDuan").on("change", function () {
                    if ($(this).val() == "1") {
                        $("#ShiPingTouGeShu").removeAttr("disabled");
                        $("#VideoServiceKind").removeAttr("disabled");

                        $('#CameraSelectedBox :input').each(function (j, item) {
                            $(item).removeAttr("disabled", "disabled");
                        })
                        $('#CameraSelectedBox select').each(function (j, item) {
                            $(item).removeAttr("disabled", "disabled");
                        })
                    } else {
                        $("#ShiPingTouGeShu").val("");
                        $("#ShiPingTouGeShu").attr("disabled", "disabled");

                        $("#VideoServiceKind").val("");
                        $("#VideoServiceKind").attr("disabled", "disabled");

                        $('#CameraSelectedBox :input').each(function (j, item) {
                            $(item).val("");
                            $(item).attr("disabled", "disabled");
                            $(item).removeAttr("checked", "checked");
                            $(item).parent().removeAttr("class", "checked");
                        })
                        $('#CameraSelectedBox select').each(function (j, item) {
                            $(item).val("驾驶员");
                            $(item).attr("disabled", "disabled");
                        })

                    }
                });

            };

            //检查摄像头数量
            function checkCamera() {
                var num = 0;
                $('#CameraSelectedBox').find('[type="checkbox"]').each(function (i, item) {
                    if ($(item).attr("checked")) {
                        num++;
                    }
                });
                if (num != parseInt($("#ShiPingTouGeShu").val())) {
                    tipdialog.errorDialog("视频头数量不等于所选数量");
                    return false;
                }
                return true;
            }
            function initCameraNameData() {
                var CameraNameAry = ["驾驶员", "车辆正前方", "车前门", "车厢前部", "车厢后部", "车后门", "驾驶席车门", "其他"];
                var selectcontent = "";
                for (var i = 0; i < CameraNameAry.length; i++) {
                    selectcontent += '<option value="' + CameraNameAry[i] + '">' + CameraNameAry[i] + '</option>';
                }
                $(".CameraNameData").each(function (index, item) {
                    $(item).attr("cameraIndex", index + 1);
                    $(item).append(selectcontent);
                })
            }

            // 车辆弹框
            function InitVehicle() {
                $("#Vehicle").CustomTable({
                    ajax: helper.AjaxData("003300300537", 
                        function (data) {
                            var pageInfo = {
                                Page: data.start / data.length + 1,
                                Rows: data.length
                            };
                            for (var i in data) {
                                delete data[i];
                            }
                            var para = {
                                ChePaiHao: $("#D_ChePaiHao").val(),
                                ChePaiYanSe: $("#D_ChePaiYanSe").val()
                            };
                            pageInfo.data = para;
                            $.extend(data, pageInfo);
                        }, null),
                    single: true,
                    filter: false,
                    ordering: true, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [{
                        render: function (data, type, row) {
                            return '<input type=checkbox class=checkboxes />';
                        }
                    },
                        { data: 'ChePaiHao' },
                        { data: 'ChePaiYanSe' }
                    ],
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        tableheadfix.ResetFix();
                        $('#Vehicle_wrapper .table-scrollable').css({ 'height': '350px', 'overflow-y': 'auto' });

                    }
                });
            };
            function GetVehicle() {
                InitVehicle();
                $(".dataTables_empty").text("请先进行查询");
                $('#btnSearchVehicle').click(function (e) {
                    e.preventDefault();
                    $("#Vehicle").CustomTable("reload");
                });
                //确定
                $('#btnSureVehicle').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#Vehicle").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择车辆信息');
                        return false;
                    }

                    if (rows.length != 1) {
                        tipdialog.errorDialog('仅可选择一辆车');
                        return false;
                    }

                    $("#ChePaiHao").val(rows[0].data.ChePaiHao);
                    $("#ChePaiYanSe").val(rows[0].data.ChePaiYanSe);
                    $("#CheLiangId").val(rows[0].data.Id);
                    $('.close').trigger('click');
                });
            }

            // 代理商弹框
            function InitDaiLiShang() {
                $("#DaiLiShang").CustomTable({
                    ajax: helper.AjaxData("003300300503",
                        function (data) {
                            var pageInfo = {
                                Page: data.start / data.length + 1,
                                Rows: data.length
                            };
                            for (var i in data) {
                                delete data[i];
                            }
                            var para = {
                                JiGouMingCheng: $("#D_DaiLiShangName").val(),
                            };
                            pageInfo.data = para;
                            $.extend(data, pageInfo);
                        }, null),
                    single: true,
                    filter: false,
                    ordering: true, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [{
                        render: function (data, type, row) {
                            return '<input type=checkbox class=checkboxes />';
                        }
                    },
                        { data: 'JiGouMingCheng' },
                        { data: 'OrgCode' }
                    ],
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        tableheadfix.ResetFix();
                        $('#DaiLiShang_wrapper .table-scrollable').css({ 'height': '350px', 'overflow-y': 'auto' });

                    }
                });
            };
            function GetDaiLiShang() {
                InitDaiLiShang();
                $(".dataTables_empty").text("请先进行查询");
                $('#btnSearchDaiLiShang').click(function (e) {
                    e.preventDefault();
                    $("#DaiLiShang").CustomTable("reload");
                });
                //确定
                $('#btnSureDaiLiShang').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#DaiLiShang").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择平台代理商信息');
                        return false;
                    }

                    if (rows.length != 1) {
                        tipdialog.errorDialog('仅可选择一个代理商');
                        return false;
                    }

                    $("#DaiLiShangOrgName").val(rows[0].data.JiGouMingCheng);
                    $("#DaiLiShangOrgCode").val(rows[0].data.OrgCode);
                    $("#DaiLiShangId").val(rows[0].data.Id);
                    $('.close').trigger('click');
                });
            }

            // 本地服务商弹框
            function InitFuWuShang() {
                $("#FuWuShang").CustomTable({
                    ajax: helper.AjaxData("003300300507",
                        function (data) {
                            var pageInfo = {
                                Page: data.start / data.length + 1,
                                Rows: data.length
                            };
                            for (var i in data) {
                                delete data[i];
                            }
                            var para = {
                                JiGouMingCheng: $("#D_FuWuShangName").val(),
                            };
                            pageInfo.data = para;
                            $.extend(data, pageInfo);
                        }, null),
                    single: true,
                    filter: false,
                    ordering: true, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [{
                        render: function (data, type, row) {
                            return '<input type=checkbox class=checkboxes />';
                        }
                    },
                        { data: 'JiGouMingCheng' },
                    { data: 'OrgCode' }
                    ],
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        tableheadfix.ResetFix();
                        $('#FuWuShang_wrapper .table-scrollable').css({ 'height': '350px', 'overflow-y': 'auto' });

                    }
                });
            };
            function GetFuWuShang() {
                InitFuWuShang();
                $(".dataTables_empty").text("请先进行查询");
                $('#btnSearchFuWuShang').click(function (e) {
                    e.preventDefault();
                    $("#FuWuShang").CustomTable("reload");
                });
                //确定
                $('#btnSureFuWuShang').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#FuWuShang").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择本地服务商信息');
                        return false;
                    }

                    if (rows.length != 1) {
                        tipdialog.errorDialog('仅可选择一个本地服务商');
                        return false;
                    }

                    $("#FuWuShangOrgName").val(rows[0].data.JiGouMingCheng);
                    $("#FuWuShangOrgCode").val(rows[0].data.OrgCode);
                    $("#FuWuShangId").val(rows[0].data.Id);
                    $('.close').trigger('click');
                });
            }

            // 终端弹框
            function InitZhongDuan() {
                $("#ZhongDuan").CustomTable({
                    ajax: helper.AjaxData("003300300521",
                        function (data) {
                            var pageInfo = {
                                Page: data.start / data.length + 1,
                                Rows: data.length
                            };
                            for (var i in data) {
                                delete data[i];
                            }
                            var para = {
                                'ZhongDuanLeiXing': $('#D_ZhongDuanLeiXing').val(),
                                'SheBeiXingHao': $('#D_SheBeiXingHao').val(),
                                'ShengChanChangJia': $('#D_ShengChanChangJia').val(),
                                'ZhuangTai': '1'
                            };
                            pageInfo.data = para;
                            $.extend(data, pageInfo);
                        }, null),
                    single: true,
                    filter: false,
                    ordering: true, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [{
                        render: function (data, type, row) {
                            return '<input type=checkbox class=checkboxes />';
                        }
                    },
                        {
                            data: 'SheBeiXingHao'
                        },
                        {
                            data: 'ShengChanChangJia'
                        },
                        {
                            data: 'ChangJiaBianHao'
                        },
                        {
                            data: 'XingHaoBianMa'
                        },
                        {
                            data: 'ZhongDuanBianMa'
                        },
                        {
                            data: 'ShiYongCheXing'
                        },
                        {
                            data: 'DingWeiMoKuai'
                        },
                        {
                            data: 'TongXunMoShi'
                        },
                        {
                            data: 'GuoJianPiCi'
                        }
                    ],
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        tableheadfix.ResetFix();
                        $('#ZhongDuan_wrapper .table-scrollable').css({ 'height': '350px', 'overflow-y': 'auto' });

                    }
                });
            }
            function GetGPSZhongDuan() {
                $("#D_ZhongDuanLeiXing option[value = 5] ").remove();
                InitZhongDuan();
                $(".dataTables_empty").text("请先进行查询");
                $('#btnSearchZhongDuan').click(function (e) {
                    e.preventDefault();
                    $("#ZhongDuan").CustomTable("reload");
                });
                //确定
                $('#btnSureZhongDuan').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#ZhongDuan").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择终端信息');
                        return false;
                    }

                    if (rows.length != 1) {
                        tipdialog.errorDialog('仅可选择一个终端');
                        return false;
                    }

                    $("#SheBeiXingHao").val(rows[0].data.SheBeiXingHao);
                    $("#ZhongDuanLeiXing").val(rows[0].data.ZhongDuanLeiXing);
                    $("#ShengChanChangJia").val(rows[0].data.ShengChanChangJia);
                    $("#ChangJiaBianHao").val(rows[0].data.ChangJiaBianHao);
                    $("#XingHaoBianMa").val(rows[0].data.XingHaoBianMa);
                    $("#ShiYongCheXing").val(rows[0].data.ShiYongCheXing);
                    $("#DingWeiMoKuai").val(rows[0].data.DingWeiMoKuai);
                    $("#TongXunMoShi").val(rows[0].data.TongXunMoShi);
                    $("#GuoJianPiCi").val(rows[0].data.GuoJianPiCi);
                    $("#ZhongDuanBianMa").val(rows[0].data.ZhongDuanBianMa);
                    $("#GongGaoRiQi").val(rows[0].data.GongGaoRiQi);
                    $("#BeiZhu").val(rows[0].data.BeiZhu);
                    $("#SheBeiId").val(rows[0].data.Id);
                    
                    $('.close').trigger('click');
                });
            }
            function GetVideoZhongDuan() {
                $("#D_ZhongDuanLeiXing option[value != 5] ").remove();
                InitZhongDuan();
                $(".dataTables_empty").text("请先进行查询");
                $('#btnSearchZhongDuan').click(function (e) {
                    e.preventDefault();
                    $("#ZhongDuan").CustomTable("reload");
                });
                //确定
                $('#btnSureZhongDuan').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#ZhongDuan").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择终端信息');
                        return false;
                    }

                    if (rows.length != 1) {
                        tipdialog.errorDialog('仅可选择一个终端');
                        return false;
                    }

                    $("#SheBeiXingHao_v").val(rows[0].data.SheBeiXingHao);
                    $("#ShengChanChangJia_v").val(rows[0].data.ShengChanChangJia);
                    $("#ChangJiaBianHao_v").val(rows[0].data.ChangJiaBianHao);
                    $("#ZhongDuanBianMa_v").val(rows[0].data.ZhongDuanBianMa);
                    $("#SheBeiId_v").val(rows[0].data.Id);

                    $('.close').trigger('click');
                });
            }

            function CheckSubmitfrm5(Data) {
                var msg = "";
                var re = /^[0-9]*[1-9][0-9]*$/;
                var ZuiGaoSuDu = Data["ZuiGaoSuDu"];
                if (ZuiGaoSuDu != "") {

                    if (!re.test(ZuiGaoSuDu)) {
                        msg += "最高速度输入的不是正整数，请输入正整数!<br/>";
                    }
                }
                var ZuiDiSuDu = Data["ZuiDiSuDu"];
                if (ZuiDiSuDu != "") {

                    if (!re.test(ZuiDiSuDu)) {
                        msg += "最低速度输入的不是正整数，请输入正整数!<br/>";
                    }
                }
                if (ZuiGaoSuDu < ZuiDiSuDu) {
                    msg += "最高速度需大于或等于最低速度!<br/>";
                }
                var ShangXianDuanKou = Data["ShangXianDuanKou"];
                if (ShangXianDuanKou != "") {
                    if (!re.test(ShangXianDuanKou)) {
                        msg += "上线端口输入的不是正整数，请输入正整数!<br/>"
                    }
                }

                //var ZhongDuanHao = Data["ZhongDuanHao"];
                //if (ZhongDuanHao != "") {

                //    if (!re.test(ZhongDuanHao)) {
                //        msg += "终端号只能是正整数!<br/>";
                //    }
                //}

                //var IMEIKaHao = Data["IMEIKaHao"];
                //if (IMEIKaHao != "") {

                //    if (!re.test(IMEIKaHao)) {
                //        msg += "IMEIKaHao只能是正整数!<br/>";
                //    }
                //}
                var SIMKaHao = Data["SIMKaHao"];
                if (SIMKaHao != "") {
                    var re1 = /^\d{11,15}$/;
                    if (!re1.test(SIMKaHao)) {
                        msg += "SIM卡号应该只允许为11位到15位数字!<br/>";
                    }
                }
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