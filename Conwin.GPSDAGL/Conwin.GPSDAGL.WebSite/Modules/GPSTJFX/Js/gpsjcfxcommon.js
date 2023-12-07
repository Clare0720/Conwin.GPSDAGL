/**
 * Created by sunjian on 17/8/17.
 */
define(['jquery'], function ($) {
    
    var common = {};
    
    /**
     * 获取URL参数
     */
    common.getURLParameter = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = decodeURI(window.location.search.substr(1)).match(reg);
        if (r != null) return unescape(r[2]);
        return null;
    };
    
    /**
     * 添加过滤字段
     */
    common.addFieldFilters = function (fieldName, operator, fieldFilters) {
        var val = $('#' + fieldName).val();
        if (val)
            fieldFilters.push({
                f: fieldName,
                o: operator,
                v: (operator === 'like' ? ('%' + val + '%') : val)
            });
    }
    
    /**
     * 添加过滤字段
     */
    common.addFieldFiltersNV = function (fieldName, fieldValue, operator, fieldFilters) {
        fieldFilters.push({
            f: fieldName,
            o: operator,
            v: fieldValue
        });
    }
    
    /**
     * 当月第一天
     */
    common.getFirstDayOfTheMonth = function (date) {
        var year = date.getFullYear(), month = date.getMonth();
        return new Date(year, month, 1);
    };
    
    /**
     * 当月最后一天
     */
    common.getLastDayOfTheMonth = function (date) {
        var year = date.getFullYear(), month = date.getMonth(), firstDayOfNextMonth = new Date(year, month + 1, 1);
        return new Date(firstDayOfNextMonth.getTime() - 1);
    };
    
    /**
     * 当年第一天
     */
    common.getFirstDayOfTheYear = function (date) {
        var year = date.getFullYear();
        return new Date(year, 0, 1);
    };
    
    /**
     * 当年最后一天
     */
    common.getLastDayOfTheYear = function (date) {
        var year = date.getFullYear();
        return new Date(year, 11, 31, 23, 59, 59, 999);
    };
    
    /**
     * 日期格式化yyyy
     */
    common.dateFormatYyyy = function (date) {
        if (!(date instanceof Date)) return '';
        
        return date.getFullYear().toString();
    };

    /**
     * 日期格式化yyyy-MM
     */
    common.dateFormatYyyymm = function (date) {
        if (!(date instanceof Date)) return '';
        
        var yyyy = common.dateFormatYyyy(date);
        var mm = date.getMonth() + 1;
        mm = (mm > 9) ? mm.toString() : ('0' + mm);
        return yyyy + '-' + mm;
    };

    /**
     * 日期格式化yyyy-MM
     */
    common.dateFormatMm = function (date) {
        if (!(date instanceof Date)) return '';
        
        var yyyy = common.dateFormatYyyy(date);
        var mm = date.getMonth() + 1;
        mm = (mm > 9) ? mm.toString() : ('0' + mm);
        return mm;
    };
    
    /**
     * 日期格式化yyyy-MM-dd
     */
    common.dateFormatYyyymmdd = function (date) {
        if (!(date instanceof Date)) return '';
        
        var yyyymm = common.dateFormatYyyymm(date);
        var dd = date.getDate();
        dd = (dd > 9) ? dd.toString() : ('0' + dd);
        return yyyymm + '-' + dd;
    };
    
    return common;
});
