/// <reference name="jquery.min.js"/>

define(['jquery', 'helper', 'tableheadfix'], function ($, helper, tableheadfix) {

    // var tableOptions;
    // var isReload = false;
    // var isFirst = true;
    $.fn.CustomTable = function (option, value) {
        var result,
            element = this.each(function () {
                var $this = $(this);

                var options = {};
                //dataTable默认参数
                options = $.extend({}, $.fn.CustomTable.defaults);

                if (typeof option === 'object') {
                    $.extend(true, options, option);
                } else if (typeof option === 'string') {
                    var data = new CustomTable($this, options);
                    result = data[option](value);
                    return;
                }
                if (!options.columns) {
                    var columns = [];
                    if (options.columnsStart) {
                        $.extend(columns, options.columnsStart);
                    }
                    $('thead>tr>th', $this).each(function (index) {
                        columns.push({
                            'data': $(this).data('name'),
                            'orderable': $(this).data('orderable')
                        });
                    });
                    if (options.columnsEnd) {
                        columns = columns.concat(options.columnsEnd);
                    }
                    options.columns = columns;
                }
                $this.data('login', options.login);
                for (var i = 0, len = options.columns.length; i < len; i++) {
                    if (!options.columns[i].render || typeof options.columns[i].render !== 'function') {
                        options.columns[i].render = renderFunc;
                    }
                    options.columns[i].orderable = !!options.columns[i].orderable;
                }


                if (helper.BrowserVersion.browser == "IE" && parseInt(helper.BrowserVersion.version) == 8) {
                    $('.group-checkable', $this).live('change', function (e) {
                        //e.preventDefault();

                        var checked = $(this).prop('checked');
                        if (checked) {
                            $(this).parent('span').addClass("checked");
                        } else {
                            $(this).parent('span').removeClass("checked");
                        }

                        $('.checkboxes', $this).prop('checked', $(this).prop('checked'));
                        $.uniform.update();
                        $('.checkboxes', $this).parents('tr').toggleClass('selected', $(this).prop('checked'));
                    });

                } else {
                    //表格数据全选或反选
                    $('.group-checkable', $this).on('change', function (e) {
                        //e.preventDefault();
                        var checked = $(this).prop('checked');
                        if (checked) {
                            $(this).parent('span').addClass("checked");
                        } else {
                            $(this).parent('span').removeClass("checked");
                        }
                        $('.checkboxes', $this).prop('checked', $(this).prop('checked'));
                        $.uniform.update();
                        $('.checkboxes', $this).parents('tr').toggleClass('selected', $(this).prop('checked'));
                    });
                }

                //是否单选
                if ($this.attr('single') || options.single) {
                    //$('.group-checkable', $this).remove();
                    $($this).on('click.checkboxes', '.checkboxes', function () {
                        if ($(this)[0].checked) {
                            $('.checkboxes', $this).not(this).prop('checked', false).parents('tr').toggleClass('selected', false);
                            $.uniform.update();
                        }
                    });
                }

                if (!$this.data('isReload')) {
                    $this.data('tableoption', $.extend({}, options));
                    delete options['serverSide'];
                    delete options['ajax'];
                    options.data = [];
                    $this.find('tbody').on('click', 'tr', function (e) {
                        if (!$(e.target).hasClass('select-ignore') && $(e.target).parents('.select-ignore').length <= 0) {
                            if (!$(e.target).is('input[type="checkbox"]')) {
                                $('.checkboxes', $(this)).trigger('click');
                                $.uniform.update();
                            }
                            $(this).toggleClass('selected', $('.checkboxes', $(this)).prop('checked'));
                        }
                        if ($('.checkboxes:checked', $this).length == $('.checkboxes', $this).length) {
                            $('.group-checkable', $this).attr("checked", true);
                            $('.group-checkable', $this).parent('span').addClass('checked');
                        } else {
                            $('.group-checkable', $this).attr("checked", false);
                            $('.group-checkable', $this).parent('span').removeClass('checked');
                        }
                    });
                    result = $this.DataTable(options);
                } else {
                    options.language.emptyTable = "找不到结果";
                    options.ajax = ajaxBuilder(options.ajax, !!options.ordering);
                    result = $this.DataTable(options);
                }

                if (options.filter) {
                    var filterDom = $this.parents('.dataTables_wrapper').find('.dataTables_filter');
                    if ($this.data('isReload')) {
                        filterDom.html($this.data('filter'));
                    } else {
                        var filterHTML = '<div>';
                        filterHTML += '<a id="customertable-datatables-filter-button" class="btn blue datatables-filter-button">表格列筛选</a>';
                        filterHTML += '<div id="customertable-datatables-filter-list" class="datatables-filter-list">';
                        filterHTML += '<ul style="padding:0;">';
                        var ths = $('thead th', $this);
                        for (var i = 0, len = options.columns.length; i < len; i++) {
                            var columnName = $.trim(ths.eq(i).text());
                            if (columnName) {
                                filterHTML += '<li style="padding: 5px 5px;"><input type="checkbox" class="checkboxes" data-column="' + i + '" checked />' + columnName + '</li>';
                            }
                        }
                        filterHTML += '<li style="padding: 5px 5px;"><a id="btnFilterClose" href="javascript:;" class="btn c-blue"><i class="fa fa-remove"></i>关闭</a></li>';
                        filterHTML += '</ul>';
                        filterHTML += '</div>';
                        filterHTML += '</div>';
                        filterDom.html(filterHTML);

                        bindFilterClick();
                    }

                }
                $this.parents('.dataTables_wrapper').find('.dataTables_filter .datatables-filter-list ul li input[type="checkbox"]').each(function (i, item) {
                    var table = $(item).parents('.dataTables_wrapper').find('table');
                    table.DataTable().column($(item).data('column')).visible($(item).prop('checked'));
                })
                $this.parents('.dataTables_wrapper').find('.dataTables_filter .datatables-filter-list ul li input[type="checkbox"]').on('click', function (e) {
                    var table = $(e.target).parents('.dataTables_wrapper').find('table');
                    table.DataTable().column($(e.target).data('column')).visible($(e.target).prop('checked'));

                    tableheadfix.ResetFix();
                    e.stopPropagation();
                });

                if (options.searchInput) {
                    $(options.searchInput).on('focusout', function () {
                        var searchValue = $(this).val();
                        if (searchValue) {
                            result.search(searchValue).draw(false);
                        }
                    });
                }
                if (options.searchButton) {
                    $(options.searchButton).on('click', function () {
                        result.ajax.reload();
                    });
                }

                $('.group-checkable').attr("checked", false);
                $('.group-checkable').parent('span').removeClass('checked');

                $('.table-scrollable').attr('style', 'margin: 1px 0 !important;');

                var firstculumms = $('th', $this).first()
                var firstculumms_name = firstculumms.data('name');
                for (var i = 0; i < options.columns.length; i++) {
                    var element = options.columns[i];
                    if (element.data == firstculumms_name) {
                        if (element.orderable == false) {
                            firstculumms.removeClass('sorting_asc');
                            $this.data('remove_sorting_asc', true);
                        } else {
                            firstculumms.removeClass('sorting_asc');
                            firstculumms.addClass('sorting');
                        }

                    }
                }
                $('input[type="checkbox"]').not('.group-checkable').uniform();
            });

        return result;
    };

    var ajaxBuilder = function (opts, ordering) {
        var conf = $.extend({
            url: '',
            data: null,
            method: opts.type || 'GET',
            cache: false,
            ordering: ordering
        }, opts);
        var dataFunction = null;
        if ($.isFunction(conf.data.body)) {
            dataFunction = conf.data.body;
        }
        var publicrequest = conf.data.publicrequest;
        return function (request, drawCallback, settings) {
            var element = $(this);
            var ischangepagesize=false;
            $.support.cors = true;
            if (dataFunction && $.isFunction(dataFunction)) {
                //var paginateData = { Page: parseInt(request.start / request.length) + 1, Rows: request.length };
                var orderData = {};
                if (conf.ordering && request.order.length) {
                    orderData.order_name = request.columns[request.order[0].column].data;
                    orderData.order_dir = request.order[0].dir;
                }
                if(request.start%request.length!=0){
                    request.start=0;
                    ischangepagesize=true;
                }
                dataFunction.call(this, request);
                var token = this.data("token");
                conf.headers.token = token;
                conf.data = {};
                conf.data.publicrequest = publicrequest;
                conf.data.publicrequest.requesttime = helper.DateFormat(new Date(), "yyyyMMddHHmmssfff"),
                    conf.data.publicrequest.reqid = helper.NewGuid(),
                    conf.data.body = request;
                $.extend(conf.data.body, orderData);
                conf.data = {
                    '': JSON.stringify(conf.data)
                };
            }
            var cb = conf.success;
            settings.jqXHR = $.ajax($.extend(conf, {
                "success": function (json) {
                    if (cb) {
                        var re = cb.call(this, json);
                        if (typeof re === 'object') {
                            json = re;
                        }
                    }
                    if (typeof this.dataProcess === 'function') {
                        this.dataProcess.call(this, json);
                    }
                    if (!json.publicresponse) {
                        json = JSON.parse(json);
                    }
                    if (json.publicresponse.statuscode == "0") {
                        json = {
                            data: json.rows || json.body.items || [],
                            iTotalRecords: (json.rows || json.body.items || '').length,
                            iTotalDisplayRecords: json.body.totalcount || 0
                        };
                        var error = json.error || json.sError;
                        if (error) {
                            settings.oApi._fnLog(settings, 0, error);
                        }
                        if(ischangepagesize){
                             settings._iDisplayStart=0;
                        }
                        settings.oApi._fnCallbackFire(settings, null, 'xhr', [settings, json]);
                        settings.json = json;
                        drawCallback(json);
                        $('.group-checkable', element).attr("checked", false);
                        $('.group-checkable', element).parent('span').removeClass('checked');
                        if (element.data('remove_sorting_asc')) {
                            $('th', element).first().removeClass('sorting_asc');
                        }
                        $('input[type="checkbox"]').not('.group-checkable').uniform();
                    }
                }
            }));
        };
    };

    var renderFunc = function (data) {
        return data === 0 ? data : (data || '');
    };

    var bindFilterClick = function (e) {
        $('.dataTables_filter .datatables-filter-button').click(function (e) {
            $(this).next('.datatables-filter-list').show();
            e.stopPropagation();
        });

        $('.dataTables_filter #btnFilterClose').click(function (e) {
            $(this).parents('.datatables-filter-list').hide();
            e.stopPropagation();
        });

        $('.dataTables_filter .datatables-filter-list, .dataTables_filter .datatables-filter-list ul').click(function (e) {
            $(this).next('.datatables-filter-list').hide();
            e.stopPropagation();
        });

        $(document).click(function (e) {
            $('#customertable-datatables-filter-list').hide();
        });
    };

    $.fn.CustomTable.defaults = {
        "processing": true, //加载数据时候是否显示进度条
        "serverSide": true, //是否从服务加载数据 
        "filter": false, //过滤功能
        "sort": false,
        "ordering": false, //排序
        "pageLength": 50, // default records per page，
        "lengthMenu": [
            [10, 20, 50, 100, 200],
            [10, 20, 50, 100, 200]
        ],
        "language": {
            "aria": {
                "sortAscending": ":升序",
                "sortDescending": ":降序"
            },
            "processing": "读取中",
            "emptyTable": "请先进行查询",
            "info": "第 _START_ 到 _END_ 共 _TOTAL_ 条",
            "infoEmpty": "",
            "infoFiltered": "", //"(从 _MAX_ 条数)",
            "lengthMenu": "显示 _MENU_ 条目",
            "search": "搜索:",
            "zeroRecords": "找不到结果",
            "lengthMenu": "  _MENU_ ",
            "paginate": {
                "previous": "<",
                "next": ">",
                "last": "<<",
                "first": ">>"
            }
        },
        "orderCellsTop": false,
        "dom": '<"datatable_filter"f>r<"table-scrollable"t><"row"<"col-md-2 col-sm-12"l><"col-md-3 col-sm-12"i><"col-md-7 col-sm-12 pagnav"p>>',
        "stateSave": false,
        "pagingType": "bootstrap_full_number",
        //自定义
        "single": false,
        "checkbox": false,
        "searchInput": null,
        "searchButton": null,
        "login": true
    };

    var CustomTable = function (element, options) {
        this.options = options;
        this.$element = element;
    };

    CustomTable.prototype = {
        reload: function () {
            var token = "";
            if (this.$element.data('login')) {
                token = helper.GetToken();
                if (token == "" || helper.IsTokenTimeOut()) {
                    helper.Login();
                    return;
                }
                this.$element.data("token", token);
            }
            if ($.fn.DataTable.isDataTable(this.$element) && this.$element.data('isReload')) {
                this.$element.DataTable().ajax.reload();
            } else {
                this.$element.data('isReload', true);
                this.$element.data('filter', this.$element.parents('.dataTables_wrapper').find('.dataTables_filter div')[0]);
                var dt = this.$element.DataTable();
                dt.destroy();
                this.$element.CustomTable(this.$element.data('tableoption'));
            }
            //$('body').removeClass('rui-window-open');
        },
        getSelected: function (ele) {
            if ($.fn.DataTable.isDataTable(this.$element)) {
                var selectedRow;
                if (ele) {
                    if (!(ele instanceof jQuery)) {
                        ele = $(ele);
                    }
                    selectedRow = ele.eq(0).parents('tr');
                } else {
                    selectedRow = this.$element.find('tbody .checker span.checked').eq(0).parents('tr');
                }
                if (selectedRow.length) {
                    return {
                        element: selectedRow[0],
                        data: this.$element.DataTable().row(selectedRow).data()
                    };
                } else {
                    return undefined;
                }
            }
        },
        getSelection: function () {
            if ($.fn.DataTable.isDataTable(this.$element)) {
                var selectedRows = this.$element.find('tbody .checker span.checked').parents('tr');
                var selection = [];
                var table = this.$element.DataTable();
                if (selectedRows.length) {
                    selectedRows.each(function () {
                        var data = table.row(this).data();
                        selection.push({
                            element: this,
                            data: data
                        });
                    });
                    return selection;
                } else {
                    return undefined;
                }
            }
        }
    };
    return CustomTable;
});