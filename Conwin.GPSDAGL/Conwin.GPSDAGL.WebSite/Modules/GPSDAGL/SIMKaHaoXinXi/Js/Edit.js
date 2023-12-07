define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'system', 'selectcity', 'filelist', 'fileupload', 'bootstrap-datepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, system, selectcity, filelist, fileupload) {
            var initPage = function () {
                common.AutoFormScrollHeight('#Form1');
                formcontrol.initial();
                var ids = window.parent.document.getElementById('hdIDS').value;
                prevNextpage.initPageInfo(ids.split(','));
                prevNextpage.bindPageClass();
                $('#btnclose').on('click', function () {
                    parent.window.$("#btnSearch").click();
                    popdialog.closeIframe();
                });
                updateData();
                $('#saveBtn').on('click', function (e) {
                    e.preventDefault();
                    var flags = formcontrol.buttonValid();
                    if (flags) {
                        save()
                    }
                });
                $('#SIMKaHao,#SIMXuLieHao').on('blur', function () {
                    checkSIM($(this));
                });
            }

            function updateData() {
                var id = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                getSIMInfo(id, function (serviceData) {
                    if (serviceData.publicresponse.statuscode == 0) {
                        fillFormData(serviceData.body)
                    } else {
                        tipdialog.errorDialog("请求数据失败")
                    }
                })
            }

            function getSIMInfo(id, callback) {
                helper.Ajax("003300300135", id, function (resultdata) {
                    if (typeof callback == 'function') {
                        callback(resultdata)
                    }
                }, false);
            }

            function fillFormData(resource) {
                $('#Form1').find('input[name],select[name],textarea[name]').each(function (i, item) {
                    var tempValue = resource[$(item).attr('name')];
                    if (tempValue != undefined) {
                        if ($(item).hasClass('datepicker')) {
                            $(item).datepicker('update', tempValue.substr(0, 10));
                        } else {
                            if ($(item).hasClass('decimal')) {
                                tempValue = formatDecimal(tempValue);
                            }
                            if ($(item).hasClass('int')) {
                                tempValue = formatInt(tempValue);
                            }
                            $(item).val(tempValue);
                        }                      
                    } else {
                        $(item).val('');
                    }
                });
                selectCity(resource.XiaQuShi);
            }

            function save() {
                var jsonData = $('#Form1').serializeObject();
                jsonData.ShiYongZhuangTai = $('#ShiYongZhuangTai').val();
                jsonData.ZhuangTai = $('#ZhuangTai').val();
                helper.Ajax("003300300133", jsonData, function (data) {
                    if (data.publicresponse.statuscode == 0 && data.body === true) {
                        parent.window.require(['toast'], function (Toastr) {
                            Toastr.success("档案保存成功");
                        });
                        parent.window.$("#btnSearch").click();
                        popdialog.closeIframe();
                    } else {
                        tipdialog.alertMsg("档案保存失败")
                    }
                }, false);              
            }

            function selectCity(XiaQu) {
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#XiaQuShi').empty().append(defaultOption);
                selectcity.setXiaQu('00000020005', {
                    "Province": "广东"
                }, '#XiaQuShi', function () {
                    if (XiaQu) {
                        $('#XiaQuShi').val(XiaQu);
                    }                    
                });
            }

            function formatDecimal(data) {
                return (Math.round(data * 100) / 100).toFixed(2);
            }

            function formatInt(data) {
                return Math.round(data).toString();
            }

            function checkSIM(element) {
                var SIMKaHao = $('#SIMKaHao').val();
                var SIMXuLieHao = $('#SIMXuLieHao').val();
                var Id = $('#Id').val();
                if (SIMKaHao != '' || SIMXuLieHao != '') {
                    helper.Ajax("003300300142", { Id: Id, SIMKaHao: SIMKaHao, SIMXuLieHao: SIMXuLieHao }, function (data) {
                        if (data.body != null && data.body.result == false) {
                            $(element).val('');
                            tipdialog.alertMsg(data.body.msg, function () {
                                $(element).focus();
                            });
                        }
                    }, false);
                }

                if (SIMKaHao != '' && SIMXuLieHao != '') {
                    helper.Ajax("003300300143", { SIMKaHao: SIMKaHao }, function (data) {
                        if (data.body != null && data.body == false) {
                            $('#ZhuangTai').val(1).attr('disabled', 'disabled');
                            $('#ShiYongZhuangTai').val(1).attr('disabled', 'disabled');
                        }
                    }, false);
                }
            }

            initPage();
        })
});