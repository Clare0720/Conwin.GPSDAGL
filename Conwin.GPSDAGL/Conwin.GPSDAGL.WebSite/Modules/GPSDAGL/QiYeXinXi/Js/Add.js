define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'selectCity2', 'filelist', 'metronic', 'customtable', 'bootstrap3-typeahead', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, selectCity2, filelist, Metronic, fileupload) {
            var userInfo = helper.GetUserInfo();

            var jiGouData = [];
            var initPage = function () {
                var tabFlag = false;
                common.AutoFormScrollHeight('#Form1');
                $('.date-picker').datepicker({ format: 'yyyy-mm-dd', autoclose: true, language: 'zh-CN' });
                formcontrol.initial();
                // 初始化智能搜索
                //InitTypeAheadData();

                initData();
                //保存
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
                    if ($.trim(fromData.SuoShuJianKongPingTai) == '') {
                        msg += "所属监控平台是必填项<br/>";
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
                    //if ($.trim(fromData.ZhuangTai) == '') {
                    //    msg += "营运状态是必选项<br/>";
                    //}
                    if ($.trim(fromData.QiYeXingZhi) == '') {
                        msg += "企业性质是必选项<br/>";
                    }
                    if ($.trim(fromData.LianXiRen) == '') {
                        msg += "联系人是必填项<br/>";
                    }
                    if ($.trim(fromData.LianXiFangShi) == '') {
                        msg += "联系电话是必填项<br/>";
                    }
                    //else {
                    //    if (check_tellnum($.trim(fromData.LianXiFangShi)) == false) {
                    //        msg += '联系电话格式有误<br/>';
                    //    }
                    //}

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
                //tab2
                $('#tab2').click(function (e) {
                    if ($('#tab3').parent('li').hasClass('active')) {
                        e.preventDefault();
                    } else {
                        if (!tabFlag) {
                            tipdialog.alertMsg('请先保存基础信息!', function () {
                                $('#tab2').parent('li').removeClass('active');
                                $('#tab1').parent('li').addClass('active');
                                $('#LianXiXinXi').removeClass('active in');
                                $('#JiChuXinXi').addClass('active in');
                            });
                            if ($('.bootbox-body').html() == '请先保存基础信息!') {
                                $('.bootbox-close-button').click(function () {
                                    $('#tab2').parent('li').removeClass('active');
                                    $('#tab1').parent('li').addClass('active');
                                    $('#LianXiXinXi').removeClass('active in');
                                    $('#JiChuXinXi').addClass('active in');
                                });
                            }
                        } else {
                            $('#LianXiXinXi').addClass('active in');
                            $('#JiChuXinXi').removeClass('active in');
                        }
                    }
                });
                //region
                initArea();
                selectCity2.initSelectView($('#JingYingFanWei'));


                if (userInfo.OrganizationType == "0") {
                    $('#add').click(function (e) {
                        e.preventDefault();
                        selectCity2.showSelectCity();

                    });
                }
                else {
                    var orgManageArea = "广东" + userInfo.OrganCity;
                    if (typeof orgManageArea != "undefined" || orgManageArea != '') {
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
                    selectCity2.setData("广东" + userInfo.OrganCity);
                    $('#add').attr("disabled", true); 
                    $(".closeSelected").remove()
                }
                //endregion
            };


            //初始化表单数据
            function initData() {
                $('#Id').val(helper.NewGuid());
            };


            //保存
            function save() {
                //TODO: 校验数据
                var jsonData1 = $('#Form1').serializeObject();
                //jsonData1.ZhuangTai = $('#ZhuangTai').val();
                for (var key in jsonData1) {
                    jsonData1[key] = jsonData1[key].replace(/\s/g, "");
                }
                jsonData1.XiaQuSheng = $('#XiaQuSheng').val();
                jsonData1.XiaQuShi = $('#XiaQuShi').val();
                jsonData1.XiaQuXian = $('#XiaQuXian').val();
                jsonData1.JingYingFanWei = $('#JingYingFanWei').text().replace("选择辖区","");
                jsonData1.JingYingXuKeZhengChangQiYouXiao = $('#Form1').find('[name="JingYingXuKeZhengChangQiYouXiao"]').parent().attr("class") == "checked";
                jsonData1.GongShangYingYeZhiZhaoChangQiYouXiao = $('#Form1').find('[name="GongShangYingYeZhiZhaoChangQiYouXiao"]').parent().attr("class") == "checked";

                if ($('#ShiFouGeTiHu').val() == "1") {
                    jsonData1.OrgType = 8;
                } else {
                    jsonData1.OrgType = 2;
                }
                //调用新增接口
                helper.Ajax("006600200015", jsonData1, function (data) {
                    if ($.type(data) == "string") {
                        data = helper.StrToJson(data);
                    }
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {
                            toast.success("保存成功");
                            window.parent.document.getElementById('hdIDS').value = jsonData1.Id;
                            setTimeout(function () { window.location.href = "Edit.html"; }, 2000);
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
            //初始化辖区
            var initArea = function () {
                var defaultOption = '<option value="" selected="selected">请选择</option>';
               
               
                //不是管理员锁死市级选项
                selectcity.setXiaQu('00000020005', { "Province": "广东" }, '#XiaQuShi', function () {
                    if (userInfo.OrganizationType != 0) {
                        $("#XiaQuShi").val(XiaQuShi);
                        var XiaQuShi = userInfo.OrganCity;
                        if (XiaQuShi != "") {
                            var list = XiaQuShi;
                            $("#XiaQuShi").find("option").each(function (index, item) {
                                if (list.indexOf($(item).val()) < 0 && $(item).val() != "") {
                                    $(item).remove();
                                }
                            });
                            $("#XiaQuShi").val(XiaQuShi);
                        }
                    }
                }, 'GetCityList', 'CityListName');
                
                
                //市级下拉改变时修改县级下拉内容
                $('#XiaQuShi').change(function () {
                    $('#XiaQuXian').empty().append(defaultOption);
                    var data = { "City": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化区县下拉框
                        selectcity.setXiaQu('00000020006', data, '#XiaQuXian', function () {
                            if (userInfo.OrganizationType != 0) {
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
                        }, 'GetDistrictList', 'DistrictName');
                    }
                });
                //县政府和企业同时锁死县级选项
                if (userInfo.OrganizationType != 0) {
                    selectcity.setXiaQu('00000020006', { "City": userInfo.OrganCity }, '#XiaQuXian', function () {
                        if (userInfo.OrganizationType != 11) {
                            var XiaQuXian = userInfo.OrganDistrict;
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
                    }, 'key', 'key', userInfo.OrganCity);
                }
                if (userInfo.OrganizationType != 0) {
                   document.getElementById("XiaQuShi").disabled = "disabled";
                    if (userInfo.OrganizationType != 11) {
                        document.getElementById("XiaQuXian").disabled = "disabled";;
                    }
                }
                $('#XiaQuSheng').val('广东');
                document.getElementById("XiaQuSheng").disabled = "disabled";
            }

            /* 机构名称智能提示Start */

            //初始化搜索框
            function InitAutoCompleteSearch() {
                $('#OrgName').typeahead({
                    source: function (query, process) {
                        var arr = getTypeAheadData(jiGouData);
                        var resultList = arr.map(function (item) {
                            return JSON.stringify(item);
                        });
                        return resultList;
                    },
                    matcher: function (item) {
                        item = JSON.parse(item);
                        return fuzzyMatch(this.query, item.text.split("|")[0]);
                    },
                    sorter: function (items) {
                        var beginswith = [],
                            caseSensitive = [],
                            caseInsensitive = [],
                            item;

                        while ((item = items.shift())) {
                            item = JSON.parse(item);
                            if (fuzzyMatch(this.query, item.text.split("|")[0]))
                                beginswith.push(item.text);
                            else if (~item.text.split("|")[0].indexOf(this.query)) caseSensitive.push(item.text);
                            else caseInsensitive.push(item.text);
                        }

                        return beginswith.concat(caseSensitive, caseInsensitive);
                    },
                    displayText: function (item) {
                        return item.split("|")[0];
                    },
                    updater: function (item) {
                        getQiYeXinXi(item.split("|")[1], function (serviceData) {
                            if (serviceData.publicresponse.statuscode == 0) {
                                fillFormData(serviceData.body);
                            } else {
                                tipdialog.errorDialog("请求数据失败");
                            }
                        });
                        $("#OrgName").on("change", function (e) {
                            e.preventDefault();
                            if ($("#OrgName").val() != item.split("|")[0]) {
                                clearFormData();
                            }
                        })
                        return item.split("|")[0];
                    }
                    //delay: 500
                });
            }

            //匹配中文
            function fuzzyMatch(query, text) {
                var p = query.replace(/(.)(?=[^$])/g, "$1,").split(",").join('.*');
                var pattern = new RegExp(p, "gi");
                return pattern.test(text);
            }

            //初始化列表数据
            function InitTypeAheadData() {
                var param = {
                    Page: 1,
                    Rows: 10000,
                    data: {}
                }
                helper.Ajax("006600200016", param, function (result) {
                    if (result.publicresponse.statuscode == 0) {
                        jiGouData = result.body;
                        InitAutoCompleteSearch();
                    } else {
                        tipdialog.errorDialog('获取组织机构信息失败，错误信息：' + result.publicresponse.message);
                    }
                }, false);
            }

            //获取搜索列表数据
            function getTypeAheadData(obj) {
                var dataArr = [];
                $.each(obj.items, function (index, item) {
                    dataArr.push({
                        text: $.trim(item.OrgName) + "|" + item.Id
                    });
                });
                return dataArr;
            }

            function getQiYeXinXi(id, callback) {
                helper.Ajax("006600200017", id, function (resultdata) {
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
                        if ($(item).hasClass('datepicker')) {
                            tempValue = tempValue.substr(0, 10);
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

                selectCity2.setData(resource['JingYingFanWei']);

                selectcity.setXiaQu('00000020004', {}, '#KongGuGongSiSuoZaiXiaQuSheng', 'GetProvinceList', 'Key', 'Key', resource.KongGuGongSiSuoZaiXiaQuSheng);
                selectcity.setXiaQu('00000020005', { "Province": resource.KongGuGongSiSuoZaiXiaQuSheng }, '#KongGuGongSiSuoZaiXiaQuShi', 'GetCityList', 'Key', 'Key', resource.KongGuGongSiSuoZaiXiaQuShi);
                //selectcity.setXiaQu('00000020004', {}, '#XiaQuSheng', '', 'Key', 'Key', resource.XiaQuSheng);
                selectcity.setXiaQu('00000020005', { "Province": "广东" }, '#XiaQuShi', function () {
                    if (userInfo.OrganizationType != 0) {
                        var XiaQuShi = userInfo.OrganCity;
                        if (XiaQuShi) {
                            XiaQuShi = XiaQuShi.replace(/广东/g, "");
                            var list = XiaQuShi.split("|");
                            //市政府限定选择范围
                            if (UserInfo.OrganizationType == 11) {
                                list = UserInfo.OrganCity;
                            }
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
                $('#XiaQuShi').change(function () {
                    $('#XiaQuXian').empty().append(defaultOption);
                    var data = { "City": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化区县下拉框
                        selectcity.setXiaQu('00000020006', data, function () {
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

                        }, '#XiaQuXian', 'GetDistrictList', 'DistrictName');
                    }
                });

                $('#XiaQuXian').change(function () {
                    var data = { "District": $(this).val() };
                });
            }
            ;

            //清空数据
            function clearFormData() {
                $('#Form1').find('input[name!="OrgName"][name!="ZhuangTai"],textarea[name]').each(function (i, item) {
                    $(item).val('');
                });
                $('#Form1').find('select[name]').each(function (i, item) {
                    $(item).find('option:first').prop("selected", 'selected');
                });
                $('.closeSelected').each(function (i, item) {
                    $(item).trigger("click");
                });
                $("#YXZT").val("正常营业");
                initData();
            }

            /* 机构名称智能提示Start */

            //endregion
            initPage();
        });


});
