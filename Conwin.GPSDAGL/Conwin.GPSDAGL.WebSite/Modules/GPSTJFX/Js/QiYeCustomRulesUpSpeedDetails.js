/**
                    $('#DateStart').datepicker('setDate', beginDateTime);
                    $('#DateEnd').datepicker('setDate', beginDateTime);
                }
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
                    }
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
                }