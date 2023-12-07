define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'metronic', 'customtable'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system,  Metronic ) {
            var initPage = function () {
                common.AutoFormScrollHeight('#Form1');

                formcontrol.initial();
                initData();

                //关闭
                $('#btnclose').click(function () {
                    parent.window.$("#btnSearch").click();
                    popdialog.closeIframe();
                });

                //tab2
                $('#tab2').click(function (e) {
                    $('#LianXiXinXi').addClass('active in');
                    $('#JiChuXinXi').removeClass('active in');
                    //}
                });
            };



            //初始化表单数据
            function initData() {

                var id = window.parent.document.getElementById('hdIDS').value;

                fillFormData(id);
                initlizableLianXiRenTable();
                
            };


            function fillFormData(id) {
                helper.Ajax("003300300319", id, function (data) {
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
                        $("#tb_lianXiRenTable").CustomTable("reload");
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);

            }

            //初始化联系人表格
            function initlizableLianXiRenTable() {
                $("#tb_lianXiRenTable").CustomTable({
                    ajax: helper.AjaxData("003300300009",
                        function (data) {
                            var pageInfo = { Page: data.start / data.length + 1, Rows: data.length };
                            for (var i in data) {
                                delete data[i];
                            }
                            var para = new Object();
                            para["BenDanWeiOrgCode"] = $("#OrgCode").val();
                            pageInfo.data = para;
                            $.extend(data, pageInfo);
                        }, null),
                    single: false,
                    //filter: true,
                    ordering: true, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [
                        { data: 'LianXiRen' },
                        {
                            data: 'LeiBie',
                            render: function (data, type, row, meta) {
                                var value = '';
                                switch (data) {
                                    case 1:
                                        value = "企业法人";
                                        break;
                                    case 2:
                                        value = "企业法人";
                                        break;
                                    case 3:
                                        value = "其他";
                                        break;
                                    case 4:
                                        value = "保险人员";
                                        break;
                                    default:
                                }
                                return value;
                            }
                        },
                        { data: 'ShenFenZheng' },
                        { data: 'ShouJiHaoMa' },
                        { data: 'YouXiang' }
                    ],
                    pageLength: 10
                });
            };


            initPage();
        });
});
