define(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'layer', 'jstree', 'customtable'],
    function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system, selectcity) {

        // NodeType 0 普通节点 1 企业节点 2 监控员节点 3 车辆父节点

        var Entity = {};
        Entity.Type = "gov"; //  gov 是政府账号  ent 是企业账号

        var ZDYJK_nodeid = '';
        var treeid = '#jstree_m';

        var getTree = function () {
            var param = parent.window.document.getElementById('hdIDS').value;
            $('#treeId').val(param);
            helper.Ajax("006600200060", param, function (data) {
                if (data.body) {
                    var treedata = data.body;
                    initTree(treedata.children);
                    $('#treeName').val(treedata.text);
                    $('#treeId').val(treedata.id);
                } else {
                    tipdialog.errorDialog('获取失败' + data.publicresponse.message);
                }
            }, false);
        };

        var initTree = function (data) {

            function customMenu(node,type) {
                var menu = {
                    'add': {
                        'label': '新增节点',
                        'action': function (obj) {
                            //reference获取当前选中节点的引用
                            var inst = jQuery.jstree.reference(obj.reference);
                            //通过get_node方法获取节点的信息，类似于实例对象
                            var clickedNode = inst.get_node(obj.reference);
                            /*
                                inst.create_node 参数1:父节点  参数2:新节点的数据
                                参数3: 1）first：当前节点下的头部新增节点
                                        2）last：当前节点下的尾部新增节点
                                        3）before：当前节点同级的上部新增节点
                                        4）after：当前节点同级的下部新增节点
                                参数4:回调函数
                                参数5:Boolean类型,内部参数，指示父节点是否成功加载
                            */
                            var newNode = inst.create_node(clickedNode,
                                {    //'id': 'ajson20',
                                    //'parent' : 'ajson2',
                                    //'icon': 'jstree-file',
                                    'text': '新节点'
                                }, 'last', function (node) {
                                    //回调返回创建后点节点，给新节点改名
                                    inst.edit(node, node.val);
                                }, '');
                        }
                    },
                    'addent': {
                        'label': '新增企业节点',
                        'action': function (obj) {
                            var inst = jQuery.jstree.reference(obj.reference);
                            var clickedNode = inst.get_node(obj.reference);
                            //debugger
                            if (clickedNode.data == null || clickedNode.data.OrgCode == null) {
                                popdialog.showModal({
                                    'url': 'XZQYDialog.html',
                                    'width': '1200px',
                                    'showSuccess': GetQiYe
                                });
                            }
                        }
                    },
                    'addcarnode': {
                        'label': '新增车辆节点',
                        'action': function (obj) {
                            var inst = jQuery.jstree.reference(obj.reference);
                            var clickedNode = inst.get_node(obj.reference);
                            if (clickedNode.data == null || clickedNode.data.NodeType == null) {
                                var data = {}
                                data.NodeType = "车辆父节点";
                                AddNode("新车辆父节点", data, function (node) {
                                    //回调返回创建后点节点，给新节点改名
                                    inst.edit(node, node.val);
                                });
                            }
                        }
                    },
                    'rename': {
                        'label': '修改',
                        'action': function (obj) {
                            var inst = jQuery.jstree.reference(obj.reference);
                            var clickedNode = inst.get_node(obj.reference);
                            inst.edit(obj.reference, clickedNode.val);
                        }
                    },
                    'delete': {
                        "label": "删除",
                        'action': function (obj) {
                            var inst = jQuery.jstree.reference(obj.reference);
                            var clickedNode = inst.get_node(obj.reference);
                            inst.delete_node(obj.reference);
                        }
                    }
                }

                if (node) {
                    var nodetype = '';
                    if (node.data != null) {
                        if (node.data.NodeType == "企业节点" || node.data.OrgCode != null) {
                            nodetype = '企业节点';
                            delete menu.add;
                            delete menu.addent;
                            delete menu.rename;
                            delete menu.addcarnode;
                        }
                        if (node.data.NodeType == "车辆父节点") {
                            nodetype = '车辆父节点';
                            delete menu.addcarnode;
                            delete menu.add;
                            delete menu.addent;
                        }
                    }

                    if (Entity.Type == "gov") {//政府账号 选择企业
                        delete menu.addcarnode;
                    }
                    else if (Entity.Type == "ent") {//企业账号 选择车辆
                        delete menu.addent;
                    }
                }

                return menu;
            }

            var treeConfig = {
                "plugins": ["wholerow", "checkbox", "dnd", "contextmenu"],
                "checkbox": {
                    "three_state": false
                },
                'core': {
                    "multiple": false,//单选
                    "themes": {
                        "responsive": false,
                        "icons": false,
                        "stripes": true
                    },
                    "check_callback": true,
                },
                'contextmenu': { 'items': customMenu }
            };

            if (data) {
                $.extend(treeConfig.core, { "data": data});
            }

            window.tree = $(treeid)
                .jstree(treeConfig)
                .on('ready.jstree', function () {
                    $(treeid).jstree().open_all();
            });;




         //选中节点事件
            //$(treeid).on("changed.jstree", function (e, data) {
            //    console.log(data);
            //    //console.log(data.selected);
            //    if (data.selected.length > 0) {
            //        $('#selected_node_show').val(data.node.text);

            //        $('#t_nodetype').val('普通节点');
            //        if (data.node.data != null) {
            //            if (data.node.data.NodeType) {
            //                $('#t_nodetype').val(data.node.data.NodeType);
            //            }
            //        }
                    
            //        //debugger
            //        //if (data.node.data && data.node.data.OrgCode) {
            //            //ShowControlBtn(true, true);
            //        //} else {
            //            //ShowControlBtn(true);
            //        //}
            //    }
            //    else {
            //        $('#selected_node_show').val('');
            //        $('#t_nodetype').val('');
            //        //ShowControlBtn(false);
            //    }
            //});

            //选中节点事件
            $(treeid).on("changed.jstree", function (e, data) {
                //console.log(data);
                //console.log(data.selected);
                if (data.selected.length > 0) {
                    $('#selected_node_show').val(data.node.text);
                    $('#t_nodetype').val('普通节点');
                    if (data.node.data) {
                        $('#t_nodetype').val(data.node.data.NodeType);
                        if (data.node.data.OrgCode) {
                            //ShowControlBtn(true, 1);
                            ShowControlBtn(false, 1);
                            $('#t_nodetype').val('企业节点');
                        } else if (data.node.data.NodeType === "车辆父节点") {
                            ShowControlBtn(true, 3);
                            var SelectedCarList = data.node.data.CarList;
                            if (SelectedCarList === undefined) {
                                SelectedCarList = [];
                            }
                            //debugger
                            ZDYJK_nodeid = data.node.id;
                            sessionStorage.setItem("ZDYJK_SelectedCars", JSON.stringify(SelectedCarList));
                        }
                    } else {
                        //ShowControlBtn(true);
                    }
                }
                else {
                    $('#selected_node_show').val('');
                    $('#t_nodetype').val('');
                    ShowControlBtn(false);
                }
            });



            //$('button').on('click', function () {
            //    $('#jstree_m').jstree(true).select_node('child_node_1');
            //    $('#jstree_m').jstree('select_node', 'child_node_1');
            //    $.jstree.reference('#jstree_m').select_node('child_node_1');
            //});
    }

    function ShowControlBtn(isShow, nodetype) {
        $('#btn_changeCars').css('display', 'none');
        //if (isShow) {
        //    $('#btn_change_node').css('display', '');
        //    $('#btn_delete_node').css('display', '');
        //    $('#btn_move_node_up').css('display', '');
        //    $('#btn_move_node_down').css('display', '');
        //} else {
        //    $('#btn_change_node').css('display', 'none');
        //    $('#btn_delete_node').css('display', 'none');
        //    $('#btn_move_node_up').css('display', 'none');
        //    $('#btn_move_node_down').css('display', 'none');
        //}
        if (nodetype === 1) {
            $('#btn_change_node').css('display', 'none');
        } else if (nodetype === 3 && isShow) {
            $('#btn_changeCars').css('display', '');
        }
    }

    //添加自定义节点
    $('#btn_add_node').click(function () {
        var newNodeText = $('#new_node_text').val();
        if (newNodeText == "") {
            layer.tips('你确定不写点什么？', '#btn_add_node');
            return;
        }
        AddNode(newNodeText);
    });

        //添加修改 车辆到节点
        $('#btn_changeCars').click(function () {
            //打开新页面添加车辆
            popdialog.showIframe({
                'url': 'VehicleSelected.html',
                head: false
            });
        });

        function _SetCarList() {
            var id = ZDYJK_nodeid;
            var tree = $(treeid).jstree(tree);
            var SelectedCarList = JSON.parse(sessionStorage.getItem("ZDYJK_SelectedCars"));
            console.log(tree._model.data[id].data)
            tree._model.data[id].data.CarList = SelectedCarList;
        }

        window.SetCarList = _SetCarList;

    //添加企业
    //$('#btn_add_ent').click(function () {
    //    var newNodeText = $('#select_ent').val();
    //    if (newNodeText == "") {
    //        layer.tips('你没有选择企业噢', '#btn_add_ent');
    //        return;
    //    }
    //    var data = {}
    //    data.OrgCode = $('#ent_code').val();
    //    data.NodeType = "企业节点";
    //    AddNode(newNodeText, data);
    //});

    function AddNode(newNodeText, data, callback) {
        var tree = $(treeid).jstree();
        var arry = tree.get_selected();
        var node = null;
        if (arry.length > 0) {
            node = arry[0];
        }
        var newnode = { 'text': newNodeText, 'data': data };

        var parentnode = tree.get_node(node);
        if (parentnode.data && parentnode.data.OrgCode) {
            layer.alert('你不能在企业节点下添加节点')
            return;
        }

        tree.create_node(node, newnode, 'last', function (n_node) {
            if (node) {
                tree.open_node(node);//添加节点后展开
                //console.log(n_node)
                if (typeof (callback) === 'function') {
                    callback(n_node)
                }
            }
            //$(treeid).jstree("refresh");
            //$('#new_node_text').val('');
        });
    }

    //删除节点
    $('#btn_delete_node').click(function () {
        var tree = $(treeid).jstree();
        var arry = tree.get_selected();
        //console.log(arry);
        var node = null;
        //询问框
        if (arry.length) {
            layer.confirm('您确认删除该节点吗', {
                btn: ['是的', '不要了'] //按钮
            }, function (index) {
                tree.delete_node(arry);
                layer.close(index);
            }, function () {

            });
        }
    });


    //修改名称
    //$('#btn_change_node').click(function () {
    //    var tree = $(treeid).jstree();
    //    var arry = tree.get_selected();
    //    //console.log(arry);
    //    var node = null;
    //    //debugger
    //    if (arry.length > 0) {
    //        node = arry[0];
    //        layer.open({
    //            closeBtn: 0, //不显示关闭按钮
    //            anim: 2,
    //            shadeClose: true, //开启遮罩关闭
    //            title: '修改节点名称',
    //            content: '<input class="layui-input" id="rename" placeholder="请输入新的名称"/>',
    //            btn: ['修改', '取消'],
    //            yes: function (index, e) {
    //                var m = $('#rename').val();
    //                if (!m.length > 0) {
    //                    layer.msg('节点名不能为空');
    //                    return;
    //                } else {
    //                    tree.rename_node(node, m);
    //                    $('#selected_node_show').val(m);
    //                }
    //                layer.close(index);
    //            }
    //        });
    //    }
    //});




        //上移节点
        $('#btn_move_node_up').click(function () {
            MoveNode('up',
                function (node, nodeParent, position) {
                    //console.log(node)
                    //console.log(position)
                });
        });


        //上移节点
        $('#btn_move_node_down').click(function () {
            MoveNode('down',
                function (node, nodeParent, position) {
                    //console.log(node)
                    //console.log(position)
                });
        });

        //展开节点
        $('#btn_open_node_all').click(function () {
            $(tree).jstree().open_all();
        });

        //收起全部
        $('#btn_close_node_all').click(function () {
            $(tree).jstree().close_all();
        });

    //添加修改 车辆到节点
    $('#btn_changeCars').click(function () {
        //打开新页面添加车辆
        popdialog.showIframe({
            'url': 'VehicleSelected.html',
            head: false
        });
    });


    function MoveNode(UorD, callback) {
        var tree = $(treeid).jstree();
        var arry = tree.get_selected(true);
        var node = null;
        if (arry.length > 0) {
            node = arry[0];
            var parent = tree.get_node(node.parent);
            var position = 0;
            for (let i = 0; i < parent.children.length; i++) {
                if (parent.children[i] == node.id) {
                    position = i;
                }
            }
            if (UorD == "up") {
                position -= 1;
            } else if (UorD == "down") {
                position += 2;
            }
            if (position >= 0 && position <= parent.children.length) {
                tree.move_node(node, node.parent, position, callback);
            }
        }
    }

    //======================= 选择企业功能 =======================
    //选择企业按钮
    $("#choose_ent").on("click", function (e) {
        e.preventDefault();
        popdialog.showModal({
            'url': 'XZQYDialog.html',
            'width': '1200px',
            'showSuccess': GetQiYe
        });
    });

    //选择企业
    function GetQiYe() {
        InitTableQiYe();
        InitButton();
    }

    function InitTableQiYe() {
        $("#tb_QiYe").CustomTable({
            ajax: helper.AjaxData("006600200016", 
                function (data) {
                    var pageInfo = {
                        Page: parseInt(data.start / data.length + 1),
                        Rows: data.length
                    };
                    for (var i in data) {
                        delete data[i];
                    }
                    var para = {
                        XiaQuShi: window.OrganizationManageArea ? window.OrganizationManageArea.trim().replace(/\|/g, ',').replace(/广东/g, '') : "",
                        YeHuMingCheng: $('#YeHuMingChengPartTwo').val(),
                        isOnlyQiYe: 1
                    };
                    pageInfo.data = para;
                    $.extend(data, pageInfo);
                }, null),
            single: true,
            filter: false,
            scrollY: 380,
            ordering: false, /////是否支持排序
            language: {
                emptyTable: "请先进行查询",
            },
            "dom": 'fr<"table-scrollable"t><"row"<"col-md-2 col-sm-12 pagination-l"l><"col-md-3 col-sm-12 pagination-i"i><"col-md-7 col-sm-12 pagnav pagination-p"p>>',
            columns: [{
                render: function (data, type, row) {
                    return '<input type=checkbox class=checkboxes />';
                }
            }, {
                data: 'XiaQuShi',
                render: function (data, type, row) {
                    return '广东' + row.XiaQuShi;
                }
            },
            { data: 'YeHuMingCheng' }
            ],
            pageLength: 10,
            "fnDrawCallback": function (oSettings) {
                // tableheadfix.ResetFix();
            }
        });
        $("#tb_QiYe").CustomTable("reload");
    };

    function InitButton() {
        $('#btnSearch').click(function (e) {
            e.preventDefault();
            $("#tb_QiYe").CustomTable("reload");
        });

        //确定
        $('#btnConfirm').on('click', function (e) {
            e.preventDefault();
            var rows = $("#tb_QiYe").CustomTable('getSelection');
            if (rows == undefined) {
                warning.errorDialog('请选择所属企业信息');
                return false;
            }
            if (rows.length != 1) {
                warning.errorDialog('仅可选择一个企业');
                return false;
            }
            //$("#select_ent").val(rows[0].data.YeHuMingCheng);
            //$("#ent_code").val(rows[0].data.OrgCode);
            $('.close').trigger('click');

            var data = {}
            data.OrgCode = rows[0].data.OrgCode;
            data.NodeType = "企业节点";
            AddNode(rows[0].data.YeHuMingCheng, data);
        });
    }


    Entity.AddSave = function () {
        //console.log(m)
        var json = $(treeid).jstree().get_json();
        var treename = $('#treeName').val().trim();
        if (treename.length > 0) {

        } else {
            tipdialog.errorDialog('请输入名称');
            return;
        }
        var param = {
            'NodeName': treename,
            'TreeNodes': json
        };

        helper.Ajax('006600200061', param, function (result) {//todo
            if (result.publicresponse.statuscode == 0) {
                toast.success('添加成功');
                parent.window.$('#btnSearch').trigger('click');
                popdialog.closeIframe();
            }
            else {
                tipdialog.errorDialog(result.publicresponse.message);
            }
        });
    }

    Entity.UpdateSave = function () {
        //console.log(m)
        var json = $(treeid).jstree().get_json();
        var treename = $('#treeName').val().trim();
        if (treename.length > 0) {

        } else {
            tipdialog.errorDialog('请输入名称');
            return;
        }
        var param = {
            'NodeName': treename,
            'TreeNodes': json,
            'NodeId': $('#treeId').val()
        };

        helper.Ajax('006600200064', param, function (result) {//todo
            if (result.publicresponse.statuscode == 0) {
                toast.success('修改成功');
                parent.window.$('#btnSearch').trigger('click');
                popdialog.closeIframe();
            }
            else {
                tipdialog.errorDialog(result.publicresponse.message);
            }
        });
    }
  


    //howControlBtn(false);
    Entity.InitTree = initTree;
    Entity.GetTree = getTree;
    return Entity;
});