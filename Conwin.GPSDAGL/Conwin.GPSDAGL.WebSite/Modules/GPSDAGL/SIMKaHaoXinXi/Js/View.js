define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'tipdialog', 'helper', 'prevNextpage', 'common', 'popdialog', 'formcontrol', 'filelist', 'fileupload'],
        function ($, tipdialog, helper, prevNextpage, common, popdialog, formcontrol, filelist, fileupload) {
            var initPage = function () {
                common.AutoFormScrollHeight('#Form1');
                formcontrol.initial();
                var ids = window.parent.document.getElementById('hdIDS').value;
                prevNextpage.initPageInfo(ids.split(','));
                prevNextpage.bindPageClass();
                $('#btnclose').on('click', function () {
                    popdialog.closeIframe()
                });
                updateData();
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
                $('#Form1').find('.form-control-static').each(function (i, item) {
                    var index = $(item).attr('for');
                    var tempValue = resource[index];
                    if (tempValue != undefined) {
                        if ($(item).hasClass('date-picker')) {
                            tempValue = tempValue.substr(0, 10);
                        }
                        if ($(item).hasClass('decimal')) {
                            tempValue = formatDecimal(tempValue);
                        }
                        if ($(item).hasClass('int')) {
                            tempValue = formatInt(tempValue);
                        }
                        if ($(item).attr('id') =='ZhuangTai') {
                            switch (tempValue) {
                                case 1:
                                    tempValue = '有效';
                                    break;
                                case 2:
                                    tempValue = '无效';
                                    break;
                                default:
                                    tempValue = ''
                                    break;
                            }
                        }
                        if ($(item).attr('id') =='ShiYongZhuangTai') {
                            switch (tempValue) {
                                case 1:
                                    tempValue = '已使用';
                                    break;
                                case 2:
                                    tempValue = '未使用';
                                    break;
                                default:
                                    tempValue = '';
                                    break;
                            }
                        }
                        $(item).html(tempValue)
                    } else {
                        $(item).html('')
                    }
                });
            }

            function formatDecimal(data) {
                return (Math.round(data * 100) / 100).toFixed(2);
            }

            function formatInt(data) {
                return Math.round(data).toString();
            }

            initPage();
        })
});