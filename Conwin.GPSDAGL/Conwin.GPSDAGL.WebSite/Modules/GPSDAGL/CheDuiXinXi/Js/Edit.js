define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'selectCity2', 'filelist', 'metronic', 'dropdown', 'fileupload',  'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, selectCity2, filelist, Metronic, dropdown, fileupload) {
            //模块初始化
            var userInfo = helper.GetUserInfo();
            var positionTypeArr = [];

            var initPage = function () {
                getPersonalPosition();

                //初始化页面样式
                //common.AutoFormScrollHeight('#Form1');
                common.AutoFormScrollHeight('#Form1', function (hg) {
                    var boxHeight = hg - $('.portlet-title').outerHeight(true) - $('.nav-tabs').outerHeight(true) - 245;
                    var me = $(".scroller", '#Form1').eq(0);
                    me.parent().css('height', boxHeight);
                    me.css('height', boxHeight);
                });
                common.AutoFormScrollHeight('#LianXiXinXi', function (hg) {
                    var boxHeight = hg - $('.portlet-title').outerHeight(true) - $('.nav-tabs').outerHeight(true) - 245;
                    var me = $(".scroller");
                    me.parent().css('height', boxHeight);
                    me.css('height', boxHeight);
                });
                common.AutoFormScrollHeight('#JiaShiYuanXinXi', function (hg) {
                    var boxHeight = hg - $('.portlet-title').outerHeight(true) - $('.nav-tabs').outerHeight(true) - 245;
                    var me = $(".scroller");
                    me.parent().css('height', boxHeight);
                    me.css('height', boxHeight);
                });
                formcontrol.initial();
                //翻页控件
                var ids = window.parent.document.getElementById('hdIDS').value;
                prevNextpage.initPageInfo(ids.split(','));
                prevNextpage.bindPageClass();

                //关闭
                $('#btnclose').click(function () {
                    tipdialog.confirm("确定关闭？", function (r) {
                        if (r) {
                            parent.window.$("#btnSearch").click();
                            popdialog.closeIframe();
                        }
                    });
                });

                //上一条
                $('#prevBtn').click(function (e) {
                    e.preventDefault();
                    prevNextpage.prev();
                    updateData();
                });

                //下一条
                $('#nextBtn').click(function (e) {
                    e.preventDefault();
                    prevNextpage.next();
                    updateData();
                });

                //初始化联系人表格
                $('#tab2').on('click', function () {
                    $("#tb_lianXiRenTable").CustomTable("reload");
                });
                $('#tab3').on('click', function () {
                    $("#tb_JiaShiYuanTable").CustomTable("reload");
                });

                updateData();
                imageLoad();
                //保存
                $('#saveBtn').on('click', function (e) {
                    e.preventDefault();
                    var flags = true;
                    var msg = '';
                    var fromData = $('#Form1').serializeObject();
                    fromData.YouXiaoZhuangTai = $('#YouXiaoZhuangTai').val();
                    if ($.trim(fromData.JiGouMingCheng) == '') {
                        msg += "机构名称 是必填项<br/>";
                    }
                    if ($.trim(fromData.JingYingQuYu) == '') {
                        msg += "经营区域 是必填项<br/>";
                    }
                    if (msg != '') {
                        flags = false;
                        tipdialog.alertMsg(msg);
                    }
                    if (flags) {
                        save();
                    }
                });

                selectCity2.initSelectView($('#JingYingQuYu'));
                var userInfo = helper.GetUserInfo();
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
                initlizableLianXiRenTable();
                initlizableJiaShiYuanTable();
            };

            function imageLoad() {
                $('#imgUpLoad').fileupload({
                    businessId: $('#Id').val(),
                    multi: false,
                    timeOut: 20000,
                    allowedContentType: 'png|jpg|jpeg|tif|gif|pdf',
                    callback: function (data) {
                        $("#CheDuiBiaoZhiId").val(data.FileId);
                        $('#' + data.FileId + 'View').hide();
                        $('#' + data.FileId + 'Delete').hide();
                        $('#divImgUpLoad').append("<img id='imgUpLoad' src='" + helper.Route('00000080004', '1.0', system.ServerAgent) + '?id=' + data.FileId + "' style='width:200px;height:200px;float:left;padding:6px;' />");
                        imageLoad();
                    }
                });
            }

            //主表-刷新数据
            function updateData() {
                var id = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                getXianLuXinXi(id, function (serviceData) {
                    if (serviceData.publicresponse.statuscode == 0) {
                        fillFormData(serviceData.body);
                        $("#tb_lianXiRenTable").CustomTable("reload");
                        $("#tb_JiaShiYuanTable").CustomTable("reload");
                    } else {
                        tipdialog.errorDialog("请求数据失败");
                    }
                });
            };

            //主表-更新tab状态
            function updateTag() {
                $('#tab1').parent('li').addClass('active');
                $('#JiChuXinXi').addClass('active in');
                $('#tab2').parent('li').removeClass('active');
                $('#LianXiXinXi').removeClass('active in');
                $('#tab3').parent('li').removeClass('active');
                $('#JiaShiYuanXinXi').removeClass('active in');
            };

            //主表-获取主表数据
            function getXianLuXinXi(id, callback) {
                helper.Ajax("003300300512", id, function (resultdata) {
                    if (typeof callback == 'function') {
                        callback(resultdata);
                    }
                }, false);
            };

            //主表-绑定主表数据
            function fillFormData(resource) {
                $('#Form1').find('input[name],select[name],textarea[name]').each(function (i, item) {
                    $(item).val('');
                    var tempValue = resource[$(item).attr('name')];
                    if (tempValue != undefined) {
                        //TODO: 赋值
                        $(item).val(tempValue);
                    } else {
                        $(item).val('');
                    }
                });
                if ($('#CheDuiBiaoZhiId').val() != '') {
                    var url = helper.Route('00000080004', '1.0', system.ServerAgent) + '?id=' + $('#CheDuiBiaoZhiId').val();
                    $('#imgUpLoad').attr("src", url);
                } else {
                    $('#imgUpLoad').attr("src", '../../Component/Img/NotPic.jpg');
                }

                if ($("#YouXiaoZhuangTai").val() == 1) {
                    $("#spYouXiaoZhuangTai").html("正常营业");
                    $("#YouXiaoZhuangTaiInput").val("正常营业")
                }
                else {
                    $("#spYouXiaoZhuangTai").html("合约到期");
                    $("#YouXiaoZhuangTaiInput").val("合约到期")
                }

                if ($("#ShenHeZhuangTai").val() == 1) {
                    $("#ShenHeZhuangTaiInput").val("待提交")
                }
                else if ($("#ShenHeZhuangTai").val() == 2) {
                    $("#ShenHeZhuangTaiInput").val("待审核")
                }
                else if ($("#ShenHeZhuangTai").val() == 3) {
                    $("#ShenHeZhuangTaiInput").val("审核通过")
                }
                else if ($("#ShenHeZhuangTai").val() == 4) {
                    $("#ShenHeZhuangTaiInput").val("审核不通过")
                }

                selectCity2.setData(resource['JingYingQuYu']);
                $("#jigoumingcheng").html($("#JiGouMingCheng").val());
                $("#fuzeren").html($("#FuZheRen").val());
                $("#fuzerendianhua").html($("#FuZheRenDianHua").val());
            };

            function initTopFuZeRenXinXi(lianXiRenData) {
                if (lianXiRenData == "" || lianXiRenData == null) {
                    $("#fuzeren").html("");
                    $("#fuzerendianhua").html("");
                }
                else {
                    $("#fuzeren").html(lianXiRenData.LianXiRen);
                    $("#fuzerendianhua").html(lianXiRenData.ShouJiHaoMa);
                }
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
                                    data = data.split(',');
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
            //初始化驾驶员表格
            function initlizableJiaShiYuanTable() {
                $("#tb_JiaShiYuanTable").CustomTable({
                    ajax: helper.AjaxData("003300300157",
                        function (data) {
                            var pageInfo = { Page: data.start / data.length + 1, Rows: data.length };
                            for (var i in data) {
                                delete data[i];
                            }
                            var para = new Object();
                            para["BenDanWeiOrgCode"] = $("#BenDanWeiOrgCode").val();
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
                        { data: 'XingMing' },
                        { data: 'XingBie' },
                        { data: 'ShenFenZhengHaoMa' },
                        { data: 'CongYeZiGeZhengHao' },
                        { data: 'YiDongDianHua' },
                        { data: 'XiaQu' },
                        {
                            data: 'ZhaoPian',
                            render: function (data, type, row) {
                                result = '<a class="btn blue ViewZhaoPian" style="padding: 5px" >' + data + '</a>';
                                return result;
                            }
                        },
                    ],
                    pageLength: 10,
                    "fnDrawCallback": function () {
                        tableButtonInit();
                        tableheadfix.ResetFix();
                    },
                });
            };
            function tableButtonInit() {
                //查看驾驶员照片
                $(".ViewZhaoPian").click(function () {
                    var eventData = $('#tb_JiaShiYuanTable').DataTable().row($(this).parents('tr')).data();
                    helper.Ajax("003300300158", eventData.Id, function (data) {
                        if (data.publicresponse.statuscode == 0) {
                            if (data.body != "" || data.body != null) {
                                sessionStorage.setItem("_JiaShiYuanZhaoPian", data.body);
                                sessionStorage.setItem("_JiaShiYuanXingMing", eventData.XingMing);
                                popdialog.showModal({
                                    'url': '../JiaShiYuanXinXi/JiaShiYuanZhaoPian.html',
                                    'width': '420px',
                                    'showSuccess': showJiaShiYuanZhaoPian
                                });
                            }
                            else {
                                tipdialog.alertMsg("该驾驶员无照片");
                            }
                        }
                        else {
                            tipdialog.alertMsg(data.publicresponse.message);
                        }
                    }, false);
                });
            }
            function showJiaShiYuanZhaoPian() {
                $("#JiaShiYuanZhaoPian").attr("src", sessionStorage.getItem("_JiaShiYuanZhaoPian"));
                $("#JiaShiYuanXingMing").text(sessionStorage.getItem("_JiaShiYuanXingMing"))
            }

            //主表-保存
            function save() {
                //TODO:数据校验
                var jsonData1 = $('#Form1').serializeObject();
                jsonData1.BeiZhu = $('#BeiZhu').val();
                for (var key in jsonData1) {
                    jsonData1[key] = jsonData1[key].replace(/\s/g, "");
                }
                //调用修改接口
                helper.Ajax("003300300514", jsonData1, function (data) {
                    if ($.type(data) == "string") {
                        data = helper.StrToJson(data);
                    }
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {
                            toast.success("档案修改成功");
                            window.parent.document.getElementById('hdIDS').value = jsonData1.Id;
                            setTimeout(function () { window.location.reload(false); }, 2000);
                        }
                        else {
                            tipdialog.alertMsg("档案修改失败");
                        }
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);
            };

            //车队标志-列表初始化
            //子表-初始化分页信息
            var tabPageInfo = tabPage();
            //子表-初始化校验信息
            var tabButtonInfo = tabButton();
            //子表-分页
            function tabPage() {
                var tabPageInfo = {};
                tabPageInfo.bindPageClass = function () {
                    var currentPageInfo = tabPageInfo.PageInfo;
                    if (currentPageInfo.HasNext) {
                        $('#nextTabBtn').removeClass('disabled');
                        $('#nextTabBtn').removeClass('c-gray-btn');
                        $('#nextTabBtn').removeAttr('disabled');
                        $('#nextTabBtn').addClass('c-green');
                        $('#nextTabBtn').children(':first').removeClass('m-icon-gray');
                        $('#nextTabBtn').children(':first').addClass('m-icon-white');
                    } else {
                        $('#nextTabBtn').addClass('disabled');
                        $('#nextTabBtn').addClass('c-gray-btn');
                        $('#nextTabBtn').attr("disabled", "disabled");
                        $('#nextTabBtn').removeClass('c-green');
                        $('#nextTabBtn').children(':first').addClass('m-icon-gray');
                        $('#nextTabBtn').children(':first').removeClass('m-icon-white');
                    }
                    if (currentPageInfo.HasPrev) {
                        $('#prevTabBtn').removeClass('disabled');
                        $('#prevTabBtn').removeClass('c-gray-btn');
                        $('#prevTabBtn').removeAttr('disabled');
                        $('#prevTabBtn').addClass('c-green');
                        $('#prevTabBtn').children(':first').removeClass('m-icon-gray');
                        $('#prevTabBtn').children(':first').addClass('m-icon-white');
                    } else {
                        $('#prevTabBtn').addClass('disabled');
                        $('#prevTabBtn').addClass('c-gray-btn');
                        $('#prevTabBtn').attr("disabled", "disabled");
                        $('#prevTabBtn').removeClass('c-green');
                        $('#prevTabBtn').children(':first').addClass('m-icon-gray');
                        $('#prevTabBtn').children(':first').removeClass('m-icon-white');
                    }
                };
                //分页信息
                tabPageInfo.PageInfo = {
                    IDS: [],
                    Index: 0,
                    PageSize: 0,
                    HasPrev: false,
                    HasNext: false
                };
                //初始化子页面记录数据
                tabPageInfo.initPageInfo = function (ids) {
                    tabPageInfo.PageInfo.IDS = ids;
                    tabPageInfo.PageInfo.Index = 0;
                    tabPageInfo.PageInfo.PageSize = ids.length;
                    tabPageInfo.PageInfo.HasNext = ids.length > 1;
                    tabPageInfo.PageInfo.HasPrev = false;
                };
                //计算分页信息
                tabPageInfo.calculatePage = function (tag) {
                    if (tag == undefined)
                        return tabPageInfo.PageInfo;
                    //标识
                    if (tag > 0) {
                        tabPageInfo.PageInfo.Index++;
                    }
                    else {
                        tabPageInfo.PageInfo.Index--;
                    }
                    tabPageInfo.PageInfo.HasNext = tabPageInfo.PageInfo.PageSize > (tabPageInfo.PageInfo.Index + 1);
                    tabPageInfo.PageInfo.HasPrev = tabPageInfo.PageInfo.Index > 0;
                    return tabPageInfo.PageInfo;
                };
                tabPageInfo.next = function () {
                    tabPageInfo.calculatePage(1);
                    tabPageInfo.bindPageClass();
                };
                tabPageInfo.prev = function () {
                    tabPageInfo.calculatePage(-1);
                    tabPageInfo.bindPageClass();
                };
                return tabPageInfo;
            }

            //子表-必填项校验
            function tabButton() {
                var button = {};
                var defaults = {};
                ///初始化
                button.initial = function (elemt, options) {
                    var param;
                    var message = '';
                    var additionMsg = '';
                    var focusId = undefined;
                    param = $.extend({}, defaults, options);
                    return button.checkItem(elemt, message, additionMsg, focusId, param);
                }
                ///提交驗證
                button.checkItem = function (elemt, message, additionMsg, focusId, param) {
                    $(elemt).find('input[name],select[name],textarea[name]').each(function (i, item) {
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
                                            $(item).parent('div').addClass('data-box-red');
                                            $(item).parent('div').css({
                                                'float': 'left',
                                                'padding': '3px 12px 3px 8px'
                                            });
                                        } else {
                                            $(item).parent('div').removeClass('data-box-red');
                                            $(item).parent('div').css('float', '');
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
                            tipdialog.errorDialog(message);
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
            }

            //个性化代码块
            //-------------弹框新增方法star--------------
            $('#btnCreate').on('click', function (e) {
                e.preventDefault();
                popdialog.showModal({
                    'url': '../PersonalInfo/AddModal.html',
                    'width': '900px',
                    'showSuccess': initAddPersonalInfo
                });
            });
            $('#btnCreateJiaShiYuan').on('click', function (e) {
                e.preventDefault();
                popdialog.showModal({
                    'url': '../JiaShiYuanXinXi/JiaShiYuanAdd.html',
                    'width': '900px',
                    'showSuccess': initAddJiaShiYuan
                });
            });
            function initAddJiaShiYuan() {
                formcontrol.initial();
                setTimeout(function () {
                    if ($("#ZhaoPianId").val() == "") {
                        if ($("#InitZhaoPian").val() == "") {
                            $("#ZhaoPianYuLan").attr("src", "../../Component/Img/NotPic.jpg");
                        } else {
                            $("#ZhaoPianYuLan").attr("src", $("#InitZhaoPian").val());
                        }
                    } else {
                        var href = $("#ZhaoPianShangChuan a:first").attr('href');
                        getBase64(href)
                            .then(function (base64) {
                                $("#ZhaoPianYuLan").attr("src", base64);
                            }, function (err) {
                                console.log(err);
                            });
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
                        allowedContentType: 'png|jpg|jpeg'
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

                $("#AddJiaShiYuanSure").on('click', function (e) {
                    e.preventDefault();
                    var code = $('#BenDanWeiOrgCode').val();
                    var param = {}
                    param.BenDanWeiOrgCode = code;
                    param.CongYeRenYuanId = $("#CongYeRenYuanId").val();
                    param.XingMing = $("#XingMing").val();
                    param.XingBie = $("#XingBie").val();
                    param.CongYeZiGeZhengHao = $("#CongYeZiGeZhengHao").val();
                    param.ShenFenZhengHaoMa = $("#ShenFenZhengHaoMa").val();
                    param.YiDongDianHua = $("#YiDongDianHua").val();
                    param.XiaQuSheng = $("#JSYXiaQuSheng").val();
                    param.XiaQuShi = $("#JSYXiaQuShi").val();
                    param.XiaQuXian = $("#JSYXiaQuXian").val();
                    param.ZhaoPian = $("#ZhaoPianId").val();
                    param.ZhaoPianBase64 = $("#ZhaoPianYuLan").attr("src");
                    // 校验数据
                    var errorMsg = "";
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
                    // 新增驾驶员信息
                    helper.Ajax("003300300156", param, function (data) {
                        if (data.body) {
                            toast.success("新增成功");
                            popdialog.closeModal();
                            $('#tab3').click();
                        }
                        else {
                            tipdialog.alertMsg(data.publicresponse.message);
                        }
                    }, false);
                });

            };
            function initAddLianXiRen() {
                formcontrol.initial();
                $('#AddZiBiaoSure').on('click', function (e) {
                    e.preventDefault();
                    var code = $('#BenDanWeiOrgCode').val();
                    var jsonData1 = $('#SelectForm').serializeObject();
                    jsonData1.BenDanWeiOrgCode = code;
                    if (!CheckSubmit(jsonData1)) {

                        return;
                    }
                    if (tabButtonInfo.initial('#SelectForm')) {
                        addZiBiaoGuanXi0(jsonData1, function () {
                            if (jsonData1.LeiBie == "2") {
                                initTopFuZeRenXinXi(jsonData1);
                            }
                            $('#tab2').click();
                            popdialog.closeModal();
                        });
                    }
                });
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

            function addZiBiaoGuanXi0(array, callback) {
                helper.Ajax("003300300010", array, function (data) {
                    if (data.body) {
                        toast.success("新增成功");
                        if (typeof callback == 'function') {
                            callback();
                        }
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);
            };
            //-------------弹框新增方法end--------------
            //-------------弹框修改方法star--------------
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
                //TODO:编写逻辑
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
            $('#btnEditJiaShiYuan').on('click', function (e) {
                e.preventDefault();
                var rows = $("#tb_JiaShiYuanTable").CustomTable('getSelection'), ids = [];
                if (rows == undefined) {
                    tipdialog.errorDialog('请选择需要修改的记录');
                    return false;
                }
                if (rows.length > 1) {
                    tipdialog.errorDialog('每次只能修改一条记录');
                    return false;
                }
                //TODO:编写逻辑
                $(rows).each(function (i, item) {
                    ids.push(item.data.Id);
                });
                sessionStorage.setItem("_SelectedJiaShiYuan", ids[0]);
                popdialog.showModal({
                    'url': '../JiaShiYuanXinXi/JiaShiYuanEdit.html',
                    'width': '900px',
                    'showSuccess': initEditJiaShiYuan
                });
            });

            function initEditJiaShiYuan() {
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
                // 获取驾驶员信息
                var id = sessionStorage.getItem("_SelectedJiaShiYuan");
                helper.Ajax("003300300160", id, function (data) {
                    if (data.body) {
                        var model = data.body;
                        $("#CongYeRenYuanId").val(model.CongYeRenYuanId);
                        $("#XingMing").val(model.XingMing);
                        $("#XingBie").val(model.XingBie);
                        $("#CongYeZiGeZhengHao").val(model.CongYeZiGeZhengHao);
                        $("#ShenFenZhengHaoMa").val(model.ShenFenZhengHaoMa);
                        $("#YiDongDianHua").val(model.YiDongDianHua);
                        $("#ZhaoPianYuLan").attr("src", model.ZhaoPianBase64);
                        $("#InitZhaoPian").val(model.ZhaoPianBase64);
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
                setTimeout(function () {
                    if ($("#ZhaoPianId").val() == "") {
                        if ($("#InitZhaoPian").val() == "") {
                            $("#ZhaoPianYuLan").attr("src", "../../Component/Img/NotPic.jpg");
                        } else {
                            $("#ZhaoPianYuLan").attr("src", $("#InitZhaoPian").val());
                        }
                    } else {
                        var href = $("#ZhaoPianShangChuan a:first").attr('href');
                        getBase64(href)
                            .then(function (base64) {
                                $("#ZhaoPianYuLan").attr("src", base64);
                            }, function (err) {
                                console.log(err);
                            });
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
                // 修改驾驶员信息
                $("#EditJiaShiYuanSure").on('click', function (e) {
                    e.preventDefault();
                    var code = $('#BenDanWeiOrgCode').val();
                    var param = {}
                    param.Id = sessionStorage.getItem("_SelectedJiaShiYuan");
                    param.BenDanWeiOrgCode = code;
                    param.CongYeRenYuanId = $("#CongYeRenYuanId").val();
                    param.XingMing = $("#XingMing").val();
                    param.XingBie = $("#XingBie").val();
                    param.CongYeZiGeZhengHao = $("#CongYeZiGeZhengHao").val();
                    param.ShenFenZhengHaoMa = $("#ShenFenZhengHaoMa").val();
                    param.YiDongDianHua = $("#YiDongDianHua").val();
                    param.XiaQuSheng = $("#JSYXiaQuSheng").val();
                    param.XiaQuShi = $("#JSYXiaQuShi").val();
                    param.XiaQuXian = $("#JSYXiaQuXian").val();
                    param.ZhaoPian = $("#ZhaoPianId").val();
                    param.ZhaoPianBase64 = $("#ZhaoPianYuLan").attr("src");
                    // 校验数据
                    var errorMsg = "";
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
                        errorMsg += "辖区市为必填项</br>"
                    }
                    if (errorMsg != "") {
                        tipdialog.alertMsg(errorMsg);
                        return;
                    }
                    // 修改驾驶员信息
                    helper.Ajax("003300300161", param, function (data) {
                        if (data.body) {
                            toast.success("修改成功");
                            popdialog.closeModal();
                            $('#tab3').click();
                        }
                        else {
                            tipdialog.alertMsg(data.publicresponse.message);
                        }
                    }, false);
                });
            }
            function initEditLianXiRen() {
                formcontrol.initial();
                //翻页控件
                tabPageInfo.initPageInfo($('#SelectData').val().split(','));
                tabPageInfo.bindPageClass();
                viewZiBiaoGuanXi0();
                //上一条
                $('#prevTabBtn').click(function (e) {
                    e.preventDefault();
                    tabPageInfo.prev();
                    viewZiBiaoGuanXi0();
                });
                var leiBie = $('#LeiBie').val();

                $('#EditZiBiaoSure').on('click', function (e) {
                    e.preventDefault();
                    var jsonData1 = $('#SelectForm').serializeObject();
                    jsonData1.Id = tabPageInfo.PageInfo.IDS[tabPageInfo.PageInfo.Index];
                    jsonData1.BenDanWeiOrgCode = $('#BenDanWeiOrgCode').val();

                    if (!CheckSubmit(jsonData1)) {

                        return;
                    }
                    if (tabButtonInfo.initial('#SelectForm')) {
                        editZiBiaoGuanXi0(jsonData1, function () {
                            if (jsonData1.LeiBie == "2") {
                                initTopFuZeRenXinXi(jsonData1);
                            }
                            else {
                                if ($('#preLeiBie').val() == "2") {
                                    if (jsonData1.LeiBie != "2") {
                                        initTopFuZeRenXinXi();
                                    }
                                }
                            }
                            $('#tab2').click();
                            popdialog.closeModal();
                        });
                    }
                });
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

            function viewZiBiaoGuanXi0() {
                //TODO: 添加逻辑
                var pageId = tabPageInfo.PageInfo.IDS[tabPageInfo.PageInfo.Index];
                helper.Ajax("003300300013", pageId, function (resultdata) {
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
                                //TODO: 赋值
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

            function editZiBiaoGuanXi0(arry, callback) {
                //TODO: 添加逻辑
                helper.Ajax("003300300011", arry, function (data) {
                    if (data.body) {
                        toast.success("保存成功");
                        if (typeof callback == 'function') {
                            callback();
                        }
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);
            };
            //-------------弹框修改方法end--------------

            //-------------删除方法star----------------
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
            $('#btnDelJiaShiYuan').on('click', function (e) {
                e.preventDefault();
                var rows = $("#tb_JiaShiYuanTable").CustomTable('getSelection'), ids = [];
                if (rows == undefined) {
                    tipdialog.errorDialog('请选择需要操作的行');
                    return false;
                }
                $(rows).each(function (i, item) {
                    ids.push(item.data.Id);
                });
                tipdialog.confirm("确定要删除选中的记录？", function (r) {
                    if (r) {
                        helper.Ajax("003300300159", ids, function (data) {
                            if (data.body) {
                                toast.success("删除成功");
                                $('#tab3').click();
                            }
                            else {
                                tipdialog.alertMsg(data.publicresponse.message);
                            }
                        }, false);
                    }
                });
            });
            //-------------删除方法end----------------

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

            /*  人员档案所需  start */
            function getPersonalPosition() {
                helper.Ajax("003300300332", { PositionTypeIndex: 0 }, function (data) {
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

            /*  人员档案所需  end */

            initPage();


            function CheckSubmit(jsonData1) {
                var msg = '';
                if (jsonData1.LianXiRen == '') {
                    msg += '联系人不能为空<br/>';
                }
                if (jsonData1.ShenFenZheng == '') {
                    msg += '身份证不能为空<br/>';
                } else {
                    if (ValidIdentityCardNumber(jsonData1.ShenFenZheng) == false) {
                        msg += '身份证号码格式不正确';
                    }
                }
                if (msg != '') {
                    tipdialog.alertMsg(msg);
                    return false;
                } else {
                    return true
                }


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










        });
});