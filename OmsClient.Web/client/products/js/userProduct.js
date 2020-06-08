$(function () {
    var objTable = {
        tableId: "tableList",
        searchKeys: {},
        searchBarHeight: win.searchBarHeight,
        category: [],
        init: function () {
            var self = this;
            self.initData();
            self.search();
            self.render();
            self.operator();
            self.addInit();
            self.import();
            self.verify();
        },
        // search初始化
        initData: function () {
            var self = this;
            $.getdata("/clientApi/UserProduct/GetCategoryList", {}, function (res) {
                var rows = $.toTree({
                    data: res,
                    //根节点Id值
                    rootIdValue: "0",
                    //ParentId名
                    parentId: 'CategoryParentId',
                    //Id名
                    id: 'CategoryId',
                    //排除子树
                    expId: ''
                });

                var arr = ['<option value="0">不限</option>'];
                $.each(rows, function (i, item) {
                    var s = "";
                    if (item.Depth > 1) {
                        for (var j = 0; j < item.Depth - 1; j++) {
                            s += "│";
                        }
                        s += "└";
                    }
                    arr.push('<option value="' + item.CategoryId + '">' + s + item.CategoryName + '</option>');
                });
                $('[name="CategoryId"]').html(arr.join(""));
                self.category = res;
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

            //导出excel
            form.on("submit(exportFormFilter)", function (data) {
                var tempForm = document.createElement("form");
                tempForm.id = "exportExcelForm";
                tempForm.action = "/clientApi/UserProduct/ExportProduct?CategoryId="+data.field.CategoryId+"&keyword="+data.field.keyword;
                tempForm.target = "_blank";
                tempForm.method = "post";
                tempForm.style.display = "none";
                document.body.appendChild(tempForm);
                tempForm.submit();

                $("#exportExcelForm").remove();
                return false;
            });
        },
        // 显示表格
        render: function () {
            var self = this;
            $.tableObject({
                tableId: self.tableId,
                tableOption: {
                    url: "/clientApi/UserProduct/GetProductList",
                    page: true,
                    height: "full-" + self.searchBarHeight,
                    where: self.searchKeys,
                    cols: [
                        [listField.numbers,
                            listField.category,
                            listField.productSku,
                            listField.productName,
                            listField.specification,
                            listField.productRemark,
                            listField.productWeight,
                            listField.orderQty,
                            listField.inTransitQty,
                            listField.saleQty,
                            listField.holdQty,
                            listField.price1,
                            listField.price10,
                            listField.price100,
                            listField.imageQty,
                            listField.areaPosition,
                            listField.createName,
                            listField.createDate,
                            {
                                field: "operator",
                                title: "操作",
                                width: 122,
                                fixed: "right",
                                align: "center",
                                toolbar: "#operator"
                            }
                        ]
                    ],
                    doneOk: function (a) {
                        //显示数量
                        $.showQty();
                        //转币种价格：a.obj = [{currency}]
                        $(this).showPrice(a.obj);
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
                        width: 600,
                        height: 680,
                        done: function () {

                            win.formFocus('addFormFilter');

                            form.on("submit(addBtnFilter)", function (data) {
                                if (data.field.CategoryId == 0) {
                                    win.msg("请选择分类");
                                    return false;
                                }
                                $.postdata(
                                    "/clientApi/UserProduct/AddProduct", data.field,
                                    function (res) {
                                        if (res) {
                                            win.close();
                                            win.msg("添加成功");
                                            self.render();

                                            // 重置
                                            data.form.reset();

                                        } else {
                                            win.alert('添加失败');
                                        }
                                    }
                                );
                                return false;
                            });
                        }
                    });
                });
        },
        import: function () {
            var self = this;
            $("#btnImport")
                .unbind("click")
                .on("click", function () {
                    win.open({
                        id: "import",
                        title: "批量导入",
                        width: 600,
                        height: 380,
                        done: function () {
                            form.on("submit(importBtnFilter)", function (data) {
                                if (data.field.CategoryId == 0) {
                                    win.msg("请选择分类");
                                    return false;
                                }

                                var fileObj = document.getElementById("FileUpload").files[0]; // js 获取文件对象
                                if (typeof (fileObj) == "undefined" || fileObj.size <= 0) {
                                    alert("请选择Excel文件");
                                    return false;
                                }

                                var formFile = new FormData();
                                formFile.append("file", fileObj);
                                formFile.append("CategoryId", data.field.CategoryId);

                                $.ajax({
                                    url: "/clientApi/UserProduct/ImportProduct",
                                    data: formFile,
                                    type: "Post",
                                    dataType: "json",
                                    cache: false,
                                    processData: false, //用于对data参数进行序列化处理 这里必须false
                                    contentType: false, //必须
                                    success: function (result) {
                                        if (result.code != 0) {
                                            win.alert(result.msg);
                                        } else {
                                            win.close();
                                            win.msg("导入成功");
                                            self.render();
                                        }
                                    },
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
                        width: 600,
                        height: 680,
                        done: function () {
                            form.val("editFormFilter", $.formData(data));


                            win.formFocus('editFormFilter');

                            form.on("submit(editBtnFilter)", function (data) {
                                if (data.field.CategoryId == 0) {
                                    win.msg("请选择分类");
                                    return false;
                                }

                                $.postdata(
                                    "/clientApi/UserProduct/UpdateProduct",
                                    data.field,
                                    function (res) {
                                        if (res) {
                                            win.close();
                                            win.msg("修改成功");
                                            self.render();

                                            // 重置
                                            data.form.reset();
                                        } else {
                                            win.alert('修改失败');
                                        }
                                    }
                                );
                                return false;
                            });
                        }
                    });
                } else if (layEvent === "del") {
                    //删除
                    win.confirm(
                        "确定删除 [" + obj.data.ProductName + "] 吗？",
                        function () {
                            $.postdata(
                                "/clientApi/UserProduct/DeleteProduct",
                                obj.data,
                                function (data) {
                                    if (data) {
                                        win.msg("删除成功");
                                        self.render();
                                    } else {
                                        win.alert('删除失败');
                                    }
                                }
                            );
                        }
                    );
                }
            });
        },
        verify: function () {
            form.verify({
                CategoryId: function (value, item) {
                    //value：表单的值、item：表单的DOM对象
                    if (value == "") {
                        return "请选择分类";
                    }
                },
            });
        }
    };
    objTable.init();

});