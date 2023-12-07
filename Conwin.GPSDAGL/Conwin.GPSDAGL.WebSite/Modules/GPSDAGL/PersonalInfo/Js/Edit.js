define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'selectCity2', 'metronic', 'fileupload', 'dropdown', 'bootbox', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, selectCity2, Metronic, fileupload, dropdown, bootbox) {
            var userInfo = helper.GetUserInfo();
            var positionTypeArr = [];
            var FileUploadPathSetting = helper.Route('00000080004', '1.0', system.ServerAgent);
            var myfiles = new Array();//文件上传列表

            var initPage = function () {
                getPersonalPosition();
                common.AutoFormScrollHeight('#Form1');
                formcontrol.initial();
                dropdown.initial();
                editPageButtonInit();
                initCurrentQiYe();
                loadData();
            };

            function editPageButtonInit() {
                $('#btnclose').click(function () {
                    tipdialog.confirm("确定关闭？", function (r) {
                        if (r) {
                            sessionStorage.setItem("IPFileId", "");
                            parent.window.$("#btnSearch").click();
                            popdialog.closeIframe();
                        }
                    });
                });

                $('#EntryDate').datepicker({
                    language: 'zh-CN',
                    format: 'yyyy-mm-dd',
                    endDate: getNowFormatDate(),//可选日期的结束日期
                    autoclose: true,
                    todayBtn: 'linked'
                });

                $('#EntryDate').removeAttr("style");

                $('#EntryDate').attr("style", "cursor:pointer");

                //$('.fa-upload').each(function (index, item) {
                //    $('#' + $(item).parent()[0].id).fileupload({
                //        multi: false,
                //        timeOut: 20000,
                //        maxSize: '3',//3M
                //        allowedContentType: 'png|jpg|jpeg',
                //        callback: ReloadIDPhoto
                //    });
                //});

                $("#saveBtn").on('click', function (e) {
                    e.preventDefault();
                    updateData();
                });
            }

            function initCurrentQiYe() {
                //$('#CompanyName').val(userInfo.OrganizationName);
                $('#CompanyName').removeAttr("style");
                $('#CompanyName').attr("readonly", "readonly");
            }

            function loadData() {
                var id = window.parent.document.getElementById('hdIDS').value;
                var hdOrgName = window.parent.document.getElementById('hdOrgName').value;
                var hdOrgCode = window.parent.document.getElementById('hdOrgCode').value;
                var hdOrgId = window.parent.document.getElementById('hdOrgId').value;
                helper.Ajax("003300300303", { Id: id, OrgId: hdOrgId }, function (data) {
                    if (data.body) {
                        var model = data.body;
                        $("#IDCardType").val(model.IDCardType);
                        $("#IDCard").val(model.IDCard);
                        $("#Name").val(model.Name);
                        $("#Sex").val(model.Sex);
                        // 新版本职务
                        if (model.Positions && model.Positions.trim() != "") {
                            model.Positions = model.Positions.split(',');
                            $.each(model.Positions, function (i, item) {
                                $("input[name='Positions'][val=" + item + "]").attr("checked", "checked");
                                $("input[name='Positions'][val=" + item + "]").parent().attr("class", "checked");
                            });
                        }
                        $("#Cellphone").val(model.Cellphone);
                        $("#NativePlace").val(model.NativePlace);
                        if (model.EntryDate && model.EntryDate != "") {
                            var date = new Date(model.EntryDate);
                            console.log($('#EntryDate').datepicker('getDate'));
                            $('#EntryDate').datepicker('setDate', date);
                        }
                        $("#CompanyName").val(hdOrgName);
                        $('#hdYeHuDaiMa').val(hdOrgCode);

                        $("#WorkingStatus").val(model.WorkingStatus);
                        if (model.IDPhoto && model.IDPhoto != '') {
                            var yuLanUrl = FileUploadPathSetting + "?id=" + model.IDPhoto;
                            $('#IDPhotoYuLan').attr("src", yuLanUrl);
                            $('#InitIDPhoto').val(model.IDPhoto);
                            $('#IDPhotoId').val(model.IDPhoto);
                            fileupload.rebindFileButtonEdit(['IDPhotoId']);
                            // 文件删除按钮
                            $('#' + model.IDPhoto + 'Delete').on('click', function () {
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

                                            }
                                        });
                                    }
                                }, 50);

                            })
                        } else {
                            $('#IDPhotoYuLan').attr("src", "../../Component/Img/NotPic.jpg");
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

                        if (model.Attachments && model.Attachments != "") {
                            loadAttachFiles(JSON.parse(model.Attachments));
                        }
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                        popdialog.closeModal();
                    }
                }, false);
            }

            function updateData() {
                if (formcontrol.buttonValid()) {
                    var param = {}
                    var positionArr = [];
                    var errorMsg = "";

                    param.Id = window.parent.document.getElementById('hdIDS').value;
                    param.IDCardType = $("#IDCardType").val();
                    param.IDCard = $("#IDCard").val().replace(/\s+/g, "");
                    param.Name = $("#Name").val().replace(/\s+/g, "");
                    param.Sex = $("#Sex").val();
                    //新版本职务
                    $('#PositionsCheckBox').find('[type="checkbox"]').each(function (i, item) {
                        if ($(item).parent().attr("class") == "checked") {
                            var positionCode = $(item).attr("val");
                            positionArr.push(positionCode);
                        }
                    });
                    param.Positions = positionArr.join(',');
                    param.Cellphone = $("#Cellphone").val().replace(/\s+/g, "");
                    param.NativePlace = $("#NativePlace").val().replace(/\s+/g, "");
                    param.EntryDate = $("#EntryDate").val();
                    param.OrgName = $("#CompanyName").val();
                    param.OrgCode = $("#hdYeHuDaiMa").val();
                    param.WorkingStatus = $("#WorkingStatus").val();
                    var fid = sessionStorage.getItem('IPFileId');
                    if (fid && fid != '') {
                        param.IDPhoto = fid;
                    } else {
                        param.IDPhoto = ''
                    }
                    param.Attachments = JSON.stringify(initfilelist(myfiles));

                    if (!param.IDPhoto || param.IDPhoto == "") {
                        if ($("#IDPhotoYuLan").attr("src") != "../../Component/Img/NotPic.jpg") {
                            param.IDPhoto = $('#InitIDPhoto').val();
                        } else {
                            errorMsg += "人员图片必须上传</br>"
                        }
                    }
                    // 校验数据
                    if (!param.IDCardType || param.IDCardType == "") {
                        errorMsg += "证件类型为必选项</br>"
                    }
                    if (param.IDCard.trim() == "") {
                        errorMsg += "证件号为必填项</br>"
                    } else if (param.IDCardType == 0 && ValidIdentityCardNumber(param.IDCard) == false) {
                        errorMsg += '证件号格式不正确</br>';
                    }
                    if (param.Name.trim() == "") {
                        errorMsg += "姓名为必填项</br>"
                    }
                    if (param.Sex.trim() == "") {
                        errorMsg += "性别为必选项</br>"
                    }
                    //if (!param.Position || param.Position == "") {
                    //    errorMsg += "职务为必选项</br>"
                    //}
                    if (param.Cellphone.trim() == "") {
                        errorMsg += "手机号码为必填项</br>"
                    }
                    //if (!checkPosition()) {
                    //    errorMsg += "职务为必选项</br>"
                    //}
                    if (param.Position.trim() == "") {
                        errorMsg += "职位为必填项</br>"
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
                                parent.$("#btnSearch").click();
                                sessionStorage.setItem("IPFileId", "");
                                popdialog.closeIframe();
                            }, 1000);

                        }
                        else {
                            layer.closeAll();
                            tipdialog.alertMsg(data.publicresponse.message);
                        }
                    }, false);
                }
            }

            /* 身份证验证 Start*/
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
            /* 身份证验证 End*/

            /* 文件上传 Start*/
            function loadAttachFiles(attachfilelist) {
                if (attachfilelist && attachfilelist.length > 0) {
                    attachfilelist.forEach(function (attachfile) {
                        InsertFileRow(attachfile);
                    });
                }
                InsertEmptyRow();
            }

            function initfilelist(ldata) {
                var ldata1 = [];
                for (var key in ldata) {
                    if ((typeof ldata[key]) != "function") {
                        ldata1.push(ldata[key]);
                    }
                }
                return ldata1;
            }

            function InsertEmptyRow() {

                var allrownumText = $("#tb_AttachInfoList_Row").val(); //获取表格生成的总行数
                var allrownum = parseInt(allrownumText);
                var lastrownum = allrownum + 1;

                var tab = document.getElementById("tb_AttachInfoList"); //获得表格
                var colsNum = tab.rows.item(0).cells.length; //表格的列数
                var tabnum = tab.rows.length; //表格当前的行数
                var tabrownum = tabnum - 1;

                //添加最后上传行
                tab.insertRow(tabrownum);
                tab.rows[tabrownum].id = "AttachInfo_Row" + lastrownum;

                //添加数据
                tab.rows[tabrownum].insertCell(0);
                tab.rows[tabrownum].cells[0].innerHTML = "<label name='filename" + lastrownum + "' id='filename" + lastrownum + "' type='text'></lable>";
                tab.rows[tabrownum].insertCell(1);
                tab.rows[tabrownum].cells[1].innerHTML = "<label name='fileremark" + lastrownum + "' id='fileremark" + lastrownum + "' type='text'></lable>";

                tab.rows[tabrownum].insertCell(2);
                tab.rows[tabrownum].cells[2].innerHTML = "<a href='javascript: ; ' id='file" + lastrownum + "' class='btn green uploadfilelist' data-rownum='" + lastrownum + "'>上传</a><button type='button' id='deletefile" + lastrownum + "' class='btn btn-danger' style='display: none' data-id='' data-rownum='" + lastrownum + "'>删除</button>";

                initUploadBtn(lastrownum);
            }

            function InsertFileRow(attachfile) {
                if (!attachfile) {
                    return;
                }
                var fileId = attachfile.Id;
                var allrownumText = $("#tb_AttachInfoList_Row").val(); //获取表格生成的总行数
                var allrownum = parseInt(allrownumText);
                var rownum = allrownum + 1;

                var tab = document.getElementById("tb_AttachInfoList"); //获得表格
                var colsNum = tab.rows.item(0).cells.length; //表格的列数
                var tabnum = tab.rows.length; //表格当前的行数
                var tabrownum = tabnum - 1;
                var filename = attachfile.AttachName + "." + attachfile.Extension;
                var remark = attachfile.Remark == null ? "" : attachfile.Remark;
                if (typeof (remark) == 'undefined') {
                    remark = "";
                }
                //添加行
                tab.insertRow(tabrownum);
                tab.rows[tabrownum].id = "AttachInfo_Row" + rownum;

                //添加数据
                tab.rows[tabrownum].insertCell(0);
                tab.rows[tabrownum].cells[0].innerHTML = "<label name='filename" + rownum + "' id='filename" + rownum + "' type='text'>" + filename + "</lable>";
                tab.rows[tabrownum].insertCell(1);
                tab.rows[tabrownum].cells[1].innerHTML = "<label name='fileremark" + rownum + "' id='fileremark" + rownum + "' type='text'>" + remark + "</lable>";

                tab.rows[tabrownum].insertCell(2);
                tab.rows[tabrownum].cells[2].innerHTML = "<button type='button' id='deletefile" + rownum + "' class='btn btn-danger' data-id='" + fileId + "' data-rownum='" + rownum + "'>删除</button><a id='" + fileId + "View' href='" + FileUploadPathSetting + "?id=" + fileId + "' class='btn green' target='_blank'>查看</a>";

                $("#tb_AttachInfoList_Row").val(rownum);
                myfiles[fileId] = attachfile;
                initUploadBtn(rownum);
            }

            function initUploadBtn(rownum) {

                $('#file' + rownum).fileupload({
                    isReadOnly: true,
                    multi: false,
                    callback: function (result) {
                        var fileId = result.FileId;

                        var attachInfo = {
                            Id: fileId,
                            AttachName: result.AttachName, //附件名称
                            DisplayName: result.AttachDisName,
                            Extension: result.Extension,
                            AttachSize: result.AttachSize,
                            AttachPath: result.AttachPath,
                            Remark: result.AttachRemark
                        };
                        myfiles[fileId] = attachInfo;

                        var allrownumText = $("#tb_AttachInfoList_Row").val(); //包括删除了的表格行数
                        var allrownum = parseInt(allrownumText);
                        var currentRowNum = allrownum + 1;
                        $("#deletefile" + currentRowNum).show();
                        $("#deletefile" + currentRowNum).attr("data-id", fileId);
                        var filename = result.AttachName + "." + result.Extension;
                        $("#filename" + currentRowNum).html(filename);
                        $("#fileremark" + currentRowNum).html(result.AttachRemark);

                        $("#tb_AttachInfoList_Row").val(currentRowNum);

                        InsertEmptyRow();
                    }
                });


                $("#deletefile" + rownum).click(function (e) {
                    e.preventDefault();
                    var attr = this.attributes;
                    var fileId = attr['data-id'].value;
                    var rownumtemp = attr['data-rownum'].value;
                    if (!fileId) return;
                    isConfirmEx('确认删除选中附件资料吗？', function () {
                        delete myfiles[fileId];
                        $("#AttachInfo_Row" + rownumtemp).remove();

                    });
                });
            }
            /* 文件上传 End*/

            //function ReloadIDPhoto(data) {
            //    sessionStorage.setItem('IPFileId', data.FileId);
            //    var href = $("#" + data.FileId + 'View').attr('href');
            //    $('#IDPhotoYuLan').attr("src", href);
            //    $("#" + data.FileId + 'Delete').live('click', function () {
            //        $('#IDPhotoYuLan').attr("src", "../../Component/Img/NotPic.jpg");
            //        sessionStorage.setItem("IPFileId", "");
            //    });
            //    $("#" + data.FileId + 'View').attr('style', "margin-left:65px;margin-top:20px;");
            //    $("#" + data.FileId + 'Delete').attr('style', "margin-top:20px;");

            //}

            //是否确认
            function isConfirmEx(msgStr, callbackFn, callbackNo) {
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
            };

            function InitPositionsCheckBox() {

                // 初始化职务
                $.each(positionTypeArr, function (index, item) {
                    var checkBoxStr = '';
                    checkBoxStr += `<div style="width:50%;float:left;margin-top:2px;">
                                    <input style="width:15px;height:15px;opacity:0.6;" type="checkbox" name="Positions" val="${item.PositionCode}" />
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
            }

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

            function getPersonalPosition() {
                helper.Ajax("003300300332", {}, function (data) {
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {
                            positionTypeArr = data.body;
                            InitPositionsCheckBox();
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

            initPage();
        });


});
