define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'metronic', 'selectCity2', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, Metronic, selectCity2, fileupload) {
            var initPage = function () {
                var tabFlag = false;
                common.AutoFormScrollHeight('#Form1');
                formcontrol.initial();
                initData();
                //getPersonalPosition();
                

                //保存
                $('#saveBtn').on('click', function (e) {
                    e.preventDefault();
                    var flags = true;
                    var msg = '';
                    var positionArr = [];
                    var fromData = $('#Form1').serializeObject();
                    //调用新增接口
                    if ($.trim(fromData.Name) == '') {
                        msg += "姓名是必填项<br/>";
                    }
                    if ($.trim(fromData.Sex) == '') {
                        msg += "性别是必选项<br/>";
                    }
                    // 校验数据
                    if (fromData.IDCard.trim() == "") {
                        msg += "证件号码是必填项</br>"
                    }
                    else if (ValidIdentityCardNumber(fromData.IDCard) == false) {
                        msg += '证件号码格式不正确</br>';
                    }
                    if ($.trim(fromData.Nation) == '') {
                        msg += "民族是必选项<br/>";
                    }
                    if ($.trim(fromData.NativePlace) == '') {
                        msg += "籍贯是必填项<br/>";
                    }
                    if ($.trim(fromData.Education) == '') {
                        msg += "学历是必选项<br/>";
                    }
                    if ($.trim(fromData.Position) == ''){
                        msg += "工作职务是必填项</br>"
                    }
                    if ($.trim(fromData.ContactNumber) == '') {
                        msg += "联系电话是必填项<br/>";
                    }
                    //else {
                    //    if (check_tellnum($.trim(fromData.ContactNumber)) == false) {
                    //        msg += '联系电话格式有误<br/>';
                    //    }
                    //}
                    if ($.trim(fromData.EntryDate) == '') {
                        msg += "入党时间是必填项<br/>";
                    }
                    if (msg != '') {
                        flags = false;
                        tipdialog.alertMsg(msg);
                    }
                    //新版本职务
                    //$('#PositionsCheckBox').find('[type="checkbox"]').each(function (i, item) {
                    //    if ($(item).parent().attr("class") == "checked") {
                    //        var positionCode = $(item).attr("val");
                    //        positionArr.push(positionCode);
                    //    }
                    //});
                    //$('#Position').val(positionArr.join(','));
                    
                    if (flags) {
                        save();
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
                //手机号码校验
                function check_tellnum(content) {
                    // 正则验证格式
                    eval("var reg = /^1[34578]\\d{9}$/;");
                    return RegExp(reg).test(content);
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

                $('#EntryDate').datepicker({
                    format: "yyyy-mm-dd",
                    language: "zh-CN",
                    endDate: getNowFormatDate(),//可选日期的结束日期
                    autoclose: true,
                    todayBtn: 'linked'
                });

                $('#EntryDate').removeAttr("style");

                $('#EntryDate').attr("style", "cursor:pointer");

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

                selectCity2.initSelectView($('.select'));
                $('#add').click(function (e) {
                    e.preventDefault();
                    selectCity2.showSelectCity();
                });

            };
            //初始化表单数据
            function initData() {
                
            };
            //检查职务是否选择
            function checkPosition() {
                var num = 0;
                $('#PositionsCheckBox').find('[type="checkbox"]').each(function (i, item) {
                    if ($(item).attr("checked")) {
                        num++;
                    }
                });
                if (num == 0) {
                    return false;
                }
                return true;
            };
            function InitPositionsCheckBox() {
                // 初始化职务
                $.each(positionTypeArr, function (index, item) {
                    var checkBoxStr = '';
                    checkBoxStr += `<div style="width:50%;float:left;margin-top:2px;">
                                    <input style="width:15px;height:15px;opacity:0.6;" type="checkbox" name="Positions" val="${item.PositionCode}" />
                                    <span style="vertical-align:top;">${item.PositionName}</span>
                                    </div>
                                    `
                    $('#PositionsCheckBox').append(checkBoxStr);
                })
                $('#PositionsCheckBox').find('[type="checkbox"]').each(function (index, item) {
                    $(item).on('click', function () {
                        if ($(item).parent().attr("class") == "checked") {
                            $(item).parent().removeAttr("class");
                        } else {
                            $(item).parent().attr("class", "checked");
                        }
                        //动态获取驾驶员信息事件
                        if ($(item).attr("val") == "QY009") {
                            if ($(item).prop("checked") == true) {
                                autoGetDriverInfo();
                            } else {
                                clearPersonalInfo();
                            }
                        }
                    })
                })

            };


            function getPersonalPosition() {
                helper.Ajax("003300300332", {}, function (data) {
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {
                            positionTypeArr = data.body;
                            InitPositionsCheckBox();
                        }
                    } else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);
            };

            //保存
            function save() {
                //todo 校验数据
                var jsonData1 = $('#Form1').serializeObject();
                //for (var key in jsonData1) {
                //    jsonData1[key] = jsonData1[key].replace(/\s/g, "");
                //}


                //调用新增接口

                //TODO
                helper.Ajax("006600200019", jsonData1, function (data) {
                    if ($.type(data) == "string") {
                        data = helper.StrToJson(data);
                    }
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {

                            toast.success("保存成功", { showDuration: 100 });
                            //window.parent.document.getElementById('hdIDS').value = jsonData1.Id;
                            setTimeout(function () { window.location.href = "List.html"; $("#tb_DangYuanInfo").CustomTable("reload");}, 2000);
                        }
                        else {
                            tipdialog.alertMsg("档案保存失败");
                        }
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);
            };


            initPage();
        });
});
