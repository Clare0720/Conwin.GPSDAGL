define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'selectCity2', 'metronic', 'fileupload', 'dropdown', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, selectCity2, Metronic, fileupload, dropdown) {
            var userInfo = helper.GetUserInfo();
            var positionTypeArr = [];

            var initPage = function () {
                common.AutoFormScrollHeight('#Form1');
                formcontrol.initial();
                dropdown.initial();
                viewPageButtonInit();
                initCurrentQiYe();
                loadData();
            };

            function viewPageButtonInit() {
                $('#btnclose').click(function () {
                    tipdialog.confirm("确定关闭？", function (r) {
                        if (r) {
                            parent.window.$("#btnSearch").click();
                            popdialog.closeIframe();
                        }
                    });
                });

                $("#backBtn").on('click', function (e) {
                    e.preventDefault();
                    parent.window.$("#btnSearch").click();
                    popdialog.closeIframe();
                });
            }

            function initCurrentQiYe() {
                $('#CompanyName').val(userInfo.OrganizationName);
            }
            function loadData() {
                var id = window.parent.document.getElementById('hdIDS').value;
                helper.Ajax("006600200021", { Id: id }, function (data) {
                    if (data.body) {
                        var model = data.body[0];
                        fillFormData(data.body[0], "Form1");
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message, function () {
                            popdialog.closeIframe();
                        });

                    }
                }, false);
            }
            function fillFormData(resource, Id) {

                $('#' + Id).find('input[name],select[name],textarea[name]').each(function (i, item) {
                    var tempValue = resource[$(item).attr('name')];

                    if (tempValue != undefined) {
                        //if (!!tempValue) {
                        if ($(item).hasClass('datetimepicker')) {
                            tempValue = tempValue.substr(11, 5);
                        }
                        if ($(item).hasClass('datepicker')) {
                            tempValue = tempValue.substr(0, 10);
                        }
                        //TODO: 赋值
                        $(item).val(tempValue.toString() == '' ? '' : tempValue.toString());

                        if ($(item).attr('name') == "Sex") {
                            var stringValue = "";
                            switch (tempValue) {
                                case 0: stringValue = "男"; break;
                                case 1: stringValue = "女"; break;
                                default:
                            }
                            $("#Sex").val(stringValue);
                        }

                        if ($(item).attr('name') == "Nation") {
                            var stringValue = "";
                            switch (tempValue) {
                                case 1: stringValue = "汉族"; break;
                                case 2: stringValue = "蒙古族"; break;
                                case 3: stringValue = "回族"; break;
                                case 4: stringValue = "藏族"; break;
                                case 5: stringValue = "维吾尔族"; break;
                                case 6: stringValue = "苗族"; break;
                                case 7: stringValue = "彝族"; break;
                                case 8: stringValue = "壮族"; break;
                                case 9: stringValue = "布依族"; break;
                                case 10: stringValue = "朝鲜族"; break;
                                case 11: stringValue = "满族"; break;
                                case 12: stringValue = "侗族"; break;
                                case 13: stringValue = "瑶族"; break;
                                case 14: stringValue = "白族"; break;
                                case 15: stringValue = "土家族"; break;
                                case 16: stringValue = "哈尼族"; break;
                                case 17: stringValue = "哈萨克族"; break;
                                case 18: stringValue = "傣族"; break;
                                case 19: stringValue = "黎族"; break;
                                case 20: stringValue = "傈僳族"; break;
                                case 21: stringValue = "佤族"; break;
                                case 22: stringValue = "畲族"; break;
                                case 23: stringValue = "高山族"; break;
                                case 24: stringValue = "拉祜族"; break;
                                case 25: stringValue = "水族"; break;
                                case 26: stringValue = "东乡族"; break;
                                case 27: stringValue = "纳西族"; break;
                                case 28: stringValue = "景颇族"; break;
                                case 29: stringValue = "柯尔克孜族"; break;
                                case 30: stringValue = "土族"; break;
                                case 31: stringValue = "达斡尔族"; break;
                                case 32: stringValue = "仫佬族"; break;
                                case 33: stringValue = "羌族"; break;
                                case 34: stringValue = "布朗族"; break;
                                case 35: stringValue = "撒拉族"; break;
                                case 36: stringValue = "毛南族"; break;
                                case 37: stringValue = "仡佬族"; break;
                                case 38: stringValue = "锡伯族"; break;
                                case 39: stringValue = "阿昌族"; break;
                                case 40: stringValue = "普米族"; break;
                                case 41: stringValue = "塔吉克族"; break;
                                case 42: stringValue = "怒族"; break;
                                case 43: stringValue = "乌孜别克族"; break;
                                case 44: stringValue = "俄罗斯族"; break;
                                case 45: stringValue = "鄂温克族"; break;
                                case 46: stringValue = "德昂族"; break;
                                case 47: stringValue = "保安族"; break;
                                case 48: stringValue = "裕固族"; break;
                                case 49: stringValue = "京族"; break;
                                case 50: stringValue = "塔塔尔族"; break;
                                case 51: stringValue = "独龙族"; break;
                                case 52: stringValue = "鄂伦春族"; break;
                                case 53: stringValue = "赫哲族"; break;
                                case 54: stringValue = "门巴族"; break;
                                case 55: stringValue = "珞巴族"; break;
                                case 56: stringValue = "基诺族"; break;
                                default:
                            }
                            $("#Nation").val(stringValue);
                        }

                        if ($(item).attr('name') == "Education") {
                            var stringValue = "";
                            switch (tempValue) {
                                case 1: stringValue = "高中，中专及以下"; break;
                                case 2: stringValue = "专科"; break;
                                case 3: stringValue = "本科"; break;
                                case 4: stringValue = "硕士研究生"; break;
                                case 5: stringValue = "博士研究生"; break;
                                default:
                            }
                            $("#Education").val(stringValue);
                        }

                        if ($(item).attr('name') == "Degree") {
                            var stringValue = "";
                            switch (tempValue) {
                                case "0": stringValue = "无学位"; break;
                                case "1": stringValue = "学士"; break;
                                case "2": stringValue = "硕士"; break;
                                case "3": stringValue = "博士"; break;
                                default:
                            }
                            $("#Degree").val(stringValue);
                        }

                        if ($(item).attr('name') == "LiuDongDangYuan") {
                            var stringValue = "";
                            switch (tempValue) {
                                case "1": stringValue = "是"; break;
                                case "2": stringValue = "否"; break;
                                default:
                            }
                            $("#LiuDongDangYuan").val(stringValue);
                        }

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

            function getPersonalPosition(positions) {
                var param = {};
                helper.Ajax("003300300332", param, function (data) {
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {
                            positionTypeArr = data.body;
                            // 新版本职务
                            var positionMap = changeMap(positionTypeArr);
                            if (positions && positions.length > 0) {
                                var positionSelectVal = "";
                                $.each(positions, function (i, item) {
                                    positionSelectVal += `${positionMap.get(item)}，`;
                                });
                                $('#Position').val(positionSelectVal.slice(0,positionSelectVal.lastIndexOf("，")));
                            }
                        }
                    } else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);
            }

            function changeMap(d) {
                var positionChangeArr = [];
                $.each(d, function (index, item) {
                    var positionEntries = Object.entries(item);
                    positionChangeArr.push([positionEntries[1][1],positionEntries[0][1]]);
                })
                return new Map(positionChangeArr);
            }

            initPage();
        });


});
