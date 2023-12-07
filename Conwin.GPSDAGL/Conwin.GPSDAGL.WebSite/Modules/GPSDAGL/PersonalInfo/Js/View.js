define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'selectCity2', 'metronic', 'fileupload', 'dropdown', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, selectCity2, Metronic, fileupload, dropdown) {
            var userInfo = helper.GetUserInfo();
            var positionTypeArr = [];
            var FileUploadPathSetting = helper.Route('00000080004', '1.0', system.ServerAgent);
            var myfiles = new Array();//文件上传列表

            var initPage = function () {
                common.AutoFormScrollHeight('#Form1');
                formcontrol.initial();
                dropdown.initial();
                viewPageButtonInit();
                initCurrentQiYe();
                loadData();
            };

            function viewPageButtonInit() {
                $('#btnclose').click(function () {
                    tipdialog.confirm("确定关闭？", function (r) {
                        if (r) {
                            parent.window.$("#btnSearch").click();
                            popdialog.closeIframe();
                        }
                    });
                });

                $("#backBtn").on('click', function (e) {
                    e.preventDefault();
                    parent.window.$("#btnSearch").click();
                    popdialog.closeIframe();
                });
            }

            function initCurrentQiYe() {
                $('#CompanyName').val(userInfo.OrganizationName);
            }

            function getEnumNameByValue(enumName, _v) {
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
            }

            function loadData() {
                var id = window.parent.document.getElementById('hdIDS').value;
                var hdOrgName = window.parent.document.getElementById('hdOrgName').value;
                var hdOrgCode = window.parent.document.getElementById('hdOrgCode').value;
                var hdOrgId = window.parent.document.getElementById('hdOrgId').value;
                helper.Ajax("003300300303", { Id: id, OrgId: hdOrgId }, function (data) {
                    if (data.body) {
                        var model = data.body;

                        getPersonalPosition(model.Positions.split(','));
                        model.IDCardType = getEnumNameByValue("IDCardType", model.IDCardType);
                        model.WorkingStatus = getEnumNameByValue("WorkingStatus", model.WorkingStatus);
                        $("#IDCardType").val(model.IDCardType);
                        $("#IDCard").val(model.IDCard);
                        $("#Name").val(model.Name);
                        $("#Sex").val(model.Sex);
                        //$("#Position").val(model.Position);
                        $("#Cellphone").val(model.Cellphone);
                        $("#NativePlace").val(model.NativePlace);
                        if (model.EntryDate && model.EntryDate != "") {
                            $("#EntryDate").val(model.EntryDate.substring(0, 10));
                        }
                        $("#CompanyName").val(hdOrgName);
                        $("#WorkingStatus").val(model.WorkingStatus);
                        if (model.IDPhoto && model.IDPhoto != '') {
                            var yuLanUrl = FileUploadPathSetting + "?id=" + model.IDPhoto;
                            $('#IDPhotoYuLan').attr("src", yuLanUrl);
                            $('#InitIDPhoto').val(model.IDPhoto);
                        } else {
                            $('#IDPhotoYuLan').attr("src", "../../Component/Img/NotPic.jpg");
                        }
                        if (model.Attachments && model.Attachments != "") {
                            loadAttachFiles(JSON.parse(model.Attachments));
                        }
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message, function () {
                            popdialog.closeIframe();
                        });

                    }
                }, false);
            }

            /* 文件上传 Start*/
            function loadAttachFiles(attachfilelist) {
                if (attachfilelist && attachfilelist.length > 0) {
                    attachfilelist.forEach(function (attachfile) {
                        InsertFileRow(attachfile);
                    });
                }
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
                tab.rows[tabrownum].cells[2].innerHTML = "<a id='" + fileId + "View' href='" + FileUploadPathSetting + "?id=" + fileId + "' class='btn green' target='_blank'>查看</a>";

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
                    }
                });


                $("#deletefile" + rownum).click(function (e) {
                    e.preventDefault();
                    var attr = this.attributes;
                    var fileId = attr['data-id'].value;
                    var rownumtemp = attr['data-rownum'].value;
                    if (!fileId) return;
                    actionhub.isConfirmEx('确认删除选中附件资料吗？', function () {
                        delete myfiles[fileId];
                        $("#AttachInfo_Row" + rownumtemp).remove();

                    });
                });
            }
            /* 文件上传 End*/

            function InitPositionsCheckBox() {
                // 初始化职务
                $.each(positionTypeArr, function (index, item) {
                    var checkBoxStr = '';
                    checkBoxStr += `<div style="width:50%;float:left;margin-top:2px;">
                                    <input style="width:15px;height:15px;opacity:1;" type="checkbox" name="Positions" val="${item.PositionCode}" disabled/>
                                    <span style="vertical-align:top;">${item.PositionName}</span>
                                    </div>
                                    `
                    $('#PositionsCheckBox').append(checkBoxStr);
                })
            }

            function getPersonalPosition(positions) {
                var param = {};
                helper.Ajax("003300300332", param, function (data) {
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {
                            positionTypeArr = data.body;
                            // 新版本职务
                            var positionMap = changeMap(positionTypeArr);
                            if (positions && positions.length > 0) {
                                var positionSelectVal = "";
                                $.each(positions, function (i, item) {
                                    positionSelectVal += `${positionMap.get(item)}，`;
                                });
                                $('#Positions').val(positionSelectVal.slice(0,positionSelectVal.lastIndexOf("，")));
                            }
                        }
                    } else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);
            }

            function changeMap(d) {
                var positionChangeArr = [];
                $.each(d, function (index, item) {
                    var positionEntries = Object.entries(item);
                    positionChangeArr.push([positionEntries[1][1],positionEntries[0][1]]);
                })
                return new Map(positionChangeArr);
            }

            initPage();
        });


});
