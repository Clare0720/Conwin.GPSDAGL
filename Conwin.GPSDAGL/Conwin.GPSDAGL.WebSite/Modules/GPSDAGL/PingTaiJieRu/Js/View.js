define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'tipdialog', 'helper', 'prevNextpage', 'common', 'popdialog', 'formcontrol', 'filelist', 'fileupload'],
        function ($, tipdialog, helper, prevNextpage, common, popdialog, formcontrol, filelist, fileupload) {
            var initPage = function () {
                common.AutoFormScrollHeight('#Form1');
                formcontrol.initial();
                var ids = window.parent.document.getElementById('hdIDS').value;
                prevNextpage.initPageInfo(ids.split(','));
                prevNextpage.bindPageClass();
                $('#btnclose').click(function () {
                    popdialog.closeIframe()
                });
                updateData();
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
                $('#Form1').find('.form-control-static').each(function (i, item) {
                    var index = $(item).attr('for');
                    var tempValue = resource[index];
                    if (tempValue != undefined) {
                        if ($(item).hasClass('date-picker')) {
                            tempValue = tempValue.substr(0, 10);
                        }
                        $(item).html(tempValue)
                    } else {
                        $(item).html('')
                    }
                });
                $('#fileId').val(resource.JieRuDengJiBiaoId || '');
                fileupload.rebindFileButtonView(['fileId']);
            }
            initPage();
        })
});