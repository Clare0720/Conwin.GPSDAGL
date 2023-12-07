define(['/Modules/Config/conwin.main.js'], function () {
    require.config({ paths: { "myjstree": "/Modules/Common/Js/MJsTree/MJsTree" } });//引入工具类
    require(['jquery', 'popdialog', 'tipdialog', 'toast', 'helper', 'common', 'tableheadfix', 'system', 'selectcity', 'myjstree','layer','jstree','customtable'],
        function ($, popdialog, tipdialog, toast, helper, common, tableheadfix, system, selectcity, myjstree,layer,) {
            var userInfo = helper.GetUserInfo();



            var treeid = '#jstree_m';

            //todo 加载地图
            var getTree = function () {
                var param = {};
                //0033002
                helper.Ajax("0033002",param, function (data) {
                    if (data.body) {
                        toast.success("启用成功");
                    } else {
                        tipdialog.errorDialog('启用失败' + data.publicresponse.message);
                    }
                }, false);
            };

            myjstree.loadTree('jstree_m');



            var initTree = function () {


                window.tree = $(treeid).jstree({
                    "plugins": ["wholerow", "checkbox"],
                    "checkbox": {
                        "three_state": false
                    },
                    'core': {
                        "multiple": false,//单选
                        "themes": {
                            "responsive": false,
                            "icons": false,
                            "stripes":true
                        },
                        "check_callback": true,
                    },
                }); 

                $(treeid).on("changed.jstree", function (e, data) {
                    console.log(data);
                    console.log(data.selected);
                    if (data.selected.length > 0) {
                        $('#selected_node_show').val(data.node.text);
                        ShowControlBtn(true);
                    }
                    else {
                        $('#selected_node_show').val('');
                        ShowControlBtn(false);
                    }
                });



                //$('button').on('click', function () {
                //    $('#jstree_m').jstree(true).select_node('child_node_1');
                //    $('#jstree_m').jstree('select_node', 'child_node_1');
                //    $.jstree.reference('#jstree_m').select_node('child_node_1');
                //});
            }

            function ShowControlBtn(isShow) {
                if (isShow) {
                    $('#btn_change_node').css('display', '');
                    $('#btn_delete_node').css('display', '');
                    $('#btn_move_node_up').css('display', '');
                    $('#btn_move_node_down').css('display', '');
                } else {
                    $('#btn_change_node').css('display', 'none');
                    $('#btn_delete_node').css('display', 'none');
                    $('#btn_move_node_up').css('display', 'none');
                    $('#btn_move_node_down').css('display', 'none');
                }
            }

            //添加自定义节点
            $('#btn_add_node').click(function(){
                var newNodeText = $('#new_node_text').val();
                if (newNodeText == "") {
                    layer.tips('你确定不写点什么？', '#btn_add_node');
                    return;
                }
                AddNode(newNodeText);
            });


            //添加企业
            $('#btn_add_ent').click(function () {
                var newNodeText = $('#select_ent').val();
                if (newNodeText == "") {
                    layer.tips('你没有选择企业噢', '#btn_add_ent');
                    return;
                }
                AddNode(newNodeText);
            });

            function AddNode(newNodeText,callback) {
                var tree = $(treeid).jstree();
                var arry = tree.get_selected();
                var node = null;
                if (arry.length > 0) {
                    node = arry[0];
                }
                var newnode = { 'text': newNodeText };
                tree.create_node(node, newnode, 'last', function () {
                    if (node) {
                        tree.open_node(node);//添加节点后展开
                    }
                    //$('#new_node_text').val('');
                });
            }


            //选择企业
            $('#choose_ent').click(function () {
                var j = $(treeid).jstree().get_json();
                //todo
            });


            //删除节点
            $('#btn_delete_node').click(function () {
                var tree = $(treeid).jstree();
                var arry = tree.get_selected();
                console.log(arry);
                var node = null;
                //询问框

                layer.confirm('您确认删除该节点吗', {
                    btn: ['是的', '不要了'] //按钮
                }, function (index) {
                        tree.delete_node(arry);
                        debugger
                        layer.close(index);
                }, function () {

                });
            });



            //修改名称
            $('#btn_change_node').click(function () {
                var tree = $(treeid).jstree();
                var arry = tree.get_selected();
                console.log(arry);
                var node = null;
                if (arry.length > 0) {
                    node = arry[0];
                    layer.open({
                        closeBtn: 0, //不显示关闭按钮
                        anim: 2,
                        shadeClose: true, //开启遮罩关闭
                        title: '修改节点名称',
                        content: '<input class="layui-input" id="rename" placeholder="请输入新的名称"/>',
                        btn: ['修改', '取消'],
                        yes: function (index, e) {
                            debugger
                            var m = $('#rename').val();
                            if (!m.length > 0) {
                                layer.msg('备注不能为空');
                                return;
                            } else {
                                tree.rename_node(node, m);
                                //todo 
                                //ajax
                            }
                            layer.close(index);
                        }
                    });
                }
            });




            //上移节点
            $('#btn_move_node_up').click(function () {
                MoveNode('up',
                    function (ndoe, nodeParent, position) {
                        console.log(position)
                    });
            });

            //下移节点
            $('#btn_move_node_down').click(function () {
                MoveNode('dwon',
                    function (ndoe, nodeParent, position) {
                        console.log(position)
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
                    } else if (UorD == "dwon") {
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
                    ajax: helper.AjaxData("006000200010",
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
                    $("#select_ent").val(rows[0].data.YeHuMingCheng);
                    $("#ent_code").val(rows[0].data.YeHuDaiMa);
                    $('.close').trigger('click');
                });
            }

            var initPage = function () {
                ShowControlBtn(false);
                //initTree();

            }


            //个性化代码块
            //region
            //endregion
            initPage();

        });
});