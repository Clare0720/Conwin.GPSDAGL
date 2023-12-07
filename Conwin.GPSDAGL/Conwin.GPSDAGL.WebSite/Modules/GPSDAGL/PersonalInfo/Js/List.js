define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'searchbox', 'customtable', 'bootstrap-datepicker.zh-CN', 'permission'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system) {
            var positionTypeArr = [];
            var userInfo = helper.GetUserInfo();

            var initPage = function () {
                getPersonalPosition();

                initlizableTable();

                listButtonInit();

                dropDownInit();
            };

            function getPersonalPosition() {
                var param = {};
                var groupFlag = false;
                if (userInfo.OrganizationType == 0) {
                    param.IsAll = 0;
                    groupFlag = true;
                }
                helper.Ajax("003300300332", param, function (data) {
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {
                            positionTypeArr = data.body;
                            bindPositionsSelectDropDown("Positions", groupFlag);
                        }
                    } else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);
            }

            // 初始化表格
            function initlizableTable() {
                $("#tb_PersonalInfo").CustomTable({
                    ajax: helper.AjaxData("003300300300",
                        function (data) {
                            var pageInfo = { Page: data.start / data.length + 1, Rows: data.length };
                            for (var i in data) {
                                delete data[i];
                            }
                            var para = $('.searchpanel-form').serializeObject();
                            $('.searchpanel-form').find('[disabled]').each(function (i, item) {
                                para[$(item).attr('name')] = $(item).val();
                            });
                            pageInfo.data = para;
                            $.extend(data, pageInfo);
                        }, null),
                    single: true,
                    filter: true,
                    //ordering: true, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [
                        {
                            render: function (data, type, row) {
                                return '<input type=checkbox class=checkboxes />';
                            }
                        },
                        { data: 'Name' },
                        { data: 'Sex' },
                        { data: 'Cellphone' },
                        { data: 'IDCard' },
                        {
                            data: 'Positions',
                            render: function (data, type, row, meta) {
                                var value = '';
                                var po = data.split(',');
                                if (po && po.length > 0) {
                                    $.each(po, function (positionIndex, positionItem) {
                                        $.each(positionTypeArr, function (tableIndex, tableItem) {
                                            if (positionItem == tableItem.PositionCode) {
                                                value += `${tableItem.PositionName}|`;
                                            }
                                        })
                                    })
                                    value = value.substring(0, value.length - 1)
                                    return value;
                                } else {
                                    return "";
                                }

                            }
                        },
                        {
                            data: 'WorkingStatus',
                            render: function (data, type, row) {
                                var statusStr = ""
                                switch (data) {
                                    case 1:
                                        statusStr = "待确认";
                                        break;
                                    case 2:
                                        statusStr = "在职";
                                        break
                                    case 3:
                                        statusStr = "离职";
                                        break
                                    default:
                                        break;
                                }
                                return statusStr;
                            }
                        },
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
                        },
                        {
                            data: 'ZhaoPian',
                            render: function (data, type, row) {
                                result = '<a class="btn blue ViewPhoto" style="padding: 5px" >' + data + '</a>';
                                return result;
                            }
                        },
                    ],
                    pageLength: 10,
                    "fnDrawCallback": function () {
                        viewPhotoInit();
                        tableheadfix.ResetFix();
                    },
                });
            };

            // 列表按钮初始化
            function listButtonInit() {
                //查询
                $('#btnSearch').click(function (e) {
                    e.preventDefault();
                    $("#tb_PersonalInfo").CustomTable("reload");
                });

                //重置
                $("#btnReset").click(function (e) {
                    e.preventDefault();
                    $('.searchpanel-form').find('input[type=text]:not(:disabled), select:not(:disabled)').val('');
                });

                //新增
                $("#btnCreate").click(function (e) {
                    e.preventDefault();
                    popdialog.showIframe({
                        'url': 'Add.html',
                        head: false
                    });
                });

                //修改
                $("#btnEdit").click(function (e) {
                    e.preventDefault();
                    var row = GetSelectRow();
                    if (!row) {
                        return false;
                    }

                    $("#hdOrgName").val(row.OrgName);
                    $("#hdOrgCode").val(row.OrgCode);
                    $("#hdOrgId").val(row.OrgId);

                    popdialog.showIframe({
                        'url': 'Edit.html',
                        head: false
                    });

                });

                //查看
                $("#btnView").click(function (e) {
                    e.preventDefault();
                    var row = GetSelectRow();
                    if (!row) {
                        return false;
                    }

                    $("#hdOrgName").val(row.OrgName);
                    $("#hdOrgCode").val(row.OrgCode);
                    $("#hdOrgId").val(row.OrgId);

                    popdialog.showIframe({
                        'url': 'View.html',
                        head: false
                    });
                });

                // 解聘
                $("#btnDismiss").click(function (e) {
                    e.preventDefault();
                    var row = GetSelectRow();
                    if (!row) {
                        return false;
                    }
                    var param = {};
                    param.Id = row.Id;
                    var paramAll = {
                        info: param,
                        type: 2
                    }
                    helper.Ajax("003300300304", paramAll, function (data) {
                        if (data.body) {
                            toast.success("解聘成功");
                            setTimeout(function () {
                                $("#tb_PersonalInfo").CustomTable("reload");
                            }, 1000);
                        }
                        else {
                            tipdialog.alertMsg(data.publicresponse.message);
                        }
                    }, false);
                });

                // 确认  
                $("#btnConfirm").click(function (e) {
                    e.preventDefault();
                    var row = GetSelectRow();
                    if (!row) {
                        return false;
                    }
                    var param = {};
                    param.Id = row.Id;
                    var paramAll = {
                        info: param,
                        type: 3
                    }
                    helper.Ajax("003300300304", paramAll, function (data) {
                        if (data.body) {
                            toast.success("确认成功");
                            setTimeout(function () {
                                $("#tb_PersonalInfo").CustomTable("reload");
                            }, 1000);
                        }
                        else {
                            tipdialog.alertMsg(data.publicresponse.message);
                        }
                    }, false);
                });

                // 删除
                $("#btnDel").click(function (e) {
                    e.preventDefault();
                    var row = GetSelectRow();
                    if (!row) {
                        return false;
                    }
                    tipdialog.confirm("是否删除选中的人员？", function (r) {
                        if (r) {
                            var param = {};
                            param.Id = row.Id;
                            param.OrgId = row.OrgId;
                            param.OrgName = row.OrgName;
                            param.OrgCode = row.OrgCode;
                            var paramAll = {
                                info: param,
                                type: 1
                            }
                            helper.Ajax("003300300304", paramAll, function (data) {
                                if (data.body) {
                                    toast.success("删除成功");
                                    setTimeout(function () {
                                        $("#tb_PersonalInfo").CustomTable("reload");
                                    }, 1000);
                                }
                                else {
                                    tipdialog.alertMsg(data.publicresponse.message);
                                }
                            }, false);
                        }
                    });
                });

                //创建账号
                $('#btnCreateUsr').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_PersonalInfo").CustomTable('getSelection');
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
                            toast.success("创建账号成功");
                            setTimeout(function () {
                                $("#tb_PersonalInfo").CustomTable("reload");
                            }, 1000);
                        }
                        else {
                            tipdialog.alertMsg(data.publicresponse.message);
                        }
                    }, false);

                });
            }

            // 初始化查看图片按钮
            function viewPhotoInit() {
                //查看驾驶员照片
                $(".ViewPhoto").click(function () {
                    var eventData = $('#tb_PersonalInfo').DataTable().row($(this).parents('tr')).data();
                    if (eventData.IDPhoto && eventData.IDPhoto != '') {
                        var imgUrl = system.GetFilePath + '?id=' + eventData.IDPhoto;
                        sessionStorage.setItem("_PersonPhoto", imgUrl);
                    }
                    sessionStorage.setItem("_PersonalName", eventData.Name);
                    popdialog.showModal({
                        'url': '../PersonalInfo/PersonalPhoto.html',
                        'width': '420px',
                        'showSuccess': showPersonalPhoto
                    });
                });
            }

            // 查看图片按钮回调
            function showPersonalPhoto() {
                $("#PersonalPhoto").attr("src", sessionStorage.getItem("_PersonPhoto"));
                $("#PersonalName").text(sessionStorage.getItem("_PersonalName"));
                $(".close").on("click", function () {
                    sessionStorage.setItem("_PersonPhoto", "");
                    sessionStorage.setItem("_PersonalName", "")
                })
            }

            // 下拉框初始化
            function dropDownInit() {
                bindSelectDropDown("WorkingStatus", "WorkingStatus");
            }

            function bindSelectDropDown(enumName, targetName, ignoreVal) {
                var tag = '<option value="">请选择</option>', optionStr = '';
                if (EnumsSet[enumName]) {
                    $.each(EnumsSet[enumName], function (i, item) {
                        if (typeof item == 'object' && $.inArray(item.value(), ignoreVal) < 0) {
                            optionStr += '<option value="' + item.value() + '">' + item.key() + '</option>';
                        }
                    });
                }
                $('#' + targetName).empty().append(tag).append(optionStr);
            }

            function bindPositionsSelectDropDown(targetName, isGroup) {
                var tag = '<option value="">请选择</option>', optionStr = '';
                if (isGroup) {
                    var isFirst = true;
                    var isAllFirst = true;
                    var groupName = "";
                    var currentSign = "";
                    $.each(positionTypeArr, function (i, item) {
                        if (typeof item == 'object') {
                            switch (item.PositionCode.slice(0, 2)) {
                                case "QY":
                                    if (currentSign != "QY") {
                                        currentSign = "QY";
                                        isFirst = true;
                                    } else {
                                        isFirst = false;
                                    }
                                    groupName = "道路运输企业";
                                    break;
                                case "JC":
                                    if (currentSign != "JC") {
                                        currentSign = "JC";
                                        isFirst = true;
                                    } else {
                                        isFirst = false;
                                    }
                                    groupName = "第三方监测中心";
                                    break;
                                case "FW":
                                    if (currentSign != "FW") {
                                        currentSign = "FW";
                                        isFirst = true;
                                    } else {
                                        isFirst = false;
                                    }
                                    groupName = "平台服务商";
                                    break;
                                case "BX":
                                    if (currentSign != "BX") {
                                        currentSign = "BX";
                                        isFirst = true;
                                    } else {
                                        isFirst = false;
                                    }
                                    groupName = "保险机构";
                                    break;
                                case "CW":
                                    if (currentSign != "CW") {
                                        currentSign = "CW";
                                        isFirst = true;
                                    } else {
                                        isFirst = false;
                                    }
                                    groupName = "平台运营商";
                                    break;
                            }
                            if (isFirst) {
                                if (isAllFirst) {
                                    optionStr += `<optgroup label="${groupName}"><option value="${item.PositionCode}">${item.PositionName}</option>`;
                                    isAllFirst = false;
                                } else {
                                    optionStr += `</optgroup ><optgroup label="${groupName}"><option value="${item.PositionCode}">${item.PositionName}</option>`;
                                }
                            } else {
                                optionStr += `<option value="${item.PositionCode}">${item.PositionName}</option>`;
                            }
                        }
                    });
                } else {
                    $.each(positionTypeArr, function (i, item) {
                        if (typeof item == 'object') {
                            optionStr += '<option value="' + item.PositionCode + '">' + item.PositionName + '</option>';
                        }
                    });
                }

                $('#' + targetName).empty().append(tag).append(optionStr);
            }

            function GetSelectRow() {
                var rows = $('#tb_PersonalInfo').CustomTable('getSelection');
                if (rows == undefined) {
                    tipdialog.errorDialog('请选择需要操作的一行');
                    return false;
                }
                var id = rows[0].data.Id;
                $('#hdIDS').val(id);
                return rows[0].data;
            };

            initPage();
        });
});