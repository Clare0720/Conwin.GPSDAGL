/** * Created by zengtao on 18/7/25. */define(['/Modules/Config/conwin.main.js'], function () {    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'gpsjcfxcommon', 'customtable', 'bootstrap-datepicker', "bootstrap-datetimepicker", 'bootstrap-datepicker.zh-CN', 'bootstrap-datetimepicker.zh-CN'],        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system, gpsjcfxcommon) {            var GPSjcfxList = {};            var dateArr = new Array();            appendCss(pathStaticResource + 'Framework/Assets/global/plugins/bootstrap-datetimepicker/css/bootstrap-datetimepicker.min.css');            /**             * 初始化方法             * **/            GPSjcfxList.initPage = function () {                //初始化页面 begin                //todo:（初始化页面）                InitTable();                InitDateTimePicker();                timeFn($("#DateStart").val(), $("#DateEnd").val());                //初始化页面 end                //初始化时间控件                function InitDateTimePicker() {                    // var currentMonth = getCurrentDateTime();                    var beginDateTime = getCurrentDate();                    var lastTimeDateStart = '';                    var lastTimeDateEnd = '';                    $("#DateStart").val(beginDateTime);                    $("#DateEnd").val(beginDateTime);                    $('#DateStart').datepicker({                        language: "zh-CN",                        format: "yyyy-mm-dd",                        autoclose: true,                        endDate: new Date(),                        startDate: new Date(new Date($("#DateEnd").val()).getTime() - (180 * 24 * 60 * 60 * 1000))                    });                    $("#DateEnd").datepicker({                        language: "zh-CN",                        autoclose: true,                        format: "yyyy-mm-dd",                        endDate: new Date(),                    });                    $('#DateStart').change(function () {                        var datePickerValue = $('#DateStart').val();                        var date = new Date(datePickerValue);                        if (getLastDateTime(date)) {                            //大于当天时间                            $(this).css("color", "red");                        } else {                            $(this).css("color", "#333");                        }                        $('#DateEnd').datepicker('setStartDate', new Date($("#DateStart").val()));                        timeFn($("#DateStart").val(), $("#DateEnd").val());                    });                    $('#DateEnd').change(function () {                        var datePickerValue = $('#DateEnd').val();                        var date = new Date(datePickerValue);                        var currentDate = new Date();                        if (getLastDateTime(date)) {                            //大于当天时间                            $(this).css("color", "red");                        } else {                            $(this).css("color", "#333");                        }                        $('#DateStart').datepicker('setStartDate', new Date(new Date($("#DateEnd").val()).getTime() - (180 * 24 * 60 * 60 * 1000)));                        $('#DateStart').datepicker('setEndDate', new Date($("#DateEnd").val()));                        var start = $('#DateStart').val();                        var end = $('#DateEnd').val();                        var stime = new Date(start.replace(/-/g, "/"));                        var etime = new Date(end.replace(/-/g, "/"));                        var dateDiff = stime.getTime() - etime.getTime();//时间差的毫秒数                        if (dateDiff > 0) {                            $('#DateStart').val($('#DateEnd').val())                        }                        timeFn($("#DateStart").val(), $("#DateEnd").val());                    });                }                function InitTime() {                    var beginDateTime = getCurrentDate();
                    $('#DateStart').datepicker('setDate', beginDateTime);
                    $('#DateEnd').datepicker('setDate', beginDateTime);
                }                //计算时间差                function timeFn(start, end) {                    dateArr = new Array();                    var stime = new Date(start.replace(/-/g, "/"));                    var etime = new Date(end.replace(/-/g, "/"));                    var dateDiff = etime.getTime() - stime.getTime();//时间差的毫秒数                    var dayDiff = Math.floor(dateDiff / (24 * 3600 * 1000));                    if (dayDiff > 0) {                        for (var i = 0; i <= dayDiff; i++) {                            var date = new Date(stime.getTime() + (i * 24 * 60 * 60 * 1000));                            var date1 = getDate(date);                            dateArr[i] = date1;                        }                    } else {                        dateArr[0] = start;                    }                }                function getLastDateTime(val) {                    if (!(val instanceof Date)) {                        val = new Date(val);                    }                    var date = new Date();                    var y = date.getFullYear() + "";                    var m = date.getMonth() + 1 + "";                    var d = date.getDate() + "";                    var result = y + "-" + m + "-" + d + " 23:59:59";                    var newDate = new Date(result);                    return val > newDate;                }                //返回当前日期                /*function getCurrentDateTime() {                    var myDate = new Date();                    //获取当前年                    var year = myDate.getFullYear();                    //获取当前月                    var month = myDate.getMonth() + 1;                    //获取当前日                    var date = myDate.getDate() - 1;                    var h = myDate.getHours();       //获取当前小时数(0-23)                    var m = myDate.getMinutes();     //获取当前分钟数(0-59)                    var s = myDate.getSeconds();                    var nowTime = year + '-' + p(month) + "-" + p(date) + " " + p(h) + ':' + p(m) + ":" + p(s);                    return nowTime;                    function p(s) {                        return s < 10 ? '0' + s : s;                    };                };*/                //返回当前日期                function getCurrentDate() {                    /*var myDate = new Date();                    //获取当前年                    var year = myDate.getFullYear();                    //获取当前月                    var month = myDate.getMonth() + 1;                    //获取当前日                    var date = myDate.getDate() - 1;                    var h = myDate.getHours();       //获取当前小时数(0-23)                    var m = myDate.getMinutes();     //获取当前分钟数(0-59)                    var s = myDate.getSeconds();                    // var nowTime = year + '-' + p(month) + "-" + p(date) + " " + p(h) + ':' + p(m) + ":" + p(s);                    var nowDate = year + '-' + p(month) + "-" + p(date);                    return nowDate;                    function p(s) {                        return s < 10 ? '0' + s : s;                    };*/                    var now = new Date();                    var today = new Date(now.getTime());                    //                    var yesterday = new Date(now.getTime() - (1000 * 60 * 60 * 24));                    return gpsjcfxcommon.dateFormatYyyymmdd(today);                };                //获取时间字符串中的时分秒                function getTime(datetimeString) {                    var myDate = new Date(datetimeString);                    var h = myDate.getHours();       //获取当前小时数(0-23)                    var m = myDate.getMinutes();     //获取当前分钟数(0-59)                    var s = myDate.getSeconds();                    return p(h) + ':' + p(m) + ":" + p(s);                    function p(s) {                        return s < 10 ? '0' + s : s;                    };                }                //获取时间字符串中的年月日                function getDate(datetimeString) {                    var myDate = new Date(datetimeString);                    //获取当前年                    var year = myDate.getFullYear();                    //获取当前月                    var month = myDate.getMonth() + 1;                    //获取当前日                    var date = myDate.getDate();                    return year + '-' + p(month) + "-" + p(date);                    function p(s) {                        return s < 10 ? '0' + s : s;                    };                }                //获取时间字符串格式化时间yyyy-MM-dd HH:mm:ss                function dateTimeFormat(dateString) {                    var date = new Date(dateString);                    return date.getFullYear() + '-' + p(date.getMonth() + 1) + '-' + p(date.getDate()) + ' ' + p(date.getHours()) + ':' + p(date.getMinutes()) + ':' + p(date.getSeconds());                    function p(s) {                        return s < 10 ? '0' + s : s;                    };                }                //秒换算时分秒                function secondsToTimeString(sec) {                    sec = +(sec || 0);                    var result = '';                    if (sec) {                        var hour = Math.floor(sec / 3600);                        result += hour ? (hour + '时') : '';                        sec -= 3600 * hour;                        var miniute = Math.floor(sec / 60);                        result += miniute ? (miniute + '分') : '';                        if (miniute) {                            sec -= 60 * miniute;                            result += sec ? (sec + '秒') : '钟';                        } else {                            sec -= 60 * miniute;                            result += sec ? (sec + '秒') : '';                        }                    }                    else {                        result = '0';                    }                    return result;                }                //分换算时分                function minuteToTimeString(sec) {                    sec = +(sec || 0);                    var result = '';                    if (sec) {                        var hour = Math.floor(sec / 60);                        result += hour ? (hour + '时') : '';                        sec -= 60 * hour;                        result += sec ? (sec + '分钟') : '';                    }                    else {                        result = '0';                    }                    return result;                }                //四舍五入                function timeToFixed(value) {                    try {                        if (typeof value !== "number") {                            value = parseFloat(value);                        }                        return value.toFixed();                    } catch (e) {                        return value;                    }                }                function valueTypeToFixed(value, number) {                    if (value == 0) {                        return value;                    }                    try {                        if (typeof value !== "number") {                            value = parseFloat(value);                        }                        return value.toFixed(number);                    } catch (e) {                        return value;                    }                }                //                //行车时长大于切换                //                $('#duration_seconds_select').change(function () {                //                    var selected = $('#duration_seconds_select option:selected').text();                //                    $(this).siblings('input').val(selected);                //                });                /**                 * 查询按钮事件                 * */                $("#btnSearch").click(function (e) {                    e.preventDefault();                    var organizationType;                    helper.UserInfo(function (userInfo) {                        organizationType = userInfo.body.OrganizationType;                    });                    if ((organizationType == 4) || (organizationType == 5) || (organizationType == 6)) { // 代理商，分公司，运营商                        var registration_noval = $("#registration_no").val();                        var enterprise_nameval = $("#enterprise_name").val();                        registration_noval = registration_noval.replace(/\s+/g, "");                        enterprise_nameval = enterprise_nameval.replace(/\s+/g, "");                        if (registration_noval.length == 0 && enterprise_nameval.length == 0) {                            tipdialog.alertMsg("请输入车牌号码或业户名称！");                            return;                        }                        else if ($.trim($('#registration_no').val()).length > 0 && $.trim($('#registration_no').val()).length < 3) {                            tipdialog.alertMsg("请输入车牌号(至少三位)！");                            return;                        }                    } else if ((organizationType == 2) || (organizationType == 7) || (organizationType == 10)) { // 企业，车队                        ;                    } else {                        tipdialog.alertMsg("无查询权限！");                        return;                    }                    $("#tb_stop").CustomTable("reload");                });                /**                 * 重置按钮事件                 * */                $("#btnReset").click(function () {                    $('.searchpanel-form').find('input[type=text]:not(:disabled), select:not(:disabled)').val('');                    $('#CheLiangZhongLei').val("全部");                    InitTime();                });                /*                * 导出按钮事件                * */                $("#btnExport").click(function () {
                    var params = $('#tb_stop').DataTable().ajax.params();


                    if (params != undefined) {
                        params.data.IsExport = 0;
                        helper.Ajax('003300300333', params, function (data) {
                            if (data.body) {
                                var resFileId = data.body.File;
                                window.location.href = helper.Route('00000080005', '1.0', system.ServerAgent) + '?id=' + resFileId;
                            } else {
                                if (data.publicresponse.message == "不存在记录") {
                                    tipdialog.errorDialog('导出失败,不存在记录');
                                } else {
                                    tipdialog.errorDialog('导出失败');
                                }
                            }
                        });
                    } else {
                        tipdialog.errorDialog('请先查询');
                    }                })                /**                 * 重新加载数据                 * */                GPSjcfxList.ReloadTable = function () {                    $("#tb_stop").CustomTable("reload");                };                /**                 * 初始化数据列表                 * */
                function InitTable() {

                    var columns = [];
                    var columnsObj = {
                        'content.YeHuMingCheng': {
                            data: 'content.YeHuMingCheng',
                            render: function (data, type, row) {
                                var result = "";
                                if (data && data !== "null") {
                                    result = data;
                                }
                                return result;
                            }
                        },
                        'content.RegistrationNo': { data: 'content.RegistrationNo' },
                        'content.RegistrationNoColor': { data: 'content.RegistrationNoColor' },
                        'content.CheLiangZhongLei': { data: 'content.CheLiangZhongLei' },
                        'StartTime': {
                            data: 'startTime',
                            render: function (data, type, row) {
                                return data ? dateTimeFormat(data) : '';
                            }
                        },
                        'EndTime': {
                            data: 'endTime',
                            render: function (data, type, row) {
                                return data ? dateTimeFormat(data) : '';
                            }
                        },
                        'content.PersistTime': {
                            data: 'content.PersistTime',
                            render: function (data, type, row) {
                                var startTime = new Date(row.startTime.replace('T', ' ').replace(/-/g, '/'));
                                var endTime = new Date(row.endTime.replace('T', ' ').replace(/-/g, '/'));
                                var result = (endTime - startTime) / 1000;
                                return secondsToTimeString(timeToFixed(result));

                                //if (eventTypeCode === "AQSJ_CLCS001,AQSJ_CLCS002" || eventTypeCode === "CLCS001" || eventTypeCode === "XXSJ001") {
                                //    return secondsToTimeString(timeToFixed(data));
                                //}
                                //return minuteToTimeString(timeToFixed(data));
                            }
                        },
                        'content.MaxSpeed': {

                            data: 'content.MaxSpeed',
                            render: function (data, type, row) {
                                if (data) {
                                    return data + "km/h";
                                }
                            }
                        },
                        'RoadType': {
                            data: 'content.Rules',
                            render: function (data, type, row) {
                                if (data) {
                                    try {
                                        var rules = JSON.parse(data);
                                        var roadType = (rules.RoadType ? rules.RoadType : "");
                                        return roadType;
                                    } catch (e) {
                                        return "";
                                    }
                                }
                            }
                        }
                    };
                    columns.push(columnsObj['content.YeHuMingCheng']);
                    columns.push(columnsObj['content.RegistrationNo']);
                    columns.push(columnsObj['content.RegistrationNoColor']);
                    columns.push(columnsObj['content.CheLiangZhongLei']);
                    columns.push(columnsObj['StartTime']);
                    columns.push(columnsObj['EndTime']);
                    columns.push(columnsObj['content.PersistTime']);
                    columns.push(columnsObj['content.MaxSpeed']);
                    columns.push(columnsObj['RoadType']);

                    $("#tb_stop").CustomTable({
                        ajax: helper.AjaxData("003300400002",
                            function (data) {
                                var pageInfo = { Page: parseInt(data.start / data.length + 1), Rows: data.length };
                                for (var i in data) {
                                    delete data[i];
                                }
                                var content = $('.searchpanel-form.form-horizontal').serializeObject();
                                if (content['RegistrationNo']) {
                                    //车牌号大写
                                    content['RegistrationNo'] = content['RegistrationNo'].toUpperCase();
                                }
                                if (content['CheLiangZhongLei'] == "全部") {
                                    content['CheLiangZhongLei'] = "";
                                }
                                content["AppCode"] = "0033";
                                content["EventTypeCode"] = "CLCS001";
                                content["RuleSourceType"] = "3";
                                content["SourceTypeKey"] = "3";

                                pageInfo.data = content;
                                $.extend(data, pageInfo);
                            }, null),
                        single: false,
                        filter: true,
                        ordering: false,
                        dom: 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
                        columns: columns,
                        pageLength: 10,
                        fnDrawCallback: function (oSettings) {
                            $('#tb_stop tbody tr').find('td:last').css('white-space', 'nowrap');
                            tableheadfix.ResetFix();
                        }
                    });
                    //初始化表头
                    tableheadfix.InitFix();
                }            }            GPSjcfxList.initPage();        });    //追加css    function appendCss(href) {        var link = document.createElement('link');        link.type = 'text/css';        link.rel = 'stylesheet';        link.href = href;        document.head.appendChild(link);    }    return {};});