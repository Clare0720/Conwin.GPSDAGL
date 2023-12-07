define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'tipdialog', 'helper', 'prevNextpage', 'common', 'popdialog', 'formcontrol', 'filelist', 'customtable'],
        function ($, tipdialog, helper, prevNextpage, common, popdialog, formcontrol, filelist) {
            var initPage = function () {
                //初始化页面样式
                common.AutoFormScrollHeight('#Form1');
                formcontrol.initial();
                //翻页控件
                var ids = window.parent.document.getElementById('hdIDS').value;
                prevNextpage.initPageInfo(ids.split(',').reverse());
                prevNextpage.bindPageClass();
                //初始化子表
                //关闭
                $('#btnclose').click(function () {
                    popdialog.closeIframe();
                });
                //上一条
                $('#prevBtn').click(function (e) {
                    e.preventDefault();
                    prevNextpage.prev();
                    updateData();
                    updateTag();
                });
                //下一条
                $('#nextBtn').click(function (e) {
                    e.preventDefault();
                    prevNextpage.next();
                    updateData();
                    updateTag();
                });
                //附件列表
                $('#tab3').click(function (e) {
                    e.preventDefault();
                    $('#FuJian').filelist({
                        'type': 'view',
                        'businessType': '000001',
                        'businessId': $('#Id').val()
                    });
                });
                updateData();
                //个性化代码块
                //region
                //endregion
            };
            //主表-刷新数据
            function updateData() {
                var id = prevNextpage.PageInfo.IDS[prevNextpage.PageInfo.Index];
                getXianLuXinXi(id, function (serviceData) {
                    if (serviceData.publicresponse.statuscode == 0) {
                        fillFormData(serviceData.body);
                    } else {
                        tipdialog.errorDialog("请求数据失败");
                    }
                });
            };
            //主表-获取主表数据
            function getXianLuXinXi(id, callback) {
                //调用获取单条信息接口
                helper.Ajax("003300300084", id, function (resultdata) {
                    if (typeof callback == 'function') {
                        callback(resultdata);
                    }
                }, false);
            };
            //主表-绑定主表数据
            function fillFormData(resource) {
                $('#Form1').find('.form-control-static').each(function (i, item) {
                    var index = $(item).attr('for');
                    var tempValue = resource[index];
                    if (tempValue != undefined) {
                        //TODO: 赋值
                        if ($(item).hasClass('datetimepicker')) {
                            tempValue = tempValue.substr(0, 10);
                        }
                        if ($(item).hasClass('datepicker')) {
                            tempValue = tempValue.substr(0, 10);
                        }
                        if ($(item).attr('for') == 'ShouFeiNeiRong') {
                            var ShouFeiStr = { 1:'卡流量费', 2:'终端费',3: '续网',4: '新装',5: '转网',6: '挂网',7: '人工费',8: '换机费'};
                            tempValue = ShouFeiStr[tempValue] != undefined ? ShouFeiStr[tempValue] : '';
                        }
                        if ($(item).attr('for') == 'ZhiFuFangShi'){
                            var PayTypeArr = { 1: '支付宝', 2: '微信', 3: '转账', 4: '现金',0:'其他' };
                            tempValue = PayTypeArr[tempValue] != undefined ? PayTypeArr[tempValue] : '';
                        }
                        if ($(item).attr('for') == 'ShouFeiMoShi') {
                            var ShouFMS = { 1: '季度', 2: '半年', 3: '一年', 4: '两年', 0: '三年' };
                            tempValue = ShouFMS[tempValue] != undefined ? ShouFMS[tempValue] : '';
                        }
                        if ($(item).attr('for') == 'ShouFeiJinE') {
                            tempValue = Math.round(tempValue * 100) / 100;
                        }
                        if ($(item).attr('for') == 'ShiShouJinE') {
                            tempValue = Math.round(tempValue * 100) / 100;
                        }
                        if ($(item).attr('for') == 'HuiKuanJinE') {
                            tempValue = Math.round(tempValue * 100) / 100;
                        }
                        $(item).html(tempValue.toString() == '' ? '' : tempValue);
                    } else {
                        $(item).html('');
                    }
                });
                $('#Id').val(resource.Id);
            };
            //主表-更新tab状态
            function updateTag() {
                $('#tab1').parent('li').addClass('active');
                $('#JiChuXinXi').addClass('active in');
                $('#tab3').parent('li').removeClass('active');
                $('#FuJian').removeClass('active in');
            };
            //子表-初始化分页信息
            var tabPageInfo = tabPage();
            //子表-分页
            function tabPage() {
                var tabPageInfo = {};
                tabPageInfo.bindPageClass = function () {
                    var currentPageInfo = tabPageInfo.PageInfo;
                    if (currentPageInfo.HasNext) {
                        $('#nextTabBtn').removeClass('disabled');
                        $('#nextTabBtn').removeClass('c-gray-btn');
                        $('#nextTabBtn').removeAttr('disabled');
                        $('#nextTabBtn').addClass('c-green');
                        $('#nextTabBtn').children(':first').removeClass('m-icon-gray');
                        $('#nextTabBtn').children(':first').addClass('m-icon-white');
                    } else {
                        $('#nextTabBtn').addClass('disabled');
                        $('#nextTabBtn').addClass('c-gray-btn');
                        $('#nextTabBtn').attr("disabled", "disabled");
                        $('#nextTabBtn').removeClass('c-green');
                        $('#nextTabBtn').children(':first').addClass('m-icon-gray');
                        $('#nextTabBtn').children(':first').removeClass('m-icon-white');
                    }
                    if (currentPageInfo.HasPrev) {
                        $('#prevTabBtn').removeClass('disabled');
                        $('#prevTabBtn').removeClass('c-gray-btn');
                        $('#prevTabBtn').removeAttr('disabled');
                        $('#prevTabBtn').addClass('c-green');
                        $('#prevTabBtn').children(':first').removeClass('m-icon-gray');
                        $('#prevTabBtn').children(':first').addClass('m-icon-white');
                    } else {
                        $('#prevTabBtn').addClass('disabled');
                        $('#prevTabBtn').addClass('c-gray-btn');
                        $('#prevTabBtn').attr("disabled", "disabled");
                        $('#prevTabBtn').removeClass('c-green');
                        $('#prevTabBtn').children(':first').addClass('m-icon-gray');
                        $('#prevTabBtn').children(':first').removeClass('m-icon-white');
                    }
                };
                //分页信息
                tabPageInfo.PageInfo = {
                    IDS: [],
                    Index: 0,
                    PageSize: 0,
                    HasPrev: false,
                    HasNext: false
                };
                //初始化子页面记录数据
                tabPageInfo.initPageInfo = function (ids) {
                    tabPageInfo.PageInfo.IDS = ids;
                    tabPageInfo.PageInfo.Index = 0;
                    tabPageInfo.PageInfo.PageSize = ids.length;
                    tabPageInfo.PageInfo.HasNext = ids.length > 1;
                    tabPageInfo.PageInfo.HasPrev = false;
                };
                //计算分页信息
                tabPageInfo.calculatePage = function (tag) {
                    if (tag == undefined)
                        return tabPageInfo.PageInfo;
                    //标识
                    if (tag > 0) {
                        tabPageInfo.PageInfo.Index++;
                    }
                    else {
                        tabPageInfo.PageInfo.Index--;
                    }
                    tabPageInfo.PageInfo.HasNext = tabPageInfo.PageInfo.PageSize > (tabPageInfo.PageInfo.Index + 1);
                    tabPageInfo.PageInfo.HasPrev = tabPageInfo.PageInfo.Index > 0;
                    return tabPageInfo.PageInfo;
                };
                tabPageInfo.next = function () {
                    tabPageInfo.calculatePage(1);
                    tabPageInfo.bindPageClass();
                };
                tabPageInfo.prev = function () {
                    tabPageInfo.calculatePage(-1);
                    tabPageInfo.bindPageClass();
                };
                return tabPageInfo;
            }
            //个性化代码块
            //region
            //endregion
            initPage();
        });
});