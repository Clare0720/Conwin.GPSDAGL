define(['/Modules/Config/conwin.main.js'], function () {
    require.config({paths: {"actionhub": "/Modules/GPSDAGL/Common/actionhub"}});//引入工具类
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'tableheadfix', 'system', 'filelist', 'metronic', 'customtable','actionhub'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, tableheadfix, system, filelist, Metronic, customtable,actionhub) {
            var initPage = function () {
                common.AutoFormScrollHeight('#Form1');

                formcontrol.initial();
                initData();


                //保存
                $('#saveBtn').on('click', function (e) {
                    e.preventDefault();
                    var flags = true;
                    var msg = '';
                    var fromData = $('#Form1').serializeObject();
                    //fromData.YouXiaoZhuangTai = $('#YouXiaoZhuangTai').val();
                    //调用新增接口
                    if ($.trim(fromData.YeHuMingCheng) == '') {
                        msg += "业户名称 是必填项<br/>";
                    }
                    if ($.trim(fromData.YeHuDaiMa) == '') {
                        msg += "业户代码 是必填项<br/>";
                    }
                    if ($.trim(fromData.TongYiSheHuiXinYongDaiMa) == '') {
                        msg += "统一社会信用代码 是必选项<br/>";
                    }
                    if ($.trim(fromData.JingYingXuKeZhengZi) == '') {
                        msg += "经营许可证字 是必选项<br/>";
                    }
                    if ($.trim(fromData.JingYingXuKeZhengHao) == '') {
                        msg += "经营许可证号 是必选项<br/>";
                    }
                    if ($.trim(fromData.YouXiaoZhuangTai) == '') {
                        msg += "有效状态 是必填项<br/>";
                    }
                    //if ($.trim(fromData.YingYeZhiZhaoFuBenId) == '') {
                    //    msg += "营业执照副本 必须上传<br/>";
                    //}
                    if (msg != '') {
                        flags = false;
                        tipdialog.alertMsg(msg);
                    }
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
                //tab2
                $('#tab2').click(function (e) {
                    $('#LianXiXinXi').addClass('active in');
                    $('#JiChuXinXi').removeClass('active in');
                });

                // 联系人表刷新按钮
                $('#btnFlash').click(function (e) {
                    $("#tb_lianXiRenTable").CustomTable("reload");
                });

                ////初始化附件上传按钮
                //$('.fa-upload').each(function (index, item) {
                //    $('#' + $(item).parent()[0].id).fileupload({
                //        multi: false,
                //        timeOut: 20000,
                //        allowedContentType: 'png|jpg|jpeg'
                //    });
                //});

                //======================联系人====================

                // 添加联系人
                $('#btnCreate').on('click', function (e) {
                    e.preventDefault();
                    popdialog.showModal({
                        'url': '../LianXiRenXinXi/LianXiRenAdd.html',
                        'width': '900px',
                        'showSuccess': initAddLianXiRen
                    });
                });

                // 修改联系人
                $('#btnEdit').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_lianXiRenTable").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要修改的记录');
                        return false;
                    }
                    if (rows.length > 1) {
                        tipdialog.errorDialog('每次只能修改一条记录');
                        return false;
                    }
                    $(rows).each(function (i, item) {
                        ids.push(item.data.Id);
                    });
                    $('#SelectData').val(ids.join(','));
                    popdialog.showModal({
                        'url': '../LianXiRenXinXi/LianXiRenEdit.html',
                        'width': '900px',
                        'showSuccess': initEditLianXiRen
                    });
                });

                // 删除联系人
                $('#btnDel').on('click', function (e) {
                    e.preventDefault();
                    var rows = $("#tb_lianXiRenTable").CustomTable('getSelection'), ids = [];
                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要操作的行');
                        return false;
                    }
                    $(rows).each(function (i, item) {
                        if (item.data.LeiBie == "2") {
                            isDeleteFaRen = true;
                        }
                        ids.push(item.data.Id);
                    });
                    tipdialog.confirm("确定要删除选中的记录？", function (r) {
                        if (r) {
                            helper.Ajax("003300300012", ids, function (data) {
                                if (data.body) {
                                    toast.success("删除成功");
                                    $("#tb_lianXiRenTable").CustomTable("reload");
                                }
                                else {
                                    tipdialog.alertMsg(data.publicresponse.message);
                                }
                            }, false);
                        }
                    });
                });
                //=====================================================




            };


            //新增联系人
            function initAddLianXiRen() {
                formcontrol.initial();
                $('#AddZiBiaoSure').on('click', function (e) {
                    e.preventDefault();
                    var jsonData1 = $('#SelectForm').serializeObject();
                    jsonData1.BenDanWeiOrgCode = $('#BenDanWeiOrgCode').val();
                    if (!CheckSubmit(jsonData1)) {
                        return;
                    }
                    //调用联系人接口
                    //helper.Ajax("true", jsonData1, function (data) {
                    helper.Ajax("003300300010", jsonData1, function (data) {
                        if (data.publicresponse.statuscode == 0) {
                            if (data.body) {
                                toast.success("新增成功");
                                popdialog.closeModal();
                                $("#tb_lianXiRenTable").CustomTable("reload");
                            }
                            else {
                                tipdialog.alertMsg(data.publicresponse.message);
                            }
                        } else {
                            tipdialog.errorDialog("请求失败");
                        }
                    }, false);

                });
            };

            //修改联系人
            function initEditLianXiRen() {
                formcontrol.initial();
                var id = $('#SelectData').val();
                getLianXiRenXinXi(id);

                $('#EditZiBiaoSure').on('click', function (e) {
                    e.preventDefault();
                    var jsonData1 = $('#SelectForm').serializeObject();
                    jsonData1.Id = $('#SelectData').val()
                    jsonData1.BenDanWeiOrgCode = $('#BenDanWeiOrgCode').val();
                    if (!CheckSubmit(jsonData1)) {
                        return;
                    }
                    //调用更新联系人接口
                    //helper.Ajax("true", jsonData1, function (data) {
                    helper.Ajax("003300300011", jsonData1, function (data) {
                        if (data.publicresponse.statuscode == 0) {
                            if (data.body) {
                                toast.success('修改成功');
                                popdialog.closeModal();
                                $("#tb_lianXiRenTable").CustomTable("reload");
                            }
                            else {
                                tipdialog.alertMsg(data.publicresponse.message);
                            }
                        }else {
                            tipdialog.errorDialog("请求失败");
                        }
                    }, false);
                });
            }

            //获取联系人
            function getLianXiRenXinXi(id) {
                helper.Ajax("003300300013", id, function (resultdata) {
                    if (resultdata.publicresponse.statuscode == 0) {
                        var resource = resultdata.body;
                        $('#QueryAddZiBiao').find('.form-control-static').each(function (i, item) {
                            var index = $(item).attr('for');
                            var tempValue = resource[index];
                            if (tempValue != undefined) {
                                $(item).html(tempValue == '' ? '' : tempValue);
                            } else {
                                $(item).html('');
                            }
                        });
                        $('#QueryAddZiBiao').find('input[name],select[name],textarea[name]').each(function (i, item) {
                            var tempValue = resource[$(item).attr('name')];
                            if (tempValue != undefined) {
                                $(item).val(tempValue.toString() == '' ? '' : tempValue);
                            } else {
                                $(item).val('');
                            }
                        });
                        $('#preLeiBie').val($('#LeiBie').val());
                    } else {
                        tipdialog.errorDialog("请求数据失败");
                    }
                }, false);
            }





            // 联系人增确认
            function CheckSubmit(jsonData1) {
                var msg = '';
                if (jsonData1.LianXiRen == '') {
                    msg += '联系人不能为空<br/>';
                }
                if (jsonData1.ShenFenZheng == '') {
                    msg += '身份证不能为空<br/>';
                }
                if (actionhub.CheckIDCard(jsonData1.ShenFenZheng) == false) {
                    msg += '身份证号码格式不正确';
                }
                if (msg != '') {
                    tipdialog.alertMsg(msg);
                    return false;
                } else {
                    return true
                }
            };



            //初始化表单数据
            function initData() {

                var id = window.parent.document.getElementById('hdIDS').value;

                helper.Ajax("003300300319", id, function (data) {
                    if ($.type(data) == "string") {
                        data = helper.StrToJson(data);
                    }
                    if (data.publicresponse.statuscode == 0) {
                        var body = data.body;
                        $("#Form1").find("input[name],select[name],textarea[name]").each(function (i, item) {
                            var tempvalues = body[$(item).attr('name')];
                            if (tempvalues !== undefined) {
                                $(item).val(tempvalues.toString() == "" ? "" : tempvalues);
                            } else {
                                $(item).val('');
                            };
                        });

                        initlizableLianXiRenTable();
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);


            };
            //保存
            function save() {
                //TODO: 校验数据
                var jsonData1 = $('#Form1').serializeObject();
                //jsonData1.QiYeLeiXing = $('#QiYeLeiXing').val();
                //jsonData1.YouXiaoZhuangTai = $('#YouXiaoZhuangTai').val();
                for (var key in jsonData1) {
                    jsonData1[key] = jsonData1[key].replace(/\s/g, "");
                }


                //调用更新接口
                //TODO
                helper.Ajax("003300300320", jsonData1, function (data) {
                    if ($.type(data) == "string") {
                        data = helper.StrToJson(data);
                    }
                    if (data.publicresponse.statuscode == 0) {
                        if (data.body) {
                            toast.success("档案保存成功");
                        }
                        else {
                            tipdialog.alertMsg("档案保存失败");
                        }
                    }
                    else {
                        tipdialog.alertMsg(data.publicresponse.message);
                    }
                }, false);
            };


            //初始化联系人表格
            function initlizableLianXiRenTable() {
                $("#tb_lianXiRenTable").CustomTable({
                    ajax: helper.AjaxData("003300300009",
                        function (data) {
                            var pageInfo = { Page: data.start / data.length + 1, Rows: data.length };
                            for (var i in data) {
                                delete data[i];
                            }
                            var para = new Object();
                            para["BenDanWeiOrgCode"] = $("#BenDanWeiOrgCode").val(); 
                            pageInfo.data = para;
                            $.extend(data, pageInfo);
                        }, null),
                    single: false,
                    //filter: true,
                    ordering: true, /////是否支持排序
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    columns: [
                        {
                            render: function (data, type, row) {
                                return '<input type=checkbox class=checkboxes />';
                            }
                        },
                        { data: 'LianXiRen' },
                        {
                            data: 'LeiBie',
                            render: function (data, type, row, meta) {
                                var value = '';
                                switch (data) {
                                    case 1:
                                        value = "企业法人";
                                        break;
                                    case 2:
                                        value = "本地负责人";
                                        break;
                                    case 3:
                                        value = "其他";
                                        break;
                                    default:
                                }
                                return value;
                            }
                        },
                        { data: 'ShenFenZheng' },
                        { data: 'ShouJiHaoMa' },
                        { data: 'YouXiang' }
                    ],
                    pageLength: 10
                });

                $("#tb_lianXiRenTable").CustomTable("reload");
            };


            initPage();
        });
});
