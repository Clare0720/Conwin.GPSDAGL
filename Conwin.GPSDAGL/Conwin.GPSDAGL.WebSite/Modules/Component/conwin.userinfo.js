define(['jquery', 'tipdialog', 'popdialog', 'system', 'toast', 'helper'], function ($, tipdialog, popdialog, system, Toastr, helper) {
    var userinfo = {};

    userinfo.ShowUserInfo = function () {
        popdialog.showModal({
            url: '/Modules/Component/UserInfo.html',
            showSuccess: function () {
                if (typeof parent.AjaxWindow == 'object' && typeof parent.AjaxWindow.closeIframe =='function')
                {
                    $('#md_UserInfo .close').bind('click', function () { parent.parent.AjaxWindow.closeIframe() });
                    $('#btnCloseUserInfo').bind('click', function () { parent.parent.AjaxWindow.closeIframe() });
                }

                var UserInfo = helper.GetUserInfo();
                var userInfo = GetUserInfo(UserInfo.Id);
                $('#Id').val(UserInfo.Id);
                $('#md_UserInfo form').find('input').each(function (i, item) {
                    var ObjectValue = userInfo[$(item).prop('id')];
                    if (ObjectValue != undefined) {
                        $(item).val(ObjectValue.toString());
                    }
                });

                $('#md_UserInfo form').find('label').each(function (i, item) {
                    var ObjectValue = UserInfo[$(item).prop('for')];
                    if (ObjectValue != undefined) {
                        $(item).text(ObjectValue.toString());
                    }
                });

                var isFirst = true;
                $('#updateUserInfo').toggle(function () {
                    $('#frmUserInfo input').each(function () { $(this).attr("readOnly", false) });
                    $(this).html('<i class="fa fa-save"></i>保存');
                }, function () {
                    if ($(this).text() == '保存') {
                        SubmitUserInfo();
                    }
                    isFirst = false;
                });

                function SubmitUserInfo() {
                    var fromData = {};
                    fromData.Id = $('#Id').val();
                    fromData.UserID = UserInfo.ExtUserId;
                    fromData.UserName = UserInfo.UserName;
                    fromData.Mobile = $('#Mobile').val();
                    fromData.IDCardNo = $('#IDCardNo').val();
                    fromData.Email = $('#Email').val();
                    fromData.Telephone = $('#Telephone').val();
                    fromData.QQ = $('#QQ').val();
                    fromData.Remark = $('#Remark').val();
                    var errorMsg = '';
                    if (!fromData.Mobile) {
                        errorMsg += '请填入手机号码<br />';
                    }
                    else {
                        if (!(/^1\d{10}$/).test(fromData.Mobile)) {
                            errorMsg += '请填入11位有效的手机号码<br />';
                        }
                    }
                    if (!fromData.Email) {
                        errorMsg += '请填入邮箱地址<br />';
                    }
                    else {
                        if (!(/^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/).test(fromData.Email)) {
                            errorMsg += '请填入有效的邮箱地址<br />';
                        }
                    }
                    if (!fromData.QQ) {
                        if (!(/^-?(?:\d+|\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/).test(fromData.QQ)) {
                            errorMsg += '请填入有效的QQ号码<br />';
                        }
                    }
                    if (errorMsg != '') {
                        tipdialog.errorDialog(errorMsg);
                        return;
                    }

                    $.ajax({
                        url: '/api/v1/UserModule/UserManagement',
                        type: 'Put',
                        data: fromData,
                        dataType: 'Json',
                        success: function (xhr) {
                            if (xhr.success) {
                                $('#frmUserInfo input').each(function () { $(this).attr("readOnly", true) });
                                $('#updateUserInfo').html('<i class="fa fa-edit"></i>修改');
                                Toastr.success('个人信息修改成功');
                            }
                            else {
                                tipdialog.errorDialog(xhr.msg);
                                return;
                            }
                        }
                    });
                };

                function GetUserInfo(Id) {
                    var data = '';
                    $.ajax({
                        url: '/api/v1/UserModule/UserManagement',
                        async: false,
                        type: 'Get',
                        data: { id: Id },
                        success: function (xhr) {
                            data = xhr.data;
                        }
                    });
                    return data;
                };



            }
        });
    };
    return userinfo;
});