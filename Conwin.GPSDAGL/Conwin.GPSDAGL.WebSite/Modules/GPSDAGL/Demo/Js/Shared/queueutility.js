/// <reference path="../../assets/global/plugins/jquery.min.js" />
var queueUtility = function (_this, wd, dom) {
    _this.push = function (name, ajaxFn) {
        if (arguments.length != 2) {
            //参数只有一个时执行
            if (typeof arguments[0] != 'function')
                return false;
            ajaxFn = arguments[0];
            name = 'defaultQueue';
        }
        $(dom).queue(name || 'defaultQueue', ajaxFn)
    };
    _this.run = function (name) {
        $(dom).dequeue(name || 'defaultQueue');
    };
    _this.pop = function (name) {
        var context = name || 'defaultQueue';
        $(dom).dequeue(context);
    }
    _this.clear = function (name) {
        $(dom).clearQueue(name || 'defaultQueue');
    };
    //指定队列的长度
    _this.length = function (name) {
        return $(dom).queue(name || 'defaultQueue').length;
    };
    return _this;
}(queueUtility || {}, window, document);