define(['/Modules/Config/conwin.main.js', '/Modules/GPSDAGL/CheLiangDangAn/Config/config.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'btn', 'fileupload', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, btn) {
            //var UserInfo = helper.GetUserInfo(); //用户信息
            var flagBae = false; //是否保存了车辆基础信息
            var flag = false; //是否保存了业户信息
            //var BaoXianId = "";

            var initPage = function (UserInfo) {
                UserInfo = UserInfo.body;
                //初始化页面样式
                common.AutoFormScrollHeight('#Form1');
                common.AutoFormScrollHeight('#Form2');
                common.AutoFormScrollHeight('#Form3');
                common.AutoFormScrollHeight('#Form4');
                formcontrol.initial();
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
                $('#tab2').on('click', function () {
                    $("#btn1Div").attr("style", "display:none")
                    $("#btn2Div").attr("style", "clear: both")
                });
                $('#tab1').on('click', function () {
                    $("#btn1Div").attr("style", "clear: both")
                    $("#btn2Div").attr("style", "display:none")
                });
                if (UserInfo.OrganizationType.toString() == "2") {
                    $("#YeHuOrgCode").val(UserInfo.OrganizationCode);
                    $("#QiYeMingCheng").val(UserInfo.OrganizationName);
                    $("#QiYeMingChengLabel").text(UserInfo.OrganizationName);

                };
                if (UserInfo.OrganizationType == 0 || UserInfo.OrganizationType == 11 || UserInfo.OrganizationType == 12) {
                    $("#btnSetYeHu").attr("style", "");
                }
                //关闭
                $('#btnclose').click(function (e) {
                    var CheliangId = $("#Id").val();
                    if (!flagBae && CheliangId.toString().trim() == "") {
                        //编写逻辑
                        tipdialog.confirm("确定放弃新增？", function (r) {
                            if (r) {
                                parent.window.$("#btnSearch").click();
                                popdialog.closeIframe();
                            }
                        });
                    }
                    else {
                        if (!flagBae) {
                            parent.window.$("#btnSearch").click();
                            popdialog.closeIframe();
                        }
                        else {
                            tipdialog.confirm("尚未完善详细信息，确定退出？", function (r) {
                                if (r) {
                                    parent.window.$("#btnSearch").click();
                                    popdialog.closeIframe();
                                }
                            });
                        }
                    }
                });

                $('#XingShiZHengSaoMiaoJian').fileupload({ multi: false });
                $("#btnSetGeTiHu").hide();

                //车主类型改变事件
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





                //选择业户
                $("#btnSetYeHu").on('click', function (e) {
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

                //快速录入车辆
                $("#btnKuaiSuLuRuCheLiang").on('click', function (e) {
                    e.preventDefault();
                    //tt201901211747 将Add页面的要传递到后台的企业组织代码和运营商组织代码清空
                    $("#Form4 #QiYeOrgCode").val('');
                    $("#Form4 #YunYingShangOG").val('');
                    popdialog.showModal({
                        'url': 'KuaiSuLuRuChengLiang.html',
                        'width': '1200px',
                        'showSuccess': GetKuaiSuLuRuCheLiang
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
                //选择车队
                $("#btnSetCheDui").on('click', function (e) {
                    e.preventDefault();
                    popdialog.showModal({
                        'url': 'CheDui.html',
                        'width': '1200px',
                        'showSuccess': GetCheDui
                    });
                });

                //保存车辆基础信息
                $('#btnSave1').on('click', function () {

                    //必填项校验
                    if (!btn.initial({ Id: "Form1" })) {
                        return;
                    }
                    var Data = $("#Form1").serializeObject();
                    var reg = /\s/g;
                    Data["ChePaiHao"] = $("#ChePaiHao").val().toUpperCase().replace(reg, "");
                    if (CheckChePaiHao(Data) == false) {
                        return;
                    };
                    Data["OrgId"] = $("#QiYeID").val();
                    Data["OrgType"] = 2;
                    Data["XiaQuSheng"] = $('#XiaQuSheng').val();
                    Data["XiaQuShi"] = $('#XiaQuShi').val();
                    Data["XiaQuXian"] = $('#XiaQuXian').val();
                    if (Data["XiaQuXian"].trim() == "") {
                        tipdialog.errorDialog('所属区域是必选项!');
                        return;
                    }
                    //保存数据
                    GetData("006600200038", Data, function (data) {
                        $("#Id").val(data);
                        flagBae = true;
                        toast.success("保存成功");
                    });
                });


                //保存车辆详细信息
                $('#btnSave2').on('click', function () {

                    //先判断是否保存了车辆基础信息
                    var CheliangId = $("#Id").val();
                    if (CheliangId.toString().trim() == "") {
                        tipdialog.errorDialog('请先保存车辆基础信息!');
                        return;
                    }

                    //必填项校验
                    if (!btn.initial({ Id: "Form2" })) {
                        return;
                    }

                    var Data = $("#Form2").serializeObject();
                    Data["CheliangId"] = CheliangId;
                    //数据校验部分


                    var s = ["3", "4", "8"];

                    //if (s.indexOf($("#CheLiangZhongLei").val()) >= 0) {
                    //    if (!Data["DunWei"]) {
                    //        tipdialog.errorDialog('吨位为必填项!');
                    //        return;
                    //    }
                    //}
                    if (CheckSubmit(Data) == false) {
                        return;
                    };

                    GetData("006600200040", Data, function (data) {
                        flagBae = false;
                        toast.success("保存成功");
                    })
                });






                //保存车辆数据上报设置
                $('#btnSave3').on('click', function () {
                    var ipreg = /^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/;
                    var numreg = /^[0-9]+.?[0-9]*$/;
                    //先判断是否保存了车辆基础信息
                    var CheliangId = $("#Id").val();
                    if (CheliangId.toString().trim() == "") {
                        tipdialog.errorDialog('请先保存车辆基础信息!');
                        return;
                    }
                    var Data = $("#Form3").serializeObject();
                    if (Data["GPSIpAddress"] && !ipreg.test(Data["GPSIpAddress"])) {
                        tipdialog.errorDialog('GPS数据上报IP的格式不正确！');
                        return;
                    }
                    if (Data["VideoIpAddress"] && !ipreg.test(Data["VideoIpAddress"])) {
                        tipdialog.errorDialog('智能视频上报IP的格式不正确！');
                        return;
                    }
                    if (Data["GPSPort"] && !numreg.test(Data["GPSPort"])) {
                        tipdialog.errorDialog('GPS数据上报PORT的格式不正确！');
                        return;
                    }
                    if (Data["VideoPort"] && !numreg.test(Data["VideoPort"])) {
                        tipdialog.errorDialog('智能视频上报PORT的格式不正确！');
                        return;
                    }
                    Data["CheliangId"] = CheliangId;
                    var list = [];
                    if (Data.IsZhuanWangShuJu == "1") {
                        list.push({ CheliangId: Data["CheliangId"], ShuJuShangBaoLeiXing: 1 });
                    }

                    if (Data.IsShangChuanYunZhenWang == "1") {
                        list.push({ CheliangId: Data["CheliangId"], ShuJuShangBaoLeiXing: 2 });
                    }
                    if (Data.IsBaoFeiCheLiang == "1") {
                        list.push({ CheliangId: Data["CheliangId"], ShuJuShangBaoLeiXing: 3 });
                    }

                    //保存数据
                    GetData("003300300106", { CheliangId: CheliangId, list: list, GPSIpAddress: Data["GPSIpAddress"], GPSPort: Data["GPSPort"], VideoIpAddress: Data["VideoIpAddress"], VideoPort: Data["VideoPort"] }, function (data) {
                        toast.success("保存成功");
                    })
                });
            };


            //级联城市下拉框
            function selectCity(UserInfo) {
                UserInfo = UserInfo.body;

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
                        $("#XiaQuShi").val(UserInfo.OrganCity);
                    }
                }, 'GetCityList', 'CityName');

                   //市级下拉改变时修改县级下拉内容
                $('#XiaQuShi').change(function () {
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
                            }
                        }, 'GetDistrictList', 'DistrictName');
                    }
                });
                //县政府和企业同时锁死县级选项
                if (UserInfo.OrganizationType != 0) {
                    selectcity.setXiaQu('00000020006', { "City": UserInfo.OrganCity }, '#XiaQuXian', function () {
                        if (UserInfo.OrganizationType != 11) {
                            var XiaQuXian = UserInfo.OrganDistrict;
                            if (XiaQuXian) {
                                var list = XiaQuXian;
                                $("#XiaQuXian").find("option").each(function (index, item) {
                                    if (list.indexOf($(item).val()) < 0 && $(item).val() != "") {
                                        $(item).remove();
                                    }
                                });
                                $("#XiaQuXian").val(XiaQuXian);
                            }
                        }
                    }, 'key', 'key', UserInfo.OrganCity);
                }


                if (UserInfo.OrganizationType != 0) {
                    document.getElementById("XiaQuShi").disabled = "disabled";
                    if (UserInfo.OrganizationType != 11) {
                        document.getElementById("XiaQuXian").disabled = "disabled";;
                    }
                }
                $('#XiaQuSheng').val('广东');
                document.getElementById("XiaQuSheng").disabled = "disabled";

            }
            



            //快速录入车辆的级联城市下拉框
            function selectKuaiSuLuRuCheLiangCity(UserInfo) {
                UserInfo = UserInfo.body;
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#FormCheLiangKuaiSuLuRu #XiaQuShi').empty().append(defaultOption);

                var data = { "Province": "广东" };///todo:初始化省份
                ///调用接口初始化城市下拉框
                selectcity.setXiaQu('00000020005', data, '#FormCheLiangKuaiSuLuRu #XiaQuShi', function () {
                    if (UserInfo.OrganizationType != 0) {
                        var XiaQuShi = UserInfo.OrganizationManageArea;
                        XiaQuShi = XiaQuShi.replace(/广东/g, "");
                        var list = XiaQuShi.split("|");
                        $("#FormCheLiangKuaiSuLuRu #XiaQuShi").find("option").each(function (index, item) {
                            if (list.indexOf($(item).val()) < 0 && $(item).val() != "") {
                                $(item).remove();
                            }
                        });
                    }
                }, 'GetCityList', 'CityName');

                $('#FormCheLiangKuaiSuLuRu #XiaQuShi').change(function () {
                    $('#XiaQuXian').empty().append(defaultOption);
                    var data = { "City": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化区县下拉框
                        selectcity.setXiaQu('00000020006', data, '#XiaQuXian', 'GetDistrictList', 'DistrictName');
                    }
                });

                $('#XiaQuXian').change(function () {
                    var data = { "District": $(this).val() };
                });
            }

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

                    //$("#JingYingXuKeZhengHao").val(rows[0].data.JingYingXuKeZhengHao);
                    if ($("#YeHuOrgType").val() == "2") {
                        $("#QiYeMingCheng").val(rows[0].data.OrgName);
                        $("#QiYeMingChengLabel").text(rows[0].data.OrgName);
                        $("#YeHuOrgCode").val(rows[0].data.OrgCode);
                        $("#QiYeID").val(rows[0].data.Id);
                    } else {
                        $("#GeTiHuMingCheng").val(rows[0].data.OrgName);
                        $("#YeHuOrgCode").val(rows[0].data.OrgCode);
                        $("#GeTiHuID").val(rows[0].data.Id);
                    }

                    $('#close1').trigger('click');
                });

            }

            //快速录入车辆的企业弹框
            function GetKuaiSuLuRuCheLiangYeHu() {
                InitYeHu($('#FormCheLiangKuaiSuLuRu #YunYingShangOG').val());
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
                        tipdialog.errorDialog('请选择企业信息');
                        return false;
                    }

                    if (rows.length != 1) {
                        tipdialog.errorDialog('仅可选择一个企业');
                        return false;
                    }
                    $("#FormCheLiangKuaiSuLuRu").val(rows[0].data.YeHuMingCheng);
                    $("#FormCheLiangKuaiSuLuRu #QiYeOrgCode").val(rows[0].data.BenDanWeiOrgCode);
                    $("#FormCheLiangKuaiSuLuRu #QiYeID").val(rows[0].data.Id);
                    $("#QiYeMingCheng").val(rows[0].data.QiYeMingCheng);
                    $("#QiYeMingChengLabel").text(rows[0].data.QiYeMingCheng);
                    $('#close1').trigger('click');
                });

            }
            //GPS运营商和分公司弹框
            function GetYunYingShang() {
                InitYunYingShang();
                $('#btnSearchYunYingShang').click(function (e) {
                    e.preventDefault();
                    $("#YUNYINGSHANG").CustomTable("reload");
                    $("#QiYeMingCheng").val('');
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
                    $("#JiGouMingCheng").val(rows[0].data.JiGouMingCheng);
                    $("#YunYingShangOG").val(rows[0].data.BenDanWeiOrgCode);
                    //$("#QiYeID").val(rows[0].data.Id);
                    $('#close1').trigger('click');
                });

            }
            //快速录入车辆的GPS运营商和分公司弹框
            function GetKuaiSuLuRuCheLiangYunYingShang() {
                InitYunYingShang();
                $('#btnSearchYunYingShang').click(function (e) {
                    e.preventDefault();
                    $("#YUNYINGSHANG").CustomTable("reload");
                    $("#FormCheLiangKuaiSuLuRu #QiYeMingCheng").val('');
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


                    $("#FormCheLiangKuaiSuLuRu #JiGouMingCheng").val(rows[0].data.JiGouMingCheng);
                    $("#FormCheLiangKuaiSuLuRu #YunYingShangOG").val(rows[0].data.BenDanWeiOrgCode);

                    $('#close1').trigger('click');
                });

            }

            //快速录入车辆
            function GetKuaiSuLuRuCheLiang() {
                var UserInfo = helper.GetUserInfo();
                if (UserInfo.OrganizationType.toString() != "4") {
                    $(".formk-1").hide();
                }
                //快速录入车辆的选择运营商
                $("#FormCheLiangKuaiSuLuRu #btnSetYunYingShang").on('click', function (e) {
                    e.preventDefault();
                    popdialog.showModal({
                        'url': 'YunYingShang.html',
                        'width': '1200px',
                        'showSuccess': GetKuaiSuLuRuCheLiangYunYingShang
                    });
                });
                //快速录入车辆页面选择业户
                $("#FormCheLiangKuaiSuLuRu #btnSetYeHu").on('click', function (e) {
                    var UserInfo = helper.GetUserInfo();
                    if ($("#FormCheLiangKuaiSuLuRu #JiGouMingCheng").val() == false && UserInfo.OrganizationType.toString() == "4") {
                        tipdialog.errorDialog('请先选择运营商!');
                        return;
                    }
                    e.preventDefault();
                    popdialog.showModal({
                        'url': 'YeHu.html',
                        'width': '1200px',
                        'showSuccess': GetKuaiSuLuRuCheLiangYeHu
                    });
                });
                helper.UserInfo(selectKuaiSuLuRuCheLiangCity);
                //时间控件
                $('.datepicker').datepicker({
                    language: 'zh-CN',
                    format: 'yyyy-mm-dd',
                    autoclose: true //选中之后自动隐藏日期选择框
                });
                //快速录入车辆的车主类型改变事件
                $("#FormCheLiangKuaiSuLuRu #YeHuOrgType").on("change", function () {
                    if ($(this).val() == "1") {
                        //企业所有
                        $("#FormCheLiangKuaiSuLuRu #GeRenCheZhuMingCheng").attr("disabled", "disabled");
                        $("#FormCheLiangKuaiSuLuRu #GeRenCheZhuMingCheng").val("");
                        $("#FormCheLiangKuaiSuLuRu #btnSetYeHu").show();

                    } else {
                        //个人所有
                        $("#FormCheLiangKuaiSuLuRu #GeRenCheZhuMingCheng").removeAttr("disabled");

                        $("#FormCheLiangKuaiSuLuRu #QiYeMingCheng").val("");
                        $("#FormCheLiangKuaiSuLuRu #QiYeOrgCode").val("");
                        $("#FormCheLiangKuaiSuLuRu #QiYeID").val("");
                        $("#FormCheLiangKuaiSuLuRu #btnSetYeHu").hide();
                    }
                });
                $('#FormCheLiangKuaiSuLuRu #XingShiZHengSaoMiaoJian').fileupload({ multi: false });
                //保存车辆快速录入信息
                $('#btnSaveCheLiangKuaiSuLuRu').on('click', function () {
                    if ($("#FormCheLiangKuaiSuLuRu").find("a").eq(1).html() == "删除") {
                        var zhi = $("#FormCheLiangKuaiSuLuRu").find("a").eq(1).attr("href");
                    }

                    var Data = $("#FormCheLiangKuaiSuLuRu").serializeObject();


                    //Data["ChePaiHao"] = $("#ChePaiHao1").val().toUpperCase();
                    var reg = /\s/g;
                    Data["ChePaiHao"] = $("#FormCheLiangKuaiSuLuRu #ChePaiHao").val().toUpperCase().replace(reg, "");
                    Data["XiaQuShi"] = $("#FormCheLiangKuaiSuLuRu #XiaQuShi").val();
                    Data["XingShiZhengDengJiRiQi"] = $("#FormCheLiangKuaiSuLuRu #XingShiZhengDengJiRiQi").val();
                    Data["XingShiZHengSaoMiaoJianId"] = zhi;
                    Data["YunYingShangOG"] = $("#FormCheLiangKuaiSuLuRu #YunYingShangOG").val();
                    Data["JiGouMingCheng"] = $("#FormCheLiangKuaiSuLuRu #JiGouMingCheng").val();
                    Data["YeHuOrgType"] = $("#FormCheLiangKuaiSuLuRu #YeHuOrgType").val();
                    Data["QiYeMingCheng"] = $("#FormCheLiangKuaiSuLuRu #QiYeMingCheng").val();
                    Data["QiYeDaiMa"] = $("#FormCheLiangKuaiSuLuRu #QiYeOrgCode").val();
                    Data["QiYeID"] = $("#FormCheLiangKuaiSuLuRu #QiYeID").val();
                    Data["GeRenCheZhuMingCheng"] = $("#FormCheLiangKuaiSuLuRu #GeRenCheZhuMingCheng").val();
                    if (CheckCheLiangKuaiSuLuRuBiTian(Data) == false) {
                        return;
                    };
                    //保存数据
                    GetData("003300300128", Data, function (data) {
                        $("#Id").val(data);
                        // flagBae = true;
                        toast.success("保存成功");
                        $('.close').trigger('click');
                    });
                });


            }


            //车队弹框
            function GetCheDui() {
                InitCheDui();
                $('#btnSearchCheDui').click(function (e) {
                    e.preventDefault();
                    $("#CheDui").CustomTable("reload");
                });
                //确定
                $('#btnYeHuCheDui').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#CheDui").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要车队信息');
                        return false;
                    }
                    if (rows.length != 1) {
                        tipdialog.errorDialog('仅可选择一个车队');
                        return false;
                    }
                    $("#CheDuiMingCheng").val(rows[0].data.JiGouMingCheng);
                    $("#CheDuiID").val(rows[0].data.Id);
                    $("#CheDuiOrgCode").val(rows[0].data.BenDanWeiOrgCode);
                    $('.close').trigger('click');
                });
            }

            //保险弹框
            //function GetBaoXian() {
            //    InitBaoXian();
            //    $('#btnSearchBaoXian').click(function (e) {
            //        e.preventDefault();
            //        $("#BaoXian").CustomTable("reload");
            //    });
            //    //确定
            //    $('#btnBaoXian').on('click', function (e) {
            //        e.preventDefault();
            //        var rows = $("#BaoXian").CustomTable('getSelection'),
            //            ids = [];
            //        if (rows == undefined) {
            //            tipdialog.errorDialog('请选择需要保险信息');
            //            return false;
            //        }
            //        if (rows.length != 1) {
            //            tipdialog.errorDialog('仅可选择一个保险机构');
            //            return false;
            //        }
            //        $("#BaoXianMingCheng").val(rows[0].data.JiGouMingCheng);
            //        $("#BaoXianID").val(rows[0].data.Id);
            //        $("#BaoXianOrgCode").val(rows[0].data.BenDanWeiOrgCode);
            //        $('.close').trigger('click');
            //    });
            //}

            //企业列表初始化
            function InitYeHu(OrgType) {
                $("#YeHu").CustomTable({
                    ajax: helper.AjaxData("006600200016",
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
                            var para = {
                                JiGouMingCheng: $("#ZiBiao_YUNYINGSHANGMINGCHENG").val()
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

            //车队列表初始化
            function InitCheDui() {
                $("#CheDui").CustomTable({
                    ajax: helper.AjaxData("003300300511", ///"00020003"为接口服务地址，需要在/Config/conwin.system.js中配置
                        function (data) {
                            var pageInfo = {
                                Page: data.start / data.length + 1,
                                Rows: data.length
                            };
                            for (var i in data) {
                                delete data[i];
                            }
                            var para = {
                                JiGouMingCheng: $("#CheDuiJiGouMingCheng").val()
                            };
                            pageInfo.data = para;
                            $.extend(data, pageInfo);
                        }, null),
                    single: false,
                    filter: false,
                    ordering: true, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [{
                        render: function (data, type, row) {
                            return '<input type=checkbox class=checkboxes />';
                        }
                    },
                    { data: 'JiGouMingCheng' }

                    ],
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        tableheadfix.ResetFix();
                    }
                });
            };
            //绑定基本信息数据方法
            function updateData() {
                var id = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                var CheliangId = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                GetData("003300300542", CheliangId, function (data) {
                    fillFormData(data, "Form1");
                    $("#Id").val(data["Id"]);

                });

                GetData("003300300050", CheliangId, function (data) {
                    fillFormData(data, "Form5");
                })
            };

            //根据服务编号获取信息
            function GetData(ServiceCode, data, callback) {
                helper.Ajax(ServiceCode, data, function (resultdata) {
                    if (typeof callback == 'function') {
                        if (typeof (resultdata) == "string") {
                            resultdata = JSON.parse(resultdata);
                        }
                        if (resultdata.publicresponse.statuscode == 0) {
                            callback(resultdata.body);
                        } else {
                            tipdialog.errorDialog('保存失败!' + resultdata.publicresponse.message);
                        }

                    }
                }, false);
            }



            //个性化代码块
            //region
            //endregion

            helper.UserInfo(initPage);
            helper.UserInfo(selectCity);

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
            function CheckCheLiangKuaiSuLuRuBiTian(Data) {
                var msg = "";
                if (Data["ChePaiHao"] == "") {
                    msg += "车牌号是必填项<br/>"
                } else {
                    var reg = /\s/g;
                    Data["ChePaiHao"] = Data["ChePaiHao"].replace(reg, "");
                    if (Data["ChePaiHao"].length > 8) {
                        msg += "请输入正确的车牌号!<br/>";
                    }
                }
                if (Data["ChePaiYanSe"] == "") {
                    msg += "车牌颜色是必选项<br/>"
                }

                if (Data["CheLiangZhongLei"] == "") {
                    msg += "车辆种类是必填项<br/>"
                }

                if (Data["CheZaiDianHua"] == "") {
                    msg += "车载电话是必填项<br/>"
                }
                if (Data["XiaQuShi"] == "") {
                    msg += "辖区市是必填项<br/>"
                }
                if (Data["XingShiZhengDengJiRiQi"] == "") {
                    msg += "行驶证登记日期是必填项<br/>"
                } else {
                    var re1 = /(([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})-(((0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)-(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8]))))|((([0-9]{2})(0[48]|[2468][048]|[13579][26])|((0[48]|[2468][048]|[3579][26])00))-02-29)/
                    if (!re1.test(Data["XingShiZhengDengJiRiQi"])) {
                        msg += "请输入正确的日期格式!<br/>";
                    }
                }

                //if (Data["XingShiZHengSaoMiaoJianId"] == "" || Data["XingShiZHengSaoMiaoJianId"] == undefined) {
                //    msg += "行驶证扫描件是必填项<br/>"
                //}
                if (Data["FaDongJiHao"] == "") {
                    msg += "发动机号是必填项<br/>"
                }

                if (Data["CheJiaHao"] == "") {
                    msg += "车架号是必填项<br/>"
                }
                if (Data["ZuoWei"] == "") {
                    msg += "座位是必填项<br/>"
                } else {
                    var re = /^(0|[1-9][0-9]*|-[1-9][0-9]*)$/;
                    if (!re.test(Data["ZuoWei"])) {
                        msg += "座位输入的不是整数，请输入整数!<br/>";

                    }
                }
                if (Data["DunWei"] == "") {
                    msg += "吨位是必填项<br/>"
                } else {
                    var re2 = /^[+-]?(0|([1-9]\d*))(\.\d+)?$/;
                    if (Data["DunWei"] != "") {

                        if (!re2.test(Data["DunWei"])) {
                            msg += "吨位输入的不是数字，请输入数字!<br/>";
                        }
                    }
                }
                if (Data["YeHuOrgType"] == "1") {
                    //业户类型为企业时 企业名称 经营许可证号为必填项
                    if (Data["QiYeMingCheng"] == undefined || Data["QiYeMingCheng"].trim() == "") {
                        msg += "企业名称是必填项<br/>"
                    }
                } else {
                    //业户类型为个人时 个人车主名称为必填项
                    if (Data["GeRenCheZhuMingCheng"] == undefined || Data["GeRenCheZhuMingCheng"].trim() == "") {
                        msg += "个人车主名称是必填项<br/>"
                    }
                }
                if (Data["LianXiFangShi"] == false) {
                    msg += "联系方式是必填项<br/>"
                }
                var UserInfo = helper.GetUserInfo();
                if (Data["JiGouMingCheng"] == false && UserInfo.OrganizationType.toString() == "4") {
                    msg += "运营商名称是必填项<br/>"

                }
                if (msg != "") {
                    tipdialog.errorDialog(msg);
                    return false;
                } else {
                    return true
                }

            }
            function CheckChePaiHao(Data) {
                var msg = "";
                //if (Data["ChePaiHao"] == "") {
                //    msg += "车牌号是必填项<br/>"
                //} else {
                //    var reg = /\s/g;
                //    Data["ChePaiHao"]= Data["ChePaiHao"].replace(reg, "");  
                //    if (Data["ChePaiHao"].length >8 ) {
                //            msg += "请输入正确的车牌号!<br/>";
                //        }
                //}
                if (msg != "") {
                    tipdialog.errorDialog(msg);
                    return false;
                } else {
                    return true
                }

            }
        });
});