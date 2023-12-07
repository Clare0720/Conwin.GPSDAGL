/**
 * Created by zh.huang on 2017/2/20. 级联城市选择控件
 * Updated by dt.long on 2020/11/5. 优化控件ID和选择更新ID
 */

define(['jquery', 'helper', 'popdialog', 'selectcity'], function ($, helper, popdialog, selectcity) {

    var Selector = {};
    var val;
    var ctarr;
    var SelectIdVla = {
        select_button_id: 'sel_btn_id',
        select_XiaQuZhen_id: 'sel_zhen_id',
        select_XiaQuXian_id: 'sel_xian_id',
        select_XiaQuShi_id: 'sel_shi_id',
        select_XiaQuSheng_id: 'sel_sheng_id'
    };
    ///正式请求方法
    Selector.showSelectCity = function (data) {
        ctarr = data;
        //popdialog.showModal({
        //    'url': 'chose.html',
        //    'width': '300px',
        //    'showSuccess': Selector.selected
        //});
        popdialog.showModalHtml({
            'html': getViewHtml(),
            'width': '300px'
        });
        Selector.selected();
    };
    Selector.initSelectView = function (items) {
        val = items;
        //var view = '<div id="testSelect" style="margin:20px"><div class="col-xs-4 "  ><div class="form-group"><button id="add" type="submit" class="btn blue" data-dismiss="modal"> 选择辖区</button><div class="col-xs-8" style="border:1px solid gray;"><ul id="content1"  style="height:30px;"></ul></div><input hidden="hidden"  type="text" class="selectcity" id="selectedVal" /></div></div></div>';
        //var view = '<div id="testSelect" style="margin:20px"><div class="col-xs-4 "  ><div class="form-group">< a id= "add" accesskey= "u" title= "选择辖区" class="btn blue" href= "javascript:;" >选择辖区</a ><div class="col-xs-8" style="border:1px solid gray;"><ul id="content1" style="height:30px;"></ul></div> <input hidden="hidden" type="text" class="selectcity" id="selectedVal" /></div ></div ></div > ';
        //var view = '<div id="testSelect" style="margin:20px"><div class="col-xs-4 "><div class="form-group"><a id="add" accesskey="u" title="选择辖区" class="btn blue" href="javascript:;">选择辖区</a><div class="col-xs-8" style="border:1px solid gray;"><ul id="content1" style="height:30px;"></ul></div><input hidden="hidden" type="text" class="selectcity" id="selectedVal" /></div></div></div>';
        var view = '<div style="border:1px solid #ddd;width:70%;float:left;padding:0px;"><ul id="content1" style="height:30px;margin:0;padding:0px;"></ul></div><a id="add" accesskey="u" title="选择辖区" style="margin-left:3px;float:left;" class="btn blue" href="javascript:;">选择辖区</a><input hidden="hidden" type="text" name="' + items.attr('name') + '" class="selectcity" id="selectedVal" />';
        items.append($(view));
    }
    Selector.getSelectedValue = function () {
        return $('#selectedVal').val();
    }
    Selector.selected = function (data) {
        Selector.initXiaQu(ctarr);
        $('#' + SelectIdVla.select_button_id).click(function () {
            var $XiaQuShi = $('#' + SelectIdVla.select_XiaQuShi_id);
            var $XiaQuSheng = $('#' + SelectIdVla.select_XiaQuSheng_id);
            if ($XiaQuShi.val() == '') return;
            var selectedStr = $('#selectedVal')[0].value;
            if (selectedStr.indexOf($XiaQuShi.val()) != -1) return;
            var value = $('#selectedVal').val() == '' ? $XiaQuSheng.val() + $XiaQuShi.val() : $('#selectedVal').val() + "|" + $XiaQuSheng.val() + $XiaQuShi.val();
            $('#selectedVal').val(value);
            var selectV = $XiaQuSheng.val() + $XiaQuShi.val();
            addView(selectV);

        });

    };
    Selector.setData = function (data) {
        $('#content1').children("li").remove();
        if (data == null || data == undefined || data == '')
            return;
        if (data.indexOf('|') == -1) {
            addView(data);
            return;
        }
        if (data.charAt(data.length - 1) == '|') {
            data = data.substring(0, data.length - 1);
        }
        var ctArr = data.indexOf('|') != -1 ? data.split('|') : data;
        //alert(ctArr[0] + 'data');
        $.each(ctArr, function (i) {
            //alert(ctArr[i]);
            addView(ctArr[i]);
        })

    }
    Selector.initXiaQu = function (data) {
        var ct = data;
        var sel_xiaqushi_id = '#' + SelectIdVla.select_XiaQuShi_id;
        var sel_xiaquxian_id = '#' + SelectIdVla.select_XiaQuXian_id;
        var sel_xiaquzhen_id = '#' + SelectIdVla.select_XiaQuZhen_id;
        var defaultOption = '<option value="" selected="selected">请选择</option>';
        $(sel_xiaqushi_id).empty().append(defaultOption);
        setXiaQu(ct, '00000020005', { "Province": "广东" }, ('#' + SelectIdVla.select_XiaQuShi_id), 'GetCityList', 'CityName');
        $(('#' + SelectIdVla.select_XiaQuShi_id)).change(function () {
            if ($(this).val() != $(sel_xiaqushi_id).val()) {

                $(sel_xiaqushi_id).val($(this).val());
                $(sel_xiaquxian_id, sel_xiaquzhen_id).empty().append(defaultOption);
                var data = { "City": $(this).val() };
                if ($(this).val() != '') {
                    ///调用接口初始化区县下拉框
                    setXiaQu(ct, '00000020006', data, sel_xiaquxian_id, 'GetDistrictList', 'DistrictName');
                }
            }
        });
        $(sel_xiaquxian_id).change(function () {
            $(sel_xiaquzhen_id).empty().append(defaultOption);
            var data = { "District": $(this).val() };
            if ($(this).val() != '') {
                ///调用接口初始化区镇下拉框
                setXiaQu(ct, '00000020007', data, sel_xiaquzhen_id, 'GetTownList', 'TownName');
            }
        });
    };
    function getViewHtml() {
        var html = '';

        html += '<div class="modal-dialog"><div class="modal-content"><div class="modal-header">'
            + '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>'
            + '<h4 class="modal-title">选择辖区市</h4>'
            + '</div>'
            + '<div class="form-group" style="padding:10px;margin-left:20px;">'
            + '<label class="control-label">辖区省</label>'
            + '<select id="' + SelectIdVla.select_XiaQuSheng_id + '" class="selectcity"><option value="广东">广东</option></select>'
            + '<label class="control-label">辖区市</label>'
            + '<select id="' + SelectIdVla.select_XiaQuShi_id + '" class="selectcity">'
            + '<option value="">请选择</option>'
            + '</select>'
            + '</div>'
            + '<div class="modal-footer">'
            + '<a href="javascript:void(0)" class="btn blue" id="' + SelectIdVla.select_button_id + '" data-dismiss="modal">&nbsp;确定</a>'
            + '</div></div></div>';
        // +'<button id="'+ SelectIdVla.select_button_id +'" type="button" class="btn blue" data-dismiss="modal"> 确定</button>'            
        return html;
    }
    function addView(val) {
        if (val == "" || val == undefined || val == null)
            return;
        if (val.length < 4)
            return;
        $('#content1').append("<li><div name=" + '"' + val + '"' + "style=" + '"' + "border:1px solid #ddd;margin:5px;" + '"' + ">" + val + "<a href=" + '"' + "#" + '"' + "class=" + '"' + "select2 - search -choice - close closeSelected" + '"' + " tabindex=" + '"' + "- 1" + '"' + "></a></div></li>");
        $('ul li').attr('style', " float: left;list-style: none;");
        $('.closeSelected').attr('style', 'margin-top:5px;');
        $('ul').attr('style', 'margin:0;padding:0;');
        $('.closeSelected').click(function (e) {
            e.preventDefault();
            var delitems = $(this).parent().text();
            var val = $('#selectedVal').val();
            //var res = upDate(val, delitems);
            //$('#selectedVal').val(res);
            var arr;
            if (val.indexOf(delitems) != -1) {
                if (val.indexOf("|" + delitems) != -1) {
                    arr = val.split("|" + delitems);
                    $('#selectedVal').val(arr[0] + arr[1])
                } else {
                    $('#selectedVal').val(val.substring(val.indexOf(delitems) + 5, val.length));
                }
            }

            $(this).parent().parent().remove();
            if ($('#content1').children("li").length == 0) {
                $('#content1').height('32px');
            }
        });
    }
    function setXiaQu(CityParm, servercode, data, control, operation, textfield, valuefield, selectvalue) {
        helper.Ajax(servercode, data, function (json) {
            if (json.publicresponse.statuscode == '0') {
                var result = json.body;
                valuefield = valuefield || textfield;
                if (result.length > 0) {
                    var optionStr = '<option value="">请选择</option>';
                    $(result).each(function (i, item) {
                        if (selectvalue != undefined && selectvalue == item[valuefield]) {
                            optionStr += '<option value="' + item[valuefield] + '" selected="selected">' + item[textfield] + '</option>';
                            //alert(item.Key + '12' + item[valuefield]);

                        } else {
                            if (CityParm != undefined && CityParm != null) {
                                for (var i = 0; i < CityParm.length; i++) {
                                    var ct = CityParm[i].substring(2, 4);
                                    if (ct == item.Key) {
                                        optionStr += '<option value="' + item.Key + '">' + item.Key + '</option>';

                                    }
                                }
                            } else {
                                optionStr += '<option value="' + item.Key + '">' + item.Key + '</option>';
                            }
                        }
                    });
                    $(control).empty().append(optionStr);
                    if (operation && $.isFunction(operation)) {
                        operation();
                    }
                } else {
                    warning.alertMsg(json.msg);
                }
            }
        }, false);
    }
    return Selector;
});