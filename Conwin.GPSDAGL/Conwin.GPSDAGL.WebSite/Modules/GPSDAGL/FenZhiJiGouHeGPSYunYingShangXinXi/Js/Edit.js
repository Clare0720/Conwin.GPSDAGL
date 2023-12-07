define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'selectCity2', 'filelist', 'fileupload', 'dropdown',  'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, selectCity2, filelist, fileupload, dropdown) {
            //模块初始化
            var userInfo = helper.GetUserInfo();
            var initPage = function () {
               
                //初始化页面样式
                common.AutoFormScrollHeight('#Form1', function (hg) {
                    var boxHeight = hg - $('.portlet-title').outerHeight(true) - $('.nav-tabs').outerHeight(true) - 245;
                    var me = $(".scroller", '#Form1').eq(0);
                    me.parent().css('height', boxHeight);
                    me.css('height', boxHeight);
                });

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
                    updateData()
                });

                //下一条
                $('#nextBtn').click(function (e) {
                    e.preventDefault();
                    prevNextpage.next();
                    updateData()
                });

                $('#saveBtn').on('click', function (e) {
                    e.preventDefault();
                    var flags = true;
                    var msg = '';
                    var fromData = $('#Form1').serializeObject();
                    fromData.YouXiaoZhuangTai = $('#YouXiaoZhuangTai').val();
                    //fromData.ShiFouKaiTongTongJiQuanXian = $('#ShiFouKaiTongTongJiQuanXian').val();
                    //调用新增接口
                    if ($.trim(fromData.OrgName) == '') {
                        msg += "机构名称 是必填项<br/>";
                    }
                    if ($.trim(fromData.YingYeZhiZhaoHao) == '' && $.trim(fromData.TongYiSheHuiXinYongDaiMa) == '') {
                        msg += "营业执照号和统一社会信用代码 必填一个<br/>";
                    }
                    if ($.trim(fromData.JingYingQuYu) == '') {
                        msg += "经营区域 是必选项<br/>";
                    }
                    if ($.trim(fromData.YouXiaoZhuangTai) == '') {
                        msg += "有效状态 是必填项<br/>";
                    }
                    if ($.trim(fromData.YouBian) != '') {
                        if (!new RegExp('^[1-9][0-9]{5}$').test($.trim(fromData.YouBian))) {
                            msg += "邮编 格式不正确<br/>";
                        }
                    }
                    if (msg != '') {
                        flags = false;
                        tipdialog.alertMsg(msg);
                    }
                    if (flags) {
                        save();
                    }
                });

                $('#tab2').on('click', function () {
                    $("#tb_lianXiRenTable").CustomTable("reload");
                });
                $('#tab3').on('click', function () {
                    $("#tb_JiaShiYuanTable").CustomTable("reload");
                });
                updateData();
                //绑定基本信息数据方法
                function updateData() {
                    var id = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                    getWuFuShang(id, function (serviceData) {
                        if (serviceData.publicresponse.statuscode == 0) {
                            fillFormData(serviceData.body);
                        } else {
                            tipdialog.errorDialog("请求数据失败");
                        }
                    });
                };
                //取单个分支机构信息接口
                function getWuFuShang(id, callback) {
                    helper.Ajax("006600200049", id, function (resultdata) {
                        if (typeof callback == 'function') {
                            callback(resultdata);
                        }
                    }, false);
                };
                //经营区域
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
            };


            //初始化记录
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

                if ($("#YouXiaoZhuangTai").val() == 1) {
                    $("#zhuangtai").html("正常营业");
                }
                else {
                    $("#zhuangtai").html("合约到期");
                }
                if ($("#YouXiaoZhuangTai").val() == 1) {
                    $("#YXZT").val("正常营业");
                }
                else {
                    $("#YXZT").val("合约到期");
                }
                selectCity2.setData(resource['JingYingQuYu']);


                $("#jigoumingcheng").html($("#JiGouMingCheng").val());
                $("#fuzeren").html($("#FuZheRen").val());
                $("#fuzerendianhua").html($("#FuZheRenDianHua").val());
            };

            //主表-保存
            function save() {
                //TODO:数据校验
                var jsonData1 = $('#Form1').serializeObject();
                jsonData1.YouXiaoZhuangTai = $('#YouXiaoZhuangTai').val();
                //jsonData1.ShiFouKaiTongTongJiQuanXian = $('#ShiFouKaiTongTongJiQuanXian').val();
                for (var key in jsonData1) {
                    jsonData1[key] = jsonData1[key].replace(/\s/g, "");
                }
                //调用修改接口
                helper.Ajax("006600200051", jsonData1, function (data) {
                    if ($.type(data) == "string") {
                        data = helper.StrToJson(data);
                    }
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {
                            toast.success("档案修改成功");
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

            initPage();

        });
});