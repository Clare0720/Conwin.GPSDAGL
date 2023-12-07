/**
 * Created by Administrator on 2017/2/16.
 */
define(['form.config'],function () {
    var XianLuAdd = {};
    require(['jquery'],function ($) {
        require(['tipdialog','toastr','areas.select','textbox', 'textarea', 'toast','button', 'radio', 'checkbox', 'dropdown','selectcity'], function (warning,Toastr,s,textbox,textarea,success,button,radiobox,checkbox,dropdown,selectcity) {
            XianLuAdd.initPage = function () {
                /* //自适应高度
                 hub.autoFormHeight({
                 data: {
                 id: '#XianLuAddForm',
                 callbackFn: function (windowHeight) {
                 $('#fileManagement').height(windowHeight - 150);
                 }
                 }
                 });
                 //绑定滚动条自适应
                 $(window).on("resize.razorui.scroller.child-api", {
                 id: '#XianLuAddForm', callbackFn: function (windowHeight) {
                 $('#fileManagement').height(windowHeight - 150);
                 }
                 }, hub.autoFormHeight);
                 hub.tabToggleEvent(function (e) {
                 var targetHash = e.target.hash;
                 if (targetHash == '#xianLuInfo') {
                 $('#saveBtn').show();
                 $('#saveAndAddBtn').show();
                 } else {
                 $('#saveBtn').hide();
                 $('#saveAndAddBtn').hide();
                 }
                 });*/
                ///控件调用
                textbox.initial(); ///文本框
                textarea.initial();///多文本
                dropdown.initial();///下拉框
                radiobox.initial(); ///单选框
                checkbox.initial(); ///多选框
                button.initial({},function () {///按钮控件
                    warning.errorDialog("普通点击事件");
                });
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                var url= '../../Modules/Template/Resource/select.json';
                $('#AreaCity, #AreaCounty').empty().append(defaultOption);
                var servercode = '0000002005';
                var data={"Province":"广东"};
                //selectcity.setXiaQu(servercode,data,'#AreaCity', 'GetDistrictList', {CityName: $(this).val()}, 'DistrictName');
                selectcity.setXiaQuMoNi(url,'#AreaCity', 'GetDistrictList', {CityName:""}, 'DistrictName');
               $('#AreaCity').change(function () {
                    $('#AreaCounty').empty().append(defaultOption);
                    var servercode = '0000002006';
                    var data={"City":"广州"};
                    //selectcity.setXiaQu(servercode,data,'#AreaCity', 'GetDistrictList', {CityName: $(this).val()}, 'DistrictName');
                    if($(this).val()!='') {
                        selectcity.setXiaQuMoNi(url, '#AreaCounty', 'GetDistrictList', {CityName: $(this).val()}, 'DistrictName');
                    }
                });
               var xiaQuSheng, xiaQuShi, xiaQuXian, guanXiaXiaQu;
                xiaQuXian = $.trim($('#XiaQuXian').data('default'));
                if (xiaQuXian != "") {
                    $('#XiaQuXian').attr('readonly', 'readonly');
                }
                xiaQuShi = $.trim($('#XiaQuShi').data('default'));
                if (xiaQuShi != "") {
                    $('#XiaQuShi').attr('readonly', 'readonly');
                }
                xiaQuSheng = $.trim($('#XiaQuSheng').data('default'));
                if (xiaQuSheng != "") {
                    $('#XiaQuSheng').attr('readonly', 'readonly');
                }
                guanXiaXiaQu = $('#hdManageArea').val();
                if (guanXiaXiaQu != "") {
                    $('#XiaQuXian').removeAttr('readonly');
                    $('#XiaQuXian').removeAttr('readonly');//IE8需要两次
                    if (guanXiaXiaQu.indexOf(xiaQuXian) < 0) {
                        guanXiaXiaQu = xiaQuXian + ',' + guanXiaXiaQu;
                    }
                }
                $('select[readonly]').attr('disabled', true);
                ///bindProvinceDropdownboxCascade(xiaQuSheng, xiaQuShi, xiaQuXian, '', guanXiaXiaQu);
                $('#fileManagement iframe').attr('src', '/File/List?AppId=&AppName=&BusinessType=00001&BusinessId=' + $('#Id').val());
                /*
                $("#saveBtn").on('click',function () {
                    if ($('.areas-select-loading').length > 0) {
                        warning.errorDialog("正在读取相关数据，暂不能进行操作");
                        return false;
                    }
                   var data = $("#XianLuAddForm").serializeObject();
                    $('select[readonly][disabled]').each(function () {
                        var $c = $(this);
                        data[$c.attr('name')] = $c.val();
                    });
                    var msg = xiaQuXian.checkedForm(data);
                    if (msg != "") {
                        warning.errorDialog(msg);
                        return false;
                    }
                    xiaQuXian.remoteCheck(data.XianLuBianMa, "", function () {
                       var data = {
                            istransaction: '是',
                            isreturndetailresult: '是',
                            items: [data]
                        };
                        h.AjaxData("00020002", data, function (json) {
                            if (json.publicresponse.stuescdoe == '0') {
                                var d = json.body;
                                if (d.success) {
                                    if (typeof parent.insertCallbackFn == 'function') {
                                        parent.insertCallbackFn(true);
                                    }
                                    parent.Toastr.success('', '新增成功', true);
                                    parent.ReloadTable();
                                    parent.AjaxWindow.closeIframe();
                                } else {
                                    warning.errorDialog(d.msg || '服务错误，请联系管理员！');
                                }
                            }
                        });
                    });
               });
                $("#saveAndAddBtn").on('click',function () {
                    if ($('.areas-select-loading').length > 0) {
                        warning.errorDialog("正在读取相关数据，暂不能进行操作");
                        return false;
                    }
                    var data = $("#XianLuAddForm").serializeObject();
                    $('select[readonly][disabled]').each(function () {
                        var $c = $(this);
                        data[$c.attr('name')] = $c.val();
                    });
                    var msg = xiaQuXian.checkedForm(data);
                    if (msg != "") {
                        warning.errorDialog(msg);
                        return false;
                    }
                    XianLuAdd.remoteCheck(data.XianLuBianMa, "", function () {
                       $.ajax({
                            url: "/api/v1/CL/NongKeXianLu",
                            type: 'post',
                            data: data,
                            dataType: 'json',
                            beforeSend: function () {
                                Metronic.blockUI({animate: true});
                            },
                            success: function (d) {
                                if (d.success) {
                                    if (typeof parent.insertCallbackFn == 'function') {
                                        parent.insertCallbackFn(true);
                                    }
                                    Toastr.success('', '新增成功', true, {
                                        onHidden: function () {
                                            parent.ReloadTable();
                                            window.location.reload(false);
                                        }
                                    });
                                } else {
                                    warning.errorDialog(d.msg || '服务错误，请联系管理员！');
                                    Metronic.unblockUI();
                                }
                            },
                            error: function () {
                                Metronic.unblockUI();
                            },
                            complete: function () {
                                //Metronic.unblockUI();
                            }
                        });
                    });
               });*/
            };
           XianLuAdd.checkedForm = function (d) {
                var errorMessage = [];
                //if (d.XianLuBianMa == "") {
                //    errorMessage.push( '线路编码不能为空');
                //}
                if ($.trim(d.XianLuMingCheng) == "") {
                    errorMessage.push('线路名称不能为空');
                }
                if ($.trim(d.ShouFaZhanMingCheng) == "") {
                    errorMessage.push('首发站名称不能为空');
                }
                if ($.trim(d.ZhongDianZhanMingCheng) == "") {
                    errorMessage.push('终点站名称不能为空');
                }
                if ($.trim(d.ShouFaZhanMingCheng) != "" && $.trim(d.ZhongDianZhanMingCheng) != "" && $.trim(d.ShouFaZhanMingCheng) == $.trim(d.ZhongDianZhanMingCheng)) {
                    errorMessage.push('首发站名称不能与终点站名称相同');
                }
                //if (d.ZhuYaoTuJingZhanDian == "") {
                //    errorMessage.push('途径主要地点不能为空');
                //}
                if ($.trim(d.XianLuChangDu) != "" && !isNumber(d.XianLuChangDu)) {
                    errorMessage.push('线路长度(公里)格式不正确');
                }
                if ($.trim(d.RiFaBanCi) != "" && !isNumber(d.RiFaBanCi)) {
                    errorMessage.push('日发班次格式不正确');
                }
                if (d.XianLuLeiXing == "") {
                    errorMessage.push('班线类型不能为空');
                }
                if ($.trim(d.PJMTYunYingShiJian) != "" && !isNumber(d.PJMTYunYingShiJian)) {
                    errorMessage.push('平均每趟运营时间(分钟)格式不正确');
                }
                var str = "";
                if (errorMessage.length > 0) {
                    str = errorMessage.join('<br />');
                }
                return str;
            }
            XianLuAdd.remoteCheck = function (code, id, fn) {
                var data = {XianLuBianMa: code, Id: id};
                var data = {
                    istransaction: '是',
                    isreturndetailresult: '是',
                    items: [data]
                };
                h.AjaxData("00020001", data, function (json) {
                    if (json.publicresponse.stuescdoe == '0') {
                        var d = json.body;
                        if (serviceData.success) {
                            if (!serviceData.data) {
                                fn();
                            } else {
                                warning.errorDialog("所在辖区已存在该线路编码");
                            }
                        } else {
                            warning.errorDialog(serviceData.msg);
                        }
                    }
                });
            }
           function isNumber(value) {
                return /^[0-9]+$/.test(value);
            };
           function bindProvinceDropdownboxCascade(province, city, district, town, managearea) {
                var xiaqushi = new s.AreasSelect({
                    id: "#XiaQuShi", valuefield: "Key",
                    change: function () {
                        xiaquxian.reload('/api/v1/BaseDataModule/Area/GetDistricts?cityName=' + $('#XiaQuShi option:selected').text());
                    }
                });
                xiaqushi.load(city, '/api/v1/BaseDataModule/Area/GetCitys?provinceName=' + province);
                var xiaquxian = new s.AreasSelect({
                    id: "#XiaQuXian", valuefield: "Key", data: managearea,
                    change: function () {
                        xiaquzhen.reload('/api/v1/BaseDataModule/Area/GetTowns?districtName=' + $('#XiaQuXian option:selected').text());
                    }
                });
                xiaquxian.load(district, '/api/v1/BaseDataModule/Area/GetDistricts?cityName=' + city);
            }
            $(function () {
                XianLuAdd.initPage();
            });
        });
    });
});