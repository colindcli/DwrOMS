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
                    url: "/clientApi/UserBuySupplier/GetBuySupplierList",
                    page: true,
                    height: "full-" + self.searchBarHeight,
                    where: self.searchKeys,
                    cols: [
                        [listField.numbers,
                            {
                                field: "SupplierCompany",
                                title: "公司名",
                            },
                            {
                                field: "SupplierName",
                                title: "联系人",
                            },
                            {
                                field: "SupplierTel",
                                title: "固话",
                            },
                            {
                                field: "SupplierMobilePhone",
                                title: "手机",
                            },
                            {
                                field: "SupplierEmail",
                                title: "邮箱",
                            },
                            {
                                field: "SupplierQQ",
                                title: "QQ",
                            },
                            {
                                field: "SupplierWechat",
                                title: "微信",
                            },
                            {
                                field: "SupplierAddress",
                                title: "地址",
                            },
                            {
                                field: "SupplierRemark",
                                title: "备注",
                            },
                            {
                                field: "operator",
                                title: "操作",
                                width: 100,
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
                        height: $(window).height() - 100,
                        done: function () {
                            win.formFocus('addFormFilter');

                            form.on("submit(addBtnFilter)", function (data) {
                                $.postdata("/clientApi/UserBuySupplier/AddBuySupplier", data.field, function (res) {
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
                        height: $(window).height() - 100,
                        done: function () {
                            form.val("editFormFilter", $.formData(data));

                            win.formFocus('editFormFilter');

                            form.on("submit(editBtnFilter)", function (data) {
                                $.postdata("/clientApi/UserBuySupplier/UpdateBuySupplier", data.field, function (res) {
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
                        "确定删除 [" + obj.data.SupplierCompany + "] 吗？",
                        function () {
                            $.postdata("/clientApi/UserBuySupplier/DeleteBuySupplier", obj.data, function (data) {
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
