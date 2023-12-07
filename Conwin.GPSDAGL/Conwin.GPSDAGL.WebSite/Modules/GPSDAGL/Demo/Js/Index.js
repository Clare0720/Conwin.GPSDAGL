define(['/Modules/Config/conwin.main.js'], function () {
   require(['jquery', 'helper', 'tipdialog', 'system', 'toast', 'cokie', 'crypto', 'uniform'], function ($, helper, tipdialog, system, Toastr) {
       //登录中状态
        var loging = false;
        var ValidID = helper.NewGuid();
        $("#btnLogin").click(function () {
            if (loging) {
                return false;
            }
            var loginID = $.trim($('#loginID').val());
            var userPwd = $.trim($('#userPwd').val());
            var loginValid = $.trim($('#txtLoginValid').val());
            var remeber = $('#cbRemberPassword')[0].checked;
            if (loginID == "") {
                alertMsg('请填写账号');
                $('#loginID').focus();
            }
            else if (userPwd == "") {
                alertMsg('请填写密码');
                $('#userPwd').focus();
            }
            else if (loginValid == "") {
                alertMsg('请填写验证码');
                $('#txtLoginValid').focus();
            } else {
                //setMaxDigits(131);
                //var key = new RSAKeyPair($('#hidExponent').val(), "", $('#hidModulus').val());
                //loginID = encryptedString(key, escape(loginID));
                if ($.trim('userPwd') != "") {
                    //userPwd = encryptedString(key, escape(userPwd));
                }
                //loginValid = encryptedString(key, escape(loginValid));
                var data = { "SysId": system.SysId, "AccountName": loginID, "Password": userPwd, "SessionId": ValidID, "Code": loginValid };
                helper.Ajax("00000030001", data, function (result) {
                    if (result.publicresponse.statuscode == 0) {
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
                        Toastr.success('登陆成功');
                        var returnurl = GetQueryString('ReturnUrl');
                        if (returnurl) {
                            window.location.href = returnurl;
                        }
                        else {
                            window.location.href = system.DefaultUrl;
                        }
                    }
                    else {
                        alertMsg('登录失败，' + result.publicresponse.message);
                        SetValideCode();
                    }
                }, false);
            }
        });
        var GetQueryString = function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
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
        var alertMsg = function (msg) {
            tipdialog.errorDialog(msg);
        }
        $("body").keyup(function(e) {
            if (e.keyCode == "13") {
                $('#btnLogin').click();
            }
        });    
        $('#imgValideCode').click(function () {
            SetValideCode();
        })
        var SetValideCode = function () {
            helper.Ajax("00000030011", ValidID, function (result) {
                if (result.publicresponse.statuscode == 0) {
                    $('#imgValideCode').attr('src', 'data:image/png;base64,' + result.body);
                }
            }, false);
        }
        function Init() {
            $('#cbRemberPassword').uniform();
            if (!!$.cookie('rember')) {
                var d = JSON.parse($.cookie('rember'));
                setRemberMsg(d);
            }
            SetValideCode();
        }
        Init();
    })
})
