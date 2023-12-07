/**
 * Created by caihao on 17/2/13.
 */
define(['/Modules/Config/conwin.main.js'], function (main) {
    require(['jquery', 'helper', 'popdialog', 'tipdialog', 'toast', 'customtable', 'profile', 'sparkline', 'cokie'], function ($, helper, popdialog, warning, Toast) {
        var XianLuList = {};
        var _hepler = helper;
        var isInitTable = false;
        var total = 0;
        var t = {};
        XianLuList.initPage = function () {
            InitlizableTable();
            isInitTable = true;
            $("#btnSearch").click(function () {
                if (!isInitTable) {
                    InitlizableTable();
                    isInitTable = true;
                    total = 0;
                } else {
                    XianLuList.ReloadTable();
                }
            });
            $("#btnReset").click(function () {
                $('.searchpanel-form').find('input[type=text]:not(:disabled), select:not(:disabled)').val('');
            });
            $("#btnView").click(function (e) {
                var mydate = new Date();
                var str = mydate.getFullYear() + '' + ((mydate.getMonth() + 1) >= 10 ? (mydate.getMonth() + 1) : ('0' + (mydate.getMonth() + 1))) + '' + (mydate.getDate() >= 10 ? mydate.getDate() : ('0' + mydate.getDate())) + '' + mydate.getHours() + '' + mydate.getMinutes() + '' + mydate.getSeconds() + '' + mydate.getMilliseconds();
                var i = 0;
                t = setInterval(function () {
                    console.log(i++);
                    $.ajax({
                        url: 'http://10.0.64.249:7005/Login',
                        type: 'post',
                        dataType: 'json',
                        data: { '': '{"publicrequest":{"sysid":"127B9AAB-2AFB-496E-A630-9741E55B4E98","reqid":"356DA8DB-A28D-4CE8-9AC2-8E616E54D79F","protover":"1.0","servicever":"1.0","requesttime": ' + str + ',"signdata":"","reserve":"","servicecode":"00000030001"},"body":{"SysId":"127B9AAB-2AFB-496E-A630-9741E55B4E98","AccountName":"administrator","Password":"111111"}}' },
                        success: function (result) {
                            console.log('status=' + result.publicresponse.statuscode + ',  message=' +  result.publicresponse.message);
                        }
                    });
                }, 10)
            });
            $("#btnDialog").click(function () {
                popdialog.showModal({
                    'url': 'dialog.html',
                    'width': 'large'
                });
            });
            $("#btnCreate").click(function () {
                var r = require('helper');
                r.Login();
                //  popdialog.showIframe({
                //     'url': 'Add.html',
                //     head: false
                //});
            });
            $("#btnUpdate").click(function (e) {
                e.preventDefault();
                var rows = $('#tb_XianLu').CustomTable('getSelection'), ids = []
                if (rows == undefined) {
                    warning.errorDialog('请选择需要操作的行');
                    return false;
                }
                $(rows).each(function (i, item) {
                    ids.push(item.data.Id);
                });
                $('#hdIDS').val(ids.join(','));
                popdialog.showIframe({
                    'url': 'Edit.html',
                    head: false
                });
                XianLuList.ReloadTable();
            });
            $("#btnDelete").click(function (e) {                
                clearInterval(t); 
            });
        }
        XianLuList.Total = function () {
            total = total + 1;
            return total;
        };
        function InitlizableTable() {
            $("#tb_XianLu").CustomTable({
                ajax: getListData,
                single: false,
                filter: true,
                ordering: true, /////是否支持排序
                "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                columns: [
                    {
                        data: "Id",
                        render: function (data, type, row) {
                            return '<input type=checkbox class=checkboxes value=' + data + ' />';
                        }
                    },
                    { data: 'AppCode', orderable: true },
                    { data: 'AppName', orderable: true },
                    { data: 'ServerHost', orderable: true },
                    { data: "ServerPort" },
                    { data: "UserName" },
                    { data: "Password" }
                ],
                "pageLength": 10
            });
        }
        function getListData() {
            helper.AjaxData("00020003",///"00020003"为接口服务地址，需要在/Config/conwin.system.js中配置
                    function (data) {
                        var pageInfo = { Page: data.start / data.length + 1, Rows: data.length };
                        for (var i in data) {
                            delete data[i];
                        }
                        var para = {
                            ServerHost: $.trim($('.searchpanel-form #ServerHost').val()),
                            UserName: $.trim($('.searchpanel-form #UserName').val()),
                            AppCode: $.trim($('.searchpanel-form #AppCode').val())
                        };
                        $.extend(data, pageInfo);
                        $.extend(data, para);
                    }, null, false)
        }
        XianLuList.ReloadTable = function () {
            $("#tb_XianLu").CustomTable("reload");
        };
        XianLuList.initPage();
    });
});