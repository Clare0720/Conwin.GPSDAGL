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
                //关闭
                $('#btnclose').click(function () {
                    popdialog.closeIframe();
                });
            };

            //初始化页面表单数据
            function initFormData() {
                var ids = window.parent.document.getElementById('hdIDS').value;
                if (ids.split(',').length == 1) {
                    var id = ids.split(',')[0];
                    $("#Id").val(id);
                    GetData("006600200031", id, function (data) {
                        fillFormData(data, "JiaShiYuan_Form")
                    });
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
                    }
                });

                //设置下拉框的值
                if ($("select[name]").length > 0) {
                    $.each($("select[name]"), function (i, select) {
                        var showText = $(select).find("option:selected").val() == '' ? '' : $(select).find("option:selected").text();
                        $(select).replaceWith('<input readonly type="text" name="' + select.name + '" id="' + select.name + '" value="' + showText + '" class="form-control" style="border:none;background:transparent!important" />');
                    });
                }

                //初始化图片查看控件
                var array = [];
                $('.fujian').each(function (index, item) {
                    if ($('#' + item.id).val() != '') {
                        array.push(item.id);
                    }
                    else {
                        //$('#' + item.id).nextAll().remove();
                        $('#' + item.id).after('<span>暂无</span>');
                    }
                });
                fileupload.rebindFileButtonView(array);

            };

            initPage();
        });
});