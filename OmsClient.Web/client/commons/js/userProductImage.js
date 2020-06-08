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
                        [
                        listField.numbers,
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
                            width: 100,
                            fixed: "right",
                            align: "center",
                            toolbar: "#operator"
                        }
                        ]
                    ],
                    doneOk: function(a){
                        //显示数量
                        $.showQty();
                        //a.obj = [{currency}]
                        $(this).showPrice(a.obj);
                    }
                }
            });
        },
        productId: '',
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
                    self.productId = data.ProductId;
                    win.open({
                        id: "info",
                        title: "管理图片",
                        width: $(window).width()-100,
                        height: $(window).height()-100,
                        done: function () {
                            // 组图
                            $.getdata('/clientApi/UserProduct/GetProductImageList?productId=' + self.productId, {}, function (paths) {
                                $.images({
                                    id: 'ProductImages',
                                    paths: paths,
                                    productId: self.productId,
                                    sortEvent: function () {
                                        var paths = [];
                                        $('.imgList .thumbnail').each(function () {
                                            var path = $(this).data('path');
                                            paths.push(path);
                                        });

                                        var obj = {
                                            ProductId: self.productId,
                                            Paths: paths
                                        };
                                        $.postdata('/clientApi/UserProduct/ImageSort', obj, function (res) {
                                            if (res) {
                                                win.msg('排序成功');
                                            }
                                        });
                                    },
                                    deleteEvent: function (path) {
                                        $.postdata('/clientApi/UserProduct/DeleteImage', {
                                            ProductId: self.productId,
                                            PathImage: path
                                        }, function (res) {
                                            if (res) {
                                                win.msg('删除成功');
                                            }
                                        });
                                    },
                                });
                            });
                        }
                    });
                }
            });
        },
    };
    objTable.init();
});