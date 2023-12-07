define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'metronic', 'selectCity2', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, Metronic, selectCity2, fileupload) {
            var userInfo = helper.GetUserInfo();
            var positionTypeArr = [];

            var initPage = function () {
                common.AutoFormScrollHeight('#Form1');

                formcontrol.initial();
                initData();

                //关闭
                $('#btnclose').click(function () {
                    parent.window.$("#btnSearch").click();
                    popdialog.closeIframe();
                });

                common.AutoFormScrollHeight('#LianXiXinXi', function (hg) {
                    var boxHeight = hg - $('.portlet-title').outerHeight(true) - $('.nav-tabs').outerHeight(true) - 245;
                    var me = $(".scroller");
                    me.parent().css('height', boxHeight);
                    me.css('height', boxHeight);
                });

                $('#tab2').on('click', function () {
                    $("#tb_lianXiRenTable").CustomTable("reload");
                });
            };



            //初始化表单数据
            function initData() {

                var id = window.parent.document.getElementById('hdIDS').value;

                fillFormData(id);
                
            };


            function fillFormData(id) {
                helper.Ajax("006600200056", id, function (data) {
                    if ($.type(data) == "string") {
                        data = helper.StrToJson(data);
                    }
                    if (data.publicresponse.statuscode == 0) {
                        var body = data.body;
                        //基本信息填充
                        $("#Form1").find("input[name],select[name],textarea[name]").each(function (i, item) {
                            var tempvalues = body[$(item).attr('name')];
                            if (tempvalues !== undefined) {
                                $(item).val(tempvalues.toString() == "" ? "" : tempvalues);
                            } else {
                                $(item).val('');
                            };
                        });

                        $('#OrgCode').val(body['BenDanWeiOrgCode']);
                        $('label[for = "XiaQuSheng"]').html(body["XiaQuSheng"]);
                        $('label[for = "XiaQuShi"]').html(body["XiaQuShi"]);
                        $('label[for = "XiaQuXian"]').html(body["XiaQuXian"]);
                        getPersonalPosition();
                        initlizableLianXiRenTable();
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);

            }

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


            initPage();
        });
});
