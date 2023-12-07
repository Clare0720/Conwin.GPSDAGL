define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'selectCity2', 'filelist', 'fileupload', 'dropdown', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, selectCity2, filelist, fileupload, dropdown) {
            //模块初始化
            var userInfo = helper.GetUserInfo();
            var initPage = function () {
                //getPersonalPosition();
                //初始化页面样式
                //common.AutoFormScrollHeight('#Form1');
                common.AutoFormScrollHeight('#Form1', function (hg) {
                    var boxHeight = hg - $('.portlet-title').outerHeight(true) - $('.nav-tabs').outerHeight(true) - 245;
                    var me = $(".scroller", '#Form1').eq(0);
                    me.parent().css('height', boxHeight);
                    me.css('height', boxHeight);
                });
                common.AutoFormScrollHeight('#Form2');
                common.AutoFormScrollHeight('#LianXiXinXi', function (hg) {
                    var boxHeight = hg - $('.portlet-title').outerHeight(true) - $('.nav-tabs').outerHeight(true) - 245;
                    var me = $(".scroller");
                    me.parent().css('height', boxHeight);
                    me.css('height', boxHeight);
                });
                //初始化checkbox
                $('.date-picker').datepicker({
                    language: 'zh-CN',
                    format: 'yyyy-mm-dd',
                    autoclose: true//选中之后自动隐藏日期选择框
                });
                formcontrol.initial();
                //翻页控件
                //var ids = window.parent.document.getElementById('hdIDS').value;
                //prevNextpage.initPageInfo(ids.split(','));
                //prevNextpage.bindPageClass();

                //关闭
                $('#btnclose').click(function () {
                    tipdialog.confirm("确定关闭？", function (r) {
                        if (r) {
                            parent.window.$("#btnSearch").click();
                            popdialog.closeIframe();
                        }
                    });
                });
                $("#GongShangYingYeZhiZhaoChangQiYouXiao").on('click', function (e) {
                    var isActivation = $('#Form1').find('[name="GongShangYingYeZhiZhaoChangQiYouXiao"]').parent().attr("class") == "checked";
                    if (isActivation) {
                        $("#GongShangYingYeZhiZhaoYouXiaoQi").val("");
                        document.getElementById("GongShangYingYeZhiZhaoYouXiaoQi").disabled = "disabled";
                    }
                    else {
                        document.getElementById("GongShangYingYeZhiZhaoYouXiaoQi").disabled = "";
                    }
                });
                $("#JingYingXuKeZhengChangQiYouXiao").on('click', function (e) {
                    var isActivation = $('#Form1').find('[name="JingYingXuKeZhengChangQiYouXiao"]').parent().attr("class") == "checked";
                    if (isActivation) {
                        $("#JingYingXuKeZhengYouXiaoQi").val("");
                        document.getElementById("JingYingXuKeZhengYouXiaoQi").disabled = "disabled";
                    }
                    else {
                        document.getElementById("JingYingXuKeZhengYouXiaoQi").disabled = "";
                    }
                });
                ////上一条
                //$('#prevBtn').click(function (e) {
                //    e.preventDefault();
                //    prevNextpage.prev();
                //    updateData();
                //});

                ////下一条
                //$('#nextBtn').click(function (e) {
                //    e.preventDefault();
                //    prevNextpage.next();
                //    updateData();
                //});

                $('#saveBtn').on('click', function (e) {
                    e.preventDefault();
                    var flags = true;
                    var msg = '';
                    var fromData = $('#Form1').serializeObject();
                    //fromData.ZhuangTai = $('#ZhuangTai').val();
                    if ($.trim(fromData.OrgName) == '') {
                        msg += "企业名称是必填项<br/>";
                    }
                    if ($.trim(fromData.OrgShortName) == '') {
                        msg += "企业简称是必填项<br/>";
                    }
                    fromData.XiaQuSheng = $('#XiaQuSheng').val();
                    fromData.XiaQuShi = $('#XiaQuShi').val();
                    fromData.XiaQuXian = $('#XiaQuXian').val();
                    fromData.JingYingFanWei = $('#JingYingFanWei').text().replace("选择辖区", "");
                    if ($.trim(fromData.JingYingFanWei) == '') {
                        msg += "经营区域是必填项<br/>";
                    }
                    if ($.trim(fromData.XiaQuXian) == '') {
                        msg += "所属区域是必选项<br/>";
                    }
                    if ($.trim(fromData.QiYeXingZhi) == '') {
                        msg += "企业性质是必选项<br/>";
                    }
                    if ($.trim(fromData.LianXiRen) == '') {
                        msg += "联系人是必填项<br/>";
                    }
                    if ($.trim(fromData.LianXiFangShi) == '') {
                        msg += "联系电话是必填项<br/>";
                    }
                    if ($.trim(fromData.JingYingXuKeZhengHao) == '') {
                        msg += "经营许可证号是必填项<br/>";
                    }
                    if ($.trim(fromData.GongShangYingYeZhiZhaoHao) == '') {
                        msg += "工商营业执照号是必填项<br/>";
                    }
                    var JingYingXuKeStatus = $('#Form1').find('[name="JingYingXuKeZhengChangQiYouXiao"]').parent().attr("class") == "checked";
                    if (!JingYingXuKeStatus) {
                        if ($.trim(fromData.JingYingXuKeZhengYouXiaoQi) == '') {
                            msg += "经营许可证有效期是必填项<br/>";
                        }
                        if (new Date(fromData.JingYingXuKeZhengYouXiaoQi).getTime() < new Date(getCurrentDate() + " 00:00").getTime()) {
                            msg += "经营许可证号有效期无效<br/>";
                        }
                    }
                    var GongShangYingYeZhiZhaoStatus = $('#Form1').find('[name="GongShangYingYeZhiZhaoChangQiYouXiao"]').parent().attr("class") == "checked";
                    if (!GongShangYingYeZhiZhaoStatus) {
                        if ($.trim(fromData.GongShangYingYeZhiZhaoYouXiaoQi) == '') {
                            msg += "工商营业执照有效期是必填项<br/>";
                        }
                        if (new Date(fromData.GongShangYingYeZhiZhaoYouXiaoQi).getTime() < new Date(getCurrentDate() + " 00:00").getTime()) {
                            msg += "工商营业执照有效期无效<br/>";
                        }
                    }
                    if (msg != '') {
                        flags = false;
                        tipdialog.alertMsg(msg);
                    }
                    if (flags) {
                        save();
                    }
                });
                //返回当前日期
                function getCurrentDate() {
                    var myDate = new Date();
                    //获取当前年
                    var year = myDate.getFullYear();
                    //获取当前月
                    var month = myDate.getMonth() + 1;
                    //获取当前日
                    var date = myDate.getDate();
                    var nowDate = year + '-' + p(month) + "-" + p(date);
                    return nowDate;

                    function p(s) {
                        return s < 10 ? '0' + s : s;
                    };
                };
                //返回一个增加天数后的日期
                function addDate(date, days) {
                    var d = new Date(date);
                    d.setDate(d.getDate() + days);
                    var m = d.getMonth() + 1;
                    return d.getFullYear() + '-' + m + '-' + d.getDate();
                }
                //手机号码校验
                function check_tellnum(content) {
                    // 正则验证格式
                    eval("var reg = /^1[34578]\\d{9}$/;");
                    return RegExp(reg).test(content);
                }

                updateData();
                initArea();

                selectCity2.initSelectView($('#JingYingFanWei'));

                if (userInfo.OrganizationType == "0") {
                    $('#add').click(function (e) {
                        e.preventDefault();
                        selectCity2.showSelectCity();
                    });
                }
                else {
                    var orgManageArea = userInfo.OrganProvince + userInfo.OrganCity;
                    if (typeof orgManageArea != "undefined" && orgManageArea != '') {
                        var manageArea = orgManageArea.split('|');
                        $('#add').click(function (e) {
                            e.preventDefault();
                            selectCity2.showSelectCity(manageArea);
                        });
                    }
                    else {
                        $('#add').click(function (e) {
                            e.preventDefault();
                            selectCity2.showSelectCity();
                        });
                    }
                }
                if (userInfo.OrganizationType != 0) {
                    selectCity2.setData(userInfo.OrganProvince + userInfo.OrganCity);
                    $('#add').attr("disabled", true); 
                    $(".closeSelected").remove()
                }
            };


            //根据组织代码获取组织基本信息
            function getYeHuBaseInfo(orgCode, callback) {
                helper.Ajax("006600200066", orgCode, function (resultdata) {
                    if (typeof callback == 'function') {
                        callback(resultdata);
                    }
                }, false);
            };

            //绑定基本信息数据方法
            function updateData() {
                var orgCode = userInfo.OrganizationCode;
                getYeHuBaseInfo(orgCode, function (serviceData) {
                    if (serviceData.publicresponse.statuscode == 0) {
                        if (serviceData.body == null) {
                            window.location.href = "/";
                        }
                        else {
                            fillFormData(serviceData.body);
                        }
                    } else {
                        tipdialog.errorDialog("请求数据失败");
                    }
                });
            };

            //绑定附件信息
            function fillFormData(resource) {
                $('#Form1').find('input[name],select[name],textarea[name]').each(function (i, item) {
                    $(item).val('');
                    var tempValue = resource[$(item).attr('name')];
                    if (tempValue != undefined) {
                        if ($(item).hasClass('datepicker')) {
                            tempValue = tempValue.substr(0, 10);
                        }
                        if ($(item).attr('type') == "checkbox") {
                            if (tempValue) {
                                $(item).attr("checked", "checked");
                                $(item).parent().attr("class", "checked");
                                if ($(item).attr('name') == "JingYingXuKeZhengChangQiYouXiao") {
                                    $("#JingYingXuKeZhengYouXiaoQi").val("");
                                    document.getElementById("JingYingXuKeZhengYouXiaoQi").disabled = "disabled";
                                }
                                if ($(item).attr('name') == "GongShangYingYeZhiZhaoChangQiYouXiao") {
                                    $("#GongShangYingYeZhiZhaoYouXiaoQi").val("");
                                    document.getElementById("GongShangYingYeZhiZhaoYouXiaoQi").disabled = "disabled";
                                }
                            }
                        }
                        $(item).val(tempValue.toString() == '' ? '' : tempValue);
                    } else {
                        $(item).val('');
                    }
                });
                //if ($("#ZhuangTai").val() == 1) {
                //    $("#zhuangtai").html("正常营业");
                //}
                //else {
                //    $("#zhuangtai").html("合约到期");
                //}
                //if ($("#ZhuangTai").val() == 1) {
                //    $("#YXZT").val("正常营业");
                //}
                //else {
                //    $("#YXZT").val("合约到期");
                //}
                if ($("#ShenHeZhuangTai").val() == 1) {
                    $("#ShenHeZhuangTaiInput").val("待提交")
                }
                else if ($("#ShenHeZhuangTai").val() == 2) {
                    $("#ShenHeZhuangTaiInput").val("待审核")
                }
                else if ($("#ShenHeZhuangTai").val() == 3) {
                    $("#ShenHeZhuangTaiInput").val("审核通过")
                }
                else if ($("#ShenHeZhuangTai").val() == 4) {
                    $("#ShenHeZhuangTaiInput").val("审核不通过")
                }
                //selectCity2.setData(resource['JingYingFanWei']);

                $("#jigoumingcheng").html($("#OrgName").val());
                $("#fuzeren").html($("#FuZheRen").val());
                $("#fuzerendianhua").html($("#FuZheRenDianHua").val());

                if ($('#QiYeBiaoZhiId').val() != '') {
                    var url = helper.Route('00000080004', '1.0', system.ServerAgent) + '?id=' + $('#QiYeBiaoZhiId').val();
                    $('#imgUpLoad').attr("src", url);
                }
                else {
                    $('#imgUpLoad').attr("src", '../../Component/Img/NotPic.jpg');
                }

                selectcity.setXiaQu('00000020004', {}, '#KongGuGongSiSuoZaiXiaQuSheng', 'GetProvinceList', 'Key', 'Key', resource.KongGuGongSiSuoZaiXiaQuSheng);
                selectcity.setXiaQu('00000020005', { "Province": resource.KongGuGongSiSuoZaiXiaQuSheng }, '#KongGuGongSiSuoZaiXiaQuShi', 'GetCityList', 'Key', 'Key', resource.KongGuGongSiSuoZaiXiaQuShi);
                //selectcity.setXiaQu('00000020004', {}, '#XiaQuSheng', '', 'Key', 'Key', resource.XiaQuSheng);
                selectcity.setXiaQu('00000020005', { "Province": "广东" }, '#XiaQuShi', function () {
                    if (userInfo.OrganizationType != 0) {
                        var XiaQuShi = userInfo.OrganCity;
                        if (XiaQuShi) {
                            var list = XiaQuShi.split("|");
                            $("#XiaQuShi").find("option").each(function (index, item) {
                                if (list.indexOf($(item).val()) < 0 && $(item).val() != "") {
                                    $(item).remove();
                                }
                            });
                        }
                    }
                    if (resource.XiaQuShi) {
                        $("#XiaQuShi").val(resource.XiaQuShi);
                    }
                }, 'key', 'key', resource.XiaQuShi);

                selectcity.setXiaQu('00000020006', { "City": resource.XiaQuShi }, '#XiaQuXian', function () {
                    if (userInfo.OrganizationType == 12) {
                        var XiaQuXian = userInfo.OrganDistrict;
                        if (XiaQuXian) {
                            var list = XiaQuXian;
                            $("#XiaQuXian").find("option").each(function (index, item) {
                                if (list.indexOf($(item).val()) < 0 && $(item).val() != "") {
                                    $(item).remove();
                                }
                            });
                        }
                    }
                    if (resource.XiaQuXian) {
                        $("#XiaQuXian").val(resource.XiaQuXian);
                    }
                }, 'key', 'key', resource.XiaQuShi);
                selectCity2.setData(resource.JingYingFanWei);
                if (userInfo.OrganizationType != 0) {
                    $('#add').attr("disabled", true);
                    $(".closeSelected").remove()
                }
                $('#XiaQuShi').on("change", function () {
                    var data = { "City": resource.XiaQuShi };
                    if (userInfo.OrganizationType == 0) {
                        data = { "City": $(this).val() };
                    }
                    if ($(this).val() != '') {
                        ///调用接口初始化区县下拉框
                        selectcity.setXiaQu('00000020006', data, '#XiaQuXian', function () {
                            if (userInfo.OrganizationType == 12) {
                                var XiaQuXian = userInfo.OrganDistrict;
                                if (XiaQuXian) {
                                    var list = XiaQuXian;
                                    $("#XiaQuXian").find("option").each(function (index, item) {
                                        if (list.indexOf($(item).val()) < 0 && $(item).val() != "") {
                                            $(item).remove();
                                        }
                                    });
                                }
                            }
                        }, function () {
                            $("#XiaQuXian").val(resource.XiaQuXian);
                        }, 'GetDistrictList', 'DistrictName');
                    }
                });
                $("#XiaQuXian").val(resource.XiaQuXian);
                if (userInfo.OrganizationType != 0) {
                    document.getElementById("XiaQuShi").disabled = "disabled";
                    if (userInfo.OrganizationType != 11) {
                        document.getElementById("XiaQuXian").disabled = "disabled";;
                    }
                }
                $('#XiaQuSheng').val('广东');
                document.getElementById("XiaQuSheng").disabled = "disabled";
            };

            //获取省市区信息
            var initArea = function () {
                //------------star-----
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#KongGuGongSiSuoZaiXiaQuSheng,#KongGuGongSiSuoZaiXiaQuShi').empty().append(defaultOption);
                ///调用接口初始化城市下拉框
                $('#KongGuGongSiSuoZaiXiaQuSheng').change(function () {
                    $('#KongGuGongSiSuoZaiXiaQuShi,#KongGuGongSiSuoZaiXiaQuShi').empty().append(defaultOption);
                    var data = { "Province": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化市下拉框
                        selectcity.setXiaQu('00000020005', data, '#KongGuGongSiSuoZaiXiaQuShi', 'GetCityList', 'CityName');
                    }
                });
                selectcity.setXiaQu('00000020004', {}, '#KongGuGongSiSuoZaiXiaQuSheng', 'GetProvinceList', 'Key', 'Key', '广东');
                selectcity.setXiaQu('00000020005', { "Province": $('#KongGuGongSiSuoZaiXiaQuSheng').val() }, '#KongGuGongSiSuoZaiXiaQuShi', 'GetCityList', 'CityListName');
                $('#KongGuGongSiSuoZaiXiaQuSheng').change();
                //------------end-----
                //------------star-----
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#XiaQuShi').empty().append(defaultOption);
                selectcity.setXiaQu('00000020005', { "Province": $('#XiaQuSheng').val() }, '#XiaQuShi', function () {
                    var XiaQuShi = userInfo.OrganizationManageArea;
                    if (XiaQuShi) {
                        XiaQuShi = XiaQuShi.replace(/广东/g, "");
                        var list = XiaQuShi.split("|");
                        $("#XiaQuShi").find("option").each(function (index, item) {
                            if (list.indexOf($(item).val()) < 0 && $(item).val() != "") {
                                $(item).remove();
                            }
                        });
                    }

                }, 'GetCityList', 'CityListName');

            };

            //主表-保存
            function save() {
                //TODO:数据校验
                var jsonData1 = $('#Form1').serializeObject();
                //jsonData1.ZhuangTai = $('#ZhuangTai').val();
                for (var key in jsonData1) {
                    jsonData1[key] = jsonData1[key].replace(/\s/g, "");
                }
                jsonData1.XiaQuSheng = $('#XiaQuSheng').val();
                jsonData1.XiaQuShi = $('#XiaQuShi').val();
                jsonData1.XiaQuXian = $('#XiaQuXian').val();
                jsonData1.JingYingFanWei = $('#JingYingFanWei').text().replace("选择辖区", "");
                jsonData1.JingYingXuKeZhengChangQiYouXiao = $('#Form1').find('[name="JingYingXuKeZhengChangQiYouXiao"]').parent().attr("class") == "checked";
                jsonData1.GongShangYingYeZhiZhaoChangQiYouXiao = $('#Form1').find('[name="GongShangYingYeZhiZhaoChangQiYouXiao"]').parent().attr("class") == "checked";
                jsonData1.IsConfirmInfo = 1;
                //调用修改接口
                helper.Ajax("006600200018", jsonData1, function (data) {
                    if ($.type(data) == "string") {
                        data = helper.StrToJson(data);
                    }
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {
                            toast.success("保存成功");
                            setTimeout(function () {
                                window.location.href = "/";
                            }, 2000);
                        }
                        else {
                            tipdialog.alertMsg("保存失败");
                        }
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);
            };
            //子表-初始化分页信息
            var tabPageInfo = tabPage();
            //子表-初始化校验信息
            var tabButtonInfo = tabButton();
            //子表-分页
            function tabPage() {
                var tabPageInfo = {};
                tabPageInfo.bindPageClass = function () {
                    var currentPageInfo = tabPageInfo.PageInfo;
                    if (currentPageInfo.HasNext) {
                        $('#nextTabBtn').removeClass('disabled');
                        $('#nextTabBtn').removeClass('c-gray-btn');
                        $('#nextTabBtn').removeAttr('disabled');
                        $('#nextTabBtn').addClass('c-green');
                        $('#nextTabBtn').children(':first').removeClass('m-icon-gray');
                        $('#nextTabBtn').children(':first').addClass('m-icon-white');
                    } else {
                        $('#nextTabBtn').addClass('disabled');
                        $('#nextTabBtn').addClass('c-gray-btn');
                        $('#nextTabBtn').attr("disabled", "disabled");
                        $('#nextTabBtn').removeClass('c-green');
                        $('#nextTabBtn').children(':first').addClass('m-icon-gray');
                        $('#nextTabBtn').children(':first').removeClass('m-icon-white');
                    }
                    if (currentPageInfo.HasPrev) {
                        $('#prevTabBtn').removeClass('disabled');
                        $('#prevTabBtn').removeClass('c-gray-btn');
                        $('#prevTabBtn').removeAttr('disabled');
                        $('#prevTabBtn').addClass('c-green');
                        $('#prevTabBtn').children(':first').removeClass('m-icon-gray');
                        $('#prevTabBtn').children(':first').addClass('m-icon-white');
                    } else {
                        $('#prevTabBtn').addClass('disabled');
                        $('#prevTabBtn').addClass('c-gray-btn');
                        $('#prevTabBtn').attr("disabled", "disabled");
                        $('#prevTabBtn').removeClass('c-green');
                        $('#prevTabBtn').children(':first').addClass('m-icon-gray');
                        $('#prevTabBtn').children(':first').removeClass('m-icon-white');
                    }
                };
                //分页信息
                tabPageInfo.PageInfo = {
                    IDS: [],
                    Index: 0,
                    PageSize: 0,
                    HasPrev: false,
                    HasNext: false
                };
                //初始化子页面记录数据
                tabPageInfo.initPageInfo = function (ids) {
                    tabPageInfo.PageInfo.IDS = ids;
                    tabPageInfo.PageInfo.Index = 0;
                    tabPageInfo.PageInfo.PageSize = ids.length;
                    tabPageInfo.PageInfo.HasNext = ids.length > 1;
                    tabPageInfo.PageInfo.HasPrev = false;
                };
                //计算分页信息
                tabPageInfo.calculatePage = function (tag) {
                    if (tag == undefined)
                        return tabPageInfo.PageInfo;
                    //标识
                    if (tag > 0) {
                        tabPageInfo.PageInfo.Index++;
                    }
                    else {
                        tabPageInfo.PageInfo.Index--;
                    }
                    tabPageInfo.PageInfo.HasNext = tabPageInfo.PageInfo.PageSize > (tabPageInfo.PageInfo.Index + 1);
                    tabPageInfo.PageInfo.HasPrev = tabPageInfo.PageInfo.Index > 0;
                    return tabPageInfo.PageInfo;
                };
                tabPageInfo.next = function () {
                    tabPageInfo.calculatePage(1);
                    tabPageInfo.bindPageClass();
                };
                tabPageInfo.prev = function () {
                    tabPageInfo.calculatePage(-1);
                    tabPageInfo.bindPageClass();
                };
                return tabPageInfo;
            }
            //子表-必填项校验
            function tabButton() {
                var button = {};
                var defaults = {};
                ///初始化
                button.initial = function (elemt, options) {
                    var param;
                    var message = '';
                    var additionMsg = '';
                    var focusId = undefined;
                    param = $.extend({}, defaults, options);
                    return button.checkItem(elemt, message, additionMsg, focusId, param);
                }
                ///提交驗證
                button.checkItem = function (elemt, message, additionMsg, focusId, param) {
                    $(elemt).find('input[name],select[name],textarea[name]').each(function (i, item) {
                        if (!Check(item)) {
                            if (item.nodeName == 'SELECT') {
                                if ($(item).val() == '') {
                                    message += getControlLabel(item).replace('*', '') + getControlDescription(item) + '<br />';
                                    $(item).parent('div').addClass('data-box-red');
                                } else {
                                    $(item).parent('div').removeClass('data-box-red');
                                }
                            } else {
                                if (item.type == 'radio' || item.type == 'checkbox') {
                                    var temp = getControlLabelByName(item.name).replace('*', '');
                                    if (message.indexOf(temp) == -1) {
                                        var ra = $(':' + item.type + ':checked');
                                        var len = ra.length;
                                        if (len == 0) {
                                            message += temp + getControlDescription(item) + '<br />';
                                            $(item).parent('div').addClass('data-box-red');
                                            $(item).parent('div').css({
                                                'float': 'left',
                                                'padding': '3px 12px 3px 8px'
                                            });
                                        } else {
                                            $(item).parent('div').removeClass('data-box-red');
                                            $(item).parent('div').css('float', '');
                                        }
                                    }
                                } else {
                                    var temp = getControlLabel(item).replace('*', '');
                                    if ($(item).val() != '' && $(item).attr('data-cwvalid') != undefined) {
                                        additionMsg += "请输入正确的" + temp + '<br />';
                                    } else {
                                        $(item).addClass('data-box-red');
                                        message += temp + getControlDescription(item) + '<br />';
                                    }
                                }
                            }
                            if (focusId == undefined) {
                                focusId = $(item).attr('id');
                            }
                        }
                    });
                    message += additionMsg;
                    if (message != '') {
                        if (focusId != undefined) {
                            tipdialog.errorDialog(message);
                        }
                        $('#TipModal').on('hidden.bs.modal', function () {
                            $('#' + focusId).focus();
                        });
                        return false;
                    } else {
                        return true;
                    }
                }

                ///檢查必填項
                function Check(item) {
                    var vaild = $(item).data("cwvalid");
                    if (vaild != null) {
                        var inputValue = $.trim($(item).val());
                        if (inputValue.length == 0) {
                            if (vaild.required == true) {
                                return false;
                            }
                        }
                        else {
                            if (vaild.regx != undefined) {
                                var regx = new RegExp(vaild.regx);
                                if (!regx.test(inputValue)) {
                                    return false;
                                }
                            }
                            if (vaild.minlength != undefined && inputValue.length < vaild.minlength) {
                                return false;
                            }
                            if (vaild.maxlength != undefined && inputValue.length > vaild.maxlength) {
                                return false;
                            }
                        }
                        return true;
                    }
                    return true;
                };

                function getControlLabel(control) {
                    return $('label[for=' + control.id + ']').text();
                }

                function getControlLabelByName(name) {
                    return $('label[for=' + name + ']').text();
                }

                function getControlDescription(control) {
                    var description = '是必填项';
                    if (!(control instanceof jQuery)) {
                        control = $(control);
                    }
                    var elementType = control.prop('nodeName');
                    switch (elementType) {
                        case 'INPUT':
                            inputType = $(control).attr('type');
                            description = getInputDescription(inputType);
                            break;
                        case 'SELECT':
                            description = '是必选项';
                            break;
                        case 'TEXTAREA':
                            description = '是必填项';
                            break;
                        default:
                            break;
                    }
                    return description;
                }

                function getInputDescription(inputType) {
                    var description = '';
                    switch (inputType) {
                        case 'checkbox':
                            description = '是必选项';
                            break;
                        case 'radio':
                            description = '是必选项';
                            break;
                        case 'hidden':
                            description = '是必填项';
                            break;
                        case 'number':
                            description = '是必填项';
                            break;
                        case 'password':
                            description = '是必填项';
                            break;
                        case 'text':
                            description = '是必填项';
                            break;
                        default:
                            break;
                    }
                    return description;
                }

                function getButtonByParam(option) {
                    var html = '<label class="control-label col-xs-4 col-sm-2"> &nbsp;' + option.labelName + '</label>' + '<div class="col-xs-8 col-sm-8"> ';
                    for (var i = 0; i < option.buttonObject.length; i++) {
                        html += '<button id="' + option.buttonObject[i].buttonId + '" class="btn ' + option.buttonObject[i].triggerBtn + '" style="line-height: 0; width:' + option.width + '; height: ' + option.height + '; color:' + option.fontColor + '; font-size: ' + option.fontSize + '; background-color: ' + option.btncolor.one + '"> <i class="fa fa-save"></i>' + option.buttonObject[i].btnValue + '</button>';
                    }
                    html += '</div>';
                    $('.button-init').append(html);
                }
                return button;
            }

            initPage();

        });
});