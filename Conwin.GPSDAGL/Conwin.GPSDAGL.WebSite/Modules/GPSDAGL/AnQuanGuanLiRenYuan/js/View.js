define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'selectCity2', 'metronic', 'fileupload', 'dropdown', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, selectCity2, Metronic, fileupload, dropdown) {
            var userInfo = helper.GetUserInfo();
            var positionTypeArr = [];

            var initPage = function () {
                common.AutoFormScrollHeight('#Form1');
                formcontrol.initial();
                dropdown.initial();
                viewPageButtonInit();
                initCurrentQiYe();
                loadData();
            };

            function viewPageButtonInit() {
                $('#btnclose').click(function () {
                    tipdialog.confirm("确定关闭？", function (r) {
                        if (r) {
                            parent.window.$("#btnSearch").click();
                            popdialog.closeIframe();
                        }
                    });
                });

                $("#backBtn").on('click', function (e) {
                    e.preventDefault();
                    parent.window.$("#btnSearch").click();
                    popdialog.closeIframe();
                });
            }

            function initCurrentQiYe() {
                $('#CompanyName').val(userInfo.OrganizationName);
            }
            function loadData() {
                var id = window.parent.document.getElementById('hdIDS').value;
                helper.Ajax("006600200026", { Id: id }, function (data) {
                    if (data.body) {
                        var model = data.body[0];
                        $("#Name").val(model.Name);

                        var stringValue = "";
                        switch (model.IDCardType) {
                            case 0: stringValue = "居民身份证"; break;
                            case 1: stringValue = "香港特区护照/身份证明"; break;
                            case 2: stringValue = "澳门特区护照/身份证明"; break;
                            case 3: stringValue = "港澳居民居住证"; break;
                            case 4: stringValue = "台湾居民居住证"; break;
                            case 5: stringValue = "台湾居民来往大陆通行证"; break;
                            case 6: stringValue = "外籍人士护照"; break;
                            case 7: stringValue = "军官证"; break;
                            case 99: stringValue = "其它"; break;
                            default:
                        }
                        $("#IDCardType").val(stringValue);

                        switch (model.KaoHeZhuangTai) {
                            case 0: stringValue = "待考核"; break;
                            case 1: stringValue = "已通过"; break;
                            default:
                        }
                        $("#KaoHeZhuangTai").val(stringValue);

                        switch (model.WorkingStatus) {
                            case 2: stringValue = "在职"; break;
                            case 3: stringValue = "离职"; break;
                            default:
                        }
                        $("#WorkingStatus").val(stringValue);

                        $("#IDCard").val(model.IDCard);
                        $("#QuYu").val(model.QuYu);
                        $("#ZhuanYeMingCheng").val(model.ZhuanYeMingCheng);
                        if (model.KaoHeRiQi && model.KaoHeRiQi != "") {
                            $("#KaoHeRiQi").val(model.KaoHeRiQi.substring(0, 10));
                        }
                        $("#KeMuMingCheng").val(model.KeMuMingCheng);
                        $("#DangZuZhiSuoZaiDi").val(model.DangZuZhiSuoZaiDi);
                        $("#LianXiDianHua").val(model.LianXiDianHua);
                        $("#ZhiWu").val(model.ZhiWu);
                   
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message, function () {
                            popdialog.closeIframe();
                        });

                    }
                }, false);
            }

            initPage();
        });


});
