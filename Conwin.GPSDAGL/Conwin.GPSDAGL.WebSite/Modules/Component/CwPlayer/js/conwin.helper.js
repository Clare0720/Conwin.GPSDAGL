define(['jquery', 'system', 'metronic'], function ($, SystemConfig, Metronic) {

    'use strict';

    var CwHelper = {}, overTimes = 1000 * 60 * 120; //超时时间

    //返回构造后的AJAX请求参数
    CwHelper.AjaxData = function (request, body, ajax, errorajax) {

        var token = "";

        if (!request && $.trim(request.servicecode) == "") {
            throw CwHelper.ErrorMessage.ERROR0003;
        }

        if ($.type(request) == "string") {
            request = { servicecode: request };
        }

        if ($.type(ajax) == "function") {
            var callback = ajax;
            ajax = {
                success: function (d) {
                    if ($.type(d) == "string") {
                        d = CwHelper.StrToJson(d);
                    }
                    callback(d);
                }
            };
        }

        var publicrequest = $.extend({
            "sysid": SystemConfig.SysId,
            "reqid": CwHelper.NewGuid(),
            "protover": "1.0",
            "servicever": "1.0",
            "requesttime": CwHelper.DateFormat(new Date(), "yyyyMMddHHmmssfff"),
            "signdata": "",
            "reserve": ""
        }, request);

        var data = { publicrequest: publicrequest, body: body };

        var ajaxObj = $.extend({
            crossDomain: true,
            url: CwHelper.Route(publicrequest.servicecode, publicrequest.servicever, SystemConfig.ServerAgent),
            type: 'post',
            cache: false,
            contentType: "application/x-www-form-urlencoded",
            dataType: 'json',
            headers: {
                "token": CwHelper.GetToken(),
                "Source-Type": "76d5f6283a57b2db",
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Headers": "X-Requested-With"
            },
            error: function (request, status, error) {
                var errorObj = { XMLHttpRequest: request, textStatus: status, errorThrown: error };
                var bv = GetBrowserVersion();
                if (request.readyState == 0 && request.statusText.indexOf("拒绝访问") >= 0 && bv.browser == "IE" && (bv.version == 8 || bv.version == 8)) {
                    alert(CwHelper.ErrorMessage.ERROR0004);
                    return;
                }
                if (errorajax != undefined) {
                    if ($.type(errorajax) == "function") {
                        errorajax();
                        return;
                    }
                }
                //TODO:ajax在完成之前请求已经被取消（ajax请求没有发出），会进入这里
                if (request.readyState == 0 && request.statusText == "error") return;
                alert(CwHelper.ErrorMessage.ERROR0002);
            },
            beforeSend: function () {
                //Metronic.blockUI({ animate: true });
            },
            complete: function () {
                var token = CwHelper.GetToken();
                //登录状态下，每次操作设置操作时间+2小时
                if (token != "" && !CwHelper.IsTokenTimeOut()) {
                    var date = new Date();
                    date.setTime(date.getTime() + overTimes);
                    $.cookie('lastOperateTime', date, {
                        path: "/"
                    });
                }
                //Metronic.unblockUI();
            }
        }, ajax, { data: data });

        return ajaxObj;
    }

    //对AJAX进行封装
    CwHelper.Ajax = function (request, body, ajax, login, errorajax) {

        //是否需要登录
        var token = "";
        if (login !== false) {
            token = CwHelper.GetToken();
            if (token == "" || CwHelper.IsTokenTimeOut()) {
                CwHelper.Login();
                return;
            }
        }
        $.support.cors = true;
        var ajaxObj = CwHelper.AjaxData(request, body, ajax, errorajax);
        ajaxObj.data = { '': CwHelper.Encode(CwHelper.JsonToStr(ajaxObj.data)) };
        $.ajax(ajaxObj);
    }

    //获取COOKIE中的TOKEN
    CwHelper.GetToken = function () {
        var token = $.trim($.cookie('token'));
        return token;
    }

    //是否超时
    CwHelper.IsTokenTimeOut = function () {
        var tokenTime = new Date($.cookie('tokenTime'));
        var lastOperateTime = new Date($.cookie('lastOperateTime'));
        var now = new Date();
        //当前时间处于 TOKEN时间 +2小时之后 ，和最后操作时间2小时内
        return (now.getTime() - tokenTime.getTime() > 0) &&
            (now.getTime() - lastOperateTime.getTime() > 0);
    }

    //登陆
    CwHelper.Login = function () {
        require(['login'], function (t) {
            t.ShowLogin();
        });
    }

    //适配旧系统登录
    CwHelper.AdaptLogin = function (token, fn) {
        if (SystemConfig.AdaptLogin === true) {
            $.ajax({
                url: '/DefaultAdapt/Login', type: 'post', data: { 'token': token }, success: function (d) {
                    if (d.success) {
                        fn();
                    } else {
                        alert(CwHelper.ErrorMessage.ERROR0002);
                    }
                }
            });
        } else {
            fn();
        }
    }

    //适配旧系统登出
    CwHelper.AdaptLogout = function (fn) {
        if (SystemConfig.AdaptLogin === true) {
            $.ajax({
                url: '/DefaultAdapt/Logout', type: 'post', data: { 'token': CwHelper.GetToken() }, success: function (d) {
                    if (d.success) {
                        fn();
                    } else {
                        alert(CwHelper.ErrorMessage.ERROR0002);
                    }
                }
            });
        }
        else {
            fn();
        }
    }

    //设置TOKEN
    CwHelper.SetToken = function (token) {
        var time = new Date();
        time.setTime(time.getTime() + overTimes)
        $.cookie('tokenTime', time, {
            path: "/"
        });
        $.cookie('lastOperateTime', time, {
            path: "/"
        });
        var token = $.cookie('token', token, {
            path: "/"
        });
    }

    //登出
    CwHelper.Logout = function () {
        CwHelper.AdaptLogout(function () {
            $.cookie('tokenTime', '', {
                path: "/"
            });
            $.cookie('lastOperateTime', '', {
                path: "/"
            });
            $.cookie('token', '', {
                path: "/"
            });

            //redirtDefault();
            sessionStorage.clear();
            var w = top || this;
            w.location.href = SystemConfig.LoginUrl;
        });
    }

    //用户菜单
    CwHelper.UserMenu = function () {
        require(['menu'], function (menu) {
            var key = CwHelper.GetToken() + "Menu";
            var data = CwHelper.Storage.get(key);
            if (data) {
                menu.SetMenu(CwHelper.StrToJson(data));
            }
            else {
                CwHelper.Ajax("00000030006", { SysId: SystemConfig.SysId, AppId: SystemConfig.AppId, Token: CwHelper.GetToken() }, function (d) {
                    require(['menu'], function (menu) {
                        if (d.publicresponse.statuscode == 0) {
                            menu.SetMenu(d);
                            CwHelper.Storage.set(CwHelper.GetToken() + "Menu", CwHelper.JsonToStr(d));
                        }
                    });
                });
            }
        });
    }

    //页面顶部信息
    CwHelper.Header = function () {
        require(['header'], function (header) {
            CwHelper.UserInfo(function (result) {
                header.SetHeaderInfo(result);
            });
        });
    }

    //临时存储数据
    CwHelper.Storage = (function () {
        var t = sessionStorage;
        var storage = {
            get: function (key) {
                return t.getItem(key);
            },
            set: function (key, value) {
                t.setItem(key, value);
            },
            clear: function () {
                t.clear();
            }
        };
        return storage;
    })();

    //获取服务器时间
    CwHelper.GetServerTime = function () {
        return new Date();
    }

    //用户信息
    CwHelper.UserInfo = function (fn) {
        var key = CwHelper.GetToken() + "UserInfo";
        var data = CwHelper.Storage.get(key);
        if (data) {
            fn(CwHelper.StrToJson(data));
        }
        else {
            CwHelper.Ajax("00000030002", CwHelper.GetToken(), function (d) {
                if (d.publicresponse.statuscode == 0) {
                    CwHelper.Storage.set(key, CwHelper.JsonToStr(d));
                    fn(d);
                }
            });
        }
    }

    CwHelper.GetUserInfo = function () {
        var key = CwHelper.GetToken() + "UserInfo";
        var userInfoResponseStr = CwHelper.Storage.get(key);
        var userInfo = {};
        if (!!userInfoResponseStr) {
            var userInfoResponse = CwHelper.StrToJson(userInfoResponseStr)
            userInfo = userInfoResponse.body;
        }
        return userInfo;
    };

    //编码
    CwHelper.Encode = function (str) {
        return str;
    }

    //解码
    CwHelper.Decode = function (str) {
        return str;
    }

    //将JSON序列化成字符串
    CwHelper.JsonToStr = function (json) {
        return JSON.stringify(json);
    }

    //将字符串反序列化成JSON
    CwHelper.StrToJson = function (str) {
        return JSON.parse(str);
    }

    //产生一个GUID
    CwHelper.NewGuid = function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(
            /[xy]/g,
            function (match) {
                var randomNibble = Math.random() * 16 | 0;
                var nibble = (match == 'y') ?
                    (randomNibble & 0x3 | 0x8) :
                    randomNibble;
                return nibble.toString(16).toUpperCase();
            });
    }

    //配置服务网关路由地址
    CwHelper.Route = function (code, ver, url) {
        var table = SystemConfig.ServiceCodeTable;
        var result = url;
        for (var i = 0; i < table.length; i++) {
            var item = table[i];
            if (item.code == code && ver == (item.ver || '1.0')) {
                result = item.url;
            }
        }

        return result;
    }

    //格式化时间,绑定到时间对象上（注意被覆盖）
    Date.prototype.Format = function (fmt) {
        return CwHelper.DateFormat(this, fmt);
    }

    //格式化时间
    CwHelper.DateFormat = function (date, fmt) {
        var o = {
            "M+": date.getMonth() + 1, //月份 
            "d+": date.getDate(), //日 
            "H+": date.getHours(), //小时 
            "m+": date.getMinutes(), //分 
            "s+": date.getSeconds(), //秒 
            "q+": Math.floor((date.getMonth() + 3) / 3), //季度 
        };
        var milliseconds = date.getMilliseconds();
        if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
        if (/(f+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? milliseconds + '' : (("000" + milliseconds).substr(("" + milliseconds).length)));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return fmt;
    }

    //普通页面判断是否返回首页进行登录还是延时当前凭据时间
    CwHelper.PageLogin = function () {
        try {
            var token = CwHelper.GetToken();
            var tokenTime = new Date($.cookie('tokenTime'));
            var lastOperateTime = new Date($.cookie('lastOperateTime'));
            var now = new Date();

            if (token == "") {
                redirtDefault();
                return;
            }

            //当前时间处于 TOKEN时间 +2小时之后 ，和最后操作时间2小时内
            if ((now.getTime() - tokenTime.getTime() > 0) &&
                now.getTime() - lastOperateTime.getTime() < 0) {
                //延长TOKEN时间
                CwHelper.Ajax('00000030005', token, function (response) {
                    if (response.publicresponse.statuscode === 0 && response.body === "true") {
                        CwHelper.SetToken(token);
                    } else {
                        redirtDefault();
                    }
                }, false);
            } else if ((now.getTime() - tokenTime.getTime() > 0) &&
                now.getTime() - lastOperateTime.getTime() > 0) {
                redirtDefault();
            }
        } catch (error) {
            redirtDefault();
        }
    }

    //指向默认登录页面
    function redirtDefault() {
        sessionStorage.clear();
        var w = top || this;
        w.location.href = SystemConfig.LoginUrl + '?ReturnUrl=' + encodeURIComponent(GetUrlRelativePath(w.location.href));
    }

    //获取相对路径
    function GetUrlRelativePath() {
        var url = document.location.toString();
        var arrUrl = url.split("//");
        var start = arrUrl[1].indexOf("/");
        var relUrl = arrUrl[1].substring(start);
        if (relUrl.indexOf("?") != -1) {
            relUrl = relUrl.split("?")[0];
        }
        return relUrl;
    }

    //获取浏览器和版本号
    function GetBrowserVersion() {
        var userAgent = navigator.userAgent,
            rMsie = /(msie\s|trident.*rv:)([\w.]+)/,
            rFirefox = /(firefox)\/([\w.]+)/,
            rOpera = /(opera).+version\/([\w.]+)/,
            rChrome = /(chrome)\/([\w.]+)/,
            rSafari = /version\/([\w.]+).*(safari)/;
        var browser;
        var version;

        var ua = userAgent.toLowerCase();
        var match = rMsie.exec(ua);

        if (match != null) {
            return { browser: "IE", version: match[2] || "0" };
        }
        var match = rFirefox.exec(ua);
        if (match != null) {
            return { browser: match[1] || "", version: match[2] || "0" };
        }
        var match = rOpera.exec(ua);
        if (match != null) {
            return { browser: match[1] || "", version: match[2] || "0" };
        }
        var match = rChrome.exec(ua);
        if (match != null) {
            return { browser: match[1] || "", version: match[2] || "0" };
        }
        var match = rSafari.exec(ua);
        if (match != null) {
            return { browser: match[2] || "", version: match[1] || "0" };
        }
        return { browser: "", version: "0" };
    };

    var BrowserVersion = GetBrowserVersion();
    CwHelper.BrowserVersion = BrowserVersion;

    //控制台输出日志信息
    CwHelper.Log = function (msg) {
        if (SystemConfig.IsTest) {
            if (console && console.info) {
                console.info(msg);
            }
        }
    }

    //保持Token有效性
    CwHelper.DelayedToken = function () {
        setInterval(function () {
            var token = CwHelper.GetToken();
            if (!!token) {
                CwHelper.Ajax("00000030002", token, function (d) { });
            }
        }, overTimes / 2);
    };

    //错误提示
    CwHelper.ErrorMessage = {
        'ERROR0001': '请重新进行登录',
        'ERROR0002': '出现网络错误，请稍后再试',
        'ERROR0003': '必须填写服务代码',
        'ERROR0004': '初次使用系统，请点击IE浏览器的的“工具->Internet 选项->安全->自定义级别”将“其他”选项中的“通过域访问数据源”选中为“启用”'
    };

    return CwHelper;
});