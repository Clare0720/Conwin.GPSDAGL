define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'searchbox', 'customtable', 'bootstrap-datepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system, selectcity) {
            var initPage = function () {
                //初始化table
                initlizableTable();
                //查询
                $('#btnSearchZhongDuan').click(function (e) {
                    e.preventDefault();
                    $("#tb_Template_zd").CustomTable("reload");
                    alert('ok');

                });
                //确定
                $('#btnXuanZe').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_CheLiangXinXi").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要查看的行');
                        return false;
                    }
                    //TODO:编写逻辑
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                    });
                    $('#hdIDS').val(ids.join(','));
                });
            };
            function initlizableTable() {
                $("#tb_Template_zd").CustomTable({
                    ajax: helper.AjaxData("003300200041",///"00020003"为接口服务地址，需要在/Config/conwin.system.js中配置
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
                    single: false,
                    filter: true,
                    ordering: true, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [
                        {
                            render: function (data, type, row) {
                                return '<input type=checkbox class=checkboxes />';
                            }
                        },
                        { data: 'SheBeiXinHao' },
                        { data: 'ChePaiYanSe' },
                        { data: 'ChangJiaBianHao' },
                        { data: 'XingHaoBianMa' },
                        { data: 'ZhongDuanBianMa' },
                        { data: 'ShiYongCheXin' },
                        { data: 'DingWeiMoKuai' },
                        { data: 'TongXunMoShi' },
                        { data: 'GuoJianPiCi' }
                    ],
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        tableheadfix.ResetFix();
                    }
                });
                tableheadfix.InitFix(system.OnlyTableFix);
            };
            //个性化代码块
            //region
            //endregion
            initPage();
        });
});