define(['jquery', 'tipdialog', 'popdialog', 'system', 'toast', 'helper'], function ($, tipdialog, popdialog, system, Toastr, helper) {
    var changepwd = {};
    changepwd.ShowChangePwd = function () {
        popdialog.showModal({
            url: '/Modules/Component/ChangePwd.html',
            width: 'normal',
            showSuccess: function () {

                if (typeof parent.AjaxWindow == 'object' && typeof parent.AjaxWindow.closeIframe == 'function') {
                    $('#md_ChangePwd .close').bind('click', function () { parent.parent.AjaxWindow.closeIframe() });
                    $('#btnClose').bind('click', function () { parent.parent.AjaxWindow.closeIframe() });
                }

                $('#md_ChangePwd').on("click", "#btnSave", function () {
                    var OldPassword = $.trim($('#md_ChangePwd #OldPassword').val());
                    var Password = $.trim($('#md_ChangePwd #NewPassword').val());
                    var ReNewPassword = $.trim($('#md_ChangePwd #ReNewPassword').val());
                    if (OldPassword == "") {
                        tipdialog.errorDialog("旧密码不能为空！");
                        return;
                    }
                    if (Password == "") {
                        tipdialog.errorDialog("新密码不能为空！");
                        return;
                    }
                    if (ReNewPassword == "") {
                        tipdialog.errorDialog("重复新密码不能为空！");
                        return;
                    }
                    if (ReNewPassword != Password) {
                        $('#md_ChangePwd #NewPassword').val('');
                        $('#md_ChangePwd #ReNewPassword').val('');
                        tipdialog.errorDialog("您输入的新密码不一致，请重新输入！");
                        return;
                    }
                    var UserInfo = helper.GetUserInfo();
                    var data = { SysId: system.SysId, AccountName: UserInfo.AccountName, OldPassword: OldPassword, Password: Password, SYS_ZuiJinXiuGaiRenId: UserInfo.AccountId, SYS_ZuiJinXiuGaiRen: UserInfo.AccountName };
                    helper.Ajax("00000010004", data, function (result) {
                        if (result.publicresponse.statuscode == 0) {
                            Toastr.success('密码修改成功');
                            popdialog.closeModal();
                            if (typeof parent.AjaxWindow == 'object' && typeof parent.AjaxWindow.closeIframe == 'function') {
                                parent.parent.AjaxWindow.closeIframe();
                            }

                        }
                        else {
                            if (result.publicresponse.message === '旧密码不正确') {
                                tipdialog.errorDialog('您输入的旧密码不正确，请重新输入！');
                                $('#md_ChangePwd #OldPassword').val('');
                            }
                            else {
                                tipdialog.errorDialog("密码修改失败！错误消息：" + result.publicresponse.message);
                            }
                        }
                    }, false);
                });
            }
        });
    };
    var CheckSubmit = function () {
        var IsSuccess = true;
        $("#md_ChangePwd input").each(function (i, item) {
            if ($.trim($(item).val()) == "") {
                IsSuccess = false;
            }
        });
        if (IsSuccess == true) {
        }
        if (IsSuccess == false) {
            tipdialog.errorDialog("必填项不能为空！");
        }
        return IsSuccess;
    };
    return changepwd;
});