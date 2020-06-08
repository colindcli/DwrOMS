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
        },
        // search初始化
        initData: function () {
            var self = this;
            $.getdata("/clientApi/UserBuyOrder/GetCategoryList", {}, function (res) {
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
                    url: "/clientApi/UserBuyOrder/GetUrgentPurchase",
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
                            field: "Urgent",
                            title: "缺货数量",
                            width: 60,
                            templet: "<div><b>{{ -(d.InTransitQty + d.SaleQty) }}</b></div>",
                            fixed: 'right',
                            align: 'center',
                        },
                        ]
                    ],
                    doneOk: function(a){
                        //显示数量
                        $.showQty();
                    }
                }
            });
        },
    };
    objTable.init();

});