/**
 * Created by  17/2/13.
 */
var pathStaticResource = "/StaticResource/",
    pathStaticTheme1 = pathStaticResource + "Theme1/",
    pathStaticFramework = pathStaticResource + "Framework/",
    pathStaticAssets = pathStaticResource + "Framework/Assets/",
    pathStaticPlugins = pathStaticResource + "Framework/Assets/global/plugins/",
    pathComponent = "../Component/";
require.config({
    baseUrl: '',
    paths: {
        "jquery": pathStaticPlugins + "jquery.min",
        "jquery-migrate": pathStaticPlugins + "jquery-migrate.min",
        "uniform": pathStaticPlugins + "uniform/jquery.uniform.min",
        "slimscroll": pathStaticPlugins + "jquery-slimscroll/jquery.slimscroll.min",
        "metronic": pathStaticAssets + "global/scripts/metronic",
        "toastr": pathStaticTheme1 + "UIControls/conwin.toastr",
        "helper": pathStaticTheme1 + "Js/conwin.helper",
        "formcontrol": "Js/Shared/formcontrol",
        "bootstrap": pathStaticPlugins + "bootstrap/js/bootstrap.min",
        "blockui": pathStaticPlugins + "jquery.blockui.min",
        "respond": pathStaticPlugins + "respond.min",
        "excanvas": pathStaticPlugins + "excanvas.min",
        'textbox': pathStaticTheme1 +'UIControls/conwin.textbox',
        'textarea': pathStaticTheme1 +'UIControls/conwin.textarea',
        'toast': pathStaticTheme1 +'UIControls/conwin.toast',
        'tipdialog': pathStaticTheme1 +'UIControls/conwin.tipdialog',
        'radio': pathStaticTheme1 +'UIControls/conwin.radio',
        'checkbox': pathStaticTheme1 +'UIControls/conwin.checkbox',
        'button': pathStaticTheme1 +'UIControls/conwin.button',
        'dropdown': pathStaticTheme1 +'UIControls/conwin.dropdown',
        "selectcity":pathStaticTheme1 + "UIControls/conwin.selectcity",
        'conwin.main':'../Config/conwin.main',
        'untility': pathStaticTheme1 + "Js/conwin.untility"
    },
    shim: {
        "jquery-migrate": ['jquery'],
        "uniform": ['jquery','jquery-migrate'],
        "blockui": {
            deps: ['jquery','jquery-migrate'],
            exports: 'jquery.blockui'
        },
        "slimscroll":{
            deps: ['jquery'],
            exports: "slimscroll"
        },
        "metronic": {
            deps: ['jquery','jquery-migrate','bootstrap','blockui','slimscroll'],
            exports: "Metronic"
        },
        "bootstrap":{
            deps: ['jquery'],
            exports: "bootstrap"
        },
    }
});
require(['conwin.main', 'jquery', 'uniform', 'slimscroll', 'metronic'], function (main, $, um, sl, Metronic) {
    Metronic.init();
});
