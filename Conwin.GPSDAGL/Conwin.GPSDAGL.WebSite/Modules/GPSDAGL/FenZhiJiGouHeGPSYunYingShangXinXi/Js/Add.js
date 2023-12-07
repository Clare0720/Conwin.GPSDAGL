define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity','selectCity2', 'filelist', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity,selectCity2, filelist, Metronic, fileupload) {
            var initPage = function () {
                var tabFlag = false;
                common.AutoFormScrollHeight('#Form1');
                common.AutoFormScrollHeight('#Form2');
                common.AutoFormScrollHeight('#Form3');
                formcontrol.initial();
                initData();
                //保存
                $('#saveBtn').on('click', function (e) {
                    e.preventDefault();
                    var flags = true;
                    var msg = '';
                    var fromData = $('#Form1').serializeObject();
                    fromData.YouXiaoZhuangTai = $('#YouXiaoZhuangTai').val();
                    fromData.ShiFouKaiTongTongJiQuanXian = $('#ShiFouKaiTongTongJiQuanXian').val();
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

                //关闭
                $('#btnclose').click(function () {
                    tipdialog.confirm("确定关闭？", function (r) {
                        if (r) {
                            parent.window.$("#btnSearch").click();
                            popdialog.closeIframe();
                        }
                    });
                });

                //tab2
                $('#tab2').click(function (e) {
                    if ($('#tab3').parent('li').hasClass('active')) {
                        e.preventDefault();
                    } else {
                        if (!tabFlag) {
                            tipdialog.alertMsg('请先保存基础信息!', function () {
                                $('#tab2').parent('li').removeClass('active');
                                $('#tab1').parent('li').addClass('active');
                                $('#LianXiXinXi').removeClass('active in');
                                $('#JiChuXinXi').addClass('active in');
                            });
                            if ($('.bootbox-body').html() == '请先保存基础信息!') {
                                $('.bootbox-close-button').click(function () {
                                    $('#tab2').parent('li').removeClass('active');
                                    $('#tab1').parent('li').addClass('active');
                                    $('#LianXiXinXi').removeClass('active in');
                                    $('#JiChuXinXi').addClass('active in');
                                });
                            }
                        } else {
                            $('#LianXiXinXi').addClass('active in');
                            $('#JiChuXinXi').removeClass('active in');
                        }
                    }
                });


                //$("#ShiFouKaiTongTongJiQuanXian").click(function () {
                //    if ($(this).is(":checked")) {
                //        $(this).val("true");
                //    } else {
                //        $(this).val("false");
                //    }
                //});

                //$("select#HeZuoFangShi").change(function () {
                //    selectTongJiQuYu();
                //});
                
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
            //初始化表单数据
            function initData() {
                $('#Id').val(helper.NewGuid());
                //selectTongJiQuYu();
            };

            function selectTongJiQuYu() {
                var heZuoFangSi = $("#HeZuoFangShi option:selected").val();
                if (heZuoFangSi == 2) {
                    $("#Allow").hide();
                } else {
                    $("#Allow").show();
                }
                if ($('#ShiFouKaiTongTongJiQuanXian').val() == "true") {
                    $("#ShiFouKaiTongTongJiQuanXian").click();
                }
            }

            //保存
            function save() {
                //TODO: 校验数据
                var jsonData1 = $('#Form1').serializeObject();
                jsonData1.YouXiaoZhuangTai = $('#YouXiaoZhuangTai').val();
                //jsonData1.ShiFouKaiTongTongJiQuanXian = $('#ShiFouKaiTongTongJiQuanXian').val();
                for (var key in jsonData1) {
                    jsonData1[key] = jsonData1[key].replace(/\s/g, "");
                }
                //调用新增接口
                helper.Ajax("006600200050", jsonData1, function (data) {
                    if ($.type(data) == "string") {
                        data = helper.StrToJson(data);
                    }
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {
                            toast.success("保存成功");
                            setTimeout(function () {
                                parent.window.$("#btnSearch").click();
                                popdialog.closeIframe();
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
            };

            //endregion
            initPage();
        });


});
