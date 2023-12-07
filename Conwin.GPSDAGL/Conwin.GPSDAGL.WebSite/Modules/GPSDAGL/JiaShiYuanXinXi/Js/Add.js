define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'selectCity2', 'filelist', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, selectCity2, filelist, Metronic, fileupload) {
            var userInfo = helper.GetUserInfo();
            var filelist = [];
            var initPage = function () {
                common.AutoFormScrollHeight('#Form1');
                formcontrol.initial();
                $('#btnclose').click(function () {
                    tipdialog.confirm("确定关闭？", function (r) {
                        if (r) {
                            parent.window.$("#btnSearch").click();
                            popdialog.closeIframe();
                        }
                    });
                });
                setTimeout(function () {
                    if ($("#ZhaoPianId").val() == "") {
                        if ($("#InitZhaoPian").val() == "") {
                            $("#ZhaoPianYuLan").attr("src", "../../Component/Img/NotPic.jpg");
                        } else {
                            $("#ZhaoPianYuLan").attr("src", $("#InitZhaoPian").val());
                        }
                    } else {
                        var href = $("#ZhaoPianShangChuan a:first").attr('href');
                        $("#ZhaoPianYuLan").attr("src", href);
                    }
                    setTimeout(arguments.callee, 1000);
                }, 1000);
                $("#ShenFenZhengHaoMa").on("blur", function () {
                    helper.Ajax("003300300155", $("#ShenFenZhengHaoMa").val(), function (data) {
                        if (data.body != null) {
                            if (data.body.ZhaoPianBase64 != "") {
                                $("#InitZhaoPian").val(data.body.ZhaoPianBase64);
                                $("#ZhaoPianYuLan").attr("src", $("#InitZhaoPian").val());
                            }
                            $("#XingMing").val(data.body.XingMing);
                            $("#XingBie").val(data.body.XingBie);
                            $("#CongYeZiGeZhengHao").val(data.body.CongYeZiGeZhengHao);
                            $("#YiDongDianHua").val(data.body.YiDongDianHua);
                            $("#CongYeRenYuanId").val(data.body.CongYeRenYuanId);
                            $("#CongYeRenYuanDianNaoBianHao").val(data.body.CongYeRenYuanDianNaoBianHao);
                            $("#JSYXiaQuSheng").val(data.body.XiaQuSheng);
                            $("#JSYXiaQuSheng").change();
                            setTimeout(function () {
                                $("#JSYXiaQuShi").val(data.body.XiaQuShi);
                                $("#JSYXiaQuShi").change();
                                setTimeout(function () {
                                    $("#JSYXiaQuXian").val(data.body.XiaQuXian);
                                }, 1000);
                            }, 800);
                        } 
                    }, false);
                });
                $('.fa-upload').each(function (index, item) {
                    $('#' + $(item).parent()[0].id).fileupload({
                        multi: false,
                        timeOut: 20000,
                        maxSize: '3',//3M
                        allowedContentType: 'png|jpg|jpeg',
                        //callback: ReloadPreNextImg
                    });
                });

                // 省市县
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#JSYXiaQuSheng, #JSYXiaQuShi, #JSYXiaQuXian').empty().append(defaultOption);
                selectcity.setXiaQu('00000020004', {}, '#JSYXiaQuSheng', 'GetProvinceList', 'ProvinceName');
                $('#JSYXiaQuSheng').change(function () {
                    $('#JSYXiaQuShi').empty().append(defaultOption);
                    $('#JSYXiaQuXian').empty().append(defaultOption);
                    var data = { "Province": $(this).val() };
                    if ($(this).val() != '') {
                        selectcity.setXiaQu('00000020005', data, '#JSYXiaQuShi', 'GetCityList', 'CityName');
                    }
                });
                $('#JSYXiaQuShi').change(function () {
                    $('#JSYXiaQuXian').empty().append(defaultOption);
                    var data = { "City": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化区县下拉框
                        selectcity.setXiaQu('00000020006', data, '#JSYXiaQuXian', 'GetDistrictList', 'DistrictName');
                    }
                });
                $('#JSYXiaQuXian').change(function () {
                    var data = { "District": $(this).val() };
                });

                $("#saveBtn").on('click', function (e) {
                    e.preventDefault();
                    var param = {}
                    var errorMsg = "";
                    param.BenDanWeiOrgCode = helper.GetUserInfo().OrganizationCode;
                    param.CongYeRenYuanId = $("#CongYeRenYuanId").val();
                    param.CongYeRenYuanDianNaoBianHao = $("#CongYeRenYuanDianNaoBianHao").val();
                    param.XingMing = $("#XingMing").val();
                    param.XingBie = $("#XingBie").val();
                    param.CongYeZiGeZhengHao = $("#CongYeZiGeZhengHao").val();
                    param.ShenFenZhengHaoMa = $("#ShenFenZhengHaoMa").val();
                    param.YiDongDianHua = $("#YiDongDianHua").val();
                    param.XiaQuSheng = $("#JSYXiaQuSheng").val();
                    param.XiaQuShi = $("#JSYXiaQuShi").val();
                    param.XiaQuXian = $("#JSYXiaQuXian").val();
                    param.ZhaoPian = $("#ZhaoPianId").val();
                    if (!param.ZhaoPian || param.ZhaoPian == "") {
                        if ($("#ZhaoPianYuLan").attr("src") != "../../Component/Img/NotPic.jpg") {
                            param.ZhaoPianBase64 = $("#ZhaoPianYuLan").attr("src");
                        } else {
                            errorMsg += "驾驶员图片必须上传</br>"
                        }

                    }
                    // 校验数据
                    if (param.XingMing.trim() == "") {
                        errorMsg += "姓名为必填项</br>"
                    }
                    if (param.XingBie.trim() == "") {
                        errorMsg += "性别为必填项</br>"
                    }
                    if (param.CongYeZiGeZhengHao.trim() == "") {
                        errorMsg += "从业资格证号为必填项</br>"
                    }
                    if (param.ShenFenZhengHaoMa.trim() == "") {
                        errorMsg += "身份证号码为必填项</br>"
                    } else if (ValidIdentityCardNumber(param.ShenFenZhengHaoMa) == false) {
                        errorMsg += '身份证号码格式不正确</br>';
                    }
                    if (param.YiDongDianHua.trim() == "") {
                        errorMsg += "移动电话为必填项</br>"
                    }
                    if (param.XiaQuSheng.trim() == "") {
                        errorMsg += "辖区省为必填项</br>"
                    }
                    if (param.XiaQuShi.trim() == "") {
                        errorMsg += "辖区市为必填项</br>"
                    }
                    if (errorMsg != "") {
                        tipdialog.alertMsg(errorMsg);
                        return;
                    }
                    // 新增驾驶员信息
                    helper.Ajax("003300300156", param, function (data) {
                        if (data.body) {
                            toast.success("新增成功");
                            parent.window.$("#btnSearch").click();
                            popdialog.closeIframe();
                        }
                        else {
                            tipdialog.alertMsg(data.publicresponse.message);
                        }
                    }, false);
                });

            };
            function ValidIdentityCardNumber(idCard) {

                //idCard = $.trim(idCard.replace(/ /g, ""));              
                idCard = $.trim(idCard);
                if (idCard.length == 15) {
                    /*return isValidityBrithBy15IdCard(idCard) == true ? '' : '身份证格式不正确<br />';*/       //进行15位身份证的验证   
                    if (isValidityBrithBy15IdCard(idCard)) {
                        return true;
                    } else {
                        //tipdialog.alertMsg("身份证号码格式不正确");
                        return false;
                    }
                } else if (idCard.length == 18) {
                    var a_idCard = idCard.split("");                // 得到身份证数组   
                    if (isValidityBrithBy18IdCard(idCard) && isTrueValidateCodeBy18IdCard(a_idCard)) {   //进行18位身份证的基本验证和第18位的验证
                        return true;
                    } else {
                        //tipdialog.alertMsg("身份证号码格式不正确");
                        return false;
                    }
                } else {
                    //tipdialog.alertMsg("身份证号码格式不正确");
                    return false;
                }
            }

            function isValidityBrithBy18IdCard(idCard18) {
                var year = idCard18.substring(6, 10);
                var month = idCard18.substring(10, 12);
                var day = idCard18.substring(12, 14);
                var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
                // 这里用getFullYear()获取年份，避免千年虫问题   
                if (temp_date.getFullYear() != parseFloat(year)
                    || temp_date.getMonth() != parseFloat(month) - 1
                    || temp_date.getDate() != parseFloat(day)) {
                    return false;
                } else {
                    return true;
                }
            }
            function isValidityBrithBy15IdCard(idCard15) {
                var year = idCard15.substring(6, 8);
                var month = idCard15.substring(8, 10);
                var day = idCard15.substring(10, 12);
                var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
                // 对于老身份证中的你年龄则不需考虑千年虫问题而使用getYear()方法   
                if (temp_date.getYear() != parseFloat(year)
                    || temp_date.getMonth() != parseFloat(month) - 1
                    || temp_date.getDate() != parseFloat(day)) {
                    return false;
                } else {
                    return true;
                }
            }
            function isTrueValidateCodeBy18IdCard(a_idCard) {
                var Wi = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1];    // 加权因子   
                var ValideCode = [1, 0, 10, 9, 8, 7, 6, 5, 4, 3, 2];            // 身份证验证位值.10代表X   
                var sum = 0;                             // 声明加权求和变量   
                if (a_idCard[17].toLowerCase() == 'x') {
                    a_idCard[17] = 10;                    // 将最后位为x的验证码替换为10方便后续操作   
                }
                for (var i = 0; i < 17; i++) {
                    sum += Wi[i] * a_idCard[i];            // 加权求和   
                }
                valCodePosition = sum % 11;                // 得到验证码所位置   
                if (a_idCard[17] == ValideCode[valCodePosition]) {
                    return true;
                } else {
                    return false;
                }
            }
            function getBase64(img) {
                function getBase64Image(img, width, height) {//width、height调用时传入具体像素值，控制大小 ,不传则默认图像大小
                    var canvas = document.createElement("canvas");
                    canvas.width = width ? width : img.width;
                    canvas.height = height ? height : img.height;
                    var ctx = canvas.getContext("2d");
                    ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
                    var dataURL = canvas.toDataURL();
                    return dataURL;
                }
                var image = new Image();
                image.crossOrigin = '';
                image.src = img;
                var deferred = $.Deferred();
                if (img) {
                    image.onload = function () {
                        deferred.resolve(getBase64Image(image));//将base64传给done上传处理
                    }
                    return deferred.promise();//问题要让onload完成后再return sessionStorage['imgTest']
                }
            }

            initPage();
        });


});
