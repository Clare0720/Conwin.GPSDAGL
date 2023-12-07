define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'system', 'selectcity', 'filelist', 'fileupload'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, system, selectcity, filelist, fileupload) {
            var initPage = function () {
                common.AutoFormScrollHeight('#Form1');
                formcontrol.initial();
                var ids = window.parent.document.getElementById('hdIDS').value;
                prevNextpage.initPageInfo(ids.split(','));
                prevNextpage.bindPageClass();
                $('#btnclose').click(function () {
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
                $('#file').fileupload({
                    businessId: $('#Id').val(),
                    multi: false,
                    timeOut: 20000
                });
                selectCity();
            }

            function updateData() {
                var id = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                getXianLuXinXi(id, function (serviceData) {
                    if (serviceData.publicresponse.statuscode == 0) {
                        fillFormData(serviceData.body)
                    } else {
                        tipdialog.errorDialog("请求数据失败")
                    }
                })
            }

            function getXianLuXinXi(id, callback) {
                helper.Ajax("003300300119", id, function (resultdata) {
                    if (typeof callback == 'function') {
                        callback(resultdata)
                    }
                }, false)
            }

            function fillFormData(resource) {
                $('#Form1').find('input[name],select[name],textarea[name]').each(function (i, item) {
                    var tempValue = resource[$(item).attr('name')];
                    if (tempValue != undefined) {
                        if ($(item).hasClass('date-picker')) {
                            tempValue = tempValue.substr(0, 10);
                        }
                        $(item).val(tempValue)
                    } else {
                        $(item).val('')
                    }
                });
                getSubOrgList(function () {
                    $('#JieRuDaiMa').val(resource.JieRuDaiMa);
                });
                if (resource.JieRuDengJiBiaoId) {
                    $('#file').remove();
                    fileupload.rebindFileButtonEdit(['fileId']);
                }               
                xiaQuXinXi(resource)
            }

            function save() {
                var jsonData = $('#Form1').serializeObject();
                check(jsonData, function () {
                    helper.Ajax("003300300117", jsonData, function (data) {
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
                });               
            }

            function check(jsonData, callback) {
                helper.Ajax("003300300120", jsonData, function (data) {
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body.result === false) {
                            tipdialog.alertMsg(data.body.msg);
                        } else {
                            callback && callback();
                        }
                    } else {
                        tipdialog.alertMsg("校验失败");
                    }
                }, false);
            }

            function selectCity() {
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#XiaQuShi, #XiaQuXian').empty().append(defaultOption);
                selectcity.setXiaQu('00000020005', {
                    "Province": "广东"
                }, '#XiaQuShi', 'GetCityList', 'CityName');
            }

            function xiaQuXinXi(resource) {
                selectcity.setXiaQu('00000020005', {
                    "Province": "广东"
                }, '#XiaQuShi', 'GetCityList', 'Key', 'Key', resource.XiaQuShi);
            }

            function getSubOrgList(callback) {
                helper.Ajax("003300300124", null, function (data) {
                    if (data.publicresponse.statuscode == 0) {
                        var defaultOption = '<option value="" selected="selected">请选择</option>';
                        $(data.body).each(function (i, item) {
                            defaultOption += '<option value="' + item.OrgCode + '">' + item.OrgName + '</option>';
                        });
                        $('#JieRuDaiMa').empty().append(defaultOption);
                        callback && callback();
                    }
                }, false);
            }
            initPage();
        })
});