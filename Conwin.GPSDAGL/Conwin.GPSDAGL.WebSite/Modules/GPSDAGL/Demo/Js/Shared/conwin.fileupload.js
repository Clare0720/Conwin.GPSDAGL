var fileupload = function (_this, wd) {
    var currentFireObj, fileHostAddress;
    var uploadAddress = '/File/AddFile',
		viewAddress = '/api/v1/FileModule/File/GetFile?id=',
        //多个删除用英文逗号隔开
		remoteAddress = '/File/DeleteFile?fileIds=';
    return {
        //获取当前触发事件的对象
        //return {ButtonName:'',TargetName:''}
        getCurrentFireObj: function () { return currentFireObj; },
        //设置附件远程地址
        setFileHostAddress: function (remoteUrl) {
            fileHostAddress = remoteUrl;
        },
        //绑定上传按钮事件
        //argArr: [{ButtonName:'',TargetName:''},...]
        //businessId: 记录ID
        bindFileUploadButtonEvent: function (argArr, businessId) {
            $(argArr).each(function (i, item) {
                if (!item) return false; //ie8
                $('#' + item.ButtonName).click(function (e) {
                    e.preventDefault();
                    currentFireObj = item;
                    AjaxWindow.showModal({
                        url: uploadAddress,
                        data: {
                            BusinessId: businessId,
                            Extends: '01|02|03',     //自定义扩展名限制
                            FileSize: 1     //自定义文件大小限制，单位mb，最大为6mb
                        }
                    });
                });
            });
        },
        //上传文件成功后，移除“浏览”按钮，替换为“查看”和“删除”按钮
        rebindSingleFileButton: function (operatorObj, isReadOnly) {
            var viewId = operatorObj.TargetName + 'View',
                deleteId = operatorObj.TargetName + 'Delete',
                targetId = $('#' + operatorObj.TargetName).val(),
                htmlStr = '';
            htmlStr = '<a id="' + viewId + '" href="' + viewAddress + targetId + '" target="_blank" class="btn green">查看</a>';
            if (!isReadOnly) {
                htmlStr += '<a id="' + deleteId + '" href="'  + remoteAddress + targetId + '" class="btn green">删除</a>'
            }
            $("#" + operatorObj.ButtonName).parent().prepend(htmlStr);
            //移除浏览按钮
            var uploadFileBtnDom = $("#" + operatorObj.ButtonName).detach();
            if (!isReadOnly) {
                //删除文件之后移除“查看”和“删除”按钮，恢复“浏览”按钮
                $("#" + deleteId).live('click', function (e) {
                    e.preventDefault();
                    $("#" + viewId).parent().prepend(uploadFileBtnDom);
                    $("#" + viewId).remove();
                    $("#" + deleteId).remove();
                    $("#" + operatorObj.TargetName).val('');
                    if (typeof fileDeleteSuccessCustomCallback == 'function') {
                        fileDeleteSuccessCustomCallback(operatorObj);
                    }
                    return false; //note: 删除附件返回 html 待解决，之后修改为 return true;
                });
            }
        },
        //上传文件成功后，移除“浏览”按钮，替换为“查看”和“删除”按钮
        rebindFileButton: function (operatorObjArr, isReadOnly) {
            $(operatorObjArr).each(function (i, item) {
                if (!item) return false;//ie8
                if ($('#' + item.TargetName).val()) {
                    fileupload.rebindSingleFileButton(item, isReadOnly);
                } else {
                    if (isReadOnly) {
                        //只读，移除上传按钮
                        $('#' + item.ButtonName).parent().append('<label class="form-control-static">&nbsp;</label>');
                        $('#' + item.ButtonName).remove();
                    }
                }
            });
        }
    };
}(fileupload || {}, window);
//上传成功回调
var UploadSuccessCallback = function (d) {
    //获取当前操作对象
    var curr = fileupload.getCurrentFireObj();
    //将返回的文件ID绑定到目标域
    $('#' + curr.TargetName).val(d.FileId);
    //上传文件成功后，移除“浏览”按钮，替换为“查看”和“删除”按钮
    fileupload.rebindSingleFileButton(curr);
    if (typeof fileUploadSuccessCustomCallback == 'function') {
        fileUploadSuccessCustomCallback(curr);
    }
};