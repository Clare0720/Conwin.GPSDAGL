/**
 * Created by caihao on 17/2/20.
 */
//
define(['../../Config/conwin.main'], function () {
    require(['jquery', 'tipdialog', 'toast', 'formcontrol', 'selectcity', 'helper', 'prevNextpage', 'common'],
        function ($, warning, success, formcontrol, selectcity, helper, prevNextpage, common) {

            var CLXXEdit = {};
            CLXXEdit.resource = {};
            CLXXEdit.focusId = '';
            CLXXEdit.OriginResource;
            CLXXEdit.flag = false;
            /**
             * 页面初始化
             * */
            CLXXEdit.Initial = function () {

                //初始化页面 begin 
                //todo:（初始化页面）

                common.AutoFormScrollHeight('#XianLuAddForm');///页面布局初始化
                formcontrol.initial();///form表单控件初始化

                var ids = parent.window.document.getElementById('hdIDS').value;
                prevNextpage.initPageInfo(ids.split(','));//初始化上一页下一页按钮
                prevNextpage.bindPageClass();

                CLXXEdit.updateData();

                //初始化页面 end 

                //绑定按钮触发事件 begin 
                //todo:（绑定按钮触发事件）

                /**
                 * 保存按钮事件
                 * */
                $('#saveBtn').click(function (e) {
                    e.preventDefault();
                    ///验证输入
                    var flag = formcontrol.buttonValid();
                    if (flag) {
                        //todo:网络提交数据
                        // CLXXEdit.save();
                        CLXXEdit.tipMsg();//todo:测试提交,此方法会在提交函数中调用

                    }
                });

                /**
                 * 上一条按钮事件
                 * */
                $('#prevBtn').click(function (e) {
                    e.preventDefault();
                    prevNextpage.prev();
                    CLXXEdit.updateData(Id);
                });

                /**
                 * 下一条按钮事件
                 * */
                $('#nextBtn').click(function (e) {
                    e.preventDefault();
                    prevNextpage.next();
                    CLXXEdit.updateData();
                });

                /**
                 * 关闭按钮事件
                 * */
                $('#btnclose').click(function () {
                    warning.confirm("确定关闭？", function () {
                        if (CLXXEdit.flag) {
                            parent.window.$("#btnSearch").click();
                        } else {
                            CLXXEdit.close();
                        }
                    });
                });

                //绑定按钮触发事件 end 
            };

            /**
             * todo:网络请求获取数据
             * */
            CLXXEdit.GetCLXX = function (id, callback) {
                helper.Ajax("00020003", ///todo:"00020003"为接口服务地址，需要在/Config/conwin.system.js中配置
                    id, function (resultdata) {
                        callback(resultdata);
                    });
            }

            /**
             * todo:网络请求提交数据
             * */
            CLXXEdit.save = function (jsonData, callbackFn) {

                //向后台提交数据ajax
                helper.Ajax("00020003", ///"00020003"为接口服务地址，需要在/Config/conwin.system.js中配置
                    function (data) {
                        var result = { '': ids };
                        $.extend(data, result);
                    }, function (resultdata) {
                        if (resultdata.success) {

                            CLXXEdit.tipMsg();                       
                        }
                        else {
                            warning.errorDialog('编辑失败');
                        }
                    }, false);
            };

            /**
             * 更新数据，上一条下一条点击后初始化页面数据
             * */
            CLXXEdit.updateData = function () {
                var Id = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                CLXXEdit.GetCLXX(Id, function (serviceData) {
                    if (serviceData.publicresponse.statuscode == 0) {
                        CLXXEdit.resource = serviceData.body;
                        CLXXEdit.BindingData();
                    } else {
                        warning.errorDialog("请求数据失败:" + serviceData.publicresponse.message);
                    }
                    CLXXEdit.OriginResource = $('#Form1').serializeObject();
                });
            };

            /**
             * 关闭页面按钮事件
             * */
            CLXXEdit.close = function () {
                $('body').removeClass('rui-window-open');
                parent.document.body.className = "";
                parent.window.$iframe.css({ display: 'none' });
            };

            /**
             * 编辑按钮提示信息
             * */
            CLXXEdit.tipMsg = function () {
                if (prevNextpage.PageInfo.PageSize == 1) {
                    success.success('编辑成功！', { toastType: 2 });
                    setTimeout(function () {
                        parent.window.$("#btnSearch").click();
                        CLXXEdit.close();
                    }, 1000);
                } else {
                    if (prevNextpage.PageInfo.Index < prevNextpage.PageInfo.PageSize - 1) {
                        CLXXEdit.flag = true;
                        warning.confirm("编辑成功！您是否需要编辑下一条数据？", function () {
                            prevNextpage.next();
                            // CLXXEdit.updateData();
                        });
                    } else {
                        //warning.confirm("您勾选的数据全已编辑成功，是否退出编辑页？", function () {
                            success.success('保存成功！', { toastType: 2 });
                            setTimeout(function () {
                                parent.window.$("#btnSearch").click();
                                CLXXEdit.close();
                            }, 1000);
                       // });
                    }
                }
            };

            /**
             * 绑定form表单控件数据
             * */
            CLXXEdit.BindingData = function () {
                $('#Form1').find('input[name],select[name],textarea[name]').each(function (i, item) {
                    var tempValue = CLXXEdit.resource[$(item).attr('name')];
                    if (tempValue != undefined) {
                        if ($(item).hasClass('date-picker')) {
                            tempValue = tempValue.substr(0, 10);
                            $(item).datepicker('remove');
                            $(item).datepicker('update', tempValue);
                        }
                        $(item).val(tempValue.toString());
                    }
                });
                $('.input-daterange input').each(function () {
                    $(this).datepicker("clearDates");
                });
                $('#Form1').find('.input-daterange input[name]').each(function (i, item) {
                    var datevalue = CLXXEdit.resource[$(item).attr('name')];
                    if (datevalue != undefined) {
                        datevalue = datevalue.substr(0, 10);
                        //$(item).val(datevalue.toString());
                        //$(item).datepicker('remove');
                        $(item).datepicker('update', datevalue);
                    }
                })
                var station = { AreaCity: $.trim(CLXXEdit.resource['City']), AreaDistirct: $.trim(CLXXEdit.resource['Distirct']) }
                CLXXEdit.setXiaQu(station);
            }

            /**
             * 判断数据是否有改动
             * */
            CLXXEdit.CheckChange = function () {
                var jsonData = JSON.stringify($('#Form1').serializeObject());
                if (JSON.stringify(CLXXEdit.OriginResource) != jsonData) {
                    return false;
                }
                return true;
            }

            /**
             * 调用初始化方法
             * */
            CLXXEdit.Initial();
        });

    return {};
});

