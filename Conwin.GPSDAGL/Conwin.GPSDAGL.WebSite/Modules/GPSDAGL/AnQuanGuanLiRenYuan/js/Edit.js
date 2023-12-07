define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'selectCity2', 'metronic', 'fileupload', 'dropdown', 'bootbox', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, selectCity2, Metronic, fileupload, dropdown, bootbox) {
            var userInfo = helper.GetUserInfo();
            var positionTypeArr = [];

            var initPage = function () {
                common.AutoFormScrollHeight('#Form1');
                //getPersonalPosition();
                formcontrol.initial();
                dropdown.initial();
                editPageButtonInit();
                loadData();
            };

            function editPageButtonInit() {
                $('#btnclose').click(function () {
                    tipdialog.confirm("确定关闭？", function (r) {
                        if (r) {
                            parent.window.$("#btnSearch").click();
                            popdialog.closeIframe();
                        }
                    });
                });

                $('#KaoHeRiQi').datepicker({
                    language: 'zh-CN',
                    format: 'yyyy-mm-dd',
                    autoclose: true,
                    startView: 'month',
                    minView: 2,
                    todayBtn: 'linked'
                });

                $('#EntryDate').removeAttr("style");

                $('#EntryDate').attr("style", "cursor:pointer");


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
                $("#saveBtn").on('click', function (e) {
                    e.preventDefault();
                    var flags = true;
                    var msg = '';
                    var positionArr = [];
                    var fromData = $('#Form1').serializeObject();

                    if ($.trim(fromData.Name) == '') {
                        msg += "姓名是必填项<br/>";
                    }
                    if ($.trim(fromData.QuYu) == '') {
                        msg += "所在区域是必填项<br/>";
                    }
                    // 校验数据
                    if (!fromData.IDCardType || fromData.IDCardType == "") {
                        msg += "证件类型是必选项</br>"
                    }
                    if (fromData.IDCard.trim() == "") {
                        msg += "证件号码是必填项</br>"
                    }
                    if ($.trim(fromData.ZhuanYeMingCheng) == '') {
                        msg += "专业名称是必填项<br/>";
                    }
                    if ($.trim(fromData.KeMuMingCheng) == '') {
                        msg += "科目名称是必填项<br/>";
                    }
                    if (fromData.ZhiWu.trim() == "") {
                        msg += "职务是必填项</br>"
                    }
                    if (fromData.LianXiDianHua.trim() == "") {
                        msg += "联系电话为是必填项</br>"
                    }
                    //else {
                    //    if (check_tellnum($.trim(fromData.LianXiDianHua)) == false) {
                    //        msg += '联系电话格式有误<br/>';
                    //    }
                    //}
                    if (!fromData.KaoHeZhuangTai || fromData.KaoHeZhuangTai == "") {
                        msg += "考核状态是必选项<br/>";
                    }
                    else if (fromData.IDCardType == "0" && ValidIdentityCardNumber(fromData.IDCard) == false) {
                        msg += '证件号码格式不正确</br>';
                    }
                    if (!fromData.WorkingStatus || fromData.WorkingStatus == "") {
                        msg += "在职状态是必选项<br/>";
                    }
                    if ($.trim(fromData.KaoHeRiQi) == '') {
                        msg += "考核日期是必填项<br/>";
                    }
                    if (fromData.KaoHeZhuangTai == "0") {
                        if (new Date(fromData.KaoHeRiQi).getTime() < new Date(getCurrentDate() + " 00:00").getTime()) {
                            msg += "考核状态为待考核时，考核日期不能小于当前日期<br/>";
                        }
                    }
                    else {
                        if (new Date(fromData.KaoHeRiQi).getTime() > new Date(getCurrentDate() + " 23:59").getTime()) {
                            msg += "考核状态为已通过时，考核日期不能大于当前日期<br/>";
                        }
                    }
                    if (msg != '') {
                        flags = false;
                        tipdialog.alertMsg(msg);
                    }
                    //新版本职务
                    $('#PositionsCheckBox').find('[type="checkbox"]').each(function (i, item) {
                        if ($(item).parent().attr("class") == "checked") {
                            var positionCode = $(item).attr("val");
                            positionArr.push(positionCode);
                        }
                    });
                    fromData.Position = positionArr.join(',');
                    fromData.id = window.parent.document.getElementById('hdIDS').value;
                    if (flags) {
                        helper.Ajax("006600200027", fromData, function (data) {
                            if (data.body) {
                                layer.closeAll();
                                toast.success("保存成功");
                                setTimeout(function () {
                                    parent.$("#btnSearch").click();
                                    popdialog.closeIframe();
                                }, 1000);

                            }
                            else {
                                layer.closeAll();
                                tipdialog.alertMsg(data.publicresponse.message);
                            }
                        }, false);
                    }

                });
            }
            //手机号码校验
            function check_tellnum(content) {
                // 正则验证格式
                eval("var reg = /^1[34578]\\d{9}$/;");
                return RegExp(reg).test(content);
            }
            function loadData() {
                var id = window.parent.document.getElementById('hdIDS').value;
                helper.Ajax("006600200026", { Id: id }, function (data) {
                    if (data.body) {
                        var model = data.body[0];
                        $("#Name").val(model.Name);
                        document.getElementById('IDCardType').value = model.IDCardType;
                        $("#IDCard").val(model.IDCard);
                        $("#QuYu").val(model.QuYu);
                        $("#ZhuanYeMingCheng").val(model.ZhuanYeMingCheng);
                        if (model.KaoHeRiQi && model.KaoHeRiQi != "") {
                            $("#KaoHeRiQi").val(model.KaoHeRiQi.substring(0, 10));
                        }
                        $("#KeMuMingCheng").val(model.KeMuMingCheng);
                        $("#LianXiDianHua").val(model.LianXiDianHua);
                        $("#ZhiWu").val(model.ZhiWu);
                        $("#DangZuZhiSuoZaiDi").val(model.DangZuZhiSuoZaiDi);
                        document.getElementById('KaoHeZhuangTai').value = model.KaoHeZhuangTai;
                        document.getElementById('WorkingStatus').value = model.WorkingStatus;
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                        popdialog.closeModal();
                    }
                }, false);
            }


            /* 身份证验证 Start*/
            function ValidIdentityCardNumber(idCard) {

                //idCard = $.trim(idCard.replace(/ /g, ""));              
                idCard = $.trim(idCard);
                if (idCard.length == 15) {
                    /*return isValidityBrithBy15IdCard(idCard) == true ? '' : '身份证格式不正确<br />';*/       //进行15位身份证的验证   
                    if (isValidityBrithBy15IdCard(idCard)) {
                        return true;
                    } else {
                        //tipdialog.alertMsg("身份证号码格式不正确");
                        return false;
                    }
                } else if (idCard.length == 18) {
                    var a_idCard = idCard.split("");                // 得到身份证数组   
                    if (isValidityBrithBy18IdCard(idCard) && isTrueValidateCodeBy18IdCard(a_idCard)) {   //进行18位身份证的基本验证和第18位的验证
                        return true;
                    } else {
                        //tipdialog.alertMsg("身份证号码格式不正确");
                        return false;
                    }
                } else {
                    //tipdialog.alertMsg("身份证号码格式不正确");
                    return false;
                }
            }

            function isValidityBrithBy18IdCard(idCard18) {
                var year = idCard18.substring(6, 10);
                var month = idCard18.substring(10, 12);
                var day = idCard18.substring(12, 14);
                var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
                // 这里用getFullYear()获取年份，避免千年虫问题   
                if (temp_date.getFullYear() != parseFloat(year)
                    || temp_date.getMonth() != parseFloat(month) - 1
                    || temp_date.getDate() != parseFloat(day)) {
                    return false;
                } else {
                    return true;
                }
            }

            function isValidityBrithBy15IdCard(idCard15) {
                var year = idCard15.substring(6, 8);
                var month = idCard15.substring(8, 10);
                var day = idCard15.substring(10, 12);
                var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
                // 对于老身份证中的你年龄则不需考虑千年虫问题而使用getYear()方法   
                if (temp_date.getYear() != parseFloat(year)
                    || temp_date.getMonth() != parseFloat(month) - 1
                    || temp_date.getDate() != parseFloat(day)) {
                    return false;
                } else {
                    return true;
                }
            }

            function isTrueValidateCodeBy18IdCard(a_idCard) {
                var Wi = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1];    // 加权因子   
                var ValideCode = [1, 0, 10, 9, 8, 7, 6, 5, 4, 3, 2];            // 身份证验证位值.10代表X   
                var sum = 0;                             // 声明加权求和变量   
                if (a_idCard[17].toLowerCase() == 'x') {
                    a_idCard[17] = 10;                    // 将最后位为x的验证码替换为10方便后续操作   
                }
                for (var i = 0; i < 17; i++) {
                    sum += Wi[i] * a_idCard[i];            // 加权求和   
                }
                valCodePosition = sum % 11;                // 得到验证码所位置   
                if (a_idCard[17] == ValideCode[valCodePosition]) {
                    return true;
                } else {
                    return false;
                }
            }
            /* 身份证验证 End*/



            function initfilelist(ldata) {
                var ldata1 = [];
                for (var key in ldata) {
                    if ((typeof ldata[key]) != "function") {
                        ldata1.push(ldata[key]);
                    }
                }
                return ldata1;
            }



            //是否确认
            function isConfirmEx(msgStr, callbackFn, callbackNo) {
                return bootbox.dialog({
                    message: msgStr,
                    title: "提示",
                    buttons: {
                        main: {
                            label: "确认",
                            className: "blue",
                            callback: callbackFn
                        },
                        Cancel: {
                            label: "取消",
                            className: "default",
                            callback: callbackNo
                        }
                    }
                });
            };

            //检查职务是否选择
            //function checkPosition() {
            //    var num = 0;
            //    $('#PositionsCheckBox').find('[type="checkbox"]').each(function (i, item) {
            //        if ($(item).attr("checked")) {
            //            num++;
            //        }
            //    });
            //    if (num == 0) {
            //        return false;
            //    }
            //    return true;
            //}

            //function getPersonalPosition() {
            //    helper.Ajax("003300300332", {}, function (data) {
            //        if (data.publicresponse.statuscode == 0) {
            //            if (data.body) {
            //                positionTypeArr = data.body;
            //                InitPositionsCheckBox();
            //            }
            //        } else {
            //            tipdialog.alertMsg(data.publicresponse.message);
            //        }
            //    }, false);
            //}
            //function InitPositionsCheckBox() {

            //    // 初始化职务
            //    $.each(positionTypeArr, function (index, item) {
            //        var checkBoxStr = '';
            //        checkBoxStr += `<div style="width:50%;float:left;margin-top:2px;">
            //                        <input style="width:15px;height:15px;opacity:0.6;" type="checkbox" name="Positions" val="${item.PositionCode}" />
            //                        <span style="vertical-align:top;">${item.PositionName}</span>
            //                        </div>
            //                        `
            //        $('#PositionsCheckBox').append(checkBoxStr);
            //    })
            //    $('#PositionsCheckBox').find('[type="checkbox"]').each(function (index, item) {
            //        $(item).on('click', function () {
            //            if ($(item).parent().attr("class") == "checked") {
            //                $(item).parent().removeAttr("class");
            //            } else {
            //                $(item).parent().attr("class", "checked");
            //            }

            //        })
            //    })
            //}

            function changeMap(d) {
                var positionChangeArr = [];
                $.each(d, function (index, item) {
                    var positionEntries = Object.entries(item);
                    positionChangeArr.push([positionEntries[1][1], positionEntries[0][1]]);
                })
                return new Map(positionChangeArr);
            }

            //获取当前日期
            function getNowFormatDate() {
                var date = new Date();
                var seperator1 = "-";
                var month = date.getMonth() + 1;
                var strDate = date.getDate();
                if (month >= 1 && month <= 9) {
                    month = "0" + month;
                }
                if (strDate >= 0 && strDate <= 9) {
                    strDate = "0" + strDate;
                }
                var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate;

                return currentdate;
            };

            initPage();
        });


});
