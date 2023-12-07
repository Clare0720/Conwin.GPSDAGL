define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'formcontrol', 'prevNextpage', 'tableheadfix', 'system', 'selectcity', 'filelist', 'fileupload', 'metronic', 'customtable', 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN','searchbox'],
        function ($, popdialog, tipdialog, toast, helper, common, formcontrol, prevNextpage, tableheadfix, system, selectcity, filelist, fileupload, searchbox) {
            var UserInfo = helper.GetUserInfo();
            var initPage = function () {
    
                //初始化table
                initlizableTable1();
                initlizableTable2();


                //页面关闭
                $('#btnclose').click(function (e) {
                    popdialog.closeIframe();
                });

                //表一查询
                $('#btnSearch1').click(function (e) {
                    e.preventDefault();

                    if ($("#JianKongYuan").val().trim() == "") {
                        tipdialog.errorDialog('请先选择监控员!');
                        return;
                    };
                    if ($("#ChePaiHao").val().trim()!=""&&$("#ChePaiHao").val().trim().length<3) {
                        tipdialog.errorDialog('请输入合法的车牌号码（至少三位）');
                        return;
                    }
                    var flag = false;
                    if ($("#QiYeMingCheng").val().trim() != "" && $("#QiYeMingCheng").val().trim() != "QingXuanZeBiaoZhiId") {
                        flag = true;
                    }
                    if ($("#CheDuiMingCheng").val().trim() != "" && $("#CheDuiMingCheng").val().trim() != "QingXuanZeBiaoZhiId") {
                        flag = true;
                    };
                    //if ($("input[name='QiYeMingCheng']").val().trim() != "") {
                    //    flag = true;
                    //}
                    //if ($("input[name='CheDuiMingCheng']").val().trim() != "") {
                    //    flag = true;
                    //};

                    //   if (flag) {
                    $("#tb_table1").CustomTable("reload");
                    //  } else {
                    //     tipdialog.errorDialog('请先选择车队或企业!');
                    // }

                });
                //表一重置
                $("#btnReset1").click(function (e) {
                    e.preventDefault();
                    $('#Form1').find('input[type=text]:not(:disabled), select:not(:disabled)').val('');
                });


                //表二查询
                $('#btnSearch2').click(function (e) {
                    e.preventDefault();

                    if ($("#JianKongYuan").val().trim() == "") {
                        tipdialog.errorDialog('请先选择监控员!');
                        return;
                    };
                    if ($("#Code_Own").val().trim() != "" && $("#Code_Own").val().trim().length < 3) {
                        tipdialog.errorDialog('请输入合法的车牌号码（至少三位）');
                        return;
                    }
                    var flag = false;
                    if ($("#QiYeMingCheng").val().trim() != "" && $("#QiYeMingCheng").val().trim() != "QingXuanZeBiaoZhiId") {
                        flag = true;
                    }
                    if ($("#CheDuiMingCheng").val().trim() != "" && $("#CheDuiMingCheng").val().trim() != "QingXuanZeBiaoZhiId") {
                        flag = true;
                    };
                    // if ($("input[name='QiYeMingCheng']").val().trim() != "") {
                    //    flag = true;
                    //}
                    //if ($("input[name='CheDuiMingCheng']").val().trim() != "") {
                    //    flag = true;
                    //};
                    // if (flag) {
                    $("#tb_table2").CustomTable("reload");
                    // } else {
                    //  tipdialog.errorDialog('请先选择车队或企业!');
                    // }
                });
                //表二重置
                $("#btnReset2").click(function (e) {
                    e.preventDefault();
                    $('#Form2').find('input[type=text]:not(:disabled), select:not(:disabled)').val('');
                });


                //添加监控
                $("#AddJK").click(function (e) {
                    var rows = $("#tb_table1").CustomTable('getSelection');

                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要添加监控的行!');
                        return false;
                    }

                    //var ChuangJianDanWeiOrgCode;

                    //if ($("#QiYeMingCheng").val().trim() != "" && $("#QiYeMingCheng").val().trim() != "QingXuanZeBiaoZhiId") {
                    //    ChuangJianDanWeiOrgCode = $("#QiYeMingCheng").val().trim();
                    //}
                    //if ($("#CheDuiMingCheng").val().trim() != "" && $("#CheDuiMingCheng").val().trim() != "QingXuanZeBiaoZhiId") {
                    //    ChuangJianDanWeiOrgCode = $("#CheDuiMingCheng").val().trim();
                    //};
                    //   if ($("input[name='QiYeMingCheng']").val().trim() != "") {
                    //    ChuangJianDanWeiOrgCode = $("input[name='QiYeMingCheng']").val().trim();
                    // }
                    //if ($("input[name='CheDuiMingCheng']").val().trim() != "") {
                    //    ChuangJianDanWeiOrgCode =$("input[name='CheDuiMingCheng']").val().trim();
                    //};
                    var data = [];
                    $.each(rows, function (idenx, item) {
                        data.push({
                            //ChuangJianDanWeiOrgCode: ChuangJianDanWeiOrgCode,
                            SysUserID: $("#JianKongYuan").val(),
                            CheLiangID: item.data.Id
                        });

                       

                    });
                    GetData("003300300549", data, function (result) {
                        if (result) {
                             
                            $("#tb_table1").CustomTable("reload");
                            $("#tb_table2").CustomTable("reload");
                        }
                    });
                });
 


                //移除监控
                $("#remoteJK").click(function (e) {
                    var rows = $("#tb_table2").CustomTable('getSelection');

                    if (rows == undefined) {
                        tipdialog.errorDialog('请选择需要移除监控的行!');
                        return false;
                    }

                    //var ChuangJianDanWeiOrgCode;

                    //if ($("#QiYeMingCheng").val().trim() != "" && $("#QiYeMingCheng").val().trim() != "QingXuanZeBiaoZhiId") {
                    //    ChuangJianDanWeiOrgCode = $("#QiYeMingCheng").val().trim();
                    //}
                    //if ($("#CheDuiMingCheng").val().trim() != "" && $("#CheDuiMingCheng").val().trim() != "QingXuanZeBiaoZhiId") {
                    //    ChuangJianDanWeiOrgCode = $("#CheDuiMingCheng").val().trim();
                    //};
                    // if ($("input[name='QiYeMingCheng']").val().trim() != "") {
                    //    ChuangJianDanWeiOrgCode = $("input[name='QiYeMingCheng']").val().trim();
                    // }
                    //if ($("input[name='CheDuiMingCheng']").val().trim() != "") {
                    //    ChuangJianDanWeiOrgCode =$("input[name='CheDuiMingCheng']").val().trim();
                    //};
                    var data = [];
                    $.each(rows, function (idenx, item) {
                        data.push({
                            //ChuangJianDanWeiOrgCode: ChuangJianDanWeiOrgCode,
                            SysUserID: $("#JianKongYuan").val(),
                            CheLiangID: item.data.Id
                        });
                      
                       

                    });
                    GetData("003300300550", data, function (result) {
                        if (result) {
                           
                            $("#tb_table1").CustomTable("reload");
                            $("#tb_table2").CustomTable("reload");
                        }
                    });
                });



                ////企业下拉框改变事件
                //$("#QiYeMingCheng").on("change", function () {
                //    GetRenYuan();
                //})
                ////车队下拉框改变事件
                //$("#CheDuiMingCheng").on("change", function () {
                //    GetRenYuan();
                //})
                
                helper.Ajax("003300300546", {}, function (result) {
                    if ($.type(result) == "string") {
                        result = helper.StrToJson(result);
                    }
                    var publicresponse = result.publicresponse;
                    var body = result.body;
                    if (publicresponse.statuscode == 0) {
                        var reResult = [];
                        reResult.push({ id: "QingXuanZeBiaoZhiId", text: "请选择" });
                        for (var i = 0; i < result.body.length; i++) {
                            reResult.push({ id: body[i].OrgCode, text: body[i].OrgName });
                        }
                        $("#QiYeMingCheng").searchbox({
                            data: reResult  
                        });
                    };
                }, false);

                helper.Ajax("003300300547", {}, function (result) {
                    if ($.type(result) == "string") {
                        result = helper.StrToJson(result);
                    }
                    var publicresponse = result.publicresponse;
                    var body = result.body;
                    if (publicresponse.statuscode == 0) {
                        var reResult = [];
                        reResult.push({ id: "QingXuanZeBiaoZhiId", text: "请选择" });
                        for (var i = 0; i < result.body.length; i++) {
                            reResult.push({ id: body[i].OrgCode, text: body[i].OrgName });
                        }
                        $("#CheDuiMingCheng").searchbox({
                            data: reResult
                        });
                    };
                }, false);
            } //end InitPage

            //表一 初始化
            function initlizableTable1() {
                $("#tb_table1").CustomTable({
                    ajax: helper.AjaxData("003300300548", 
                        function (data) {
                            var pageInfo = {
                                Page: parseInt(data.start / data.length + 1),
                                Rows: data.length
                            };
                            for (var i in data) {
                                delete data[i];
                            }
                            var para = $('Form1').serializeObject();
                            $('#Form1 .KFPSearch').each(function (i, item) {
                                para[$(item).attr('name')] = $(item).val();
                            });
                            var OrgCode = "";
                            if ($("#QiYeMingCheng").val() != "QingXuanZeBiaoZhiId") {
                                OrgCode = $("#QiYeMingCheng").val();
                            } else {
                                if ($("#CheDuiMingCheng").val() != "QingXuanZeBiaoZhiId") {
                                    OrgCode = $("#CheDuiMingCheng").val();
                                }
                            }

                            para["OrgCode"] = OrgCode;
                              
                            para["SysUserID"] = $("#JianKongYuan").val();
                           // para["CheDuiOrgCode"] = $("#CheDuiMingCheng").val() != "QingXuanZeBiaoZhiId" ? $("#CheDuiMingCheng").val() : "";
                           
                            para["IsFPJK"] = false;
                            pageInfo.data = para;
                            $.extend(data, pageInfo);
                        }, null),
                    single: false,
                    filter: false,
                    ordering: true, /////是否支持排序
                    //"dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-3 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-6 col-sm-12 pagnav pagination-p"p>>',
                    columns: [{
                        render: function (data, type, row) {
                            return '<input type=checkbox class=checkboxes />';
                        }
                    },
                    {
                        data: 'GeRenCheZhuMingCheng', render: function (data, type, row) {
                            if (row.QiYeMingCheng != "" && row.QiYeMingCheng != "QingXuanZeBiaoZhiId") {
                                return row.QiYeMingCheng; QiYeMingCheng
                            } else {
                                return row.GeRenCheZhuMingCheng;
                            }
                        }
                    },
                    {
                        data: 'CheLiangZhongLei',
                        render: function (data, type, row) {
                            switch (data) {
                                case 1:
                                    return "客运班车";
                                    break;
                                case 2:
                                    return "旅游包车";
                                    break;
                                case 3:
                                    return "危险货运";
                                    break;
                                case 4:
                                    return "重型货车";
                                    break;
                                case 5:
                                    return "公交客运";
                                    break;
                                case 6:
                                    return "出租客运";
                                    break;
                                case 7:
                                    return "教练员车";
                                    break;
                                case 8:
                                    return "普通货运";
                                    break;
                                case 9:
                                    return "其它车辆";
                                    break;
                                default:
                                    return "";
                            }
                        }
                    },
                    {
                        data: 'ChePaiHao'
                    },
                    {
                        data: 'ChePaiYanSe'
                    }],
                    pageLength: 10,
                    //"fnDrawCallback": function (oSettings) {
                    //   // tableheadfix.ResetFix();
                      
                    //}
                });
                //tableheadfix.InitFix(system.OnlyTableFix);
            };
            //表二 初始化
            function initlizableTable2() {
                $("#tb_table2").CustomTable({
                    ajax: helper.AjaxData("003300300548", 
                        function (data) {
                            var pageInfo = {
                                Page: parseInt(data.start / data.length + 1),
                                Rows: data.length
                            };
                            for (var i in data) {
                                delete data[i];
                            }
                            var para = $('#Form2').serializeObject();
                            $('#Form2').find('input').each(function (i, item) {
                                para[$(item).attr('name')] = $(item).val();
                            });
                            var OrgCode = "";
                            if ($("#QiYeMingCheng").val() != "QingXuanZeBiaoZhiId") {
                                OrgCode = $("#QiYeMingCheng").val();
                            } else {
                                if ($("#CheDuiMingCheng").val() != "QingXuanZeBiaoZhiId") {
                                    OrgCode = $("#CheDuiMingCheng").val();
                                }
                            }

                            para["OrgCode"] = OrgCode;

                            //para["OrgId"] = $("#QiYeMingCheng").val() != "QingXuanZeBiaoZhiId" ? $("#QiYeMingCheng").val() : "";
                            para["SysUserID"] = $("#JianKongYuan").val();
                           // para["CheDuiOrgCode"] = $("#CheDuiMingCheng").val() != "QingXuanZeBiaoZhiId" ? $("CheDuiMingCheng").val() : "";                         
                            para["IsFPJK"] = true;
                            pageInfo.data = para;
                            $.extend(data, pageInfo);
                        }, null),
                    single: false,
                    filter: false,
                    ordering: true, /////是否支持排序
                    //"dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                    "dom": 'fr<"table-scrollable"t><"row"<"col-md-3 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-6 col-sm-12 pagnav pagination-p"p>>',
                    columns: [{
                        render: function (data, type, row) {
                            return '<input type=checkbox class=checkboxes />';
                        }
                    },
                    {
                        data: 'GeRenCheZhuMingCheng', render: function (data, type, row) {
                            if (row.QiYeMingCheng != "" && row.QiYeMingCheng != "QingXuanZeBiaoZhiId") {
                                return row.QiYeMingCheng; QiYeMingCheng
                            } else {
                                return row.GeRenCheZhuMingCheng;
                            }
                        }
                    },
                    {
                        data: 'CheLiangZhongLei',
                        render: function (data, type, row) {
                            switch (data) {
                                case 1:
                                    return "客运班车";
                                    break;
                                case 2:
                                    return "旅游包车";
                                    break;
                                case 3:
                                    return "危险货运";
                                    break;
                                case 4:
                                    return "重型货车";
                                    break;
                                case 5:
                                    return "公交客运";
                                    break;
                                case 6:
                                    return "出租客运";
                                    break;
                                case 7:
                                    return "教练员车";
                                    break;
                                case 8:
                                    return "普通货运";
                                    break;
                                case 9:
                                    return "其它车辆";
                                    break;
                                default:
                                    return "";
                            }
                        }
                    },
                    {
                        data: 'ChePaiHao'
                    },
                    {
                        data: 'ChePaiYanSe'
                    }],
                    pageLength: 10
                    //"fnDrawCallback": function (oSettings) {
                    //    tableheadfix.ResetFix();
                    //}
                });
                //tableheadfix.InitFix(system.OnlyTableFix);
            };


            //设置企业下拉框
            //function GetYeHu() {
            //    GetData("003300300546", {}, function (result) {
            //        var optionStr = '<option value="">请选择</option>';
            //        $(result).each(function (i, item) {
            //            optionStr += '<option value="' + item["OrgCode"] + '">' + item["OrgName"] + '</option>';
            //        });
            //        $("#QiYeMingCheng").empty().append(optionStr);
            //    });
            //}
              
                 
                 
            //设置车队下拉框
            //function GetCheDui() {
            //    GetData("003300300547", {}, function (result) {
            //        var optionStr = '<option value="">请选择</option>';
            //        $(result).each(function (i, item) {
            //            optionStr += '<option value="' + item["OrgCode"] + '">' + item["OrgName"] + '</option>';
            //        });
            //        $("#CheDuiMingCheng").empty().append(optionStr);
            //    });
            //}

            //根据人员组织Id获取人员信息写入下拉框
            function GetRenYuan() {

                var SysOrgCode = UserInfo.OrganizationCode;
                //if ($("#QiYeMingCheng").val().trim() != "") {
                //    SysOrgCode = $("#QiYeMingCheng").val();
                //}
                //else if ($("#CheDuiMingCheng").val().trim() != "") {
                //    SysOrgCode = $("#CheDuiMingCheng").val();
                //}
                //else {
                //    return;
                //}
                var Data = {
                    "SysId": "60190FC4-5103-4C76-94E4-12A54B62C92A",
                    "SysRoleId": "354d4da6-133f-4ca7-909a-c33311d75b7d",
                    "SysOrgCode": SysOrgCode
                };

                GetData("00000030036", Data, function (result) {
                    var optionStr = '<option value="">请选择</option>';
                    $(result).each(function (i, item) {
                        optionStr += '<option value="' + item["Id"] + '">' + item["UserName"] + '</option>';
                    });
                    $("#JianKongYuan").empty().append(optionStr);
                });
            }


            //根据服务编号获取信息
            function GetData(ServiceCode, data, callback) {
                helper.Ajax(ServiceCode, data, function (resultdata) {
                    if (typeof callback == 'function') {
                        if (typeof (resultdata) == "string") {
                            resultdata = JSON.parse(resultdata);
                        }
                        if (resultdata.publicresponse.statuscode == 0) {
                            callback(resultdata.body);
                        } else {
                            tipdialog.errorDialog('获取数据失败!' + resultdata.publicresponse.message);
                        }

                    }
                }, false);
            }







            initPage();
           // GetYeHu();
          //  GetCheDui();
            GetRenYuan();
        });
});