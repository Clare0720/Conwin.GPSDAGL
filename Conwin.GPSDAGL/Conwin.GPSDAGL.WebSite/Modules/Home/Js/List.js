define(['/Modules/Config/conwin.main.js'], function () {
    require(['jquery', 'helper', 'tableheadfix', 'common'], function ($, helper, tableheadfix) {
        var InitPage = function () {
            tableheadfix.InitFix();
        }
        InitPage();
    });
});