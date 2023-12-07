define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'system', 'selectcity', 'filelist', 'fileupload'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, system, selectcity, filelist, fileupload) {
            var initPage = function () {
                common.AutoFormScrollHeight('#Form1');
                formcontrol.initial();
                initData();
                $('#saveBtn').on('click', function (e) {
                    e.preventDefault();
                    var flags = formcontrol.buttonValid();
                    if (flags) {
                        save()
                    }
                });
                $('#btnclose').click(function () {
                    parent.window.$("#btnSearch").click();
                    popdialog.closeIframe();
                });
                $('#file').fileupload({
                    businessId: $('#Id').val(),
                    multi: false,
                    timeOut: 20000
                });
                selectCity();                
            };

            function initData() {
                var user = helper.GetUserInfo();
                $('#Id').val(helper.NewGuid());
                $('#JieRuShiJian').val(helper.GetServerTime().Format('yyyy-MM-dd'));
                getSubOrgList();
            }

            function save() {
                var jsonData = $('#Form1').serializeObject();
                check(jsonData, function () {
                    helper.Ajax("003300300116", jsonData, function (data) {
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

            function selectCity() {
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#XiaQuShi, #XiaQuXian').empty().append(defaultOption);
                selectcity.setXiaQu('00000020005', {
                    "Province": "广东"
                }, '#XiaQuShi', 'GetCityList', 'CityName');
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

            function getSubOrgList(callback) {
                helper.Ajax("003300300124", null, function (data) {
                    if (data.publicresponse.statuscode == 0) {
                        var defaultOption = '<option value="" selected="selected">请选择</option>';
                        $(data.body).each(function (i, item) {
                            defaultOption += '<option value="' + item.OrgCode + '">' + item.OrgName +'</option>';
                        });
                        $('#JieRuDaiMa').empty().append(defaultOption);
                        callback && callback();
                    }
                }, false);
            }

            initPage()
        })
});