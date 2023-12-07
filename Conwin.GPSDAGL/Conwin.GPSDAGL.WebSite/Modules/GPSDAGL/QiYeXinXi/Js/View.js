define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'fileupload', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, fileupload) {
            var userInfo = helper.GetUserInfo();
            var positionTypeArr = [];

            var initPage = function () {
                //getPersonalPosition();
                //初始化页面样式
                initlizableLianXiRenTable();
                initlizableJiaShiYuanTable();
                //common.AutoFormScrollHeight('#Form1');
                common.AutoFormScrollHeight('#Form1', function (hg) {
                    var boxHeight = hg - $('.portlet-title').outerHeight(true) - $('.nav-tabs').outerHeight(true) - 245;
                    var me = $(".scroller", '#Form1').eq(0);
                    me.parent().css('height', boxHeight);
                    me.css('height', boxHeight);
                });
                common.AutoFormScrollHeight('#Form2');
                common.AutoFormScrollHeight('#LianXiXinXi', function (hg) {
                    var boxHeight = hg - $('.portlet-title').outerHeight(true) - $('.nav-tabs').outerHeight(true) - 245;
                    var me = $(".scroller");
                    me.parent().css('height', boxHeight);
                    me.css('height', boxHeight);
                });
                common.AutoFormScrollHeight('#Form3');
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
                //初始化子表
                //关闭
                $('#btnclose').click(function () {
                    popdialog.closeIframe();
                });
                //上一条
                $('#prevBtn').click(function (e) {
                    e.preventDefault();
                    prevNextpage.prev();
                    updateData();
                    //updateTag();
                });
                //下一条
                $('#nextBtn').click(function (e) {
                    e.preventDefault();
                    prevNextpage.next();
                    updateData();
                    //updateTag();
                });
                $('#tab4').on('click', function () {
                    $("#tb_FuWuShangGuanLianXinXiTable").CustomTable("reload");
                });
                updateData();
                initlizableFuWuShangTable();
            };
            //绑定基本信息数据方法
            function updateData() {
                var id = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                getPingTaiDaiLiShang(id, function (serviceData) {
                    if (serviceData.publicresponse.statuscode == 0) {
                        //updateTag();
                        fillFormData(serviceData.body);

                    } else {
                        tipdialog.errorDialog("请求数据失败");
                    }
                });
            };
            //主表-刷新数据
            function updateData() {
                var id = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                getQiYeXinXi(id, function (serviceData) {
                    if (serviceData.publicresponse.statuscode == 0) {
                        fillFormData(serviceData.body);
                        $("#tb_lianXiRenTable").CustomTable("reload");
                        $("#tb_JiaShiYuanTable").CustomTable("reload");
                    } else {
                        tipdialog.errorDialog("请求数据失败");
                    }
                });
            };
            //主表-获取主表数据
            function getQiYeXinXi(id, callback) {
                //调用获取单条信息接口
                helper.Ajax("006600200017", id, function (resultdata) {
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
                        if ($(item).hasClass('datetimepicker')) {
                            tempValue = tempValue.substr(11, 5);
                        }
                        if ($(item).hasClass('datepicker')) {
                            tempValue = tempValue.substr(0, 10);
                        }
                        if ($(item).attr('type') == "checkbox") {
                            if (tempValue) {
                                $(item).attr("checked", "checked");
                                $(item).parent().attr("class", "checked");
                            }
                        }
                        //TODO: 赋值
                        $(item).val(tempValue.toString() == '' ? '' : tempValue);
                    } else {
                        $(item).val('');
                    }
                });
                $('#Form1').find('.form-control-static').each(function (i, item) {
                    $(item).html('');
                    var index = $(item).attr('for');
                    var tempValue = resource[index];
                    if (tempValue != undefined) {
                        //TODO: 赋值
                        if ($(item).hasClass('datetimepicker')) {
                            tempValue = tempValue.substr(11, 5);
                        }
                        if ($(item).hasClass('datepicker')) {
                            tempValue = tempValue.substr(0, 10);
                        }
                        $(item).html(tempValue == '' ? '' : tempValue);
                    } else {
                        $(item).html('');
                    }
                });

                $('#Id').val(resource.Id);
                if ($("#ShiFouGeTiHu").val() == 0) {
                    $("#ShiFouGeTiHus").html("否");
                }
                else {
                    $("#ShiFouGeTiHus").html("是");
                }

                //区域
                xiaQuXinXi(resource);

                var jjlx = $("#JingJiLeiXing").val();
                if (jjlx == "1") jjlx = "国有企业";
                if (jjlx == "2") jjlx = "民营企业";
                if (jjlx == "3") jjlx = "外资独资";
                if (jjlx == "4") jjlx = "中外合资";
                if (jjlx == "5") jjlx = "其他";
                $("#JingJiLeiXings").text(jjlx);
            };
            //主表-更新tab状态
            function updateTag() {
                $('#tab1').parent('li').addClass('active');
                $('#JiChuXinXi').addClass('active in');
            };
           //-------------服务商信息相关start--------------

            if (userInfo.OrganizationType == 2 || userInfo.OrganizationType == 0 || userInfo.OrganizationType == 11 || userInfo.OrganizationType == 12) {
                $("#btnViewFuWuShangXinXi").show();
            }
            //初始化列表
            function initlizableFuWuShangTable() {
                $("#tb_FuWuShangGuanLianXinXiTable").CustomTable({
                    ajax: helper.AjaxData("006600200044",
                        function (data) {
                            var pageInfo = { Page: data.start / data.length + 1, Rows: data.length };
                            for (var i in data) {
                                delete data[i];
                            }
                            var para = new Object();
                            para["QiYeCode"] = $("#OrgCode").text();
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
                        { data: 'FuWuShangMingCheng' },
                        { data: 'ZhuLianLuIP' },
                        { data: 'ZhuLianLuDuanKou' },
                        { data: 'CongLianLuIP' },
                        { data: 'CongLianLuDuanKou' },
                        { data: 'XiaQuSheng' },
                        { data: 'XiaQuShi' },
                        { data: 'XiaQuXian' },
                        {
                            data: 'ZhuangTai',
                            render: function (data, type, row) {
                                switch (data) {
                                    case 1: return "已关联"; break;
                                    default: return "未关联";
                                }

                            }
                        },
                    ],
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        tableheadfix.ResetFix();
                    }
                });
            };
            //查看服务商关联信息
            $('#btnViewFuWuShangXinXi').on('click', function (e) {
                e.preventDefault();
                var rows = $("#tb_FuWuShangGuanLianXinXiTable").CustomTable('getSelection');
                if (rows == undefined) {
                    tipdialog.errorDialog('请选择需要查看的记录');
                    return false;
                }
                if (rows.length > 1) {
                    tipdialog.errorDialog('每次只能查看一条记录');
                    return false;
                }
                var guanLianXinXiId = rows[0].data.Id;
                sessionStorage.setItem("_GuanLianXiXiId", guanLianXinXiId);
                popdialog.showModal({
                    'url': 'ViewFuWuShangModal.html',
                    'width': '600px',
                    'showSuccess': initViewFuWuShang
                });
            });
            function initViewFuWuShang() {
                // 获取关联服务商信息
                var id = sessionStorage.getItem("_GuanLianXiXiId");
                helper.Ajax("006600200045", id, function (data) {
                    if (data.body) {
                        var model = data.body;
                        $("#fwsglId").val(model.Id);
                        $("#FuWuShangOrgName").val(model.FuWuShangMingCheng);
                        $("#ZhuLianLuIP").val(model.ZhuLianLuIP);
                        $("#ZhuLianLuDuanKou").val(model.ZhuLianLuDuanKou);
                        $("#CongLianLuIP").val(model.CongLianLuIP);
                        $("#CongLianLuDuanKou").val(model.CongLianLuDuanKou);
                        $("#FuWuShangOrgCode").val(model.FuWuShangCode);
                        $("#PingTaiJieRuMa").val(model.PingTaiJieRuMa);
                        $("#LoginName").val(model.LoginName);
                        $("#M1").val(model.M1);
                        $("#IA1").val(model.IA1);
                        $("#IC1").val(model.IC1);
                        $("#LoginPassWord").val(model.LoginPassWord);
                        //辖区省
                        $('#ViewFuwuShangXiaQuSheng').val('广东');
                        //辖区市
                        selectcity.setXiaQu('00000020005', { "Province": "广东" }, '#ViewFuwuShangXiaQuShi', function () {
                            if (model.XiaQuShi != "") {
                                $("#ViewFuwuShangXiaQuShi").val(model.XiaQuShi);
                            }
                        }, 'GetCityList', 'CityListName');
                        //辖区县
                        if (model.XiaQuShi != "") {
                            selectcity.setXiaQu('00000020006', { "City": model.XiaQuShi }, '#ViewFuwuShangXiaQuXian', function () {
                                if (model.XiaQuXian != "") {
                                    $("#ViewFuwuShangXiaQuXian").val(model.XiaQuXian);
                                }
                            }, 'key', 'key', userInfo.OrganCity);
                        }
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                        popdialog.closeModal();
                    }
                }, false);

                //市级下拉改变时修改县级下拉内容
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#EditFuwuShangXiaQuShi').change(function () {
                    $('#EditFuwuShangXiaQuXian').empty().append(defaultOption);
                    var data = { "City": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化区县下拉框
                        selectcity.setXiaQu('00000020006', data, '#EditFuwuShangXiaQuXian', function () { }, 'GetDistrictList', 'DistrictName');
                    }
                });

                //提交修改
                $('#ViewFuWuShangInfoSure').on('click', function (e) {
                    e.preventDefault();
                    var msg = '';
                    var flags = true;
                    var fromData = $('#EditWuFuShangForm').serializeObject();
                    fromData.QiYeCode = $("#OrgCode").val();
                    fromData.FuWuShangCode = $("#FuWuShangOrgCode").val();
                    fromData.XiaQuSheng = $("#EditFuwuShangXiaQuSheng").val();
                    fromData.XiaQuShi = $("#EditFuwuShangXiaQuShi").val();
                    fromData.XiaQuXian = $("#EditFuwuShangXiaQuXian").val();
                    fromData.ZhuLianLuIP = $("#ZhuLianLuIP").val();
                    fromData.ZhuLianLuDuanKou = $("#ZhuLianLuDuanKou").val();

                    if ($.trim(fromData.XiaQuShi) == '') {
                        msg += "辖区市是必选项<br/>";
                    }
                    if ($.trim(fromData.XiaQuXian) == '') {
                        msg += "辖区县是必选项<br/>";
                    }
                    if ($.trim(fromData.CongLianLuIP) == '') {
                        msg += "从链路IP是必填项<br/>";
                    }
                    if ($.trim(fromData.CongLianLuDuanKou) == '') {
                        msg += "从链路端口是必填项<br/>";
                    }
                    if (msg != '') {
                        flags = false;
                        tipdialog.alertMsg(msg);
                    }
                    if (flags) {
                        helper.Ajax("006600200042", fromData, function (data) {
                            if ($.type(data) == "string") {
                                data = helper.StrToJson(data);
                            }
                            if (data.publicresponse.statuscode == 0) {
                                if (data.body) {
                                    toast.success("保存成功");
                                    setTimeout(function () {
                                        $('#tb_FuWuShangGuanLianXinXiTable').CustomTable('reload');
                                        popdialog.closeModal();
                                    }, 2000);
                                }
                                else {
                                    tipdialog.alertMsg("保存失败");
                                }
                            }
                            else {
                                tipdialog.alertMsg(data.publicresponse.message);
                            }
                        }, false);
                    }

                });
            }

           //-------------服务商信息相关end--------------


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
                        //{
                        //    render: function (data, type, row) {
                        //        return '<input type=checkbox class=checkboxes />';
                        //    }
                        //},
                        { data: 'Name' },
                        {
                            data: 'Positions',
                            render: function (data, type, row, meta) {
                                var value = '';
                                var po = data.split(',');;
                                $.each(data, function (pi, p) {
                                    $.each(positionTypeArr, function (ti, t) {
                                        if (p == t.PositionCode) {
                                            value += `${t.PositionName}|`;
                                        }
                                    })
                                })
                                value = value.substring(0, value.length - 1)
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
                            para["BenDanWeiOrgCode"] = $("#BenDanWeiOrgCode").val();;
                            pageInfo.data = para;
                            $.extend(data, pageInfo);
                        }, null),
                    single: false,
                    //filter: true,
                    ordering: true, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [
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

            function xiaQuXinXi(resource) {
                var area = resource.JingYingQuYuSuoZaiXiaQuSheng + " " + resource.JingYingQuYuSuoZaiXiaQuShi;
                $('#Area').html(area);
            };
            //子表-初始化分页信息
            var tabPageInfo = tabPage();
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

            //个性化代码块
            //region
            initPage();
        });
});