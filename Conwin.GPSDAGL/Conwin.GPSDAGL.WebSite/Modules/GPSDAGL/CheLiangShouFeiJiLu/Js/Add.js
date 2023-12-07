define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, Metronic) {
            var initPage = function () {
                var tabFlag = false;
                common.AutoFormScrollHeight('#Form1');
                formcontrol.initial();

                initData();
              
                //保存
                $('#saveBtn').on('click', function (e) {
                    e.preventDefault();
                    var flags = formcontrol.buttonValid();
                    if (flags) {
                        save();
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
                //tab3
                $('#tab3').click(function (e) {
                    if ($('#tab3').parent('li').hasClass('active')) {
                        e.preventDefault();
                    } else {
                        if (!tabFlag) {
                            tipdialog.alertMsg('请先点击暂存/保存按钮!', function () {
                                $('#tab3').parent('li').removeClass('active');
                                $('#tab1').parent('li').addClass('active');
                                $('#FuJian').removeClass('active in');
                                $('#JiChuXinXi').addClass('active in');
                            });
                            if ($('.bootbox-body').html() == '请先点击暂存/保存按钮!') {
                                $('.bootbox-close-button').click(function () {
                                    $('#tab3').parent('li').removeClass('active');
                                    $('#tab1').parent('li').addClass('active');
                                    $('#FuJian').removeClass('active in');
                                    $('#JiChuXinXi').addClass('active in');
                                });
                            }
                        } else {
                            $('#FuJian').addClass('active in');
                            $('#JiChuXinXi').removeClass('active in');
                        }
                    }
                });
                //选择车辆信息
                $('#btnSearchCarList').click(function (e) {
                    e.preventDefault();
                    popdialog.showModal({
                        'url': 'SearchCarList.html',
                        'width': '1000px',
                        'showSuccess': initSearchCarListZiBiao

                    });
                });


                //个性化代码块
                //region
                selectCity();
                $('.datepicker').datepicker({
                    language: 'zh-CN',
                    format: 'yyyy-mm-dd',
                    autoclose: true//选中之后自动隐藏日期选择框
                });
                $('.datetimepicker').datetimepicker({
                    language: 'zh-CN',
                    startView: 2,
                    maxView: 3,
                    format: 'yyyy-mm-dd hh:ii',
                    autoclose: true//选中之后自动隐藏日期选择框
                });
                //endregion
            };
            function initSearchCarListZiBiao() {
                $('#btnGetCarInfo').click(function (e) {
                    e.preventDefault();
                    var rows = $("#tb_Template_zb").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要查看的行');
                        return false;
                    }
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                    });
                    if (ids.length > 1) {
                        tipdialog.errorDialog('只允许选择一行');
                        return false;
                    }
                    //alert(ids[0]);

                    //调用查询车辆接口信息取回车辆数据。
                    //var ChePaiHao = $('#ChePaiHao').val();
                    //var QueryCarInfoApiCode = "003300530009";//？修改接口编号
                    helper.Ajax("003300300092", ids[0], function (data) {
                        if (data.body) {
                            //查询到车辆信息。
                            //绑定信息到控件上。
                            //?并且将控件属性设置为只读？
                            //console.log(data.body.ChePaiYanSe);
                            $('#XiaQuShi').attr('readonly', 'readonly').val(data.body.XiaQuShi == null ? '' :data.body.XiaQuShi);
                            $('#XiaQuXian').attr('readonly', 'readonly').val(data.body.XiaQuXian == null ? '' :data.body.XiaQuXian);
                            $('#YeHuXinXi').attr('readonly', 'readonly').val(data.body.YeHuXinXi == null ? '' :data.body.YeHuXinXi);
                            $('#ChePaiHao').attr('readonly', 'readonly').val(data.body.ChePaiHao == null ? '' : data.body.ChePaiHao);
                            $('#ChePaiYanSe').val(data.body.ChePaiYanSe == null ? '' : data.body.ChePaiYanSe);
                            $('#FuWuQiShiRiQi').val(data.body.FuWuQiShiRiQi == null ? '' : data.body.FuWuQiShiRiQi.split('T')[0]);
                            $('#FuWuJieShuRiQi').val(data.body.FuWuJieZhiRiQi == null ? '' : data.body.FuWuJieZhiRiQi.split('T')[0]);
                        } else {
                            //请求错误处理。
                            tipdialog.alertMsg("车辆信息获取失败");
                        }
                    }, false);
                    popdialog.closeModal();
                });
                $('#btnZiBiao').click(function (e) {
                      if ($("#ChePaiHaoZiBiao").val().trim() != "" && $("#ChePaiHaoZiBiao").val().trim().length < 3) {
                            tipdialog.errorDialog('请输入合法的车牌号码（至少三位）');
                            return;
                        }
                    $("#tb_Template_zb").CustomTable("reload");
                });
                $("#tb_Template_zb").CustomTable({
                    ajax: helper.AjaxData("003300300091",///"00020003"为接口服务地址，需要在/Config/conwin.system.js中配置
                        function (data) {
                            var pageInfo = { Page: parseInt(data.start / data.length + 1), Rows: data.length };
                            for (var i in data) {
                                delete data[i];
                            }
                            //var para = $('.searchpanel-form').serializeObject();
                            //var date = new Date();

                            //alert(date.getFullYear() + '-' + date.getMonth() + '-' + date.getDay());
                            //var para = { 'FuWuJieShuRiQi': '2018-04-23 00:00:00'};
                            //alert(para['FuWuJieShuRiQi']);
                            //$('.searchpanel-form').find('[disabled]').each(function (i, item) {
                            //    para[$(item).attr('name')] = $(item).val();  
                            //});
                            var para = { 'ChePaiHao': $('#ChePaiHaoZiBiao').val(), 'ChePaiYanSe': $('#ChePaiYanSeZiBiao').val() };
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
                        { data: 'ChePaiHao' },
                        {
                            data: 'ChePaiYanSe'
                            //, render: function (data, type, row) {
                            //    if (data == undefined) return "";
                            //    //var colorStr = new Array('黄色', '黑色', '蓝色', '白色', '其他');
                            //    //return colorStr[data - 1];
                            //}
                        },
                        { data: 'XiaQuShi' },
                        { data: 'XiaQuXian' }
                    
                    ],
                    pageLength: 10,
                    "fnDrawCallback": function (oSettings) {
                        $('#tb_Template_zb_wrapper .table-scrollable').css({ 'height': '420px', 'overflow-y': 'auto' });
                        //tableheadfix.ResetFix();
                    }
                });
                //tableheadfix.InitFix(system.OnlyTableFix);
                



            }

            //初始化表单数据
            function initData() {
                $('#Id').val(helper.NewGuid());
                //跳转的传参.
                var tag = window.parent.document.getElementById('hdTag').value;
                if (typeof(tag) == 'string' && tag != '') {
                    helper.Ajax("003300300092", tag, function (data) {
                        if (data.body) {
                            //查询到车辆信息。
                            //绑定信息到控件上。
                            //?并且将控件属性设置为只读？
                            //console.log(data.body.ChePaiYanSe);
                            $('#XiaQuShi').attr('readonly', 'readonly').val(data.body.XiaQuShi == null ? '' : data.body.XiaQuShi);
                            $('#XiaQuXian').attr('readonly', 'readonly').val(data.body.XiaQuXian == null ? '' : data.body.XiaQuXian);
                            $('#YeHuXinXi').attr('readonly', 'readonly').val(data.body.YeHuXinXi == null ? '' : data.body.YeHuXinXi);
                            $('#ChePaiHao').attr('readonly', 'readonly').val(data.body.ChePaiHao == null ? '' : data.body.ChePaiHao);
                            $('#ChePaiYanSe').val(data.body.ChePaiYanSe == null ? '' : data.body.ChePaiYanSe);
                            $('#FuWuQiShiRiQi').val(data.body.FuWuQiShiRiQi == null ? '' : data.body.FuWuQiShiRiQi.split('T')[0]);
                            $('#FuWuJieShuRiQi').val(data.body.FuWuJieZhiRiQi == null ? '' : data.body.FuWuJieZhiRiQi.split('T')[0]);
                        } else {
                            //请求错误处理。
                            tipdialog.alertMsg("车辆信息获取失败,请尝试点击【获取车辆资料】按钮添加收费记录");
                        }
                    }, false);
                }
            };
            //保存
            function save() {
                //TODO: 校验数据
                var jsonData1 = $('#Form1').serializeObject();
                jsonData1["ChePaiYanSe"] = $("#ChePaiYanSe").val();
                //调用新增接口
                helper.Ajax("003300300085", jsonData1, function (data) {
                    if (data.body) {
                        toast.success("档案保存成功");
                        //window.parent.document.getElementById('hdIDS').value = jsonData1.Id;
                        //setTimeout(function () { window.location.href = "Edit.html"; }, 800);
                        //setTimeout(function () { window.location.href = "List.html"; },1000);
                        //parent.window.$("#btnSearch").click();
                        //parent.window. $("#tb_Template").CustomTable("reload");
                        //$('#saveBtn').attr('disabled', true);
                        parent.window.$("#btnSearch").click();
                        popdialog.closeIframe();

                    }
                    else {
                        tipdialog.alertMsg("档案保存失败");
                    }
                }, false);
            };
            //个性化代码块
            //region       
            function selectCity() {
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#XiaQuShi, #XiaQuXian').empty().append(defaultOption);
                selectcity.setXiaQu('00000020005', { "Province": "广东" }, '#XiaQuShi', 'GetCityList', 'CityName');
                $('#XiaQuShi').change(function () {
                    $('#XiaQuXian,#XiaQuZhen').empty().append(defaultOption);
                    var data = { "City": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化区县下拉框
                        selectcity.setXiaQu('00000020006', data, '#XiaQuXian', 'GetDistrictList', 'DistrictName');
                    }
                });
                $('#XiaQuXian').change(function () {
                    $('#XiaQuZhen').empty().append(defaultOption);
                    var data = { "District": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化区镇下拉框
                        selectcity.setXiaQu('00000020007', data, '#XiaQuZhen', 'GetTownList', 'TownName');
                    }
                });
            };

            function Split(str, tag) {
                if (str == null || str == '' || typeof (str) != 'string') {
                    return '';
                }
                else if (tag == null || tag == '' || typeof (tag) != 'string') {
                    return str;
                }
                else {
                    return str.split(tag);
                }
            }
            //endregion
            initPage();
        });
});
