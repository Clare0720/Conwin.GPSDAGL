define(['jquery', 'bootstrap', 'helper', 'tipdialog'], function ($, b, helper, warning) {

    var button = {};

    var defaults = {};
    ///初始化
    button.initial = function (options) {
        var param;
        var message = '';
        var additionMsg = '';
        var focusId = undefined;
        param = $.extend({}, defaults, options);
        ///getButtonByParam(param);
        return button.checkItem(message, additionMsg, focusId, param);
    }


    ///提交驗證
    button.checkItem = function (message, additionMsg, focusId, param) {
        $("#" + param.Id).find('input[name],select[name],textarea[name]').each(function (i, item) {
            if (!Check(item)) {
                if (item.nodeName == 'SELECT') {
                    if ($(item).val() == '') {
                        message += getControlLabel(item).replace('*', '') + getControlDescription(item) + '<br />';
                        $(item).parent('div').addClass('data-box-red');
                    } else {
                        $(item).parent('div').removeClass('data-box-red');
                    }
                } else {
                    if (item.type == 'radio' || item.type == 'checkbox') {
                        var temp = getControlLabelByName(item.name).replace('*', '');

                        if (message.indexOf(temp) == -1) {
                            var ra = $(':' + item.type + ':checked');
                            var len = ra.length;
                            if (len == 0) {
                                message += temp + getControlDescription(item) + '<br />';
                                /// if (helper.BrowserVersion.browser == "IE" && helper.BrowserVersion.version == 8) { }
                                $(item).parent('div').addClass('data-box-red');
                                $(item).parent('div').css({
                                    'float': 'left',
                                    'padding': '3px 12px 3px 8px'
                                });
                                /*else {
                                    $(item).parent('span').parent('div').parent('div').addClass('data-box-red');
                                    $(item).parent('span').parent('div').parent('div').css({
                                        'float': 'left',
                                        'padding': '3px 12px 3px 8px'
                                    });
                                }*/
                            } else {
                                /// if (helper.BrowserVersion.browser == "IE" && helper.BrowserVersion.version == 8) {}
                                $(item).parent('div').removeClass('data-box-red');
                                $(item).parent('div').css('float', '');
                                /* else {
                                    $(item).parent('span').parent('div').parent('div').removeClass('data-box-red');
                                    $(item).parent('span').parent('div').parent('div').css('float', '');
                                }*/
                            }
                        }
                    } else {
                        var temp = getControlLabel(item).replace('*', '');
                        if ($(item).val() != '' && $(item).attr('data-cwvalid') != undefined) {
                            additionMsg += "请输入正确的" + temp + '<br />';
                        } else {
                            $(item).addClass('data-box-red');
                            message += temp + getControlDescription(item) + '<br />';
                        }
                    }
                }
                if (focusId == undefined) {
                    focusId = $(item).attr('id');
                }
            }
        });
        message += additionMsg;
        if (message != '') {
            if (focusId != undefined) {
                warning.errorDialog(message);
            }
            $('#TipModal').on('hidden.bs.modal', function () {
                $('#' + focusId).focus();
            });
            return false;
        } else {
            return true;
        }
    }

    ///檢查必填項
    function Check(item) {
        var vaild = $(item).data("cwvalid");
        if (vaild != null) {
            var inputValue = $.trim($(item).val());
            if (inputValue.length == 0) {
                if (vaild.required == true) {
                    return false;
                }
            }
            else {
                if (vaild.regx != undefined) {
                    var regx = new RegExp(vaild.regx);
                    if (!regx.test(inputValue)) {
                        return false;
                    }
                }

                if (vaild.minlength != undefined && inputValue.length < vaild.minlength) {
                    return false;
                }

                if (vaild.maxlength != undefined && inputValue.length > vaild.maxlength) {
                    return false;
                }
            }
            return true;
        }
        return true;
    };


    function getControlLabel(control) {
        return $('label[for=' + control.id + ']').text();
    }

    function getControlLabelByName(name) {
        return $('label[for=' + name + ']').text();
    }

    function getControlDescription(control) {
        var description = '是必填项';
        if (!(control instanceof jQuery)) {
            control = $(control);
        }
        var elementType = control.prop('nodeName');
        switch (elementType) {

            case 'INPUT':
                inputType = $(control).attr('type');
                description = getInputDescription(inputType);
                break;

            case 'SELECT':
                description = '是必选项';
                break;

            case 'TEXTAREA':
                description = '是必填项';
                break;

            default:
                break;
        }
        return description;
    }

    function getInputDescription(inputType) {
        var description = '';
        switch (inputType) {
            case 'checkbox':
                description = '是必选项';
                break;

            case 'radio':
                description = '是必选项';
                break;

            case 'hidden':
                description = '是必填项';
                break;

            case 'number':
                description = '是必填项';
                break;

            case 'password':
                description = '是必填项';
                break;

            case 'text':
                description = '是必填项';
                break;

            default:
                break;
        }
        return description;
    }

    function getButtonByParam(option) {
        var html = '<label class="control-label col-xs-4 col-sm-2"> &nbsp;' + option.labelName + '</label>' + '<div class="col-xs-8 col-sm-8"> ';
        for (var i = 0; i < option.buttonObject.length; i++) {
            html += '<button id="' + option.buttonObject[i].buttonId + '" class="btn ' + option.buttonObject[i].triggerBtn + '" style="line-height: 0; width:' + option.width + '; height: ' + option.height + '; color:' + option.fontColor + '; font-size: ' + option.fontSize + '; background-color: ' + option.btncolor.one + '"> <i class="fa fa-save"></i>' + option.buttonObject[i].btnValue + '</button>';
        }
        html += '</div>';
        $('.button-init').append(html);
    }

    return button;
});