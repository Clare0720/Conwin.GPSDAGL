define(["jquery", 'helperCallBack', "lame", "alaw", "cms", "config", "inherites", "event-emitter", "exception", "utf8-conv", "browser", "polyfill", "logging-control", "inherites", "cms-demuxer", "cmsimg-demuxer", "flv-demuxer", "exp-golomb", "amf-parser", "sps-parser", "demux-errors", "img-remuxer", "mp4-remuxer", "aac-silent", "mp4-generator", "features", "media-segment-info", "mse-events", "media-info", "mse-controller", "transmuxing-controller", "transmuxing-events", "transmuxing-worker", "transmuxer", "loader", "io-controller", "range-seek-handler", "fetch-stream-loader", "xhr-moz-chunked-loader", "param-seek-handler", "xhr-msstream-loader", "speed-sampler", "xhr-range-loader", "flv-player", "player-events", "native-player", "player-errors", "logger", "flv"], function ($, helper) {
    //初始化数据
    function cwplayer() {
        this.count = 1;
        this.checkStop = null;
        this.seekLength = 0;
        this.hasInit = false;
        this.channelsIndex = -1;
        this.channels = null;
        this.errorArray = [];
        this.ip = null;
        this.port = null;
        this.width = null;
        this.height = null;
        this.authCode = null;
        this.mainObjct = null;
        this.errorCallback = null;
        this.openAndPlayCallback = null;
        this.refreshCallback = null;
        //无直播流刷新计时器
        this.noLiveStreamTimeOut = null;
        //无直播流刷新时长
        this.noLiveStreamTime = 10 * 1000;
        this.setTimeoutLength = 5 * 60 * 1000;
        this.ajaxErrorTimeout = null;
        this.ajaxErrorTimes = 5;
        this.divIndex = 0;
        this.CwMediaPlayer = null;
        this.vehicleNo = null;
        this.vehicleColor = null;
        this.videoServiceKind = null;
        this.closeCallback = null;
        this.playing = false;
        this.playerType = 2;
        this.playerUrl = "103.56.76.161";
        this.ipReg = /^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/;
    }
    //播放
    cwplayer.prototype.play = function (plateNo, platecolor, channelNo, videoServiceKind) {
        //this.stopAll();
        //if (this.isPlaying()) {
        //    this.stopAll();
        //    //console.log("播放正在进行中...");
        //}

        if (!this.hasInit) {
            throw new Error("尚未进行初始化init");
        }
        this.errorArray = [];
        this.channels = channelNo.split(/,|，/);
        //this.splitScreen();
        this.vehicleNo = plateNo;
        this.vehicleColor = platecolor;
        this.videoServiceKind = videoServiceKind;
        if (!videoServiceKind) {
            videoServiceKind = 2;
        }
        if (videoServiceKind != 1) {//车辆所属平台类型：1、紫光，2、有为
            this.validate();//判断是否需要登录
            this.openAndPlay(plateNo);
        } else {
            this.openAndPlayByGdunis(plateNo, platecolor);
        }
    };
    function getUrl(index) {
        var urlary = ["video1.gdunis.com", "video2.gdunis.com", "video3.gdunis.com", "video4.gdunis.com"];
        return urlary[parseInt((index) / 4)];
    }
    cwplayer.prototype.validate = function () {
        if (!this.authCode) {
            throw new Error("您未登录媒体服务器，请先登录媒体服务器！");
        }

        if (!this.ip || !this.ipReg.test(this.ip)) {
            throw new Error("请输入合法的ip地址！");
        }


        if (!this.port || isNaN(this.port)) {
            throw new Error("请输入媒体服务器端口！");
        }

    };
    cwplayer.prototype.openAndPlayByGdunis = function (vehicleNo, vehicleColor) {
        console.log(vehicleNo + vehicleColor);
        var _this = this;
        var para = {
            "registrationno": vehicleNo,
            "registrationcolor": vehicleColor,
            "logicdeviceno": _this.channels[0]
        };
        _this.channelsIndex++;
        helper.Ajax("003300100036", para, function (result) {
            if (result.publicresponse.statuscode == 0) {
                if (result.body) {
                    var urlary = JSON.parse(result.body);
                    var urlIndex = parseInt((_this.divIndex) / 4);
                    if (urlIndex > urlary.length - 1) {
                        urlIndex = urlary.length - 1;
                    }
                    var url = urlary[urlIndex];
                    if (flvjs.isSupported()) {
                        var videoElement = document.getElementById('videoElement' + _this.divIndex);
                        var flvPlayer = flvjs.createPlayer({
                            isLive: true,
                            type: 'cms',
                            url: url
                        });
                        _this.CwMediaPlayer = flvPlayer;
                        flvPlayer.attachMediaElement(videoElement);
                        flvPlayer.load();
                        _this.openAndPlayCallback(_this.divIndex)
                        _this.addClickListener();
                        console.log(_this.CwMediaPlayer);
                    }

                }
            } else {
                _this.errorCallback(result.publicresponse.message, _this.divIndex, _this.channels[0]);
            }
        }, false, function () {
                _this.errorCallback(vehicleNo + " 摄像头" + _this.channels[0] + "：网络请求错误", _this.divIndex, _this.channels[0]);
        });

    }
    cwplayer.prototype.openAndPlay = function (vehicleNo) {
        var _this = this;
        //_this.playerUrl = getUrl(_this.divIndex);
        _this.channelsIndex++;
        $.ajax({
            type: "POST",
            url: "http://" + _this.playerUrl + ":8011/open/" + vehicleNo + "_" + _this.channels[0] + ".flv",
            data: 'params=' + JSON.stringify({
                "auth_code": _this.authCode
            }),
            dataType: "jsonp",
            jsonp: "callback",
            jsonpCallback: "jsonpCallback",
            success: function (data) {
                var result = data.result;
                window.clearTimeout(_this.ajaxErrorTimeout);
                _this.ajaxErrorTimeout = null;
                if (result == 0) {
                    var params = '"{\"auth_code\":\"' + _this.authCode + '\"}"';
                    var url = "http://" + _this.playerUrl + ":8011/live/" + vehicleNo + "_" + _this.channels[0] + ".flv?params=" + params +
                        "&subid=" + data.subid;
                    if (flvjs.isSupported()) {
                        var videoElement = document.getElementById('videoElement' + _this.divIndex);
                        var flvPlayer = flvjs.createPlayer({
                            isLive: true,
                            type: 'flv',
                            url: url
                        });
                        _this.CwMediaPlayer = flvPlayer;
                        flvPlayer.attachMediaElement(videoElement);
                        flvPlayer.load();
                        console.log(flvPlayer);
                        _this.openAndPlayCallback(_this.divIndex)
                        _this.addClickListener();
                    }
                } else {
                    _this.ajaxErrorTimes--;
                    _this.channelsIndex = -1;
                    window.clearTimeout(_this.ajaxErrorTimeout);
                    _this.ajaxErrorTimeout = null;

                    if (_this.ajaxErrorTimes < 0) {
                        if (_this.errorCallback !== null) {
                            _this.errorCallback(vehicleNo + " 摄像头" + _this.channels[0] + "：网络请求错误", _this.divIndex);
                        }
                    } else {
                        if (_this.ajaxErrorTimeout == null) {
                            _this.ajaxErrorTimeout = setTimeout(function () {
                                _this.openAndPlay(vehicleNo);
                            }, 500);
                        }
                    }
                }

            },
            error: function (jqXHR) {
                _this.ajaxErrorTimes--;
                window.clearTimeout(_this.ajaxErrorTimeout);
                _this.ajaxErrorTimeout = null;

                if (_this.ajaxErrorTimes < 0) {
                    if (_this.errorCallback !== null) {
                        _this.errorCallback(vehicleNo + " 摄像头" + _this.channels[_this.channelsIndex] + "：网络请求错误", _this.divIndex);
                    }
                } else {
                    if (_this.ajaxErrorTimeout == null) {
                        _this.ajaxErrorTimeout = setTimeout(function () {
                            _this.openAndPlay(vehicleNo);
                        }, 500);
                    }
                }
            }
        });


    }
    cwplayer.prototype.refresh = function () {
        this.CwMediaPlayer.unload();
        this.CwMediaPlayer.load();
    }

    cwplayer.prototype.pause = function (index) {
        if (this.CwMediaPlayer) {
            this.CwMediaPlayer.pause();
            this.playing = false;
        }
    }
    cwplayer.prototype.start = function (index) {
        if (this.CwMediaPlayer) {
            
            this.CwMediaPlayer.play();
            this.playing = true;
        }
    }
    cwplayer.prototype.init = function (option) {
        this.ip = option.ip;
        this.port = option.port;
        this.width = option.width;
        this.height = option.height;
        this.authCode = option.authCode;
        this.hasInit = true;
        this.errorCallback = option.errorCallback; // warning.errorDialog;
        //this.promptCallback = Toast.message;//option.promptCallback;
        this.openAndPlayCallback = option.openAndPlayCallback;
        this.closeCallback = option.closeCallback
        this.refreshCallback = option.refreshCallback;
        this.divIndex = option.index;
        this.key = option.key;
        
    };
    cwplayer.prototype.dispose = function () {
        this.hasInit = false;
        this.playing = false;
        this.channelsIndex = -1;
        this.channels = null;
        this.errorArray = [];

        this.ip = null;
        this.port = null;
        this.width = null;
        this.height = null;
        this.authCode = null;
        this.mainObjct = null;
        this.CwMediaPlayer.unload();
        //setTimeout(function () { this.CwMediaPlayer = null; }, 0);
    };

    function checkStopVdieo(videoElement) {
        var _this = videoElement;
        try {
            if (_this.seekLength == 0) {
                _this.seekLength = _this.CwMediaPlayer._mediaElement.seekable.end(0)
            } else {
                if (_this.CwMediaPlayer._mediaElement.seekable.end(0) >= 4000000) {
                    _this.refresh();
                    clearInterval(_this.checkStop);
                    _this.checkStop = null;
                    _this.seekLength = 0;
                    return;
                } else {
                    _this.seekLength = _this.CwMediaPlayer._mediaElement.seekable.end(0);
                }
            }
            
            //_this.CwMediaPlayer._mediaElement.buffered.end(0)
        } catch (e) {
            _this.refresh();
            clearInterval(_this.checkStop);
            _this.checkStop = null;
        }
    }
    
    cwplayer.prototype.addClickListener = function () {
        var _this = this;
        $("#PlayWindow" + _this.divIndex + " .bar-vehicleInfo").text(_this.vehicleNo + "-监控" + _this.channels[0]);
        _this.CwMediaPlayer.on("error", function (e) {
            console.log(e);
            _this.refresh();

        });
        
        $(_this.CwMediaPlayer._mediaElement).on("loadstart", function (e) {
            //console.log($("#container" + _this.divIndex + " .video-loading").css("display"));
            if ($("#container" + _this.divIndex + " .video-loading").css("display") == "none") {
                var x = $("#container" + _this.divIndex).width() / 2 - 25;
                var y = $("#container" + _this.divIndex).height() / 2 - 25;
                var h = `<div class="buffer-loading" style="position:relative;top:${y + "px"};left:${x + "px"};width:50px;height:50px;border-radius:25px !important;z-index:999;background-color:black" > <img id="loadingImg" style="height:99%;width:99%;" src="/Modules/VideoMonitor/page/Img/oval.svg" alt="loading" /></div>`;
                $("#container" + _this.divIndex).prepend(h);
                $("#flash" + _this.divIndex).css("margin-top", "-50px");
            }
        });
        $(_this.CwMediaPlayer._mediaElement).on('playing', function (e) { console.log(e); });
        $(_this.CwMediaPlayer._mediaElement).on('canplay', function () {
            console.log("canplay");
            if (_this.checkStop == null) {
                _this.checkStop = setInterval(function (data) { checkStopVdieo(data) }, 1000,_this);
            }

                
            _this.start();
        });
        //$(_this.CwMediaPlayer._mediaElement).on('play', function () {

        //    $("#container" + _this.divIndex + " .buffer-loading").remove();
        //    $("#flash" + _this.divIndex).css("margin-top", "0px");
        //    $("#flash" + _this.divIndex).css("display", "block");
        //    $("#container" + _this.divIndex + " .video-loading").css("display", "none");
        //    $("#container" + _this.divIndex + " .jwplayer").css("display", "block");
        //});

        $(_this.CwMediaPlayer._mediaElement).on('error', function () {
            console.log("error");
            
            var obj = _this.cyberPlayer;
            if (_this.refreshCallback !== null && typeof (_this.refreshCallback) === "function") {
                _this.refreshCallback(_this.divIndex, _this.vehicleNo, _this.channels[0]);
            }
            if (obj) {
                //obj.stop();
                obj.remove();
            }
        });

        //$(_this.CwMediaPlayer._mediaElement).on('noLiveStream', function () {
        //    if ($("#container" + cwplayer.divIndex + " .video-loading").css("display") == "none") {
        //        if (cwplayer.noLiveStreamTimeOut == null) {
        //            console.log("noLiveStream-->>" + cwplayer.divIndex);
        //            cwplayer.noLiveStreamTimeOut = setTimeout(function () {
        //                var obj = cwplayer.cyberPlayer;
        //                if (cwplayer.refreshCallback !== null && typeof (cwplayer.refreshCallback) === "function") {
        //                    cwplayer.refreshCallback(cwplayer.divIndex, cwplayer.vehicleNo, cwplayer.channels[0]);
        //                }
        //                if (obj) {
        //                    //obj.stop();
        //                    obj.remove();
        //                }
        //            }, cwplayer.noLiveStreamTime);
        //        }
        //    }
        //});

        //cwplayer.cyberPlayer.on('alive', function () {
        //    console.log("alive-->>" + cwplayer.divIndex);
        //    window.clearTimeout(cwplayer.noLiveStreamTimeOut);
        //    cwplayer.noLiveStreamTimeOut = null;
        //});
        //cwplayer.cyberPlayer.on('pause', function () {
        //    console.log("pause");
        //    //window.clearTimeout(cwplayer.noLiveStreamTimeOut);
        //    //cwplayer.noLiveStreamTimeOut = null;
        //});


        //双击最大化、还原操作
        $(_this.CwMediaPlayer._mediaElement).unbind("dblclick");
        $(_this.CwMediaPlayer._mediaElement).dblclick(function (event) {
            event.stopPropagation();
            if (_this.CwMediaPlayer._mediaElement.webkitDisplayingFullscreen) {
                _this.CwMediaPlayer._mediaElement.webkitExitFullScreen();
            } else {
                _this.CwMediaPlayer._mediaElement.webkitRequestFullScreen();
            }


        });

        //视频的播放、暂停
        $("#ywPlayer" + this.divIndex + " .play-pause").unbind("click");
        $("#ywPlayer" + this.divIndex + " .play-pause").click(function (event) {
            event.stopPropagation();
            var obj = _this.CwMediaPlayer._mediaElement;
            if (obj) {
                if (!obj.paused) {
                    _this.pause();
                    $(this).attr("title", locale.play).attr("class", "glyphicon glyphicon-play play-pause playToolBar");
                } else {
                    _this.start();
                    $(this).attr("title", locale.pause).attr("class", "glyphicon glyphicon-pause play-pause playToolBar");
                }
            }
        });


        //关闭音视频
        $("#ywPlayer" + this.divIndex + " .glyphicon-remove-circle").unbind("click");
        $("#ywPlayer" + this.divIndex + "  .glyphicon-remove-circle").click(function (event) {
            event.stopPropagation();
            var obj = _this.CwMediaPlayer._mediaElement;
            _this.dispose();
            if (_this.closeCallback !== null && typeof (_this.closeCallback) === "function") {
                _this.closeCallback(_this.divIndex);
            }
            if (obj) {
                $(obj).remove();
            }

        });

        //截屏
        $("#ywPlayer" + this.divIndex + " .glyphicon-picture").unbind("click");
        $("#ywPlayer" + this.divIndex + " .glyphicon-picture").click(function (event) {
            event.stopPropagation();
            var playObj = $(this).parent().parent()[0];
            var id = $(playObj).attr("id");
            var index = id.replace("PlayWindow", "");
            var obj = _this.CwMediaPlayer;

            //if (obj.getState() != "playing" && obj.getState() != "paused") {
            //    return;
            //}

            var canvasId = "canvas" + index;
            var isCanvas = document.querySelector("#" + canvasId);
            if (isCanvas == null) {
                var str = "<canvas id=" + canvasId + " style='display:none'></canvas>";
                $(playObj).append(str);
            }

            var canvas = $("#canvas" + index)[0];
            var ctx = canvas.getContext('2d');

            var video = $(playObj).find("video")[0];
            var w = _this.CwMediaPlayer._mediaElement.scrollWidth;//视频原有尺寸  
            var h = _this.CwMediaPlayer._mediaElement.scrollHeight;//视频原有尺寸  
            $(canvas).attr({
                width: w,
                height: h,
            });

            ctx.drawImage(video, 0, 0, w, h);
            var pictureName = _this.vehicleNo + "_" + dateFtt("yyyyMMddhhmmss", new Date()) + ".jpg";
            canvas.toBlob(function (blob) {
                saveAs(blob, pictureName);
            }, "image/jpeg", 0.95);
        });


        //录像
        var fileExtension = "webm";
        var mimeType = "video/webm";
        $("#ywPlayer" + this.divIndex + " .glyphicon-camera").unbind("click");
        $("#ywPlayer" + this.divIndex + " .glyphicon-camera").click(function (event) {
            event.stopPropagation();
            var playObj = $(this).parent().parent()[0];
            //obj = _this.CwMediaPlayer._mediaElement;

            var img = $(playObj).find("img")[0];
            var display = $(img).css("display");
            if (display == "none") {
                $(img).css("display", "inline");
                $(this).attr("title", locale.stopRecord);
                img.mediaCapturedCallback = function () {
                    var options = {};
                    if (typeof MediaRecorder !== 'undefined') {
                        img.blobs = [];
                        options.ondataavailable = function (blob) {
                            img.blobs.push(blob);
                        };
                    }
                    img.recordRTC = RecordRTC(img.stream, options);
                    img.recordRTC.startRecording();
                };

                var commonConfig = {
                    onMediaCaptured: function (stream) {
                        img.stream = stream;
                        img.mediaCapturedCallback();
                    },

                    onMediaCapturingFailed: function (error) {
                        console.error('onMediaCapturingFailed:', error);
                        if (error.toString().indexOf('no audio or video tracks available') !== -1) {
                            _this.errorCallback('RecordRTC failed to start because there are no audio or video tracks available.');
                        }
                        if (error.name === 'PermissionDeniedError' && DetectRTC.browser.name === 'Firefox') {
                            _this.errorCallback('Firefox requires version >= 52. Firefox also requires HTTPs.');
                        }
                        commonConfig.onMediaStopped();
                    }
                };

                captureAudioPlusVideo(commonConfig, playObj);
            } else {
                $(img).css("display", "none");
                $(this).attr("title", locale.startRecord);
                function stopStream() {
                    if (img.stream && img.stream.stop) {
                        img.stream.stop();
                        img.stream = null;
                    }

                    if (img.stream instanceof Array) {
                        img.stream.forEach(function (stream) {
                            stream.stop();
                        });
                        img.stream = null;
                    }
                }

                if (img.recordRTC) {
                    img.recordRTC.stopRecording(function () {
                        saveToDisk(img.recordRTC, _this.vehicleNo);
                        stopStream();
                    });
                }
                return;
            }
        });

        function captureAudioPlusVideo(config, playObj) {
            captureMediaStream(function (audioVideoStream) {
                config.onMediaCaptured(audioVideoStream);
            }, function (error) {
                config.onMediaCapturingFailed(error);
            }, playObj
            );
        }

        //获取媒体流
        function captureMediaStream(successCallback, errorCallback, playObj) {
            var stream = null;
            var userAgent = navigator.userAgent;
            if (userAgent.indexOf("Firefox") > -1) {
                stream = $(playObj).find("video")[0].mozCaptureStream();
            } else if (userAgent.indexOf("Chrome") > -1) {
                stream = $(playObj).find("video")[0].captureStream();
            } else {
                alert("请使用谷歌浏览器或者火狐浏览器！");
            }

            successCallback(stream);
        }

        //将录制的视频文件保存到本地磁盘
        function saveToDisk(recordRTC, plateNo) {
            var fileName = getFileName(fileExtension, plateNo);
            if (!recordRTC) {
                return alert('No recording found.');
            }
            var file = new File([recordRTC.getBlob()], fileName, {
                type: mimeType
            });
            invokeSaveAsDialog(file, file.name);
        }

        //生成视频文件名
        function getFileName(fileExtension, plateNo) {
            var d = new Date();
            var dateStr = dateFtt("yyyyMMddhhmmss", d);
            return plateNo + "_" + dateStr + '.' + fileExtension;
        }

        //声音的关闭开启
        $("#ywPlayer" + this.divIndex + " .volume-icon").unbind("click");
        $("#ywPlayer" + this.divIndex + " .volume-icon").click(function (event) {
            event.stopPropagation();
            var playObj = $(this).parent().parent()[0];

            var obj = _this.CwMediaPlayer._mediaElement;
            if (obj) {
                var volumn = $(this).attr("title");
                var video = $(playObj).children("div").find("video")[0];
                if (volumn === locale.volumeOff) {
                    $(this).attr("title", locale.volumeOn).attr("class", "glyphicon glyphicon-volume-off volume-icon playToolBar");
                    obj.muted = true;
                } else {
                    $(this).attr("title", locale.volumeOff).attr("class", "glyphicon glyphicon-volume-up volume-icon playToolBar");
                    obj.muted = false;
                }
            }

        });





        //刷新
        $("#ywPlayer" + this.divIndex + " .glyphicon-refresh").unbind("click");
        $("#ywPlayer" + this.divIndex + " .glyphicon-refresh").click(function (event) {
            event.stopPropagation();
            if (_this.refreshCallback !== null && typeof (_this.refreshCallback) === "function") {
                _this.refreshCallback(_this.divIndex, _this.vehicleNo, _this.channels[0], _this.vehicleColor, _this.videoServiceKind);
            }
            var obj = _this.CwMediaPlayer._mediaElement;
            if (obj) {
                _this.dispose();
                $(obj).remove();
            }

        });


    };

    var dateFtt = function (fmt, date) {
        var o = {
            "M+": date.getMonth() + 1,                 //月份   
            "d+": date.getDate(),                    //日   
            "h+": date.getHours(),                   //小时   
            "m+": date.getMinutes(),                 //分   
            "s+": date.getSeconds()                 //秒   
        };
        if (/(y+)/.test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
        }

        for (var k in o) {
            if (new RegExp("(" + k + ")").test(fmt)) {
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            }
        }
        return fmt;
    };

    return cwplayer;
});