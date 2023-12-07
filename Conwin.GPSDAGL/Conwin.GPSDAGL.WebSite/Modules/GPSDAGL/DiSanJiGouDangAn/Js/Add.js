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
                helper.UserInfo(selectCity);

                //保存
                $('#saveBtn').on('click', function (e) {
                    e.preventDefault();
                    var flags = true;
                    var msg = '';
                    var fromData = $('#Form1').serializeObject();
                    fromData.YouXiaoZhuangTai = $('#YouXiaoZhuangTai').val();
                    //fromData.QiYeLeiXing = $('#QiYeLeiXing').val();
                    //调用新增接口
                    if ($.trim(fromData.JiGouMingCheng) == '') {
                        msg += "机构名称 是必填项<br/>";
                    }
                    if ($.trim(fromData.JiGouLeiXing) == '') {
                        msg += "机构类型 是必选项<br/>";
                    }
                    //if ($.trim(fromData.YingYeZhiZhaoHao) == '' && $.trim(fromData.TongYiSheHuiXinYongDaiMa) == '') {
                    //    msg += "营业执照号和统一社会信用代码 必填一个<br/>";
                    //}
                    //if ($.trim(fromData.HeZuoFangShi) == '') {
                    //    msg += "合作方式 是必选项<br/>";
                    //}
                    if ($.trim(fromData.JingYingQuYu) == '') {
                        msg += "经营区域 是必选项<br/>";
                    }

                    if ($.trim(fromData.YouXiaoZhuangTai) == '') {
                        msg += "有效状态 是必填项<br/>";
                    }
                    if ($.trim(fromData.XiaQuShi) == '') {
                        msg += "辖区市 是必填项<br/>";
                    }
                    //if ($.trim(fromData.YouBian) != '') {
                    //    if (!new RegExp('^[1-9][0-9]{5}$').test($.trim(fromData.YouBian))) {
                    //        msg += "邮编 格式不正确<br/>";
                    //    }
                    //}
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
                helper.Ajax("006600200055", jsonData1, function (data) {
                    if ($.type(data) == "string") {
                        data = helper.StrToJson(data);
                    }
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {                    
                            toast.success("创建成功",{ showDuration:100});
                            setTimeout(function () {
                                parent.window.$("#btnSearch").click();
                                popdialog.closeIframe();
                            }, 2000);
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
            //级联城市下拉框
            function selectCity(UserInfo) {
                UserInfo = UserInfo.body;
                var defaultOption = '<option value="" selected="selected">请选择</option>';
                $('#XiaQuShi, #XiaQuXian').empty().append(defaultOption);

                var data = { "Province": "广东" };///todo:初始化省份
                ///调用接口初始化城市下拉框
                selectcity.setXiaQu('00000020005', data, '#XiaQuShi', function () {
                    var XiaQuShi = UserInfo.OrganizationManageArea;
                    XiaQuShi = XiaQuShi.replace(/广东/g, "");
                    var list = XiaQuShi.split("|");
                    $("#XiaQuShi").find("option").each(function (index, item) {
                        if (list.indexOf($(item).val()) < 0 && $(item).val() != "") {
                            $(item).remove();
                        }
                    });
                }, 'GetCityList', 'CityName');

                $('#XiaQuShi').change(function () {
                    $('#XiaQuXian').empty().append(defaultOption);
                    var data = { "City": $(this).val() };
                    if ($(this).val() != '') {
                        ///调用接口初始化区县下拉框
                        selectcity.setXiaQu('00000020006', data, '#XiaQuXian', 'GetDistrictList', 'DistrictName');
                    }
                });

                $('#XiaQuXian').change(function () {
                    var data = { "District": $(this).val() };
                });
            }

            initPage();
        });
});
