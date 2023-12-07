define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'fileupload', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, fileupload) {
            var userInfo = helper.GetUserInfo();

            var initPage = function () {
                //初始化页面样式
                common.AutoFormScrollHeight('#Form1', function (hg) {
                    var boxHeight = hg - $('.portlet-title').outerHeight(true) - $('.nav-tabs').outerHeight(true) - 245;
                    var me = $(".scroller", '#Form1').eq(0);
                    me.parent().css('height', boxHeight);
                    me.css('height', boxHeight);
                });
               // common.AutoFormScrollHeight('#Form1');
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

                //关闭
                $('#btnclose').click(function () {
                    popdialog.closeIframe();
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

                $('#tab2').on('click', function () {
                    $("#tb_lianXiRenTable").CustomTable("reload");
                });
                $('#tab3').on('click', function () {
                    $("#tb_JiaShiYuanTable").CustomTable("reload");
                });

                updateData();
            

            };
  
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
            //主表-获取主表数据
            function getXianLuXinXi(id, callback) {
                //调用获取单条信息接口
                helper.Ajax("006600200049", id, function (resultdata) {
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
                        $(item).html(tempValue == '' ? '' : tempValue);
                    } else {
                        $(item).html('');
                    }
                });

                $('#Id').val(resource.Id);

                if ($('#FenZhiJiGouBiaoZhiId').val() != '') {
                    $('#imgUpLoad').attr("src", '' + helper.Route('00000080004', '1.0', system.ServerAgent) + '?id=' + $('#FenZhiJiGouBiaoZhiId').val());
                } else {
                    $('#imgUpLoad').attr("src", '../../Component/Img/NotPic.jpg');
                }

                var val = $("#YouXiaoZhuangTai").val();
                if (val == "1") {
                    $(".YouXiaoZhuangTai").text("正常营业");
                    $("#zhuangtai").html("正常营业");

                } else {
                    $(".YouXiaoZhuangTai").text("合约到期");
                    $("#zhuangtai").html("合约到期");
                }

                $("#jigoumingcheng").html($("#JiGouMingCheng").val());
                $("#fuzeren").html($("#FuZheRen").val());
                $("#fuzerendianhua").html($("#FuZheRenDianHua").val());
            };

            initPage();
        });
});