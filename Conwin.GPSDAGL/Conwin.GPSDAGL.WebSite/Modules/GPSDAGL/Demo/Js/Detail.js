/**
 * Created by caihao on 17/2/21.
 */
define(['../../Config/conwin.main'], function () {
    require(['jquery', 'tipdialog', 'helper', 'prevNextpage', 'common'], function ($, warning, helper, prevNextpage, common) {
        var CLXXDetail = {};

        /**
         * 初始化方法
         * **/
        CLXXDetail.Initial = function () {

            //初始化页面 begin 
            //todo:（初始化页面）

            common.AutoFormScrollHeight('#chuZuViewForm');//初始化页面布局
            //初始化上一条下一条按钮
            var ids = parent.window.document.getElementById('hdIDS').value;
            prevNextpage.initPageInfo(ids.split(','));
            prevNextpage.bindPageClass();
            CLXXDetail.updateData();

            //初始化页面 end 

            //绑定按钮触发事件 begin 
            //todo:（绑定按钮触发事件）

            /**
             * 关闭页面按钮事件
             * ***/
            $('#btnclose').click(function () {
                warning.confirm("确定要关闭吗？", function () {
                    $('body').removeClass('rui-window-open');
                    parent.document.body.className = "";
                    parent.window.$iframe.css({ display: 'none' });
                });
            });

            /**
             * 上一条按钮事件
             * ***/
            $('#prevBtn').click(function (e) {
                e.preventDefault();
                prevNextpage.prev();
                CLXXDetail.updateData();
            });

            /**
             * 下一条按钮事件
             * ***/
            $('#nextBtn').click(function (e) {
                e.preventDefault();
                prevNextpage.next();
                CLXXDetail.updateData();
            });

            //绑定按钮触发事件 end 
        };

        /**
         * todo:网络请求获取数据
         * ***/
        CLXXDetail.GetClXXDetail = function (id, callback) {
            helper.Ajax("00020003", ///todo:"00020003"为接口服务地址，需要在/Config/conwin.system.js中配置
                id, function (resultdata) {
                    if (typeof callback == 'function') {
                        callback(resultdata);
                    }
                }, false);
        }

        /**
         * 更新form表单数据
         * ***/
        CLXXDetail.updateData = function (id) {
            var id = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index]
            CLXXDetail.GetClXXDetail(id, function (serviceData) {
                  if (serviceData.publicresponse.statuscode == 0) {
                        CLXXEdit.resource = serviceData.body;
                        CLXXEdit.BindingData();
                    } else {
                        warning.errorDialog("请求数据失败:"+serviceData.publicresponse.message);
                    }
                CLXXDetail.OriginResource = $('#Form1').serializeObject();
            });
        };

        /**
         * 绑定form表单数据
         * ***/
        CLXXDetail.fillFormData = function (resource) {
            $('#Form1').find('.form-control-static').each(function (i, item) {
                var index = $(item).attr('for');
                var tempValue = resource[index];
                if (tempValue != undefined) {
                    if ($(item).hasClass('date')) {
                        tempValue = tempValue.substr(0, 10);
                    }
                    $(item).html(tempValue == '' ? '&nbsp' : tempValue);
                } else {
                    $(item).html('&nbsp;&nbsp;');
                }
            });
            $('#VehicleImage').attr('src', '/api/v1/FileModule/File/GetFile?id=' + resource['VehicleImage']);
            //CLXXDetail.bindUploadFile();
        };

        /**
         * 调用初始化方法
         * */
        CLXXDetail.Initial();
    });

    return {};
});
