$(function () {
    var objTable = {
        tableId: 'auth-table',
        init: function () {
            var self = this;
            self.addInit();
            self.operator();
        },
        // 添加初始化
        addInit: function () {
            var self = this;
            $("#btnAdd")
                .unbind("click")
                .on("click", function () {
                    win.open({
                        id: "add",
                        title: "添加",
                        width: 500,
                        height: 500,
                        done: function () {
                            win.formFocus('addFormFilter');

                            form.on("submit(addBtnFilter)", function (data) {
                                $.postdata(
                                    "/clientApi/UserCategory/AddCategory",  data.field, function (res) {
                                        if(res){
                                            win.close();
                                            win.msg("添加成功");
                                            self.render();

                                            // 重置
                                            data.form.reset();

                                        } else {
                                            win.alert('添加失败');
                                        }
                                    });
                                return false;
                            });
                        }
                    });
                });
        },
        render: function () {
            renderTable();
        },
        // 表格操作
        operator: function () {
            var self = this;
            //监听工具条
            table.on("tool(" + self.tableId + ")", function (obj) {
                //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
                var data = obj.data; //获得当前行数据
                var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
                var tr = obj.tr; //获得当前行 tr 的DOM对象
                if (layEvent === "edit") {
                    win.open({
                        id: "info",
                        title: "编辑",
                        width: 500,
                        height: 500,
                        done: function () {
                            form.val("editFormFilter", $.formData(data));

                            win.formFocus('editFormFilter');
                            $.getdata('/clientApi/UserCategory/GetCategoryList', {}, function(res){
                                var rows = $.toTree({
                                    data: res,
                                    //根节点Id值
                                    rootIdValue: 0,
                                    //ParentId名
                                    parentId: 'CategoryParentId',
                                    //Id名
                                    id: 'CategoryId',
                                    //排除子树
                                    expId: data.CategoryId
                                });

                                var arr = ['<option value="0">顶级节点</option>'];
                                $.each(rows, function(i, item){
                                    var s = "";
                                    if(item.Depth>1){
                                        for(var j=0;j<item.Depth-1;j++){
                                            s+="│";
                                        }
                                        s+="└";
                                    }
                                    if(item.CategoryId==data.CategoryParentId){
                                        arr.push('<option value="'+item.CategoryId+'" selected="selected">'+s+item.CategoryName+'</option>');
                                    }else{
                                        arr.push('<option value="'+item.CategoryId+'">'+s+item.CategoryName+'</option>');
                                    }
                                });
                                $("#selectParentId").html(arr.join(''));

                                form.on("submit(editBtnFilter)", function (data) {
                                    $.postdata(
                                        "/clientApi/UserCategory/UpdateCategory",
                                        data.field,
                                        function (res) {
                                            if(res){
                                                win.close();
                                                win.msg("修改成功");
                                                self.render();

                                                // 重置
                                                data.form.reset();
                                            } else{
                                                win.alert('修改失败');
                                            }
                                        }
                                    );
                                    return false;
                                });
                            });

                        }
                    });
                } else if (layEvent === "del") {
                    //删除
                    win.confirm(
                        "确定删除 [" + obj.data.CategoryName + "] 吗？",
                        function () {
                            $.postdata("/clientApi/UserCategory/DeleteCategory", obj.data,
                                function (data) {
                                    if (data) {
                                        win.msg("删除成功");
                                        self.render();
                                    } else {
                                        win.alert('删除失败。1、有子项不能删除；2、关联了内容不能删除。');
                                    }
                                });
                        }
                    );
                } else if (layEvent === "add") {
                    //添加子菜单
                    win.open({
                        id: "add",
                        title: "添加",
                        width: 500,
                        height: 500,
                        done: function () {
                            win.formFocus('addFormFilter');
                            var id = data.CategoryId;
                            form.on("submit(addBtnFilter)", function (data) {
                                data.field.CategoryParentId = id;
                                $.postdata(
                                    "/clientApi/UserCategory/AddCategory",
                                    data.field,
                                    function (res) {
                                        if(res){
                                            win.close();
                                            win.msg("添加成功");
                                            self.render();

                                            // 重置
                                            data.form.reset();
                                        } else {
                                            win.alert('添加失败');
                                        }
                                    });
                                return false;
                            });
                        }
                    });
                }
            });
        },
        verify: function () {
            form.verify({
            });
        }
    };

    var treetable = layui.treetable;

    // 渲染表格
    var renderTable = function () {
        layer.load(2);
        treetable.render({
            treeColIndex: 1,
            treeSpid: 0,
            treeIdName: 'CategoryId',
            treePidName: 'CategoryParentId',
            elem: '#auth-table',
            url: '/clientApi/UserCategory/GetCategoryList',
            page: false,
            cols: [
                [
                    listField.numbers,
                    {
                        field: 'CategoryName',
                        title: '名称'
                    },
                    {
                        field: "Sort",
                        title: "排序",
                        width: 60,
                    },
                    listField.createDate,
                    {
                        templet: '#auth-state',
                        width: 150,
                        align: 'center',
                        title: '操作'
                    }
                ]
            ],
            done: function () {
                layer.closeAll('loading');

                objTable.init();
            },
        });
    };
    renderTable();
});