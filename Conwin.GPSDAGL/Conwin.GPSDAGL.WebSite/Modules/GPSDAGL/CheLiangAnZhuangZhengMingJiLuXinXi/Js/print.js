define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'searchbox', 'customtable', 'bootstrap-datepicker.zh-CN'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system, selectcity) {
            var id = (window.location.search.split('=')[1]).split('&')[0];
            var type = window.location.search.split('&')[1].split('=')[1];
            id = id != undefined ? id : "";
            //alert(id);
            helper.Ajax('003300300099', id, function (data) {
                //console.info(data);
                if (data.body) {
                    $('#btnPrint').click(function (e) {
                        if (!!window.ActiveXObject || "ActiveXObject" in window) {
                            remove_ie_header_and_footer();
                        }
                        window.print();                     
                    });
                    $('.name').text(removeNull(data.body.Name));
                    $('.p-value').each(function (i, item) {
                        var v = $(item).attr('name');
                        //alert(v);
                        if (v == 'North') {
                            var isNorth = data.body.DingWeiMoKuai.indexOf('北斗') != -1 ? true : false;
                            if (isNorth)
                                $(item).text('√');

                        } else if (v == 'Double') {
                            var isDouble = data.body.DingWeiMoKuai.indexOf('双模') != -1 ? true : false;
                            if (isDouble)
                                $(item).text('√');
                        } else if (v == 't0') {
                            $(item).text('√');
                        } else if (v == 'fw-year') {
                            //var year = data.body.FuWuJieZhiRiQi.split('-')[0];
                            var year = Split(data.body.FuWuJieZhiRiQi, '-')[0];
                            $(item).text(year);
                        } else if (v == 'fw-month') {
                            //var month = data.body.FuWuJieZhiRiQi.split('-')[1];
                            var month = Split(data.body.FuWuJieZhiRiQi, '-')[1];
                            $(item).text(month);
                        } else if (v == 'fw-day') {
                            //var day = data.body.FuWuJieZhiRiQi.split('-')[2].split('T')[0];
                            var day = Split(Split(data.body.FuWuJieZhiRiQi, '-')[2], 'T')[0];
                            $(item).text(day);
                        } else if (v == 'cc-year') {

                            //var year = data.body.AnZhuangRiQi.split('-')[0];
                            var year = Split(data.body.AnZhuangRiQi, '-')[0];
                            $(item).text(year);
                        } else if (v == 'cc-month') {
                            //var month = data.body.AnZhuangRiQi.split('-')[1];
                            var month = Split(data.body.AnZhuangRiQi, '-')[1];

                            $(item).text(month);
                        } else if (v == 'cc-day') {
                            //var day = data.body.AnZhuangRiQi.split('-')[2].split('T')[0];
                            var day = Split(Split(data.body.AnZhuangRiQi, '-')[2], 'T')[0];
                            $(item).text(day);
                        }
                        else if (v == 'year-fo') {
                            //var year = data.body.TiemNow.split('/')[0];
                            var year = Split(data.body.TiemNow, '-')[0];
                            $(item).text(year);
                        } else if (v == 'month-fo') {
                            //var month = data.body.TiemNow.split('/')[1];
                            var month = Split(data.body.TiemNow, '-')[1];
                            $(item).text(month);
                        } else if (v == 'day-fo') {
                            //var day = data.body.TiemNow.split('/')[2].split(' ')[0];
                            var day = Split(Split(data.body.TiemNow, '-')[2], 'T')[0];
                            $(item).text(day);
                        } /*else if (v == 'comLi') {
                            $(item).text('广东恒慧信息科技有限公司')
                        }*/ else if (v == 'piCi2') {
                            $(item).text('十四');
                        } else if (v == 'PingTaiBianHao') {
                            $(item).text('51439');
                        }
                        else {
                            $(item).text(removeNull(data.body[v]))
                        }
                        //alert(v);


                    });
                    if (type == 1) {
                        $('.img_gz').attr("src", '' + helper.Route('00000080004', '1.0', system.ServerAgent) + '?id=' + data.body.GongZhangZhaoPianId)
                            .show();
                        //$('.img_gz').attr('src', '' + helper.Route('00000080004', '1.0', system.ServerAgent) + '?id=' + data.body.GongZhangZhaoPianId);
                        //写记录
                    }
                } else {
                    //tipdialog.errorDialog("请求数据失败");
                    alert("请求数据失败");
                }
            });
            helper.Ajax('003300300097', { "CheLiangAnZhuangZhengMingJiLUID": id }, function (data) {
            });

        });
    function removeNull(data) {
        if (typeof (data) != 'string')
            return ''
        return data;
    }

    function Split(str, tag) {
        if (str == null || str == '' || typeof (str) != 'string') {
            return '';
        }
        else if (tag == null || tag == '' || typeof (tag) != 'string') {
            return str;
        }
        else {
            return str.split(tag);
        }
    }
    function remove_ie_header_and_footer() {
        var hkey_root, hkey_path, hkey_key;
        hkey_path = "HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\PageSetup\\";
        try {
            var RegWsh = new ActiveXObject("WScript.Shell");
            RegWsh.RegWrite(hkey_path + "header", "");
            RegWsh.RegWrite(hkey_path + "footer", "");
        } catch (e) { }
    }

});