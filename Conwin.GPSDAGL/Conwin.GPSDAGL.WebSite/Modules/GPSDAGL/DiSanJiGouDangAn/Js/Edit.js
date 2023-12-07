define(['/Modules/Config/conwin.main.js'], function () {
    require.config({paths: {"actionhub": "/Modules/GPSDAGL/Common/actionhub"}});//引入工具类
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'metronic', 'selectCity2', 'customtable', 'actionhub', 'dropdown', 'fileupload', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, Metronic, selectCity2, customtable, actionhub, dropdown, fileupload) {
            var userInfo = helper.GetUserInfo();
            var XiaQuXian = "";
            var positionTypeArr = [];
            helper.UserInfo(selectCity);
            var initPage = function () {
                common.AutoFormScrollHeight('#Form1');
                common.AutoFormScrollHeight('#LianXiXinXi', function (hg) {
                    var boxHeight = hg - $('.portlet-title').outerHeight(true) - $('.nav-tabs').outerHeight(true) - 245;
                    var me = $(".scroller");
                    me.parent().css('height', boxHeight);
                    me.css('height', boxHeight);
                });

                formcontrol.initial();
                initData();


                //保存
                $('#saveBtn').on('click', function (e) {
                    e.preventDefault();
                    var flags = true;
                    var msg = '';
                    var fromData = $('#Form1').serializeObject();
                    fromData.YouXiaoZhuangTai = $('#YouXiaoZhuangTai').val();
                    fromData.QiYeLeiXing = $('#QiYeLeiXing').val();
                    //调用新增接口
                    if ($.trim(fromData.JiGouMingCheng) == '') {
                        msg += "机构名称 是必填项<br/>";
                    }
                    if ($.trim(fromData.JiGouLeiXing) == '') {
                        msg += "机构类型 是必选项<br/>";
                    }
                    //if ($.trim(fromData.YingYeZhiZhaoHao) == '' && $.trim(fromData.TongYiSheHuiXinYongDaiMa) == '') {
                    //    msg += "营业执照号和统一社会信用代码 必填一个<br/>";
                    //}
                    //if ($.trim(fromData.HeZuoFangShi) == '') {
                    //    msg += "合作方式 是必选项<br/>";
                    //}
                    if ($.trim(fromData.JingYingQuYu) == '') {
                        msg += "经营区域 是必选项<br/>";
                    }

                    if ($.trim(fromData.YouXiaoZhuangTai) == '') {
                        msg += "有效状态 是必填项<br/>";
                    }
                    if ($.trim(fromData.XiaQuShi) == '') {
                        msg += "辖区市 是必填项<br/>";
                    }
                    //if ($.trim(fromData.YouBian) != '') {
                    //    if (!new RegExp('^[1-9][0-9]{5}$').test($.trim(fromData.YouBian))) {
                    //        msg += "邮编 格式不正确<br/>";
                    //    }
                    //}
                    //if ($.trim(fromData.YingYeZhiZhaoFuBenId) == '') {
                    //    msg += "营业执照副本 必须上传<br/>";
                    //}

                    ///===test
                    //save();
                    //return;
                    //===test
                    if (msg != '') {
                        flags = false;
                        tipdialog.alertMsg(msg);
                    }
                    if (flags) {
                        save();
                    }
                });

                //关闭
                $('#btnclose').click(function () {
                    tipdialog.confirm("确定关闭？", function (r) {
                        if (r) {
                            parent.window.$("#btnSearch").click();
                            popdialog.closeIframe();
                        }
                    });
                });
                //tab2
                $('#tab2').click(function (e) {
                    $("#tb_lianXiRenTable").CustomTable("reload");
                });


                ////初始化附件上传按钮
                //$('.fa-upload').each(function (index, item) {
                //    $('#' + $(item).parent()[0].id).fileupload({
                //        multi: false,
                //        timeOut: 20000,
                //        allowedContentType: 'png|jpg|jpeg'
                //    });
                //});

                //======================联系人====================

                // 添加联系人
                $('#btnCreate').on('click', function (e) {
                    e.preventDefault();
                    popdialog.showModal({
                        'url': '../PersonalInfo/AddModal.html',
                        'width': '900px',
                        'showSuccess': initAddPersonalInfo
                    });
                });

                // 修改联系人
                $('#btnEdit').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_lianXiRenTable").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要修改的记录');
                        return false;
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('每次只能修改一条记录');
                        return false;
                    }
                    //$(rows).each(function (i, item) {
                    //    ids.push(item.data.Id);
                    //});
                    //$('#SelectData').val(ids.join(','));
                    var personalId = rows[0].data.Id;
                    sessionStorage.setItem("_PersonId", personalId);
                    popdialog.showModal({
                        'url': '../PersonalInfo/EditModal.html',
                        'width': '900px',
                        'showSuccess': initEditPersonalInfo
                    });
                });

                // 删除联系人
                $('#btnDel').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_lianXiRenTable").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要操作的行');
                        return false;
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('只能选择一条记录进行删除');
                        return false;
                    }
                    //var isDeleteFaRen = false;
                    //$(rows).each(function (i, item) {
                    //    if (item.data.LeiBie == "2") {
                    //        isDeleteFaRen = true;
                    //    }
                    //    ids.push(item.data.Id);
                    //});
                    var param = {};
                    param.Id = rows[0].data.Id;
                    param.OrgId = rows[0].data.OrgId;
                    var paramAll = {
                        info: param,
                        type: 1
                    };
                    tipdialog.confirm("确定要删除选中的记录？", function (r) {
                        if (r) {
                            helper.Ajax("003300300304", paramAll, function (data) {
                                if (data.body) {
                                    //if (isDeleteFaRen) {
                                    //    initTopFuZeRenXinXi();
                                    //}
                                    toast.success("删除成功");
                                    setTimeout(function () {
                                        $('#tab2').click();
                                    }, 1000);
                                }
                                else {
                                    tipdialog.alertMsg(data.publicresponse.message);
                                }
                            }, false);
                        }
                    });
                });
                //=====================================================

                //-------------创建人员账号start-------------
                $('#btnCreateUsr').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_lianXiRenTable").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要操作的行');
                        return false;
                    }

                    var param = {};
                    $(rows).each(function (i, item) {
                        param[item.data.Id] = {
                            "OrgName": item.data.OrgName,
                            "OrgCode": item.data.OrgCode,
                            "OrgId": item.data.OrgId
                        }
                    });

                    helper.Ajax("003300300330", { "accounts": param }, function (data) {
                        if (data.body) {
                            toast.success("创建成功");
                            $('#tab2').click();
                        }
                        else {
                            tipdialog.alertMsg(data.publicresponse.message);
                        }
                    }, false);

                });
                //-------------创建人员账号end-------------


                selectCity2.initSelectView($('.select'));
                $('#add').click(function (e) {
                    e.preventDefault();
                    selectCity2.showSelectCity();

                });

            };


            //新增联系人
            function initAddLianXiRen() {
                formcontrol.initial();
                $('#AddZiBiaoSure').on('click', function (e) {
                    e.preventDefault();
                    var jsonData1 = $('#SelectForm').serializeObject();
                    jsonData1.BenDanWeiOrgCode = $('#BenDanWeiOrgCode').val();
                    if (!CheckSubmit(jsonData1)) {
                        return;
                    }
                    //调用联系人接口
                    //helper.Ajax("true", jsonData1, function (data) {
                    helper.Ajax("003300300010", jsonData1, function (data) {
                        if (data.publicresponse.statuscode == 0) {
                            if (data.body) {
                                toast.success("新增成功");
                                popdialog.closeModal();
                                $("#tb_lianXiRenTable").CustomTable("reload");
                            }
                            else {
                                tipdialog.alertMsg(data.publicresponse.message);
                            }
                        } else {
                            tipdialog.errorDialog("请求失败");
                        }
                    }, false);

                });
            };

            //修改联系人
            function initEditLianXiRen() {
                formcontrol.initial();
                var id = $('#SelectData').val();
                getLianXiRenXinXi(id);

                $('#EditZiBiaoSure').on('click', function (e) {
                    e.preventDefault();
                    var jsonData1 = $('#SelectForm').serializeObject();
                    jsonData1.Id = $('#SelectData').val()
                    jsonData1.BenDanWeiOrgCode = $('#BenDanWeiOrgCode').val();
                    if (!CheckSubmit(jsonData1)) {
                        return;
                    }
                    //调用更新联系人接口
                    //helper.Ajax("true", jsonData1, function (data) {
                    helper.Ajax("003300300011", jsonData1, function (data) {
                        if (data.publicresponse.statuscode == 0) {
                            if (data.body) {
                                toast.success('修改成功');
                                popdialog.closeModal();
                                $("#tb_lianXiRenTable").CustomTable("reload");
                            }
                            else {
                                tipdialog.alertMsg(data.publicresponse.message);
                            }
                        }else {
                            tipdialog.errorDialog("请求失败");
                        }
                    }, false);
                });
            }

            //获取联系人
            function getLianXiRenXinXi(id) {
                helper.Ajax("003300300013", id, function (resultdata) {
                    if (resultdata.publicresponse.statuscode == 0) {
                        var resource = resultdata.body;
                        $('#QueryAddZiBiao').find('.form-control-static').each(function (i, item) {
                            var index = $(item).attr('for');
                            var tempValue = resource[index];
                            if (tempValue != undefined) {
                                $(item).html(tempValue == '' ? '' : tempValue);
                            } else {
                                $(item).html('');
                            }
                        });
                        $('#QueryAddZiBiao').find('input[name],select[name],textarea[name]').each(function (i, item) {
                            var tempValue = resource[$(item).attr('name')];
                            if (tempValue != undefined) {
                                $(item).val(tempValue.toString() == '' ? '' : tempValue);
                            } else {
                                $(item).val('');
                            }
                        });
                        $('#preLeiBie').val($('#LeiBie').val());
                    } else {
                        tipdialog.errorDialog("请求数据失败");
                    }
                }, false);
            }





            // 联系人增确认
            function CheckSubmit(jsonData1) {
                var msg = '';
                if (jsonData1.LianXiRen == '') {
                    msg += '联系人不能为空<br/>';
                }
                if (jsonData1.ShenFenZheng == '') {
                    msg += '身份证不能为空<br/>';
                }
                if (actionhub.CheckIDCard(jsonData1.ShenFenZheng) == false) {
                    msg += '身份证号码格式不正确';
                }
                if (msg != '') {
                    tipdialog.alertMsg(msg);
                    return false;
                } else {
                    return true
                }
            };

            function initAddPersonalInfo() {
                // 清除证件照缓存
                sessionStorage.removeItem("IPFileId");
                formcontrol.initial();
                dropdown.initial();
                // 初始化入职时间框
                $('#EntryDate').datepicker({
                    language: 'zh-CN',
                    format: 'yyyy-mm-dd',
                    endDate: getNowFormatDate(),//可选日期的结束日期
                    autoclose: true//选中之后自动隐藏日期选择框
                });
                $('#EntryDate').removeAttr("style");
                $('#EntryDate').attr("style", "cursor:pointer;background-color:white !important");
                // 初始化证件照上传
                $('.fa-upload').each(function (index, item) {
                    $('#' + $(item).parent()[0].id).fileupload({
                        multi: false,
                        timeOut: 20000,
                        maxSize: '3',//3M
                        allowedContentType: 'png|jpg|jpeg',
                        callback: function (data) {
                            sessionStorage.setItem('IPFileId', data.FileId);
                            var href = $("#" + data.FileId + 'View').attr('href');

                            $('#IDPhotoYuLan').attr("src", href);
                            setTimeout(function () {
                                $("#" + data.FileId + 'Delete').live('click', function () {
                                    $('#IDPhotoYuLan').attr("src", "../../Component/Img/NotPic.jpg");
                                    sessionStorage.setItem("IPFileId", "");
                                });
                            }, 2000);

                            $("#" + data.FileId + 'View').attr('style', "margin-left:75px;margin-top:20px;");
                            $("#" + data.FileId + 'Delete').attr('style', "margin-top:20px;");

                        }
                    });
                })
                // 初始化职务
                $.each(positionTypeArr, function (index, item) {
                    var checkBoxStr = '';
                    checkBoxStr += `<div style="width:50%;float:left;margin-top:8px;">
                                    <input style="margin-top:2px;width:15px;height:15px;opacity:0.6;" type="checkbox" name="Positions" val="${item.PositionCode}" />
                                    <span style="vertical-align:top;">${item.PositionName}</span>
                                    </div>
                                    `
                    $('#PositionsCheckBox').append(checkBoxStr);
                })
                $('#PositionsCheckBox').find('[type="checkbox"]').each(function (index, item) {
                    $(item).on('click', function () {
                        if ($(item).parent().attr("class") == "checked") {
                            $(item).parent().removeAttr("class");
                        } else {
                            $(item).parent().attr("class", "checked");
                        }

                    })
                })
                // 新增人员信息
                $("#AddPersonalInfoSure").on('click', function (e) {
                    e.preventDefault();
                    if (formcontrol.buttonValid()) {
                        var param = {}
                        var positionArr = [];
                        var errorMsg = "";
                        param.IDCardType = $("#IDCardType").val();
                        param.IDCard = $("#IDCard").val().replace(/\s+/g, "");
                        param.Name = $("#Name").val().replace(/\s+/g, "");
                        param.Sex = $("#Sex").val();
                        param.Cellphone = $("#Cellphone").val().replace(/\s+/g, "");
                        param.EntryDate = $("#EntryDate").val();
                        param.CompanyName = $("#JiGouMingCheng").val();
                        param.WorkingStatus = 2;

                        $('#PositionsCheckBox').find('[type="checkbox"]').each(function (i, item) {
                            if ($(item).parent().attr("class") == "checked") {
                                var positionCode = $(item).attr("val");
                                positionArr.push(positionCode);
                            }
                        });

                        param.Positions = positionArr.join(',');
                        param.OrgName = $('#JiGouMingCheng').val();
                        param.OrgCode = $("#BenDanWeiOrgCode").val();
                        param.OrgId = $('#Id').val();

                        // 校验数据
                        var fid = sessionStorage.getItem('IPFileId');
                        if (fid && fid != '') {
                            param.IDPhoto = fid;
                        } else {
                            errorMsg += "人员图片必须上传</br>"
                        }
                        if (!checkPosition()) {
                            errorMsg += '职务为必选项</br>';
                        }
                        if ($('#Email').val() && !checkEmailFormat($('#Email').val())) {
                            errorMsg += '邮箱格式不正确</br>';
                        }
                        if ($('#Cellphone').val() && !checkCellPhoneFormat($('#Cellphone').val())) {
                            errorMsg += '手机号码格式不正确</br>';
                        }
                        if ($.inArray("QY009", param.Positions) >= 0 || $.inArray("QY010", param.Positions) >= 0) {
                            if (!param.IDCard || param.IDCard == '') {
                                errorMsg += '证件号为必填项</br>';
                            }
                        }
                        if (param.IDCardType == 0 && param.IDCard && param.IDCard != '' && ValidIdentityCardNumber(param.IDCard) == false) {
                            errorMsg += '证件号格式不正确</br>';
                        }
                        if (errorMsg != "") {
                            tipdialog.alertMsg(errorMsg);
                            return;
                        }

                        // 新增人员信息
                        layer.load(2);
                        helper.Ajax("003300300301", param, function (data) {
                            if (data.body) {
                                layer.closeAll();
                                toast.success("新增成功");
                                setTimeout(function () {
                                    $('#tb_lianXiRenTable').CustomTable('reload');
                                    popdialog.closeModal();
                                }, 1000);
                            }
                            else {
                                layer.closeAll();
                                tipdialog.alertMsg(data.publicresponse.message);
                            }
                        }, false);
                    }
                })
            }

            function initEditPersonalInfo() {
                var positionArr = [];
                // 清除证件照缓存
                sessionStorage.removeItem("IPFileId");
                formcontrol.initial();
                dropdown.initial();
                // 初始化入职时间框
                $('#EntryDate').datepicker({
                    language: 'zh-CN',
                    format: 'yyyy-mm-dd',
                    endDate: getNowFormatDate(),//可选日期的结束日期
                    autoclose: true//选中之后自动隐藏日期选择框
                });
                $('#EntryDate').removeAttr("style");
                $('#EntryDate').attr("style", "cursor:pointer;background-color:white !important");
                // 初始化证件照上传
                //$('.fa-upload').each(function (index, item) {
                //    $('#' + $(item).parent()[0].id).fileupload({
                //        multi: false,
                //        timeOut: 20000,
                //        maxSize: '3',//3M
                //        allowedContentType: 'png|jpg|jpeg',
                //        callback: function (data) {
                //            sessionStorage.setItem('IPFileId', data.FileId);
                //            var href = $("#" + data.FileId + 'View').attr('href');

                //            $('#IDPhotoYuLan').attr("src", href);
                //            setTimeout(function () {
                //                $("#" + data.FileId + 'Delete').live('click', function () {
                //                    $('#IDPhotoYuLan').attr("src", "../../Component/Img/NotPic.jpg");
                //                    sessionStorage.setItem("IPFileId", "");
                //                });
                //            }, 2000);
                //            $("#" + data.FileId + 'View').attr('style', "margin-left:75px;margin-top:20px;");
                //            $("#" + data.FileId + 'Delete').attr('style', "margin-top:20px;");

                //        }
                //    });
                //})
                // 初始化职务
                $.each(positionTypeArr, function (index, item) {
                    var checkBoxStr = '';
                    checkBoxStr += `<div style="width:50%;float:left;margin-top:8px;">
                                    <input style="margin-top:2px;width:15px;height:15px;opacity:0.6;" type="checkbox" name="Positions" val="${item.PositionCode}" />
                                    <span style="vertical-align:top;">${item.PositionName}</span>
                                    </div>
                                    `
                    $('#PositionsCheckBox').append(checkBoxStr);
                })
                $('#PositionsCheckBox').find('[type="checkbox"]').each(function (index, item) {
                    $(item).on('click', function () {
                        if ($(item).parent().attr("class") == "checked") {
                            $(item).parent().removeAttr("class");
                        } else {
                            $(item).parent().attr("class", "checked");
                        }

                    })
                })
                // 加载详情
                var pId = sessionStorage.getItem("_PersonId");
                helper.Ajax("003300300303", { Id: pId, OrgId: $("#Id").val() }, function (data) {
                    if (data.body) {
                        sessionStorage.removeItem("_PersonId");
                        SinglefillFormData('PersonlForm', data.body);
                        $("#hdPersonalId").val(data.body.Id);
                        if (data.body.IDPhoto && data.body.IDPhoto != '') {
                            sessionStorage.setItem("IPFileId", data.body.IDPhoto);
                            $('#IDPhotoYuLan').attr("src", system.GetFilePath + "?id=" + data.body.IDPhoto);
                            $('#IDPhotoId').val(data.body.IDPhoto);
                            fileupload.rebindFileButtonEdit(['IDPhotoId']);
                            // 文件删除按钮
                            $('#' + data.body.IDPhoto + 'Delete').on('click', function () {
                                $('#IDPhotoYuLan').attr("src", "../../Component/Img/NotPic.jpg");
                                sessionStorage.setItem("IPFileId", "");
                                var timer1 = setInterval(function () {
                                    if ($('#IDPhoto').length > 0) {
                                        //停止定时器
                                        clearInterval(timer1);
                                        $('#IDPhoto').css("margin-left", "25px");
                                        //重新绑定
                                        $('#IDPhoto').unbind();
                                        $('#IDPhoto').fileupload({
                                            multi: false,
                                            timeOut: 20000,
                                            maxSize: '3',//3M
                                            allowedContentType: 'png|jpg|jpeg',
                                            callback: function (data) {
                                                sessionStorage.setItem('IPFileId', data.FileId);
                                                var href = $("#" + data.FileId + 'View').attr('href');

                                                $('#IDPhotoYuLan').attr("src", href);
                                                setTimeout(function () {
                                                    $("#" + data.FileId + 'Delete').live('click', function () {
                                                        $('#IDPhotoYuLan').attr("src", "../../Component/Img/NotPic.jpg");
                                                        sessionStorage.setItem("IPFileId", "");
                                                    });
                                                }, 2000);
                                                //$("#" + data.FileId + 'View').attr('style', "margin-left:3px;margin-top:20px;");
                                                //$("#" + data.FileId + 'Delete').attr('style', "margin-top:20px;");

                                            }
                                        });
                                    }
                                }, 50);

                            })
                        } else {
                            $('#IDPhotoUploadBtn').attr("style", "margin-left:25px");
                            $('#IDPhotoUploadBtn').fileupload({
                                multi: false,
                                timeOut: 20000,
                                maxSize: '3',//3M
                                allowedContentType: 'png|jpg|jpeg',
                                callback: function (data) {
                                    sessionStorage.setItem('IPFileId', data.FileId);
                                    var href = $("#" + data.FileId + 'View').attr('href');

                                    $('#IDPhotoYuLan').attr("src", href);
                                    setTimeout(function () {
                                        $("#" + data.FileId + 'Delete').live('click', function () {
                                            $('#IDPhotoYuLan').attr("src", "../../Component/Img/NotPic.jpg");
                                            sessionStorage.setItem("IPFileId", "");
                                        });
                                    }, 2000);

                                }
                            });
                        }
                        data.body.Positions = data.body.Positions.split(',');
                        if (data.body.Positions && data.body.Positions.length > 0) {
                            $.each(data.body.Positions, function (i, item) {
                                $("input[name='Positions'][val=" + item + "]").attr("checked", "checked");
                                $("input[name='Positions'][val=" + item + "]").parent().attr("class", "checked");
                            });
                        }
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);
                // 修改人员信息
                $("#EditPersonalInfoSure").on('click', function (e) {
                    positionArr = [];
                    e.preventDefault();
                    if (formcontrol.buttonValid()) {
                        var param = {}
                        var errorMsg = "";
                        param.Id = $("#hdPersonalId").val();
                        param.IDCardType = $("#IDCardType").val();
                        param.IDCard = $("#IDCard").val().replace(/\s+/g, "");
                        param.Name = $("#Name").val().replace(/\s+/g, "");
                        param.Sex = $("#Sex").val();
                        param.Cellphone = $("#Cellphone").val().replace(/\s+/g, "");
                        param.EntryDate = $("#EntryDate").val();
                        param.WorkingStatus = 2;
                        param.OrgName = $("#JiGouMingCheng").val();
                        param.OrgCode = $("#BenDanWeiOrgCode").val();
                        param.OrgId = $("#Id").val();

                        $('#PositionsCheckBox').find('[type="checkbox"]').each(function (i, item) {
                            if ($(item).parent().attr("class") == "checked") {
                                var positionCode = $(item).attr("val");
                                positionArr.push(positionCode);
                            }
                        });
                        param.Positions = positionArr.join(',');

                        // 校验数据
                        var fid = sessionStorage.getItem('IPFileId');
                        if (fid && fid != '') {
                            param.IDPhoto = fid;
                        } else {
                            errorMsg += "人员图片必须上传</br>"
                        }
                        if (!checkPosition()) {
                            errorMsg += '职务为必选项</br>';
                        }
                        if ($.inArray("QY009", param.Positions) >= 0 || $.inArray("QY010", param.Positions) >= 0) {
                            if (!param.IDCard || param.IDCard == '') {
                                errorMsg += '证件号为必填项</br>';
                            }
                        }
                        if (param.IDCardType == 0 && param.IDCard && param.IDCard != '' && ValidIdentityCardNumber(param.IDCard) == false) {
                            errorMsg += '证件号格式不正确</br>';
                        }
                        if ($('#Email').val() && !checkEmailFormat($('#Email').val())) {
                            errorMsg += '邮箱格式不正确</br>';
                        }
                        if ($('#Cellphone').val() && !checkCellPhoneFormat($('#Cellphone').val())) {
                            errorMsg += '手机号码格式不正确</br>';
                        }
                        if (errorMsg != "") {
                            tipdialog.alertMsg(errorMsg);
                            return;
                        }

                        var paramAll = {
                            info: param,
                            type: 0
                        }
                        // 修改人员信息
                        layer.load(2);
                        helper.Ajax("003300300304", paramAll, function (data) {
                            if (data.body) {
                                layer.closeAll();
                                toast.success("修改成功");
                                setTimeout(function () {
                                    $('#tb_lianXiRenTable').CustomTable('reload');
                                    popdialog.closeModal();
                                }, 1000);
                            }
                            else {
                                layer.closeAll();
                                tipdialog.alertMsg(data.publicresponse.message);
                            }
                        }, false);
                    }
                })
            }

            //初始化表单数据
            function initData() {

                var id = window.parent.document.getElementById('hdIDS').value;

                helper.Ajax("006600200056", id, function (data) {
                    if ($.type(data) == "string") {
                        data = helper.StrToJson(data);
                    }
                    if (data.publicresponse.statuscode == 0) {
                        var body = data.body;
                        $("#Form1").find("input[name],select[name],textarea[name]").each(function (i, item) {
                            var tempvalues = body[$(item).attr('name')];
                            if (tempvalues !== undefined) {
                                $(item).val(tempvalues.toString() == "" ? "" : tempvalues);
                            } else {
                                $(item).val('');
                            };
                        });
                        selectCity2.setData(body['JingYingQuYu']);
                        getPersonalPosition();
                        initlizableLianXiRenTable();

                        //初始化辖区省市县
                        selectcity.setXiaQu('00000020005', { "Province": "广东" }, '#XiaQuShi', function () {
                            if (body.XiaQuShi) {
                                $("#XiaQuShi").val(body.XiaQuShi);
                            }
                        }, 'key', 'key', body.XiaQuShi);

                        selectcity.setXiaQu('00000020006', { "City": body.XiaQuShi }, '#XiaQuXian', function () {
                            if (body.XiaQuXian) {
                                $("#XiaQuXian").val(body.XiaQuXian);
                            }
                        }, 'key', 'key', body.XiaQuShi);

                        $('#XiaQuShi').on("change", function () {
                            var data = { "City": $(this).val() };
                            if ($(this).val() != '') {
                                ///调用接口初始化区县下拉框
                                selectcity.setXiaQu('00000020006', data, '#XiaQuXian', function () { }, 'GetDistrictList', 'DistrictName');
                            }
                        });
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);


            };
            //保存
            function save() {
                //TODO: 校验数据
                var jsonData1 = $('#Form1').serializeObject();
                //jsonData1.QiYeLeiXing = $('#QiYeLeiXing').val();
                //jsonData1.YouXiaoZhuangTai = $('#YouXiaoZhuangTai').val();
                for (var key in jsonData1) {
                    jsonData1[key] = jsonData1[key].replace(/\s/g, "");
                }


                //调用更新接口
                //TODO
                helper.Ajax("006600200057", jsonData1, function (data) {
                    if ($.type(data) == "string") {
                        data = helper.StrToJson(data);
                    }
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {
                            toast.success("档案保存成功");
                        }
                        else {
                            tipdialog.alertMsg("档案保存失败");
                        }
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);
            };


            //初始化联系人表格
            function initlizableLianXiRenTable() {
                $("#tb_lianXiRenTable").CustomTable({
                    ajax: helper.AjaxData("003300300329",
                        function (data) {
                            var pageInfo = { Page: data.start / data.length + 1, Rows: data.length };
                            for (var i in data) {
                                delete data[i];
                            }
                            var para = new Object();
                            para["OrgId"] = $("#Id").val();
                            pageInfo.data = para;
                            $.extend(data, pageInfo);
                        }, null),
                    single: false,
                    //filter: true,
                    ordering: true, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [
                        {
                            render: function (data, type, row) {
                                return '<input type=checkbox class=checkboxes />';
                            }
                        },
                        { data: 'Name' },
                        {
                            data: 'Positions',
                            render: function (data, type, row, meta) {
                                var value = '';
                                if (data) {
                                    data = data;
                                    $.each(data, function (pi, p) {
                                        $.each(positionTypeArr, function (ti, t) {
                                            if (p == t.PositionCode) {
                                                value += `${t.PositionName}|`;
                                            }
                                        })
                                    })
                                    value = value.substring(0, value.length - 1)
                                }
                                return value;
                            }
                        },
                        { data: 'IDCard' },
                        { data: 'Cellphone' },
                        {
                            data: 'IsCreateAccount',
                            render: function (data, type, row, meta) {
                                var createAccountStr = "";
                                switch (data) {
                                    case 1:
                                        createAccountStr = "已创建";
                                        break;
                                    case 0:
                                        createAccountStr = "未创建";
                                        break;
                                    default:
                                        createAccountStr = ""
                                        break;
                                }
                                return createAccountStr;
                            }
                        }
                    ],
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        tableheadfix.ResetFix();
                    }
                });
            };

            /*  人员档案所需  start */
            function getPersonalPosition() {
                var type = $('#JiGouLeiXing').val();
                var index = -1;
                if (type) {
                    index = type == 9 ? 3 : (type == 10 ? 1 : -1);
                }
                if (index == -1) {
                    return;
                }
                helper.Ajax("003300300332", { PositionTypeIndex: index }, function (data) {
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {
                            positionTypeArr = data.body;
                        }
                    } else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);
            }

            //获取当前日期
            function getNowFormatDate() {
                var date = new Date();
                var seperator1 = "-";
                var month = date.getMonth() + 1;
                var strDate = date.getDate();
                if (month >= 1 && month <= 9) {
                    month = "0" + month;
                }
                if (strDate >= 0 && strDate <= 9) {
                    strDate = "0" + strDate;
                }
                var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate;

                return currentdate;
            };

            //检查职务是否选择
            function checkPosition() {
                var num = 0;
                $('#PositionsCheckBox').find('[type="checkbox"]').each(function (i, item) {
                    if ($(item).attr("checked")) {
                        num++;
                    }
                });
                if (num == 0) {
                    return false;
                }
                return true;
            }

            function checkEmailFormat(el) {
                return /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+/.test(el);
            }

            function checkCellPhoneFormat(el) {
                return /^[0-9]{11}$/.test(el);
            }

            function SinglefillFormData(formId, resource) {
                $('#' + formId).find('input[name],select[name],textarea[name]').each(function (i, item) {
                    $(item).val('');
                    var tempValue = resource[$(item).attr('name')];
                    if (tempValue != undefined) {
                        if ($(item).hasClass('datepicker')) {
                            tempValue = tempValue.substr(0, 10);
                        }
                        $(item).val(tempValue.toString() == '' ? '' : tempValue);
                    } else {
                        $(item).val('');
                    }
                });
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
            /*  人员档案所需  end */


            //级联城市下拉框
            function selectCity(UserInfo) {
                UserInfo = UserInfo.body;
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#XiaQuShi, #XiaQuXian').empty().append(defaultOption);

                var data = { "Province": "广东" };///todo:初始化省份
                ///调用接口初始化城市下拉框
                selectcity.setXiaQu('00000020005', data, '#XiaQuShi', function () {
                    var XiaQuShi = UserInfo.OrganizationManageArea;
                    XiaQuShi = XiaQuShi.replace(/广东/g, "");
                    var list = XiaQuShi.split("|");
                    $("#XiaQuShi").find("option").each(function (index, item) {
                        if (list.indexOf($(item).val()) < 0 && $(item).val() != "") {
                            $(item).remove();
                        }
                    });
                }, 'GetCityList', 'CityName');

                $('#XiaQuShi').on("change", function () {
                    $('#XiaQuXian').empty().append(defaultOption);
                    var data = { "City": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化区县下拉框
                        selectcity.setXiaQu('00000020006', data, '#XiaQuXian', function () {
                            $("#XiaQuXian").val(XiaQuXian);
                        }, 'GetDistrictList', 'DistrictName');
                    }
                });

                $('#XiaQuXian').change(function () {
                    var data = { "District": $(this).val() };
                });
            }

            initPage();
        });
});
