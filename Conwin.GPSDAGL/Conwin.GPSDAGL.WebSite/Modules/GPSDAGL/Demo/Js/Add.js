define(['../../Config/conwin.main'], function () {
    require(['jquery', 'tipdialog', 'toast', 'formcontrol', 'selectcity', 'helper', 'common'],
        function ($, warning, success, formcontrol, selectcity, helper, common) {
            var CLXXAdd = {};
            CLXXAdd.resource = {};
            CLXXAdd.focusId = '';
            CLXXAdd.OriginResourceZc; ///暂存对象
            CLXXAdd.OriginResourceBc; ///保存对象
            CLXXAdd.OriginResourceBcAndAdd; ///保存并新增对象
            CLXXAdd.dataList = new Array('', '', ''); ///对象集合
            CLXXAdd.pageSize = 3; ///tab数量
            CLXXAdd.pageIndex = 0; ///第一个tab
            /**
             * 初始化方法
             * */
            CLXXAdd.Initial = function () {
                //初始化页面 begin 
                //todo:（初始化页面）
                common.AutoFormHeight('#form1');
                formcontrol.initial();
                ///初始化省市县下拉框
                selectCity();
                Metronic.init();
                //初始化页面 end 
                //绑定按钮触发事件 begin 
                //todo:（绑定按钮触发事件）
                /**
                 * 关闭按钮事件
                 * ***/
                $('#btnclose').click(function () {
                    warning.confirm("确定要关闭吗？", function () {
                            CLXXAdd.close();
                    });
                });
                /**
                 * 暂存按钮事件
                 * ***/
                $('#stopSaveBtn').click(function (e) {
                    e.preventDefault();
                    CLXXAdd.dataList[CLXXAdd.pageIndex] = JSON.stringify($('#form' + (CLXXAdd.pageIndex + 1)).serializeObject());
                    success.success('暂存成功!', { toastType: 2 });
                });
                /**
                 * 保存按钮事件
                 * ***/
                $('#saveBtn').click(function (e) {
                    e.preventDefault();
                    var flag = formcontrol.buttonValid();///验证按钮
                    if (flag) {
                        ///判断是否有多个tab.若有一个则直接新增关闭窗体,多个则只新增不关闭
                        if (CLXXAdd.pageSize == 1) {
                            ///新增操作
                            CLXXAdd.dataList[CLXXAdd.pageIndex] = JSON.stringify($('#form' + (CLXXAdd.pageIndex + 1)).serializeObject());
                            ///模拟提交
                            success.success('保存成功！', { toastType: 2 });
                            ///todo:  进行网络提交
                            //CLXXAdd.save();
                            CLXXAdd.close();
                        } else {
                            ///新增操作
                            CLXXAdd.dataList[CLXXAdd.pageIndex] = JSON.stringify($('#form' + (CLXXAdd.pageIndex + 1)).serializeObject());
                            ///模拟提交
                            success.success('保存成功！', { toastType: 2 });
                            //todo:  进行网络提交
                            //CLXXAdd.save();
                        }
                    }
                });
                /**
                 * 保存并新增按钮事件
                 * ***/
                $('#saveAndAddBtn').click(function (e) {
                    e.preventDefault();
                    var flag = formcontrol.buttonValid();///验证按钮
                    if (flag) {
                        CLXXAdd.dataList[CLXXAdd.pageIndex] = JSON.stringify($('#form' + (CLXXAdd.pageIndex + 1)).serializeObject());
                        ///模拟提交
                        success.success('保存并新增成功！', { toastType: 2 });
                        //CLXXAdd.save(jsonData);
                        ///清空所有
                        $('#form1').find('input[type=text]:not(:disabled),textarea:not(:disabled), select:not(:disabled)').val('');
                        if (helper.BrowserVersion.browser == "IE" && helper.BrowserVersion.version == 8) {
                            $('#form1').find('input[name]').removeAttr('checked');
                        } else {
                            $('#form1').find('input[name]').removeAttr('checked');
                            $('#form1').find('input[name]').each(function (i, item) {
                                $(item).parent('span').removeClass('checked');
                            })
                        }
                    }
                });
                /**
                 * 重置按钮事件
                 * ***/
                $("#ResetBtn").click(function () {
                    CLXXAdd.BindingData();
                });
                /**
                 * tab切换事件，每次切换都要提示保存或暂存数据
                 * ***/
                $('#tab1').click(function (e) {
                    if ($('#tab1').parent('li').hasClass('active')) {
                        e.preventDefault();
                    } else {
                        CLXXAdd.pageIndex = 0;
                        var jsonData = JSON.stringify($('#form2').serializeObject());
                        if (jsonData != CLXXAdd.dataList[1]) {
                            warning.alertMsg('请先点击暂存/保存按钮!', function () {
                                CLXXAdd.pageIndex = 1;
                                $('#tab1').parent('li').removeClass('active');
                                $('#tab2').parent('li').addClass('active');
                            });
                            if ($('.bootbox-body').html() == '请先点击暂存/保存按钮!') {
                                $('.bootbox-close-button').click(function () {
                                    CLXXAdd.pageIndex = 1;
                                    $('#tab1').parent('li').removeClass('active');
                                    $('#tab2').parent('li').addClass('active');
                                });
                            }
                        } else {
                            $('#divform1').addClass('active');
                            $('#divform2').removeClass('active');
                            CLXXAdd.BindingData('#form1');
                        }
                    }
                });
                $('#tab2').click(function (e) {
                    if ($('#tab2').parent('li').hasClass('active')) {
                        e.preventDefault();
                    } else {
                        CLXXAdd.pageIndex = 1;
                        var jsonData = JSON.stringify($('#form1').serializeObject());
                        if (jsonData != CLXXAdd.dataList[0]) {
                            warning.alertMsg('请先点击暂存/保存按钮!', function () {
                                CLXXAdd.pageIndex = 0;
                                $('#tab2').parent('li').removeClass('active');
                                $('#tab1').parent('li').addClass('active');
                            });
                            if ($('.bootbox-body').html() == '请先点击暂存/保存按钮!') {
                                $('.bootbox-close-button').click(function () {
                                    CLXXAdd.pageIndex = 0;
                                    $('#tab2').parent('li').removeClass('active');
                                    $('#tab1').parent('li').addClass('active');
                                });
                            }
                        } else {
                            $('#divform2').addClass('active');
                            $('#divform1').removeClass('active');
                            CLXXAdd.BindingData('#form2');
                        }
                    }
                });
                //绑定按钮触发事件 end 
            };
            /**
             * todo:调用服务接口提交数据
             * ***/
            CLXXAdd.save = function (jsonData, callbackFn) {
                helper.Ajax("00020003", ///todo:"00020003"为接口服务地址，需要在/Config/conwin.system.js中配置
                    function (data) {
                        var result = { '': ids };///todo:接口参数
                        $.extend(data, result);
                    }, function (resultdata) {
                        if (resultdata.success) {
                            success.success('新增成功');
                        }
                        else {
                            success.error('', '新增失败');
                        }
                    }, false);
            };
            /**
             * 关闭当前页
             * ***/
            CLXXAdd.close = function () {
                $('body').removeClass('rui-window-open');
                parent.document.body.className = "";
                parent.window.$iframe.css({ display: 'none' });
            };
            /**
             * 检查数据是否有改动
             * **/
            CLXXAdd.CheckChange = function () {
                var jsonData = JSON.stringify($('#Form1').serializeObject());
                if (JSON.stringify(CLXXAdd.OriginResource) != jsonData) {
                    return false;
                }
                return true;
            }
            /**
             * 绑定表单数据
             * ***/
            CLXXAdd.BindingData = function (tab) {
                $(tab).find('input[name],select[name],textarea[name]').each(function (i, item) {
                    var tempValue = CLXXAdd.resource[$(item).attr('name')];
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
                $(tab).find('.input-daterange input[name]').each(function (i, item) {
                    var datevalue = CLXXAdd.resource[$(item).attr('name')];
                    if (datevalue != undefined) {
                        datevalue = datevalue.substr(0, 10);
                        //$(item).val(datevalue.toString());
                        //$(item).datepicker('remove');
                        $(item).datepicker('update', datevalue);
                    }
                })
                var station = { AreaCity: $.trim(CLXXAdd.resource['City']), AreaCountry: $.trim(CLXXAdd.resource['Distirct']) }
                ///CLXXAdd.setXiaQu(station);
            }
            /**
             * 级联城市下拉框
             * **/
            function selectCity() {
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#AreaCity, #AreaCounty').empty().append(defaultOption);
                var data = { "Province": "广东" };///todo:初始化省份
                ///调用接口初始化城市下拉框
                selectcity.setXiaQu('00000020005', data, '#AreaCity', 'GetCityList', 'CityName');
                $('#AreaCity').change(function () {
                    $('#AreaCounty').empty().append(defaultOption);
                    var data = { "City": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化区县下拉框
                        selectcity.setXiaQu('00000020006', data, '#AreaCounty', 'GetDistrictList', 'DistrictName');
                    }
                });
            }
            /**
            * 调用初始化方法
            * */
            CLXXAdd.Initial();
        });
    return {};
});
