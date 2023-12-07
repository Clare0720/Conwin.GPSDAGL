define(['/Modules/Config/conwin.main.js', '/Modules/GPSDAGL/CheLiangDangAn/Config/config.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'fileupload', 'btn', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, fileupload, btn) {

            var UserInfo = helper.GetUserInfo(); //用户信息
            var id = window.parent.document.getElementById('hdIDS').value;

            var initPage = function () {

                //关闭
                $('#btnclose').click(function () {
                    popdialog.closeIframe();
                });

                var para = {
                    "Id": id,
                    "Type": "GPS"
                };
                helper.Ajax("003300300532", para, function (data) {
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {
                            data.body.DeviceInstallCertDate = data.body.DeviceInstallCertDate ? data.body.DeviceInstallCertDate.substring(0, 10) : "";
                            data.body.DeviceInstallCertExpired = data.body.DeviceInstallCertExpired ? data.body.DeviceInstallCertExpired.substring(0, 10) : "";
                            fillFormData(data.body, "Form1");
                            fillFormData(data.body, "Form2");
                            if (data.body.CameraSelected) {
                                fillCan(data.body.CameraSelected);
                            }
                        }
                    }
                }, false);

            };

            function fillCan(canStr) {
                var arr = canStr.split(",");
                $.each(arr, function (i, item) {
                    if (item != '') {
                        var lemp = item.split("|");
                        $("input[name='CameraSelected'][val=" + lemp[0] + "]").attr("checked", "checked");
                        $("input[name='CameraSelected'][val=" + lemp[0] + "]").parent().attr("class", "checked");
                        $(".CameraNameData[val=" + lemp[0] + "]").text(lemp[1]);
                    }

                });
            }


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

            };


            initPage();

        });
});