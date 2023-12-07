/// <reference path="../../../../zhyspt/conwin.zhyspt/conwin.zhyspt.website/scripts/default/json2.js" />
define(['jquery', 'tipdialog', 'popdialog', 'system', 'toast', 'helper'], function ($, tipdialog, popdialog, system, Toastr, helper) {

    var enumsSet = {};

    //组织——职务 映射关系 JG对应致职务类型 数字对应用户组织编码
    enumsSet.OrgPositionMap = {
        One: ['JG001', 2, 'One'],
        Two: ['JG005', 0, 'Two'],
        Three: ['JG003', 4, 'Three'],
        Four: ['JG003', 5, 'Four'],
        Five: ['JG003', 6, 'Five'],
        Six: ['JG001', 7, 'Six'],
        Seven: ['JG004', 9, 'Seven'],
        Eight: ['JG002', 10, 'Eight'],
        _key: '组织职务映射'
    }


    this.Object.prototype.toString = function () {
        return this._key;
    }
    this.Array.prototype.toEnumString = function () {
        return this[0];
    }
    this.Array.prototype.name = function () {
        return this[2];
    }
    this.Array.prototype.key = this.Array.prototype.toEnumString;
    this.Array.prototype.enumvalue = function () {
        return this[1];
    }

    return enumsSet;
});