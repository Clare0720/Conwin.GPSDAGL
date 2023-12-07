define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'fileupload', 'layer','bootbox','metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, fileupload, layer, bootbox) {
            var initPage = function () {
                //$("#ZNSPBJJKFW").attr("checked", "checked");
                //$("#ZNSPBJJKFW").parent().attr("class", "checked");
                $('#btnclose').click(function () {
                    popdialog.closeIframe();
                });
                $('#btnBack1').click(function () {
                    popdialog.closeIframe();
                });
                $('#btnSave1').click(function () {
                    var btncontent = ['重新选择', '确认选择'];
                    var ChePaiHao = window.parent.document.getElementById('hdChePaiHao').value;
                    var services = getSelectService();
                    if (services.length == 0) {
                        btncontent = ['重新选择'];
                    }
                    var content = $("#confirmcontent").html();
                    layer.confirm(content,
                        { title: '请确认' + ChePaiHao + '所选择的服务信息', skin: "my-skin", btn: btncontent,area:["500px","270px"] },
                        function () {
                            layer.close(layer.index);

                        }, function () {
                            layer.close(layer.index);
                            if (checkSelected()) {
                                AddFuWuXinXi(services);
                            } else {
                                toast.success("保存成功!");
                                setTimeout(function () { $('#btnclose').click() }, 1000);
                            }
                        });
                    return;
                    
                });

               //改为后端接口更新底座信息
                { /** 
                //gps底座
                $(".GPS").click(function () {


                    var contenthtml = "";
                    var valAry = [];
                    var xuhao = 1;
                    $("#Form1").find('input[type=checkbox]:not(:disabled)').each(function (index, item) {
                        if ($(item).hasClass("basedOnGPS") && $(item).parent().attr("class") == "checked") {
                            contenthtml += "<div style='color:black;'>" + xuhao + "、" + $(item).attr("serviceName") + "</div>";
                            valAry.push($(item).val() + "|" + $(item).attr("serviceName"));
                            xuhao++;
                        }
                    })

                    if (xuhao <= 1) {
                        return;
                    }
                    //取消选中 基于GPS底座的服务
                    if (checkAllUnSelected("GPS")) {
                        $("#Form1").find('input[type=checkbox]:not(:disabled)').each(function (index, item) {
                            if ($(item).hasClass("GPS") && !$(item).parent().hasClass("checked")) {
                                $(item).prop("checked", "checked");
                                $(item).parent().prop("class", "checked")
                            }
                        });
                        bootbox.dialog({
                            message: "以下服务绑定了企业卫星定位监控软件服务，若继续取消将会同时取消以下服务<br>" + contenthtml + "<br>是否确定取消？",
                            title: "提示",
                            buttons: {
                                main: {
                                    label: "确定",
                                    className: "blue",
                                    callback: function () {
                                        $("#Form1").find('input[type=checkbox]:not(:disabled)').each(function (index, item) {

                                            if ($(item).hasClass("GPS") && $(item).parent().hasClass("checked")) {
                                                $(item).prop("checked", "none");
                                                $(item).parent().removeClass("checked")
                                            }

                                            if ($(item).hasClass("basedOnGPS") && $(item).parent().hasClass("checked")) {
                                                $(item).prop("checked", "none");
                                                $(item).parent().removeClass("checked")
                                            }
                                        })
                                    }
                                },
                                Cancel: {
                                    label: "取消",
                                    className: "default",
                                    callback: function () {
                                        $("#Form1").find('input[type=checkbox]:not(:disabled)').each(function (index, item) {
                                            if ($(item).hasClass("GPS") && !$(item).parent().hasClass("checked")) {
                                                $(item).prop("checked", "checked");
                                                $(item).parent().prop("class", "checked")
                                            }
                                        })
                                    }
                                }
                            }
                        });
                    }
                    
                });
                //基于GPS底座的服务
                $(".basedOnGPS").click(function () {
                    if (checkSelectedForClass("basedOnGPS")) {
                        $("#Form1").find('input[type=checkbox]:not(:disabled)').each(function (index, item) {
                            if ($(item).hasClass("GPS") && !$(item).parent().hasClass("checked")) {
                                $(item).prop("checked","checked");
                                $(item).parent().prop("class","checked")
                            }
                        })
                    } 
                });

                //视频底座
                $(".Video").click(function () {

                    var contenthtml = "";
                    var valAry = [];
                    var xuhao = 1;
                    $("#Form1").find('input[type=checkbox]:not(:disabled)').each(function (index, item) {
                        if ($(item).hasClass("basedOnVideo") && $(item).parent().attr("class") == "checked") {
                            contenthtml += "<div style='color:black;'>" + xuhao + "、" + $(item).attr("serviceName") + "</div>";
                            valAry.push($(item).val() + "|" + $(item).attr("serviceName"));
                            xuhao++;
                        }
                    })

                    if (xuhao <= 1) {
                        return;
                    }

                    //取消选中 基于视频底座的服务
                    if (checkAllUnSelected("Video")) {
                        $("#Form1").find('input[type=checkbox]:not(:disabled)').each(function (index, item) {
                            if ($(item).hasClass("Video") && !$(item).parent().hasClass("checked")) {
                                $(item).prop("checked", "checked");
                                $(item).parent().prop("class", "checked")
                            }
                        });
                        
                        bootbox.dialog({
                            message: "以下服务绑定了企业智能视频报警监控软件服务，若继续取消将会同时取消以下服务<br>" + contenthtml+"<br>是否确定取消？",
                            title: "提示",
                            buttons: {
                                main: {
                                    label: "确定",
                                    className: "blue",
                                    callback: function () {
                                        $("#Form1").find('input[type=checkbox]:not(:disabled)').each(function (index, item) {

                                            if ($(item).hasClass("Video") && $(item).parent().hasClass("checked")) {
                                                $(item).prop("checked", "none");
                                                $(item).parent().removeClass("checked")
                                            }

                                            if ($(item).hasClass("basedOnVideo") && $(item).parent().hasClass("checked")) {
                                                $(item).prop("checked", "none");
                                                $(item).parent().removeClass("checked")
                                            }
                                        })
                                    }
                                },
                                Cancel: {
                                    label: "取消",
                                    className: "default",
                                    callback: function () {
                                        $("#Form1").find('input[type=checkbox]:not(:disabled)').each(function (index, item) {
                                            if ($(item).hasClass("Video") && !$(item).parent().hasClass("checked")) {
                                                $(item).prop("checked", "checked");
                                                $(item).parent().prop("class", "checked")
                                            }
                                        })
                                    }
                                }
                            }
                        });


                    }

                });
                //基于GPS底座的服务
                $(".basedOnVideo").click(function () {
                    if (checkSelectedForClass("basedOnVideo")) {
                        $("#Form1").find('input[type=checkbox]:not(:disabled)').each(function (index, item) {
                            if ($(item).hasClass("Video") && !$(item).parent().hasClass("checked")) {
                                $(item).prop("checked", "checked");
                                $(item).parent().prop("class", "checked")
                            }
                        })
                    }
                });
                */}

                GetFuWuXinXi();
            };
            function AddFuWuXinXi(serviceAry) {
                var id = window.parent.document.getElementById('hdIDS').value;
                var para = {
                    id: id,
                    serviceAry: serviceAry
                }
                console.log(para);
                helper.Ajax("003300300403", para, function (result) {
                    if (result.publicresponse.statuscode == 0) {
                        if (result.body) {
                            toast.success("保存成功!");
                            setTimeout(function () { $('#btnclose').click() }, 1000);
                        }
                        else {
                            tipdialog.errorDialog('保存失败，添加到车辆名单出错！');
                        }
                    }
                    else {
                        tipdialog.errorDialog('获取服务项目信息失败,' + result.publicresponse.message);
                    }
                }, true);
            }
            function GetFuWuXinXi() {
                var ChePaiHao = window.parent.document.getElementById('hdChePaiHao').value;
                var ChePaiYanSe = window.parent.document.getElementById('hdChePaiYanSe').value;
                var para = {
                    "ChePaiHao": ChePaiHao,
                    "ChePaiYanSe": ChePaiYanSe
                }
                helper.Ajax("003300300402", para, function (result) {
                    if (result.publicresponse.statuscode == 0) {
                        if (result.body && result.body.length>0) {
                            $("#Form1").find('input[type=checkbox]').each(function (index, item) {
                                for (var i = 0; i < result.body.length; i++) {
                                    if ($(item).val() == result.body[i]) {
                                        $(item).attr("checked", "checked");
                                        $(item).parent().attr("class", "checked");
                                        $(item).attr("disabled", "disabled");
                                        $(item).parent().parent().attr("class", "checker disabled");
                                    }
                                }

                            })
                            if (checkNoSelected()) {
                                $('#btnSave1').css("display", "inline-block");
                            }
                        }
                        else {
                            $('#btnSave1').css("display", "inline-block");
                        }
                    }
                    else {
                        tipdialog.errorDialog('获取服务项目信息失败,' + result.publicresponse.message);
                    }
                }, true);
            }
            function getSelectService() {
                var contenthtml = "";
                var valAry = [];
                var xuhao = 1;
                $("#Form1").find('input[type=checkbox]:not(:disabled)').each(function (index, item) {
                    if ($(item).parent().attr("class") == "checked") {
                        contenthtml += "<div style='color:black;'>" + xuhao + "、" + $(item).attr("serviceName") + "</div>";
                        valAry.push($(item).val() + "|" + $(item).attr("serviceName"));
                        xuhao++;
                    }
                })
                if (xuhao == 1) {
                    contenthtml = "<div style='color:red;'>没有选中任何服务!</div>";
                }
                $("#services").html("");
                $("#services").html(contenthtml);
                return valAry;
            }
            function checkSelected() {
                var flag = false;
                $("#Form1").find('input[type=checkbox]:not(:disabled)').each(function (index, item) {
                    if ($(item).parent().attr("class") == "checked") {
                        flag = true;
                    }
                })
                return flag;
            }
            function checkNoSelected() {
                var flag = false;
                $("#Form1").find('input[type=checkbox]:not(:disabled)').each(function (index, item) {
                    if ($(item).parent().attr("class") != "checked") {
                        flag = true;
                    }
                })
                return flag;

            }

            
            function checkSelectedForClass(className) {
                var flag = false;
                $("#Form1").find('input[type=checkbox]:not(:disabled)').each(function (index, item) {
                        if ($(item).hasClass(className) && $(item).parent().attr("class") == "checked") {
                            flag = true;
                        }
                })
                return flag;
            }
            function checkAllUnSelected(className) {
                var flag = true;
                $("#Form1").find('input[type=checkbox]:not(:disabled)').each(function (index, item) {
                    if ($(item).hasClass(className) && $(item).parent().attr("class") == "checked") {
                        flag = false
                    } 
                })
                return flag;
            }

            initPage();
        });
});