define(['jquery', 'helper', 'system', 'bootbox', 'popdialog', 'tipdialog', 'bootstrap3-typeahead', 'jstree'], 
    function ($, helper, system, bootbox, popdialog, tipdialog) {
        var SelectUnit = {};
        var orgdata = {};
        var portletHtml = '';
        var tag = false;
        //初始化数据
        SelectUnit.Init = function() {
            var postdata = { "SysId" : system.SysId };
            helper.Ajax("00000030024", postdata, function (result) {
                    if (result.publicresponse.statuscode == 0) {
                        orgdata = result.body;
                        generateOrgTypeahead();
                    } else {
                        tipdialog.errorDialog('获取组织信息失败，错误信息：' + result.publicresponse.message);
                    }
                }, false);
        };
        SelectUnit.Show = function() {
            generateOrgPortlet();
            showTreeView(portletHtml);                        
            bindJstree();
        };
        SelectUnit.getOrganizationInfo = function(organizationName){
            var organizationinfo = {};
            if (organizationName != "") {
                var org = getChildOrgByName(orgdata, organizationName);
                if (!!org) {
                    organizationinfo.code = $.trim(org.attributes.code);
                    organizationinfo.type = $.trim(org.attributes.orgType);
                }
            }
            return organizationinfo;
        };
        //组织弹窗
        var showTreeView = function(msg) {
            bootbox.dialog({
                message: msg, title: "选择单位", locale: "zh_CN", size: "middle", buttons: {
                    cancle: {
                        label: '取消'
                    },
                    ok: {
                        label: '确定',
                        className: 'green',
                        callback: function () {
                            var selected = $('.participant-list').jstree("get_selected", true);
                            if (selected.length <= 0) {
                                alertMsg("请选择单位");
                                return false;
                            }
                            var text = selected[0].text;
                            $("#lbOrganization").val(text);
                        }
                    }
                }
            });
        }
        //构建组织搜索框
        var generateOrgTypeahead = function() {
            $('#lbOrganization').typeahead({
                source: function (query, process) {
                    var arr = getTypeAheadData(orgdata);
                    var resultList = arr.map(function (item) {
                        return JSON.stringify(item);
                    });
                    return resultList;
                },
                matcher: function (item) {
                    item = JSON.parse(item);
                    return fuzzyMatch(this.query, item.text) || fuzzyPinYinMatch(this.query, item.pinyin);
                },
                sorter: function (items) {
                    var beginswith = [],
                        caseSensitive = [],
                        caseInsensitive = [],
                        item;
                    while ((item = items.shift())) {
                        item = JSON.parse(item);
                        if (fuzzyMatch(this.query, item.text) || fuzzyPinYinMatch(this.query, item.pinyin))
                            beginswith.push(item.text);
                        else if (~item.text.indexOf(this.query)) caseSensitive.push(item.text);
                        else caseInsensitive.push(item.text);
                    }
                    return beginswith.concat(caseSensitive, caseInsensitive);
                }
            });
        }
        //构建弹窗搜索
        var generateOrgPortlet = function() {
            var html = [];
            html.push('<div class="portlet green-meadow box" style="margin-bottom:0px;border-color:#ccc;">');
            html.push('<div class="portlet-body" style="border-top: 1px solid #ccc;"><div class="row" style="margin-bottom:5px;"><div class="col-md-12"><label class="conwinlabel" style="width:98px;float: left">拼音检索：</label>');
            html.push('</div><div class="col-md-12"><input type="text" class="form-control" name="pinyin-search" style="width:240px;float:left;height:34px;">');
            html.push('<a href="javascript:void(0)" class="btn blue clear-pinyin-search"  style=" height: 34px; padding: 7px 14px;"><i class="iconfont icon-zhongzhi"></i>&nbsp;&nbsp;重置</a></div>');
            html.push('</div>');
            html.push('<div class="participant-list" style="max-height:250px;overflow-y:auto;"><ul>');
            $.each(orgdata, function (index, item) {
                html.push(generateTreeItem(item));
            });
            html.push('</ul></div>');
            html.push('</div></div>');
            portletHtml = html.join('');
        }
        //构建树列表项
        var generateTreeItem = function(data) {
            var pinyin = "";
            var code = "";
            var orgtype = "";
            if (data.attributes) {
                pinyin = data.attributes.pinyin;
                code = data.attributes.code;
                orgtype = data.attributes.orgType;
            }
            else{
                code = data.OrganizationCode;
                orgtype = data.OrganizationType;
            }
            var html = [];
            html.push('<li data-id="' + data.Id + '" data-code="' + code + '" data-stype="' + orgtype + '" data-pinyin="' + pinyin + '" data-jstree={"disabled":false}>');
            html.push(data.text);
            if (!!data.children && !!data.children.length) {
                html.push('<ul>');
                $.each(data.children, function (index, item) {
                    html.push(generateTreeItem(item));
                });
                html.push('</ul>');
            }
            html.push('</li>');
            return html.join('');
        }
        //绑定树
        var bindJstree = function() {
            $('.participant-list').jstree({
                "core": {
                    "multiple": false
                },
                "plugins": ["search"]
            });
            $('.participant-list').jstree('open_all');
            $('a.clear-pinyin-search').on('click', function () {
                $('input[name="pinyin-search"]').val('');
                $('.participant-list').jstree('clear_search');
            });
            $('input[name="pinyin-search"]').typeahead({
                source: function (query, process) {
                    var jstree_data = $('.participant-list').jstree('get_json');
                    var jstree_arr = getTreeText(jstree_data);
                    var resultList = jstree_arr.map(function (item) {
                        return JSON.stringify(item);
                    });
                    return resultList;
                },
                matcher: function (item) {
                    item = JSON.parse(item);
                    $('.participant-list').jstree('search', this.query);
                    return fuzzyMatch(this.query, item.text) || fuzzyPinYinMatch(this.query, item.pinyin);
                },
                sorter: function (items) {
                    var beginswith = [],
                        caseSensitive = [],
                        caseInsensitive = [],
                        item;
                    while ((item = items.shift())) {
                        item = JSON.parse(item);
                        if (fuzzyMatch(this.query, item.text) || fuzzyPinYinMatch(this.query, item.pinyin))
                            beginswith.push(item.text);
                        else if (~item.text.indexOf(this.query)) caseSensitive.push(item.text);
                        else caseInsensitive.push(item.text);
                    }
                    return beginswith.concat(caseSensitive, caseInsensitive);
                },
                afterSelect: function (text) {
                    tag = true;
                    $('.participant-list').jstree('search', text);
                }
            });
            $('.participant-list').on('search.jstree', function (event, nodes) {
                if (tag) {
                    tag = false;
                    $('.participant-list').jstree('select_node', nodes.nodes);
                }
            });
        }
        //获取搜索列表数据
        var getTypeAheadData = function(obj) {
            var treeArr = [];
            $.each(obj, function (index, item) {
                treeArr.push({
                    text: $.trim(item.text),
                    pinyin: $.trim(item.attributes.pinyin)
                });
                if (!!item.children) {
                    treeArr = treeArr.concat(getTypeAheadData(item.children));
                }
            });
            return treeArr;
        }
        //获取树选项数据
        var getTreeText = function(obj) {
            var treeArr = [];
            $.each(obj, function (index, item) {
                treeArr.push({
                    text: $.trim(item.text),
                    pinyin: $.trim(item.data.pinyin)
                });
                if (item.children.length) {
                    treeArr = treeArr.concat(getTreeText(item.children));
                }
            });
            return treeArr;
        }
        //根据名字选择组织信息
        var getChildOrgByName = function(org, name) {
            if (!!org && $.trim(name) != "") {
                var length = org.length;
                for (var i = 0; i < length; i++) {
                    var temp = org[i];
                    if (temp.text == name) {
                        return temp;
                    }
                    if (!!temp.children && temp.children.length > 0) {
                        var code = getChildOrgByName(temp.children, name);
                        if (!!code) {
                            return code;
                        }
                    }
                }
            }
            return "";
        }
        //匹配中文
        var fuzzyMatch = function(query, text) {
            var p = query.replace(/(.)(?=[^$])/g, "$1,").split(",").join('.*');
            var pattern = new RegExp(p, "gi");
            return pattern.test(text);
        }
        //匹配拼音首字母
        var fuzzyPinYinMatch = function(query, text) {
            return text.toUpperCase().indexOf($.trim(query).toUpperCase()) >= 0;
        }
        //信息提示窗
        var alertMsg = function (msg) {
            //bootbox.alert(msg);
            tipdialog.errorDialog(msg);
        }
        if (!Array.prototype.map) {
            Array.prototype.map = function (callback, thisArg) {
                var T, A, k;
                if (this == null) {
                    throw new TypeError(" this is null or not defined");
                }
                var O = Object(this);
                var len = O.length >>> 0;
                if (typeof callback !== "function") {
                    throw new TypeError(callback + " is not a function");
                }
                if (thisArg) {
                    T = thisArg;
                }
                A = new Array(len);
                k = 0;
                while (k < len) {
                    var kValue, mappedValue;
                    if (k in O) {
                        kValue = O[k];
                        mappedValue = callback.call(T, kValue, k, O);
                        A[k] = mappedValue;
                    }
                    k++;
                }
                return A;
            };
        }
       return SelectUnit;
});