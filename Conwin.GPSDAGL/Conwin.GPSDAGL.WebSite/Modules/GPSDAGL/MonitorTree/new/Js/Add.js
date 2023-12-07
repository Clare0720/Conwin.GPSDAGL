define(['/Modules/Config/conwin.main.js'], function () {
    require.config({ paths: { thisshare: 'Js/share' } });
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity','thisshare', 'layer','jstree','customtable'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system, selectcity, thisshare, layer) {
            var userInfo = helper.GetUserInfo();


            thisshare.InitTree();


            /**
            * 关闭按钮事件
            * ***/
            $('#btnclose').click(function () {
                popdialog.closeIframe();
            });


            $('#saveBtn').click(function () {
                thisshare.AddSave();
            });


            //个性化代码块
            //region
            //endregion

        });
});