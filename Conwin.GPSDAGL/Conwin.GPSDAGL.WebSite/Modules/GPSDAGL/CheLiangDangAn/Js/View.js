define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'fileupload', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, fileupload) {
            var initPage = function () {
                var UserInfo = helper.GetUserInfo(); //用户信息
                if (UserInfo.OrganizationType == 0) {
                    $("#zhongduanxinxi_tab").show();
                };
                //初始化页面样式
                common.AutoFormScrollHeight('#Form1');
                common.AutoFormScrollHeight('#Form2');
                formcontrol.initial();
                initCameraNameData();
                //initCameraNameData();
                //时间控件
                $('.datepicker').datepicker({
                    language: 'zh-CN',
                    format: 'yyyy-mm-dd',
                    autoclose: true //选中之后自动隐藏日期选择框
                });
                $('.datetimepicker').datetimepicker({
                    language: 'zh-CN',
                    startView: 1,
                    maxView: 0,
                    format: 'hh:ii',
                    autoclose: true //选中之后自动隐藏日期选择框
                });
                //翻页控件
                var ids = window.parent.document.getElementById('hdIDS').value;
                prevNextpage.initPageInfo(ids.split(','));
                prevNextpage.bindPageClass();
                //初始化子表
                //关闭
                $('#btnclose').click(function () {
                    popdialog.closeIframe();
                });
                //上一条
                $('#prevBtn').click(function (e) {
                    e.preventDefault();
                    prevNextpage.prev();
                    updateData();
                    updateTag();
                });
                //下一条
                $('#nextBtn').click(function (e) {
                    e.preventDefault();
                    prevNextpage.next();
                    updateData();
                    updateTag();
                });

                //车辆详细信息
                $('#tab2').on('click', function () {
                    var CheliangId = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                    GetData("006600200003", CheliangId, function (data) {
                        var fileId1 = $("#XingShiZHengSaoMiaoJianId").val();
                        if (fileId1) {
                            $("#" + fileId1 + "View").remove();
                        }
                        if (data) {
                            fillFormData(data, "Form2");
                        }
                        fileupload.rebindFileButtonView(['XingShiZHengSaoMiaoJianId']);
                    })
                });

                //终端信息
                $('#tab3').on('click', function () {
                    var CheliangId = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                    GetData("006600200068", CheliangId, function (data) {
                        if (data) {
                            var param = data.GpsInfo;
                            Object.assign(param, data.VideoInfo);
                            //param.VideoDeviceMDT = data.VideoInfo.VideoDeviceMDT;
                            fillFormData(param, "Form3");

                            var arrayCameraSelected = [];
                            $("input[name='CameraSelected']").removeAttr("checked")
                            $("input[name='CameraSelected']").parent().removeAttr("class");
                            if (data.GpsInfo.ShiPinTouAnZhuangXuanZe) {
                                arrayCameraSelected = data.GpsInfo.ShiPinTouAnZhuangXuanZe.split(',');
                                $.each(arrayCameraSelected, function (i, item) {
                                    if (item != '') {
                                        var lemp = item.split("|");
                                        $("input[name='CameraSelected'][val=" + lemp[0] + "]").attr("checked", "checked");
                                        $("input[name='CameraSelected'][val=" + lemp[0] + "]").parent().attr("class", "checked");
                                        $("select[cameraIndex=" + lemp[0] + "] option[value = " + lemp[1] + "]").attr("selected", "selected");
                                        if (typeof lemp[2] == 'undefined' || lemp[2] != "0") {
                                            $("input[name='AudioSelected'][val=" + lemp[0] + "]").attr("checked", "checked");
                                            $("input[name='AudioSelected'][val=" + lemp[0] + "]").parent().attr("class", "checked");
                                        }
                                    }

                                });
                            }
                            $('#CameraSelectedBox :input').each(function (j, item) {
                                $(item).attr("disabled", "disabled");
                            })
                        }
                    })
                });



                updateData();
                //个性化代码块


                function initCameraNameData() {
                    var CameraNameAry = ["驾驶员", "车辆正前方", "车前门", "车厢前部", "车厢后部", "车后门", "驾驶席车门", "其他"];
                    var selectcontent = "";
                    for (var i = 0; i < CameraNameAry.length; i++) {
                        selectcontent += '<option value="' + CameraNameAry[i] + '">' + CameraNameAry[i] + '</option>';
                    }
                    $(".CameraNameData").each(function (index, item) {
                        $(item).attr("cameraIndex", index + 1);
                        $(item).append(selectcontent);
                    })
                }

                //region
                //endregion
            };

            //绑定基本信息数据方法
            function updateData() {
                var id = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                var CheliangId = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                GetData("006600200039", CheliangId, function (data) {
                    fillFormData(data, "Form1");
                });
            };




            /**
             * 根据服务编号获取数据
             * @param {string} ServiceCode 服务编号
             * @param {object} data  body数据
             * @param {function} callback 回调函数
             */
            function GetData(ServiceCode, data, callback) {
                helper.Ajax(ServiceCode, data, function (resultdata) {
                    if (typeof callback == 'function') {
                        if (typeof (resultdata) == "string") {
                            resultdata = JSON.parse(resultdata);
                        }
                        if (resultdata.publicresponse.statuscode == 0) {
                            callback(resultdata.body);
                        } else {
                            tipdialog.errorDialog('获取数据失败!' + resultdata.publicresponse.message);
                        }

                    }
                }, false);
            }



            /**
             * form表单数据填充
             * @param {JSON} resource 数据源
             * @param {string} Id form控件的Id 
             */
            function fillFormData(resource, Id) {

                $('#' + Id).find('input[name],select[name],textarea[name],label[name]').each(function (i, item) {
                    var tempValue = resource[$(item).attr('name')];
                   
                    if (tempValue != undefined) {
                        if ($(item).hasClass('datetimepicker')) {
                            tempValue = tempValue.substr(11, 5);
                        }
                        if ($(item).hasClass('datepicker')) {
                            tempValue = tempValue.substr(0, 10);
                        }
                        if (item.localName == 'label') {
                            $(item).text(tempValue.toString() == '' ? '' : tempValue.toString());
                        }
                        //TODO: 赋值
                        $(item).val(tempValue.toString() == '' ? '' : tempValue.toString());

                        if ($(item).attr('name') == "CheLiangLeiXing") {
                            var stringValue = "";
                            switch (tempValue) {
                                case 0: stringValue = "其它车辆"; break;
                                case 1: stringValue = "重型货车"; break;
                                case 2: stringValue = "大型货车"; break;
                                case 3: stringValue = "中型货车"; break;
                                case 4: stringValue = "小型货车"; break;
                                case 5: stringValue = "特大型客车"; break;
                                case 6: stringValue = "大型客车"; break;
                                case 7: stringValue = "中型客车"; break;
                                case 8: stringValue = "小型客车"; break;
                                case 9: stringValue = "特大型卧铺"; break;
                                case 10: stringValue = "大型卧铺"; break;
                                case 11: stringValue = "中型卧铺"; break;
                                case 12: stringValue = "出租的士"; break;
                                case 13: stringValue = "公交车"; break;
                            }
                            $("#CheLiangLeiXing").text(stringValue);
                        }
                        if ($(item).attr('name') == "CheZhuLeiXing") {
                            var stringValue = "企业所有";
                            switch (tempValue) {
                                case 2: stringValue = "企业所有"; break;
                                case 8: stringValue = "个人所有"; break;
                                default:
                            }
                            $("#CheZhuLeiXing").text(stringValue);
                        }

                        if ($(item).attr('name') == "CheLiangZhongLei") {
                            var stringValue = "";
                            switch (tempValue) {
                                case 1: stringValue = "客运班车"; break;
                                case 2: stringValue = "旅游包车"; break;
                                case 3: stringValue = "危险货运"; break;
                                case 4: stringValue = "重型货车"; break;
                                case 5: stringValue = "公交客运"; break;
                                case 6: stringValue = "出租客运"; break;
                                case 7: stringValue = "教练员车"; break;
                                case 8: stringValue = "普通货运"; break;
                                case 9: stringValue = "其它车辆"; break;
                                default:
                            }
                            $("#CheLiangZhongLei").text(stringValue);
                        }
                        if ($(item).attr('name') == "NianShenZhuangTai") {
                            var stringValue = "";
                            switch (tempValue) {
                                case 0: stringValue = "未通过"; break;
                                case 1: stringValue = "通过"; break;
                                case 2: stringValue = "未审核"; break;
                                default:
                            }
                            $("#NianShenZhuangTai").text(stringValue);
                        }
                        if ($(item).attr('name') == "CheShenYanSe") {
                            var stringValue = "";
                            switch (tempValue) {
                                case 1: stringValue = "黄色"; break;
                                case 2: stringValue = "黑色"; break;
                                case 3: stringValue = "蓝色"; break;
                                case 4: stringValue = "白色"; break;
                                case 5: stringValue = "其它"; break;
                                default:
                            }
                            $("#CheShenYanSe").text(stringValue);
                        }
                        if ($(item).attr('name') == "RanLiao") {
                            var stringValue = "";
                            switch (tempValue) {
                                case 1: stringValue = "柴油"; break;
                                case 2: stringValue = "油气双燃料"; break;
                                case 3: stringValue = "节能油电混合动力"; break;
                                case 4: stringValue = "纯电动"; break;
                                case 5: stringValue = "插电式混合动力"; break;
                                case 6: stringValue = "氢燃料"; break;
                                case 7: stringValue = "燃油"; break;
                                case 8: stringValue = "其它"; break;
                                default:
                            }
                            $("#RanLiao").text(stringValue);
                        }

                    } 
                });

                //设置下拉框的值
                if ($(this).siblings("select").length) {
                    var selectedOption = $(this).siblings("select").find("option");
                    $(selectedOption).each(function (i, item0) {
                       if (item0.value == tempValue) {
                            $(item).html(item0);
                        }
                    })
                };

            };


            var setdate = function () { };

            //个性化代码块
            //region
            //endregion
            initPage();
        });
});