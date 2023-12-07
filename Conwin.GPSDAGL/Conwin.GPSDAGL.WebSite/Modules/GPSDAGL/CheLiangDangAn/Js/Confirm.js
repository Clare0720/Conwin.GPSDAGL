define(['/Modules/Config/conwin.main.js', '/Modules/Component/Cwplayer/Js/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'Cwplayer', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system, Cwplayer) {
            var defaultConfig = {};
            var cheliangId = window.parent.document.getElementById('hdIDS').value;
            var zhongduanId = "";
            var fileUrl = system.GetFilePath+ "?id=";
            var baoJingLeiXing = {
                "ADAS": ["ZHBJSJ017", "ZHBJSJ018", "ZHBJSJ019", "ZHBJSJ020", "ZHBJSJ021", "ZHBJSJ022", "ZHBJSJ023"],
                "DMS": ["ZHBJSJ000", "ZHBJSJ001", "ZHBJSJ002", "ZHBJSJ003", "ZHBJSJ004", "ZHBJSJ005", "ZHBJSJ006", "ZHBJSJ007", "ZHBJSJ008", "ZHBJSJ009", "ZHBJSJ010", "ZHBJSJ011", "ZHBJSJ012", "ZHBJSJ013", "ZHBJSJ014", "ZHBJSJ015", "ZHBJSJ016", "ZHBJSJ024"],
                "右路盲区": ["ZHBJSJ027"]
            }

            var init = function () {
                //loginMediaServer();
                loadingData();
                
                $("#btnSave").click(function () {
                    save();
                });
                //关闭
                $('#btnclose').click(function () {
                    tipdialog.confirm("确定关闭？", function (r) {
                        if (r) {
                            parent.window.$("#btnSearch").click();
                            parent.window.$("#ajax-iframe").css("display", "none");
                            parent.window.$("#ajax-iframe").children().children().attr("src", "");
                            //popdialog.closeIframe();
                        }
                    });
                });
            }
            

            function loadingData() {
                var param = cheliangId;
                helper.Ajax("006600200070", param, function (result) {
                    var data = result.body;
                    if (data) {
                        
                        zhongduanId = data.ZhongDuanId;
                        if (data.ShuJuJieRu) {
                            $(".ShuJuJieRu").attr("checked", "checked");
                            $(".ShuJuJieRu").parent().attr("class", "checked");
                        }
                        if (data.LatestGpsTime)
                        {
                            $(".GPS-val").text("√");
                            $(".GPS-time").text(data.LatestGpsTime.replace("T", " "));
                        }
                        if (data.SheBeiWanZheng) {
                            $(".SheBeiWanZheng").attr("checked", "checked");
                            $(".SheBeiWanZheng").parent().attr("class", "checked");
                        }
                        $(".videodiv").empty();
                        fillFormData(data, "Form1");
                        loadingBaoJingData();
                        $(".SheBeiJiShenLeiXing").each(function (index, item) {
                            if ($(item).val() == data.SheBeiJiShenLeiXing) {
                                $(item).attr("checked", "checked");
                                $(item).parent().attr("class", "checked");
                            }
                        });
                        $(".SheBeiGouCheng").each(function (index, item) {
                            if (data.SheBeiGouCheng) {
                                var sheBeiGouCheng = data.SheBeiGouCheng.split("|");
                                for (var i = 0; i < sheBeiGouCheng.length; i++) {
                                    if ($(item).val() == sheBeiGouCheng[i]) {
                                        $(item).attr("checked", "checked");
                                        $(item).parent().attr("class", "checked");
                                    }
                                }
                            }
                        });
                        if (data.FileList && data.FileList.length > 0) {
                            for (var i = 0; i < data.FileList.length; i++) {
                                var item = $(".img-content")[data.FileList[i].FileType - 1];
                                $(item).empty();
                                $(item).append('<img src="' + fileUrl + data.FileList[i].FileId + '" style="width:100%;height:100%;"/>');
                            }
                        }
                        if (data.ShiPinTouGeShu != null && data.ShiPinTouGeShu > 0 && data.ShiPinTouAnZhuangXuanZe) {
                            var shiPinTou = data.ShiPinTouAnZhuangXuanZe.split(",");
                            for (var i = 0; i < shiPinTou.length; i++) {
                                var shiPinTouData = shiPinTou[i].split("|");
                                var videohtml = `<div class="video-box">
                                                    <div class="video-title">通道${shiPinTouData[0]}</div>
                                                    <div class="video-content" >
                                                        <video id="videoElement${shiPinTouData[0]}" controls="controls" width="100%" height="100%" object-fit="fill" autoplay>Not supported</video>
                                                    </div>
                                                </div>`;
                                
                                $(".videodiv").append(videohtml);
                                playVideoByTypeTwo(data.ChePaiHao, data.ChePaiYanSe, shiPinTouData[0], 1);
                            }
                        }
                    }
                    else {
                        if (result.publicresponse.message) {
                            tipdialog.alertMsg(result.publicresponse.message);
                        } else {
                            tipdialog.alertMsg("该车辆无终端设备信息！");
                            setTimeout(function () {
                                
                                parent.window.$("#btnSearch").click();
                                parent.window.$("#ajax-iframe").css("display", "none");
                                parent.window.$("#ajax-iframe").children().children().attr("src", "");
                                //popdialog.closeIframe();
                            },3000);
                            
                        }
                        
                    }
                }, false);
            }


            function loadingBaoJingData() {
                var param = {
                    "ChePaiHao": $("#ChePaiHao").val(),
                    "ChePaiYanSe": $("#ChePaiYanSe").val(),
                    //"EventTypeCode": "CLCS001",
                    //"StartTime": "2020-09-29 08:03:00"
                }
                helper.Ajax("006600300024", param, function (result) {
                    if (result.publicresponse.statuscode == 0) {
                        var data = result.body;
                        var adasTime, dmsTime, ylmqTime, qtTime;
                        if (data) {
                            for (var i = 0; i < data.length; i++) {
                                switch (checkExistBaoJing(data[i].EventTypeCode)) {
                                    case "ADAS":
                                        if (adasTime) {
                                            if (compare(data[i].StartTime, adasTime)) {
                                                adasTime = data[i].StartTime;
                                            }
                                        } else {
                                            adasTime = data[i].StartTime;
                                        }; break;
                                    case "DMS":
                                        if (dmsTime) {
                                            if (compare(data[i].StartTime, dmsTime)) {
                                                dmsTime = data[i].StartTime;
                                            }
                                        } else {
                                            dmsTime = data[i].StartTime;
                                        }; break;
                                    case "右路盲区":
                                        if (ylmqTime) {
                                            if (compare(data[i].StartTime, ylmqTime)) {
                                                ylmqTime = data[i].StartTime;
                                            }
                                        } else {
                                            ylmqTime = data[i].StartTime;
                                        }; break;
                                    default:
                                        if (qtTime) {
                                            if (compare(data[i].StartTime, qtTime)) {
                                                qtTime = data[i].StartTime;
                                            }
                                        } else {
                                            qtTime = data[i].StartTime;
                                        }; break;
                                }
                            }

                            if (adasTime) {
                                $(".ADAS-val").text("√");
                                $(".ADAS-time").text(adasTime.replace("T"," "));
                            }
                            if (dmsTime) {
                                $(".DMS-val").text("√");
                                $(".DMS-time").text(dmsTime.replace("T", " "));
                            }
                            if (ylmqTime) {
                                $(".YLMQ-val").text("√"); 
                                $(".YLMQ-time").text(ylmqTime.replace("T", " "));
                            }
                            if (qtTime) {
                                $(".QT-val").text("√");
                                $(".QT-time").text(qtTime.replace("T", " "));
                            }
                        } else {
                            tipdialog.alertMsg("获取数据失败，返回数据为空");
                        }
                    }
                    else {
                        tipdialog.alertMsg(result.publicresponse.message);
                    }
                }, false);
            }

            function save() {
                var param = {
                    "CheLiangId": cheliangId,
                    "ZhongDuanId": zhongduanId,
                    "ShuJuJieRu": $(".ShuJuJieRu").parent().attr("class")=="checked"?true:false,
                    "SheBeiWanZheng": $(".SheBeiWanZheng").parent().attr("class") == "checked" ? true : false,
                    "NeiRong": $("#NeiRong").val()
                }
                helper.Ajax("006600200071", param, function (result) {
                    if (result.publicresponse.statuscode == 0) {
                        var data = result.body;
                        if (data) {
                            toast.success('保存成功！');
                            setTimeout(function () {
                                
                                parent.window.$("#btnSearch").click();
                                parent.window.$("#ajax-iframe").css("display", "none");
                                parent.window.$("#ajax-iframe").children().children().attr("src", "");
                                //popdialog.closeIframe();
                            }, 3000);
                        } else {
                            tipdialog.alertMsg("获取数据失败，返回数据为空");
                        }
                    }
                    else {
                        tipdialog.alertMsg(result.publicresponse.message);
                    }
                }, false);
            }


            //登陆流媒体
            function loginMediaServer() {
                helper.Ajax("003300200050", null, function (data) {
                    if (data.publicresponse.statuscode == 0) {
                        defaultConfig.authCode = data.body.AuthCode;
                        defaultConfig.port = data.body.Port;
                        defaultConfig.ip = data.body.Ip;
                        defaultConfig.ClientId = data.body.ClientId;
                        authCode = defaultConfig.authCode;

                        //心跳
                        sendHeartBeatTimer = setInterval(sendHeartBeat, 10 * 1000);
                        console.log("成功登录媒体服务器");
                        //playVideoByTypeTwo("粤RV1595", "黄色", "1", 1);
                        //playVideoByTypeTwo("粤E44410", "黄色", "2", 1);
                        //playVideoByTypeTwo("粤E44410", "黄色", "3", 1);
                    } else {
                        toast.success('帐号异常，请联系管理员！');
                    }
                });
            }
            //保持流媒体心跳
            function sendHeartBeat() {
                var data = { "auth_code": defaultConfig.authCode };
                $.ajax({
                    type: "post",
                    url: "http://" + defaultConfig.ip + ":" + defaultConfig.port + "/api/sys_mng/keep_alive",
                    data: 'params=' + JSON.stringify(data),
                    dataType: "jsonp",
                    jsonp: "callback",
                    jsonpCallback: "jsonpCallback",
                    success: function (data) {
                        if (data.result == 1) {
                            popdialog.errorDialog("未授权！");
                            if (sendHeartBeatTimer !== null) {
                                clearInterval(sendHeartBeatTimer);
                            }
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        //that.errorCallback(errorThrown);
                    }
                });
            }

            function playVideoByTypeTwo(vehicleNo,vehicleColor,tongDao,videoKind) {
                var player = null;
                var opt = {};
                //opt.element = "ywPlayer0";
                opt.ip = defaultConfig.ip;
                opt.port = defaultConfig.port;
                opt.width = "100%";
                opt.height = "100%";
                opt.index = tongDao;
                opt.divId = "";
                opt.authCode = defaultConfig.authCode;
                opt.errorCallback = function (msg, i) {
                    console.log("error");
                };
                //opt.promptCallback = Toast.message;
                opt.openAndPlayCallback = function (i) {
                    console.log(1);
                };
                opt.closeCallback = function (i) {
                    console.log(2);
                };
                opt.refreshCallback = function (i, vehicleNo, chan) {
                    console.log(3);
                };
                player = new Cwplayer();
                player.init(opt);
                player.play(vehicleNo, vehicleColor, tongDao, videoKind);
                return player;
            }
            function compare(date1, date2) {
                var oDate1 = new Date(date1);
                var oDate2 = new Date(date2);
                if (oDate1.getTime() > oDate2.getTime()) {
                    return true;
                }
                return false;
            }

            function checkExistBaoJing(code) {
                for (var i = 0; i < baoJingLeiXing["ADAS"].length; i++) {
                    if (code == baoJingLeiXing["ADAS"][i]) {
                        return "ADAS";
                    }
                }
                for (var i = 0; i < baoJingLeiXing["DMS"].length; i++) {
                    if (code == baoJingLeiXing["DMS"][i]) {
                        return "DMS";
                    }
                }
                if (code == baoJingLeiXing["右路盲区"][0]) {
                    return "右路盲区";
                }
                return null;
            }

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
            
            init();
        });
});