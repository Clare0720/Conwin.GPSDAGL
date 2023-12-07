define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'customtable', 'common'],
        function ($, popdialog, warning, Toast, CwHelper) {


        var initPage = function(){
            //初始化页面 begin 
            //todo:（初始化页面）
            InitlizableTable();

            /**
            * 查询按钮事件
            * */
            $("#btnSearch").click(function () {
                ReloadTable();
            });


            /**
             * 重置按钮事件
             * */
            $("#btnReset").click(function () {
                $('.searchpanel-form').find('input[type=text]:not(:disabled), select:not(:disabled)').val('');
            });
            /**
             * 新增按钮事件
             * */
            $("#btnCreate").click(function () {
                gotoPage('Add.html');
                //openPage('Add.html');
            });
            /**
             * 编辑按钮事件
             * */
            $("#btnUpdate").click(function (e) {
                ///判断所选项
                if (bindSelectIds(e)) {
                    gotoPage('Edit.html');
                }
            });

            //启用
            $("#btnEnabled").click(function (e) {
                e.preventDefault();
                var rows = $('#tb_XianLu').CustomTable('getSelection'), id = '';
                if (rows == undefined) {
                    warning.errorDialog('请选择需要操作的行');
                    return false;
                }
                if (rows.length > 1) {
                    warning.errorDialog('只能选择一个显示');
                    return false;
                }
                id = rows[0].data.Id;
                CwHelper.Ajax("006600200065", ///todo
                    id, function (resultdata) {
                        if (resultdata.publicresponse.statuscode == 0) {
                            Toast.success('启用成功');
                            $('#btnSearch').trigger('click');
                            $('#chk_all').parent().removeClass('checked');
                        }
                        else {
                            Toast.success('启用失败');
                            console.log(resultdata.publicresponse.message);
                        }
                    }, false);
            });
            
            /**
             * 删除按钮事件
             * */
            $("#btnDelete").click(function (e) {
                //先验证所选项
                e.preventDefault();
                var rows = $('#tb_XianLu').CustomTable('getSelection'), ids = [];
                if (rows == undefined) {
                    warning.errorDialog('请选择需要操作的行');
                    return false;
                }
                $(rows).each(function (i, item) {
                    ids.push(item.data.Id);
                });
                var info = ids.length > 1 ? "您已选择批量删除信息记录，删除后将无法恢复数据，请确认是否批量删除？" : "删除信息后无法恢复,请确认是否删除?";
                //删除操作确认
                warning.confirm(info, function () {
                    CwHelper.Ajax("006600200062", ///todo
                        ids, function (resultdata) {
                            if (resultdata.publicresponse.statuscode == 0) {
                                Toast.success('删除成功');
                                $('#btnSearch').trigger('click');
                                $('#chk_all').parent().removeClass('checked');
                            }
                            else {
                                Toast.success('删除失败');
                            }
                        }, false);
                });
            });
            /**
             * 查看按钮事件
             * */
            //$("#btnView").click(function (e) {
            //    ///判断所选项
            //    if (bindSelectIds(e)) {
            //        gotoPage('Detail.html');
            //    }
            //});
            ///**
            // * todo:测试----弹出对话框按钮事件
            // * */
            //$("#btnDialog").click(function () {
            //    popdialog.showModal({
            //        'url': 'dialog.html',
            //        'width': 'large'
            //    });
            //});
            //绑定按钮触发事件 end 
        


        }
/**
* 重新加载数据
* */
        var ReloadTable = function () { $("#tb_XianLu").CustomTable("reload"); };
        /**
         * 初始化数据列表
         * */
        function InitlizableTable() {
            $("#tb_XianLu").CustomTable({
                ajax: CwHelper.AjaxData("006600200063",//
                    function (data) {
                        var pageInfo = { Page: data.start / data.length + 1, Rows: data.length };
                        for (var i in data) {
                            delete data[i];
                        }
                        ////todo:设置调用查询列表数据接口参数
                        var para = {
                            NodeName: $.trim($('.searchpanel-form #Nodename').val()),
                        };
                        pageInfo.data = para;
                        $.extend(data, pageInfo);
                    }, null),
                single: false,
                //filter: true,   /////是否支持过滤显示字段列
                ordering: true, /////是否支持排序
                "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                columns: [
                    {
                        data: "Id",
                        render: function (data, type, row) {
                            return '<input type=checkbox class=checkboxes value=' + data + ' />';
                        }
                    },
                    {
                        data: 'SNo',
                        render: function (data, type, full, meta) {
                            return meta.row + 1;
                        }
                    },
                    { data: 'NodeName' },
                    {   data: 'Enabled' ,
                        render: function (data, type, row) {
                            if (data) { return '主显示' }
                            else {
                                return ''
                            };
                        }
                    }

                ],
                "pageLength": 10 /////默认分页数
            });
        }
        /***
         * 判断选择项，并绑定所选项
         * **/
        function bindSelectIds(e) {
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
            return true;
        }
        /**
         * 进入新增、修改、查看页
         * */


            function gotoPage(pageUrl) {
                popdialog.showIframe({
                    url: pageUrl,
                    head: false
                });
            }
        /**
         * todo:测试：根据搜索框内容进行网络请求获取列表数据
         * **/
        function getListData() {
            var ajaxObj = CwHelper.AjaxData("00000070007",///"00020003"为接口服务地址，需要在/Config/conwin.system.js中配置
                function (data) {
                    var pageInfo = { Page: parseInt(data.start / data.length + 1), Rows: data.length };
                    for (var i in data) {
                        delete data[i];
                    }
                    var para = {
                        ServerHost: $.trim($('.searchpanel-form #ServerHost').val()),
                        UserName: $.trim($('.searchpanel-form #UserName').val()),
                        AppCode: $.trim($('.searchpanel-form #AppCode').val())
                    };
                    pageInfo.data = para;
                    $.extend(data, pageInfo);
                }, null, false);
            return ajaxObj;
        }
        /**
        * 调用初始化方法
        * */
        initPage();
    });
});