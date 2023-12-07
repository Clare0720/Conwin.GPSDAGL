define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'fileupload', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, fileupload) {
            var initPage = function () {
                //初始化页面样式
                common.AutoFormScrollHeight('#JiaShiYuan_Form');
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
                //初始化附件上传按钮
                $('.fa-upload').each(function (index, item) {
                    $('#' + $(item).parent()[0].id).fileupload({
                        multi: false,
                        timeOut: 20000,
                        allowedContentType: 'png|jpg|jpeg'
                    });
                });
                //关闭
                $('#btnclose').click(function () {
                    tipdialog.confirm("确定要关闭吗？", function (r) {
                        if (r) {
                            popdialog.closeIframe();
                        }
                    });
                });
                //取消
                $('#cancelBtn').click(function () {
                    tipdialog.confirm("确定要取消吗？", function (r) {
                        if (r) {
                            popdialog.closeIframe();
                        }
                    });
                });
                //保存
                $('#saveBtn').click(function () {
                    var flag = true;
                    var msg = '';
                    var fromData = $('#JiaShiYuan_Form').serializeObject();
                    //数据验证
                    if ($.trim(fromData.Name) == '') {
                        flag = false;
                        msg += '姓名是必填项<br/>';
                    }
                    if ($.trim(fromData.IDCard) == '') {
                        flag = false;
                        msg += '证件号码是必填项<br/>';
                    }
                    else {
                        if (check_idcard($.trim(fromData.IDCard)) == false) {
                            flag = false;
                            msg += '证件号码格式有误<br/>';
                        }
                    }
                    if ($.trim(fromData.Cellphone) == '') {
                        flag = false;
                        msg += '联系电话是必填项<br/>';
                    }
                    //else {
                    //    if (check_tellnum($.trim(fromData.Cellphone)) == false) {
                    //        flag = false;
                    //        msg += '联系电话格式有误<br/>';
                    //    }
                    //}
                    if ($.trim(fromData.Certification) == '') {
                        flag = false;
                        msg += '从业资格证号是必填项<br/>';
                    }
                    //提交
                    if (!flag) {
                        tipdialog.alertMsg(msg);
                    }
                    else {
                        helper.Ajax("006600200029", fromData, function (data) {
                            debugger
                            if ($.type(data) == "string") {
                                data = helper.StrToJson(data);
                            }
                            if (data.publicresponse.statuscode == 0) {
                                if (data.body) {
                                    toast.success("保存成功", { showDuration: 100 });
                                    setTimeout(function () {
                                        parent.$("#btnSearch").click();
                                        popdialog.closeIframe();
                                    }, 2000);
                                }
                                else {
                                    tipdialog.alertMsg("驾驶员档案保存失败");
                                }
                            }
                            else {
                                tipdialog.alertMsg(data.publicresponse.message);
                            }
                        }, false);
                    }
                });
            };

            //身份证号码校验
            function check_idcard(value) {
                var arrExp = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2];//加权因子  
                var arrValid = [1, 0, "X", 9, 8, 7, 6, 5, 4, 3, 2];//校验码  
                if (/^\d{17}\d|x$/i.test(value)) {
                    var sum = 0, idx;
                    for (var i = 0; i < value.length - 1; i++) {
                        // 对前17位数字与权值乘积求和  
                        sum += parseInt(value.substr(i, 1), 10) * arrExp[i];
                    }
                    // 计算模（固定算法）  
                    idx = sum % 11;
                    // 检验第18为是否与校验码相等  
                    return arrValid[idx] == value.substr(17, 1).toUpperCase();
                } else {
                    return false;
                }
            }

            //手机号码校验
            function check_tellnum(content) {
                // 正则验证格式
                eval("var reg = /^1[34578]\\d{9}$/;");
                return RegExp(reg).test(content);
            }

            initPage();
        });
});