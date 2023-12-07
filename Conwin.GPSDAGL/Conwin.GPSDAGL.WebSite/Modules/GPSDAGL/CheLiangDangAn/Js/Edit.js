define(['/Modules/Config/conwin.main.js', '/Modules/GPSDAGL/CheLiangDangAn/Config/config.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'fileupload', 'btn', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, fileupload, btn) {
            var UserInfo = helper.GetUserInfo(); //用户信息

            if (UserInfo.OrganizationType.toString() == "2") {
                $("#btnSetYeHu").hide();
            };
            if (UserInfo.OrganizationType== 0) {
                $("#zhongduanxinxi_tab").show();
            };
            $('#tab2').on('click', function () {
                $("#btn1Div").attr("style", "display:none")
                $("#btn2Div").attr("style", "clear: both")
                $("#btn3Div").attr("style", "display:none")
            });
            $('#tab1').on('click', function () {
                $("#btn1Div").attr("style", "clear: both")
                $("#btn2Div").attr("style", "display:none")
                $("#btn3Div").attr("style", "display:none")
            });
            $('#tab3').on('click', function () {
                $("#btn1Div").attr("style", "display:none")
                $("#btn2Div").attr("style", "display:none")
                $("#btn3Div").attr("style", "clear: both")
            });
            var initPage = function () {
                //初始化页面样式
                common.AutoFormScrollHeight('#Form1');
                common.AutoFormScrollHeight('#Form2');
                formcontrol.initial();
                selectCity();
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

                //翻页控件
                var ids = window.parent.document.getElementById('hdIDS').value;
                prevNextpage.initPageInfo(ids.split(','));
                prevNextpage.bindPageClass();
                //初始化子表
                //关闭
                $('#btnclose').click(function () {
                    tipdialog.confirm("确定关闭？", function (r) {
                        if (r) {
                            parent.window.$("#btnSearch").click();
                            popdialog.closeIframe();
                        }
                    });
                });
                //上一条
                $('#prevBtn').click(function (e) {
                    e.preventDefault();
                    prevNextpage.prev();
                    updateData();
                    updateTag();
                });
                //下一条
                $('#nextBtn').click(function (e) {
                    e.preventDefault();
                    prevNextpage.next();
                    updateData();
                    updateTag();
                });




                $("#YeHuOrgType").on("change", function () {
                    if ($(this).val() == "2") {
                        //企业所有
                        $("#GeTiHuMingCheng").val("");
                        $("#GeTiHuID").val("");
                        $("#btnSetGeTiHu").hide();
                        $("#btnSetYeHu").show();
                    } else {
                        //个人所有
                        $("#QiYeMingCheng").val("");
                        $("#QiYeID").val("");
                        $("#btnSetYeHu").hide();
                        $("#btnSetGeTiHu").show();
                    }
                });



                $('#XingShiZHengSaoMiaoJian').fileupload({ multi: false });

                //车辆详细信息
                $('#tab2').on('click', function () {
                    var CheliangId = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                    GetData("006600200003", CheliangId, function (data) {

                        var fileId1 = $("#XingShiZHengSaoMiaoJianId").val();
                        if (fileId1) {
                            $("#" + fileId1 + "View").remove();
                        }

                        if (data) {
                            fillFormData(data, "Form2");
                            $('#CheLiangExId').val(data.Id);
                            fileupload.rebindFileButtonEdit(['XingShiZHengSaoMiaoJianId']);
                            if (data["XingShiZHengSaoMiaoJianId"]) {
                                $("#XingShiZHengSaoMiaoJian").remove();
                            }

                        }
                    })
                });
                //终端信息
                $('#tab3').on('click', function () {
                    var CheliangId = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                    GetData("006600200068", CheliangId, function (data) {
                        if (data) {
                            var param = data.GpsInfo;
                            if (data.VideoInfo) {
                                Object.assign(param, data.VideoInfo);
                            }
                            //param.VideoDeviceMDT = data.VideoInfo.VideoDeviceMDT;
                            if (param) {
                                fillFormData(param, "Form3");
                            }

                            var arrayCameraSelected = [];
                            $("input[name='CameraSelected']").removeAttr("checked")
                            $("input[name='CameraSelected']").parent().removeAttr("class");
                            if (data.GpsInfo?.ShiPinTouAnZhuangXuanZe) {
                                arrayCameraSelected = data.GpsInfo.ShiPinTouAnZhuangXuanZe.split(',');
                                $.each(arrayCameraSelected, function (i, item) {
                                    if (item != '') {
                                        var lemp = item.split("|");
                                        $("input[name='CameraSelected'][val=" + lemp[0] + "]").attr("checked", "checked");
                                        $("input[name='CameraSelected'][val=" + lemp[0] + "]").parent().attr("class", "checked");
                                        $("select[cameraIndex=" + lemp[0] + "] option[value = " + lemp[1] + "]").attr("selected", "selected");
                                        if (typeof lemp[2] == 'undefined' || lemp[2] != "0") {
                                            $("input[name='AudioSelected'][val=" + lemp[0] + "]").attr("checked", "checked");
                                            $("input[name='AudioSelected'][val=" + lemp[0] + "]").parent().attr("class", "checked");
                                        }
                                    }

                                });
                            }
                        }
                    })
                });




                //选择业户
                $("#btnSetYeHu").on('click', function (e) {
                    e.preventDefault();
                    popdialog.showModal({
                        'url': 'YeHu.html',
                        'width': '1200px',
                        'showSuccess': GetYeHu
                    });
                });

                //选择gps运营商和分公司
                $("#btnSetYunYingShang").on('click', function (e) {
                    e.preventDefault();
                    popdialog.showModal({
                        'url': 'YunYingShang.html',
                        'width': '1200px',
                        'showSuccess': GetYunYingShang
                    });
                });


                /**
                **保存
                **/

                //修改车辆基本信息
                $('#btnSave1').on('click', function () {

                    //必填项校验
                    if (!btn.initial({ Id: "Form1" })) {
                        return;
                    }
                    var Data = $("#Form1").serializeObject();
                    //保存数据

                    Data["Id"] = $("#Id").val();
                    Data["ChePaiHao"] = $("#ChePaiHao").val().toUpperCase();

                    Data["XiaQuSheng"] = $('#XiaQuSheng').val();
                    Data["XiaQuShi"] = $('#XiaQuShi').val();
                    Data["XiaQuXian"] = $('#XiaQuXian').val();
                    if (Data["XiaQuXian"].trim() == "") {
                        tipdialog.errorDialog('所属区域是必选项!');
                        return;
                    }
                    GetData("006600200004", Data, function (data) {
                        toast.success("保存成功");
                    });
                });

                //车辆详细信息修改
                $('#btnSave2').on('click', function () {

                    //必填项校验
                    if (!btn.initial({ Id: "Form2" })) {
                        return;
                    }

                    var Data = $("#Form2").serializeObject();
                    Data["CheliangId"] = $("#Id").val();


                    //数据校验部分

                    //var s = ["3", "4", "8"];
                    //if (s.indexOf($("#CheLiangZhongLei").val()) >= 0) {
                    //    if (!Data["DunWei"]) {
                    //        tipdialog.errorDialog('吨位为必填项!');
                    //        return;
                    //    }
                    //}
                    if (CheckSubmit(Data) == false) {
                        return;
                    };
                    if ($("#CheLiangExId").val().trim) {
                        if ($("#CheLiangExId").val().trim() != "") {
                            Data["Id"] = $("#CheLiangExId").val();

                            GetData("006600200005", Data, function (data) {
                                toast.success("保存成功");
                            })
                        }
                        else {
                            GetData("006600200040", Data, function (data) {
                                toast.success("保存成功");
                            })

                        }
                    }
                });

                //保存车辆终端信息
                $('#SaveZhongDuanInfo').on('click', function () {

                    var Data = $("#Form3").serializeObject();

                    var CameraSelected = [];
                    $('#CameraSelectedBox').find('[name="Combination"]').each(function (i, item) {
                        var box1 = $(this).find('[name="CameraSelected"]');
                        var box2 = $(this).find('[name="AudioSelected"]');
                        var isCheckedBox1 = box1.parent().attr("class") == "checked";
                        var box1val = box1.attr("val");
                        var isCheckedBox2 = box2.parent().attr("class") == "checked";
                        var selectText = $(this).find("select").val();
                        if (isCheckedBox1) {
                            var isAudio = 0;
                            if (isCheckedBox2) isAudio = 1;
                            var lemp = box1val + "|" + selectText + "|" + isAudio;
                            CameraSelected.push(lemp);
                        }
                    });
                    var gpsInfo = {};

                    gpsInfo.SIMKaHao = Data["SIMKaHao"];
                    gpsInfo.ZhongDuanMDT = Data["ZhongDuanMDT"];
                    gpsInfo.M1 = Data["M1"];
                    gpsInfo.IA1 = Data["IA1"];
                    gpsInfo.IC1 = Data["IC1"];
                    gpsInfo.ShiFouAnZhuangShiPinZhongDuan = Data["ShiFouAnZhuangShiPinZhongDuan"];

                    gpsInfo.ShiPingChangShangLeiXing = Data["ShiPingChangShangLeiXing"];
                    gpsInfo.ShiPinTouGeShu = Data["ShiPinTouGeShu"];
                    gpsInfo.Remark = Data["Remark"];

                    if (gpsInfo.ShiFouAnZhuangShiPinZhongDuan == "1") {
                        if (gpsInfo.ShiPinTouGeShu == "" || gpsInfo.ShiPinTouGeShu == undefined) {
                            tipdialog.errorDialog('视频头个数必填!');
                            return;

                        }
                        if ($("#ShiPingChangShangLeiXing").val() == "") {
                            tipdialog.errorDialog('没有选择厂商类型!');
                            return;
                        }
                        if (!checkCamera()) {
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
                        gpsInfo.ShiPinTouAnZhuangXuanZe = CameraSelected.join(",");
                    }



                    var videoInfo = {};
                    videoInfo.VideoDeviceMDT = Data["VideoDeviceMDT"].trim();

                    var param = {};
                    param.CheLiangId = $("#Id").val();
                    param.GpsInfo = gpsInfo;
                    param.VideoInfo = videoInfo;


                    //保存数据
                    helper.Ajax("006600200069", param, function (data) {
                        if (data.body) {
                            toast.success("保存成功!");
                        } else {
                            tipdialog.errorDialog(data.publicresponse.message);
                        }
                    }, false);

                });

                updateData();
                //个性化代码块


                $("#ShiFouAnZhuangShiPinZhongDuan").on("change", function () {
                    if ($(this).val() == "1") {
                        $("#ShiPinTouGeShu").removeAttr("disabled");
                        $("#ShiPingChangShangLeiXing").removeAttr("disabled");

                        $('#CameraSelectedBox :input').each(function (j, item) {
                            $(item).removeAttr("disabled", "disabled");
                        })
                        $('#CameraSelectedBox select').each(function (j, item) {
                            $(item).removeAttr("disabled", "disabled");
                        })
                    } else {
                        $("#ShiPinTouGeShu").val("");
                        $("#ShiPinTouGeShu").attr("disabled", "disabled");

                        $("#ShiPingChangShangLeiXing").val("");
                        $("#ShiPingChangShangLeiXing").attr("disabled", "disabled");

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

                //检查摄像头数量
                function checkCamera() {
                    var num = 0;
                    $('#CameraSelectedBox').find('[type="checkbox"]', '[name="CameraSelected"]').each(function (i, item) {
                        if ($(item).attr("checked") && $(item).attr("name") == "CameraSelected") {
                            num++;
                        }
                    });
                    if (num != parseInt($("#ShiPinTouGeShu").val())) {
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
                //region
                //endregion
            };
            //检查摄像头数量
            //企业弹框
            function GetYeHu() {
                InitYeHu($("#YeHuOrgType").val());
                $('#btnSearchYeHu').click(function (e) {
                    e.preventDefault();
                    $("#YeHu").CustomTable("reload");
                });
                //确定
                $('#btnYeHuXuanZe').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#YeHu").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择业户信息');
                        return false;
                    }

                    if (rows.length != 1) {
                        tipdialog.errorDialog('仅可选择一个业户');
                        return false;
                    }

                    $("#JingYingXuKeZhengHao").val(rows[0].data.JingYingXuKeZhengHao);
                    if ($("#YeHuOrgType").val() == "2") {
                        $("#QiYeMingCheng").val(rows[0].data.OrgName);
                        $("#YeHuOrgCode").val(rows[0].data.OrgCode);
                        $("#QiYeID").val(rows[0].data.Id);
                    } else {
                        $("#GeRenCheZhuMingCheng").val(rows[0].data.OrgName);
                        $("#YeHuOrgCode").val(rows[0].data.OrgCode);
                        $("#GeTiHuID").val(rows[0].data.Id);
                    }
                    $('.close').trigger('click');
                });

            }
            //GPS运营商和分公司弹框
            function GetYunYingShang() {
                InitYunYingShang();
                $('#btnSearchYunYingShang').click(function (e) {
                    e.preventDefault();
                    $("#YUNYINGSHANG").CustomTable("reload");
                    //$("#QiYeMingCheng").val('');
                });
                //确定
                $('#btnYunYingShangXuanZe').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#YUNYINGSHANG").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择运营商信息');
                        return false;
                    }

                    if (rows.length != 1) {
                        tipdialog.errorDialog('仅可选择一个运营商');
                        return false;
                    }

                    //$("#JingYingXuKeZhengHao").val(rows[0].data.JingYingXuKeZhengHao);
                    //debugger;
                    $("#JiGouMingCheng").val(rows[0].data.JiGouMingCheng);
                    $("#YunYingShangOG").val(rows[0].data.BenDanWeiOrgCode);
                    //$("#QiYeID").val(rows[0].data.Id);
                    $('#close1').trigger('click');
                });

            }


            //企业列表初始化
            function InitYeHu(OrgType) {
                $("#YeHu").CustomTable({
                    ajax: helper.AjaxData("003300300533",
                        function (data) {
                            var pageInfo = {
                                Page: parseInt(data.start / data.length + 1),
                                // Page: data.start / data.length + 1,
                                Rows: data.length
                            };
                            for (var i in data) {
                                delete data[i];
                            }
                            var para = {
                                YeHuMingCheng: $("#ZiBiao_QiYeMingCheng").val(),
                                OrgType: OrgType
                            };
                            pageInfo.data = para;
                            $.extend(data, pageInfo);
                        }, null),
                    single: false,
                    filter: false,
                    ordering: false, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [{
                        data: 'Id',
                        render: function (data, type, row) {
                            return '<input type=checkbox class=checkboxes />';
                        }
                    },
                    { data: 'OrgName' },
                    { data: 'JingYingXuKeZhengHao' }
                        //{ data: 'JingYingXuKeZhengHao' }
                    ],
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        // $("#YeHu").CustomTable("reload");
                        tableheadfix.ResetFix();
                        $('#YeHu_wrapper .table-scrollable').css({ 'height': '350px', 'overflow-y': 'auto' });
                    }
                });
                //  tableheadfix.InitFix(system.OnlyTableFix);
            };
            //Gps运营商和分公司列表初始化
            function InitYunYingShang() {
                $("#YUNYINGSHANG").CustomTable({
                    ajax: helper.AjaxData("003300300130", ///"00020003"为接口服务地址，需要在/Config/conwin.system.js中配置
                        function (data) {
                            var pageInfo = {
                                Page: parseInt(data.start / data.length + 1),
                                // Page: data.start / data.length + 1,
                                Rows: data.length
                            };
                            for (var i in data) {
                                delete data[i];
                            }
                            //debugger;
                            var para = {
                                JiGouMingCheng: $("#ZiBiao_YUNYINGSHANGMINGCHENG").val(),
                                QiYeOrgCode: $("#QiYeOrgCode").val()//传企业的组织代码
                            };
                            pageInfo.data = para;
                            $.extend(data, pageInfo);
                        }, null),
                    single: false,
                    filter: false,
                    ordering: false, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [{
                        data: 'Id',
                        render: function (data, type, row) {
                            return '<input type=checkbox class=checkboxes />';
                        }
                    },
                    { data: 'JiGouMingCheng' },
                    ],
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        tableheadfix.ResetFix();
                        $('#YUNYINGSHANG_wrapper .table-scrollable').css({ 'height': '450px', 'overflow-y': 'auto' });
                    }
                });
                //  tableheadfix.InitFix(system.OnlyTableFix);
            };

            //绑定基本信息数据方法
            function updateData() {
                var id = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                var CheliangId = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                GetData("006600200039", CheliangId, function (data) {
                    fillFormData(data, "Form1");
                    $("#Id").val(data["Id"]);

                    $("#XiaQuShi").trigger("change");

                    //var selectedOption = $("#XiaQuXian").find("option");
                    //$(selectedOption).each(function (i, item) {
                    //    if (item.value == data["XiaQuXian"]) {
                    //        $(item).html(data["XiaQuXian"]);
                    //    }
                    //})

                    XiaQuShi = data["XiaQuShi"];
                    XiaQuXian = data["XiaQuXian"];
                    if (XiaQuXian) {
                        ///调用接口初始化城市下拉框
                        if (XiaQuShi) {
                            var citydata = { "City": XiaQuShi };
                            selectcity.setXiaQu('00000020006', citydata, '#XiaQuXian', function () { $("#XiaQuXian").val(XiaQuXian); }, 'GetCityList', 'CityName');
                        }

                    }

                });

            };

            /**
             * 根据服务编号获取数据
             * @param {string} ServiceCode 服务编号
             * @param {object} data  body数据
             * @param {function} callback 回调函数
             */
            function GetData(ServiceCode, data, callback) {
                helper.Ajax(ServiceCode, data, function (resultdata) {
                    if (typeof callback == 'function') {
                        if (typeof (resultdata) == "string") {
                            resultdata = JSON.parse(resultdata);
                        }
                        if (resultdata.publicresponse.statuscode == 0) {
                            callback(resultdata.body);
                        } else {
                            tipdialog.errorDialog('获取数据失败!' + resultdata.publicresponse.message);
                        }

                    }
                }, false);
            }



            /**
             * form表单数据填充
             * @param {JSON} resource 数据源
             * @param {string} Id form控件的Id 
             */
            function fillFormData(resource, Id) {

                $('#' + Id).find('input[name],select[name],textarea[name],label[name]').each(function (i, item) {
                    var tempValue = resource[$(item).attr('name')];
                    var UserInfo = helper.GetUserInfo();
                    if (UserInfo.OrganizationType == 4) {

                    }
                    if ($(item).attr('name') == "ShiFouAnZhuangShiPinZhongDuan") {
                        if (resource[$(item).attr('name')] != "1") {
                            $("#ShiPinTouGeShu").attr("disabled", "disabled");
                            $("#ShiPingChangShangLeiXing").attr("disabled", "disabled");
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

                    }
                    if (tempValue != undefined) {
                        if ($(item).hasClass('datetimepicker')) {
                            tempValue = tempValue.substr(11, 5);
                        }
                        if ($(item).hasClass('datepicker')) {
                            tempValue = tempValue.substr(0, 10);
                        }
                        if (item.localName == 'label') {
                            $(item).text(tempValue.toString() == '' ? '' : tempValue.toString());
                        }
                        //debugger;
                        //TODO: 赋值
                        $(item).val(tempValue.toString() == '' ? '' : tempValue.toString());

                    }
                });

                //设置下拉框的值
                if ($(this).siblings("select").length) {
                    var selectedOption = $(this).siblings("select").find("option");
                    $(selectedOption).each(function (i, item0) {
                        if (item0.value == tempValue) {
                            $(item).html(item0);
                        }
                    })
                };
            };

            //级联城市下拉框
            function selectCity() {
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#XiaQuShi, #XiaQuXian').empty().append(defaultOption);

                var data = { "Province": "广东" };///todo:初始化省份
                ///调用接口初始化城市下拉框
                selectcity.setXiaQu('00000020005', data, '#XiaQuShi', function () {
                    if (UserInfo.OrganizationType != 0) {
                        var list = UserInfo.OrganCity;
                        $("#XiaQuShi").find("option").each(function (index, item) {
                            if (list.indexOf($(item).val()) < 0 && $(item).val() != "") {
                                $(item).remove();
                            }
                        });
                    }

                }, 'GetCityList', 'CityName');

                $('#XiaQuShi').on("change", function () {
                    $('#XiaQuXian').empty().append(defaultOption);
                    var data = { "City": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化区县下拉框
                        selectcity.setXiaQu('00000020006', data, '#XiaQuXian', function () {
                            if ((UserInfo.OrganizationType == 12 || UserInfo.OrganizationType == 2) && UserInfo.OrganDistrict != "") {
                                var XiaQuXian = UserInfo.OrganDistrict;
                                if (XiaQuXian) {
                                    var list = XiaQuXian;
                                    $("#XiaQuXian").find("option").each(function (index, item) {
                                        if (list.indexOf($(item).val()) < 0 && $(item).val() != "") {
                                            $(item).remove();
                                        }
                                    });
                                }
                                $("#XiaQuXian").val(XiaQuXian);
                            };

                        }, 'GetDistrictList', 'DistrictName');
                    }
                });
                if (UserInfo.OrganizationType != 0) {
                    document.getElementById("XiaQuShi").disabled = "disabled";
                    if (UserInfo.OrganizationType != 11) {
                        document.getElementById("XiaQuXian").disabled = "disabled";;
                    }
                }
                $('#XiaQuSheng').val('广东');
                document.getElementById("XiaQuSheng").disabled = "disabled";
            }

            //个性化代码块
            initPage();
            function CheckSubmit(Data) {
                var msg = "";
                //var re = /^[0-9]+.?[0-9]*$/;
                var re = /(0|([1-9]\d*))\.\d+/;
                var res = /^[0-9]*[1-9][0-9]*$/;
                var paiQiLiang = Data["PaiQiLiang"];
                if (paiQiLiang != "") {

                    if (!(re.test(paiQiLiang) || res.test(paiQiLiang))) {
                        msg += "排气量输入的不是数字，请输入数字!<br/>";
                    }
                }
                var zongZhiLiang = Data["ZongZhiLiang"];
                if (zongZhiLiang != "") {

                    if (!(re.test(zongZhiLiang) || res.test(zongZhiLiang))) {
                        msg += "总质量输入的不是数字，请输入数字!<br/>";
                    }
                }
                var CheGao = Data["CheGao"];
                if (CheGao != "") {

                    if (!(re.test(CheGao) || res.test(CheGao))) {
                        msg += "车高输入的不是数字，请输入数字!<br/>";
                    }
                }
                var CheChang = Data["CheChang"];
                if (CheChang != "") {

                    if (!(re.test(CheChang) || res.test(CheChang))) {
                        msg += "车长输入的不是数字，请输入数字!<br/>";
                    }
                }
                var CheKuan = Data["CheKuan"];
                if (CheKuan != "") {

                    if (!(re.test(CheKuan) || res.test(CheKuan))) {
                        msg += "车宽输入的不是数字，请输入数字!<br/>";
                    }
                }
                var ZuoWei = Data["ZuoWei"];
                if (ZuoWei != "") {
                    if (!res.test(ZuoWei)) {
                        msg += "座位输入的不是正整数，请输入正整数!<br/>";
                    }
                }
                var DunWei = Data["DunWei"];
                if (DunWei != "") {

                    if (!(re.test(DunWei) || res.test(DunWei))) {
                        msg += "吨位输入的不是数字，请输入数字!<br/>";
                    }
                }

                if (msg != "") {
                    tipdialog.errorDialog(msg);
                    return false;
                } else {
                    return true
                }
            }

            $("#btnSetGeTiHu").on('click', function (e) {
                //if($("#JiGouMingCheng").val()==false&&UserInfo.OrganizationType.toString()=="4"){
                //     tipdialog.errorDialog('请先选择运营商!');
                //     return;
                // }
                e.preventDefault();
                popdialog.showModal({
                    'url': 'YeHu.html',
                    'width': '1200px',
                    'showSuccess': GetYeHu
                });
            });

        });
});