define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'system', 'selectcity', 'filelist', 'fileupload'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, system, selectcity, filelist, fileupload) {
            var initPage = function () {
                common.AutoFormScrollHeight('#Form1');
                formcontrol.initial();
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
                setTimeout(function () {
                    if ($("#ZhaoPianId").val() == "") {
                        if ($("#InitZhaoPian").val() == "") {
                            $("#ZhaoPianYuLan").attr("src", "../../Component/Img/NotPic.jpg");
                        } else {
                            var filePath = system.GetFilePath + "?id=" + $("#InitZhaoPian").val();
                            $("#ZhaoPianYuLan").attr("src", filePath);
                        }
                    } else {
                        var href = $("#ZhaoPianShangChuan a:first").attr('href');
                        $("#ZhaoPianYuLan").attr("src", href);
                        //getBase64(href)
                        //    .then(function (base64) {
                        //        $("#ZhaoPianYuLan").attr("src", base64);
                        //    }, function (err) {
                        //        console.log(err);
                        //    });
                    }
                    setTimeout(arguments.callee, 1000);
                }, 1000);
                $('.fa-upload').each(function (index, item) {
                    $('#' + $(item).parent()[0].id).fileupload({
                        multi: false,
                        timeOut: 20000,
                        maxSize: '3',//3M
                        allowedContentType: 'png|jpg|jpeg'
                    });
                });
                var ids = window.parent.document.getElementById('hdIDS').value;
                prevNextpage.initPageInfo(ids.split(','));
                prevNextpage.bindPageClass();
                updateData();
                $('#btnclose').click(function () {
                    parent.window.$("#btnSearch").click();
                    popdialog.closeIframe();
                });
                $('#saveBtn').on('click', function (e) {
                    e.preventDefault();
                    save();
                });
            }

            function updateData() {
                var id = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                helper.Ajax("003300300160", id, function (data) {
                    if (data.body) {
                        var model = data.body;
                        $("#CongYeRenYuanId").val(model.CongYeRenYuanId);
                        $("#CongYeRenYuanDianNaoBianHao").val(model.CongYeRenYuanDianNaoBianHao);
                        $("#XingMing").val(model.XingMing);
                        $("#XingBie").val(model.XingBie);
                        $("#CongYeZiGeZhengHao").val(model.CongYeZiGeZhengHao);
                        $("#ShenFenZhengHaoMa").val(model.ShenFenZhengHaoMa);
                        $("#YiDongDianHua").val(model.YiDongDianHua);
                        if (model.ZhaoPian && model.ZhaoPian != "") {    
                            $("#InitZhaoPian").val(model.ZhaoPian);
                        }
                        //$("#ZhaoPianYuLan").attr("src", model.ZhaoPianBase64);
                        //$("#InitZhaoPian").val(model.ZhaoPianBase64);
                        $("#JSYXiaQuSheng").val(model.XiaQuSheng);
                        $("#JSYXiaQuSheng").change();
                        // 为了防止异步导致辖区县加载不出，因此用setTimeout延迟1s加载
                        setTimeout(function () {
                            $("#JSYXiaQuShi").val(model.XiaQuShi);
                            $("#JSYXiaQuShi").change();
                            setTimeout(function () {
                                $("#JSYXiaQuXian").val(model.XiaQuXian);
                            }, 1000);
                        }, 800);
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                        popdialog.closeModal();
                    }
                }, false);
            }


            function save() {
                var param = {}
                var errorMsg = "";
                param.Id = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
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
                    var initZhaoPian = $("#InitZhaoPian").val();
                    if (initZhaoPian && initZhaoPian != "") {
                        param.ZhaoPian = $("#InitZhaoPian").val();
                    } else {
                        errorMsg += "驾驶员图片必须上传</br>"
                    }
                    
                }

                //param.ZhaoPianBase64 = $("#ZhaoPianYuLan").attr("src");
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
                if (param.XiaQuShi.trim() == "") {
                    errorMsg += "辖区市为必填项</br>"
                }
                if (param.XiaQuSheng.trim() == "") {
                    errorMsg += "辖区省为必填项</br>"
                }
                if (errorMsg != "") {
                    tipdialog.alertMsg(errorMsg);
                    return;
                }
                // 修改驾驶员信息
                helper.Ajax("003300300161", param, function (data) {
                    if (data.body) {
                        parent.window.require(['toast'], function (Toastr) {
                            Toastr.success("保存成功");
                        });
                        parent.window.$("#btnSearch").click();
                        popdialog.closeIframe();
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);               
            }
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
                function getBase64Image(img, width, height) {//width、height调用时传入具体像素值，控制大小  ,不传则默认图像大小
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
        })
});