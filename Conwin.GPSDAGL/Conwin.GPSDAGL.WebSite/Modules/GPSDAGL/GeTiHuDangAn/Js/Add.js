define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'metronic', 'selectCity2', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, Metronic, selectCity2, fileupload) {
            var initPage = function () {
                var tabFlag = false;
                common.AutoFormScrollHeight('#Form1');
                //common.AutoFormScrollHeight('#Form2');
                //common.AutoFormScrollHeight('#Form3');
                formcontrol.initial();
                initData();


                //保存
                $('#saveBtn').on('click', function (e) {
                    e.preventDefault();
                    var flags = true;
                    var msg = '';
                    var fromData = $('#Form1').serializeObject();
                    fromData.YouXiaoZhuangTai = $('#YouXiaoZhuangTai').val();
                    //调用新增接口
                    if ($.trim(fromData.YeHuMingCheng) == '') {
                        msg += "业户名称 是必填项<br/>";
                    }
                    if ($.trim(fromData.YeHuDaiMa) == '') {
                        msg += "业户代码 是必填项<br/>";
                    }
                    if ($.trim(fromData.TongYiSheHuiXinYongDaiMa) == '') {
                        msg += "统一社会信用代码 是必选项<br/>";
                    }
                    if ($.trim(fromData.JingYingXuKeZhengZi) == '') {
                        msg += "经营许可证字 是必选项<br/>";
                    }
                    if ($.trim(fromData.JingYingXuKeZhengHao) == '') {
                        msg += "经营许可证号 是必选项<br/>";
                    }
                    if ($.trim(fromData.YouXiaoZhuangTai) == '') {
                        msg += "有效状态 是必填项<br/>";
                    }
                    //if ($.trim(fromData.YingYeZhiZhaoFuBenId) == '') {
                    //    msg += "营业执照副本 必须上传<br/>";
                    //}
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

                ////初始化附件上传按钮
                //$('.fa-upload').each(function (index, item) {
                //    $('#' + $(item).parent()[0].id).fileupload({
                //        multi: false,
                //        timeOut: 20000,
                //        allowedContentType: 'png|jpg|jpeg'
                //    });
                //});



                selectCity2.initSelectView($('.select'));
                $('#add').click(function (e) {
                    e.preventDefault();
                    selectCity2.showSelectCity();
                });

            };
            //初始化表单数据
            function initData() {
                $('#Id').val(helper.NewGuid());
            };
            //保存
            function save() {
                //todo 校验数据
                var jsonData1 = $('#Form1').serializeObject();
                //jsonData1.QiYeLeiXing = $('#QiYeLeiXing').val();
                jsonData1.YouXiaoZhuangTai = $('#YouXiaoZhuangTai').val();
                for (var key in jsonData1) {
                    jsonData1[key] = jsonData1[key].replace(/\s/g, "");
                }


                //调用新增接口

                //TODO
                helper.Ajax("003300300318", jsonData1, function (data) {
                    if ($.type(data) == "string") {
                        data = helper.StrToJson(data);
                    }
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {
                            
                            toast.success("创建成功",{ showDuration:100});
                            window.parent.document.getElementById('hdIDS').value = jsonData1.Id;
                            setTimeout(function () { window.location.href = "Edit.html"; }, 2000);
                        }
                        else {
                            tipdialog.alertMsg("档案保存失败");
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
