define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'system', 'selectcity', 'filelist', 'fileupload', 'bootstrap-datepicker.zh-CN'],
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
                $('#btnclose').on('click', function () {
                    parent.window.$("#btnSearch").click();
                    popdialog.closeIframe();
                });
                $('#SIMKaHao,#SIMXuLieHao').on('blur', function () {
                    checkSIM($(this));
                });
                $('#KaiKaRiQi').datepicker();
                selectCity();
            };

            function initData() {
                var user = helper.GetUserInfo();
                $('#Id').val(helper.NewGuid());
                $('#KaiKaRiQi').val(helper.GetServerTime().Format('yyyy-MM-dd'));
            }

            function save() {
                var jsonData = $('#Form1').serializeObject();
                jsonData.ShiYongZhuangTai = $('#ShiYongZhuangTai').val();
                jsonData.ZhuangTai = $('#ZhuangTai').val();
                helper.Ajax("003300300132", jsonData, function (data) {
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

            function selectCity() {
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#XiaQuShi').empty().append(defaultOption);
                selectcity.setXiaQu('00000020005', {
                    "Province": "广东"
                }, '#XiaQuShi', function () {
                    $('#XiaQuShi').val('广州');
                });
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
                
                if (SIMKaHao != '') {
                    helper.Ajax("003300300143", { SIMKaHao: SIMKaHao }, function (data) {
                        if (data.body != null && data.body == false) {
                            $('#ZhuangTai').val(1).attr('disabled', 'disabled');
                            $('#ShiYongZhuangTai').val(1).attr('disabled', 'disabled');
                        }
                    }, false);
                }                
            }

            initPage()
        })
});