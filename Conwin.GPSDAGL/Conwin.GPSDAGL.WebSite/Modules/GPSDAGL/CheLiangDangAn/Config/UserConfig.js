define(['/Modules/Config/conwin.main.js'], function () {
    var config = {};
    require(['jquery'], function ($) {

        config.UserName = "A000013";
        config.Passwd = "111111";
        config.ticket = "";

        config.ApiAddress = "http://10.0.64.214:9510";

        //config.getToken = function () {
        //    $.ajax({
        //        url: config.ApiAddress + '/api/ServiceGateway/Authorize',
        //        type: 'POST',
        //        data: {
        //            userName: config.UserName,
        //            password: config.Passwd
        //        },
        //        success: function (result) {
        //            if (result.IsSuccess) {
        //                config.ticket = result.Result;
        //                console.log("认证成功!");
        //            } else {
        //                alert(result.ErrorMessage || '认证失败');
        //            }
        //        }
        //    });

        //};

        /**
         * wcf服务网关调用
         * @param {json} data JSON 对象 传到服务器上的数据 { Data : { } }
         * @param {function} callback 
         */
        config.Ajax = function (ServiceCode, data, callback) {

            var Data = {
                ReqNo: uuid(),
                ServiceCode: ServiceCode,
                Version: "v1.0",
                PageInfo: {
                    "page": 1,
                    "rows": 5
                },
                Data: {}
            };

            $.extend(Data, data);
            var value = JSON.stringify(Data);
            console.log(value);
            $.ajax({
                url: config.ApiAddress + '/api/ServiceGateway/Upload',
                type: 'POST',
                data: { '': value },
                beforeSend: function (XHR) {
                    XHR.setRequestHeader('Authorization', 'BasicAuth ' + config.ticket);
                },
                success: function (result) {
                    try {
                        callback(result);
                    } catch (err) {
                        console.log(err);
                    }
                },
                error: function (xhr, textStatus, error) {
                    if (xhr.status == 401) {
                        console.log("调用报错了!" + error);
                    } else {
                        alert(error);
                    }
                }
            });
        };

        /**
         * GUID生成算法
         */
        function uuid() {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0,
                    v = c == 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        }

        //config.getToken();



    }); //end require
    return config;
}); //end define