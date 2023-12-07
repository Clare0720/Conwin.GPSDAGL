/// <reference path="../../../../zhyspt/conwin.zhyspt/conwin.zhyspt.website/scripts/default/json2.js" />
define(['jquery', 'tipdialog', 'popdialog', 'system', 'toast', 'helper'], function ($, tipdialog, popdialog, system, Toastr, helper) {
    var cwLogin = {};
    //登录中状态
    var loging = false;
    var ValidID = helper.NewGuid();
    cwLogin.ShowLogin = function () {
        popdialog.showModal({
            url: '/Modules/Component/login.html',
            showSuccess: function () {
                $('#cbRemberPassword').uniform();
                $.uniform.update();
                initEvent();
                if (!!$.cookie('rember')) {
                    var d = JSON.parse($.cookie('rember'));
                    setRemberMsg(d);
                }
                SetValideCode();
            }
        });
    };
    var initEvent = function () {
        $("#btnLogin").click(function () {
            if (loging) {
                return false;
            }
            var loginID = $.trim($('#loginID').val());
            var userPwd = $.trim($('#userPwd').val());
            var loginValid = $.trim($('#txtLoginValid').val());
            var remeber = $('#cbRemberPassword')[0].checked;
            if (loginID == "") {
                alertMsg('请填写账号', function () {
                    $('#loginID').focus();
                });
            }
            else if (userPwd == "") {
                alertMsg('请填写密码', function () {
                    $('#userPwd').focus();
                });
            }
            else if (loginValid == "") {
                alertMsg('请填写验证码', function () {
                    $('#txtLoginValid').focus();
                });
            } else {
                //setMaxDigits(131);
                //var key = new RSAKeyPair($('#hidExponent').val(), "", $('#hidModulus').val());
                //loginID = encryptedString(key, escape(loginID));
                if ($.trim('userPwd') != "") {
                    //userPwd = encryptedString(key, escape(userPwd));
                }
                //loginValid = encryptedString(key, escape(loginValid));
                var data = { "SysId": system.SysId, "AccountName": loginID, "Password": userPwd, "ValidCode": loginValid };
                helper.Ajax("00000030001", data, function (result) {
                    if (result.publicresponse.statuscode == 0) {
                        helper.AdaptLogin(result.body, function () {
                            var obj = {};
                            obj.account = loginID;
                            obj.password = userPwd;
                            if (remeber) {
                                setRememberCookies(obj);
                            } else {
                                clearRememberCookies();
                                loginkey = null;
                            }
                            helper.SetToken(result.body);
                            helper.UserInfo(function () {
                                Toastr.success('登陆成功');
                                SetUserInfo();
                                popdialog.closeModal();
                            });
                        });
                    }
                    else {
                        alertMsg('登录失败，' + result.message);
                        SetValideCode();
                    }
                }, false);
            }
            return false;
        });
        $('#md_login #loginID, #md_login #userPwd, #md_login #txtLoginValid, #md_login #cbRemberPassword').keyup(function (e) {
            if (e.keyCode == "13") {
                $('#btnLogin').click();
            }
        });
        $('#md_login .lg-close').click(function () {
            popdialog.closeModal();
        });
        $('#imgValideCode').click(function () {
            SetValideCode();
        });
    };
    var setRememberCookies = function (d, u) {
        d = JSON.stringify(d);
        $.cookie('rember', d, { expires: 7 });
    }
    var clearRememberCookies = function () {
        $.cookie('rember', '', { expires: -1 });
    }
    var rememberPassword = function () {
        clearRemberMsg();
        if (!!$.cookie('rember')) {
            var d = JSON.parse($.cookie('rember'));
            setRemberMsg(d);
        }
    }
    var setRemberMsg = function (d) {
        $('#loginID').val(d.account);
        $('#userPwd').val(d.password);
        //$('#cbRemberPassword')[0].checked = true;
        $.uniform.update($("#cbRemberPassword").attr("checked", true));
    }
    var clearRemberMsg = function () {
        $('#loginID').val('');
        $('#userPwd').val('');
        $('#cbRemberPassword')[0].checked = false;
    }
    //信息提示窗
    var alertMsg = function (msg, func) {
        tipdialog.errorDialog(msg);
    }
    var SetValideCode = function () {
        helper.Ajax("00000030011", ValidID, function (result) {
            if (result.publicresponse.statuscode == 0) {
                $('#imgValideCode').attr('src', 'data:image/png;base64,' + result.body);
            }
        }, false);
    }

    var SetUserInfo = function () {
        var UserInfo = helper.GetUserInfo();
        if (!!UserInfo) {
            $('.username').text(UserInfo.UserName);
        }
    }
    return cwLogin;
});
