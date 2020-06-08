//
$(function () {
    var objTable = {
        tableId: "tableList",
        searchKeys: {},
        searchBarHeight: win.searchBarHeight,
        init: function () {
            var self = this;
            self.initData();
            self.search();
            self.render();
            self.operator();
            self.addInit();
            self.verify();
        },
        // search初始化
        initData: function () {
            var self = this;
        },
        // 点击搜索
        search: function () {
            var self = this;
            form.on("submit(searchFormFilter)", function (data) {
                $.extend(self.searchKeys, data.field);
                self.render();
                return false;
            });
        },
        // 显示表格
        render: function () {
            var self = this;
            $.tableObject({
                tableId: self.tableId,
                tableOption: {
                    url: "/clientApi/UserUserRole/GetUserRoleList",
                    page: true,
                    height: "full-" + self.searchBarHeight,
                    where: self.searchKeys,
                    cols: [
                        [listField.numbers,
                            {
                                field: "UserRoleName",
                                title: "角色名称",
                                width: 130,
                            },
                            {
                                field: "UserNames",
                                title: "用户",
                            },
                            {
                                field: "UserRoleSort",
                                title: "排序",
                                width: 60,
                            },
                            {
                                field: "operator",
                                title: "操作",
                                width: 160,
                                fixed: "right",
                                align: "center",
                                toolbar: "#operator"
                            }
                        ]
                    ]
                }
            });
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
                        height: 300,
                        done: function () {
                            win.formFocus('addFormFilter');

                            form.on("submit(addBtnFilter)", function (data) {
                                $.postdata("/clientApi/UserUserRole/AddUserRole", data.field, function (res) {
                                    if(res){
                                        win.close();
                                        win.msg("添加成功");
                                        self.render();

                                        // 重置
                                        data.form.reset();

                                    }else{
                                        win.alert("添加失败");
                                    }
                                });
                                return false;
                            });
                        }
                    });
                });
        },
        rowId: '',
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
                        height: 300,
                        done: function () {
                            form.val("editFormFilter", $.formData(data));

                            win.formFocus('editFormFilter');

                            form.on("submit(editBtnFilter)", function (data) {
                                $.postdata("/clientApi/UserUserRole/UpdateUserRole", data.field, function (res) {
                                    if(res){
                                        win.close();
                                        win.msg("修改成功");
                                        self.render();

                                        // 重置
                                        data.form.reset();
                                    } else{
                                        win.alert("修改失败");
                                    }
                                });
                                return false;
                            });
                        }
                    });
                } else if (layEvent === "set") {
                    win.open({
                        id: "set",
                        title: "设置菜单",
                        width: $(window).width()-100,
                        height: $(window).height()-100,
                        done: function () {
                            self.rowId = data.UserRoleId;

                            $.getdata('/clientApi/UserUserRole/GetUserRoleSetById?userRoleId='+self.rowId, { }, function(res){
                                var arr = [];
                                $.each(res, function(i, item){
                                    arr.push('<div class="form-group">');
                                    arr.push('<label class="col-sm-2 control-label">'+item.Name+'</label>');
                                    arr.push('<div class="col-sm-10">');

                                    $.each(item.Lists, function(j, row){
                                        arr.push('<input type="checkbox" name="'+row.UserMenuId+'" value="'+row.UserMenuId+'" title="'+row.UserMenuText+'" '+(row.IsChecked?"checked":"")+'>');
                                    });

                                    arr.push('</div>');
                                    arr.push('</div>');
                                    arr.push('<hr>');
                                });

                                $("#setBody").html(arr.join(''));

                                form.render(null, 'setFormFilter');

                                form.on("submit(setBtnFilter)", function (data) {
                                    var ids = [];
                                    $.each(data.field, function(i, item){
                                        ids.push(item);
                                    });
    
                                    var obj = {UserMenuIds: ids, UserRoleId: self.rowId};
    
                                    $.postdata("/clientApi/UserUserRole/UpdateUserRoleSet", obj, function (res) {
                                        if(res){
                                            win.close();
                                            win.msg("修改成功");
                                            self.render();
    
                                            // 重置
                                            data.form.reset();
                                        } else{
                                            win.alert("修改失败");
                                        }
                                    });
                                    return false;
                                });
                            });
                        }
                    });
                } else if (layEvent === "del") {
                    //删除
                    win.confirm(
                        "确定删除 [" + obj.data.UserRoleName + "] 吗？",
                        function () {
                            $.postdata("/clientApi/UserUserRole/DeleteUserRole", obj.data, function (data) {
                                if (data) {
                                    win.msg("删除成功");
                                    self.render();
                                } else {
                                    win.alert("删除失败");
                                }
                            });
                        }
                    );
                }
            });
        },
        verify: function () {
            form.verify({
            });
        }
    };
    objTable.init();
});
