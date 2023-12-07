define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'fileupload', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, fileupload) {
            var initPage = function () {
                //初始化页面样式
                common.AutoFormScrollHeight('#JiaShiYuan_Form');
                formcontrol.initial();
                //初始化表单数据
                initFormData();
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
                        helper.Ajax("006600200032", fromData, function (data) {
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


            //初始化页面表单数据
            function initFormData() {
                var ids = window.parent.document.getElementById('hdIDS').value;
                if (ids.split(',').length == 1) {
                    var id = ids.split(',')[0];
                    $("#Id").val(id);
                    GetData("006600200031", id, function (data) {
                        fillFormData(data, "JiaShiYuan_Form")
                    })
                }
                else {
                    tipdialog.errorDialog('请先选择需要操作的行');
                }
            }




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
                    } else {
                        if ($(item).attr('name') == "ShiFouKaiTongJianKongFuWu" || $(item).attr('name') == "ShiFouKaiTongShouJiChaChe") {
                            $(item).val('');
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

                //设置上传组件
                var array = [];
                $('.fujian').each(function (index, item) {
                    array.push(item.id);
                });
                fileupload.resetFileUpLoad(array);
                fileupload.rebindFileButtonEdit(array);

            };


            var setdate = function () { };

            //个性化代码块

            initPage();
        });
});