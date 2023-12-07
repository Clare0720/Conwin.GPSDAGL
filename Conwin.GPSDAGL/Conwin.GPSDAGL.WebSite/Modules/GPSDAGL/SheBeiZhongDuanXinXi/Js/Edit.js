define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, Metronic) {
            //模块初始化
            var initPage = function () {
                //初始化页面样式
                common.AutoFormScrollHeight('#Form1');
                formcontrol.initial();
                //翻页控件
                var ids = window.parent.document.getElementById('hdIDS').value;
                prevNextpage.initPageInfo(ids.split(','));
                prevNextpage.bindPageClass();
                //初始化子表
                //关闭
                $('#btnclose').click(function () {
                    tipdialog.confirm("确定关闭？", function (r) {
                        if (r) {
                            parent.window.$("#btnSearch").click();
                            popdialog.closeIframe();
                        }
                    });
                });
                //上一条
                $('#prevBtn').click(function (e) {
                    e.preventDefault();
                    prevNextpage.prev();
                    updateData();
                });
                //下一条
                $('#nextBtn').click(function (e) {
                    e.preventDefault();
                    prevNextpage.next();
                    updateData();
                });
                //tab3
                $('#tab3').click(function (e) {
                    e.preventDefault();
                    $('#FuJian').filelist({
                        'type': 'edit',
                        'businessType': '000001',
                        'timeOut': 20000,
                        'businessId': $('#Id').val()
                    });
                });
                updateData();
                //保存
                $('#saveBtn').on('click', function (e) {
                    e.preventDefault();
                    var flags = formcontrol.buttonValid();
                    if (flags) {
                        save();
                    }
                });
                //个性化代码块
                //region
                selectCity();
                $('.datepicker').datepicker({
                    language: 'zh-CN',
                    format: 'yyyy-mm-dd',
                    autoclose: true//选中之后自动隐藏日期选择框
                });
                $('.datetimepicker').datetimepicker({
                    language: 'zh-CN',
                    startView: 1,
                    maxView: 0,
                    format: 'hh:ii',
                    autoclose: true//选中之后自动隐藏日期选择框
                });
                //endregion
            };
            //主表-刷新数据
            function updateData() {
                var id = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                getXianLuXinXi(id, function (serviceData) {
                    if (serviceData.publicresponse.statuscode == 0) {
                        updateTag();
                        fillFormData(serviceData.body);
                    } else {
                        tipdialog.errorDialog("请求数据失败");
                    }
                });
            };
            //主表-更新tab状态
            function updateTag() {
                $('#tab1').parent('li').addClass('active');
                $('#JiChuXinXi').addClass('active in');
                $('#tab3').parent('li').removeClass('active');
                $('#FuJian').removeClass('active in');
            };
            //主表-获取主表数据
            function getXianLuXinXi(id, callback) {
                helper.Ajax("003300300522", id, function (resultdata) {
                    if (typeof callback == 'function') {
                        callback(resultdata);
                    }
                }, false);
            };
            //主表-绑定主表数据
            function fillFormData(resource) {
                $('#Form1').find('input[name],select[name],textarea[name]').each(function (i, item) {
                    var tempValue = resource[$(item).attr('name')];
                    if (tempValue != undefined) {
                        if ($(item).hasClass('datetimepicker')) {
                            tempValue = tempValue.substr(0, 10) + ' ' + tempValue.substr(11, 5);
                            $(item).val(tempValue == undefined ? '' : tempValue);
                        }else if ($(item).hasClass('datepicker')) {
                            tempValue = tempValue.substr(0, 10);
                            $(item).val(tempValue == undefined ? '' : tempValue);
                        } else {
                            //TODO: 赋值
                            $(item).val(tempValue.toString() == '' ? '' : tempValue);
                        }
                    } else {
                        $(item).val('');
                    }
                });
                xiaQuXinXi(resource);

                if ($('#ZhongDuanBiaoZhiId').val() != '') {
                    var url = helper.Route('00000080004', '1.0', system.ServerAgent) + '?id=' + $('#ZhongDuanBiaoZhiId').val();
                    $('#imgUpLoad').attr("src", url);
                }
                imageLoad();



                $('.SheBeiXingHao').html($('#SheBeiXingHao').val());
                $('.infoList').append($('#ShengChanChangJia').val() != "" ? '<li>' + $('#ShengChanChangJia').val() + '</li>':'')
                    .append($('#ShiYongCheXing').val() != "" ? '<li>' + $('#ShiYongCheXing').val() + '</li>':'')
                    .append($('#DingWeiMoKuai').val() != "" ? '<li>' + $('#DingWeiMoKuai').val() + '</li>' : '');
            };
            //主表-保存

            function imageLoad() {
                $('#imgUpLoad').fileupload({
                    businessId: $('#Id').val(),
                    multi: false,
                    timeOut: 20000,
                    allowedContentType: 'png|jpg|jpeg|tif|gif|pdf',
                    callback: function (data) {
                        $("#ZhongDuanBiaoZhiId").val(data.FileId);
                        $('#' + data.FileId + 'View').hide();
                        $('#' + data.FileId + 'Delete').hide();
                        $('#divImgUpLoad').append("<img id='imgUpLoad' src='" + helper.Route('00000080004', '1.0', system.ServerAgent) + '?id=' + data.FileId + "' style='width:200px;height:200px;float:left;padding:6px;' />");
                        imageLoad();
                    }
                });
            }

            function save() {
                //TODO:数据校验
                var jsonData1 = $('#Form1').serializeObject();
                // var ZhongDuanBianMaData = { "data": jsonData1.ZhongDuanBianMa };
                //调用查重接口
                helper.Ajax("003300300523", jsonData1, function (data) {
                    if (data.body) {
                        //调用修改接口
                        helper.Ajax("003300300525", jsonData1, function (data) {
                            if (data.body) {
                                toast.success("档案保存成功");
                                setTimeout(function () { window.location.reload(false); }, 800);
                            }
                            else {
                                tipdialog.alertMsg("档案保存失败");
                            }
                        }, false);
                    } else {
                        tipdialog.alertMsg("相同的终端已经存在，无法保存");
                    }
                }, false);

            };
            //子表-初始化分页信息
            var tabPageInfo = tabPage();
            //子表-初始化校验信息
            var tabButtonInfo = tabButton();
            //子表-分页
            function tabPage() {
                var tabPageInfo = {};
                tabPageInfo.bindPageClass = function () {
                    var currentPageInfo = tabPageInfo.PageInfo;
                    if (currentPageInfo.HasNext) {
                        $('#nextTabBtn').removeClass('disabled');
                        $('#nextTabBtn').removeClass('c-gray-btn');
                        $('#nextTabBtn').removeAttr('disabled');
                        $('#nextTabBtn').addClass('c-green');
                        $('#nextTabBtn').children(':first').removeClass('m-icon-gray');
                        $('#nextTabBtn').children(':first').addClass('m-icon-white');
                    } else {
                        $('#nextTabBtn').addClass('disabled');
                        $('#nextTabBtn').addClass('c-gray-btn');
                        $('#nextTabBtn').attr("disabled", "disabled");
                        $('#nextTabBtn').removeClass('c-green');
                        $('#nextTabBtn').children(':first').addClass('m-icon-gray');
                        $('#nextTabBtn').children(':first').removeClass('m-icon-white');
                    }
                    if (currentPageInfo.HasPrev) {
                        $('#prevTabBtn').removeClass('disabled');
                        $('#prevTabBtn').removeClass('c-gray-btn');
                        $('#prevTabBtn').removeAttr('disabled');
                        $('#prevTabBtn').addClass('c-green');
                        $('#prevTabBtn').children(':first').removeClass('m-icon-gray');
                        $('#prevTabBtn').children(':first').addClass('m-icon-white');
                    } else {
                        $('#prevTabBtn').addClass('disabled');
                        $('#prevTabBtn').addClass('c-gray-btn');
                        $('#prevTabBtn').attr("disabled", "disabled");
                        $('#prevTabBtn').removeClass('c-green');
                        $('#prevTabBtn').children(':first').addClass('m-icon-gray');
                        $('#prevTabBtn').children(':first').removeClass('m-icon-white');
                    }
                };
                //分页信息
                tabPageInfo.PageInfo = {
                    IDS: [],
                    Index: 0,
                    PageSize: 0,
                    HasPrev: false,
                    HasNext: false
                };
                //初始化子页面记录数据
                tabPageInfo.initPageInfo = function (ids) {
                    tabPageInfo.PageInfo.IDS = ids;
                    tabPageInfo.PageInfo.Index = 0;
                    tabPageInfo.PageInfo.PageSize = ids.length;
                    tabPageInfo.PageInfo.HasNext = ids.length > 1;
                    tabPageInfo.PageInfo.HasPrev = false;
                };
                //计算分页信息
                tabPageInfo.calculatePage = function (tag) {
                    if (tag == undefined)
                        return tabPageInfo.PageInfo;
                    //标识
                    if (tag > 0) {
                        tabPageInfo.PageInfo.Index++;
                    }
                    else {
                        tabPageInfo.PageInfo.Index--;
                    }
                    tabPageInfo.PageInfo.HasNext = tabPageInfo.PageInfo.PageSize > (tabPageInfo.PageInfo.Index + 1);
                    tabPageInfo.PageInfo.HasPrev = tabPageInfo.PageInfo.Index > 0;
                    return tabPageInfo.PageInfo;
                };
                tabPageInfo.next = function () {
                    tabPageInfo.calculatePage(1);
                    tabPageInfo.bindPageClass();
                };
                tabPageInfo.prev = function () {
                    tabPageInfo.calculatePage(-1);
                    tabPageInfo.bindPageClass();
                };
                return tabPageInfo;
            }
            //子表-必填项校验
            function tabButton() {
                var button = {};
                var defaults = {};
                ///初始化
                button.initial = function (elemt, options) {
                    var param;
                    var message = '';
                    var additionMsg = '';
                    var focusId = undefined;
                    param = $.extend({}, defaults, options);
                    return button.checkItem(elemt, message, additionMsg, focusId, param);
                }
                ///提交驗證
                button.checkItem = function (elemt, message, additionMsg, focusId, param) {
                    $(elemt).find('.required').filter('input[name],select[name],textarea[name]').each(function (i, item) {
                        if (!Check(item)) {
                            if (item.nodeName == 'SELECT') {
                                if ($(item).val() == '') {
                                    message += getControlLabel(item).replace('*', '') + getControlDescription(item) + '<br />';
                                    $(item).parent('div').addClass('data-box-red');
                                } else {
                                    $(item).parent('div').removeClass('data-box-red');
                                }
                            } else {
                                if (item.type == 'radio' || item.type == 'checkbox') {
                                    var temp = getControlLabelByName(item.name).replace('*', '');
                                    if (message.indexOf(temp) == -1) {
                                        var ra = $(':' + item.type + ':checked');
                                        var len = ra.length;
                                        if (len == 0) {
                                            message += temp + getControlDescription(item) + '<br />';
                                            $(item).parent('div').addClass('data-box-red');
                                            $(item).parent('div').css({
                                                'float': 'left',
                                                'padding': '3px 12px 3px 8px'
                                            });
                                        } else {
                                            $(item).parent('div').removeClass('data-box-red');
                                            $(item).parent('div').css('float', '');
                                        }
                                    }
                                } else {
                                    var temp = getControlLabel(item).replace('*', '');
                                    if ($(item).val() != '' && $(item).attr('data-cwvalid') != undefined) {
                                        additionMsg += "请输入正确的" + temp + '<br />';
                                    } else {
                                        $(item).addClass('data-box-red');
                                        message += temp + getControlDescription(item) + '<br />';
                                    }
                                }
                            }
                            if (focusId == undefined) {
                                focusId = $(item).attr('id');
                            }
                        }
                    });
                    message += additionMsg;
                    if (message != '') {
                        if (focusId != undefined) {
                            tipdialog.errorDialog(message);
                        }
                        $('#TipModal').on('hidden.bs.modal', function () {
                            $('#' + focusId).focus();
                        });
                        return false;
                    } else {
                        return true;
                    }
                }
                ///檢查必填項
                function Check(item) {
                    var vaild = $(item).data("cwvalid");
                    if (vaild != null) {
                        var regxString = $(item).data("cwvalid").regx;
                        if (regxString != undefined) {
                            var regx = new RegExp(regxString);
                            if (regx.test($.trim($(item).val()))) {
                                return true;
                            }
                        } else if ($.trim($(item).val()).length > 0) {
                            if (item.nodeName == 'INPUT') {
                                return true;
                            }
                            if (item.nodeName == 'TEXTAREA' && $.trim($(item).val()).length >= vaild.minlength && $.trim($(item).val()).length <= vaild.maxlength) {
                                return true;
                            }
                        }
                    }
                    return false;
                };
                function getControlLabel(control) {
                    return $('label[for=' + control.id + ']').text();
                }
                function getControlLabelByName(name) {
                    return $('label[for=' + name + ']').text();
                }
                function getControlDescription(control) {
                    var description = '是必填项';
                    if (!(control instanceof jQuery)) {
                        control = $(control);
                    }
                    var elementType = control.prop('nodeName');
                    switch (elementType) {
                        case 'INPUT':
                            inputType = $(control).attr('type');
                            description = getInputDescription(inputType);
                            break;
                        case 'SELECT':
                            description = '是必选项';
                            break;
                        case 'TEXTAREA':
                            description = '是必填项';
                            break;
                        default:
                            break;
                    }
                    return description;
                }
                function getInputDescription(inputType) {
                    var description = '';
                    switch (inputType) {
                        case 'checkbox':
                            description = '是必选项';
                            break;
                        case 'radio':
                            description = '是必选项';
                            break;
                        case 'hidden':
                            description = '是必填项';
                            break;
                        case 'number':
                            description = '是必填项';
                            break;
                        case 'password':
                            description = '是必填项';
                            break;
                        case 'text':
                            description = '是必填项';
                            break;
                        default:
                            break;
                    }
                    return description;
                }
                function getButtonByParam(option) {
                    var html = '<label class="control-label col-xs-4 col-sm-2"> &nbsp;' + option.labelName + '</label>' + '<div class="col-xs-8 col-sm-8"> ';
                    for (var i = 0; i < option.buttonObject.length; i++) {
                        html += '<button id="' + option.buttonObject[i].buttonId + '" class="btn ' + option.buttonObject[i].triggerBtn + '" style="line-height: 0; width:' + option.width + '; height: ' + option.height + '; color:' + option.fontColor + '; font-size: ' + option.fontSize + '; background-color: ' + option.btncolor.one + '"> <i class="fa fa-save"></i>' + option.buttonObject[i].btnValue + '</button>';
                    }
                    html += '</div>';
                    $('.button-init').append(html);
                }
                return button;
            }
            //个性化代码块
            //region
            $('.datepicker').datepicker({
                language: 'zh-CN',
                format: 'yyyy-mm-dd',
                autoclose: true//选中之后自动隐藏日期选择框
            });
            $('.datetimepicker').datetimepicker({
                language: 'zh-CN',
                startView: 2,
                maxView: 3,
                format: 'yyyy-mm-dd hh:ii',
                autoclose: true//选中之后自动隐藏日期选择框
            });
            function selectCity() {
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#XiaQuShi, #XiaQuXian').empty().append(defaultOption);
                selectcity.setXiaQu('00000020005', { "Province": "广东" }, '#XiaQuShi', 'GetCityList', 'CityName');
                $('#XiaQuShi').change(function () {
                    $('#XiaQuXian,#XiaQuZhen').empty().append(defaultOption);
                    var data = { "City": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化区县下拉框
                        selectcity.setXiaQu('00000020006', data, '#XiaQuXian', 'GetDistrictList', 'DistrictName');
                    }
                });
                $('#XiaQuXian').change(function () {
                    $('#XiaQuZhen').empty().append(defaultOption);
                    var data = { "District": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化区镇下拉框
                        selectcity.setXiaQu('00000020007', data, '#XiaQuZhen', 'GetTownList', 'TownName');
                    }
                });
            };
            function xiaQuXinXi(resource) {
                selectcity.setXiaQu('00000020005', { "Province": "广东" }, '#XiaQuShi', 'GetCityList', 'Key', 'Key', resource.XiaQuShi);
                selectcity.setXiaQu('00000020006', { 'City': resource.XiaQuShi }, '#XiaQuXian', 'GetDistrictList', 'Key', 'Key', resource.XiaQuXian);
                if (resource.XiaQuXian) {
                    selectcity.setXiaQu('00000020007', { 'District': resource.XiaQuXian }, '#XiaQuZhen', 'GetTownList', 'Key', 'Key', resource.XiaQuZhen);
                }
            };
            //endregion
            initPage();
        });
});