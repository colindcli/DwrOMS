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

            //角色
            $.getdata('/clientApi/UserUser/GetUserRoleList', {}, function(res){
                var arr = [];
                $.each(res, function(i, item){
                    arr.push('<option value="'+item.UserRoleId+'">'+item.UserRoleName+'</option>')
                });
                $('[name="UserRoleId"]').html(arr.join(''));
            });
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
                    url: "/clientApi/UserUser/GetUserList",
                    page: true,
                    height: "full-" + self.searchBarHeight,
                    where: self.searchKeys,
                    cols: [
                        [listField.numbers,
                            {
                                field: "UserRoleName",
                                title: "业务角色",
                            },
                            {
                                field: "UserEmail",
                                title: "邮箱",
                            },
                            {
                                field: "UserNickName",
                                title: "英文名",
                            },
                            {
                                field: "UserChnName",
                                title: "中文名",
                            },
                            {
                                field: "IsAdmin",
                                title: "系统管理",
                                width: 90,
                                templet: '<div>{{#  if(d.IsAdmin){ }}<i class="iconfont icon-dagou"></i>{{#  } }}</div>'
                            },
                            listField.remark,
                            {
                                field: "Status",
                                title: "状态",
                                width: 60,
                                templet: "#Status"
                            },
                            {
                                field: "operator",
                                title: "操作",
                                width: 150,
                                fixed: "right",
                                align: "center",
                                toolbar: "#operator"
                            }
                        ]
                    ],
                    doneOk: function(){
                        $(".delRow").each(function(){
                            $(this).parent().parent().parent().addClass("delRow");
                        });
                        $(".disRow").each(function(){
                            $(this).parent().parent().parent().addClass("disRow");
                        });
                    }
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
                        height: 550,
                        done: function () {
                            win.formFocus('addFormFilter');

                            form.on("submit(addBtnFilter)", function (data) {
                                $.postdata("/clientApi/UserUser/AddUser", data.field, function (res) {
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
                        height: 550,
                        done: function () {
                            form.val("editFormFilter", $.formData(data));

                            win.formFocus('editFormFilter');

                            form.on("submit(editBtnFilter)", function (data) {
                                $.postdata("/clientApi/UserUser/UpdateUser", data.field, function (res) {
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
                } else if (layEvent === "pwd") {
                    win.open({
                        id: "pwd",
                        title: "修改密码",
                        width: 500,
                        height: 350,
                        done: function () {
                            form.val("pwdFormFilter", $.formData(data));

                            win.formFocus('pwdFormFilter');

                            $('#pwd [name="UserPwd"]').val('');

                            form.on("submit(pwdBtnFilter)", function (data) {
                                $.postdata("/clientApi/UserUser/UpdateUserPwd", data.field, function (res) {
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
                } else if (layEvent === "del") {
                    //删除
                    win.confirm(
                        "确定删除 [" + obj.data.UserEmail + "] 吗？",
                        function () {
                            $.postdata("/clientApi/UserUser/DeleteUser", obj.data, function (data) {
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
