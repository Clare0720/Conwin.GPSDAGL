define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, Metronic) {
            var initPage = function () {
                var tabFlag = false;
                common.AutoFormScrollHeight('#Form1');
                formcontrol.initial();
                initData();
                //保存
                $('#saveBtn').on('click', function (e) {
                    e.preventDefault();
                    var flags = formcontrol.buttonValid();
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
            };
            //初始化表单数据
            function initData() {
                $('#Id').val(helper.NewGuid());
            };
            //保存
            function save() {
                
                var $excelFile = $('#excelFile');
                var file = $excelFile.prop('files')[0];
                var reader = new FileReader();
                //reader.readAsText(file[0], "UTF-8");
               
                reader.readAsDataURL(file);
                //var _this = this;
                //var file = e.target.files[0];
                //var reader = new FileReader();
                //reader.readAsDataURL(file);
                reader.onloadend = function () {
                    var base64 = reader.result;

                    helper.Ajax("001700000006", {
                        "data": { "dataUrl": base64, "fileType": file.type } }, function (response) {
                            if (response.body) {
                                toast.success("成功");
                                console.log(response);
                                //var json = response.data.result;
                                var json = response.body["result"];
                                $('#msg').html("");
                                //alert(json);
                                $('#msg').append("成功：" + json["SuccessCount"]+"条记录");
                                for (var i = 0; i < json["ErrorLists"].length; i++) {

                                    //alert("错误" + json["ErrorLists"][i]["No"]);
                                    $('#msg').append("<p>No:" + json["ErrorLists"][i]["No"]+"增加失败</p>");
                                }
                            //for (var i = 0; i < json.length; i++) {
                            //    if (json[i].Name != "F2") {
                            //        _this.entitys.push(json[i]);
                            //    }
                            //    //setTimeout(function () { window.location.reload(false); }, 800);
                            //}
                        } else {
                            console.log(response);
                            tipdialog.alertMsg("失败");
                        }
                    }, false);
                    
                }
            };
            initPage();

        });
});
