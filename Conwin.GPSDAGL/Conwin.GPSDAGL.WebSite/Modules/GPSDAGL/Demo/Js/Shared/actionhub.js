/// <reference path="queueutility.js" />
$(function () {
    $("select[id=BianGengQingKuang]").change(function () {
        var val = $(this).val();
        if (val == "8") {
            $("#changedatespan").css({ display: "none" });
        } else {
            $("#changedatespan").css({ display: "block" });
        }
    });
});
var hub = function () {
    //
    var currentLoginUserInfo, areaRemoteAddress, fileUploadRemoteAddress, busNumberPrefix, formErrorMsg;
    var areaFlag = true,
    isNumber = function (value) {
        return /^-?(?:\d+|\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/.test(value);
    };
    //正数（不包括0）
    positive = function (value) {
        return /^$|^([1-9][0-9]*(\.\d+)?)$|^0\.\d*[1-9]\d*$/.test(value);
    };
    //非负数
    nonenegative = function (value) {
        return /^[+]{0,1}(\d+)$|^[+]{0,1}(\d+\.\d+)$/.test(value);
    };
    //整数或小数
    custominteger = function (value) {
        return /^[0-9]+([.]{1}[0-9]+){0,1}$/.test(value);
    };
    //正整数（不能以0开头）
    positiveinteger= function (value) {
        return /^[1-9]\d*$/.test(value);
    };
    return {
        //获取表单提交错误信息
        getFormErrorMsg: function () { return formErrorMsg; },
        //获取一个GUID作为记录主键 
        getGuid: function (callbackFn) {
            //
        },
        //获取登录用户信息
        getLoginUserInfo: function (callbackFn) {
            //## 测试 start
            //if (typeof callbackFn == 'function') {
            //    callbackFn(currentLoginUserInfo = { XingZhengJiBie: '市级', XiaQuShi: '广州', XiaQuXian: '海珠' });
            //    return false;
            //}
            //return currentLoginUserInfo = { XingZhengJiBie: '市级', XiaQuShi: '广州', XiaQuXian: '海珠' };
            //## 测试 end
            $("#Id").after('<div class="areas-select-loading"></div>');
            if (currentLoginUserInfo) {
                if (typeof callbackFn == 'function') {
                    callbackFn(currentLoginUserInfo);
                    return true;
                }
                return currentLoginUserInfo;
            }
            $.ajax({
                url: '/api/v1/CL/CheLiang/GetLoginUserInfo',
                type: 'Get',
                dataType: 'JSON',
                success: function (jsonData) {
                    if (jsonData.success) {
                        //
                        jsonData.row.XingZhengJiBie = jsonData.row.AdminLevel;
                        jsonData.row.XiaQuShi = jsonData.row.City;
                        jsonData.row.XiaQuXian = jsonData.row.District;
                        currentLoginUserInfo = jsonData.row;
                        if (typeof callbackFn == 'function') {
                            callbackFn(jsonData.row);
                        }
                    } else {
                        hub.errorDialog(jsonData.msg);
                    }
                    $("#Id").next('.areas-select-loading').remove();
                }
            });
        },
        //根据QueryString参数名称获取值
        getQueryStringByName: function (key) {
            var strExp = "[\?\&]" + key + "=([^\&]+)",
                queryStr = decodeURI(location.search),
                result = queryStr.match(new RegExp(strExp, "i"));
            if (result == null || result.length < 1) {
                return undefined;
            }
            return result[1];
        },
        //获取附件远程地址
        getUploadFileRemoteAddress: function (callbackFn) {
            if (fileUploadRemoteAddress) {
                if (typeof callbackFn == 'function') {
                    callbackFn(fileUploadRemoteAddress);
                    return true;
                }
                return fileUploadRemoteAddress;
            }
            fileUploadRemoteAddress = '';
            if (typeof callbackFn == 'function') {
                callbackFn(fileUploadRemoteAddress);
            }
            //$.ajax({
            //    url: '/api/v1/CL/CheLiang/GetFileServiceUrl',
            //    type: 'Get',
            //    dataType: 'json',
            //    success: function (resultdata) {
            //        fileUploadRemoteAddress = resultdata;
            //        if (typeof callbackFn == 'function') {
            //            callbackFn(resultdata);
            //        }
            //    }
            //});
        },
        //获取辖区远程地址
        getAreaRemoteAddress: function (callbackFn) {
            if (areaRemoteAddress) {
                if (typeof callbackFn == 'function') {
                    callbackFn(areaRemoteAddress);
                    return true;
                }
                return areaRemoteAddress;
            }
            areaRemoteAddress = "";
            if (typeof callbackFn == 'function') {
                callbackFn(areaRemoteAddress);
            }
            //$.ajax({
            //    url: '/api/v1/CL/CheLiang/GetAreaServiceUrl',
            //    type: 'get',
            //    dataType: 'JSON',
            //    success: function (resultdata) {
            //        areaRemoteAddress = resultdata;
            //        if (typeof callbackFn == 'function') {
            //            callbackFn(resultdata);
            //        }
            //    }
            //});
        },
        //获取车牌号前缀
        getBusNumberPrefix: function (callbackFn) {
            if (!currentLoginUserInfo) {
                //无当前用户相关信息
                return false;
            }
            if (currentLoginUserInfo.XingZhengJiBie == '省级') {
                busNumberPrefix = '粤';
                if (typeof callbackFn == 'function') {
                    callbackFn('粤');
                    return true;
                }
                return '粤';
            }
            $.ajax({
                url: encodeURI('/api/v1/CL/CheLiang/GetChePaiQianZhui?xiaQuShi=' + currentLoginUserInfo.XiaQuShi),
                type: 'get',
                dataType: 'json',
                success: function (retrunData) {
                    if (retrunData.success) {
                        busNumberPrefix = retrunData.ChePaiQianZhui;
                        if (typeof callbackFn == 'function') {
                            callbackFn(retrunData.ChePaiQianZhui);
                            return true;
                        }
                        return retrunData.ChePaiQianZhui;
                    }
                }
            })
        },
        //根据枚举值获取名称
        getEnumNameByValue: function (enumName, _v) {
            var targetName = '';
            if (EnumsSet[enumName]) {
                $.each(EnumsSet[enumName], function (i, item) {
                    if (typeof item == 'object') {
                        if (item.value() == _v) {
                            targetName = item.key();
                            return true;
                        }
                    }
                });
            }
            return targetName;
        },
        //获取单条车辆信息
        getSingleCheLiangInfo: function (data, type, callbackFn) {
            var id = "";
            var url = "/api/v1/CL/CheLiang/GetCheLiangById?id=";
            if (type == 'other') {
                type = data.type;
                id = data.id;
                url = data.url + "?id=";
            } else {
                id = data;
            }
            url = url + id + "&type=" + type;
            $.ajax({
                url: url,
                type: 'Get',
                dataType: 'json',
                success: function (resultdata) {
                    if (typeof callbackFn == 'function') {
                        callbackFn(resultdata);
                    }
                }
            });
        },
        //滚动条自适应
        autoFormHeight: function (event) {
            var id = event.data.id, formsectionHeight = 0;
            var parentHeight = $(parent.window).height();
            $('.form-section', id).each(function () {
                formsectionHeight += $(this).height();
            });
            var currWindowHeight = $(window).height();
            currWindowHeight = currWindowHeight > 0 ? currWindowHeight : parentHeight;
            var boxHeight = currWindowHeight - $('.portlet-title').outerHeight(true) - $('.nav-tabs').outerHeight(true) - 65;
            var me = $(".scroller", id).eq(0);
            if (me.length == 1) {
                var h = boxHeight;
                me.parent().css('height', h);
                me.css('height', h);
            } else {
                me = $(".form-body", id);
                me.css('height', boxHeight);
            }
            if (typeof event.data.callbackFn == 'function') {
                var hg = $(window).height();
                if (hg <= 0) hg = parentHeight;
                event.data.callbackFn(hg);
            }
        },
        //tab切换事件
        tabToggleEvent: function (callbackFn) {
            $('.nav-tabs a[data-toggle="tab"]').on('click', function (e) {
                if (typeof callbackFn == 'function')
                    callbackFn(e);
                return true;
            });
        },
        bindAreaDataB: function () {
            var xiaQuSheng, xiaQuShi, xiaQuXian, guanXiaXiaQu;
            xiaQuXian = $.trim($('#XiaQuXian').data('default'));
            if (xiaQuXian != "") {
                $('#XiaQuXian').attr('readonly', 'readonly');
            }
            xiaQuShi = $.trim($('#XiaQuShi').data('default'));
            if (xiaQuShi != "") {
                $('#XiaQuShi').attr('readonly', 'readonly');
            }
            xiaQuSheng = $.trim($('#XiaQuSheng').data('default'));
            if (xiaQuSheng != "") {
                $('#XiaQuSheng').attr('readonly', 'readonly');
            }
            guanXiaXiaQu = $('#hdManageArea').val();
            if (guanXiaXiaQu != "") {
                $('#XiaQuXian').removeAttr('readonly');
                $('#XiaQuXian').removeAttr('readonly');//IE8必须去两次
                if (guanXiaXiaQu.indexOf(xiaQuXian) < 0) {
                    guanXiaXiaQu = xiaQuXian + ',' + guanXiaXiaQu;
                }
            }
            $('select[readonly]').attr('disabled', true);
            bindProvinceDropdownboxCascade(xiaQuSheng, xiaQuShi, xiaQuXian, '', guanXiaXiaQu);
            //switch ($('#hdAdminLevel').val()) {
            //    case "县级":
            //        xiaQuXian = $.trim($('#XiaQuXian').data('default'));
            //        guanXiaXiaQu = $('#hdManageArea').val();
            //        if (guanXiaXiaQu != "") { //目前只适应县级判断
            //            if (guanXiaXiaQu.indexOf(xiaQuXian) < 0) {
            //                guanXiaXiaQu = xiaQuXian + ',' + guanXiaXiaQu;
            //            }
            //        } else {
            //            $('#XiaQuXian').attr('readonly', 'readonly');
            //        }
            //    case "市级":
            //        xiaQuShi = $('#XiaQuShi').data('default');
            //        $('#XiaQuShi').attr('readonly', 'readonly');
            //    case "省级":
            //        xiaQuSheng = $('#XiaQuSheng').data('default');
            //        $('#XiaQuSheng').attr('readonly', 'readonly');
            //    default:
            //        $('select[readonly]').attr('disabled', true);
            //        bindProvinceDropdownboxCascade(xiaQuSheng, xiaQuShi, xiaQuXian, '', guanXiaXiaQu);
            //        break;
            //}
            return true;
        },
        //绑定辖区下拉框值
        //remteAddr为辖区远程地址
        bindAreaData: function () {
            var userInfo = currentLoginUserInfo;
            switch (userInfo.XingZhengJiBie) {
                case '市级':
                    $('#XiaQuShi').attr('disabled', 'disabled');
                    $('#XiaQuShi').val(userInfo.XiaQuShi);
                    $('#XiaQuShi').firecascade(function () {
                        if (queueUtility.length() > 0) {
                            queueUtility.pop();
                        }
                    });
                    break;
                case '县级':
                    if (guanXiaXiaQu != "") {
                        if (guanXiaXiaQu.indexOf(xiaQuXian) < 0) {
                            guanXiaXiaQu = xiaQuXian + ',' + guanXiaXiaQu;
                        }
                    }
                    $('#XiaQuShi').attr('disabled', 'disabled');
                    $('#XiaQuShi').val(userInfo.XiaQuShi);
                    $('#XiaQuShi').firecascade(function () {
                        $('#XiaQuXian').val(userInfo.XiaQuXian);
                        $('#XiaQuXian').attr('disabled', 'disabled');
                        if (queueUtility.length() > 0) {
                            queueUtility.pop();
                        }
                    });
                    break;
                //case '':
                //    $('#XiaQuShi').attr('disabled', 'disabled');
                //    $('#XiaQuShi').val(userInfo.XiaQuShi);
                //    $('#XiaQuXian').val(userInfo.XiaQuXian);
                //    break;
                default:
                    //没有找到此行政级别
                    if (queueUtility.length() > 0) {
                        queueUtility.pop();
                    }
                    break;
            }
            return true;
        },
        //下拉框级联绑定
        bindDropdownboxCascade: function (remoteAddr, tempCallbackFn) {
            $("#XiaQuShi").cascade({
                url: remoteAddr + '/api/v1/BaseDataModule/Area/GetCitys?provinceName',
                valuefield: 'Key',
                callbackfn: tempCallbackFn,
                childrens: [
                    {
                        targetname: 'XiaQuXian',
                        url: remoteAddr + '/api/v1/BaseDataModule/Area/GetDistricts?cityName=',
                        valuefield: 'Key',
                    }
                ]
            });
        },
        //绑定下拉框数据
        //arr: 数组
        bindDropdownboxData: function (arr) {
            var tag = '<option value="">请选择</option>', optionStr = '';
            $(arr).each(function (i, name) {
                optionStr = '';
                $.each(EnumsSet[name], function (i, item) {
                    if (typeof item == 'object')
                        optionStr += '<option value="' + item.value() + '">' + item.key() + '</option>';
                });
                $('#' + name).empty().append(tag).append(optionStr);
            });
        },
        //绑定下拉框数据
        //enumName: 枚举名称，targetName: 控件名称，ignoreVal[]: 忽略选项
        bindSingleDropdownboxData: function (enumName, targetName, ignoreVal) {
            var tag = '<option value="">请选择</option>', optionStr = '';
            if (EnumsSet[enumName]) {
                $.each(EnumsSet[enumName], function (i, item) {
                    if (typeof item == 'object' && $.inArray(item.value(), ignoreVal) < 0) {
                        optionStr += '<option value="' + item.value() + '">' + item.key() + '</option>';
                    }
                });
            }
            $('#' + targetName).empty().append(tag).append(optionStr);
        },
        //绑定组合的下拉框数据
        //enumArr:['enumName',...]
        bindCombinationDropdownData: function (enumArr, targetName) {
            var tag = '<option value="">请选择</option>', optionStr = '';
            $(enumArr).each(function (index, enumName) {
                $.each(EnumsSet[enumName], function (i, item) {
                    if (typeof item == 'object')
                        optionStr += '<option value="' + item.value() + '">' + item.key() + '</option>';
                });
            });
            $('#' + targetName).empty().append(tag).append(optionStr);
        },
        //初始化表单
        bindFormAttr: function (targetName) {
            var targetForm = $('#' + targetName);
            targetForm.validate({
                doNotHideMessage: true, //this option enables to show the error/success messages on tab switch.
                errorElement: 'span', //default input error message container
                errorClass: 'help-block help-block-error', // default input error message class
                focusInvalid: false, // do not focus the last invalid input
                ignore: '',
                rules: {
                    ChePaiHao: {
                        maxlength: 7,
                        minlength: 7
                    },
                    CheChang: {
                        number: true
                    },
                    CheKuan: {
                        number: true
                    },
                    CheGao: {
                        number: true
                    },
                    PaiQiLiang: {
                        number: true
                    },
                    ZongZhiLiang: {
                        number: true
                    },
                    ZuoWei: {
                        number: true
                    },
                    HeDingZaiKeShu: {
                        number: true
                    }
                }
            });
        },
        //公交车辆的表单验证
        checkedForm: function (jsonObj) {
            errorMessage = '';
            if ($('.areas-select-loading').length > 0) {
                hub.errorDialog("正在读取相关数据，暂不能进行操作");
                return false;
            }
            if (!jsonObj.ChePaiHao.replace(/\s/g, ""))
                errorMessage += '车牌号不能为空<br />';
            if (!jsonObj.ChangPai)
                errorMessage += '厂牌不能为空<br />';
            if (!jsonObj.XingHao)
                errorMessage += '型号不能为空<br />';
            if (!jsonObj.CheChang)
                errorMessage += '车长不能为空<br />';
            if (!jsonObj.CheKuan)
                errorMessage += '车宽不能为空<br />';
            if (!jsonObj.CheGao)
                errorMessage += '车高不能为空<br />';
            if (!jsonObj.FaDongJiHao)
                errorMessage += '发动机号不能为空<br />';
            if (!jsonObj.CheJiaHao)
                errorMessage += '车架号不能为空<br />';
            if (!jsonObj.PaiQiLiang)
                errorMessage += '排量不能为空<br />';
            if (!jsonObj.ZongZhiLiang)
                errorMessage += '总质量不能为空<br />';
            if (!jsonObj.ZuoWei)
                errorMessage += '座位数不能为空<br />';
            if (!jsonObj.HeDingZaiKeShu)
                errorMessage += '核定载客数不能为空<br />';
            if (!jsonObj.CheLiangDengJiZhengZhengShuBianHao)
                errorMessage += '登记证编号不能为空<br />';
            if (!jsonObj.XingShiZhengCheLiangLeiXing)
                errorMessage += '行驶证车辆类型不能为空<br />';
            if (!jsonObj.YeHuDaiMa)
                errorMessage += '请选择业户<br />';
            if (!jsonObj.ChePaiYanSe)
                errorMessage += '请选择车牌颜色<br />';
            if (!jsonObj.CheLiangLeiXing)
                errorMessage += '请选择车辆类型<br />';
            if (!jsonObj.PaiFangBiaoZhun)
                errorMessage += '请选择排放标准<br />';
            if (!jsonObj.RanLiaoLeiXing)
                errorMessage += '请选择燃料类型<br />';
            if (!jsonObj.RanLiao && jsonObj.RanLiaoLeiXing != 6)
                errorMessage += '请选择燃料<br />';
            if (!jsonObj.RanLiaoEr && (jsonObj.RanLiaoLeiXing == 5 || (jsonObj.RanLiaoLeiXing == 3 && jsonObj.RanLiao == 7)))
                errorMessage += '请选择燃料二<br />';
            if (!jsonObj.ShiFouKongTiaoChe)
                errorMessage += '请选择是否空调车<br />';
            if (!jsonObj.ShiFouAnZhuangGPS)
                errorMessage += '请选择是否安装GPS<br />';
            if (!jsonObj.ShiFouShuangCengChe)
                errorMessage += '请选择是否双层车<br />';
            if (!jsonObj.ShiFouBRT)
                errorMessage += '请选择是否BRT<br />';
            if (!jsonObj.ShiFouNaRuXinNengYuanGongJiaoTuiGuangMuLu)
                errorMessage += '请选择是否纳入新能源公交推广目录<br />';
            if (!jsonObj.BianGengQingKuang)
                errorMessage += '请选择变更情况<br />';
            if (!jsonObj.BianSuQiXingShi)
                errorMessage += '请选择变速器形式<br />';
            if (jsonObj.BianGengQingKuang != "8") {
                if (!jsonObj.BianGengRiQi)
                    errorMessage += '请选择变更日期<br />';
            }
            if (!jsonObj.CheLiangDengJiZhuCeShiJian)
                errorMessage += '请选择登记证发证日期<br />';
            if (!jsonObj.XingShiZhengDengJiRiQi)
                errorMessage += '请选择行驶证登记日期<br />';
            if (!jsonObj.ShouCiTouRuYunYingShiJian)
                errorMessage += '请选择首次投入营运日期<br />';
            if (!jsonObj.CheLiangZhaoPianWaiJian)
                errorMessage += '请上传车牌照片<br />';
            if (!jsonObj.CheLiangDengJiZhengShuWaiJian)
                errorMessage += '请上传登记证扫描件<br />';
            if (!jsonObj.XingShiZhengSaoMiaoJianWaiJian)
                errorMessage += '请上传行驶证扫描件<br />';
            if (jsonObj.RanLiaoLeiXing == "3" && !jsonObj.XinNengYuanCheGouZhiFaPiao)
                errorMessage += '请上传新能源购置发票扫描件<br />';
            //验证格式
            if (jsonObj.ChePaiHao.length != 7 && jsonObj.ChePaiHao.length != 8)
                errorMessage += '车牌号长度不正确，长度应为7或8位<br />';
            //else {
            //    if (currentLoginUserInfo) {
            //        if (currentLoginUserInfo.XingZhengJiBie == '省级') {
            //            var chePaiQianZhuiFlag = jsonObj.ChePaiHao.replace(/\s/g, "").substring(0, 1);
            //            if (chePaiQianZhuiFlag != busNumberPrefix)
            //                errorMessage += '车牌号前缀不正确，应以' + busNumberPrefix + '开头';
            //        }
            //        if (currentLoginUserInfo.XingZhengJiBie == '市级') {
            //            var chePaiQianZhuiFlag = jsonObj.ChePaiHao.replace(/\s/g, "").substring(0, 2);
            //            if (chePaiQianZhuiFlag != busNumberPrefix)
            //                errorMessage += '车牌号前缀不正确，应以"' + busNumberPrefix + '"开头';
            //        }
            //    }
            //}
            if (jsonObj.CheChang && !positive(jsonObj.CheChang))
                errorMessage += '车长格式不正确<br />';
            if (jsonObj.CheKuan && !positive(jsonObj.CheKuan))
                errorMessage += '车宽格式不正确<br />';
            if (jsonObj.CheGao && !positive(jsonObj.CheGao))
                errorMessage += '车高格式不正确<br />';
            if (jsonObj.FaDongJiGongLv && !positive(jsonObj.FaDongJiGongLv))
                errorMessage += '发动机功率格式不正确<br />';
            if (jsonObj.PaiQiLiang && !nonenegative(jsonObj.PaiQiLiang))
                errorMessage += '排量格式不正确<br />';
            if (jsonObj.ZongZhiLiang && !positive(jsonObj.ZongZhiLiang))
                errorMessage += '总质量格式不正确<br />';
            if (jsonObj.ZuoWei && !positiveinteger(jsonObj.ZuoWei))
                errorMessage += '座位数格式不正确<br />';
            if (jsonObj.HeDingZaiKeShu && !positiveinteger(jsonObj.HeDingZaiKeShu))
                errorMessage += '核定载客数格式不正确<br />';
            var change = parseInt(jsonObj.CheChang);
            var kuan = parseInt(jsonObj.CheKuan);
            var gao = parseInt(jsonObj.CheGao);
            var gonglv = parseInt(jsonObj.FaDongJiGongLv);
            var paiqiliang = parseInt(jsonObj.PaiQiLiang);
            var weigth = parseFloat(jsonObj.ZongZhiLiang);
            if (change < 3000 || change > 18000)
                errorMessage += '车长范围为：3000-18000<br />';
            if (kuan < 1000 || kuan > 3000)
                errorMessage += '车宽范围为：1000-3000<br />';
            if ((gao < 1000 || gao > 5000))
                errorMessage += '车高范围为：1000-5000<br />';
            if (gonglv > 500)
                errorMessage += '发动机功率范围为：0-500<br />';
            if ((paiqiliang < 100 || paiqiliang > 20000) && paiqiliang != 0)
                errorMessage += '排量范围为：100-20000<br />';
            if (weigth < 500 || weigth > 30000)
                errorMessage += '总质量范围为：500-30000<br />';
            //燃料
            if (jsonObj.RanLiao == '插电式混合动力(含增程式)') {
                //
                if (!jsonObj.RanLiaoEr)
                    errorMessage += '请继续选择燃料选项';
            }
           //日期校验
            //sys date yyyy-mm-dd
            var sysdate = new Date().toLocaleDateString();
            //if (jsonObj.CheLiangDengJiZhuCeShiJian && jsonObj.XingShiZhengDengJiRiQi) {
            //    if (jsonObj.CheLiangDengJiZhuCeShiJian > jsonObj.XingShiZhengDengJiRiQi)
            //        errorMessage += "登记证发证日期必须小于或等于行驶证登记日期<br />";
            //}
            //if (jsonObj.ShouCiTouRuYunYingShiJian && jsonObj.XingShiZhengDengJiRiQi) {
            //    if (jsonObj.ShouCiTouRuYunYingShiJian < jsonObj.XingShiZhengDengJiRiQi)
            //        errorMessage += "首次投入营运日期必须大于或等于行驶证登记日期<br />";
            //}
            if (jsonObj.DaoLuYunShuZhengYouXiaoQiXianQiShiRiQi && jsonObj.DaoLuYunShuZhengYouXiaoQiXianZhongZhiRiQi) {
                if (jsonObj.DaoLuYunShuZhengYouXiaoQiXianQiShiRiQi >= jsonObj.DaoLuYunShuZhengYouXiaoQiXianZhongZhiRiQi)
                    errorMessage += "有效日期起必须小于有效日期止<br />";
            }
            formErrorMsg = errorMessage;
            if (errorMessage == '')
                return true;
            return false;
        },
        //农客运车辆的表单验证
        checkedNongKeForm: function (jsonObj) {
            errorMessage = '';
            if ($('.areas-select-loading').length > 0) {
                hub.errorDialog("正在读取相关数据，暂不能进行操作");
                return false;
            }
            if (!jsonObj.ChePaiHao.replace(/\s/g, ""))
                errorMessage += '车牌号不能为空<br />';
            if (!jsonObj.XianLuMingCheng)
                errorMessage += '线路名称不能为空<br />';
            if (!jsonObj.ChangPai)
                errorMessage += '厂牌不能为空<br />';
            if (!jsonObj.XingHao)
                errorMessage += '型号不能为空<br />';
            if (!jsonObj.CheChang)
                errorMessage += '车长不能为空<br />';
            if (!jsonObj.CheKuan)
                errorMessage += '车宽不能为空<br />';
            if (!jsonObj.CheGao)
                errorMessage += '车高不能为空<br />';
            if (!jsonObj.FaDongJiHao)
                errorMessage += '发动机号不能为空<br />';
            if (!jsonObj.CheJiaHao)
                errorMessage += '车架号不能为空<br />';
            if (!jsonObj.PaiQiLiang)
                errorMessage += '排量不能为空<br />';
            if (!jsonObj.ZongZhiLiang)
                errorMessage += '总质量不能为空<br />';
            if (!jsonObj.ZuoWei)
                errorMessage += '座位数不能为空<br />';
            if (!jsonObj.HeDingZaiKeShu)
                errorMessage += '核定载客数不能为空<br />';
            if (!jsonObj.CheLiangDengJiZhengZhengShuBianHao)
                errorMessage += '登记证编号不能为空<br />';
            if (!jsonObj.XingShiZhengCheLiangLeiXing)
                errorMessage += '行驶证车辆类型不能为空<br />';
            if (!jsonObj.YeHuDaiMa)
                errorMessage += '请选择业户<br />';
            if (!jsonObj.ChePaiYanSe)
                errorMessage += '请选择车牌颜色<br />';
            if (!jsonObj.CheLiangLeiXing)
                errorMessage += '请选择车辆类型<br />';
            if (!jsonObj.PaiFangBiaoZhun)
                errorMessage += '请选择排放标准<br />';
            if (!jsonObj.RanLiaoLeiXing)
                errorMessage += '请选择燃料类型<br />';
            if (!jsonObj.RanLiao && jsonObj.RanLiaoLeiXing != 6)
                errorMessage += '请选择燃料<br />';
            if (!jsonObj.RanLiaoEr && (jsonObj.RanLiaoLeiXing == 5 || (jsonObj.RanLiaoLeiXing == 3 && jsonObj.RanLiao == 7)))
                errorMessage += '请选择燃料二<br />';
            if (!jsonObj.ShiFouKongTiaoChe)
                errorMessage += '请选择是否空调车<br />';
            if (!jsonObj.ShiFouAnZhuangGPS)
                errorMessage += '请选择是否安装GPS<br />';
            if (!jsonObj.ShiFouShuangCengChe)
                errorMessage += '请选择是否双层车<br />';
            //if (!jsonObj.ShiFouNaRuXinNengYuanGongJiaoTuiGuangMuLu)
            //    errorMessage += '请选择是否纳入新能源公交推广目录<br />';
            if (!jsonObj.BianGengQingKuang)
                errorMessage += '请选择变更情况<br />';
            if (!jsonObj.BianSuQiXingShi)
                errorMessage += '请选择变速器形式<br />';
            if (jsonObj.BianGengQingKuang != "8") {
                if (!jsonObj.BianGengRiQi)
                    errorMessage += '请选择变更日期<br />';
            }
            if (!jsonObj.CheLiangDengJiZhuCeShiJian)
                errorMessage += '请选择登记证发证日期<br />';
            if (!jsonObj.XingShiZhengDengJiRiQi)
                errorMessage += '请选择行驶证登记日期<br />';
            if (!jsonObj.DaoLuYunShuZhengFaZhengRiQi)
                errorMessage += '请选择初次配发日期<br />';
            if (!jsonObj.ShouCiTouRuYunYingShiJian)
                errorMessage += '请选择首次投入营运日期<br />';
            if (!jsonObj.CheLiangZhaoPianWaiJian)
                errorMessage += '请上传车牌照片<br />';
            if (!jsonObj.CheLiangDengJiZhengShuWaiJian)
                errorMessage += '请上传登记证扫描件<br />';
            if (!jsonObj.XingShiZhengSaoMiaoJianWaiJian)
                errorMessage += '请上传行驶证扫描件<br />';
            if (jsonObj.RanLiaoLeiXing == "3" && !jsonObj.XinNengYuanCheGouZhiFaPiao)
                errorMessage += '请上传新能源购置发票扫描件<br />';
            //验证格式
            if (jsonObj.ChePaiHao.length != 7 && jsonObj.ChePaiHao.length != 8)
                errorMessage += '车牌号长度不正确，长度应为7或8位<br />';
            //else {
            //    if (currentLoginUserInfo) {
            //        if (currentLoginUserInfo.XingZhengJiBie == '省级') {
            //            var chePaiQianZhuiFlag = jsonObj.ChePaiHao.replace(/\s/g, "").substring(0, 1);
            //            if (chePaiQianZhuiFlag != busNumberPrefix)
            //                errorMessage += '车牌号前缀不正确，应以' + busNumberPrefix + '开头';
            //        }
            //        if (currentLoginUserInfo.XingZhengJiBie == '市级') {
            //            var chePaiQianZhuiFlag = jsonObj.ChePaiHao.replace(/\s/g, "").substring(0, 2);
            //            if (chePaiQianZhuiFlag != busNumberPrefix)
            //                errorMessage += '车牌号前缀不正确，应以"' + busNumberPrefix + '"开头';
            //        }
            //    }
            //}
            if (jsonObj.CheChang && !positive(jsonObj.CheChang))
                errorMessage += '车长格式不正确<br />';
            if (jsonObj.CheKuan && !positive(jsonObj.CheKuan))
                errorMessage += '车宽格式不正确<br />';
            if (jsonObj.CheGao && !positive(jsonObj.CheGao))
                errorMessage += '车高格式不正确<br />';
            if (jsonObj.FaDongJiGongLv && !positive(jsonObj.FaDongJiGongLv))
                errorMessage += '发动机功率格式不正确<br />';
            if (jsonObj.PaiQiLiang && !nonenegative(jsonObj.PaiQiLiang))
                errorMessage += '排量格式不正确<br />';
            if (jsonObj.ZongZhiLiang && !positive(jsonObj.ZongZhiLiang))
                errorMessage += '总质量格式不正确<br />';
            if (jsonObj.ZuoWei && !positiveinteger(jsonObj.ZuoWei))
                errorMessage += '座位数格式不正确<br />';
            if (jsonObj.HeDingZaiKeShu && !positiveinteger(jsonObj.HeDingZaiKeShu))
                errorMessage += '核定载客数格式不正确<br />';
            var change = parseInt(jsonObj.CheChang);
            var kuan = parseInt(jsonObj.CheKuan);
            var gao = parseInt(jsonObj.CheGao);
            var gonglv = parseInt(jsonObj.FaDongJiGongLv);
            var paiqiliang = parseInt(jsonObj.PaiQiLiang);
            var weigth = parseFloat(jsonObj.ZongZhiLiang);
            if (change < 3000 || change > 18000)
                errorMessage += '车长范围为：3000-18000<br />';
            if (kuan < 1000 || kuan > 3000)
                errorMessage += '车宽范围为：1000-3000<br />';
            if ((gao < 1000 || gao > 5000))
                errorMessage += '车高范围为：1000-5000<br />';
            if (gonglv > 500)
                errorMessage += '发动机功率范围为：0-500<br />';
            if ((paiqiliang < 100 || paiqiliang > 20000) && paiqiliang != 0)
                errorMessage += '排量范围为：100-20000<br />';
            if (weigth < 500 || weigth > 30000)
                errorMessage += '总质量范围为：500-30000<br />';
            //燃料
            if (jsonObj.RanLiao == '插电式混合动力(含增程式)') {
                //
                if (!jsonObj.RanLiaoEr)
                    errorMessage += '请继续选择燃料选项';
            }
            //日期校验
            //sys date yyyy-mm-dd
            var sysdate = new Date().toLocaleDateString();
            //if (jsonObj.CheLiangDengJiZhuCeShiJian && jsonObj.XingShiZhengDengJiRiQi) {
            //    if (jsonObj.CheLiangDengJiZhuCeShiJian > jsonObj.XingShiZhengDengJiRiQi)
            //        errorMessage += "登记证发证日期必须小于或等于行驶证登记日期<br />";
            //}
            //if (jsonObj.ShouCiTouRuYunYingShiJian && jsonObj.XingShiZhengDengJiRiQi) {
            //    if (jsonObj.ShouCiTouRuYunYingShiJian < jsonObj.XingShiZhengDengJiRiQi)
            //        errorMessage += "首次投入营运日期必须大于或等于行驶证登记日期<br />";
            //}
            if (jsonObj.DaoLuYunShuZhengYouXiaoQiXianQiShiRiQi && jsonObj.DaoLuYunShuZhengYouXiaoQiXianZhongZhiRiQi) {
                if (jsonObj.DaoLuYunShuZhengYouXiaoQiXianQiShiRiQi >= jsonObj.DaoLuYunShuZhengYouXiaoQiXianZhongZhiRiQi)
                    errorMessage += "有效日期起必须小于有效日期止<br />";
            }
            formErrorMsg = errorMessage;
            if (errorMessage == '')
                return true;
            return false;
        },
        //出租车辆的表单验证
        checkedChuZuForm: function (jsonObj) {
            errorMessage = '';
            if ($('.areas-select-loading').length > 0) {
                hub.errorDialog("正在读取相关数据，暂不能进行操作");
                return false;
            }
            if (!jsonObj.ChePaiHao.replace(/\s/g, ""))
                errorMessage += '车牌号不能为空<br />';
            if (!jsonObj.ChangPai)
                errorMessage += '厂牌不能为空<br />';
            if (!jsonObj.XingHao)
                errorMessage += '型号不能为空<br />';
            if (!jsonObj.CheChang)
                errorMessage += '车长不能为空<br />';
            if (!jsonObj.CheKuan)
                errorMessage += '车宽不能为空<br />';
            if (!jsonObj.CheGao)
                errorMessage += '车高不能为空<br />';
            if (!jsonObj.FaDongJiHao)
                errorMessage += '发动机号不能为空<br />';
            if (!jsonObj.CheJiaHao)
                errorMessage += '车架号不能为空<br />';
            if (!jsonObj.PaiQiLiang)
                errorMessage += '排量不能为空<br />';
            if (!jsonObj.ZongZhiLiang)
                errorMessage += '总质量不能为空<br />';
            if (!jsonObj.ZuoWei)
                errorMessage += '座位数不能为空<br />';
            if (!jsonObj.HeDingZaiKeShu)
                errorMessage += '核定载客数不能为空<br />';
            if (!jsonObj.CheLiangDengJiZhengZhengShuBianHao)
                errorMessage += '登记证编号不能为空<br />';
            if (!jsonObj.XingShiZhengCheLiangLeiXing)
                errorMessage += '行驶证车辆类型不能为空<br />';
            if (!jsonObj.YeHuDaiMa)
                errorMessage += '请选择业户<br />';
            if (!jsonObj.ChePaiYanSe)
                errorMessage += '请选择车牌颜色<br />';
            if (!jsonObj.CheLiangLeiXing)
                errorMessage += '请选择车辆类型<br />';
            if (!jsonObj.PaiFangBiaoZhun)
                errorMessage += '请选择排放标准<br />';
            if (!jsonObj.RanLiaoLeiXing)
                errorMessage += '请选择燃料类型<br />';
            if (!jsonObj.RanLiao && jsonObj.RanLiaoLeiXing != 6)
                errorMessage += '请选择燃料<br />';
            if (!jsonObj.RanLiaoEr && (jsonObj.RanLiaoLeiXing == 5 || (jsonObj.RanLiaoLeiXing == 3 && jsonObj.RanLiao == 7)))
                errorMessage += '请选择燃料二<br />';
            if (!jsonObj.ShiFouKongTiaoChe)
                errorMessage += '请选择是否空调车<br />';
            if (!jsonObj.ShiFouAnZhuangGPS)
                errorMessage += '请选择是否安装GPS<br />';
            if (!jsonObj.BianGengQingKuang)
                errorMessage += '请选择变更情况<br />';
            if (!jsonObj.BianSuQiXingShi)
                errorMessage += '请选择变速器形式<br />';
            if (jsonObj.BianGengQingKuang != "8") {
                if (!jsonObj.BianGengRiQi)
                    errorMessage += '请选择变更日期<br />';
            }
           if (!jsonObj.CheLiangDengJiZhuCeShiJian)
                errorMessage += '请选择登记证发证日期<br />';
            if (!jsonObj.XingShiZhengDengJiRiQi)
                errorMessage += '请选择行驶证登记日期<br />';
            if (!jsonObj.ShouCiTouRuYunYingShiJian)
                errorMessage += '请选择首次投入营运日期<br />';
            if (!jsonObj.CheLiangZhaoPianWaiJian)
                errorMessage += '请上传车牌照片<br />';
            if (!jsonObj.CheLiangDengJiZhengShuWaiJian)
                errorMessage += '请上传登记证扫描件<br />';
            if (!jsonObj.XingShiZhengSaoMiaoJianWaiJian)
                errorMessage += '请上传行驶证扫描件<br />';
            if (jsonObj.RanLiaoLeiXing == "3" && !jsonObj.XinNengYuanCheGouZhiFaPiao)
                errorMessage += '请上传新能源购置发票扫描件<br />';
            //验证格式
            if (jsonObj.ChePaiHao.length != 7 && jsonObj.ChePaiHao.length != 8)
                errorMessage += '车牌号长度不正确，长度应为7或8位<br />';
            //else {
            //    if (currentLoginUserInfo) {
            //        if (currentLoginUserInfo.XingZhengJiBie == '省级') {
            //            var chePaiQianZhuiFlag = jsonObj.ChePaiHao.replace(/\s/g, "").substring(0, 1);
            //            if (chePaiQianZhuiFlag != busNumberPrefix)
            //                errorMessage += '车牌号前缀不正确，应以' + busNumberPrefix + '开头';
            //        }
            //        if (currentLoginUserInfo.XingZhengJiBie == '市级') {
            //            var chePaiQianZhuiFlag = jsonObj.ChePaiHao.replace(/\s/g, "").substring(0, 2);
            //            if (chePaiQianZhuiFlag != busNumberPrefix)
            //                errorMessage += '车牌号前缀不正确，应以"' + busNumberPrefix + '"开头';
            //        }
            //    }
            //}
            if (jsonObj.CheChang && !positive(jsonObj.CheChang))
                errorMessage += '车长格式不正确<br />';
            if (jsonObj.CheKuan && !positive(jsonObj.CheKuan))
                errorMessage += '车宽格式不正确<br />';
            if (jsonObj.CheGao && !positive(jsonObj.CheGao))
                errorMessage += '车高格式不正确<br />';
            if (jsonObj.FaDongJiGongLv && !positive(jsonObj.FaDongJiGongLv))
                errorMessage += '发动机功率格式不正确<br />';
            if (jsonObj.PaiQiLiang && !nonenegative(jsonObj.PaiQiLiang))
                errorMessage += '排量格式不正确<br />';
            if (jsonObj.ZongZhiLiang && !positive(jsonObj.ZongZhiLiang))
                errorMessage += '总质量格式不正确<br />';
            if (jsonObj.ZuoWei && !positiveinteger(jsonObj.ZuoWei))
                errorMessage += '座位数格式不正确<br />';
            if (jsonObj.HeDingZaiKeShu && !positiveinteger(jsonObj.HeDingZaiKeShu))
                errorMessage += '核定载客数格式不正确<br />';
            var change = parseInt(jsonObj.CheChang);
            var kuan = parseInt(jsonObj.CheKuan);
            var gao = parseInt(jsonObj.CheGao);
            var gonglv = parseInt(jsonObj.FaDongJiGongLv);
            var paiqiliang = parseInt(jsonObj.PaiQiLiang);
            var weigth = parseFloat(jsonObj.ZongZhiLiang);
            if (change < 3000 || change > 18000)
                errorMessage += '车长范围为：3000-18000<br />';
            if (kuan < 1000 || kuan > 3000)
                errorMessage += '车宽范围为：1000-3000<br />';
            if ((gao < 1000 || gao > 5000))
                errorMessage += '车高范围为：1000-5000<br />';
            if (gonglv > 500)
                errorMessage += '发动机功率范围为：0-500<br />';
            if ((paiqiliang < 100 || paiqiliang > 20000) && paiqiliang != 0)
                errorMessage += '排量范围为：100-20000<br />';
            if (weigth < 500 || weigth > 30000)
                errorMessage += '总质量范围为：500-30000<br />';
           //日期校验
            //sys date yyyy-mm-dd
            var sysdate = new Date().toLocaleDateString();
            //if (jsonObj.CheLiangDengJiZhuCeShiJian && jsonObj.XingShiZhengDengJiRiQi) {
            //    if (jsonObj.CheLiangDengJiZhuCeShiJian > jsonObj.XingShiZhengDengJiRiQi)
            //        errorMessage += "登记证发证日期必须小于或等于行驶证登记日期<br />";
            //}
            //if (jsonObj.ShouCiTouRuYunYingShiJian && jsonObj.XingShiZhengDengJiRiQi) {
            //    if (jsonObj.ShouCiTouRuYunYingShiJian < jsonObj.XingShiZhengDengJiRiQi)
            //        errorMessage += "首次投入营运日期必须大于或等于行驶证登记日期<br />";
            //}
            if (jsonObj.DaoLuYunShuZhengYouXiaoQiXianQiShiRiQi && jsonObj.DaoLuYunShuZhengYouXiaoQiXianZhongZhiRiQi) {
                if (jsonObj.DaoLuYunShuZhengYouXiaoQiXianQiShiRiQi >= jsonObj.DaoLuYunShuZhengYouXiaoQiXianZhongZhiRiQi)
                    errorMessage += "有效日期起必须小于有效日期止<br />";
            }
            formErrorMsg = errorMessage;
            if (errorMessage == '')
                return true;
            return false;
        },
        //新增
        insert: function (jsonData, callbackFn) {
            var url = '/api/v1/CL/CheLiang';
            if (jsonData.url) {
                url = jsonData.url;
            }
            $.ajax({
                url: url,
                type: 'Post',
                data: jsonData,
                dataType: 'JSON',
                success: function (_rst) {
                    if (typeof callbackFn == 'function') {
                        callbackFn(_rst);
                    }
                }
            });
        },
        //更新
        update: function (jsonData, callbackFn) {
            var url = '/api/v1/CL/CheLiang';
            if (jsonData.url) {
                url = jsonData.url;
            }
            $.ajax({
                url: url,
                type: 'Put',
                data: jsonData,
                dataType: 'JSON',
                success: function (_rst) {
                    if (typeof callbackFn == 'function') {
                        callbackFn(_rst);
                    }
                }
            });
        },
        //注销
        deleteCheLiang: function (dataArr, remarkArg, time, businessType, type, callbackFn) {
            $.ajax({
                url: '/api/v1/CL/CheLiang/BatchDeleteCheLiang?remark=' + remarkArg + '&time=' + time + '&businessType=' + businessType + '&type=' + type,
                type: 'Put',
                data: { '': dataArr },
                dataType: 'JSON',
                success: function (_rst) {
                    if (typeof callbackFn == 'function') {
                        callbackFn(_rst);
                    }
                }
            });
        },
        //报停 & 恢复营运 保存
        insertBaoTing: function (jsonData, callbackFn) {
            $.ajax({
                url: '/api/v1/CL/CheLiangBaoTing',
                type: 'Post',
                data: jsonData,
                dataType: 'JSON',
                success: function (_rst) {
                    if (typeof callbackFn == 'function') {
                        callbackFn(_rst);
                    }
                }
            });
        },
        //
        errorDialog: function (msgStr) {
            return bootbox.dialog({
                message: msgStr,
                title: "提示",
                buttons: {
                    Cancel: {
                        label: "关闭",
                        className: "gray"
                    }
                }
            });
        },
        //
        growl: function (msgStr, options) {
            return $.bootstrapGrowl(msgStr, $.extend({
                ele: 'body', // which element to append to
                type: 'success', // (null, 'info', 'danger', 'success', 'warning')
                offset: {
                    from: 'bottom',
                    amount: 10
                }, // 'top', or 'bottom'
                align: 'right', // ('left', 'right', or 'center')
                width: 250, // (integer, or 'auto')
                delay: 5000, // Time while the message will be displayed. It's not equivalent to the *demo* timeOut!
                allow_dismiss: true, // If true then will display a cross to close the popup.
                stackup_spacing: 10 // spacing between consecutively stacked growls.
            }, options));
        },
        //用正则表达式实现html转码
        htmlEncodeByRegExp: function (str) {
            var s = "";
            if (str.length == 0) return "";
            s = str.replace(/&/g, "&amp;");
            s = s.replace(/</g, "&lt;");
            s = s.replace(/>/g, "&gt;");
            s = s.replace(/ /g, "&nbsp;");
            s = s.replace(/\'/g, "&#39;");
            s = s.replace(/\"/g, "&quot;");
            return s;
        },
        //用正则表达式实现html解码
        htmlDecodeByRegExp: function (str) {
            var s = "";
            if (str.length == 0) return "";
            s = str.replace(/&amp;/g, "&");
            s = s.replace(/&lt;/g, "<");
            s = s.replace(/&gt;/g, ">");
            s = s.replace(/&nbsp;/g, " ");
            s = s.replace(/&#39;/g, "\'");
            s = s.replace(/&quot;/g, "\"");
            return s;
        },
        //提示信息
        //msgStr:String
        alertMsg: function (msgStr, callbackFn) {
            var result = bootbox.dialog({
                message: msgStr,
                title: "提示",
                buttons: {
                    Cancel: {
                        label: "关闭",
                        className: "gray",
                        callback: callbackFn
                    }
                }
            });
            $(".modal-backdrop").css("z-index", "10049");
            return result;
        },
        //confirm
        confirm: function (msgStr, callbackFn) {
            return bootbox.dialog({
                message: msgStr,
                title: "提示",
                buttons: {
                    main: {
                        label: "确定",
                        className: "blue",
                        callback: callbackFn
                    },
                    Cancel: {
                        label: "取消",
                        className: "gray"
                    }
                }
            });
        },
        //是否确认
        isConfirmEx: function (msgStr, callbackFn, callbackNo) {
            return bootbox.dialog({
                message: msgStr,
                title: "提示",
                buttons: {
                    main: {
                        label: "确认",
                        className: "blue",
                        callback: callbackFn
                    },
                    Cancel: {
                        label: "取消",
                        className: "default",
                        callback: callbackNo
                    }
                }
            });
        },
        //jQuery ajax helper
        ajax: function (argObj) {
            $.ajax({
                url: argObj.URL,
                type: argObj.TYPE || 'Get',
                data: argObj.DATA,
                dataType: 'JSON',
                beforeSend: function () { Metronic.blockUI({ animate: true }); },
                complete: function () { Metronic.unblockUI(); },
                success: function (xhr) {
                    if (typeof argObj.CALLBACKFN !== 'undefined') argObj.CALLBACKFN(xhr);
                }
            });
        },
        progressBar: function (msg,method) {
            var constData;
            var obj = {};
            //进度条
            if ($("#conwin_basic").html() == undefined) {
                var html = "<div class=\"modal conwinModal\" id=\"conwin_basic\" tabindex=\"-1\" aria-hidden=\"true\" style=\"display: none;top:0;background-color:#ddd;opacity:0.4;\">";
                html += "<div class=\"modal-dialog\" style=\"text-align:center;top:50px;font-size: 17px;font-weight: 600;color:red;\"><div class=\"modal-content\"><div class=\"modal-body\">";
                html += "<img src=\"/Scripts/assets/global/img/loading-spinner-grey.gif\" alt=\"\" class=\"loading\">";
                html += "<span id=\"conwinModalMsg\">&nbsp;&nbsp;正在加载...</span></div></div></div></div>";
                $("body").append(html);
            }
            msg = "&nbsp;&nbsp;" + msg;
            $("#conwinModalMsg").html(msg);
            if (method == "show") {
                $("#conwin_basic").css({ display: "block" });
                $("#conwin_basic").attr("aria-hidden", "false");
            } else if (method == "hide") {
                $("#conwin_basic").css({ display: "none" });
                $("#conwin_basic").attr("aria-hidden", "true");
            }
        }
    };
}();
$(function () {
    //支持跨域访问
    $.support.cors = true;
});
//todo: 序列化为JSON对象
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};
//选择业户回调
var chooseCompanyCallbackFn = function (obj) {
    $('#YeHuDianNaoBianHao').val(obj.data.Id);
    $('#YeHuMingCheng').val(obj.data.YeHuMingCheng);
    $("#YeHuDaiMa").val(obj.data.YeHuDaiMa);
};
//给表单赋值
function SetFormValue(rows, prefix) {
    var control;
    if (prefix == undefined) {
        prefix = "";
    }
    for (var row in rows) {
        if (row.charAt(0) == "$") {
            continue;
        }
        control = $("#" + prefix + row);
        var tagName = "";
        if (control != undefined)
            tagName = $(control).prop("tagName");
        if (tagName)
            tagName = tagName.toLowerCase();
        switch (tagName) {
            case "input":
                var type = $(control).attr("type");
                if (type == "radio") {
                    SetRadioValue(row, rows[row]);
                }
                else if (type == "checkbox") {
                    //var parent = $(control).parent()
                    //$(parent).addClass("checked");
                    //$(control).attr("checked", "checked");
                    $(control).prop('checked', (rows[row] === true || rows[row] === 'true'));
                    $(control).uniform.update();
                }
                else {
                    $(control).val(rows[row]);
                }
                break;
            case "textarea":
                $(control).val(rows[row]);
                break;
            case "select":
                $(control).val(rows[row]);
                break;
            case "label":
                $(control).html(rows[row]);
                break;
            case "":
                //找不到对象
                try { $(control).val(rows[row]); }
                catch (ex) { }
                break;
            default:
                $(control).html(rows[row]);
                break;
        }
    }
}
//取表单数据
function GetFormValue(rows, prefix) {
    if (prefix == undefined) {
        prefix = "";
    }
    if (rows == undefined) {
        var json = "{";
        var inputs = $("input");
        var textarea = $("textarea");
        var selects = $("select");
        var radioName = "";
        for (var i = 0; i < inputs.length; i++) {
            var type = $(inputs[i]).attr("type");
            var value = $(inputs[i]).val();
            if (type == "radio") {
                if (radioName == "") {
                    radioName = $(inputs[i]).attr("name");
                } else {
                    if (radioName == $(inputs[i]).attr("name")) {
                        continue;
                    }
                }
                value = GetRadioValue(radioName);
            }
            var id = $(inputs[i]).attr("id");
            if (id == undefined) {
                id = $(inputs[i]).attr("name");
            }
            json += "\"" + id + "\":\"" + value + "\",";
        }
        for (var j = 0; j < textarea.length; j++) {
            var val = $(textarea[j]).val();
            if (val == undefined) {
                val = "";
            }
            json += "\"" + $(textarea[j]).attr("id") + "\":\"" + val + "\",";
        }
        for (var k = 0; k < selects.length; k++) {
            var selected = $(selects[k]).val();
            if (selected == undefined) {
                selected = "";
            }
            json += "\"" + $(selects[k]).attr("id") + "\":\"" + selected + "\",";
        }
        if (json != "{")
            json = json.substring(0, json.length - 1);
        json += "}";
        rows = JSON.parse(json);
    } else {
        var control;
        for (var row in rows) {
            control = $("#" + prefix + row);
            var tagName = "";
            if (control != undefined)
                tagName = $(control).prop("tagName");
            if (tagName) {
                tagName = tagName.toLowerCase();
            }
            var type = $(control).attr("type");
            switch (tagName) {
                case "input":
                    if (type == "radio") {
                        rows[row] = GetRadioValue(row);
                    } else {
                        var val = $(control).val();
                        if (val == undefined) {
                            val = "";
                        }
                        rows[row] = val;
                    }
                    break;
                case "textarea":
                    var text = $(control).val();
                    if (text == undefined) {
                        text = "";
                    }
                    rows[row] = text;
                    break;
                case "select":
                    var seleted = $(control).val();
                    if (selected == undefined) {
                        selected = "";
                    }
                    rows[row] = selected;
                    break;
                case "":
                    //找不到对象
                    break;
                default:
                    var html = $(control).html();
                    if (html == undefined) {
                        html = "";
                    }
                    rows[row] = html;
                    break;
            }
        }
    }
    return rows;
}
//给radio组赋值
function SetRadioValue(radioName, radioValue, prefix) {
    if (prefix == undefined) {
        prefix = "";
    }
    if (radioName == undefined) {
        return;
    }
    if (radioValue == undefined) {
        return;
    }
    radioName = prefix + radioName;
    var radios = null;
    try {
        radios = $("input[type='radio']");
    } catch (ex) {
    }
    if (radios != null && radios != undefined && radios.length > 0) {
        for (var i = 0; i < radios.length; i++) {
            var name = $(radios[i]).attr("name");
            if (name != radioName) {
                continue;
            }
            if ($(radios[i]).attr("value") == radioValue) {
                var parent = $(radios[i]).parent()
                $(parent).addClass("checked");
                $(radios[i]).attr("checked", "checked");
            } else {
                $(radios[i]).removeAttr("checked");;
            }
        }
    }
}
//取radio组的值
function GetRadioValue(radioName, prefix) {
    if (prefix == undefined) {
        prefix = "";
    }
    if (radioName == undefined) {
        return;
    }
    radioName = prefix + radioName;
    var radios = null;
    var value = undefined;
    try {
        radios = $("input[type='radio']");
    } catch (ex) {
    }
    if (radios != null && radios != undefined && radios.length > 0) {
        for (var i = 0; i < radios.length; i++) {
            var name = $(radios[i]).attr("name");
            if (name != radioName) {
                continue;
            }
            if ($(radios[i]).attr("checked") == "checked") {
                value = $(radios[i]).attr("value");
                if (value == undefined) {
                    value = "";
                }
            }
        }
    }
    return value;
}
function GetControlValue(control) {
    var value = "";
    var tagName = "";
    if (control == undefined) {
        return "";
    }
    var name = $(control).attr("name");
    if (name == undefined) {
        name = "";
    }
    try {
        tagName = $(control).prop("tagName");
        if (tagName != "" && tagName != undefined)
            tagName = tagName.toLowerCase();
        var type = $(control).attr("type");
        if (type == undefined) {
            type = "";
        }
        switch (tagName) {
            case "input":
                if (type == "radio") {
                    value = GetRadioValue(name);
                } else {
                    value = $(control).val();
                }
                break;
            case "textarea":
                value = $(control).val();
                break;
            case "select":
                value = $(control).val();
                break;
            case "":
                //找不到对象
                value = $(control).html();
                break;
            default:
                value = $(control).html();
                break;
        }
    } catch (e) {
    }
    return value;
}
function SetControlValue(control, value, id) {
    var tagName = "";
    if (control == undefined) {
        return "";
    }
    try {
        tagName = $(control).prop("tagName");
        if (tagName != "" && tagName != undefined)
            tagName = tagName.toLowerCase();
        var type = $(control).attr("type");
        if (type == undefined) {
            type = "";
        }
        switch (tagName) {
            case "input":
                if (type == "radio") {
                    SetRadioValue(id, value);
                } else {
                    $(control).val(value);
                }
                break;
            case "textarea":
                $(control).val(value);
                break;
            case "select":
                $(control).val(value);
                break;
            case "":
                //找不到对象
                $(control).html(value);
                break;
            default:
                $(control).html(value);
                break;
        }
    } catch (e) {
    }
}
//区域选择级联下拉框
function AreasSelect(options) {
    var that = this;
    that.options = options || {};
    that.options.textfield = that.options.textfield || 'Key';
    that.options.valuefield = that.options.valuefield || 'Value';
    if (!options.id) {
        throw new Error("AreasSelect: 参数options没有id");
    }
    var o = that.options;
    var $c = $(o.id);
    that.state = 1;
    that.load = function (val, url) {
        //目前支持逗号隔开的字符串
        if ($.trim(o.data) != "") {
            var arr = options.data.split(/\||、|，|:|,/);
            var optionStr = '<option value="">请选择</option>';
            $(arr).each(function (i, item) {
                var selected = "";
                if (item == val) {
                    selected = "selected";
                }
                optionStr += '<option value="' + item + '" ' + selected + '>' + item + '</option>';
            });
            $c.empty().append(optionStr);
        } else {
            var tempurl = url || o.url;
            $.ajax({
                url: encodeURI(tempurl),
                type: 'Get',
                dataType: 'JSON',
                async: false,
                beforeSend: function () {
                    if (!$c.attr('disabled')) {
                        that.state = 0;
                    }
                    $c.after('<div class="areas-select-loading"></div>');
                },
                success: function (jsonArray) {
                    var optionStr = '<option value="">请选择</option>';
                    $(jsonArray).each(function (i, item) {
                        var selected = "";
                        if (item[o.textfield] == val) {
                            selected = "selected";
                        }
                        optionStr += '<option value="' + item[o.valuefield] + '" ' + selected + '>' + item[o.textfield] + '</option>';
                    });
                    $c.empty().append(optionStr);
                },
                complete: function () {
                    if (that.state == 0 && !!$c.attr('disabled')) {
                        that.state = 1;
                    }
                    $c.next('.areas-select-loading').remove();
                }
            });
        }
    };
    that.reload = function (url) {
        $c.empty();
        that.load($c.val(), url);
        that.change();
    };
    that.change = o.change || function () {
    };
    $c.bind('change', that.change);
}
function bindProvinceDropdownboxCascade(province, city, district, town,managearea) {
    //var xiaqusheng = new AreasSelect({
    //    id: "#XiaQuSheng",
    //    valuefield: "Key",
    //    url: '/api/v1/BaseDataModule/Area/GetProvince',
    //    change: function () {
    //        xiaqushi.reload('/api/v1/BaseDataModule/Area/GetCitys?provinceName=' + $('#XiaQuSheng option:selected').text());
    //    }
    //});
    //xiaqusheng.load(province);
    var xiaqushi = new AreasSelect({
        id: "#XiaQuShi", valuefield: "Key",
        change: function () {
            xiaquxian.reload('/api/v1/BaseDataModule/Area/GetDistricts?cityName=' + $('#XiaQuShi option:selected').text());
        }
    });
    xiaqushi.load(city, '/api/v1/BaseDataModule/Area/GetCitys?provinceName=' + province);
    var xiaquxian = new AreasSelect({
        id: "#XiaQuXian", valuefield: "Key", data: managearea,
        //change: function () {
        //    xiaquzhen.reload('/api/v1/BaseDataModule/Area/GetTowns?districtName=' + $('#XiaQuXian option:selected').text());
        //}
    });
    xiaquxian.load(district, '/api/v1/BaseDataModule/Area/GetDistricts?cityName=' + city);
}