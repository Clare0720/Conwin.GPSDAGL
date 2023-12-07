define(['jquery', 'system', 'toast', 'popdialog', 'warning', 'helper'], function ($, system, toast, popdialog, warning, CwHelper) {
    var fileupload = {};
    var optionsArray = [];
    var defaults = {
        maxSize: '6',//文件大小限制
        allowedContentType: 'png|jpg|jpeg|tif|gif|pdf|doc|docs|docx|xls|xlsx|ppt|pptx|txt|rar|zip|apk',//支持的文件格式
        isReadOnly: false,//是否只显示查看按钮
        type: 'add',//默认是新增附件
        timeOut: '10000',//超时时间
        multi: false,//是否可以多次上传附件
        businessType: '000000',//业务类型
        businessId: null,//业务Id
        systemId: null,//系统Id
        modelUrl:'#',
        callback: null //回调函数
    };

    //超时设置
    var isTimeout = false;
    var timeout;

    //初始化上传附件
    $.fn.fileupload = function (options) {
        var defaultAllowedContentTypeArray = defaults.allowedContentType.split('|');
        var optionsAllowedContentTypeArray;
        if (options && options.allowedContentType != null && options.allowedContentType != undefined && options.allowedContentType != '') {
            optionsAllowedContentTypeArray = options.allowedContentType.split('|');
            for (var i = 0; i < optionsAllowedContentTypeArray.length; i++) {
                if ($.inArray(optionsAllowedContentTypeArray[i], defaultAllowedContentTypeArray) < 0) {
                    warning.errorDialog('存在不支持的文件格式');
                    return false;
                }
            }
        }

        var options = $.extend({}, defaults, options);
        var selectName = this.selector.substring(1);
        $(this).bind('click', function () {
            //是否需要登录
            var token = "";
            token = CwHelper.GetToken();
            if (token == "" || CwHelper.IsTokenTimeOut()) {
                CwHelper.Login();
                return;
            }

            popdialog.showModalHtml({
                'html': getFileHtml(options.modelUrl),
                'width': 'normal'
            });

            fileupload.fileChange(options);
            fileupload.fileDelete();
            CwHelper.UserInfo(function (userInfo) {
                var user = { AccountId: userInfo.body.userInfo, AccountName: userInfo.body.AccountName };
                fileupload.fileUpload(options, selectName, user);
            });

        });
    };

    //表单文件改变
    fileupload.fileChange = function (options) {
        $('#fileUploadModal_file').on('change', function () {
            var fileName = $(this).val();
            var sindex = fileName.lastIndexOf('\\') + 1;
            var eindex = fileName.lastIndexOf('.');
            if (options.allowedContentType.indexOf(fileName.substring(eindex + 1).toLowerCase()) < 0) {
                warning.errorDialog('不允许上传' + fileName.substring(eindex + 1).toLowerCase() + '格式的文件!', function () {
                    $('#fileUploadForm')[0].reset();
                });
                return false;
            }
            $('#fileUploadModal_extends').val(fileName.substring(eindex + 1));
            $("#fileUploadModal_fileName").val(fileName.substring(sindex, eindex));
            $('.file-captionkv-fileinput-caption').html('');
            $('.file-captionkv-fileinput-caption').append('<div class="file-caption-name" title="' + fileName.substring(sindex) + '"><span class="glyphicon glyphicon-file kv-caption-icon"></span>' + fileName.substring(sindex) + '</div>')
        });
    };

    //表单删除文件
    fileupload.fileDelete = function () {
        $('#btnDelte').click(function () {
            $('#fileUploadForm')[0].reset();
            $('.file-caption-name').remove();
        });
    };


    $(window).on('message', function (e) {
        if (!isTimeout) {
            var result = eval("(" + e.originalEvent.data + ")");
            if (result.success) {
                fileupload.success(result);
            }
            else {
                Metronic.unblockUI('#ajax-modal .modal-body');
                var tipMsg = '';
                if (typeof result.msg != 'undefined') {
                    tipMsg = result.msg;
                }
                warning.confirm("上传失败，" + tipMsg, function (r) {
                    if (r) {
                        $('.file-caption-name').remove();
                        $('#fileUploadForm')[0].reset();
                    }
                });
            }
        }
        isTimeout = false;
        if (!!timeout) {
            window.clearTimeout(timeout);
        }
    });



    //上传文件
    fileupload.fileUpload = function (options, selectName, user) {
        optionsArray = [];
        optionsArray.push({ elementId: selectName, option: options });
        $('#fileUploadModal_Upload').click(function (e) {
            e.preventDefault();
            if (!$("#fileUploadModal_fileName").val()) {
                warning.alertMsg('请输入文件名!');
                return false;
            }
            if (!$("#fileUploadModal_file").val()) {
                warning.alertMsg('还未选择文件!');
                return false;
            }
            if (fileupload.checkFileSize(options) == false) {
                warning.errorDialog('上传文件不能超过' + options.maxSize + 'M!', function () {
                    $('#fileUploadForm')[0].reset();
                });
                return false;
            }
            $('#fileUploadForm').find('input[name=AllowedContentType]').val(options.allowedContentType);
            $('#fileUploadForm').find('input[name=MaxSize]').val(options.maxSize);
            $('#fileUploadForm').find('input[name=SystemId]').val(system.SysId);
            $('#fileUploadForm').find('input[name=AppId]').val(system.AppId);
            $('#fileUploadForm').find('input[name=AppName]').val(system.AppName);
            $('#fileUploadForm').find('input[name=BusinessType]').val(options.businessType);
            $('#fileUploadForm').find('input[name=BusinessId]').val(options.businessId);
            $('#fileUploadForm').find('input[name=CreatorId]').val(user.AccountId);
            $('#fileUploadForm').find('input[name=CreatorName]').val(user.AccountName);
            $('#fileUploadForm').find('input[name=ElementId]').val(selectName);
            $('#fileUploadForm').find('input[name=DisplayName]').val($("#fileUploadModal_fileName").val());

            Metronic.blockUI({
                target: '#ajax-modal .modal-body',
                textOnly: true,
                boxed: true,
                message: '<div class="imgloading"></div>上传中...'
            });
            isTimeout = false;
            $('#fileUploadForm').submit();
            timeout = setTimeout(function () {
                isTimeout = true;
                Metronic.unblockUI('#ajax-modal .modal-body');
                warning.confirm("上传失败，请重试！", function (r) {
                    if (r) {
                        $('.file-caption-name').remove();
                        $('#fileUploadForm')[0].reset();
                    }
                });
            }, options.timeOut);

        });
    };


    //上传成功以后
    fileupload.success = function (result) {
        var options;
        for (var i = 0; i < optionsArray.length; i++) {
            if (result.data.ElementId == optionsArray[i].elementId) {
                options = optionsArray[i].option;
                var element = { ButtonName: result.data.ElementId, TargetName: result.data.ElementId + 'Id' };
                Metronic.unblockUI('#ajax-modal .modal-body');
                //$('.file-caption-name').remove();
                //$('#fileUploadForm')[0].reset();
                toast.success('上传成功!');
                if (options.type == 'add') {
                    var valNow;
                    var fileId = result.data.FileId;
                    if (!$('#' + element.TargetName).val()) {
                        valNow = fileId;
                    }
                    else {
                        valNow = $('#' + element.TargetName).val() + ',' + fileId;
                    }
                    $('#' + element.TargetName).val(valNow);
                    //fileupload.rebindSingleFileButton(element, fileId, options.isReadOnly, options.multi);
                } else if (options.type == 'list') {
                    $("#sample_1").CustomTable("reload");
                }
                if (typeof options.callback == 'function') {
                    options.callback(result.data);
                }
                if (!options.multi) {
                   // popdialog.closeModal();
                }
            }
        }

    };


    //上传文件成功后，移除“浏览”按钮，替换为“查看”和“删除”按钮
    fileupload.rebindSingleFileButton = function (operatorObj, fileId, isReadOnly, multi) {
        var viewId = fileId + 'View',
            deleteId = fileId + 'Delete',
            targetId = fileId,
            htmlStr = '';
        htmlStr = '<a id="' + viewId + '" href="' + CwHelper.Route('00000080004', '1.0', system.ServerAgent) + '?id=' + targetId + '" target="_blank" class="btn green">查看</a>';
        if (!isReadOnly) {
            htmlStr += '<a id="' + deleteId + '" href="' + targetId + '" fileId="' + targetId + '" class="btn green">删除</a>'
        }
        var uploadFileBtnDom;
        if (!multi) {
            $("#" + operatorObj.ButtonName).parent().append(htmlStr);
            uploadFileBtnDom = $("#" + operatorObj.ButtonName).detach();
        }
        else {
            htmlStr = '<div style="margin-top:5px;">' + htmlStr + '</div>';
            $("#" + operatorObj.ButtonName).parent().append(htmlStr);
        }
        //移除浏览按钮
        if (!isReadOnly) {
            $("#" + deleteId).die();
            //删除文件之后移除“查看”和“删除”按钮，恢复“浏览”按钮
            $("#" + deleteId).live('click', function (e) {
                e.preventDefault();
                var fileId = $(this).attr('fileId');
                CwHelper.Ajax("00000080003", fileId, function (data) {
                    if (!data.body) {
                        warning.errorDialog(data.publicresponse.message);
                    }
                    else {
                        toast.success('删除成功!',
                            {
                                onShown: function () {

                                    if (!multi) {
                                        $("#" + viewId).parent().append(uploadFileBtnDom);
                                        $("#" + viewId).remove();
                                        $("#" + deleteId).remove();
                                        $("#" + operatorObj.TargetName).val('');
                                    }
                                    else {
                                        var fileIdValue = $("#" + operatorObj.TargetName).val();
                                        var filIdArray = fileIdValue.split(',');
                                        var fileIdNowArray = [];
                                        for (var i = 0; i < filIdArray.length; i++) {
                                            if (deleteId.indexOf(filIdArray[i]) >= 0) {
                                            }
                                            else {
                                                fileIdNowArray.push(filIdArray[i]);
                                            }
                                        }

                                        $("#" + operatorObj.TargetName).val(fileIdNowArray.join(','));
                                        $("#" + deleteId).parent().remove();

                                    }
                                    return false; //note: 删除附件返回 html 待解决，之后修改为 return true;
                                }
                            });
                    }
                });
                return false;
            });
        }
    };

    //上传文件成功后，移除“浏览”按钮，替换为“查看”和“删除”按钮
    fileupload.rebindSingleFileButtonForViewAndEdit = function (operatorObj, fileId, isReadOnly, multi) {
        var fileIdArray = fileId.split(',');
        for (var i = 0; i < fileIdArray.length; i++) {
            var viewId = fileIdArray[i] + 'View',
                deleteId = fileIdArray[i] + 'Delete',
                targetId = fileIdArray[i],
                htmlStr = '';
            htmlStr = '<a id="' + viewId + '" href="' + CwHelper.Route('00000080004', '1.0', system.ServerAgent) + '?id=' + targetId + '" target="_blank" class="btn green">查看</a>';
            if (!isReadOnly) {
                htmlStr += '<a id="' + deleteId + '" href="' + targetId + '" fileId="' + targetId + '" class="btn green">删除</a>'
            }
            var buttonName = operatorObj.ButtonName.substring(0, operatorObj.ButtonName.indexOf('Id'));
            var uploadFileBtnDom = '<button type="button" class="btn blue" id="' + buttonName + '"><i class="fa fa-upload"></i>&nbsp;上传</button>';
            if (!multi) {
                $("#" + operatorObj.ButtonName).parent().append(htmlStr);
                //uploadFileBtnDom = $("#" + operatorObj.ButtonName).detach();
            }
            else {
                //htmlStr = '<div style="margin-top:5px;">' + htmlStr + '</div>';
                $("#" + operatorObj.ButtonName).parent().append(htmlStr);
            }
            //移除浏览按钮
            if (!isReadOnly) {
                $("#" + deleteId).die();
                //删除文件之后移除“查看”和“删除”按钮，恢复“浏览”按钮
                $("#" + deleteId).live('click', function (e) {
                    e.preventDefault();
                    var fileId = $(this).attr('fileId');
                    CwHelper.Ajax("00000080003", fileId, function (data) {
                        if (!data.body) {
                            warning.errorDialog(data.publicresponse.message);
                        }
                        else {
                            toast.success('删除成功!',
                                {
                                    onShown: function (e) {

                                        var b = this;
                                        if (!multi) {
                                            $("#" + viewId).parent().append(uploadFileBtnDom);
                                            $("#" + buttonName).fileupload();
                                            $("#" + viewId).remove();
                                            $("#" + deleteId).remove();
                                            $("#" + operatorObj.TargetName).val('');
                                        }
                                        else {
                                            var fileIdValue = $("#" + operatorObj.TargetName).val();
                                            var filIdArray = fileIdValue.split(',');
                                            var fileIdNowArray = [];
                                            for (var i = 0; i < filIdArray.length; i++) {
                                                if (deleteId.indexOf(filIdArray[i]) >= 0) {
                                                }
                                                else {
                                                    fileIdNowArray.push(filIdArray[i]);
                                                }
                                            }

                                            $("#" + operatorObj.TargetName).val(fileIdNowArray.join(','));
                                            $("#" + deleteId).parent().remove();

                                        }
                                        return false; //note: 删除附件返回 html 待解决，之后修改为 return true;
                                    }
                                });
                        }
                    });

                });
            }
        }

    };

    //上传文件成功后，移除“浏览”按钮，替换为“查看”和“删除”按钮
    fileupload.rebindSingleFileButtonForViewAndEditOptions = function (operatorObj, fileId, options, isReadOnly, multi) {
        var fileIdArray = fileId.split(',');
        for (var i = 0; i < fileIdArray.length; i++) {
            var viewId = fileIdArray[i] + 'View',
                deleteId = fileIdArray[i] + 'Delete',
                targetId = fileIdArray[i],
                htmlStr = '';
            htmlStr = '<a id="' + viewId + '" href="' + CwHelper.Route('00000080004', '1.0', system.ServerAgent) + '?id=' + targetId + '" target="_blank" class="btn green">查看</a>';
            if (!isReadOnly) {
                htmlStr += '<a id="' + deleteId + '" href="' + targetId + '" fileId="' + targetId + '" class="btn green">删除</a>'
            }
            var buttonName = operatorObj.ButtonName.substring(0, operatorObj.ButtonName.indexOf('Id'));
            var uploadFileBtnDom = '<button type="button" class="btn blue" id="' + buttonName + '"><i class="fa fa-upload"></i>&nbsp;上传</button>';
            if (!multi) {
                $("#" + operatorObj.ButtonName).parent().append(htmlStr);
                //uploadFileBtnDom = $("#" + operatorObj.ButtonName).detach();
            }
            else {
                //htmlStr = '<div style="margin-top:5px;">' + htmlStr + '</div>';
                $("#" + operatorObj.ButtonName).parent().append(htmlStr);
            }
            //移除浏览按钮
            if (!isReadOnly) {
                $("#" + deleteId).die();
                //删除文件之后移除“查看”和“删除”按钮，恢复“浏览”按钮
                $("#" + deleteId).live('click', function (e) {
                    e.preventDefault();
                    var fileId = $(this).attr('fileId');
                    CwHelper.Ajax("00000080003", fileId, function (data) {
                        if (!data.body) {
                            warning.errorDialog(data.publicresponse.message);
                        }
                        else {
                            toast.success('删除成功!',
                                {
                                    onShown: function (e) {

                                        var b = this;
                                        if (!multi) {
                                            $("#" + viewId).parent().append(uploadFileBtnDom);
                                            $("#" + buttonName).fileupload(options);
                                            $("#" + viewId).remove();
                                            $("#" + deleteId).remove();
                                            $("#" + operatorObj.TargetName).val('');
                                        }
                                        else {
                                            var fileIdValue = $("#" + operatorObj.TargetName).val();
                                            var filIdArray = fileIdValue.split(',');
                                            var fileIdNowArray = [];
                                            for (var i = 0; i < filIdArray.length; i++) {
                                                if (deleteId.indexOf(filIdArray[i]) >= 0) {
                                                }
                                                else {
                                                    fileIdNowArray.push(filIdArray[i]);
                                                }
                                            }

                                            $("#" + operatorObj.TargetName).val(fileIdNowArray.join(','));
                                            $("#" + deleteId).parent().remove();

                                        }
                                        return false; //note: 删除附件返回 html 待解决，之后修改为 return true;
                                    }
                                });
                        }
                    });

                });
            }
        }

    };

    //查看页面附件
    fileupload.rebindFileButtonView = function (operatorObjArr) {
        for (var i = 0; i < operatorObjArr.length; i++) {
            if ($('#' + operatorObjArr[i]).val()) {
                var element = { ButtonName: operatorObjArr[i], TargetName: operatorObjArr[i] };
                $('#' + operatorObjArr[i]).nextAll().remove();
                fileupload.rebindSingleFileButtonForViewAndEdit(element, $('#' + operatorObjArr[i]).val(), true);
            }
        }

    };

    //编辑页面附件
    fileupload.rebindFileButtonEdit = function (operatorObjArr) {
        for (var i = 0; i < operatorObjArr.length; i++) {
            if ($('#' + operatorObjArr[i]).val()) {
                var element = { ButtonName: operatorObjArr[i], TargetName: operatorObjArr[i] };
                $('#' + operatorObjArr[i]).nextAll().remove();
                fileupload.rebindSingleFileButtonForViewAndEdit(element, $('#' + operatorObjArr[i]).val())
            }
        }
    };

    //编辑页面附件
    fileupload.rebindFileButtonEditOptions = function (operatorObjArr, options) {
        for (var i = 0; i < operatorObjArr.length; i++) {
            if ($('#' + operatorObjArr[i]).val()) {
                var element = { ButtonName: operatorObjArr[i], TargetName: operatorObjArr[i] };
                $('#' + operatorObjArr[i]).nextAll().remove();
                fileupload.rebindSingleFileButtonForViewAndEditOptions(element, $('#' + operatorObjArr[i]).val(), options)
            }
        }
    };

    fileupload.resetFileUpLoad = function (operatorObjArr) {
        for (var i = 0; i < operatorObjArr.length; i++) {
            var element = { ButtonName: operatorObjArr[i], TargetName: operatorObjArr[i] };
            $('#' + operatorObjArr[i]).nextAll().remove();
            var id = operatorObjArr[i].substring(0, operatorObjArr[i].lastIndexOf('Id'));
            var html = '<button type="button" class="btn blue" id=' + id + '><i class="fa fa-upload"></i>&nbsp;上传</button>';
            $('#' + operatorObjArr[i]).parent().append(html);
            $('#' + id).fileupload();
        }
    };

    fileupload.checkFileSize = function (options) {
        var obj_file = document.getElementById("fileUploadModal_file");
        if (obj_file.value == "") {
            alert("请先选择上传文件");
        }
        else {
            var filesize = 0;
            if (!!obj_file.files && obj_file.files.length > 0) {
                filesize = obj_file.files[0].size;
                return (filesize > options.maxSize * 1024 * 1024) ? false : true;
            }
            return true;
        }
    };

    var getFileHtml = function (modelUrl) {
        if (typeof (modelUrl) != 'string') {
            modelUrl = '#';
        }
        var html = '';
        html += '<div class="modal-dialog">';
        html += '<div class="modal-content">';
        html += '<div class="modal-header">';
        html += '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>';
        html += '<h4 class="modal-title">导入数据</h4>';
        html += '</div>';
        html += '<div class="modal-body">';
        html += '<form action="' + CwHelper.Route('00000080000', '1.0', system.ServerAgent) + '"  method="post" enctype="multipart/form-data" id="fileUploadForm" target="IframeUpload">';
        html += '<div class="form-group">';
        html += '<label for="fileUploadModal_fileName" class="control-label">文件名:</label>';
        html += '<input type="text" class="form-control" id="fileUploadModal_fileName" name="FileName">';

        html += '<a href="' + modelUrl + '">点此下载导入模板</a>';
    
        html += '</div>';



        
        html += '<div class="form-group">';
        html += '<label for="fileUploadModal_file" class="control-label">文件:</label>';
        html += '<div class="file-input file-input-new">';
        html += '<div class="input-group ">';
        html += '<div tabindex="500" class="form-control file-captionkv-fileinput-caption">';
        html += '';
        html += '</div>';
        html += '<div class="input-group-btn">';
        html += '<button id="btnDelte" type="button" tabindex="500" title="Clear selected files" class="btn btn-default fileinput-remove fileinput-remove-button"><i class="glyphicon glyphicon-trash"></i> <span class="hidden-xs">&nbsp;删除</span></button>';
        html += '<div tabindex="500" class="btn btn-primary btn-file"><i class="glyphicon glyphicon-folder-open"></i> <span class="hidden-xs">&nbsp;浏览</span><input type="file" id="fileUploadModal_file" name="File" style="height:29px;"></div>';
        html += '</div>';
        html += '</div>';
        html += '</div>';
        html += '</div>';

        // html += '<div class="form-group">';
        // html += '<label for="fileUploadModal_remark" class="control-label">备注:</label>';
        // html += '<textarea class="form-control" id="fileUploadModal_remark" rows="3" name="Remark"></textarea>';
        // html += '</div>';
        
        html += '<input type="hidden" name="AllowedContentType" value="">';
        html += '<input type="hidden" name="MaxSize" value="">';
        html += '<input type="hidden" name="SystemId" value="">';
        html += '<input type="hidden" name="AppId" value="">';
        html += '<input type="hidden" name="AppName" value="">';
        html += '<input type="hidden" name="BusinessType" value="">';
        html += '<input type="hidden" name="BusinessId" value="">';
        html += '<input type="hidden" name="CreatorId" value="">';
        html += '<input type="hidden" name="CreatorName" value="">';
        html += '<input type="hidden" name="DisplayName" value="">';
        html += '<input type="hidden" name="ElementId" value="">';
        html += '<input type="hidden" name="Iframe" value="true">';
        html += '</form>';
        html += '<iframe name="IframeUpload" style="display:none"></iframe>';
        html += '<div class="modal-footer">';
        html += '<a href="javascript:void(0)" class="btn blue" id="fileUploadModal_Upload"  type="submit"><i class="fa fa-upload"></i>&nbsp;上传</a>';
        html += '<a href="javascript:void(0)" disabled class="btn c-cyan" id="import-btn"  ><i class="fa fa-plus"></i>&nbsp;导入</a>';
        html += '</div>';
        html += '</div>';
        html += '</div>';

        return html;
    };

    return fileupload;

});