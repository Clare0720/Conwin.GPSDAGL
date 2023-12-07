define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'selectCity2', 'filelist', 'metronic', 'fileupload', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity,selectCity2, filelist, Metronic, fileupload) {
            var userInfo = helper.GetUserInfo();
            var viewYuanGongId = '';
            var id = window.parent.document.getElementById('hdIDS').value;
            var initPage = function () {
                id = window.parent.document.getElementById('hdIDS').value;
                initlizableTable();
                selectCity();
                common.AutoFormScrollHeight('#Form1');
                $('.date-picker').datepicker({ format: 'yyyy-mm-dd', autoclose: true, language: 'zh-CN' });
                formcontrol.initial();
                //保存
                $('#saveBtn').on('click', function (e) {
                    e.preventDefault();
                    var flags = true;
                    var msg = '';
                    var formData = $('#QiYeRenZhengXinXiForm').serializeObject();
                    // 做非空校验
                    if ($.trim(formData.QiYeMingCheng) == '') {
                        msg += "企业名称 是必填项<br/>";
                    }
                    if ($.trim(formData.TongYiSheHuiXinYongDaiMa) == '') {
                        msg += "统一社会信用代码 是必填项<br/>";
                    }
                    if ($.trim(formData.YingYunZhiZhaoDiZhi) == '') {
                        msg += "营运执照地址 是必填项<br/>";
                    }
                    if ($.trim(formData.YunYingZhiZhaoId) == '') {
                        msg += "运营执照 是必填项<br/>";
                    }
                    if ($.trim(formData.FaRenXingMing) == '') {
                        msg += "法人姓名 是必填项<br/>";
                    }
                    if ($.trim(formData.FaRenShenFenZhengHao) == '') {
                        msg += "法人身份证号 是必填项<br/>";
                    }
                    if ($.trim(formData.FaRenShouJiHaoMa) == '') {
                        msg += "法人手机号码 是必填项<br/>";
                    }
                    if ($.trim(formData.FaRenShenFenZhengZhaoId) == '') {
                        msg += "法人身份证照 是必填项<br/>";
                    }
                    if ($.trim(formData.QiYeLeiXing) == '') {
                        msg += "企业类型 是必填项<br/>";
                    }
                    if ($.trim(formData.RenZhengZhuangTai) == '') {
                        msg += "认证状态 是必填项<br/>";
                    }
                    if ($.trim(formData.ShuJuLaiYuan) == '') {
                        msg += "数据来源 是必填项<br/>";
                    }
                    if ($.trim(formData.XiaQuSheng) == '') {
                        msg += "辖区省 是必填项<br/>";
                    }
                    if ($.trim(formData.XiaQuShi) == '') {
                        msg += "辖区市 是必填项<br/>";
                    }
                    if ($.trim(formData.XiaQuXian) == '') {
                        msg += "辖区县 是必填项<br/>";
                    }
                    // 通过非空校验则进行格式校验
                    if (msg == '') {
                        var telphoneReg = /^1\d{10}$/;
                        if (telphoneReg.test($.trim(formData.FaRenShouJiHaoMa)) == false) {
                            msg += "法人手机号码 格式不正确<br/>";
                        }
                        if (ValidIdentityCardNumber($.trim(formData.FaRenShenFenZhengHao)) == false) {
                            msg += "法人身份证号 格式不正确<br/>";
                        }
                    }
                    if (msg != '') {
                        flags = false;
                        tipdialog.alertMsg(msg);
                    }
                    if (flags) {
                        save(id);
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



                var id = window.parent.document.getElementById('hdIDS').value;
                showData(id);
                //查询员工列表
                $('#btnSearchYuanGong').click(function (e) {
                    e.preventDefault();
                    $("#tb_Template").CustomTable("reload");
                });
                //新增员工
                $("#btnCreateYuanGong").on('click', function (e) {
                    e.preventDefault();
                    popdialog.showModal({
                        'url': 'AddYuanGong.html',
                        'width': '800px',
                        'showSuccess': addYuanGong
                    });
                });
                //查看员工
                $('#btnViewYuanGong').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要查看的行');
                        return false;
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('只能选择一行进行查看');
                        return false;
                    }
                    //TODO:编写逻辑
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                    });
                    viewYuanGongId = ids.join(',');
                    /*
                    popdialog.showIframe({
                        'url': 'View.html',
                        head: false
                    });
                    */
                    popdialog.showModal({
                        'url': 'ViewYuanGong.html',
                        'width': '800px',
                        'showSuccess': viewYuanGong
                    });
                });
                //修改员工
                $('#btnEditYuanGong').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template").CustomTable('getSelection'),
                        ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要查看的行');
                        return false;
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('只能选择一行进行操作');
                        return false;
                    }
                    //TODO:编写逻辑
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                    });
                    viewYuanGongId = ids.join(',');
                    popdialog.showModal({
                        'url': 'EditYuanGong.html',
                        'width': '800px',
                        'showSuccess': editYuanGong
                    });
                });
                
                //运营执照文件上传
                $('#YunYingZhiZhao').fileupload({ multi: false });
                //法人身份证照文件上传
                $('#FaRenShenFenZhengZhao').fileupload({ multi: false });
                

              
                if (userInfo.OrganizationType == "0") {
                    $('#add').click(function (e) {
                        e.preventDefault();
                        selectCity2.showSelectCity();

                    });
                }
                else {
                    var orgManageArea = helper.GetUserInfo().OrganizationManageArea;
                    if (typeof orgManageArea != "undefined" || orgManageArea != '') {
                        var manageArea = orgManageArea.split('|');
                        $('#add').click(function (e) {
                            e.preventDefault();
                            selectCity2.showSelectCity(manageArea);

                        });
                    }
                    else {
                        $('#add').click(function (e) {
                            e.preventDefault();
                            selectCity2.showSelectCity();

                        });
                    }
                }
                //endregion
            };
            //显示企业认证信息
            function showData(id) {
                getQiYeRenZhengXinXi(id, function (serviceData) {
                    if (serviceData.publicresponse.statuscode == 0) {
                        fillFormData(serviceData.body);
                        // 文件组件
                        var YunYingZhiZhaoId = $("#YunYingZhiZhaoId").val();
                        if (YunYingZhiZhaoId) {
                            $("#" + YunYingZhiZhaoId + "View").remove();
                        }
                        var FaRenShenFenZhengZhaoId = $("#FaRenShenFenZhengZhaoId").val();
                        if (FaRenShenFenZhengZhaoId) {
                            $("#" + FaRenShenFenZhengZhaoId + "View").remove();
                        }
                        fileupload.rebindFileButtonEdit(['YunYingZhiZhaoId', 'FaRenShenFenZhengZhaoId']);
                        if (serviceData.body["YunYingZhiZhaoId"]) {
                            $("#YunYingZhiZhao").remove();
                        }
                        if (serviceData.body["FaRenShenFenZhengZhaoId"]) {
                            $("#FaRenShenFenZhengZhao").remove();
                        }
                    } else {
                        tipdialog.errorDialog("请求数据失败");
                    }
                });
            }
            //获取数据
            function getQiYeRenZhengXinXi(id, callback) {
                //调用获取单条信息接口
                helper.Ajax("000000000004", id, function (resultdata) {
                    if (typeof callback == 'function') {
                        callback(resultdata);
                    }
                }, false);
            };
            //绑定数据
            function fillFormData(resource) {
                $('#QiYeRenZhengXinXiForm').find('input[name],select[name],textarea[name]').each(function (i, item) {
                    var tempValue = resource[$(item).attr('name')];
                    /*
                    if ($(item).attr('name') == "ShiFouAnZhuangShiPingZhongDuan") {
                        if (!resource[$(item).attr('name')]) {
                            $("#ShiPingTouGeShu").attr("disabled", "disabled");
                        }

                    }
                    */
                    if (tempValue != undefined) {
                        if ($(item).hasClass('datetimepicker')) {
                            tempValue = tempValue.substr(11, 5);
                        }
                        if ($(item).hasClass('datepicker')) {
                            tempValue = tempValue.substr(0, 10);
                        }
                        /*
                        if ($(item).attr('name') == "QiYeLeiXing" || $(item).attr('name') == "RenZhengZhuangTai") {
                            $(item).val(tempValue);
                        } else {
                            $(item).val(tempValue.toString() == '' ? '' : tempValue.toString());
                        }
                        */
                        $(item).val(tempValue.toString() == '' ? '' : tempValue.toString());
                        if ($(item).attr('name') == "XiaQuShi") {
                            selectcity.setXiaQu('00000020006', { "City": tempValue.toString() }, '#XiaQuXian', function () {
                                $("#XiaQuXian").val(resource.XiaQuXian);
                            }, 'GetDistrictList', 'DistrictName');
                        }
                    }
                });

                //设置下拉框的值
                if ($(this).siblings("select").length) {
                    var selectedOption = $(this).siblings("select").find("option");
                    $(selectedOption).each(function (i, item0) {
                        if (item0.value == tempValue) {
                            $(item).html(item0);
                        }
                    })
                };
                $('#Id').val(resource.Id);
            };

            //录入员工
            function addYuanGong() {
                //员工身份证照文件上传
                $('#YG_ShenFenZhengZhao').fileupload({ multi: false });

                //保存员工信息
                $("#btnSaveYuanGongAdd").on('click', function (e) {
                    e.preventDefault();
                    var flags = true;
                    var msg = '';
                    var formData = $('#FormYuanGong').serializeObject();
                    // 做非空校验
                    if ($.trim(formData.YG_XingMing) == '') {
                        msg += "员工姓名 是必填项<br/>";
                    }
                    if ($.trim(formData.YG_ShenFenZhengHao) == '') {
                        msg += "员工身份证号 是必填项<br/>";
                    }
                    if ($.trim(formData.YG_ShouJiHaoMa) == '') {
                        msg += "手机号码 是必填项<br/>";
                    }
                    if ($.trim(formData.YG_ZhiWu) == '') {
                        msg += "职务 是必填项<br/>";
                    }
                    if ($.trim(formData.YG_CongYeZiGeZhengHao) == '') {
                        msg += "从业资格证号 是必填项<br/>";
                    }
                    if ($.trim(formData.YG_ShuJuLaiYuan) == '') {
                        msg += "数据来源 是必填项<br/>";
                    }
                    if ($.trim(formData.YG_RenZhengZhuangTai) == '') {
                        msg += "认证状态 是必填项<br/>";
                    }
                    if ($.trim(formData.YG_ShenFenZhengZhaoId) == '') {
                        msg += "身份证照 是必填项<br/>";
                    }
                    // 通过非空校验则进行格式校验
                    if (msg == '') {
                        var telphoneReg = /^1\d{10}$/;
                        if (telphoneReg.test($.trim(formData.YG_ShouJiHaoMa)) == false) {
                            msg += "手机号码 格式不正确<br/>";
                        }
                        if (ValidIdentityCardNumber($.trim(formData.YG_ShenFenZhengHao)) == false) {
                            msg += "法人身份证号 格式不正确<br/>";
                        }
                    }
                    if (msg != '') {
                        flags = false;
                        tipdialog.alertMsg(msg);
                    }
                    if (flags) {
                        for (var key in formData) {
                            formData[key] = formData[key].replace(/\s/g, "");
                        }
                        var param = {
                            "YeHuID": id,
                            "XingMing": formData.YG_XingMing,
                            "ShenFenZhengHaoMa": formData.YG_ShenFenZhengHao,
                            "ShouJiHao": formData.YG_ShouJiHaoMa,
                            "ZhiWu": formData.YG_ZhiWu,
                            "CongYeZiGeZhengHao": formData.YG_CongYeZiGeZhengHao,
                            "SYS_ShuJuLaiYuan": formData.YG_ShuJuLaiYuan,
                            "RenZhengZhuangTai": formData.YG_RenZhengZhuangTai,
                            "ShenFenZhengZhaoId": formData.YG_ShenFenZhengZhaoId
                        };
                        //调用新增接口
                        helper.Ajax("000000000006", param, function (data) {
                            if ($.type(data) == "string") {
                                data = helper.StrToJson(data);
                            }
                            if (data.publicresponse.statuscode == 0) {
                                if (data.body) {
                                    toast.success("添加成功");
                                    $('.close').trigger('click');
                                    $("#btnSearchYuanGong").click();
                                }
                                else {
                                    tipdialog.alertMsg("添加失败");
                                }
                            }
                            else {
                                tipdialog.alertMsg(data.publicresponse.message);
                            }
                        }, false);
                    }
                });
            }
            //查看员工
            function viewYuanGong() {
                getYuanGongXinXi(viewYuanGongId, function (serviceData) {
                    if (serviceData.publicresponse.statuscode == 0) {
                        fillFormDataYuanGong(serviceData.body);
                        // 文件组件
                        var YG_ShenFenZhengZhaoId = $("#YG_ShenFenZhengZhaoId").val();
                        if (YG_ShenFenZhengZhaoId) {
                            $("#" + YG_ShenFenZhengZhaoId + "View").remove();
                        }
                        fileupload.rebindFileButtonView(['YG_ShenFenZhengZhaoId']);
                    } else {
                        tipdialog.errorDialog("请求数据失败");
                    }
                });
            }
            //修改员工
            function editYuanGong() {
                getYuanGongXinXi(viewYuanGongId, function (serviceData) {
                    if (serviceData.publicresponse.statuscode == 0) {
                        fillFormDataYuanGong(serviceData.body);
                        // 文件组件
                        var YG_ShenFenZhengZhaoId = $("#YG_ShenFenZhengZhaoId").val();
                        if (YG_ShenFenZhengZhaoId) {
                            $("#" + YG_ShenFenZhengZhaoId + "View").remove();
                        }
                        fileupload.rebindFileButtonEdit(['YG_ShenFenZhengZhaoId']);
                        if (serviceData.body["YG_ShenFenZhengZhaoId"]) {
                            $("#YG_ShenFenZhengZhao").remove();
                        }
                    } else {
                        tipdialog.errorDialog("请求数据失败");
                    }
                });

                //保存员工信息
                $("#btnSaveYuanGongEdit").on('click', function (e) {
                    e.preventDefault();
                    var flags = true;
                    var msg = '';
                    var formData = $('#FormYuanGong').serializeObject();
                    // 做非空校验
                    if ($.trim(formData.YG_XingMing) == '') {
                        msg += "员工姓名 是必填项<br/>";
                    }
                    if ($.trim(formData.YG_ShenFenZhengHao) == '') {
                        msg += "员工身份证号 是必填项<br/>";
                    }
                    if ($.trim(formData.YG_ShouJiHaoMa) == '') {
                        msg += "手机号码 是必填项<br/>";
                    }
                    if ($.trim(formData.YG_ZhiWu) == '') {
                        msg += "职务 是必填项<br/>";
                    }
                    if ($.trim(formData.YG_CongYeZiGeZhengHao) == '') {
                        msg += "从业资格证号 是必填项<br/>";
                    }
                    if ($.trim(formData.YG_ShuJuLaiYuan) == '') {
                        msg += "数据来源 是必填项<br/>";
                    }
                    if ($.trim(formData.YG_RenZhengZhuangTai) == '') {
                        msg += "认证状态 是必填项<br/>";
                    }
                    if ($.trim(formData.YG_ShenFenZhengZhaoId) == '') {
                        msg += "身份证照 是必填项<br/>";
                    }
                    // 通过非空校验则进行格式校验
                    if (msg == '') {
                        var telphoneReg = /^1\d{10}$/;
                        if (telphoneReg.test($.trim(formData.YG_ShouJiHaoMa)) == false) {
                            msg += "手机号码 格式不正确<br/>";
                        }
                        if (ValidIdentityCardNumber($.trim(formData.YG_ShenFenZhengHao)) == false) {
                            msg += "法人身份证号 格式不正确<br/>";
                        }
                    }
                    if (msg != '') {
                        flags = false;
                        tipdialog.alertMsg(msg);
                    }
                    if (flags) {
                        for (var key in formData) {
                            formData[key] = formData[key].replace(/\s/g, "");
                        }
                        var param = {
                            "Id": viewYuanGongId,
                            "YeHuID": id,
                            "XingMing": formData.YG_XingMing,
                            "ShenFenZhengHaoMa": formData.YG_ShenFenZhengHao,
                            "ShouJiHao": formData.YG_ShouJiHaoMa,
                            "ZhiWu": formData.YG_ZhiWu,
                            "CongYeZiGeZhengHao": formData.YG_CongYeZiGeZhengHao,
                            "SYS_ShuJuLaiYuan": formData.YG_ShuJuLaiYuan,
                            "RenZhengZhuangTai": formData.YG_RenZhengZhuangTai,
                            "ShenFenZhengZhaoId": formData.YG_ShenFenZhengZhaoId
                        };
                        //调用修改接口
                        helper.Ajax("000000000008", param, function (data) {
                            if ($.type(data) == "string") {
                                data = helper.StrToJson(data);
                            }
                            if (data.publicresponse.statuscode == 0) {
                                if (data.body) {
                                    toast.success("修改成功");
                                    $('.close').trigger('click');
                                    $("#btnSearchYuanGong").click();
                                }
                                else {
                                    tipdialog.alertMsg("修改失败");
                                }
                            }
                            else {
                                tipdialog.alertMsg(data.publicresponse.message);
                            }
                        }, false);
                    }
                });
            }
            //获取数据
            function getYuanGongXinXi(id, callback) {
                //调用获取单条信息接口
                helper.Ajax("000000000005", id, function (resultdata) {
                    if (typeof callback == 'function') {
                        callback(resultdata);
                    }
                }, false);
            };
            //绑定数据
            function fillFormDataYuanGong(resource) {
                $('#FormYuanGong').find('input[name],select[name],textarea[name]').each(function (i, item) {
                    $(item).val('');
                    var tempValue = resource[$(item).attr('name')];
                    if (tempValue != undefined) {
                        $(item).val(tempValue.toString() == '' ? '' : tempValue);
                    } else {
                        $(item).val('');
                    }
                });
                $('#FormYuanGong').find('.form-control-static').each(function (i, item) {
                    $(item).html('');
                    var index = $(item).attr('for');
                    var tempValue = resource[index];
                    if (tempValue != undefined) {
                        $(item).html(tempValue == '' ? '' : tempValue);
                    } else {
                        $(item).html('');
                    }
                });
                $('#Id').val(resource.Id);
            };


            //列表初始化
            function initlizableTable() {
                $("#tb_Template").CustomTable({
                    ajax: helper.AjaxData("000000000002",
                        function (data) {
                            var pageInfo = {
                                Page: parseInt(data.start / data.length + 1),
                                Rows: data.length
                            };
                            for (var i in data) {
                                delete data[i];
                            }
                            /*
                            var para = $('.searchpanel-form').serializeObject();
                            $('.searchpanel-form').find('[disabled]').each(function (i, item) {
                                para[$(item).attr('name')] = $(item).val();
                            });
                            */
                            var para = {
                                QiYeId: window.parent.document.getElementById('hdIDS').value,
                                YuanGongBianHao: $('#YuanGongBianHao').val(),
                                YuanGongXingMing: $('#YuanGongXingMing').val(),
                                RenZhengZhuangTai: $('#YuanGongRenZhengZhuangTai').val()
                            };
                            pageInfo.data = para;
                            $.extend(data, pageInfo);
                        }, null),
                    single: false,
                    filter: true,
                    ordering: true, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [{
                        render: function (data, type, row) {
                            return '<input type=checkbox class=checkboxes />';
                        }
                    },
                        { data: 'YuanGongBianHao' },
                        {
                            data: 'ZhiWu',
                            render: function (data, type, row) {
                                switch (data) {
                                    case 1:
                                        return "企业法人";
                                        break;
                                    case 2:
                                        return "管理人员";
                                        break;
                                    case 3:
                                        return "工作人员";
                                        break;
                                    case 4:
                                        return "司机";
                                        break;
                                    default:
                                        return "";
                                }
                            }
                        },
                        { data: 'YuanGongXingMing' },
                        { data: 'ShenFenZhengHao' },
                        { data: 'ShouJiHaoMa' },
                        { data: 'QiYeZhangHao' },
                        { data: 'ShuJuLaiYuan' },
                        {
                            data: 'ChuangJianShiJian',
                            render: function (data, type, row) {
                                if (!data) {
                                    return "";
                                } else {
                                    //return data.replace(/T/g, " ");
                                    return new Date(data.toString()).Format("yyyy-MM-dd HH:mm:ss");
                                }
                            }
                        },
                        {
                            data: 'RenZhengZhuangTai',
                            render: function (data, type, row) {
                                switch (data) {
                                    case 1:
                                        return "认证中";
                                        break;
                                    case 2:
                                        return "未认证";
                                        break;
                                    case 3:
                                        return "已认证";
                                        break;
                                    default:
                                        return "";
                                }
                            }
                        }
                    ],
                    //paging: false,
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        $("#tb_CheLiangXinXi_filter").css("z-index", "1000");
                        tableheadfix.ResetFix();
                    }
                });
                tableheadfix.InitFix(system.OnlyTableFix);
            };
            //级联城市下拉框
            function selectCity() {
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#XiaQuShi, #XiaQuXian').empty().append(defaultOption);

                var data = { "Province": "广东" };
                ///调用接口初始化城市下拉框
                selectcity.setXiaQu('00000020005', data, '#XiaQuShi', function () {
                    var XiaQuShi = userInfo.OrganizationManageArea;
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
                        selectcity.setXiaQu('00000020006', data, '#XiaQuXian', function () {
                            $("#XiaQuXian").val(XiaQuXian);
                        }, 'GetDistrictList', 'DistrictName');
                    }
                });

                $('#XiaQuXian').change(function () {
                    var data = { "District": $(this).val() };
                });
            }

            function ValidIdentityCardNumber(idCard) {
                idCard = $.trim(idCard);
                if (idCard.length == 15) {
                    if (isValidityBrithBy15IdCard(idCard)) {
                        return true;
                    } else {
                        return false;
                    }
                } else if (idCard.length == 18) {
                    var a_idCard = idCard.split("");
                    if (isValidityBrithBy18IdCard(idCard) && isTrueValidateCodeBy18IdCard(a_idCard)) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
            }

            function isValidityBrithBy18IdCard(idCard18) {
                var year = idCard18.substring(6, 10);
                var month = idCard18.substring(10, 12);
                var day = idCard18.substring(12, 14);
                var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
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
                if (temp_date.getYear() != parseFloat(year)
                    || temp_date.getMonth() != parseFloat(month) - 1
                    || temp_date.getDate() != parseFloat(day)) {
                    return false;
                } else {
                    return true;
                }
            }
            function isTrueValidateCodeBy18IdCard(a_idCard) {
                var Wi = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1];
                var ValideCode = [1, 0, 10, 9, 8, 7, 6, 5, 4, 3, 2];
                var sum = 0;
                if (a_idCard[17].toLowerCase() == 'x') {
                    a_idCard[17] = 10;
                }
                for (var i = 0; i < 17; i++) {
                    sum += Wi[i] * a_idCard[i];
                }
                valCodePosition = sum % 11;
                if (a_idCard[17] == ValideCode[valCodePosition]) {
                    return true;
                } else {
                    return false;
                }
            }

            //保存
            function save(id) {
                var formData = $('#QiYeRenZhengXinXiForm').serializeObject();
                for (var key in formData) {
                    formData[key] = formData[key].replace(/\s/g, "");
                }
                var param = {
                    "Id": id,
                    "YeHuMingCheng": formData.QiYeMingCheng,
                    "TongYiSheHuiXinYongDaiMa": formData.TongYiSheHuiXinYongDaiMa,
                    "DiZhi": formData.YingYunZhiZhaoDiZhi,
                    "YingYeZhiZhaoSaoMiaoJianId": formData.YunYingZhiZhaoId,
                    "FaDingDaiBiaoRen": formData.FaRenXingMing,
                    "FaDingDaiBiaoRenShenFenZhengHao": formData.FaRenShenFenZhengHao,
                    "FaDingDaiBiaoRenLianXiDianHua": formData.FaRenShouJiHaoMa,
                    "FaDingDaiBiaoRenShenFenZhengId": formData.FaRenShenFenZhengZhaoId,
                    "YeHuXingZhi": formData.QiYeLeiXing,
                    "RenZhengZhuangTai": formData.RenZhengZhuangTai,
                    "SYS_ShuJuLaiYuan": formData.ShuJuLaiYuan,
                    "XiaQuSheng": formData.XiaQuSheng,
                    "XiaQuShi": formData.XiaQuShi,
                    "XiaQuXian": formData.XiaQuXian
                };
                //调用新增接口
                helper.Ajax("000000000007", param, function (data) {
                    if ($.type(data) == "string") {
                        data = helper.StrToJson(data);
                    }
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {
                            tipdialog.confirm("修改成功，是否修改员工信息？", function (r) {
                                if (r) {

                                } else {
                                    parent.window.$("#btnSearch").click();
                                    setTimeout(function () { popdialog.closeIframe(); }, 1000);
                                }
                            });
                        }
                        else {
                            tipdialog.alertMsg("修改失败");
                        }
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);
            };


            //endregion
            initPage();
        });


});
